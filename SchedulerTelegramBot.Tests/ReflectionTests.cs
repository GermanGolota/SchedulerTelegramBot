using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using Xunit;
using WebAPI.Extensions;

namespace SchedulerTelegramBot.Tests
{
    public class ReflectionTests
    {
        [Fact]
        public void TypeShouldContainAssignedGenericType()
        {
            //Arrange
            Type commandType = typeof(StartCommand);
            Type controllerType = typeof(CommandController<>);
            //Act
            controllerType = controllerType.MakeGenericType(commandType);
            //Assert
            Assert.Contains(commandType, controllerType.GetGenericArguments());
        }
        //Fails with addition of commands
        [Fact]
        public void ShouldGetAllCommands()
        {
            //Arrange
            Assembly assembly = GetWEBAPIAssembly();
            Type command = typeof(ICommand);
            //Act
            List<Type> commands = assembly.GetTypesThatImplement(command).ToList();
            //Assert
            Assert.Equal(5, commands.Count);
            Assert.Contains(typeof(StartCommand), commands);
            Assert.Contains(typeof(SetupCommand), commands);
            Assert.Contains(typeof(DeleteScheduleCommand), commands);
            Assert.Contains(typeof(DeleteChatCommand), commands);
            Assert.Contains(typeof(CreateScheduleCommand), commands);
        }
        [Fact]
        public void ShouldGetMatcher()
        {
            //Arrange
            Type command = typeof(StartCommand);
            Type expected = typeof(IMatcher<StartCommand>);
            //Act
            Type actual = command.GetIMatcher();
            Type expectedGeneric = expected.GetGenericArguments().FirstOrDefault();
            Type actualGeneric = actual.GetGenericArguments().FirstOrDefault();
            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(actualGeneric, expectedGeneric);
        }
        [Fact]
        public void ShouldGetMatcherImplementation()
        {
            //Arrange
            Assembly assembly = GetWEBAPIAssembly();
            Type command = typeof(StartCommand);
            Type expected = typeof(StartCommandMatcher);
            //Act
            Type actual = assembly.GetMatcherImplementationFor(command);
            //Assert
            Assert.Equal(expected.Name, actual.Name);
        }
        [Fact]
        public void ShouldGetIMatcher()
        {
            //Arrange
            Type deleteChat = typeof(DeleteChatCommandMatcher);
            Type expected = typeof(IMatcher<DeleteChatCommand>);
            //Act
            List<Type> interfaces = deleteChat.GetInterfaces().ToList();
            //Assert
            Assert.Contains(expected, interfaces);
        }
        public Assembly GetWEBAPIAssembly()
        {
            Assembly assembly = typeof(StandardMessages).Assembly;
            return assembly;
        }
    }
}
