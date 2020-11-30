﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shipdoku.ViewModels
{
    public class StartMenuViewModel : BindableBase
    {
        public string Title { get; } = "Shipdoku";
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public StartMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath, new NavigationParameters());
        }
    }
}