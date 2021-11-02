using DayZTediratorToolz.Helpers;
using System.Windows.Controls;

namespace DayZTediratorToolz.Views
{
    public partial class SpawnPointsEditorView : BaseView
    {
        public const string _titleKey = "SpawnPositionTitle";
        public override ViewMenuData ViewMenuData { get; set; }
        public SpawnPointsEditorView()
        {
            ViewMenuData = new ViewMenuData() { ViewIcon = MaterialDesignThemes.Wpf.PackIconKind.Download, ViewIndex=5, ViewLabel= _titleKey.LoadFromRes<string>(), ViewType=DayZTediratorConstants.ViewTypes.VanillaTool };
            InitializeComponent();
            DataContext = this;
        }
    }
}
