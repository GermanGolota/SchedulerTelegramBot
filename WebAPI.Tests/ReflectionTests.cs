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
            Type commandType = typeof(StartCommand);
            Type controllerType = typeof(CommandController<>);
            controllerType = controllerType.MakeGenericType(commandType);

            Assert.Contains(commandType, controllerType.GetGenericArguments());
        }
        [Fact]
        public void ShouldGetAllCommands()
        {
            Assembly assembly = GetWEBAPIAssembly();
            Type command = typeof(ICommand);
            List<Type> commands = assembly.GetTypesThatImplement(command).ToList();

            Assert.Equal(4, commands.Count);
            Assert.Contains(typeof(StartCommand), commands);
            Assert.Contains(typeof(SetupCommand), commands);
            Assert.Contains(typeof(DeleteScheduleCommand), commands);
            Assert.Contains(typeof(DeleteChatCommand), commands);
        }
        [Fact]
        public void ShouldGetMatcher()
        {
            Assembly assembly = GetWEBAPIAssembly();

            Type command = typeof(StartCommand);
            Type expected = typeof(IMatcher<StartCommand>);

            Type actual = assembly.GetIMatcherFor(command);

            Assert.Equal(expected.Name, actual.Name);
            Type expectedGeneric = expected.GetGenericArguments().FirstOrDefault();
            Type actualGeneric = actual.GetGenericArguments().FirstOrDefault();

            Assert.Equal(actualGeneric, expectedGeneric);

        }
        [Fact]
        public void ShouldGetMatcherImplementation()
        {
            Assembly assembly = GetWEBAPIAssembly();
            Type command = typeof(StartCommand);
            Type expected = typeof(StartCommandMatcher);

            Type actual = assembly.GetMatcherImplementationFor(command);

            Assert.Equal(expected.Name, actual.Name);
        }
        public Assembly GetWEBAPIAssembly()
        {
            Assembly assembly = typeof(StandardMessages).Assembly;
            return assembly;
        }
    }
}
