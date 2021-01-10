using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    class CroneVerificationException:DataAccessException
    {
        public CroneVerificationException():base(ExceptionMessages.BadCrones)
        {

        }
    }
}
