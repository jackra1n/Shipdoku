using Shipdoku.Converters;
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

            for (int i = 0; i < (8*8); i++)
            {
                var playingfieldbutton = new Button();
                //playingfieldbutton.Command = new Binding("");
                //playingfieldbutton.CommandParameter = "1,1";
                //var image = new Image();
                //var binding = new Binding();
                //binding.Path = new Path("Playingfield.ShipdokuField[1,1]");
                //binding.Converter = imageConverter;
                //image.Source = new Binding("playingfield[1,1] conv")
                //playingfieldbutton.Content = new Image();
                playingfieldbutton.Content = i;
                //PlayingFieldGrid.
                //PlayingFieldGrid.Children.Insert(i,playingfieldbutton);
                Grid.SetRow(playingfieldbutton, i);
            }
        }
    }
}
