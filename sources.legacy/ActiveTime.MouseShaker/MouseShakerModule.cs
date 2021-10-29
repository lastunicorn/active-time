using Microsoft.Practices.Prism.Modularity;

namespace DustInTheWind.ActiveTime.MouseShaker
{
    public class MouseShakerModule : IModule
    {
        private MouseShakeBehavior mouseShakeBehavior;

        public void Initialize()
        {
            mouseShakeBehavior = new MouseShakeBehavior();
            mouseShakeBehavior.Start();
        }
    }
}