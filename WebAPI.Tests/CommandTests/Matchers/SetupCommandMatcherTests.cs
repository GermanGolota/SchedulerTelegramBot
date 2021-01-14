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
    public class SetupCommandMatcherTests : CommandMatcherTestBase
    {
        public IMatcher<SetupCommand> _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();

        private const string FileId = "123";
        public SetupCommandMatcherTests()
        {
            _sut = new SetupCommandMatcher(_repoMock.Object, _clientMock.Object);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            bool actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandMatched(actual);
        }
        private void SetupUserIsAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns(UserId);
        }
        private void SetupUserIsNotAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns("Not" + UserId);
        }
        [Fact]
        public async Task IsMatching_CommandNotMatches_NotValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupUserIsAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            bool actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandNotMatched(actual);
        }
        [Fact]
        public async Task IsMatching_ShouldFail_UserNotAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupUserIsNotAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandNotMatched(actual);
        }
        private Update GetUpdateWithMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Caption = "/setup",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(UserId)
                    }
                }
            };
        }
        private Update GetUpdateWithNotMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Caption = "/notSetup",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(UserId)
                    },
                    Document = new Document
                    {
                        FileId = FileId
                    }
                }
            };
        }
    }
}
