using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public static class CommandNames
    {
        public static string Setup { get; } = "setup";
        public static string DeleteSchedule { get; } = "deleteSchedule";
        public static string DeleteChat { get; } = "deleteChat";
        public static string Start { get; } = "start";
        public static string CreateSchedule { get; } = "createSchedule";
    }
}
