using DayZTediratorToolz.Views;

namespace DayZTediratorToolz.Helpers
{
    public class ViewBucket
    {
        public ViewBucket(IBaseView baseView) => ViewRef = baseView;

        public IBaseView ViewRef { get; set; }
    }
}