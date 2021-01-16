using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Commands;
using WebAPI.Commands.Verifiers;

namespace WebAPI.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypesThatImplement(this Assembly assembly, Type interfaceType)
        { 
            List<Type> commands = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(interfaceType)).ToList();
            return commands;
        }
        public static Type GetIMatcherFor(this Assembly assembly, Type command)
        {
            Type output = typeof(IMatcher<>);
            output = output.MakeGenericType(command);
            return output;
        }
        public static Type GetMatcherImplementationFor(this Assembly assembly, Type command)
        {
            Type output = assembly.GetTypes()
               .Where(x => x.GetInterfaces()
                   .Where(i => i.Name.Equals(typeof(IMatcher<>).Name) && i.GetGenericArguments().Contains(command)).Any())
               .FirstOrDefault();
            return output ;
        }
        private static IEnumerable<Type> InheritsFrom(this IEnumerable<Type> types, Type interfaceType)
        {
            return types.Where(x => x.GetInterfaces().Contains(interfaceType));
        }
    }
}
