using System;
using ShipdokuGUI.ViewModels;
using Xunit;

namespace ShipdokuGUI.Tests
{
    public class ShellViewModelTest
    {
        private readonly ShellViewModel _testee;

        public ShellViewModelTest()
        {
            _testee = new ShellViewModel();
        }

        [Fact]
        public void ShellViewModel_Title()
        {
            Assert.Equal("Shipdoku",_testee.Title);
        }
    }
}
