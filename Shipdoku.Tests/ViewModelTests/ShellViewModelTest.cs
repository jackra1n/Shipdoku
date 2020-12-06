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
            _testee = new ShellViewModel();
        }

        [Fact]
        public void ShellViewModel_Title()
        {
            Assert.Equal("Shipdoku",_testee.Title);
        }
    }
}
