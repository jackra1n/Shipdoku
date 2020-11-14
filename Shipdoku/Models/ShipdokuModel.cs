using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Shipdoku.Enums;

namespace Shipdoku.Models
{
    public class ShipdokuModel : BindableBase
    {
        // private Fields
        private EShipdokuField[,] _shipdokuField;
        private int[] _horizontalCounts;
        private int[] _verticalCounts;

        /// <summary>
        /// Property für die verschiedenen Anzahlen von Schiffen den horizontalen Zeilen
        /// </summary>
        public int[] HorizontalCounts
        {
            get => _horizontalCounts;
            set => SetProperty(ref _horizontalCounts, value);
        }

        /// <summary>
        /// Property für die verschiedenen Anzahlen von Schiffen den vertikalen Zeilen
        /// </summary>
        public int[] VerticalCounts
        {
            get => _verticalCounts;
            set => SetProperty(ref _verticalCounts, value);
        }

        /// <summary>
        /// Stellt das Spielfeld mit Schiffen und Wasser dar
        /// </summary>
        public EShipdokuField[,] ShipdokuField
        {
            get => _shipdokuField;
            set => SetProperty(ref _shipdokuField, value);
        }

        /// <summary>
        /// Property mit einer möglichen Lösung des Rätels
        /// </summary>
        public EShipdokuField[,] SolvedShipdokuField { get; set; }
    }
}
