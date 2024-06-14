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

using System;
using System.Collections;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Domain.Services
{
    public class Dwarfs : IEnumerable<IDwarf>
    {
        private readonly List<IDwarf> list = new List<IDwarf>();

        public void Add(IDwarf dwarf)
        {
            if (dwarf == null) throw new ArgumentNullException(nameof(dwarf));
            list.Add(dwarf);
        }

        public List<Exception> StartAll()
        {
            List<Exception> errors = new List<Exception>();

            foreach (IDwarf dwarf in list)
            {
                try
                {
                    dwarf.Start();
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            return errors;
        }

        public List<Exception> StopAll()
        {
            List<Exception> errors = new List<Exception>();

            foreach (IDwarf dwarf in list)
            {
                try
                {
                    dwarf.Stop();
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            return errors;
        }

        public IEnumerator<IDwarf> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}