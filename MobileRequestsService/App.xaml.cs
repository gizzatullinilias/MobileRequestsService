﻿using Microsoft.Extensions.DependencyInjection;
using MobileRequestsService.ViewModels;
using MobileRequestsService.Services;

namespace MobileRequestsService
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
