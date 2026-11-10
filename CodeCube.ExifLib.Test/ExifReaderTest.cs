using System.IO;
using CodeCube.ExifLib.Enumerations;
using Xunit;

namespace CodeCube.ExifLib.Test
{
    public sealed class ExifReaderTest
    {
        private const double ExpectedLatitude = 14.78;
        private const double ExpectedLongitude = 28.47;

        [Fact]
        public void MinimalTest_GetGPSLocationFromImage_With_JPEGInfo()
        {
            //Setup
            JpegInfo jpegInfo;
            FileInfo fileInfo = new FileInfo("london-bridge.jpg");

            //Act
            using (FileStream fs = fileInfo.OpenRead())
            {
                jpegInfo = ExifReader.Read(fs);
            }

            //Assert
            Assert.NotNull(jpegInfo);
            Assert.Equal(ExpectedLatitude, jpegInfo.GpsLatitude[2]);
            Assert.Equal(ExpectedLongitude, jpegInfo.GpsLongitude[2]);
        }

        [Fact]
        public void MinimalTest_GetGPSLocationFromImage()
        {
            //Setup
            FileInfo fileInfo = new FileInfo("london-bridge.jpg");

            double[] latitude;
            double[] longitude;

            //Act
            using (FileStream fs = fileInfo.OpenRead())
            {
                var reader = new ExifReaderExtended(fs);

                reader.GetTagValue(ExifTags.GPSLatitude, out latitude);
                reader.GetTagValue(ExifTags.GPSLongitude, out longitude);
            }

            //Assert
            Assert.Equal(ExpectedLatitude, latitude[2]);
            Assert.Equal(ExpectedLongitude, longitude[2]);
        }
    }
}
