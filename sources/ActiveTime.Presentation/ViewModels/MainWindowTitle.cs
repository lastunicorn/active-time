using System.Reflection;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class MainWindowTitle
    {
        private readonly string value;

        public MainWindowTitle()
        {
            value = BuildWindowTitle();
        }

        private static string BuildWindowTitle()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyName assemblyName = assembly.GetName();

            return $"ActiveTime {assemblyName.Version.ToString(3)}";
        }

        public override string ToString()
        {
            return value;
        }
    }
}