using System;

namespace cdio.net
{
    public class CdioDeviceException : Exception
    {
        public CdioDeviceException(long value, string errorMessage)
            : base($"{value} {errorMessage}")
        {
            Value = value;
            ErrorMessage = errorMessage;
        }

        public long Value { get; }
        public string ErrorMessage { get; }
    }
}
