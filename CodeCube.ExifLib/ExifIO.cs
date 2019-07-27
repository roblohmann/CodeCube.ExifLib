using System;

namespace CodeCube.ExifLib
{
    public static class ExifIo
    {
        public static short ReadShort(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt16(data, offset);
            }

            return BitConverter.ToInt16(new[] { data[offset + 1], data[offset] }, 0);
        }

        public static ushort ReadUShort(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(data, offset);
            }

            return BitConverter.ToUInt16(new[] { data[offset + 1], data[offset] }, 0);
        }

        public static int ReadInt(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(data, offset);
            }

            return BitConverter.ToInt32(new[] { data[offset + 3], data[offset + 2], data[offset + 1], data[offset] }, 0);
        }

        public static uint ReadUInt(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt32(data, offset);
            }

            return BitConverter.ToUInt32(new[] { data[offset + 3], data[offset + 2], data[offset + 1], data[offset] }, 0);
        }

        public static float ReadSingle(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToSingle(data, offset);
            }

            return BitConverter.ToSingle(new[] { data[offset + 3], data[offset + 2], data[offset + 1], data[offset] }, 0);
        }

        public static double ReadDouble(byte[] data, int offset, bool littleEndian)
        {
            if (littleEndian && BitConverter.IsLittleEndian || !littleEndian && !BitConverter.IsLittleEndian)
            {
                return BitConverter.ToDouble(data, offset);
            }

            return BitConverter.ToDouble(new[]{
                                            data[offset + 7],
                                            data[offset + 6],
                                            data[offset + 5],
                                            data[offset + 4],
                                            data[offset + 3],
                                            data[offset + 2],
                                            data[offset + 1],
                                            data[offset]
                                    }, 0);
        }
    }
}
