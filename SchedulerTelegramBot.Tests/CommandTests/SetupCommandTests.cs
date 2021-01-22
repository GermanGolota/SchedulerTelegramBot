using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Moq;
using Newtonsoft.Json;
using SchedulerTelegramBot.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class SetupCommandTests : CommandTestBase
    {
        private SetupCommand _sut;
        private string testingFileLocation = System.IO.Directory.GetCurrentDirectory() + @"\test.json";
        private readonly Mock<IJobManager> _jobMock = new Mock<IJobManager>();
        private readonly Mock<IUpdateManager> _updaterMock = new Mock<IUpdateManager>();
        private const string FileId = "123";
        public SetupCommandTests()
        {
            _sut = new SetupCommand(_clientMock.Object, _jobMock.Object,
                new LoggerMock<SetupCommand>(), _updaterMock.Object);
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldSetupjobs_FileProvided()
        {
            //Arrange
            Setup();

            Update update = GetUpdateWithFile();
            //Act
            await _sut.Execute(update);
            //Assert
            AssertJobsBeenSet();
        }
        private void Setup()
        {
            SetupClient();
            SetupJobManager();
        }
        private void SetupClient()
        {
            SetupMessageSendingMock();
            var testingSchedule = new ScheduleModel
            {
                Name = "Test",
                Alerts = new List<AlertModel>
                {
                    new AlertModel
                    {
                        Cron = "* * * * *",
                        Message = "Cron has been set"
                    }
                }
            };
            string fileContent = JsonConvert.SerializeObject(testingSchedule);
            if(System.IO.File.Exists(testingFileLocation))
            {
                System.IO.File.Delete(testingFileLocation);
            }
            using (FileStream file = new FileStream(testingFileLocation, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(fileContent);
                }
            }
            _updaterMock.Setup(x => x.GetFileContentsFrom(It.IsAny<Update>())).ReturnsAsync(fileContent);
        }
        private void SetupJobManager()
        {
            _jobMock.Setup(x => x.SetupJobsForChat(It.IsAny<ScheduleModel>(), It.IsAny<ChatId>()));
        }
        private void AssertJobsBeenSet()
        {
            _jobMock.Verify(x => x.SetupJobsForChat(It.IsAny<ScheduleModel>(), It.IsAny<ChatId>()), Times.Once);
        }
        private void AssertJobsNotBeenSet()
        {
            _jobMock.Verify(x => x.SetupJobsForChat(It.IsAny<ScheduleModel>(), It.IsAny<ChatId>()), Times.Never);
        }

        private Update GetUpdateWithFile()
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
                    },
                    Document = new Document
                    {
                        FileId = FileId
                    }
                }
            };
        }
        private Update GetUpdateWithNoFile()
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
    }
}
