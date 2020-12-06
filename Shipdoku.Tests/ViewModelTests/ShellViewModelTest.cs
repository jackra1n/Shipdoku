using System;
using Moq;
using Prism.Regions;
using Shipdoku.ViewModels;
using Xunit;

namespace Shipdoku.Tests.ViewModelTests
{
    public class ShellViewModelTest
    {
        private readonly ShellViewModel _testee;

        public ShellViewModelTest()
        {
            var regionManagerMock = new Mock<IRegionManager>();
            _testee = new ShellViewModel(regionManagerMock.Object);
        }

        [Fact]
        public void ShellViewModel_Title()
        {
            Assert.Equal("Shipdoku",_testee.Title);
        }
    }
}
