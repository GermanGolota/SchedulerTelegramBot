using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    class ScheduleAlreadyAttachedException:DataAccessException
    {
        public ScheduleAlreadyAttachedException():base(ExceptionMessages.ScheduleAlreadyAttached)
        {

        }
    }
}
