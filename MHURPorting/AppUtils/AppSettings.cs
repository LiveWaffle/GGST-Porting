﻿using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.UE4.Versions;
using GGSTPorting.Services.Endpoints.Models;
using Newtonsoft.Json;

namespace GGSTPorting.AppUtils;

public partial class AppSettings : ObservableObject
{
    public static AppSettings Current;

    public static readonly DirectoryInfo DirectoryPath = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GGSTPorting"));
    public static readonly DirectoryInfo FilePath = new(Path.Combine(DirectoryPath.FullName, "AppSettings.json"));

    public static void Load()
    {
        if (File.Exists(FilePath.FullName))
        {
            Current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(FilePath.FullName));
        }

        Current ??= new AppSettings();

    }

    public static void Save()
    {
        File.WriteAllText(FilePath.FullName, JsonConvert.SerializeObject(Current, Formatting.Indented));
    }

    [ObservableProperty] 
    private string? archivePath;
    
    [ObservableProperty] 
    private ELanguage language;
    
    [ObservableProperty] 
    private EInstallType installType;
    
    [ObservableProperty] 
    private ERichPresenceAccess discordRPC;
    
    [ObservableProperty] 
    private AesResponse aesResponse;
}