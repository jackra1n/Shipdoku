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

            playingfield.ShipdokuField = DissolveFiled(shipdokuField);

            return playingfield;
        }

        private EShipdokuField[,] DissolveFiled(EShipdokuField[,] shipdokuField)
        {
            int count = 0;
            foreach (var singleShipdokuField in shipdokuField)
            {
                count++;
                if (_random.Next(6) != 1)
                    singleShipdokuField = EShipdokuField.Empty;
            }

            return shipdokuField;
        }

        private int[] GetShipCountsForRows(EShipdokuField[,] field)
        {
            int[] counts = new int[PlayingFieldHeight];

            for (int j = 0; j < PlayingFieldHeight; j++)
            {
                int count = 0;

                for (int i = 0; i < PlayingFieldWidht; i++)
                {
                    if (field[i, j] != EShipdokuField.Water)
                        count++;
                }

                counts[j] = count;
            }
            
            return counts;
        }

        private int[] GetShipCountsForColumns(EShipdokuField[,] field)
        {
            int[] counts = new int[PlayingFieldWidht];

            for (int j = 0; j < PlayingFieldWidht; j++)
            {
                int count = 0;

                for (int i = 0; i < PlayingFieldHeight; i++)
                {
                    if (field[i, j] != EShipdokuField.Water)
                        count++;
                }

                counts[j] = count;
            }

            return counts;
        }

        private void FillInShip(EShipdokuField[,] field, int xCoordinate, int yCoordinate, int length, EShipStartDirection direction)
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

        private bool CheckSurroundingsOfShip(EShipdokuField[,] field, int xCoordinate, int yCoordinate, int length, EShipStartDirection direction)
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
        
        private bool CheckSurroundingsOfField(EShipdokuField[,] field, in int xCoordinate, in int yCoordinate)
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

        private void ReplaceEmptyFieldWithWater(EShipdokuField[,] field)
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
