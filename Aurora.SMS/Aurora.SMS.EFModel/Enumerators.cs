using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.SMS.EFModel.Enumerators
{
    public enum MessageStatus
    {
        Pending,
        Delivered,
        Skipped,
        Error
    }
}
