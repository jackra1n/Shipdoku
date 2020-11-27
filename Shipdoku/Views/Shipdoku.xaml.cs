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

            int buttonNumber = 0;
            for (int y = 0; y < 8; y++)
            {
                StackPanel stackPanel = new StackPanel
                {
                    Name = "panelRow" + y,
                    Orientation = Orientation.Horizontal
                };
                for (int x = 0; x < 8; x++)
                {
                    var playingFieldbutton = new Button();
                    //playingfieldbutton.Command = new Binding("");
                    //playingfieldbutton.CommandParameter = "1,1";
                    playingFieldbutton.Width = 50;
                    playingFieldbutton.Height = 50;
                    var binding = new Binding();
                    binding.Source = $"Field[{x}, {y}]";
                    //binding.Path = new PropertyPath($"Field[{x}, {y}]");
                    //binding.Converter = imageConverter;
                    var image = new Image();
                    playingFieldbutton.SetBinding(Button.ContentProperty, $"Field[{x}, {y}]");
                    stackPanel.Children.Add(playingFieldbutton);
                    buttonNumber++;
                }
                PlayingFieldPanel.Children.Add(stackPanel);
                    //image.Source = new Binding("playingfield[1,1] conv");
                //playingfieldbutton.Content = new Image();

            }
        }
    }
}
