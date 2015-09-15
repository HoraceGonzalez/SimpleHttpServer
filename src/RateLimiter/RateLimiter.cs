using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
    public class RateLimiter
    {
        public bool IsValidRequest(string ip, DateTime timestamp)
        {
            return timestamp.Ticks % 2 == 0;
        }
    }
}
