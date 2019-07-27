using System;
using System.Globalization;
using System.Text;
using CodeCube.ExifLib.Enumerations;

namespace CodeCube.ExifLib
{
    public class ExifTag
    {
        private static readonly int[] BytesPerFormat = {0,1,1,2,4,8,1,1,2,4,8,4,8};

        public int Tag { get; }

        public ExifTagFormat Format { get; }

        public int Components { get; }

        public byte[] Data { get; }

        public bool LittleEndian { get; }

        public ExifTag(byte[] section,int sectionOffset,int offsetBase,int length,bool littleEndian)
        {
            IsValid = false;
            Tag = ExifIo.ReadUShort(section, sectionOffset, littleEndian);
            int index = ExifIo.ReadUShort(section, sectionOffset + 2, littleEndian);
            if (index < 1 || index > 12)
            {
                return;
            }

            Format = (ExifTagFormat)index;
            Components = ExifIo.ReadInt(section, sectionOffset + 4, littleEndian);
            if (Components > 65536)
            {
                return;
            }

            LittleEndian = littleEndian;
            int length1 = Components * BytesPerFormat[index];
            int sourceIndex;
            if (length1 > 4)
            {
                int num = ExifIo.ReadInt(section, sectionOffset + 8, littleEndian);
                if (num + length1 > length)
                    return;
                sourceIndex = offsetBase + num;
            }
            else
            {
                sourceIndex = sectionOffset + 8;
            }

            Data = new byte[length1];
            Array.Copy(section, sourceIndex, Data, 0, length1);
            IsValid = true;
        }

        public bool IsValid { get; }

        private short ReadShort(int offset)
        {
            return ExifIo.ReadShort(Data, offset, LittleEndian);
        }

        private ushort ReadUShort(int offset)
        {
            return ExifIo.ReadUShort(Data, offset, LittleEndian);
        }

        private int ReadInt(int offset)
        {
            return ExifIo.ReadInt(Data, offset, LittleEndian);
        }

        private uint ReadUInt(int offset)
        {
            return ExifIo.ReadUInt(Data, offset, LittleEndian);
        }

        private float ReadSingle(int offset)
        {
            return ExifIo.ReadSingle(Data, offset, LittleEndian);
        }

        private double ReadDouble(int offset)
        {
            return ExifIo.ReadDouble(Data, offset, LittleEndian);
        }

        public bool IsNumeric
        {
            get
            {
                switch (Format)
                {
                    case ExifTagFormat.STRING:
                    case ExifTagFormat.UNDEFINED:
                        return false;
                    default:
                        return true;
                }
            }
        }

        public int GetInt(int componentIndex)
        {
            return (int)GetNumericValue(componentIndex);
        }

        public double GetNumericValue(int componentIndex)
        {
            switch (Format)
            {
                case ExifTagFormat.BYTE:
                    return Data[componentIndex];
                case ExifTagFormat.USHORT:
                    return ReadUShort(componentIndex * 2);
                case ExifTagFormat.ULONG:
                    return ReadUInt(componentIndex * 4);
                case ExifTagFormat.URATIONAL:
                    return ReadUInt(componentIndex * 8) / (double)ReadUInt(componentIndex * 8 + 4);
                case ExifTagFormat.SBYTE:
                    return (sbyte)Data[componentIndex];
                case ExifTagFormat.SSHORT:
                    return ReadShort(componentIndex * 2);
                case ExifTagFormat.SLONG:
                    return ReadInt(componentIndex * 4);
                case ExifTagFormat.SRATIONAL:
                    return ReadInt(componentIndex * 8) / (double)ReadInt(componentIndex * 8 + 4);
                case ExifTagFormat.SINGLE:
                    return ReadSingle(componentIndex * 4);
                case ExifTagFormat.DOUBLE:
                    return ReadDouble(componentIndex * 8);
                default:
                    return 0.0;
            }
        }

        public string GetStringValue()
        {
            return GetStringValue(0);
        }

        public string GetStringValue(int componentIndex)
        {
            switch (Format)
            {
                case ExifTagFormat.STRING:
                case ExifTagFormat.UNDEFINED:
                    return Encoding.UTF8.GetString(Data, 0, Data.Length).Trim(' ', '\t', '\r', '\n', char.MinValue);
                case ExifTagFormat.URATIONAL:
                    return ReadUInt(componentIndex * 8).ToString() + "/" + ReadUInt(componentIndex * 8 + 4).ToString();
                case ExifTagFormat.SRATIONAL:
                    return ReadInt(componentIndex * 8).ToString() + "/" + ReadInt(componentIndex * 8 + 4).ToString();
                default:
                    return GetNumericValue(componentIndex).ToString(CultureInfo.InvariantCulture);
            }
        }

        public virtual void Populate(JpegInfo info, ExifIFD ifd)
        {
            switch (ifd)
            {
                case ExifIFD.Exif:
                    switch ((ExifId)Tag)
                    {
                        case ExifId.ImageWidth:
                            info.Width = GetInt(0);
                            return;
                        case ExifId.ImageHeight:
                            info.Height = GetInt(0);
                            return;
                        case ExifId.Description:
                            info.Description = GetStringValue();
                            return;
                        case ExifId.Make:
                            info.Make = GetStringValue();
                            return;
                        case ExifId.Model:
                            info.Model = GetStringValue();
                            return;
                        case (ExifId)273:
                            return;
                        case ExifId.Orientation:
                            info.Orientation = (ExifOrientation)GetInt(0);
                            return;
                        case ExifId.XResolution:
                            info.XResolution = GetNumericValue(0);
                            return;
                        case ExifId.YResolution:
                            info.YResolution = GetNumericValue(0);
                            return;
                        case ExifId.ResolutionUnit:
                            info.ResolutionUnit = (ExifUnit)GetInt(0);
                            return;
                        case ExifId.Software:
                            info.Software = GetStringValue();
                            return;
                        case ExifId.DateTime:
                            info.DateTime = GetStringValue();
                            return;
                        case ExifId.Artist:
                            info.Artist = GetStringValue();
                            return;
                        case ExifId.ThumbnailOffset:
                            info.ThumbnailOffset = GetInt(0);
                            return;
                        case ExifId.ThumbnailLength:
                            info.ThumbnailSize = GetInt(0);
                            return;
                        case ExifId.Copyright:
                            info.Copyright = GetStringValue();
                            return;
                        case (ExifId)33433:
                            return;
                        case ExifId.ExposureTime:
                            info.ExposureTime = GetNumericValue(0);
                            return;
                        case ExifId.FNumber:
                            info.FNumber = GetNumericValue(0);
                            return;
                        case ExifId.DateTimeOriginal:
                            info.DateTimeOriginal = GetStringValue();
                            return;
                        case ExifId.FlashUsed:
                            info.Flash = (ExifFlash)GetInt(0);
                            return;
                        case ExifId.UserComment:
                            info.UserComment = GetStringValue();
                            return;
                        default:
                            return;
                    }
                case ExifIFD.Gps:
                    switch (Tag)
                    {
                        case 1:
                            if (GetStringValue() == "N")
                            {
                                info.GpsLatitudeRef = ExifGpsLatitudeRef.North;
                                return;
                            }
                            if (GetStringValue() != "S")
                                return;
                            info.GpsLatitudeRef = ExifGpsLatitudeRef.South;
                            return;
                        case 2:
                            if (Components != 3)
                                return;
                            info.GpsLatitude[0] = GetNumericValue(0);
                            info.GpsLatitude[1] = GetNumericValue(1);
                            info.GpsLatitude[2] = GetNumericValue(2);
                            return;
                        case 3:
                            if (GetStringValue() == "E")
                            {
                                info.GpsLongitudeRef = ExifGpsLongitudeRef.East;
                                return;
                            }
                            if (GetStringValue() != "W")
                                return;
                            info.GpsLongitudeRef = ExifGpsLongitudeRef.West;
                            return;
                        case 4:
                            if (Components != 3)
                                return;
                            info.GpsLongitude[0] = GetNumericValue(0);
                            info.GpsLongitude[1] = GetNumericValue(1);
                            info.GpsLongitude[2] = GetNumericValue(2);
                            return;
                        default:
                            return;
                    }
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(64);
            stringBuilder.Append("0x");
            stringBuilder.Append(Tag.ToString("X4"));
            stringBuilder.Append("-");
            stringBuilder.Append(((ExifId)Tag).ToString());
            if (Components > 0)
            {
                stringBuilder.Append(": (");
                stringBuilder.Append(GetStringValue(0));
                if (Format != ExifTagFormat.UNDEFINED && Format != ExifTagFormat.STRING)
                {
                    for (int componentIndex = 1; componentIndex < Components; ++componentIndex)
                        stringBuilder.Append(", " + GetStringValue(componentIndex));
                }
                stringBuilder.Append(")");
            }
            return stringBuilder.ToString();
        }
    }
}
