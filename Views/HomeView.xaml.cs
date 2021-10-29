using System;
using System.Reflection;
using System.Windows.Controls;
using DayZTediratorToolz.Helpers;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    public partial class HomeView : BaseView
    {
        public override ViewMenuData ViewMenuData { get; set; }
        public string CurrentVersion { get; set; }

        public HomeView() : base()
        {
            ViewMenuData = new() { ViewIndex = 0, ViewLabel = "Home", ViewIcon = PackIconKind.SignRoutes, ViewType = DayZTediratorConstants.ViewTypes.Info};

            InitializeComponent();
            DataContext = this;
            CurrentVersion = $"Version {Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "Version Unknown"}";
        }
    }
}