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

using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;

namespace DustInTheWind.ActiveTime
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            Uri uri = new Uri("/ActiveTime;component/ModuleCatalog.xaml", UriKind.Relative);
            ModuleCatalog moduleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(uri);

            return moduleCatalog;


            //var catalog = new DirectoryModuleCatalog();
            //catalog.ModulePath = @".\Modules";
            //return catalog;
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }
}
