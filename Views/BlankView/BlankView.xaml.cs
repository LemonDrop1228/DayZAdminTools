using System.Windows.Controls;
using DayZTediratorToolz.Helpers;
using MaterialDesignThemes.Wpf;

namespace DayZTediratorToolz.Views
{
    public partial class BlankView : BaseView
    {
        public override ViewMenuData ViewMenuData { get; set; }

        public BlankView()
        {
            ViewMenuData = new ViewMenuData
            {
                ViewIndex = 0,
                ViewLabel = "Test",
                ViewIcon = PackIconKind.Abacus,
                ViewType = DayZTediratorConstants.ViewTypes.Info
            };

            InitializeComponent();
        }
    }
}
