using System;

namespace SgoApi.Results
{

    public interface IResult
    {
        Exception Error { get; }

        string ErrorReason { get; }

        bool IsSuccess { get; }
    }
}
