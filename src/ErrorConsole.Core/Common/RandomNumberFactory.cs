using System;

namespace ErrorConsole.Core.Common
{
    public static class RandomNumberFactory
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int Create(int min = 0, int max = 30)
        {
            lock (syncLock)
                return random.Next(min, max);
        }
    }
}
