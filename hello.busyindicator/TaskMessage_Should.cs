using System;
using System.Diagnostics;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace hello.busyindicator
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class TaskMessage_Should
    {
        [Test]
        public async void Be_creatable()
        {
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskMessage(null)).ShouldThrow<ArgumentNullException>();

            using (var sut = new TaskMessage((t,p) => {throw new NotImplementedException();}))
            {
                sut.WaitingFor.Should().Be("Please wait...");
                await sut.Run().ShouldThrow<NotImplementedException>();
            }

            using (var sut = new TaskMessage((t, p) =>
                {
                    for (int i=1; i<=10; i++)
                    {
                        t.ThrowIfCancellationRequested();
                        Thread.Sleep(20);
                        p?.Report(i*10);
                    }
                }, "test..."))
            {
                sut.WaitingFor.Should().Be("test...");
                var task = sut.Run(new Progress<int>(p => Trace.TraceInformation($"{p}%")));
                Thread.Sleep(100);
                sut.Cancel();
                await task.ShouldThrow<OperationCanceledException>();
            }
        }
    }
}