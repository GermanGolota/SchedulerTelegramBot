using Moq;
using SchedulerTelegramBot.Tests.Mocks;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using WebAPI.Jobs;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class DeleteScheduleCommandTests:CommandTestBase
    {
        private DeleteScheduleCommand _sut;
        private readonly Mock<IJobManager> _jobMock = new Mock<IJobManager>();
        public DeleteScheduleCommandTests()
        {
            _sut = new DeleteScheduleCommand( _clientMock.Object,
                _jobMock.Object, new LoggerMock<DeleteScheduleCommand>());
        }
        [Fact]
        public async Task ExecuteCommandIfMatched_ShouldSetupJobs()
        {
            //Arrange
            SetupMessageSendingMock();
            _jobMock.Setup(x => x.DeleteJobsFromChat(It.IsAny<ChatId>()));


            Update update = GetUpdate();
            //Act
            await _sut.Execute(update);
            //Assert
            AssertJobBeenPerformed();
        }

        private void AssertJobBeenPerformed()
        {
            _jobMock.Verify(x => x.DeleteJobsFromChat(It.IsAny<ChatId>()), Times.Once);
        }

        private Update GetUpdate()
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
    }
}
