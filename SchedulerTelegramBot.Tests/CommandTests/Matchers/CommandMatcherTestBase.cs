using Xunit;

namespace SchedulerTelegramBot.Tests
{

    public class CommandMatcherTestBase: CommandTestBase
    {
        protected void AssertCommandMatched(bool actual)
        {
            Assert.True(actual);
        }
        protected void AssertCommandNotMatched(bool actual)
        {
            Assert.False(actual);
        }
    }
}
