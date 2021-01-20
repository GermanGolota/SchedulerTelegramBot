using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public static class CommandNames
    {
        public readonly static string Setup = "setup";
        public readonly static string DeleteSchedule = "deleteSchedule";
        public readonly static string DeleteChat = "deleteChat";
        public readonly static string Start = "start";
        public readonly static string CreateSchedule = "createSchedule";
        public readonly static string AddAlerts = "addAlerts";
        public readonly static string GetSchedule = "getSchedule";
    }
}
