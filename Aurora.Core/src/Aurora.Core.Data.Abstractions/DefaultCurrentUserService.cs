using System;

namespace Aurora.Core.Data.Abstractions
{
    public sealed class DefaultCurrentUserService:ICurrentUserService
    {
        public string GetCurrentUser()
        {
            return Guid.Empty.ToString();
        }
    }
}
