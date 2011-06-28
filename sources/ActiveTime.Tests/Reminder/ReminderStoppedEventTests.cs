using System;
using System.Threading;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 5: Initialize - Start - Stop - Stopped event -> check is raised, check status")]
    public class ReminderStoppedEventTests
    {
        private Reminder reminder;

        [SetUp]
        public void SetUp()
        {
            reminder = new Reminder();
        }

        [TearDown]
        public void TearDown()
        {
            reminder.Dispose();
        }

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void StopEvent_Status()
        {
            const int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;
                bool stoppedEventWasRaised = false;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) => ringEvent.Set());

                reminder.Stopped += new EventHandler((sender, e) =>
                                                         {
                                                             if (sender != null)
                                                             {
                                                                 status = ((Reminder) sender).Status;
                                                             }
                                                             stoppedEventWasRaised = true;
                                                         });

                reminder.Start(ringMiliseconds);

                Thread.Sleep(50);

                reminder.Stop();

                if (stoppedEventWasRaised)
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.NotStarted));
                }
                else
                {
                    Assert.Fail("The Stopped event was not raised.");
                }
            }
        }
    }
}
