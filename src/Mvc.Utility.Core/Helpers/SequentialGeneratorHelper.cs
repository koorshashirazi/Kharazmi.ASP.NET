using System;
using System.Security.Cryptography;

namespace Mvc.Utility.Core.Helpers
{
    public enum SequentialGuidType
    {
        SequentialAsString,
        SequentialAsBinary,
        SequentialAtEnd
    }

    public static class SequentialGeneratorHelper
    {
        private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        public static Guid NewSequentialGuid(SequentialGuidType guidType = SequentialGuidType.SequentialAtEnd)
        {
            var randomBytes = new byte[10];
            _rng.GetBytes(randomBytes);

            var timestamp = DateTime.UtcNow.Ticks / 10000L;
            var timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian) Array.Reverse(timestampBytes);

            var guidBytes = new byte[16];

            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes);
        }

        public static int NewSequentialInteger()
        {
            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour)
                .AddMinutes(now.Minute)
                .AddSeconds(now.Second)
                .AddMilliseconds(now.Millisecond);
            var uniqueId = (int) (zeroDate.Ticks / 10000);

            return uniqueId;
        }
    }
}