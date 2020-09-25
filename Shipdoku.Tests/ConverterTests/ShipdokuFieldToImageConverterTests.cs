using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipdoku.Converters;
using Shipdoku.Enums;
using Xunit;

namespace Shipdoku.Tests.ConverterTests
{
    public class ShipdokuFieldToImageConverterTests
    {
        private ShipdokuFieldToImageConverter _testee;

        public ShipdokuFieldToImageConverterTests()
        {
            _testee = new ShipdokuFieldToImageConverter();
        }

        [Fact]
        public void ConvertBackMethodThrowsException()
        {
            // Act / Assert
            Assert.Throws<NotSupportedException>(
                () => _testee.ConvertBack(null,
                    null,
                    null,
                    CultureInfo.CurrentCulture));
        }

        [Fact]
        public void ConvertWaterReturnsPathToWaterImage()
        {
            // Arrange
            var fieldType = EShipdokuField.Water;
            
            // Act
            string returnedPath = 
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/water.png", returnedPath);
        }

        [Fact]
        public void ConvertShipMiddleReturnsPathToShipMiddleImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipMiddle;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipMiddle.png", returnedPath);
        }

        [Fact]
        public void ConvertShipUpReturnsPathToShipUpImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipUp;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipUp.png", returnedPath);
        }

        [Fact]
        public void ConvertShipLeftReturnsPathToShipLeftImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipLeft;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipLeft.png", returnedPath);
        }

        [Fact]
        public void ConvertShipDownReturnsPathToShipDownImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipDown;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipDown.png", returnedPath);
        }

        [Fact]
        public void ConvertShipRightReturnsPathToShipRightImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipRight;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipRight.png", returnedPath);
        }

        [Fact]
        public void ConvertShipSingleReturnsPathToShipSinglesImage()
        {
            // Arrange
            var fieldType = EShipdokuField.ShipSingle;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal("Images/shipSingle.png", returnedPath);
        }

        [Fact]
        public void ConvertEmptyFieldReturnsEmptyString()
        {
            // Arrange
            var fieldType = EShipdokuField.Empty;

            // Act
            string returnedPath =
                _testee.Convert(fieldType, null, null, CultureInfo.CurrentCulture)?.ToString();

            // Assert
            Assert.Equal(string.Empty, returnedPath);
        }
    }
}
