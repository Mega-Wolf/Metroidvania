using System;

namespace UnityEngine.Experimental.Input.Utilities
{
    internal static class ExceptionHelpers
    {
        public static bool IsExceptionIndicatingBugInCode(this Exception exception)
        {
            Debug.Assert(exception != null, "Exception is null");

            return exception is NullReferenceException ||
                exception is IndexOutOfRangeException ||
                exception is ArgumentException;
        }
    }
}
