namespace CodeCube.ExifLib.Enumerations
{
    public enum ExifTagFormat
    {
        BYTE = 1,
        STRING = 2,
        USHORT = 3,
        ULONG = 4,
        URATIONAL = 5,
        SBYTE = 6,
        UNDEFINED = 7,
        SSHORT = 8,
        SLONG = 9,
        SRATIONAL = 10, // 0x0000000A
        SINGLE = 11, // 0x0000000B
        DOUBLE = 12, // 0x0000000C
        NUM_FORMATS = 12, // 0x0000000C
    }
}
