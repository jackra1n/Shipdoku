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

namespace Shipdoku.Services
{
    class ExportService : IExportService
    {
        private readonly int pixelMarginImage = 30;
        private readonly int pixelPerField = 50;
        private readonly int pixelMarginBetweenFields = 2;

        public void ExportPlayingFieldToPng(EShipdokuField[,] shipdokuFields)
        {
            var image = CreateBitmapImageFromPlayingField(shipdokuFields);

            SaveImage(image);
        }

        private void SaveImage(Image image)
        {
            string filePath = AskForFilePath();

            if (filePath != string.Empty)
            {
                using var fs = new FileStream(filePath, FileMode.Create);
                
                image.Save(fs, ImageFormat.Jpeg);
            }
        }

        private string AskForFilePath()
        {
            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            fileDialog.Title = "Save an Image File";
            fileDialog.AddExtension = true;

            fileDialog.ShowDialog();

            return fileDialog.FileName;
        }

        private Image CreateBitmapImageFromPlayingField(EShipdokuField[,] shipdokuFields)
        {
            int horizonzalFields = shipdokuFields.GetLength(0);
            int verticalFields = shipdokuFields.GetLength(1);

            int horizontalPixelSum = GetPixelSum(horizonzalFields);
            int verticalPixelSum = GetPixelSum(verticalFields);

            int baseFieldPixelWidth = horizontalPixelSum - 2 * pixelMarginImage;
            int baseFieldPixelHeight = verticalPixelSum - 2 * pixelMarginImage;

            var image = new Bitmap(horizontalPixelSum, verticalPixelSum);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.White);

                Brush blackBrush = new SolidBrush(Color.Black);
                Brush whiteBrush = new SolidBrush(Color.White);

                Rectangle baseRectangle = new Rectangle(pixelMarginImage, pixelMarginImage, baseFieldPixelWidth, baseFieldPixelHeight);

                graphics.FillRectangle(blackBrush, baseRectangle);

                int marginTop = pixelMarginImage + pixelMarginBetweenFields;

                var converter = new ShipdokuFieldToImageConverter();

                for (int row = 0; row < shipdokuFields.GetLength(1); row++)
                {
                    int marginLeft = pixelMarginImage + pixelMarginBetweenFields;


                    for (int column = 0; column < shipdokuFields.GetLength(0); column++)
                    {
                        Rectangle fieldRectangle = new Rectangle(marginLeft, marginTop, pixelPerField, pixelPerField);

                        graphics.FillRectangle(whiteBrush, fieldRectangle);

                        if (shipdokuFields[column, row] != EShipdokuField.Empty)
                        {
                            Image fieldImage = Image.FromFile((string)converter.Convert(shipdokuFields[row, column], null, null, null));

                            graphics.DrawImage(fieldImage, marginLeft, marginTop, pixelPerField, pixelPerField);
                        }

                        marginLeft += pixelMarginBetweenFields + pixelPerField;
                    }

                    marginTop += pixelMarginBetweenFields + pixelPerField;
                }

                graphics.Flush();
            }

            return image;
        }

        private int GetPixelSum(int fieldCount)
        {
            return 2 * pixelMarginImage 
                + fieldCount * pixelPerField 
                + fieldCount * pixelMarginBetweenFields + pixelMarginBetweenFields;
        }
    }
}
