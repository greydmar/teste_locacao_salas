using System;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace locacao.tests.Internal
{
    public static class ExceptionUtils
    {
        public static bool HasUniqueItem<TException>(this AggregateException source)
            where TException : Exception
        {
            return source.InnerExceptions.Count == 1
                   && source.InnerExceptions[0] is TException;
        }

        public static bool IsOrWraps<TException>(this Exception source)
            where TException: Exception
        {
            if (source is AggregateException exception)
                return HasUniqueItem<TException>(exception);

            return source is TException 
                   || (source.GetBaseException()!=null && source.GetBaseException() is TException);
        }

        public static bool IsOrWraps(this Exception source, params Type[] exceptionTypes)
        {
            if (exceptionTypes == null || exceptionTypes.Length == 0)
                return false;

            Exception exceptionToTest = source;
            if (source is AggregateException exception)
            {
                exceptionToTest = exception.InnerExceptions.Count == 1
                    ? exception.InnerExceptions[0]
                    : null;
            }

            if (exceptionToTest == null)
                return false;

            var typeofEx = exceptionToTest.GetType();

            return exceptionTypes.Any(t => t == typeofEx
                                           || t.IsAssignableFrom(typeofEx)
                                           || typeofEx.IsAssignableFrom(t));
        }

        /// <summary>
        /// Rethrows the extended <see cref="Exception"/>, <paramref name="exceptionPossiblyToThrow"/>, using the
        /// <see cref="ExceptionDispatchInfo"/> class to rethrow it with its original stack trace, if
        /// <paramref name="exceptionPossiblyToThrow"/> differs from <paramref name="exceptionToCompare"/>.
        /// </summary>
        /// <param name="exceptionPossiblyToThrow">The exception to throw, if it differs from <paramref name="exceptionToCompare"/></param>
        /// <param name="exceptionToCompare">The exception to compare against.</param>
        public static void RethrowWithOriginalStackTraceIfDiffersFrom(this Exception exceptionPossiblyToThrow, Exception exceptionToCompare)
        {
            if (exceptionPossiblyToThrow != exceptionToCompare)
            {
                ExceptionDispatchInfo.Capture(exceptionPossiblyToThrow).Throw();
            }
        }

        public static void RethrowWithOriginalStackTrace(this Exception exceptionPossiblyToThrow)
        {
            ExceptionDispatchInfo.Capture(exceptionPossiblyToThrow).Throw();
        }
    }
}