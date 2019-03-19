using System;

namespace CustomObjs
{
    public class NotEmptyString
    {
        readonly string value;

        private NotEmptyString() { }

        public static implicit operator string(NotEmptyString nes)
        {
            return nes.value;
        }

        private NotEmptyString(string value)
        {
            this.value = value;
        }

        public static implicit operator NotEmptyString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("invalid value");

            return new NotEmptyString(value);
        }
    }
}
