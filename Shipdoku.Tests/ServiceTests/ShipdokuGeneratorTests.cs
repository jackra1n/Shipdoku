using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Shipdoku.Enums;
using Shipdoku.Services;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Shipdoku.Tests.ServiceTests
{
    public class ShipdokuGeneratorTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ShipdokuGenerator _testee;

        public ShipdokuGeneratorTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _testee = new ShipdokuGenerator();
        }

        [Fact]
        public void GenerateShipdokuModel_VerticalShipCountsAddsUp()
        {
            // Arrange
            int expectedShipCount = 20;

            // Act
            var result = _testee.GenerateShipdokuModel();

            // Assert
            Assert.Equal(expectedShipCount, result.VerticalCounts.Sum(i => i));
        }

        [Fact]
        public void GenerateShipdokuModel_HorizontalShipCountsAddsUp()
        {
            // Arrange
            int expectedShipCount = 20;

            // Act
            var result = _testee.GenerateShipdokuModel();

            // Assert
            Assert.Equal(expectedShipCount, result.HorizontalCounts.Sum(i => i));
        }

        [Fact]
        public void GenerateShipdokuModel_VerticalShipCountIsCorrect()
        {
            // Act
            var result = _testee.GenerateShipdokuModel();

            PrintField(result.SolvedShipdokuField, result.HorizontalCounts, result.VerticalCounts);

            // Assert
            for (int column = 0; column < result.SolvedShipdokuField.GetLength(0); column++)
            {
                Assert.Equal(result.VerticalCounts[column], GetShipCountForColumn(result.SolvedShipdokuField, column));
            }
        }

        [Fact]
        public void GenerateShipdokuModel_HorizontalShipCountIsCorrect()
        {
            // Act
            var result = _testee.GenerateShipdokuModel();

            // Assert
            for (int row = 0; row < result.SolvedShipdokuField.GetLength(1); row++)
            {
                Assert.Equal(result.HorizontalCounts[row], GetShipCountForRow(result.SolvedShipdokuField, row));
            }
        }

        [Fact]
        public void GenerateShipdokuModel_ShipsArePlacedCorrect()
        {
            // Act
            var result = _testee.GenerateShipdokuModel();

            // Assert
            for (int column = 0; column < result.SolvedShipdokuField.GetLength(0); column++)
            {
                for (int row = 0; row < result.SolvedShipdokuField.GetLength(1); row++)
                {
                    Assert.True(CheckIfFieldIsValid(result.SolvedShipdokuField, column, row));
                }
            }
        }

        #region Helper Methods

        private int GetShipCountForRow(EShipdokuField[,] playingField, int row)
        {
            int shipCount = 0;

            for (int column = 0; column < playingField.GetLength(0); column++)
            {
                if (playingField[column, row] != EShipdokuField.Water)
                    shipCount++;
            }

            return shipCount;
        }

        private int GetShipCountForColumn(EShipdokuField[,] playingField, int column)
        {
            int shipCount = 0;

            for (int row = 0; row < playingField.GetLength(1); row++)
            {
                if (playingField[column, row] != EShipdokuField.Water)
                    shipCount++;
            }

            return shipCount;
        }

        private void PrintField(EShipdokuField[,] playingField, int[] horizontalCounts, int[] verticalCounts)
        {
            for (int row = 0; row < playingField.GetLength(1); row++)
            {
                string rowString = "";

                for (int column = 0; column < playingField.GetLength(0); column++)
                {
                    rowString += playingField[column, row] + " ";
                }

                _logger.WriteLine(rowString + horizontalCounts[row]);
            }
        }

        private bool CheckIfFieldIsValid(EShipdokuField[,] solvedShipdokuField, int column, int row)
        {
            if (solvedShipdokuField[column, row] == EShipdokuField.Water ||
                GetCountOfSurroundingShips(solvedShipdokuField, column, row) == 0 ||
                GetCountOfSurroundingShips(solvedShipdokuField, column, row) == 1 ||
                GetCountOfSurroundingShips(solvedShipdokuField, column, row) == 2 &&
                CheckIfTwoSurroundingShipTilesAreValid(solvedShipdokuField, column, row))
            {
                return true;
            }

             return false;
        }

        private bool CheckIfTwoSurroundingShipTilesAreValid(EShipdokuField[,] solvedShipdokuField, int column, int row)
        {
            bool upperFieldIsShip = row != 0 
                                    && solvedShipdokuField[column, row - 1] != EShipdokuField.Water;

            bool lowerFieldIsShip = row < (solvedShipdokuField.GetLength(1) - 1)
                                    && solvedShipdokuField[column, row + 1] != EShipdokuField.Water;

            bool leftFieldIsShip = column != 0 
                                   && solvedShipdokuField[column - 1, row] != EShipdokuField.Water;

            bool rightFieldIsShip = column < (solvedShipdokuField.GetLength(0) - 1) 
                                    && solvedShipdokuField[column + 1, row] != EShipdokuField.Water;

            if (rightFieldIsShip && leftFieldIsShip || lowerFieldIsShip && upperFieldIsShip)
            {
                return true;
            }

            return false;
        }

        private int GetCountOfSurroundingShips(EShipdokuField[,] solvedShipdokuField, int column, int row)
        {
            int count = 0;

            if (column < (solvedShipdokuField.GetLength(0) - 1)
                && solvedShipdokuField[column + 1, row] != EShipdokuField.Water)
                count++;

            if (column != 0
                && solvedShipdokuField[column - 1, row] != EShipdokuField.Water)
                count++;

            if (row < (solvedShipdokuField.GetLength(1) - 1)
                && solvedShipdokuField[column, row + 1] != EShipdokuField.Water)
                count++;

            if (row != 0
                && solvedShipdokuField[column, row - 1] != EShipdokuField.Water)
                count++;

            return count;
        }



        #endregion
    }
}
