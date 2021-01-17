using Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using Xunit;

namespace SchedulerTelegramBot.Tests.CommandTests.Matchers
{
    public class DeleteScheduleMatcherTests : CommandMatcherTestBase
    {
        private IMatcher<DeleteScheduleCommand> _sut;
        private Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        public DeleteScheduleMatcherTests()
        {
            _sut = new DeleteScheduleCommandMatcher(_repoMock.Object, _clientMock.Object);
        }
        [Fact]
        public async Task IsMatching_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();
            SetupUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            var actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task IsMatching_CommandNotMatches_NotValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();
            SetupUserIsAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandNotMatched(actual);
        }
        [Fact]
        public async Task IsMatching_CommandNotMatches_UserNotAdmin()
        {
            //Arrange
            SetupMessageSendingMock();
            SetupUserIsNotAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            var actual = await _sut.IsMatching(update);
            //Assert    
            AssertCommandNotMatched(actual);
        }


        public void SetupUserIsAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns(UserId);
        }
        public void SetupUserIsNotAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns("Not"+UserId);
        }
        private Update GetUpdateWithMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Text = "/deleteSchedule",
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
                    Text = "/not",
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
    }
}
