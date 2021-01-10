using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public static class ExceptionMessages
    {
        public static string ChatExistsMessage { get;  } = "This chat is alredy registered";
        public static string ChatDontExistsMessage { get; } = "That chat is not yet in the system";
        public static string ScheduleAlreadyAttached { get; } = "This chat already has schedule attached to it";
        public static string BadCrones { get; } = "Some of the crones are not proper";
        public static string ScheduleDontExistsMessage { get; } = "This chat don't have schedule attached to it";
    }
}
