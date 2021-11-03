using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using DayZTediratorToolz.Services;
using PropertyChanged;

namespace DayZTediratorToolz.Helpers.CustomControls
{
    [AddINotifyPropertyChangedInterface]
    public partial class FileInput : ContentControl
    {
        private static bool isBusy;
        private static string _dialogTitle;
        private object _toolBarContent;

        private IGeneralHelperService _generalHelperService { get; set; }
        public string FilePath { get; set; }
        public DayZTediratorConstants.PathTypes LocalPathType { get; set; }
        public bool ShowIndicator { get => isBusy; }


        #region Dependancy Props

            public static readonly DependencyProperty dependencyProperty_IsBusy =
                DependencyProperty.Register("IsBusy", typeof(bool), typeof(FileInput),
                    new PropertyMetadata(false,new PropertyChangedCallback(ToggleBusy)));

            public bool IsBusy
            {
                get=> (bool)GetValue(dependencyProperty_IsBusy);
                set=> SetValue(dependencyProperty_IsBusy, value);
            }

            private static void ToggleBusy(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                isBusy = (bool)e.NewValue;
            }

            public static readonly DependencyProperty dependencyProperty_TitleString =
                DependencyProperty.Register("Title", typeof(string), typeof(FileInput),
                    new PropertyMetadata("Placeholder Title",new PropertyChangedCallback(SetTitle)));

            public string Title
            {
                get=> (string)GetValue(dependencyProperty_TitleString);
                set=> SetValue(dependencyProperty_TitleString, value);
            }

            private static void SetTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                (d as FileInput).TBlock_Title.Text = (string)e.NewValue;
            }

            public static readonly DependencyProperty dependencyProperty_DialogTitle =
                DependencyProperty.Register("DialogTitle", typeof(string), typeof(FileInput),
                    new PropertyMetadata("Placeholder Title",new PropertyChangedCallback(SetDialogTitle)));

            private bool isAnimatingClosed;
            private bool isAnimatingOpen;


            public string DialogTitle
            {
                get=> (string)GetValue(dependencyProperty_TitleString);
                set=> SetValue(dependencyProperty_TitleString, value);
            }

            private static void SetDialogTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                _dialogTitle = (string) e.NewValue;
            }

            #endregion


        public FileInput()
        {
            InitializeComponent();
            PathBox.DataContext = this;
        }

        public void ConfigureHelperService(IGeneralHelperService service) => _generalHelperService = service;

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;

            var tempFilePath = _generalHelperService.GetPathFromUser(LocalPathType,
                DayZTediratorConstants.DialogTypes.Open, _dialogTitle);

            if (_generalHelperService.VerifyFilePath(tempFilePath))
            {
                FilePath = tempFilePath;
                if(FileOpened is not null)
                    FileOpened(this, new() {FilePath = this.FilePath});
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (FileSaved is not null)
                FileSaved(this, new() {FilePath = this.FilePath});
        }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;

            var tempFilePath = _generalHelperService.GetPathFromUser(LocalPathType,
                DayZTediratorConstants.DialogTypes.Export, _dialogTitle);

            if (_generalHelperService.VerifyFilePath(tempFilePath))
            {
                FilePath = tempFilePath;
                if(FileOpened is not null)
                    FileOpened(this, new() {FilePath = this.FilePath});
            }
        }


        public delegate void OpenFileEventHandler(object sender, FileEventArgs args);
        public event OpenFileEventHandler FileOpened;

        public delegate void SaveFileEventHandler(object sender, FileEventArgs args);
        public event SaveFileEventHandler FileSaved;

        public delegate void ExportFileEventHandler(object sender, FileEventArgs args);
        public event ExportFileEventHandler FileExported;


        private void PathBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //(sender as TextBox).Width = 500;
        }

        private void PathBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //(sender as TextBox).Width = 300;
        }
    }

    public class FileEventArgs
    {
        public string FilePath { get; set; }
    }
}