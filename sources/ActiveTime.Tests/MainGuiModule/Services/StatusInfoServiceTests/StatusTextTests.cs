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

using DustInTheWind.ActiveTime.MainGuiModule.Services;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.Services.StatusInfoServiceTests
{
    [TestFixture]
    public class StatusTextTests
    {
        private StatusInfoService statusInfoService;
        private const string Text = "same test text";

        [SetUp]
        public void SetUp()
        {
            statusInfoService = new StatusInfoService();
        }

        [Test]
        public void set_value()
        {
            statusInfoService.StatusText = Text;

            Assert.That(statusInfoService.StatusText, Is.EqualTo(Text));
        }

        [Test]
        public void set_null_value()
        {
            statusInfoService.StatusText = null;

            Assert.That(statusInfoService.StatusText, Is.Null);
        }

        [Test]
        public void raises_StatusTextChanged_event()
        {
            bool eventRaised = false;
            statusInfoService.StatusTextChanged += (s, e) => eventRaised = true;

            statusInfoService.StatusText = Text;

            if (!eventRaised)
                Assert.Fail("Event StatusTextChanged was not raised.");
        }
    }
}
