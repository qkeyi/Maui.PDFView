# Maui.PDFView
Library for display PDF files in .NET MAUI on Android, iOS, MacOS and Windows

![NuGet Downloads](https://img.shields.io/nuget/dt/Vitvov.Maui.PDFView?style=for-the-badge)
![GitHub License](https://img.shields.io/github/license/vitalii-vov/Maui.PDFView?style=for-the-badge)
![last commit](https://img.shields.io/github/last-commit/vitalii-vov/Maui.PDFView?style=for-the-badge)

| .NET MAUI | .NET 8   | .NET 9   |
| :-------- | :------- | :------- |

| Platform  | Android | iOS | MacOS | Windows |
| :-------- | :-----  | :-- | :---- | :------ |
| Supported | ✅      | ✅   | ✅    | ✅      |

https://github.com/vitalii-vov/Maui.PDFView/assets/71486507/4977ede8-c8db-454f-930d-ba2ec704f16d


&nbsp;<br>
## Installation
```
Install-Package Vitvov.Maui.PDFView
```

&nbsp;<br>
## Usage
Add `.UseMauiPdfView()` to MauiProgram
```C#
using Maui.PDFView;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiPdfView();   // <- Write this

        return builder.Build();
    }
}
```

&nbsp;<br>
Add `PdfView` to XAML
```xaml
<ContentPage
    x:Class="Example.Business.UI.Pages.MainPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:pdf="clr-namespace:Maui.PDFView;assembly=Maui.PDFView">

    <!--
    IsHorizontal — display PDF horizontally
    Uri — path to the file on the device
    MaxZoom — max zoom level
    PageChangedCommand — event of changing the current page
    -->
    <pdf:PdfView
        IsHorizontal="{Binding IsHorizontal}"
        Uri="{Binding PdfSource}"
        MaxZoom="4"
        PageChangedCommand="{Binding PageChangedCommand}">
    </pdf:PdfView>

</ContentPage>
```

> [!IMPORTANT]
> To use a component with `.net9` add `HandlerProperties.DisconnectPolicy="Manual"` to `PdfView`
> ```XAML
> <pdf:PdfView
>     HandlerProperties.DisconnectPolicy="Manual" />
> ```

&nbsp;<br>
Set `PdfSource` in ViewModel
```C#
internal partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private string _pdfSource;

    [RelayCommand] private void ChangeUri()
    {
        try 
        {
            //  See the example project to understand how to work with paths.
            PdfSource = "/path/to/file.pdf";
        }
        catch(Exception ex)
        {
             // handle exceptions
        }
    }
}
```

&nbsp;<br>
## Personalization
You can customize the way pages are displayed by modifying the `PageAppearance` property in the `PdfView` component.
```xaml
<pdf:PdfView Uri="{Binding PdfSource}">

    <!--
    Margin — Adds indents around pages
    ShadowEnabled — Adds a shadow to each page
    Crop — crops out the part around the page
    -->
    <pdf:PdfView.PageAppearance>
        <pdf:PageAppearance 
            Margin="16,8" 
            ShadowEnabled="True"
            Crop="64,32"/>
    </pdf:PdfView.PageAppearance>

</pdf:PdfView>
```
