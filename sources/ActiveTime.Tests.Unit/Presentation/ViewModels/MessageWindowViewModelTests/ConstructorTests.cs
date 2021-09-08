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

using DustInTheWind.ActiveTime.Presentation.Properties;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels.MessageWindowViewModelTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void WhenCreatingANewInstance_ThenDefaultMessageIsSet()
        {
            MessageViewModel messageViewModel = new MessageViewModel();

            string expectedMessage = Resources.MessageWindow_DefaultText;
            Assert.That(messageViewModel.Message, Is.EqualTo(expectedMessage));
        }
    }
}