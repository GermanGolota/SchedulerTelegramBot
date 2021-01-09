using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IChatRepo
    {
        string GetAdminIdOfChat(string ChatId);
    }
}
