﻿using Foundation;
using Maui.PDFView.Events;
using Microsoft.Maui.Handlers;
using PdfKit;
using UIKit;

namespace Maui.PDFView.Platforms.iOS
{
    internal class PdfViewHandler : ViewHandler<IPdfView, PdfKit.PdfView>
    {
        private readonly static PropertyMapper<PdfView, PdfViewHandler> PropertyMapper = new(ViewMapper)
        {
            [nameof(IPdfView.Uri)] = MapUri,
            [nameof(IPdfView.IsHorizontal)] = MapIsHorizontal,
            [nameof(IPdfView.MaxZoom)] = MapMaxZoom
        };

        private string _fileName;
        private NSObject? _pageChangedObserver;

        public PdfViewHandler() : base(PropertyMapper, null)
        {
        }

        static void MapUri(PdfViewHandler handler, IPdfView pdfView)
        {
            handler._fileName = pdfView.Uri;
            handler.RenderPages();
        }

        static void MapIsHorizontal(PdfViewHandler handler, IPdfView pdfView)
        {
            handler.PlatformView.DisplayDirection = pdfView.IsHorizontal
                                        ? PdfDisplayDirection.Horizontal
                                        : PdfDisplayDirection.Vertical;
        }

        static void MapMaxZoom(PdfViewHandler handler, IPdfView pdfView)
        {
            // reset MaxScaleFactor inside RenderPages
            handler.RenderPages();
        }

        protected override PdfKit.PdfView CreatePlatformView()
        {
            var pdfView = new PdfKit.PdfView();

            // Subscribe to notification of page changes
            _pageChangedObserver = NSNotificationCenter.DefaultCenter.AddObserver(
                PdfKit.PdfView.PageChangedNotification, 
                PageChangedNotificationHandler, 
                pdfView);

            return pdfView;
        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            RenderPages();
            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        void RenderPages()
        {
            if (string.IsNullOrEmpty(_fileName) || PlatformView == null)
                return;

            PlatformView.Document?.Dispose();

            PlatformView.Document = new PdfDocument(NSData.FromFile(_fileName));

            PlatformView.AutosizesSubviews = true;
            PlatformView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
            PlatformView.DisplayMode = PdfDisplayMode.SinglePageContinuous;
            PlatformView.DisplaysPageBreaks = true;

            PlatformView.MaxScaleFactor = VirtualView.MaxZoom;
            //PlatformView.MinScaleFactor = PlatformView.ScaleFactorForSizeToFit;
            PlatformView.MinScaleFactor = (nfloat)(UIScreen.MainScreen.Bounds.Height * 0.00075);

            PlatformView.AutoScales = true;
        }

        private void PageChangedNotificationHandler(NSNotification notification)
        {
            var currentPage = PlatformView.CurrentPage;
            if (currentPage is null)
                return;

            var document = PlatformView.Document;
            if (document is null)
                return;

            if (!(VirtualView.PageChangedCommand?.CanExecute(null) ?? false)) 
                return;

            VirtualView.PageChangedCommand.Execute(new PageChangedEventArgs((int)(document.GetPageIndex(currentPage) + 1), (int)document.PageCount));
        }

        protected override void DisconnectHandler(PdfKit.PdfView platformView)
        {
            if (_pageChangedObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_pageChangedObserver);
                _pageChangedObserver.Dispose();
                _pageChangedObserver = null;
            }

            if (platformView != null)
            {
                platformView.Document?.Dispose();
                platformView.Dispose();
            }
            base.DisconnectHandler(platformView);
        }
    }
}
