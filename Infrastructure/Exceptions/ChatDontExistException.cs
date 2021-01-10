using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Exceptions
{
    class ChatDontExistException:DataAccessException
    {
        public ChatDontExistException():base(ExceptionMessages.ChatDontExistsMessage)
        {

        }
    }
}
