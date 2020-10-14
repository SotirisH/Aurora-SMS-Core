using System;
using Aurora.Core.Data;

namespace Aurora.Insurance.WebAPI
{
    [Obsolete("The user name will be resolved by the JWT")]
    public class CurrentUserService : ICurrentUserService
    {
        public string GetCurrentUser()
        {
            var userName = "Anonymous";
            return userName;
        }
    }
}
