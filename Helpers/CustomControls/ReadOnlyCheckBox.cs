using System.Windows.Controls;

namespace DayZTediratorToolz.Helpers.CustomControls
{
    public class ReadOnlyCheckBox : CheckBox
    {
        public ReadOnlyCheckBox() : base()
        {
            this.IsHitTestVisible = false;
            this.Focusable = false;
        }
    }
}