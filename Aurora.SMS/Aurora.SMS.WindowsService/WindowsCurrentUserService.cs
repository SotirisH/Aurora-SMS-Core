using Aurora.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aurora.SMS.WindowsService
{
    /// <summary>
    /// Get the current windows user that the service runs under
    /// </summary>
    public class WindowsCurrentUserService : ICurrentUserService
    {
 
        public string GetCurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }
}
