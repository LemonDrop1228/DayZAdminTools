﻿using System.Windows;
using System.Windows.Controls;
using DayZTediratorToolz.Services;
using PropertyChanged;

namespace DayZTediratorToolz.Helpers.CustomControls
{
    [AddINotifyPropertyChangedInterface]
    public partial class FileInput : UserControl
    {
        private static bool isBusy;

        private IGeneralHelperService _generalHelperService { get; set; }
        public string FilePath { get; set; }
        public DayZTediratorConstants.PathTypes LocalPathType { get; set; }




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

            public string TitleString { get; set; }

            private static void SetTitle(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                (d as FileInput).TBlock_Title.Text = (string)e.NewValue;
            }

        #endregion






        public FileInput()
        {
            InitializeComponent();
        }

        public void ConfigureHelperService(IGeneralHelperService service)
        {
            _generalHelperService = service;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;

            var tempFilePath = _generalHelperService.GetPathFromUser(LocalPathType,
                DayZTediratorConstants.DialogTypes.Open);

            if (_generalHelperService.VerifyFilePath(tempFilePath))
            {
                FilePath = tempFilePath;
                if(FileOpened != null)
                    FileOpened(this, new() {FilePath = tempFilePath});
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {

        }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;

            var tempFilePath = _generalHelperService.GetPathFromUser(LocalPathType,
                DayZTediratorConstants.DialogTypes.Export);

            if (_generalHelperService.VerifyFilePath(tempFilePath))
            {
                FilePath = tempFilePath;
                if(FileOpened != null)
                    FileOpened(this, new() {FilePath = tempFilePath});
            }
        }


        public delegate void OpenFileEventHandler(object sender, FileEventArgs args);
        public event OpenFileEventHandler FileOpened;

        public delegate void SaveFileEventHandler(object sender, FileEventArgs args);
        public event SaveFileEventHandler FileSaved;

        public delegate void ExportFileEventHandler(object sender, FileEventArgs args);
        public event ExportFileEventHandler FileExported;
    }

    public class FileEventArgs
    {
        public string FilePath { get; set; }
    }
}