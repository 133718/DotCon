using System;
using System.Diagnostics;

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
            => new(null, null);
        
        public static RuntimeResult FromSuccess(string reason)
            => new(null, reason);

        public static RuntimeResult FromError(string reason)
            => new(null, reason);

        public static RuntimeResult FromError(Exception ex)
            => new(ex,  ex.Message);

        public static RuntimeResult FromError(IResult result)
            => new(result.Error, result.ErrorReason);

    }
}
