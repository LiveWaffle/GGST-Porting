﻿using System;
using System.Linq;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse_Conversion.Textures;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using GGSTPorting.Export;
using GGSTPorting.ViewModels;
using SharpGLTF.Schema2;
using SkiaSharp;

namespace GGSTPorting.Views.Controls;

public partial class StyleSelector
{
    public UObject OptionUsed;

    public StyleSelector(UObject[] options, UObject[] actualObjects )
    {
        InitializeComponent();
        DataContext = this;
        for (int i = 0; i < options.Length; i++)
        {
            UObject uiData = options[i];
            uiData.TryGetValue(out FText channel, "DisplayName");
            if (!uiData.TryGetValue(out UTexture2D previewTexture, "Swatch")) return;
            var previewBitmap = previewTexture.Decode();
            if (previewBitmap is null) continue;

            var fullBitmap = new SKBitmap(previewBitmap.Width, previewBitmap.Height, previewBitmap.ColorType,
                previewBitmap.AlphaType);
            using (var fullCanvas = new SKCanvas(fullBitmap))
            {
                DrawBackground(fullCanvas, Math.Max(previewBitmap.Width, previewBitmap.Height));
                fullCanvas.DrawBitmap(previewBitmap, 0, 0);
            }

            Options.Items.Add(new StyleSelectorItem(actualObjects[i],uiData,channel.ToString(), fullBitmap));
        }

        Options.SelectedIndex = 0;
    }



    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Options.SelectedItem is not StyleSelectorItem selectedItem) return;
    }
    
    private void DrawBackground(SKCanvas canvas, int size)
    {
        SKShader BackgroundShader(params SKColor[] colors)
        {;
            return SKShader.CreateRadialGradient(new SKPoint(size / 2f, size / 2f), size / 5 * 4, colors,
                SKShaderTileMode.Clamp);
        }

        canvas.DrawRect(new SKRect(0, 0, size, size), new SKPaint
        {
            Shader = BackgroundShader(SKColor.Parse("#50C8FF"), SKColor.Parse("#1B7BCF"))
        });
    }
}