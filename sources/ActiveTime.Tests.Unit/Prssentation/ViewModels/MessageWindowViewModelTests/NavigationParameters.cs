// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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

using System.Collections.Generic;
using DustInTheWind.ActiveTime.Presentation.Properties;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.MainGuiModule.ViewModels.MessageWindowViewModelTests
{
    [TestFixture]
    public class NavigationParameters
    {
        private MessageViewModel messageViewModel;

        [SetUp]
        public void SetUp()
        {
            messageViewModel = new MessageViewModel();
        }

        [Test]
        public void Message_value_is_set_to_value_contained_by_the_Text_parameter()
        {
            const string text = "something";

            messageViewModel.NavigationParameters = new Dictionary<string, object>
            {
                { "Text", text }
            };

            Assert.That(messageViewModel.Message, Is.EqualTo(text));
        }

        [Test]
        public void PropertyChanged_event_is_raised_after_Message_is_set_from_the_Text_parameter()
        {
            bool eventWasRaised = false;
            messageViewModel.PropertyChanged += (sender, args) => eventWasRaised = true;

            messageViewModel.NavigationParameters = new Dictionary<string, object>
            {
                { "Text", "something" }
            };

            Assert.That(eventWasRaised, Is.True);
        }

        [Test]
        public void PropertyChanged_event_contains_the_name_of_the_Message_property()
        {
            string propertyName = null;
            messageViewModel.PropertyChanged += (sender, args) => propertyName = args.PropertyName;

            messageViewModel.NavigationParameters = new Dictionary<string, object>
            {
                { "Text", "something" }
            };

            Assert.That(propertyName, Is.EqualTo("Message"));
        }

        [Test]
        public void Message_remains_unchanged_if_NavigationParameters_does_not_contain_Text()
        {
            messageViewModel.NavigationParameters = new Dictionary<string, object>
            {
                { "MyParameter", "some value here" }
            };

            string expectedMessage = Resources.MessageWindow_DefaultText;
            Assert.That(messageViewModel.Message, Is.EqualTo(expectedMessage));
        }
    }
}
