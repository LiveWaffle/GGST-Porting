using System.Collections.Generic;
using System.IO;
using CUE4Parse.UE4.Versions;

namespace GGSTPorting.Services.Endpoints;

public class GGSTPortingFileProvider : CustomFileProvider
{
    public GGSTPortingFileProvider(bool isCaseInsensitive = false, VersionContainer? versions = null) : base(isCaseInsensitive, versions)
    {
        
    }

    public GGSTPortingFileProvider(DirectoryInfo mainDirectory, List<DirectoryInfo> extraDirectories, SearchOption searchOption, bool isCaseInsensitive = false, VersionContainer? versions = null) : base(mainDirectory, extraDirectories, searchOption, isCaseInsensitive, versions)
    {
    }
    
    public GGSTPortingFileProvider(DirectoryInfo mainDirectory, SearchOption searchOption, bool isCaseInsensitive = false, VersionContainer? versions = null) : base(mainDirectory, searchOption, isCaseInsensitive, versions)
    {
    }
}