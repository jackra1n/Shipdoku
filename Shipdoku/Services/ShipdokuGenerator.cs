using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;

namespace Shipdoku.Services
{
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

        private static EShipdokuField GetShipPart(int index, int length, EShipStartDirection direction)
        {
            if (length == 1)
            {
                return EShipdokuField.ShipSingle;
            }
            else if (index == 1)
            {
                return GetShipFieldFromDirection(direction);
            }
            else if (index == length)
            {
                return GetShipFieldFromDirection(InvertDirection(direction));
            }
            else
            {
                return EShipdokuField.ShipMiddle;
            }
        }

        private static EShipStartDirection InvertDirection(EShipStartDirection direction)
        {
            switch (direction)
            {
                case EShipStartDirection.Up:
                    return EShipStartDirection.Down;
                case EShipStartDirection.Down:
                    return EShipStartDirection.Up;
                case EShipStartDirection.Left:
                    return EShipStartDirection.Right;
                case EShipStartDirection.Right:
                    return EShipStartDirection.Left;
                default:
                    throw new NotSupportedException();
            }
        }

        private static EShipdokuField GetShipFieldFromDirection(EShipStartDirection direction)
        {
            switch (direction)
            {
                case EShipStartDirection.Up:
                    return EShipdokuField.ShipDown;
                case EShipStartDirection.Down:
                    return EShipdokuField.ShipUp;
                case EShipStartDirection.Left:
                    return EShipdokuField.ShipRight;
                case EShipStartDirection.Right:
                    return EShipdokuField.ShipLeft;
                default:
                    throw new NotSupportedException();
            }
        }

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
        
        private static bool CheckSurroundingsOfField(EShipdokuField[,] field, int yCoordinate, int xCoordinate)
        { 
            if (yCoordinate > 7 || yCoordinate < 0
                || xCoordinate > 7 || xCoordinate < 0
                || field[yCoordinate, xCoordinate] != EShipdokuField.Water // Feld checken
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
