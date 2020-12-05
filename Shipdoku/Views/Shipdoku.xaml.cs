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
            Binding createEmptyBinding = new Binding("CreateEmpty");

            for (int row = 0; row < 9; row++)
            {
                StackPanel gameFieldPanel = new StackPanel
                {
                    Name = "panelRow" + row,
                    Orientation = Orientation.Horizontal
                };
                for (int column = 0; column < 9; column++)
                {
                    if (column < 8 && row != 8)
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

                        Binding fieldClickBinding = new Binding($"ButtonCommand");
                        playingFieldbutton.CommandParameter = $"{row},{column}";
                        playingFieldbutton.SetBinding(Button.CommandProperty, fieldClickBinding);

                        gameFieldPanel.Children.Add(playingFieldbutton);
                    }
                    else if (row == 8 && column != 8)
                    {
                        TextBox columnShipCount = new TextBox();
                        columnShipCount.VerticalContentAlignment = VerticalAlignment.Center;
                        columnShipCount.HorizontalContentAlignment = HorizontalAlignment.Center;
                        columnShipCount.SetBinding(TextBox.IsEnabledProperty, createEmptyBinding);
                        columnShipCount.FontSize = 18;
                        columnShipCount.Width = 50;
                        columnShipCount.Height = 50;
                        Binding columnCountBinding = new Binding($"ShipdokuModel.HorizontalCounts[{column}]");
                        columnShipCount.SetBinding(TextBox.TextProperty, columnCountBinding);
                        gameFieldPanel.Children.Add(columnShipCount);
                    }
                    else if (row != 8)
                    {
                        TextBox rowShipCount = new TextBox();
                        rowShipCount.VerticalContentAlignment = VerticalAlignment.Center;
                        rowShipCount.HorizontalContentAlignment = HorizontalAlignment.Center;
                        rowShipCount.SetBinding(TextBox.IsEnabledProperty, createEmptyBinding);
                        rowShipCount.FontSize = 18;
                        rowShipCount.Width = 50;
                        rowShipCount.Height = 50;
                        Binding rowCountBinding = new Binding($"ShipdokuModel.VerticalCounts[{row}]");
                        rowShipCount.SetBinding(TextBox.TextProperty, rowCountBinding);
                        gameFieldPanel.Children.Add(rowShipCount);
                    }
                }
                PlayingFieldPanel.Children.Add(gameFieldPanel);
            }
        }
    }
}
