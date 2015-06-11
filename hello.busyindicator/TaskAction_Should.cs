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
        public async void Be_creatable()
        {
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskAction(null)).ShouldThrow<ArgumentNullException>();

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
    }
}