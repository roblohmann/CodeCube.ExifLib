using System.IO;
using CodeCube.ExifLib.Enumerations;
using NUnit.Framework;

namespace CodeCube.ExifLib.Test
{
    [TestFixture]
    public sealed class ExifReaderTest
    {
        //[Test]
        //public void MinimalTest_GetGPSLocationFromImage_With_JPEGInfo()
        //{
        //    //Setup
        //    JpegInfo jpegInfo;
        //    FileInfo fileInfo = new FileInfo("london-bridge.jpg");

        //    //Act
        //    // Load a filestream and put its content into the byte[]
        //    using (FileStream fs = fileInfo.OpenRead())
        //    {
        //        jpegInfo = ExifReader.ReadJpeg(fs);
        //    }

        //    //Assert
        //    Assert.IsNotNull(jpegInfo);

        //    var latitude = jpegInfo.GpsLatitude[2];
        //    var longitude = jpegInfo.GpsLongitude[2];

        //    const double expectedLatitude = 14.78;
        //    const double expectedLongitude = 28.47;

        //    Assert.AreEqual(expectedLatitude,latitude);
        //    Assert.AreEqual(expectedLongitude, longitude);
        //}

        [Test]
        public void MinimalTest_GetGPSLocationFromImage_With_JPEGInfo()
        {
            //Setup
            JpegInfo jpegInfo;
            FileInfo fileInfo = new FileInfo("london-bridge.jpg");

            //Act
            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                jpegInfo = ExifReader.Read(fs);
            }

            //Assert
            Assert.IsNotNull(jpegInfo);

            var latitude = jpegInfo.GpsLatitude[2];
            var longitude = jpegInfo.GpsLongitude[2];

            const double expectedLatitude = 14.78;
            const double expectedLongitude = 28.47;

            Assert.AreEqual(expectedLatitude, latitude);
            Assert.AreEqual(expectedLongitude, longitude);
        }

        [Test]
        public void MinimalTest_GetGPSLocationFromImage()
        {
            //Setup
            FileInfo fileInfo = new FileInfo("london-bridge.jpg");

            const double expectedLatitude = 14.78;
            const double expectedLongitude = 28.47;

            double[] latitude;
            double[] longitude;

            //Act
            // Load a filestream and put its content into the byte[]
            using (FileStream fs = fileInfo.OpenRead())
            {
                var reader = new ExifReaderExtended(fs);

                reader.GetTagValue(ExifTags.GPSLatitude, out latitude);
                reader.GetTagValue(ExifTags.GPSLongitude, out longitude);
            }

            //Assert
            Assert.AreEqual(expectedLatitude, latitude[2]);
            Assert.AreEqual(expectedLongitude, longitude[2]);
        }
    }
}
