using System;
using System.Collections.Generic;
using Shipdoku.ViewModels;
using System.Text;
using Moq;
using Shipdoku.Interfaces;

namespace Shipdoku.Tests.ViewModelTests
{
    public class ShipdokuViewModelTests
    {
        private ShipdokuViewModel _testee;
        private readonly Mock<IShipdokuGenerator> _shipdokuGeneratorMock;
        private readonly Mock<IExportService> _exportServiceMock;

        public ShipdokuViewModelTests()
        {
            _shipdokuGeneratorMock = new Mock<IShipdokuGenerator>();
            _exportServiceMock = new Mock<IExportService>();

            _testee = new ShipdokuViewModel(_shipdokuGeneratorMock.Object, _exportServiceMock.Object);
        }
    }
}
