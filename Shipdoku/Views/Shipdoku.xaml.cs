using Shipdoku.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

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

            InitializePlayingField();
        }

        private void InitializePlayingField()
        {
            var arrayMultiConverter = new MultiDimensionalCoverter();
            var createEmptyBinding = new Binding("IsCreateEmpty");

            for (int row = 0; row < 9; row++)
            {
                StackPanel gameFieldStackPanel = new StackPanel
                {
                    Name = "panelRow" + row,
                    Orientation = Orientation.Horizontal
                };
                for (int column = 0; column < 9; column++)
                {
                    if (column < 8 && row != 8)
                    {
                        Binding gameFieldArrayBinding = new Binding("PlayingField");
                        Binding xBinding = new Binding {Source = column};
                        Binding yBinding = new Binding {Source = row};

                        MultiBinding multiBinding = new MultiBinding();
                        multiBinding.Bindings.Add(gameFieldArrayBinding);
                        multiBinding.Bindings.Add(yBinding);
                        multiBinding.Bindings.Add(xBinding);
                        multiBinding.Converter = arrayMultiConverter;

                        var btnImage = new Image();
                        btnImage.SetBinding(Image.SourceProperty, multiBinding);

                        var playingFieldbutton = new Button
                        {
                            Width = 50,
                            Height = 50,
                            Background = Brushes.White,
                            Content = btnImage
                        };

                        Binding fieldClickBinding = new Binding($"SetFieldCommand");
                        playingFieldbutton.CommandParameter = $"{row},{column}";
                        playingFieldbutton.SetBinding(ButtonBase.CommandProperty, fieldClickBinding);

                        gameFieldStackPanel.Children.Add(playingFieldbutton);
                    }
                    else if (row == 8 ^ column == 8)
                    {
                        var shipCountTextBox = new TextBox
                        {
                            VerticalContentAlignment = VerticalAlignment.Center,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            FontSize = 18,
                            Width = 50,
                            Height = 50
                        };
                        shipCountTextBox.SetBinding(IsEnabledProperty, createEmptyBinding);

                        Binding countBinding = null;

                        if (row == 8 && column != 8)
                            countBinding = new Binding($"ShipdokuModel.HorizontalCounts[{column}]");
                        else if (row != 8)
                            countBinding = new Binding($"ShipdokuModel.VerticalCounts[{row}]");

                        shipCountTextBox.SetBinding(TextBox.TextProperty, countBinding!);
                        gameFieldStackPanel.Children.Add(shipCountTextBox);
                    }
                }

                PlayingFieldPanel.Children.Add(gameFieldStackPanel);
            }
        }
    }
}
