using System;
using System.Diagnostics;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace hello.busyindicator
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class TaskAction_Should
    {
        [Test]
        public async void Support_cancelable_tasks()
        {
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskAction((Action<CancellationToken, IProgress<int>>) null)).ShouldThrow<ArgumentNullException>();

            using (var sut = new TaskAction((t,p) => {throw new NotImplementedException();}))
            {
                await sut.Run().ShouldThrow<NotImplementedException>();
            }

            using (var sut = new TaskAction((t, p) =>
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        t.ThrowIfCancellationRequested();
                        Thread.Sleep(20);
                        p?.Report(i * 10);
                    }
                }))
            {
                var task = sut.Run(new Progress<int>(p => Trace.TraceInformation($"{p}%")));
                Thread.Sleep(100);
                sut.Cancel();
                await task.ShouldThrow<OperationCanceledException>();
            }
        }

        [Test]
        public async void Support_blocking_threads()
        {
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskAction((Action) null)).ShouldThrow<ArgumentNullException>();

            using (var sut = new TaskAction(() =>
            {
                while (true)
                    Thread.Sleep(500);
                // ReSharper disable once FunctionNeverReturns
            }))
            {
                var task = sut.Run();
                Thread.Sleep(100);
                sut.Cancel();
                await task.ShouldThrow<OperationCanceledException>();
            }
        }
    }
}