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
using WebAPI.Jobs;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class SetupCommandTests : CommandTestBase
    {
        private SetupCommand _sut;
        private readonly Mock<IChatRepo> _repoMock = new Mock<IChatRepo>();
        private readonly Mock<IJobManager> _jobManager = new Mock<IJobManager>();
        private const string FileId = "123";
        public SetupCommandTests()
        {
            _sut = new SetupCommand(_repoMock.Object, _clientMock.Object, _jobManager.Object,
                new LoggerMock<SetupCommand>());
        }

        [Fact]
        public async Task ExecuteCommandIfMatched_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupSuccessfullFileDownload();

            SetupSuccessfullJobInstalation();

            SetupUserIsAdmin();

            Update update = GetUpdateWithMatchingCommand();
            //Act

            var actual = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            AssertCommandMatched(actual);
        }
        private void SetupUserIsAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns(AdminId);
        }
        private void SetupUserIsNotAdmin()
        {
            _repoMock.Setup(x => x.GetAdminIdOfChat(It.IsAny<string>())).Returns("Not" + AdminId);
        }
        private void SetupSuccessfullFileDownload()
        {
            _clientMock.Setup(x => x.DownloadFileFromId(It.IsAny<string>()));
        }
        private void SetupSuccessfullJobInstalation()
        {
            _jobManager.Setup(x => x.SetupJobsForChat(It.IsAny<ScheduleModel>(), It.IsAny<ChatId>()));
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_CommandNotMatches_NotValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupSuccessfullFileDownload();

            SetupSuccessfullJobInstalation();

            SetupUserIsAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var replie = await _sut.ExecuteCommandIfMatched(update);
            //Assert    
            Assert.Equal(CommandMatchResult.NotMatching, replie);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldFail_UserNotAdmin()
        {
            //Arrange
            SetupMessageSendingMock();

            SetupSuccessfullFileDownload();

            SetupSuccessfullJobInstalation();

            SetupUserIsNotAdmin();

            Update update = GetUpdateWithNotMatchingCommand();
            //Act

            var actual = await _sut.ExecuteCommandIfMatched(update);
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
                    Caption = "/notSetup",
                    Chat = new Chat
                    {
                        Id = long.Parse(TestChatId)
                    },
                    From = new User
                    {
                        Id = int.Parse(AdminId)
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
