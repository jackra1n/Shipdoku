﻿using System;
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
            var playingfield = new ShipdokuModel();

            foreach (var shipLegth in _ships.OrderByDescending(l => l))
            {
                int startX;
                int startY;

                int tryCount = 0;

                EShipStartDirection direction;
                do
                {
                    // Trz again after 10 unsuccessful tries of trying to Place a ship
                    if (tryCount++ > 10)
                    {
                        return GenerateShipdokuModel();
                    }

                    startX = _random.Next(PlayingFieldWidht);
                    startY = _random.Next(PlayingFieldHeight);

                    direction = (EShipStartDirection)_random.Next(4);

                } while (!CheckSurroundingsOfShip(shipdokuField, startX, startY, shipLegth, direction));

                FillInShip(shipdokuField, startX, startY, shipLegth, direction);
            }

            ReplaceEmptyFieldWithWater(shipdokuField);

            playingfield.HorizontalCounts = GetShipCountsForRows(shipdokuField);

            playingfield.VerticalCounts = GetShipCountsForColumns(shipdokuField);

            // ToDo: Checken, dass Solved und nicht Solved nicht die gleiche Referenz haben
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
                field[xCoordinate, yCoordinate] = EShipdokuField.ShipMiddle;

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

        private static bool CheckSurroundingsOfShip(EShipdokuField[,] field, int xCoordinate, int yCoordinate, int length, EShipStartDirection direction)
        {
            for (int i = 0; i < length; i++)
            {
                if (!CheckSurroundingsOfField(field, xCoordinate, yCoordinate))
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
        
        private static bool CheckSurroundingsOfField(EShipdokuField[,] field, in int xCoordinate, in int yCoordinate)
        {
            if (xCoordinate > 7 || xCoordinate < 0
                || yCoordinate > 7 || yCoordinate < 0
                || field[xCoordinate, yCoordinate] == EShipdokuField.ShipMiddle // Feld checken
                || (xCoordinate != 0 && field[xCoordinate - 1, yCoordinate] == EShipdokuField.ShipMiddle)
                || (xCoordinate != PlayingFieldWidht - 1 && field[xCoordinate + 1, yCoordinate] == EShipdokuField.ShipMiddle)
                || (yCoordinate != 0 && field[xCoordinate, yCoordinate - 1] == EShipdokuField.ShipMiddle)
                || (yCoordinate != PlayingFieldHeight - 1 && field[xCoordinate, yCoordinate + 1] == EShipdokuField.ShipMiddle))
            {
                return false;
            }

            return true;
        }

        private static void ReplaceEmptyFieldWithWater(EShipdokuField[,] field)
        {
            for (int i = 0; i < PlayingFieldWidht; i++)
            {
                for (int j = 0; j < PlayingFieldWidht; j++)
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
