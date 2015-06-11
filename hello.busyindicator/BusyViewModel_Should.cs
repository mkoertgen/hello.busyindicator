using System;
using System.Linq;
using System.Threading;
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
        public async void Throw_on_task_errors()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);
            await sut.Handle(new StartTaskMessage((c, p) => { throw new InvalidOperationException("test"); }))
                .ShouldThrow<InvalidOperationException>();
            events.Received().PublishOnUIThread(TaskState.Faulted);
        }

        [Test]
        public async void Catch_canceled_exceptions()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);
            await sut.Handle(new StartTaskMessage((c, p) => { throw new OperationCanceledException("test"); }));
            events.Received().PublishOnUIThread(TaskState.Canceled);
        }

        [Test]
        public async void Notify_completed()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);
            var task = sut.Handle(new StartTaskMessage((c, p) => { }));
            events.Received().PublishOnUIThread(TaskState.Started);
            await task;
            events.Received().PublishOnUIThread(TaskState.Completed);
        }

        [Test]
        public async void Report_progress()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);
            var task = sut.Handle(new StartTaskMessage((c, p) => Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                Thread.Sleep(10);
                p.Report(i * 10);
            }), "test"));

            sut.IsBusy.Should().BeTrue();
            sut.Progress.Should().Be(-1);
            sut.WaitingFor.Should().Be("test");
            sut.IsIndeterminate.Should().BeTrue();

            await task;

            sut.Progress.Should().Be(100);
            sut.IsBusy.Should().BeFalse();
            sut.IsIndeterminate.Should().BeFalse();
        }

        [Test]
        public void Support_Cancelation()
        {
            var events = Substitute.For<IEventAggregator>();
            var sut = new BusyViewModel(events);
            var task = sut.Handle(new StartTaskMessage((c, p) => Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                Thread.Sleep(10);
                p.Report(i * 10);
                c.ThrowIfCancellationRequested();
            }), "test"));

            sut.IsBusy.Should().BeTrue();
            sut.WaitingFor.Should().Be("test");

            sut.Cancel();

            sut.WaitingFor.Should().Be("Canceling...");
            sut.IsIndeterminate.Should().BeTrue();
            sut.IsBusy.Should().BeTrue();

            // ReSharper disable once MethodSupportsCancellation
            task.Wait();

            sut.IsBusy.Should().BeFalse();
            events.Received().PublishOnUIThread(TaskState.Canceled);
        }
    }
}