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
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Jobs;
using Xunit;

namespace SchedulerTelegramBot.Tests.CommandTests.Matchers
{
    public class StartCommandMatcherTests:CommandMatcherTestBase
    {
        public IMatcher<StartCommand> _sut;
        public StartCommandMatcherTests()
        {
            
            _sut = new StartCommandMatcher();
        }
        [Fact]
        public async Task IsMatching_CommandMatches_ValidUpdate()
        {
            //Arrange
            SetupMessageSendingMock();

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
                    Text = "/start",
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
                    Text = "/notStart",
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
