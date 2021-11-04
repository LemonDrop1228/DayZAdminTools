using System.Windows.Controls;
using PropertyChanged;

namespace DayZTediratorToolz.Helpers.CustomControls
{
    [AddINotifyPropertyChangedInterface]
    public partial class AdvancedGroupBox : UserControl
    {
        public object HostedHeaderContent { get; set; }
        public object HostedBodyContent { get; set; }

        public AdvancedGroupBox()
        {
            InitializeComponent();
            BodyControlHost.DataContext = HeaderControlHost.DataContext = this;
        }
    }
}