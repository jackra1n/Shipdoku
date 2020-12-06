using System;
using System.Collections.Generic;
using Shipdoku.ViewModels;
using System.Text;
using Moq;
using Prism.Regions;
using Shipdoku.Interfaces;

namespace Shipdoku.Tests.ViewModelTests
{
    public class ShipdokuViewModelTests
    {
        private ShipdokuViewModel _testee;
        private readonly Mock<IShipdokuGenerator> _shipdokuGeneratorMock;
        private readonly Mock<IExportService> _exportServiceMock;
        private readonly Mock<IRegionManager> _regionManagerMock;

        public ShipdokuViewModelTests()
        {
            _shipdokuGeneratorMock = new Mock<IShipdokuGenerator>();
            _exportServiceMock = new Mock<IExportService>();
            _regionManagerMock = new Mock<IRegionManager>();

            _testee = new ShipdokuViewModel(_shipdokuGeneratorMock.Object, _exportServiceMock.Object, _regionManagerMock.Object);
        }
    }
}
