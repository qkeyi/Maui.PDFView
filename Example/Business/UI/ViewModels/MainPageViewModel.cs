﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Example.Business.Services;
using Maui.PDFView.Events;
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
            System.Diagnostics.Debug.WriteLine($"Текущая страница: {args.CurrentPage} из {args.TotalPages}");
        }
    }
}
