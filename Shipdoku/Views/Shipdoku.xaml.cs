using Shipdoku.Converters;
using Shipdoku.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shipdoku.Views
{
    /// <summary>
    /// Interaction logic for Shipdoku.xaml
    /// </summary>
    public partial class Shipdoku : UserControl
    {
        public Shipdoku()
        {
            InitializeComponent();

            var imageConverter = new ShipdokuFieldToImageConverter();
            var arrayMultiConverter = new MultiDimensionalCoverter();

            for (int row = 0; row < 9; row++)
            {
                StackPanel gameFieldPanel = new StackPanel
                {
                    Name = "panelRow" + row,
                    Orientation = Orientation.Horizontal
                };
                for (int column = 0; column < 8; column++)
                {
                    if (row < 8)
                    {
                        Binding gameFieldArray = new Binding("PlayingField");
                        Binding xBinding = new Binding();
                        Binding yBinding = new Binding();
                        xBinding.Source = column;
                        yBinding.Source = row;

                        MultiBinding multiBinding = new MultiBinding();
                        multiBinding.Bindings.Add(gameFieldArray);
                        multiBinding.Bindings.Add(yBinding);
                        multiBinding.Bindings.Add(xBinding);
                        multiBinding.Converter = arrayMultiConverter;

                        var btnImage = new Image();
                        btnImage.SetBinding(Image.SourceProperty, multiBinding);

                        var playingFieldbutton = new Button();
                        playingFieldbutton.Width = 50;
                        playingFieldbutton.Height = 50;
                        playingFieldbutton.Content = btnImage;
                        gameFieldPanel.Children.Add(playingFieldbutton);
                    }
                    else
                    {
                        TextBox columnShipCount = new TextBox();
                        columnShipCount.Width = 40;
                        Binding columnCountBinding = new Binding($"ShipdokuModel.VerticalCounts[{column}]");
                        columnShipCount.SetBinding(TextBox.TextProperty, columnCountBinding);
                        gameFieldPanel.Children.Add(columnShipCount);
                    }
                }
                TextBox rowShipCount = new TextBox();
                Binding rowCountBinding = new Binding($"ShipdokuModel.HorizontalCounts[{row}]");
                rowShipCount.SetBinding(TextBox.TextProperty, rowCountBinding);
                gameFieldPanel.Children.Add(rowShipCount);
                PlayingFieldPanel.Children.Add(gameFieldPanel);
                
            }
        }
    }
}
