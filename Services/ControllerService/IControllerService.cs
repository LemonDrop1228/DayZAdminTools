using System.Collections.ObjectModel;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Views;
using DayZTediratorToolz.Views.AdminPanel;
using PropertyChanged;

namespace DayZTediratorToolz.Services
{
    public interface IControllerService
    {
        IBaseView CurrentView { get; }
        void InitializeViews(params IBaseView[] views);
        void SetView(DayZTediratorConstants.Views admin);
        bool CheckView(DayZTediratorConstants.Views viewId);
    }
    
    [AddINotifyPropertyChangedInterface]
    public class ControllerService : IControllerService
    {
        DayZTediratorConstants.Views CurrentViewID { get; set; } 
        public IBaseView CurrentView { get => ViewCollection[(int)CurrentViewID];}
        ObservableCollection<IBaseView> ViewCollection { get; set; }

        public ControllerService()
        {
        }
        
        public void InitializeViews(params IBaseView[] views)
        {
            ViewCollection = new ObservableCollection<IBaseView>();
            ViewCollection.AddRange(views);
            
            CurrentViewID = DayZTediratorConstants.Views.Home;
        }

        public void SetView(DayZTediratorConstants.Views viewID)
        {
            CurrentViewID = viewID;
        }

        public bool CheckView(DayZTediratorConstants.Views viewId)
        {
            return CurrentViewID == viewId;
        }
    }

}