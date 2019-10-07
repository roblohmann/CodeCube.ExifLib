using System;
using System.IO;
using CodeCube.ExifLib.Enumerations;


namespace CodeCube.ExifLib
{
    public sealed class ExifReader
    {
        private bool littleEndian;

        public JpegInfo info { get; set; }

        [Obsolete("Use Read()-method. ReadJpeg will be removed in a future version.")]
        public static JpegInfo ReadJpeg(Stream stream)
        {
            return Read(stream);
        }

        public static JpegInfo Read(Stream stream)
        {
            DateTime now = DateTime.UtcNow;

            return new ExifReader(stream)
            {
                    info = { LoadTime = (DateTime.UtcNow - now) }
            }.info;
        }

        private ExifReader(Stream stream)
        {
            info = new JpegInfo();
            if (stream.ReadByte() != byte.MaxValue || stream.ReadByte() != 216)
            {
                return;
            }

            info.IsValid = true;
            while (true)
            {
                int num1 = 0;
                int num2 = 0;
                int marker;
                while (true)
                {
                    marker = stream.ReadByte();
                    if (marker == byte.MaxValue || num1 != byte.MaxValue)
                    {
                        num1 = marker;
                        ++num2;
                    }
                    else
                        break;
                }
                int num3 = stream.ReadByte();
                int num4 = stream.ReadByte();
                int length = num3 << 8 | num4;
                byte[] numArray = new byte[length];
                numArray[0] = (byte)num3;
                numArray[1] = (byte)num4;
                if (stream.Read(numArray, 2, length - 2) == length - 2)
                {
                    switch (marker)
                    {
                        case 192:
                        case 193:
                        case 194:
                        case 195:
                        case 197:
                        case 198:
                        case 199:
                        case 201:
                        case 202:
                        case 203:
                        case 205:
                        case 206:
                        case 207:
                            ProcessSof(numArray);
                            break;
                        case 217:
                            goto label_7;
                        case 218:
                            goto label_14;
                        case 225:
                            if (numArray[2] == 69 && numArray[3] == 120 && (numArray[4] == 105 && numArray[5] == 102))
                            {
                                ProcessExif(numArray);
                                break;
                            }
                            break;
                    }
                    GC.Collect();
                }
                else
                    break;
            }
            return;
        label_7:
            return;
        label_14:;
        }

        private void ProcessExif(byte[] section)
        {
            int num1 = 6;
            byte[] numArray1 = section;
            int index1 = num1;
            int num2 = index1 + 1;
            if (numArray1[index1] != 0)
            {
                return;
            }

            byte[] numArray2 = section;
            int index2 = num2;
            int index3 = index2 + 1;
            if (numArray2[index2] != 0)
            {
                return;
            }

            if (section[index3] == 73 && section[index3 + 1] == 73)
            {
                littleEndian = true;
            }
            else
            {
                if (section[index3] != 77 || section[index3 + 1] != 77)
                {
                    return;
                }
                littleEndian = false;
            }
            int offset1 = index3 + 2;
            int num3 = ExifIo.ReadUShort(section, offset1, littleEndian);
            int offset2 = offset1 + 2;
            if (num3 != 42)
            {
                return;
            }
            int num4 = ExifIo.ReadInt(section, offset2, littleEndian);
            if ((num4 < 8 || num4 > 16) && (num4 < 16 || num4 > section.Length - 16))
            {
                return;
            }

            ProcessExifDir(section, num4 + 8, 8, section.Length - 8, 0, ExifIFD.Exif);
        }

        private static int DirOffset(int start, int num)
        {
            return start + 2 + 12 * num;
        }

        private void ProcessExifDir(byte[] section, int offsetDir, int offsetBase, int length, int depth, ExifIFD ifd)
        {
            if (depth > 4)
            {
                return;
            }

            ushort num1 = ExifIo.ReadUShort(section, offsetDir, littleEndian);
            if (offsetDir + 2 + 12 * num1 >= offsetDir + length)
            {
                return;
            }

            for (int num2 = 0; num2 < (int)num1; ++num2)
            {
                int sectionOffset = DirOffset(offsetDir, num2);
                ExifTag exifTag = new ExifTag(section, sectionOffset, offsetBase, length, littleEndian);
                if (exifTag.IsValid)
                {
                    switch (exifTag.Tag)
                    {
                        case 34665:
                            int offsetDir1 = offsetBase + exifTag.GetInt(0);
                            if (offsetDir1 <= offsetBase + length)
                            {
                                ProcessExifDir(section, offsetDir1, offsetBase, length, depth + 1, ExifIFD.Exif);
                            }
                            continue;
                        case 34853:
                            int offsetDir2 = offsetBase + exifTag.GetInt(0);
                            if (offsetDir2 <= offsetBase + length)
                            {
                                ProcessExifDir(section, offsetDir2, offsetBase, length, depth + 1, ExifIFD.Gps);
                            }
                            continue;
                        default:
                            exifTag.Populate(info, ifd);
                            continue;
                    }
                }
            }
            if (DirOffset(offsetDir, num1) + 4 <= offsetBase + length)
            {
                int num2 = ExifIo.ReadInt(section, offsetDir + 2 + 12 * num1, littleEndian);
                if (num2 > 0)
                {
                    int offsetDir1 = offsetBase + num2;
                    if (offsetDir1 <= offsetBase + length && offsetDir1 >= offsetBase)
                    {
                        ProcessExifDir(section, offsetDir1, offsetBase, length, depth + 1, ifd);
                    }
                }
            }
            if (info.ThumbnailData != null || info.ThumbnailOffset <= 0 || info.ThumbnailSize <= 0)
            {
                return;
            }

            info.ThumbnailData = new byte[info.ThumbnailSize];
            Array.Copy(section, offsetBase + info.ThumbnailOffset, info.ThumbnailData, 0, info.ThumbnailSize);
        }

        private void ProcessSof(byte[] section)
        {
            info.Height = section[3] << 8 | section[4];
            info.Width = section[5] << 8 | section[6];
            info.IsColor = section[7] == 3;
        }
    }
}