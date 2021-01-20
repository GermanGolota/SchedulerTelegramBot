using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public static class ExceptionMessages
    {
        public readonly static string ChatExistsMessage  = "This chat is alredy registered";
        public readonly static string ChatDontExistsMessage  = "That chat is not yet in the system";
        public readonly static string ScheduleAlreadyAttached = "This chat already has schedule attached to it";
        public readonly static string BadCrones = "Some of the crones are not proper";
        public readonly static string ScheduleDontExistsMessage = "This chat dones not a have schedule attached to it";
    }
}
