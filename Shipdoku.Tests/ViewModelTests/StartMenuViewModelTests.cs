using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Prism.Regions;
using Shipdoku.ViewModels;
using Xunit;

namespace Shipdoku.Tests.ViewModelTests
{
    public class StartMenuViewModelTests
    {
        private readonly StartMenuViewModel _testee;
        private readonly Mock<IRegionManager> _regionManagerMock;

        public StartMenuViewModelTests()
        {
            _regionManagerMock = new Mock<IRegionManager>();

            _testee = new StartMenuViewModel(_regionManagerMock.Object);
        }

        [Fact]
        public void NavigateCommand_NavigationIsCalled()
        {
            // Arrange
            _regionManagerMock.Setup(r =>
                r.RequestNavigate("ContentRegion", "Shipdoku", It.IsAny<NavigationParameters>()));

            // Act
            _testee.NavigateCommand.Execute("true");

            // Assert
            _regionManagerMock.Verify(r => r.RequestNavigate("ContentRegion", "Shipdoku", It.IsAny<NavigationParameters>()));
        }
    }
}
