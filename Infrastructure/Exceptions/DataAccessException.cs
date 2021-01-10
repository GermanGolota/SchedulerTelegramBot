using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class DataAccessException:Exception
    {
        public DataAccessException(string message):base(message)
        {

        }
    }
}
