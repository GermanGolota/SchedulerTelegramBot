using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public static class StandardMessages
    {
        public static string ChatRegistration { get; } = "Activated";
        public static string PermissionDenied { get; } = "You don't have permission to do that";
        public static string NoFileAttached { get; } = "Please attach a file";
        public static string ScheduleSetSuccess { get; } = "Schedule have been succesfully set";
        public static string ScheduleDeletionSuccess { get; } = "Succesfully deleted schedule";
        public static string ChatDeletionSuccess { get; } = "Successfully deleted chat";
        public static string BadFileData { get; } = "Data in the file is not valid";
        public static string AddedAlertsSuccess { get; } = "Successfully added alerts";
    }
}
