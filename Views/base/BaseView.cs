using System.Windows.Controls;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    public interface IBaseView
    {
        void CloseView();
    }
    
    [AddINotifyPropertyChangedInterface]
    public class BaseView : UserControl, IBaseView
    {
        public virtual void CloseView()
        {
            
        }
    }
}