using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;

namespace Shipdoku.ViewModels
{
    public class ShipdokuViewModel : BindableBase
    {
        private readonly IShipdokuGenerator _shipdokuGenerator;

        private bool _showSolution = false;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator)
        {
            _shipdokuGenerator = shipdokuGenerator;

            Playingfield = _shipdokuGenerator.GenerateShipdokuModel();
        }

        public ShipdokuModel Playingfield { get; set; }

        public EShipdokuField[,] Field => _showSolution ? Playingfield.ShipdokuField : Playingfield.ShipdokuField;
    }
}
