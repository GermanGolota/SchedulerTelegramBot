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
    public class SetupCommandTests : CommandMatcherTestBase
    {
        private SetupCommand _sut;
        private string testingFileLocation = System.IO.Directory.GetCurrentDirectory() + @"\test.json";
        private readonly Mock<IJobManager> _jobMock = new Mock<IJobManager>();
        private readonly Mock<IMatcher<SetupCommand>> _matcherMock = new Mock<IMatcher<SetupCommand>>();
        private const string FileId = "123";
        public SetupCommandTests()
        {
            _sut = new SetupCommand(_matcherMock.Object, _clientMock.Object, _jobMock.Object,
                new LoggerMock<SetupCommand>());
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldNotSetupjobs_NoFile()
        {
            //Arrange
            Setup();

            Update update = GetUpdateWithNoFile();
            //Act
            _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertJobsNotBeenSet();
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldSetupjobs_FileProvided()
        {
            //Arrange
            Setup();

            Update update = GetUpdateWithFile();
            //Act
            _sut.ExecuteCommandIfMatched(update);
            //Assert
            AssertJobsBeenSet();
        }
        private void Setup()
        {
            SetupClient();
            SetupJobManager();
            SetupMatcherVaildCommand();
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
            _clientMock.Setup(x => x.DownloadFileFromId(It.IsAny<string>())).ReturnsAsync(testingFileLocation);
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

        private void SetupMatcherVaildCommand()
        {
            _matcherMock.Setup(x => x.IsMatching(It.IsAny<Update>())).ReturnsAsync(true);
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
                        Id = int.Parse(AdminId)
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
                        Id = int.Parse(AdminId)
                    }
                }
            };
        }
    }
}
