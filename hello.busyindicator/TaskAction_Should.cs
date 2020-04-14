using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace hello.busyindicator
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class TaskAction_Should
    {
        [Test]
        public void Copy_names()
        {
            const string taskName = nameof(Copy_names);

            // test all ctors
            using (var sut = new TaskAction((c, p) => { }, taskName))
                sut.Name.Should().Be(taskName);

            using (var sut = new TaskAction(() => { }, taskName))
                sut.Name.Should().Be(taskName);
        }

        [Test]
        public async Task Support_cancelable_tasks()
        {
            const string taskName = nameof(Support_cancelable_tasks);
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskAction((Action<CancellationToken, IProgress<int>>) null, taskName))
                .Should().Throw<ArgumentNullException>();

            using (var sut = new TaskAction((t,p) => throw new NotImplementedException(), taskName))
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
                }, taskName))
            {
                var task = sut.Run(new Progress<int>(p => Trace.TraceInformation($"{p}%")));
                Thread.Sleep(100);
                sut.Cancel();
                await task.ShouldThrow<OperationCanceledException>();
            }
        }

        [Test]
        public async Task Support_blocking_threads()
        {
            // ReSharper disable once ObjectCreationAsStatement
            0.Invoking(x => new TaskAction((Action) null, "test"))
                .Should().Throw<ArgumentNullException>();

            using (var sut = new TaskAction(() =>
            {
                while (true)
                    Thread.Sleep(500);
                // ReSharper disable once FunctionNeverReturns
            }, "test"))
            {
                var task = sut.Run();
                Thread.Sleep(100);
                sut.Cancel();
                await task.ShouldThrow<OperationCanceledException>();
            }
        }
    }
}