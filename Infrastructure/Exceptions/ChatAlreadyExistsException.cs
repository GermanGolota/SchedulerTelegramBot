using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    public class ChatAlreadyExistsException:DataAccessException
    {
        public ChatAlreadyExistsException(string message):base(message)
        {

        }
    }
}
