﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Example.Business.Services;
using Example.Business.UI.Pages;
using Maui.PDFView.Events;
using System.Diagnostics;
using System.Windows.Input;

namespace Example.Business.UI.ViewModels
{
    internal partial class MainPageViewModel : ObservableObject
    {
        private readonly IRepositoryService _repository = new RepositoryService();

        [ObservableProperty] private string _pdfSource;
        [ObservableProperty] private bool _isHorizontal;
        [ObservableProperty] private float _maxZoom = 4;

        [RelayCommand] private void Appearing()
        {
            ChangeUri();
        }

        [RelayCommand] private void ChangeUri()
        {
            PdfSource = _repository.GetPdfSource();
        }

        [RelayCommand] private void PageChanged(PageChangedEventArgs args)
        {
            Debug.WriteLine($"Current page: {args.CurrentPage} of {args.TotalPages}");
        }

        [RelayCommand] private void OpenNewPage()
        {
            App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}
