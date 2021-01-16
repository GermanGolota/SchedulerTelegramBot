using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;
using Xunit;

namespace SchedulerTelegramBot.Tests
{
    public class ReflectionTests
    {
        [Fact]
        private void ShouldReturnAppropriateMatcher()
        {
            var expected = typeof(StartCommandMatcher);

            var actual =  GetMatcherType(typeof(StartCommand));

            Assert.Equal(expected.Name, actual.Name);
        }
        private Type GetMatcherType(Type commandType)
        {
            Assembly assembly = typeof(StandardMessages).Assembly;
            Type output = assembly.GetTypes()
                .Where(x => x.GetInterfaces()
                .Where(i => i.Name.Equals(typeof(IMatcher<>).Name) && i.GetGenericArguments().Contains(commandType)).Any())
                .FirstOrDefault();

            return output ?? throw new ArgumentException("No such command", nameof(commandType));
        }
        [Fact]
        public void TypeShouldContainAssignedGenericType()
        {
            Type commandType = typeof(StartCommand);
            Type controllerType = typeof(CommandController<>);
            controllerType = controllerType.MakeGenericType(commandType);

            Assert.Contains(commandType, controllerType.GetGenericArguments());
        }
    }
}
