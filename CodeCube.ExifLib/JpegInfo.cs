using System;
using CodeCube.ExifLib.Enumerations;

namespace CodeCube.ExifLib
{
    public class JpegInfo
    {
        public double[] GpsLatitude = new double[3];
        public double[] GpsLongitude = new double[3];

        public string FileName { get; set; }

        public int FileSize { get; set; }

        public bool IsValid { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public bool IsColor { get; set; }

        public ExifOrientation Orientation { get; set; }

        public double XResolution { get; set; }

        public double YResolution { get; set; }

        public ExifUnit ResolutionUnit { get; set; }

        public string DateTime { get; set; }

        public string DateTimeOriginal { get; set; }

        public string Description { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Software { get; set; }

        public string Artist { get; set; }

        public string Copyright { get; set; }

        public string UserComment { get; set; }

        public double ExposureTime { get; set; }

        public double FNumber { get; set; }

        public ExifFlash Flash { get; set; }

        public ExifGpsLatitudeRef GpsLatitudeRef { get; set; }

        public ExifGpsLongitudeRef GpsLongitudeRef { get; set; }

        public int ThumbnailOffset { get; set; }

        public int ThumbnailSize { get; set; }

        public byte[] ThumbnailData { get; set; }

        /// <summary>
        /// This time is in UTC>
        /// </summary>
        public TimeSpan LoadTime { get; set; }
    }
}
