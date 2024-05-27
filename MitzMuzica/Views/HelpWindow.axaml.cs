/**************************************************************************
 *                                                                        *
 *  Description: Help windows pdf rendering code                          *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Panciuc Ilie Cosmin                            *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/
using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using MuPDFCore.MuPDFRenderer;
using MuPDFCore;

namespace MitzMuzica.Views;

public partial class HelpWindow : Window
{
    private int _pageNumber = 0;
    private readonly int _pageCount = 3;
    private readonly PDFRenderer _pdfRenderer;
    private readonly MemoryStream _pdfContent = new ();
    
    public HelpWindow()
    {
        InitializeComponent();
        _pdfRenderer = this.FindControl<PDFRenderer>("MuPDFRenderer")!;
        var stream= AssetLoader.Open(new Uri("avares://MitzMuzica/Assets/DOCS_C_.pdf"));
        stream.CopyTo(_pdfContent);
    }
    
    private void WindowOpened(object sender, EventArgs e)
    {
        UpdatePdf();
    }
    
    private void PreviousPage_Click(object? sender, RoutedEventArgs e)
    {
        if (_pageNumber > 0)
        {
            _pageNumber--;
            UpdatePdf();
        }

    }

    private void NextPage_Click(object? sender, RoutedEventArgs e)
    {
        if (_pageNumber < _pageCount)
        {
            _pageNumber++;
            UpdatePdf();
        }

    }

    private void UpdatePdf()
    {
        _pdfRenderer.Initialize(_pdfContent, InputFileTypes.PDF, 1, _pageNumber);
    }
}

