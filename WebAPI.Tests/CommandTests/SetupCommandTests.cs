using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Moq;
using SchedulerTelegramBot.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using WebAPI.Jobs;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class SetupCommandTests : CommandMatcherTestBase
    {
        private SetupCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private readonly Mock<IJobManager> _jobManager = new Mock<IJobManager>();
        private readonly Mock<IMatcher<SetupCommand>> _matcherMock = new Mock<IMatcher<SetupCommand>>();
        private const string FileId = "123";
        public SetupCommandTests()
        {
            _sut = new SetupCommand(_matcherMock.Object, _clientMock.Object, _jobManager.Object,
                new LoggerMock<SetupCommand>());
        }
    }
}
