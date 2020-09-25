using System;
using System.Collections.Generic;
using System.Text;
using Shipdoku.Interfaces;
using Shipdoku.Models;

namespace Shipdoku.ViewModels
{
    public class ShipdokuViewModel
    {
        private readonly IShipdokuGenerator _shipdokuGenerator;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator)
        {
            _shipdokuGenerator = shipdokuGenerator;
        }

        public ShipdokuModel Playingfield { get; set; }
    }
}
