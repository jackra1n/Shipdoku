using Shipdoku.Enums;
using Shipdoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Drawing.Imaging;
using Shipdoku.Converters;
using System.Reflection;
using System.Windows;
using Shipdoku.Models;
using FontStyle = System.Drawing.FontStyle;

namespace Shipdoku.Services
{
    class ExportService : IExportService
    {
        private readonly int pixelMarginImage = 30;
        private readonly int pixelPerField = 50;
        private readonly int pixelMarginBetweenFields = 2;
        private readonly Brush _blackBrush = new SolidBrush(Color.Black);
        private readonly Brush _whiteBrush = new SolidBrush(Color.White);
        private readonly Font _blackFont = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular);
        private readonly ShipdokuFieldToImageConverter _converter = new ShipdokuFieldToImageConverter();

        public void ExportPlayingFieldToPng(ShipdokuModel shipdokuModel, bool exportSolved)
        {
            var fieldToExport = exportSolved ? shipdokuModel.SolvedShipdokuField : shipdokuModel.ShipdokuField;
            var image = CreateBitmapImageFromPlayingField(fieldToExport, shipdokuModel.VerticalCounts, shipdokuModel.HorizontalCounts);

            SaveImage(image);
        }

        /// <summary>
        /// Saves the Image
        /// </summary>
        /// <param name="image">Image to Save</param>
        private void SaveImage(Image image)
        {
            string filePath = AskForFilePath();

            if (!string.IsNullOrEmpty(filePath))
            {
                using var fs = new FileStream(filePath, FileMode.Create);
                
                image.Save(fs, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Asks the User, where he wants to Save the Image
        /// </summary>
        /// <returns>Path to save the Image</returns>
        private string AskForFilePath()
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = "JPeg Image|*.jpg|All files (*.*)|*.*", 
                Title = "Save an Image File", 
                AddExtension = true
            };

            fileDialog.ShowDialog();

            return fileDialog.FileName;
        }

        /// <summary>
        /// Creates the BitmapImage from the specified Field
        /// </summary>
        /// <param name="shipdokuField"></param>
        /// <param name="verticalCounts"></param>
        /// <param name="horizontalCounts"></param>
        /// <returns>The created Image</returns>
        private Image CreateBitmapImageFromPlayingField(EShipdokuField[,] shipdokuField, int[] verticalCounts, int[] horizontalCounts)
        {
            // ToDo: evtl. Refactor
            int horizonzalFields = shipdokuField.GetLength(0);
            int verticalFields = shipdokuField.GetLength(1);

            int baseFieldPixelWidth = GetBaseFieldPixelSum(horizonzalFields);
            int baseFieldPixelHeight = GetBaseFieldPixelSum(verticalFields);

            int horizontalPixelSum = baseFieldPixelWidth + 2 * pixelMarginImage + pixelMarginBetweenFields + pixelPerField;
            int verticalPixelSum = baseFieldPixelHeight + 2 * pixelMarginImage + 3 * pixelMarginBetweenFields + 3 * pixelPerField;

            var image = new Bitmap(horizontalPixelSum, verticalPixelSum);

            using Graphics graphics = Graphics.FromImage(image);

            graphics.Clear(Color.White);

            Rectangle baseRectangle = new Rectangle(pixelMarginImage, pixelMarginImage, baseFieldPixelWidth, baseFieldPixelHeight);

            graphics.FillRectangle(_blackBrush, baseRectangle);

            int marginTop = pixelMarginImage + pixelMarginBetweenFields;

            for (int row = 0; row < shipdokuField.GetLength(1); row++)
            {
                int marginLeft = pixelMarginImage + pixelMarginBetweenFields;


                for (int column = 0; column < shipdokuField.GetLength(0); column++)
                {
                    Rectangle fieldRectangle = new Rectangle(marginLeft, marginTop, pixelPerField, pixelPerField);

                    graphics.FillRectangle(_whiteBrush, fieldRectangle);

                    string path = (string)_converter.Convert(shipdokuField[row, column], null, null, null);
                    if (shipdokuField[row, column] != EShipdokuField.Empty && !string.IsNullOrEmpty(path))
                    {
                        Image fieldImage = Image.FromFile(path);

                        graphics.DrawImage(fieldImage, marginLeft, marginTop, pixelPerField, pixelPerField);
                    }

                    marginLeft += pixelMarginBetweenFields + pixelPerField;
                }

                Rectangle numberRectangle = new Rectangle(marginLeft, marginTop, pixelPerField, pixelPerField);

                graphics.DrawString(verticalCounts[row].ToString(), _blackFont, _blackBrush, numberRectangle);

                marginTop += pixelMarginBetweenFields + pixelPerField;
            }

            int marginCountLeft = pixelMarginImage + pixelMarginBetweenFields;

            for (int column = 0; column < shipdokuField.GetLength(0); column++)
            {
                Rectangle numberRectangle = new Rectangle(marginCountLeft, baseFieldPixelHeight + pixelMarginImage, pixelPerField, pixelPerField);

                graphics.DrawString(horizontalCounts[column].ToString(), _blackFont, _blackBrush, numberRectangle);

                marginCountLeft += pixelMarginBetweenFields + pixelPerField;
            }

            AddShipsToField(graphics, baseFieldPixelHeight + pixelMarginBetweenFields + pixelPerField);

            graphics.Flush();

            return image;
        }

        /// <summary>
        /// Adds the default Ship to the Bottom of the Picture
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="baseFieldHeight"></param>
        private void AddShipsToField(Graphics graphics, int baseFieldHeight)
        {
            // Ships are distributed over two Rows
            EShipdokuField[] firsRow = { EShipdokuField.ShipLeft, EShipdokuField.ShipMiddle, EShipdokuField.ShipMiddle, 
                EShipdokuField.ShipRight, EShipdokuField.ShipLeft, EShipdokuField.ShipMiddle, EShipdokuField.ShipRight, 
                EShipdokuField.ShipLeft, EShipdokuField.ShipMiddle, EShipdokuField.ShipRight };

            EShipdokuField[] secondRow = { EShipdokuField.ShipLeft, EShipdokuField.ShipRight, EShipdokuField.ShipLeft, EShipdokuField.ShipRight,
                EShipdokuField.ShipLeft, EShipdokuField.ShipRight, EShipdokuField.ShipSingle, EShipdokuField.ShipSingle,
                EShipdokuField.ShipSingle, EShipdokuField.ShipSingle};

            // Factor for making these Ships smaller than the normal ships
            double fieldWidhtFactor = 0.75;

            // Margin from top of Image
            int marginTop = baseFieldHeight + pixelMarginImage + pixelMarginBetweenFields;

            // Margin from left side of Image
            int marginLeft = pixelMarginImage + pixelMarginBetweenFields;

            // Draw the first Row
            foreach (var t in firsRow)
            {
                Image fieldImage = Image.FromFile(((string)_converter.Convert(t, null, null, null))!);

                graphics.DrawImage(fieldImage, marginLeft, marginTop, (int)(pixelPerField * fieldWidhtFactor), (int)(pixelPerField * fieldWidhtFactor));

                marginLeft += (int)(pixelMarginBetweenFields + pixelPerField * fieldWidhtFactor);
            }

            marginTop += pixelPerField + pixelMarginBetweenFields;

            marginLeft = pixelMarginImage + pixelMarginBetweenFields;

            // Draw the second Row
            foreach (var t in secondRow)
            {
                Image fieldImage = Image.FromFile(((string)_converter.Convert(t, null, null, null))!);

                graphics.DrawImage(fieldImage, marginLeft, marginTop, (int)(pixelPerField * fieldWidhtFactor), (int)(pixelPerField * fieldWidhtFactor));

                marginLeft += (int)(pixelMarginBetweenFields + pixelPerField * fieldWidhtFactor);
            }
        }

        /// <summary>
        /// Gets the Pixel-Count for the base Field
        /// </summary>
        /// <param name="fieldCount"></param>
        /// <returns></returns>
        private int GetBaseFieldPixelSum(int fieldCount)
        {
            return fieldCount * pixelPerField 
                + (fieldCount + 1) * pixelMarginBetweenFields;
        }
    }
}
