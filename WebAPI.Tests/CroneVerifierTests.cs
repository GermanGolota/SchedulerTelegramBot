using Infrastructure.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class CroneVerifierTests
    {
        private ICroneVerifier _sut;
        public CroneVerifierTests()
        {
            _sut = new CroneVerifier();
        }
        [Fact]
        public void VerifyCron_ShouldFail_EmptyCron()
        {
            //Arrange
            string cron = "";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("-1 * * * *")]
        [InlineData("60 * * * *")]
        [InlineData("* -1 * * *")]
        [InlineData("* 24 * * *")]
        [InlineData("* * 0 * *")]
        [InlineData("* * 32 * *")]
        [InlineData("* * * 0 *")]
        [InlineData("* * * 13 *")]
        [InlineData("* * * * -1")]
        [InlineData("* * * * 7")]
        public void VerifyCron_ShouldFail_OutOfBoundsValue(string cron)
        {
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("* * * *")]
        [InlineData("60 * * *")]
        [InlineData("* * *")]
        [InlineData("* *")]
        [InlineData("*")]
        [InlineData("* 32 * *")]
        [InlineData("* * 0 *")]
        [InlineData("* 13 *")]
        [InlineData("* -1")]
        [InlineData("7")]
        public void VerifyCron_ShouldFail_MissingArguments(string cron)
        {
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void VerifyCron_ShouldWork_AllStarValue()
        {
            string cron = "* * * * *";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyCron_ShouldWork_ProperValue()
        {
            string cron = "5 0 * 8 *";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyCron_ShouldWork_HasRange()
        {
            string cron = "5-21 0 * 8 *";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyCron_ShouldWork_HasSeparator()
        {
            string cron = "1 0,7,8,9 * 8 *";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyCron_ShouldWork_HasSeparatorAndRange()
        {
            string cron = "51 0,5-21 * 8 *";
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("* * 4-2 * *")]
        [InlineData("53-48 * * *")]
        public void VerifyCron_ShouldFail_SecondValueOfRangeLessThenFirst(string cron)
        {
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("* * 4,4-2 * *")]
        [InlineData("50,52,53-48 * * *")]
        public void VerifyCron_ShouldFail_SecondValueOfRangeLessThenFirstIncludesSeparator(string cron)
        {
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData("51 5-21,23-22 * 8 *")]
        [InlineData("50,51-52,53-48 * * *")]
        public void VerifyCron_ShouldFail_SecondValueInSecondRangeLessThenFirstOne(string cron)
        {
            //Act
            bool result = _sut.VerifyCron(cron);
            //Assert
            Assert.False(result);
        }
    }
}
