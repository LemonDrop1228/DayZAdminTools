using System;
using System.Reflection;
using System.Windows.Controls;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    public partial class HomeView : BaseView
    {
        public string CurrentVersion { get; set; }

        public HomeView() : base()
        {
            InitializeComponent();
            DataContext = this;
            CurrentVersion = $"Version {Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "Version Unknown"}";
        }
    }
}