using System.Windows.Controls;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    public interface IBaseView
    {
        
    }
    
    [AddINotifyPropertyChangedInterface]
    public class BaseView : UserControl, IBaseView
    {
        
    }
}