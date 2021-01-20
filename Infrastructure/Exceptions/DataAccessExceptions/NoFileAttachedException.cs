using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class NoFileAttachedException:DataAccessException
    {
        public NoFileAttachedException():base(ExceptionMessages.NoFileMessage)
        {

        }
    }
}
