using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions.DataAccessExceptions
{
    public class ScheduleDontExistException:DataAccessException
    {
        public ScheduleDontExistException():base(ExceptionMessages.ScheduleDontExistsMessage)
        {

        }
    }
}
