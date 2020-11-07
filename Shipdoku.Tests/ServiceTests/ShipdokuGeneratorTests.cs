using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipdoku.Enums;
using Shipdoku.Services;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Shipdoku.Tests.ServiceTests
{
    public class ShipdokuGeneratorTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ShipdokuGenerator _testee;

        public ShipdokuGeneratorTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _testee = new ShipdokuGenerator();
        }

        [Fact]
        public void TestTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = _testee.GenerateShipdokuModel();
            sw.Stop();
            _logger.WriteLine(sw.ElapsedMilliseconds.ToString());

            Assert.NotNull(result);
            Assert.Equal(20, result.HorizontalCounts.Sum(c => c));
        }
    }
}
