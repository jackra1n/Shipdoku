using CommonServiceLocator;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Regions;

namespace Shipdoku.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private string _title = "Shipdoku";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
