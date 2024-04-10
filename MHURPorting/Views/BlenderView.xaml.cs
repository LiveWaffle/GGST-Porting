using System.Windows;
using GGSTPorting.ViewModels;
using GGSTPorting.AppUtils;
using GGSTPorting.Services;

namespace GGSTPorting.Views;

public partial class BlenderView
{
    public BlenderView()
    {
        InitializeComponent();
        AppVM.BlenderVM = new BlenderViewModel();
        DataContext = AppVM.BlenderVM;
    }

}