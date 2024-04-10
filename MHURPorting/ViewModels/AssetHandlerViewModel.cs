using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using CUE4Parse.UE4.AssetRegistry.Objects;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.SkeletalMesh;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using MHURPorting.Views.Extensions;
using Serilog;
using ZstdSharp.Unsafe;
using CUE4Parse.UE4.Objects.Core.i18N;
using MHURPorting.AppUtils;
using MHURPorting.Views.Controls;
using SharpGLTF.Schema2;
using System.Windows.Markup;
using CUE4Parse.FileProvider.Objects;

namespace MHURPorting.ViewModels;

public class AssetHandlerViewModel
{
    public readonly Dictionary<EAssetType, AssetHandlerData> Handlers ;


    public AssetHandlerViewModel()
    {
        Handlers = new Dictionary<EAssetType, AssetHandlerData>
        {
            { EAssetType.Character, _characterHandler }
        };

    }

    private readonly AssetHandlerData _characterHandler = new()
    {
        AssetType = EAssetType.Character,
        TargetCollection = AppVM.MainVM.Outfits,
        ClassNames = new List<string> { "Skeleton" },
        IconGetter = UI_Asset =>
        {
            UI_Asset.TryGetValue(out UTexture2D? previewImage, "DisplayIcon");
            return previewImage;
        }
        //IconGetter = UI_Asset =>
        //{
        //UI_Asset.TryGetValue(out UTexture2D? previewImage, "DisplayIcon");
        //return previewImage;
        //}
    };




    
    public async Task Initialize()
    {
        await _characterHandler.Execute(); // default tab
    }
}



public class AssetHandlerData
{
    public bool HasStarted { get; private set; }
    public Pauser PauseState { get; } = new();

    public EAssetType AssetType;
    public ObservableCollection<AssetSelectorItem> TargetCollection;
    public List<string> ClassNames;
    public Func<UObject, UTexture2D?> IconGetter;

    public async Task Execute()
    {
        Console.WriteLine("Executing Loading Handle thing");
        if (HasStarted) return;
        HasStarted = true;
        var items = new List<FAssetData>();
        Console.WriteLine("Loading asset thinge bingie idfk");
        foreach (var variable in AppVM.CUE4ParseVM.AssetRegistry.PreallocatedAssetDataBuffers) //search for Classes in AssetRegistry
        {

            foreach (var tagsAndValue in variable.TagsAndValues)
            {
                if (tagsAndValue.Key.PlainText == "MeshArray") 
                {
                    if(tagsAndValue.Value.ToString().Contains("MeshArray"))
                    {
                        Console.WriteLine(tagsAndValue);
                    }
                    var exist = await AppVM.CUE4ParseVM.Provider.TryLoadObjectAsync(variable.ObjectPath); // check if the model actually exists didn't find any better solution :(
                    if (exist is not null)
                    {
                        Console.WriteLine($"File {variable.AssetName} typ {tagsAndValue}");
                        items.Add(variable);
                    }

                }
                
                
            }
        }
        Console.WriteLine(items.Count);
        await Parallel.ForEachAsync(items, async (data, token) => //load if found
        {
            Console.WriteLine("Loading Following Data");
            Console.WriteLine(data);
            await DoLoadSkeletalMesh(data);
        });
    }

    private async Task DoLoadSkeletalMesh(FAssetData data)
    {
        Console.WriteLine(data.PackageName);
        UObject Asset = new UObject();
        UTexture2D fake = new UTexture2D();
        string stupidname = data.AssetName.ToString();
        Asset = await AppVM.CUE4ParseVM.Provider.TryLoadObjectAsync(data.ObjectPath);
        fake.Name = "kys";
        await Application.Current.Dispatcher.InvokeAsync(() => TargetCollection.Add(new AssetSelectorItem(Asset, Asset, Asset, new UTexture2D(), stupidname, false)), DispatcherPriority.Background);


    }
    private async Task DoLoad(FAssetData data, bool random = false)
    {
        Console.WriteLine("Loading Data");
        await PauseState.WaitIfPaused();
        UObject actualAsset = new UObject();
        UObject uiAsset = new UObject();
        var firstTag = data.ObjectPath;

        if (firstTag.Contains("NPE") || firstTag.Contains("Random")) return;

        actualAsset = await AppVM.CUE4ParseVM.Provider.TryLoadObjectAsync(firstTag);
        Console.WriteLine(firstTag);
        if (actualAsset == null) return;

        var uBlueprintGeneratedClass = actualAsset as UBlueprintGeneratedClass;
        actualAsset = uBlueprintGeneratedClass.ClassDefaultObject.Load();
        var mainA = actualAsset;

        if (actualAsset.TryGetValue(out UBlueprintGeneratedClass uiObject, "UIData"))
        {
            uiAsset = uiObject.ClassDefaultObject.Load();
        }
        // switch on asset type
        string loadable = "None";
        switch (AssetType)
        {
            case EAssetType.Character:

                break;
        }

        if (actualAsset.TryGetValue(out UBlueprintGeneratedClass blueprintObject, loadable))
        {
            actualAsset = blueprintObject.ClassDefaultObject.Load();
        }
        var previewImage = IconGetter(uiAsset);
        if (previewImage is null) return;
        await Application.Current.Dispatcher.InvokeAsync(() => TargetCollection.Add(new AssetSelectorItem(actualAsset, uiAsset, mainA, previewImage,"d", random)), DispatcherPriority.Background);
    }
}