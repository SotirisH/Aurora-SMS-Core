using Aurora.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aurora.SMS.WebApi
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetCurrentUser()
        {
            return "WebAPI";
        }
    }
}
