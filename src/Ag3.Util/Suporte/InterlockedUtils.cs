using System;
using System.Threading;

namespace Ag3.Util
{
    public static class InterlockedUtils
    {
        public static void ThreadSafeReplacement<TInstance>(ref TInstance current, Func<TInstance> newInstanceProvider)
            where TInstance: class

        {
            TInstance snapshot, newCache;
            do
            {
                snapshot = current;
                newCache = newInstanceProvider();

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref current, newCache, snapshot), snapshot));
        }

        public static void ThreadSafeReplacement(ref long current, long newValue)

        {
            long snapshot, newCache;
            do
            {
                snapshot = current;
                newCache = newValue;

            } while (Interlocked.CompareExchange(ref current, newCache, snapshot) != snapshot);
        }

    }
}