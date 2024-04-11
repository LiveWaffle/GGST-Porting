﻿using System.Windows;
using GGSTPorting.AppUtils;
using GGSTPorting.ViewModels;

namespace GGSTPorting.Views;

public partial class StartupView
{
    public StartupView()
    {
        InitializeComponent();
        AppVM.StartupVM = new StartupViewModel();
        DataContext = AppVM.StartupVM;
        
        AppVM.StartupVM.CheckForInstallation();
    }

    private async void OnClickContinue(object sender, RoutedEventArgs e)
    {
        Close();
        await AppVM.MainVM.Initialize();
    }
    
    private void OnClickInstallation(object sender, RoutedEventArgs e)
    {
        if (AppHelper.TrySelectFolder(out var path))
        {
            AppVM.StartupVM.ArchivePath = path;
        }
    }
}