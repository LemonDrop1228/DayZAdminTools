using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Services;
using DayZTediratorToolz.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace DayZTediratorToolz
{
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow
    {
        private readonly IServerInspectionService _serverInspectionService;
        private readonly IControllerService _controller;
        private readonly INotificationService _notificationService;


        System.Media.SoundPlayer soundPlayer { get; set; } = new System.Media.SoundPlayer();
        public IBaseView ActiveView { get => _controller.CurrentView;}
        
        
        public MainWindow(IAppSettingsManager appSettingsManager, 
            IServerInspectionService serverInspectionService, 
            IControllerService controller,
            INotificationService notificationService)
        {
            _serverInspectionService = serverInspectionService;
            _controller = controller;
            _notificationService = notificationService;
            InitializeComponent();
            DataContext = this;
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal: WindowState.Maximized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _serverInspectionService.ShutDownInspectionService();
            _controller.CloseViews();
            base.OnClosed(e);
        }

        private void IconImage_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                soundPlayer.SoundLocation = "EEGG/TBEEG.wav";
                soundPlayer.LoadCompleted += delegate(object sender, AsyncCompletedEventArgs e) {
                    soundPlayer.Play();
                };
                soundPlayer.LoadAsync();
            }
        }

        private void TitleCard_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1 && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // _notificationService.Initialize();
        }

        private void NavClickedAdminButton(object sender, RoutedEventArgs e)
        {
            ChangeView(DayZTediratorConstants.Views.Admin);
        }
        
        private void NavClickedTypesEditorButton(object sender, RoutedEventArgs e)
        {
            ChangeView(DayZTediratorConstants.Views.TypesEditor);
        }

        private void ChangeView(DayZTediratorConstants.Views viewID)
        {
            if (!_controller.CheckView(viewID))
            {
                _controller.SetView(viewID);
                HostCard.GetBindingExpression(Card.ContentProperty).UpdateTarget();
            }
                
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void NavClickedHomeButton(object sender, RoutedEventArgs e)
        {
            ChangeView(DayZTediratorConstants.Views.Home);
        }

        private void NavClickedTestButton(object sender, RoutedEventArgs e)
        {
            ChangeView(DayZTediratorConstants.Views.TestView);
        }

        private void NavClickedEffectAreaEditorButton(object sender, RoutedEventArgs e)
        {
            ChangeView(DayZTediratorConstants.Views.EffectAreaEditor);
        }
    }
}
