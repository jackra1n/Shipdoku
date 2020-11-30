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
                    //var binding = new Binding();
                    //binding.Source = $"Field[{x}, {y}]";
                    //binding.Path = new PropertyPath($"Field[{x}, {y}]");
                    //binding.Converter = imageConverter;

                    Binding array = new Binding("PlayingField");
                    Binding xBinding = new Binding();
                    xBinding.Source = x;
                    Binding yBinding = new Binding();
                    yBinding.Source = y;

                    MultiBinding multiBinding = new MultiBinding();
                    multiBinding.Bindings.Add(array);
                    multiBinding.Bindings.Add(xBinding);
                    multiBinding.Bindings.Add(yBinding);

                    multiBinding.Converter = arrayMultiConverter;

                    var image = new Image();
                    image.Width = 50;
                    image.Height = 50;

                    image.SetBinding(Image.SourceProperty, multiBinding);

                    playingFieldbutton.Content = image;

                    stackPanel.Children.Add(playingFieldbutton);
                }


                PlayingFieldPanel.Children.Add(stackPanel);
                    //image.Source = new Binding("playingfield[1,1] conv");
                //playingfieldbutton.Content = new Image();

            }
        }
    }
}
