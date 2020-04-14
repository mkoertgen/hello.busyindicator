using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace hello.busyindicator
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class BusyViewModel_Should
    {
        [Test]
        public async Task Throw_on_task_errors()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Throw_on_task_errors);
            var msg = new StartTaskMessage((c, p) => throw new InvalidOperationException("test"), taskName);

            await sut.Handle(msg)
                .ShouldThrow<InvalidOperationException>();
            events.Received().PublishOnUIThread(TaskState.Faulted);
        }

        [Test]
        public async Task Catch_canceled_exceptions()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Throw_on_task_errors);
            var msg = new StartTaskMessage((c, p) => throw new OperationCanceledException("test"), taskName);

            await sut.Handle(msg);
            events.Received().PublishOnUIThread(TaskState.Canceled);
        }

        [Test]
        public async Task Notify_completed()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Throw_on_task_errors);
            var msg = new StartTaskMessage((c, p) => { }, taskName);

            var task = sut.Handle(msg);
            events.Received().PublishOnUIThread(TaskState.Started);
            await task;
            events.Received().PublishOnUIThread(TaskState.Completed);
        }

        [Test]
        public async Task Report_progress()
        {

            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Report_progress);
            var msg = new StartTaskMessage((c, p) => 
                Enumerable.Range(1, 10)
                    .ToList()
                    .ForEach(i =>
                    {
                        Thread.Sleep(10);
                        p.Report(i * 10);
                    }), taskName);

            var task = sut.Handle(msg);

            sut.IsBusy.Should().BeTrue();
            sut.Progress.Should().Be(-1);
            sut.WaitingFor.Should().Be($"Waiting for '{taskName}'...");
            sut.IsIndeterminate.Should().BeTrue();

            await task;

            sut.Progress.Should().Be(100);
            sut.IsBusy.Should().BeFalse();
            sut.IsIndeterminate.Should().BeFalse();
        }

        [Test]
        public void Support_Cancellation()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Support_Cancellation);
            var msg = new StartTaskMessage((c, p) => Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                Thread.Sleep(10);
                p.Report(i * 10);
                c.ThrowIfCancellationRequested();
            }), taskName);

            var task = sut.Handle(msg);

            sut.IsBusy.Should().BeTrue();
            sut.WaitingFor.Should().Be($"Waiting for '{taskName}'...");

            sut.Cancel();

            sut.WaitingFor.Should().Be($"Cancelling '{taskName}'...");
            sut.IsIndeterminate.Should().BeTrue();
            sut.IsBusy.Should().BeTrue();

            // ReSharper disable once MethodSupportsCancellation
            task.Wait();

            sut.IsBusy.Should().BeFalse();
            events.Received().PublishOnUIThread(TaskState.Canceled);
        }

        [Test]
        public async Task Support_exception_handling()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);

            const string taskName = nameof(Support_exception_handling);
            // 1. assert not rethrowing handled exceptions
            var taskException = new InvalidOperationException("test");
            var startMsg = new StartTaskMessage((c, p) => { throw taskException; }, taskName);

            TaskExceptionMessage exMsg = null;
            events.When(x => x.PublishOnUIThread(Arg.Any<TaskExceptionMessage>()))
                .Do(x =>
                {
                    exMsg = x.Arg<TaskExceptionMessage>();
                    if (exMsg.Exception is InvalidOperationException)
                        exMsg.Handled = true;
                });

            await sut.Handle(startMsg);

            events.Received().PublishOnUIThread(TaskState.Faulted);
            exMsg.TaskName.Should().Be(nameof(Support_exception_handling));
            exMsg.Exception.Should().Be(taskException);

            // 2. assert rethrow for unhandled exceptions
            startMsg = new StartTaskMessage((c,p) => throw new NotImplementedException("test"), taskName);
            events.ClearReceivedCalls();
            await sut.Handle(startMsg).ShouldThrow<NotImplementedException>();
            events.Received().PublishOnUIThread(TaskState.Faulted);
        }
    }
}