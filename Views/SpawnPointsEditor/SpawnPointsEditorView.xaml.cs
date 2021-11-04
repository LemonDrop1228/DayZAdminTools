using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Services;
using System.Windows.Controls;

namespace DayZTediratorToolz.Views
{
    public partial class SpawnPointsEditorView : BaseView
    {
        public const string _titleKey = "SpawnPositionTitle";
        private readonly IGeneralHelperService _generalHelperService;

        public override ViewMenuData ViewMenuData { get; set; }
        public SpawnPointsEditorView(IGeneralHelperService generalHelperService)

        {    
            ViewMenuData = new ViewMenuData() { ViewIcon = MaterialDesignThemes.Wpf.PackIconKind.Download, ViewIndex=5, ViewLabel= _titleKey.LoadFromRes<string>(), ViewType=DayZTediratorConstants.ViewTypes.VanillaTool };
            _generalHelperService = generalHelperService;
            InitializeComponent();
            DataContext = this;
            SpawnFileIput.ConfigureHelperService(generalHelperService);
        }
        private void SpawnFileIput_FileOpened(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }

        private void SpawnFileIput_FileExported(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }

        private void SpawnFileIput_FileSaved(object sender, Helpers.CustomControls.FileEventArgs args)
        {

        }
    }
}
