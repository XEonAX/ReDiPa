using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReDiPa.Server.Models
{
    public enum PackTaskState
    {
        Created,
        Pending,
        Hashing,
        Packing,
        Packed,
        Paused,
        Cancelled,
        Error
    }
}
