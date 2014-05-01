// ActiveTime
// Copyright (C) 2011 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Threading;
using DustInTheWind.ActiveTime.MainGuiModule.Services;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.Services
{
    [TestFixture]
    public class StatusInfoServiceTests
    {
        private const string Text = "same test text";

        private static StatusInfoService CreateStatusInfoService()
        {
            return new StatusInfoService();
        }

        [Test]
        public void Constructor_trows_no_exception()
        {
            CreateStatusInfoService();
        }

        #region StatusText

        [Test]
        public void DefaultStatusText_constant_value()
        {
            Assert.That(StatusInfoService.DEFAULT_STATUS_TEXT, Is.EqualTo("Ready"));
        }

        [Test]
        public void StatusText_initial_value()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();

            Assert.That(statusInfoService.StatusText, Is.EqualTo("Ready"));
        }

        [Test]
        public void StatusText_set_value()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            statusInfoService.StatusText = Text;

            Assert.That(statusInfoService.StatusText, Is.EqualTo(Text));
        }

        [Test]
        public void StatusText_set_null_value()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            statusInfoService.StatusText = null;

            Assert.That(statusInfoService.StatusText, Is.Null);
        }

        [Test]
        public void StatusText_raises_StatusTextChanged_event()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            bool eventRaised = false;
            statusInfoService.StatusTextChanged += (s, e) => eventRaised = true;

            statusInfoService.StatusText = Text;

            if (!eventRaised)
                Assert.Fail("Event StatusTextChanged was not raised.");
        }

        #endregion

        #region SetStatus

        [Test]
        public void SetStatus_sets_correct_status_text()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            statusInfoService.SetStatus(Text);

            Assert.That(statusInfoService.StatusText, Is.EqualTo(Text));
        }

        [Test]
        public void SetStatus_resets_status_text_after_specified_time()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            statusInfoService.SetStatus(Text, 100);

            Thread.Sleep(120);

            Assert.That(statusInfoService.StatusText, Is.EqualTo(StatusInfoService.DEFAULT_STATUS_TEXT));
        }

        [Test]
        public void SetStatus_raises_StatusTextChanged_event()
        {
            StatusInfoService statusInfoService = CreateStatusInfoService();
            bool eventRaised = false;
            statusInfoService.StatusTextChanged += (s, e) => eventRaised = true;

            statusInfoService.SetStatus(Text);

            if (!eventRaised)
                Assert.Fail("Event StatusTextChanged was not raised.");
        }

        #endregion
    }
}
