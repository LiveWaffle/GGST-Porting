﻿using System;
using System.Windows;
using AdonisUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using GGSTPorting.AppUtils;
using MessageBox = AdonisUI.Controls.MessageBox;
using MessageBoxImage = AdonisUI.Controls.MessageBoxImage;

namespace GGSTPorting.ViewModels;

public class ApplicationViewModel : ObservableObject
{
    public MainViewModel MainVM;
    public StartupViewModel StartupVM;
    public SettingsViewModel SettingsVM;
    public CUE4ParseViewModel CUE4ParseVM;
    public AssetHandlerViewModel? AssetHandlerVM;
    public BlenderViewModel BlenderVM;

    public void RestartWithMessage(string caption, string message)
    {
        var messageBox = new MessageBoxModel
        {
            Caption = caption,
            Icon = MessageBoxImage.Exclamation,
            Text = message,
            Buttons = new[] { MessageBoxButtons.Ok() }
        };

        MessageBox.Show(messageBox);
        Restart();
    }
    
    public void Restart()
    {
        AppHelper.Launch(AppDomain.CurrentDomain.FriendlyName, shellExecute: false);
        Application.Current.Shutdown();
    }
    
    public void Quit()
    {
        Application.Current.Shutdown();
    }
}