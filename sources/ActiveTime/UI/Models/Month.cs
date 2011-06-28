
namespace DustInTheWind.ActiveTime.UI.Models
{
    public class Month
    {
        private int value;

        public int Value
        {
            get { return this.value; }
        }

        private string text;

        public string Text
        {
            get { return text; }
        }

        public Month(int value, string text)
        {
            this.value = value;
            this.text = text;
        }
    }
}
