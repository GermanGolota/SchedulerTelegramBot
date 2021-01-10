using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class ScheduleDontExistException:DataAccessException
    {
        public ScheduleDontExistException():base(ExceptionMessages.ScheduleDontExistsMessage)
        {

        }
    }
}
