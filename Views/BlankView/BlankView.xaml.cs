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
            DataContext = this;
        }

        private void BlankTitle_FileOpened(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }

        private void BlankTitle_FileExported(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }

        private void BlankTitle_FileSaved(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }
    }
}
