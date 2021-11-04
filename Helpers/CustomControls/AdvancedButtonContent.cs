using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using StackPanel = AutoGrid.StackPanel;

namespace DayZTediratorToolz.Helpers.CustomControls
{
    public class AdvancedButtonContent : StackPanel
    {
        public AdvancedButtonContent(PackIconKind image,
            string label,
            DayZTediratorConstants.AdvBttnContentType type,
            Thickness? childMargin = null,
            Size? iconSize = null)
        {
            Image = image;
            Label = label;
            Type = type;
            this.ChildMargin = childMargin ?? new Thickness(3);
            IconSize = iconSize ?? new Size(24,24);
            this.Orientation = Orientation.Horizontal;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var Icon = new PackIcon() {Kind = Image, Width = IconSize.Width, Height = IconSize.Height, Margin = ChildMargin};
            var Text = new TextBlock() {Text = Label, Margin = ChildMargin, VerticalAlignment = VerticalAlignment.Center};
            switch (Type)
            {
                case DayZTediratorConstants.AdvBttnContentType.Both:
                    this.Children.Add(Icon);
                    this.Children.Add(Text);
                    break;
                case DayZTediratorConstants.AdvBttnContentType.Image:
                    this.Children.Add(Icon);
                    break;
                case DayZTediratorConstants.AdvBttnContentType.Txt:
                    this.Children.Add(Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public PackIconKind Image { get; set; }
        public string Label { get; set; }
        public DayZTediratorConstants.AdvBttnContentType Type { get; set; }
        public Thickness ChildMargin { get; set; }
        public Size IconSize { get; set; }
    }
}