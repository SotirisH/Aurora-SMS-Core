using Aurora.Core.Data.Abstractions;

namespace Aurora.Core.Data
{
    public sealed class DefaultCurrentUserService:ICurrentUserService
    {
        public string GetCurrentUser()
        {
            return "<system>";
        }
    }
}
