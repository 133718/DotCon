using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgoApi.Results
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class RuntimeResult : IResult
    {
        private RuntimeResult(Exception error, string reason)
        {
            Error = error;
            Reason = reason;
        }

        public Exception Error { get; }

        public string Reason { get; }

        public bool IsSuccess => Error == null;

        string IResult.ErrorReason => Reason;

        public override string ToString() => Reason ?? (IsSuccess ? "Successful" : "Unsuccessful");
        private string DebuggerDisplay => IsSuccess ? $"Success: {Reason ?? "No Reason"}" : $"{Error}: {Reason}";

        public static RuntimeResult FromSuccess()
            => new RuntimeResult(null, null);
        
        public static RuntimeResult FromSuccess(string reason)
            => new RuntimeResult(null, reason);

        public static RuntimeResult FromError(string reason)
            => new RuntimeResult(null, reason);

        public static RuntimeResult FromError(Exception ex)
            => new RuntimeResult(ex,  ex.Message);

        public static RuntimeResult FromError(IResult result)
            => new RuntimeResult(result.Error, result.ErrorReason);

    }
}
