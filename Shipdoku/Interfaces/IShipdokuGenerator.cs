using System;
using System.Collections.Generic;
using System.Text;
using Shipdoku.Models;

namespace Shipdoku.Interfaces
{
    public interface IShipdokuGenerator
    {
        ShipdokuModel GenerateShipdokuModel();
    }
}
