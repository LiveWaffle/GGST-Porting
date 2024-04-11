using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.UE4.Versions;
using GGSTPorting.AppUtils;
using Newtonsoft.Json;
using YamlDotNet;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GGSTPorting.ViewModels;

public class StartupViewModel : ObservableObject
{
    public string ArchivePath
    {
        get => AppSettings.Current.ArchivePath;
        set
        {
            AppSettings.Current.ArchivePath = value;
            OnPropertyChanged();
        }
    }
    
    public ELanguage Language
    {
        get => AppSettings.Current.Language;
        set
        {
            AppSettings.Current.Language = value;
            OnPropertyChanged();
        }
    }
    
    public void CheckForInstallation()
    {
        bool found_install = false;
        foreach (var drive in DriveInfo.GetDrives())
        {
            var launcherInstalledPath = $"{drive.Name}Program Files (x86)\\Steam\\steamapps\\common\\GUILTY GEAR STRIVE\\RED\\Content\\Paks";
            if (Directory.Exists(launcherInstalledPath))
            {
                found_install = true;
                ArchivePath = launcherInstalledPath;
                Log.Information("Found Install at {0} :D",ArchivePath);
                break;

            }
        }
        if (!found_install) {
            ArchivePath = "GUILTY GEAR STRIVE\\RED\\Content\\Paks";
            Log.Information("No install found!");
        }

    }
    
}