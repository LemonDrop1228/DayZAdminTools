using System.Windows.Controls;
using DayZTediratorToolz.Helpers;
using MaterialDesignThemes.Wpf;

namespace DayZTediratorToolz.Views.Dialogs
{
    public partial class MapDialogView : BaseView
    {
        public override ViewMenuData ViewMenuData { get; set; }

        public MapDialogView()
        {
            ViewMenuData = new() { ViewIndex = 3, ViewLabel = "Map Input Dialog", ViewIcon = PackIconKind.Map, ViewType = DayZTediratorConstants.ViewTypes.Dialog};

            InitializeComponent();
        }
    }
}
