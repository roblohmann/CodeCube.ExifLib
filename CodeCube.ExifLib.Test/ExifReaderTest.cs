using System.IO;
using NUnit.Framework;

namespace CodeCube.ExifLib.Test
{
    [TestFixture]
    public sealed class ExifReaderTest
    {
        [Test]
        public void MinimalTest_GetGPSLocationFromIMage()
        {
            //Setup
            JpegInfo jpegInfo;
            FileInfo fileInfo = new FileInfo("london-bridge.jpg");

            //Act
            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                jpegInfo = ExifReader.ReadJpeg(fs);
            }

            //Assert
            Assert.IsNotNull(jpegInfo);

            var latitude = jpegInfo.GpsLatitude[2];
            var longitude = jpegInfo.GpsLongitude[2];

            const double expectedLatitude = 14.78;
            const double expectedLongitude = 28.47;

            Assert.AreEqual(expectedLatitude,latitude);
            Assert.AreEqual(expectedLongitude, longitude);
        }
    }
}
