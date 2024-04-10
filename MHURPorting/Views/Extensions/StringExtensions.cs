﻿using System.Globalization;

namespace GGSTPorting.Views.Extensions;

public static class StringExtensions
{
    public static string TitleCase(this string text)
    {
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text);
    }
}