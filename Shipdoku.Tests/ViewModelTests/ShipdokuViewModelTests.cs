using Moq;
using Prism.Regions;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;
using Shipdoku.ViewModels;
using System;
using Xunit;

namespace Shipdoku.Tests.ViewModelTests
{
    public class ShipdokuViewModelTests
    {
        private readonly ShipdokuViewModel _testee;
        private readonly Mock<IShipdokuGenerator> _shipdokuGeneratorMock;
        private readonly Mock<IExportService> _exportServiceMock;
        private readonly Mock<IRegionManager> _regionManagerMock;
        private readonly Mock<IRegionNavigationService> _regionNavigationServiceMock;

        public ShipdokuViewModelTests()
        {
            _shipdokuGeneratorMock = new Mock<IShipdokuGenerator>();
            _exportServiceMock = new Mock<IExportService>();
            _regionManagerMock = new Mock<IRegionManager>();
            _regionNavigationServiceMock = new Mock<IRegionNavigationService>();

            _testee = new ShipdokuViewModel(_shipdokuGeneratorMock.Object, _exportServiceMock.Object, _regionManagerMock.Object);
        }

        [Fact]
        public void OnNavigatedTo_CreatesNewPuzzle()
        {
            // Arrange
            var parameters = new NavigationParameters();
            var navigationContext = new NavigationContext(_regionNavigationServiceMock.Object, new Uri("Shipdoku", UriKind.Relative), parameters);
            navigationContext.Parameters.Add("createEmpty", false);
            var shipdokuModel = new ShipdokuModel();
            _shipdokuGeneratorMock.Setup(s => s.GenerateShipdokuModel()).Returns(shipdokuModel);

            // Act
            _testee.OnNavigatedTo(navigationContext);

            // Assert
            Assert.True(_testee.CanShowSolution);
            Assert.False(_testee.ShowSolution);
            Assert.False(_testee.IsCreateEmpty);
            Assert.Equal(shipdokuModel, _testee.ShipdokuModel);
            Assert.Equal(shipdokuModel.ShipdokuField, _testee.PlayingField);
            _shipdokuGeneratorMock.Verify(s => s.GenerateShipdokuModel(), Times.Once);
        }

        [Fact]
        public void OnNavigatedTo_CreatesEmptyPuzzle()
        {
            // Arrange
            var parameters = new NavigationParameters();
            var navigationContext = new NavigationContext(_regionNavigationServiceMock.Object, new Uri("Shipdoku", UriKind.Relative), parameters);
            navigationContext.Parameters.Add("createEmpty", true);
            var shipdokuModel = new ShipdokuModel();
            _shipdokuGeneratorMock.Setup(s => s.GenerateShipdokuModel());
            _testee.ShowSolution = true;

            // Act
            _testee.OnNavigatedTo(navigationContext);

            // Assert
            Assert.False(_testee.CanShowSolution);
            Assert.False(_testee.ShowSolution);
            Assert.True(_testee.IsCreateEmpty);
            _shipdokuGeneratorMock.Verify(s => s.GenerateShipdokuModel(), Times.Never);
        }

        [Fact]
        public void IsNavigationTarget_ReturnsTrue()
        {
            // Act
            var result = _testee.IsNavigationTarget(null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNavigationTarget_NoException()
        {
            // Act
            _testee.OnNavigatedFrom(null);
        }

        [Fact]
        public void StartMenuNavigateCommand_NavigateToStartMenu()
        {
            // Arrange
            _regionManagerMock.Setup(m => m.RequestNavigate("ContentRegion", "StartMenu"));

            // Act
            _testee.StartMenuNavigateCommand.Execute();
            
            // Assert
            _regionManagerMock.Verify(m => m.RequestNavigate("ContentRegion", "StartMenu"), Times.Once);
            _regionManagerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ExportCommand_ExportServiceIsCalled()
        {
            // Arrange
            var shipdokuModel = new ShipdokuModel();
            _testee.ShipdokuModel = shipdokuModel;
            _exportServiceMock.Setup(e => e.ExportPlayingFieldToPng(shipdokuModel, false));

            // Act
            _testee.ExportCommand.Execute();

            // Assert
            _exportServiceMock.Verify(e => e.ExportPlayingFieldToPng(shipdokuModel, false), Times.Once);
        }

        [Fact]
        public void GenerateNewFieldCommand_NewFieldGenerated()
        {
            // Act
            _testee.GenerateNewFieldCommand.Execute();

            // Assert
            Assert.True(_testee.CanShowSolution);
            Assert.False(_testee.IsCreateEmpty);
        }

        [Fact]
        public void SetFieldCommand_SetsRightField()
        {
            // Arrange
            int x = 3;
            int y = 4;
            string parameter = $"{x},{y}";
            _testee.SetCurrentFieldTypeCommand.Execute("ShipMiddle");

            // Act

            _testee.SetFieldCommand.Execute(parameter);

            // Assert
            Assert.Equal(EShipdokuField.ShipMiddle, _testee.PlayingField[x,y]);
        }

        [Fact]
        public void SetCurrentFieldType_SetCorrectFieldType()
        {
            // Arrange
            var shipdokuField = EShipdokuField.ShipDown;

            // Act
            _testee.SetCurrentFieldTypeCommand.Execute(shipdokuField.ToString());

            // Assert
            Assert.Equal(shipdokuField, _testee.CurrentFieldType);
        }
    }
}
