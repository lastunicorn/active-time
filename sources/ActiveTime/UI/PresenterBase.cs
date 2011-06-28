using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.UI
{
    internal class PresenterBase<T> where T : class,IView
    {
        private T view;

        public T View
        {
            get { return view; }
            set { view = value; }
        }

        public PresenterBase(T view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            this.view = view;
        }
    }
}
