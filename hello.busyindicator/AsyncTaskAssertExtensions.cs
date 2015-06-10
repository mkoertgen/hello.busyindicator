using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace hello.busyindicator
{
    static class AsyncTaskAssertExtensions
    {
        public static async Task ShouldThrow<TException>(this Task task, string because = null) where TException : Exception
        {
            try
            {
                await task;
                Assert.Fail(because ?? $"The specified task was expected to throw '{typeof(TException).Name}', but no exception occured.");
            }
            catch (TException) { }
        }
    }
}