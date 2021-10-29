using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public IEnumerable<ViewBucket> DefaultViews { get => _controller.GetInfoAdminViews().Select(v => new ViewBucket(v));}

        public IEnumerable<ViewBucket> VanillaToolViews { get => _controller.GetVanillaToolViews().Select(v => new ViewBucket(v));}

        public IEnumerable<ViewBucket> ModToolViews { get => _controller.GetModToolViews().Select(v => new ViewBucket(v));}

        public ICommand ChangeView => new RelayCommand(o =>
        {
            _controller.SetView((o as ViewBucket).ViewRef);
            HostCard.GetBindingExpression(Card.ContentProperty).UpdateTarget();
        }, o => true);


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

        private void CloseButton_OnClick(object sender, RoutedEventArgs e) => Close();

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

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _controller.StartAtHome();
            HostCard.GetBindingExpression(Card.ContentProperty).UpdateTarget();
        }
    }
}