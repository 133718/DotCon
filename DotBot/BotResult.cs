using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotBot
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class BotResult
    {
        public Exception Exception { get; private set; }
        public string Reason { get; private set; }

        public static BotResult Success(string reason = "") => new() { Reason = reason };
        public static BotResult Error(Exception ex, string reason) => new() { Reason = reason, Exception = ex};
        public static BotResult Error(string reason) => new() { Reason = reason };

        public bool IsSuccess => Exception == null;

        public override string ToString() => Reason ?? (IsSuccess ? "Successful" : "Unsuccessful");
        private string DebuggerDisplay => IsSuccess ? $"Success: {Reason ?? "No Reason"}" : $"{Exception}: {Reason}";
    }
}
