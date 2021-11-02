using System.Windows.Controls;

namespace DayZTediratorToolz.Views
{
    public partial class SpawnPointsEditorView : BaseView
    {
        public override ViewMenuData ViewMenuData { get; set; }
        public SpawnPointsEditorView()
        {
            ViewMenuData = new ViewMenuData() { ViewIcon = MaterialDesignThemes.Wpf.PackIconKind.Download, ViewIndex=4   };
            InitializeComponent();
        }
    }
}
