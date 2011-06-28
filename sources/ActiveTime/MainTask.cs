using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSharp.Core.Tasks;
using MVCSharp.Core.Configuration.Tasks;

namespace DustInTheWind.ActiveTime
{
    internal class MainTask : TaskBase
    {
        [InteractionPoint(typeof(MainPresenter), Comments)]
        public const string Main = "Main";

        [InteractionPoint(typeof(CommentsPresenter))]
        public const string Comments = "Comments";

        [InteractionPoint(typeof(TrayIconPresenter), Main)]
        public const string TrayIcon = "TrayIcon";

        public override void OnStart(object param)
        {
            Navigator.NavigateDirectly(TrayIcon);
        }
    }
}
