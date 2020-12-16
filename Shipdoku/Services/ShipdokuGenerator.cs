using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;
using System;
using System.Linq;

namespace Shipdoku.Services
{
    ///<inheritdoc cref="IShipdokuGenerator"/>
    public class ShipdokuGenerator : IShipdokuGenerator
    {
        private const int PlayingFieldHeight = 8;
        private const int PlayingFieldWidht = 8;
        private readonly int[] _ships = {1,1,1,1,2,2,2,3,3,4};
        private readonly Random _random = new Random();

        public ShipdokuModel GenerateShipdokuModel()
        {
            var shipdokuField = new EShipdokuField[PlayingFieldWidht,PlayingFieldHeight];
            ReplaceEmptyFieldsWithWater(shipdokuField);
            var playingfield = new ShipdokuModel();

            foreach (var shipLegth in _ships.OrderByDescending(l => l))
            {
                int startX;
                int startY;

                int tryCount = 0;

                EShipStartDirection direction;
                do
                {
                    // Try again after 20 unsuccessful tries of trying to Place a ship
                    if (tryCount++ > 20)
                    {
                        return GenerateShipdokuModel();
                    }

                    startX = _random.Next(PlayingFieldWidht);
                    startY = _random.Next(PlayingFieldHeight);

                    direction = (EShipStartDirection)_random.Next(4);

                } while (!CheckSurroundingsOfShip(shipdokuField, startX, startY, shipLegth, direction));

                FillInShip(shipdokuField, startX, startY, shipLegth, direction);
            }


            playingfield.HorizontalCounts = GetShipCountsForRows(shipdokuField);

            playingfield.VerticalCounts = GetShipCountsForColumns(shipdokuField);

            playingfield.SolvedShipdokuField = shipdokuField;

            playingfield.ShipdokuField = DissolveField(shipdokuField);

            return playingfield;
        }

        /// <summary>
        /// Dissolves the given Playingfield
        /// </summary>
        /// <param name="shipdokuField">ShipdokuField to dissolve</param>
        /// <returns>Dissolved Field</returns>
        private EShipdokuField[,] DissolveField(EShipdokuField[,] shipdokuField)
        {
            EShipdokuField[,] dissolvedField = new EShipdokuField[PlayingFieldHeight,PlayingFieldWidht];
            for (int j = 0; j < PlayingFieldHeight; j++)
            {
                for (int i = 0; i < PlayingFieldWidht; i++)
                {
                    if (_random.Next(9) == 1)
                        dissolvedField[j, i] = shipdokuField[j, i];
                }
            }

            return dissolvedField;
        }

        /// <summary>
        /// Gets the horizontal Ship-counts as an Array
        /// </summary>
        /// <param name="field">Generated Playingfield</param>
        /// <returns>Horizontal Shipcounts as Array</returns>
        private static int[] GetShipCountsForRows(EShipdokuField[,] field)
        {
            int[] counts = new int[PlayingFieldHeight];

            for (int row = 0; row < PlayingFieldHeight; row++)
            {
                int count = 0;

                for (int column = 0; column < PlayingFieldWidht; column++)
                {
                    if (field[column, row] != EShipdokuField.Water)
                        count++;
                }

                counts[row] = count;
            }
            
            return counts;
        }

        /// <summary>
        /// Gets the vertical Ship-counts as an Array
        /// </summary>
        /// <param name="field">Generated Playingfield</param>
        /// <returns>Vertical Shipcounts as Array</returns>
        private static int[] GetShipCountsForColumns(EShipdokuField[,] field)
        {
            int[] counts = new int[PlayingFieldWidht];

            for (int column = 0; column < PlayingFieldWidht; column++)
            {
                int count = 0;

                for (int row = 0; row < PlayingFieldHeight; row++)
                {
                    if (field[column, row] != EShipdokuField.Water)
                        count++;
                }

                counts[column] = count;
            }

            return counts;
        }

        /// <summary>
        /// Places a Ship on the Playinfield
        /// </summary>
        /// <param name="field">Playingfield</param>
        /// <param name="xCoordinate">Start X-Coordinate of Ship</param>
        /// <param name="yCoordinate">Start Y-Coordinate of Ship</param>
        /// <param name="length">Length of ship</param>
        /// <param name="direction">Direction of ship</param>
        private static void FillInShip(EShipdokuField[,] field, int xCoordinate, int yCoordinate, int length, EShipStartDirection direction)
        {
            for (int i = 0; i < length; i++)
            {
                var ka = GetShipPart(i + 1, length, direction);
                field[yCoordinate, xCoordinate] = GetShipPart(i + 1, length, direction);

                switch (direction)
                {
                    case EShipStartDirection.Down:
                        yCoordinate++;
                        break;
                    case EShipStartDirection.Up:
                        yCoordinate--;
                        break;
                    case EShipStartDirection.Left:
                        xCoordinate--;
                        break;
                    case EShipStartDirection.Right:
                        xCoordinate++;
                        break;
                }

            }
        }

        /// <summary>
        /// Retuns the right Shipf-Part for the Ship Index
        /// </summary>
        /// <param name="index">Field Index</param>
        /// <param name="length">Lengt of ship</param>
        /// <param name="direction">Direction of Ship</param>
        /// <returns></returns>
        private static EShipdokuField GetShipPart(int index, int length, EShipStartDirection direction)
        {
            if (length == 1)
            {
                return EShipdokuField.ShipSingle;
            }
            if (index == 1)
            {
                return GetShipFieldFromDirection(direction);
            }
            if (index == length)
            {
                return GetShipFieldFromDirection(InvertDirection(direction));
            }
            
            return EShipdokuField.ShipMiddle;
        }

        /// <summary>
        /// Inverts the direction of the Ship-Startposition
        /// </summary>
        /// <param name="direction">Original direction</param>
        /// <returns>Inverter direction</returns>
        private static EShipStartDirection InvertDirection(EShipStartDirection direction)
        {
            return direction switch
            {
                EShipStartDirection.Up => EShipStartDirection.Down,
                EShipStartDirection.Down => EShipStartDirection.Up,
                EShipStartDirection.Left => EShipStartDirection.Right,
                EShipStartDirection.Right => EShipStartDirection.Left,
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Returns a Ship-Part for the given Direction
        /// </summary>
        /// <param name="direction">Direction of ship</param>
        /// <returns>Ship Part</returns>
        private static EShipdokuField GetShipFieldFromDirection(EShipStartDirection direction)
        {
            return direction switch
            {
                EShipStartDirection.Up => EShipdokuField.ShipDown,
                EShipStartDirection.Down => EShipdokuField.ShipUp,
                EShipStartDirection.Left => EShipdokuField.ShipRight,
                EShipStartDirection.Right => EShipdokuField.ShipLeft,
                _ => throw new NotSupportedException()
            };
        }

        /// <summary>
        /// Checks, if a ship can be placed in the given Position
        /// </summary>
        /// <param name="field">Playingfield</param>
        /// <param name="xCoordinate">Start X-Coordinate of Ship</param>
        /// <param name="yCoordinate">Start Y-Coordinate of Ship</param>
        /// <param name="length">Length of Ship</param>
        /// <param name="direction">Direction of Ship</param>
        /// <returns>True if Position is Valid</returns>
        private static bool CheckSurroundingsOfShip(EShipdokuField[,] field, int xCoordinate, int yCoordinate, int length, EShipStartDirection direction)
        {
            for (int i = 0; i < length; i++)
            {
                if (!CheckSurroundingsOfField(field, yCoordinate, xCoordinate))
                {
                    return false;
                }

                switch (direction)
                {
                    case EShipStartDirection.Down:
                        yCoordinate++;
                        break;
                    case EShipStartDirection.Up:
                        yCoordinate--;
                        break;
                    case EShipStartDirection.Left:
                        xCoordinate--;
                        break;
                    case EShipStartDirection.Right:
                        xCoordinate++;
                        break;
                }

            }

            return true;
        }

        /// <summary>
        /// Checks, if the surroundings of a given Field are already Taken by an other Ship
        /// </summary>
        /// <param name="field">Playingfield</param>
        /// <param name="yCoordinate">Start X-Coordinate of Ship</param>
        /// <param name="xCoordinate">Start Y-Coordinate of Ship</param>
        /// <returns>True if there are no surrounding Ships</returns>
        private static bool CheckSurroundingsOfField(EShipdokuField[,] field, int yCoordinate, int xCoordinate)
        { 
            if (yCoordinate > PlayingFieldWidht - 1 || yCoordinate < 0
                || xCoordinate > PlayingFieldHeight - 1 || xCoordinate < 0
                || field[yCoordinate, xCoordinate] != EShipdokuField.Water
                || !CheckForDiagonalShips(field, yCoordinate, xCoordinate)
                || (yCoordinate != 0 && field[yCoordinate - 1, xCoordinate] != EShipdokuField.Water)
                || (yCoordinate != PlayingFieldWidht - 1 && field[yCoordinate + 1, xCoordinate] != EShipdokuField.Water)
                || (xCoordinate != 0 && field[yCoordinate, xCoordinate - 1] != EShipdokuField.Water)
                || (xCoordinate != PlayingFieldHeight - 1 && field[yCoordinate, xCoordinate + 1] != EShipdokuField.Water))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if there are any ships diagonaly to the given Coordinate
        /// </summary>
        /// <param name="solvedShipdokuField">Playingfield</param>
        /// <param name="yCoordinate">Start Y-Coordinate of Ship</param>
        /// <param name="xCoordinate">Start X-Coordinate of Ship</param>
        /// <returns></returns>
        private static bool CheckForDiagonalShips(EShipdokuField[,] solvedShipdokuField, int yCoordinate, int xCoordinate)
        {
            if (yCoordinate != 0 && xCoordinate != 0 &&
                    solvedShipdokuField[yCoordinate - 1, xCoordinate - 1] != EShipdokuField.Water)
                return false;

            if (yCoordinate != PlayingFieldWidht - 1 && xCoordinate != 0 &&
                    solvedShipdokuField[yCoordinate + 1, xCoordinate - 1] != EShipdokuField.Water)
                return false;

            if (yCoordinate != PlayingFieldWidht - 1 && xCoordinate != PlayingFieldHeight - 1 &&
                    solvedShipdokuField[yCoordinate + 1, xCoordinate + 1] != EShipdokuField.Water)
                return false;

            if (yCoordinate != 0 && xCoordinate != PlayingFieldHeight - 1 &&
                    solvedShipdokuField[yCoordinate - 1, xCoordinate + 1] != EShipdokuField.Water)
                return false;

            return true;
        }

        /// <summary>
        /// Replaces all empty fields of the playingfield with water
        /// </summary>
        /// <param name="field">Playingfield</param>
        private static void ReplaceEmptyFieldsWithWater(EShipdokuField[,] field)
        {
            for (int i = 0; i < PlayingFieldWidht; i++)
            {
                for (int j = 0; j < PlayingFieldHeight; j++)
                {
                    if (field[i,j] == EShipdokuField.Empty)
                    {
                        field[i, j] = EShipdokuField.Water;
                    }
                }
            }
        }
    }
}
