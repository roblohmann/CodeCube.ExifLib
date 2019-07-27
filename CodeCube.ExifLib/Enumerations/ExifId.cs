namespace CodeCube.ExifLib.Enumerations
{
    public enum ExifId
    {
        Unknown = -1,
        ImageWidth = 256, // 0x00000100
        ImageHeight = 257, // 0x00000101
        Description = 270, // 0x0000010E
        Make = 271, // 0x0000010F
        Model = 272, // 0x00000110
        Orientation = 274, // 0x00000112
        XResolution = 282, // 0x0000011A
        YResolution = 283, // 0x0000011B
        ResolutionUnit = 296, // 0x00000128
        Software = 305, // 0x00000131
        DateTime = 306, // 0x00000132
        Artist = 315, // 0x0000013B
        ThumbnailOffset = 513, // 0x00000201
        ThumbnailLength = 514, // 0x00000202
        Copyright = 33432, // 0x00008298
        ExposureTime = 33434, // 0x0000829A
        FNumber = 33437, // 0x0000829D
        DateTimeOriginal = 36867, // 0x00009003
        FlashUsed = 37385, // 0x00009209
        UserComment = 37510, // 0x00009286
    }
}
