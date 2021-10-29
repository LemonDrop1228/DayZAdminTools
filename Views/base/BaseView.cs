using System.Windows.Controls;
using DayZTediratorToolz.Helpers;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    public interface IBaseView
    {
        void CloseView();
        ViewMenuData ViewMenuData { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class BaseView : UserControl, IBaseView
    {
        public virtual void CloseView()
        {

        }

        public virtual ViewMenuData ViewMenuData { get; set; }
    }

    public record ViewMenuData
    {
        public int ViewIndex { get; set; }
        public string ViewLabel { get; set; }
        public PackIconKind ViewIcon { get; set; }
        public DayZTediratorConstants.ViewTypes ViewType { get; set; }
    }

}