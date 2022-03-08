using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotBot
{
    public class BotResult
    {
        public Exception Exception { get; private set; }
        public string Reason { get; private set; }

        public static BotResult Success(string reason = "") => new BotResult() { Reason = reason };
        public static BotResult Error(Exception ex, string reason = "") => new BotResult() { Reason = reason, Exception = ex};
    }
}
