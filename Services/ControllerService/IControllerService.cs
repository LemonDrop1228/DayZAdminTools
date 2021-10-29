using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Views;
using PropertyChanged;

namespace DayZTediratorToolz.Services
{
    public interface IControllerService
    {
        IBaseView CurrentView { get; }
        void InitializeViews(IEnumerable<IBaseView> views);
        void SetView(IBaseView view);
        void CloseViews();
        IEnumerable<IBaseView> GetInfoAdminViews();
        IEnumerable<IBaseView> GetVanillaToolViews();
        IEnumerable<IBaseView> GetModToolViews();
        void StartAtHome();
    }

    [AddINotifyPropertyChangedInterface]
    public class ControllerService : IControllerService
    {
        public IBaseView CurrentView { get; private set; }
        ObservableCollection<IBaseView> ViewCollection { get; set; }

        public void InitializeViews(IEnumerable<IBaseView> views)
        {
            ViewCollection = new ObservableCollection<IBaseView>();
            ViewCollection.AddRange(views.OrderBy(v => v.ViewMenuData.ViewIndex).ToArray());
        }

        public void SetView(IBaseView view) => CurrentView = view;

        public void CloseViews() {foreach (var view in ViewCollection) view.CloseView();}

        public IEnumerable<IBaseView> GetInfoAdminViews() => ViewCollection.Where(v => v.ViewMenuData.ViewType.In(DayZTediratorConstants.ViewTypes.Info, DayZTediratorConstants.ViewTypes.Admin));

        public IEnumerable<IBaseView> GetVanillaToolViews() => ViewCollection.Where(v => v.ViewMenuData.ViewType == DayZTediratorConstants.ViewTypes.VanillaTool);

        public IEnumerable<IBaseView> GetModToolViews() => ViewCollection.Where(v => v.ViewMenuData.ViewType == DayZTediratorConstants.ViewTypes.ModTool);
        public void StartAtHome() => CurrentView = ViewCollection.FirstOrDefault();
    }

}