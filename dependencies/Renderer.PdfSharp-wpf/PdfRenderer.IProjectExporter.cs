﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
#if WPF
using System.Windows.Media.Imaging;
#endif

#if WPF
namespace Renderer.PdfSharp_wpf
#elif CORE
namespace Renderer.PdfSharp_core
#endif
{
    /// <summary>
    /// PdfSharp pdf <see cref="Core2D.Interfaces.IProjectExporter"/> implementation.
    /// </summary>
    public partial class PdfRenderer : Core2D.Renderer.ShapeRenderer, Core2D.Interfaces.IProjectExporter
    {
        /// <inheritdoc/>
        void Core2D.Interfaces.IProjectExporter.Save(string path, Core2D.Project.XContainer container)
        {
            using (var pdf = new PdfDocument())
            {
                Add(pdf, container);
                pdf.Save(path);
            }
        }

        /// <inheritdoc/>
        void Core2D.Interfaces.IProjectExporter.Save(string path, Core2D.Project.XDocument document)
        {
            using (var pdf = new PdfDocument())
            {
                var documentOutline = default(PdfOutline);

                foreach (var container in document.Pages)
                {
                    var page = Add(pdf, container);

                    if (documentOutline == null)
                    {
                        documentOutline = pdf.Outlines.Add(
                            document.Name,
                            page,
                            true,
                            PdfOutlineStyle.Regular,
                            XColors.Black);
                    }

                    documentOutline.Outlines.Add(
                        container.Name,
                        page,
                        true,
                        PdfOutlineStyle.Regular,
                        XColors.Black);
                }

                pdf.Save(path);
                ClearCache(isZooming: false);
            }
        }

        /// <inheritdoc/>
        void Core2D.Interfaces.IProjectExporter.Save(string path, Core2D.Project.XProject project)
        {
            using (var pdf = new PdfDocument())
            {
                var projectOutline = default(PdfOutline);

                foreach (var document in project.Documents)
                {
                    var documentOutline = default(PdfOutline);

                    foreach (var container in document.Pages)
                    {
                        var page = Add(pdf, container);

                        if (projectOutline == null)
                        {
                            projectOutline = pdf.Outlines.Add(
                                project.Name,
                                page,
                                true,
                                PdfOutlineStyle.Regular,
                                XColors.Black);
                        }

                        if (documentOutline == null)
                        {
                            documentOutline = projectOutline.Outlines.Add(
                                document.Name,
                                page,
                                true,
                                PdfOutlineStyle.Regular,
                                XColors.Black);
                        }

                        documentOutline.Outlines.Add(
                            container.Name,
                            page,
                            true,
                            PdfOutlineStyle.Regular,
                            XColors.Black);
                    }
                }

                pdf.Save(path);
                ClearCache(isZooming: false);
            }
        }

        private PdfPage Add(PdfDocument pdf, Core2D.Project.XContainer container)
        {
            // Create A3 page size with Landscape orientation.
            PdfPage pdfPage = pdf.AddPage();
            pdfPage.Size = PageSize.A3;
            pdfPage.Orientation = PageOrientation.Landscape;

            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage))
            {
                // Calculate x and y page scale factors.
                double scaleX = pdfPage.Width.Value / container.Template.Width;
                double scaleY = pdfPage.Height.Value / container.Template.Height;
                double scale = Math.Min(scaleX, scaleY);

                // Set scaling function.
                _scaleToPage = (value) => value * scale;

                // Draw container template contents to pdf graphics.
                if (container.Template.Background.A > 0)
                {
                    Fill(gfx, 0, 0, pdfPage.Width.Value / scale, pdfPage.Height.Value / scale, container.Template.Background);
                }

                // Draw template contents to pdf graphics.
                Draw(gfx, container.Template, 0.0, 0.0, container.Data.Properties, container.Data.Record);

                // Draw page contents to pdf graphics.
                Draw(gfx, container, 0.0, 0.0, container.Data.Properties, container.Data.Record);
            }

            return pdfPage;
        }
    }
}
