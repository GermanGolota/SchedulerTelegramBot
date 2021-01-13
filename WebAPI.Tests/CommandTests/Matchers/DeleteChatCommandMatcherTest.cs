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
using WebAPI.Jobs;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class DeleteChatCommandMatcherTest:CommandMatcherTestBase
    {
        public IMatcher<DeleteChatCommand> _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private readonly Mock<IJobManager> _jobManager = new Mock<IJobManager>();
        private readonly Mock<DeleteChatCommand> _commandMock = new Mock<DeleteChatCommand>();

        private const string FileId = "123";
        public DeleteChatCommandMatcherTest()
        {
            _sut = new DeleteChatCommandMatcher(_commandMock.Object, _repoMock.Object, _clientMock.Object);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldWork_CommandMatches()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            var actual = await _sut.IsMatching(update);
            //Assert
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_CommandDontMatch()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act
            var actual = await _sut.IsMatching(update);
            //Assert
            AssertCommandNotMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldWork_UserIsAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act
            var actual = await _sut.IsMatching(update);
            //Assert
            AssertCommandMatched(actual);
        }
        [Fact]
        public async Task ExecuteCommandAsync_ShouldFail_UserNotAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupRepoUserIsNotAdmin();

            Update update = GetUpdateWithMatchingCommand();

            //Act
            var actual = await _sut.IsMatching(update);
            //Assert
            //permission denied message
            AssertCommandNotMatched(actual);
        }
        private void SetupRepoUserIsAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns(AdminId);
        }
        private void SetupRepoUserIsNotAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns("not" + AdminId);
        }
       
        private Update GetUpdateWithMatchingCommand()
        {
            return new Update
            {
                Message = new Message
                {
                    Text = "/deleteChat",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(AdminId)
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
                    Text = "/notdeleteChat",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(AdminId)
                    }
                }
            };
        }
    }
}
