using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Shipdoku.ViewModels
{
    public class StartMenuViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get;  }
        public string Title { get; set; } = "Shipdoku";

        public StartMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string createEmpty)
        {
            var navParameters = new NavigationParameters {{"createEmpty", bool.Parse(createEmpty)}};
            _regionManager.RequestNavigate("ContentRegion", "Shipdoku", navParameters);
        }
    }
}
