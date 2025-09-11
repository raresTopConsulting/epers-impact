using Epers.Models.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EpersBackend.Services.PDF
{
    public class PdfService : IPdfService
    {
        private readonly IPdfEvaluareDocumentsDataSource _pdfEvaluareDocumentsDataSource;
        private readonly IPdfObiectiveDocumentsDataSource _pdfObiectiveDocumentsDataSource;
        private readonly IPdfEvalareDocumentsConclzuieDataSource _pdfEvalareDocumentsConclzuieDataSource;
        private readonly IPdfPipDocumentsDataSource _pdfPipDocumentsDataSource;

        public PdfEvaluareModel PdfEvaluareModel { get; set; } = new PdfEvaluareModel();
        public PdfObiectiveModel PdfObiectiveModel { get; set; } = new PdfObiectiveModel();
        public PdfPipModel PdfPipModel { get; set; } = new PdfPipModel();
        public PdfEvaluareConcluziiModel PdfEvaluareConcluziiModel { get; set; } = new PdfEvaluareConcluziiModel();

        public PdfService(IPdfEvaluareDocumentsDataSource pdfEvaluareDocumentsDataSource,
            IPdfObiectiveDocumentsDataSource pdfObiectiveDocumentsDataSource,
            IPdfEvalareDocumentsConclzuieDataSource pdfEvalareDocumentsConclzuieDataSource,
            IPdfPipDocumentsDataSource pdfPipDocumentsDataSource)
        {
            _pdfEvaluareDocumentsDataSource = pdfEvaluareDocumentsDataSource;
            _pdfObiectiveDocumentsDataSource = pdfObiectiveDocumentsDataSource;
            _pdfEvalareDocumentsConclzuieDataSource = pdfEvalareDocumentsConclzuieDataSource;
            _pdfPipDocumentsDataSource = pdfPipDocumentsDataSource;
        }

        public byte[] GeneratePDFEvaluare(int idAngajat, int? anul = null)
        {
            PdfEvaluareModel = _pdfEvaluareDocumentsDataSource.GetPdfEvaluareData(idAngajat, anul);

            var documentEvaluare = Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri));

                        page.Header().Element(ComposeHeaderEvaluare);
                        page.Content().DefaultTextStyle(x => x.FontSize(8)).PaddingTop(20).Element(ComposeTableEvaluare);

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                    });
            });
            byte[] pdfBytes = documentEvaluare.GeneratePdf();

            return pdfBytes;
        }

        public byte[] GeneratePDFObiectiveActuale(int idAngajat, int? anul = null)
        {
            PdfObiectiveModel = _pdfObiectiveDocumentsDataSource.GetPdfObActualeData(idAngajat, anul);

            var documentEvaluare = Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri));

                        page.Header().Element(ComposeHeaderObiective);
                        page.Content().DefaultTextStyle(x => x.FontSize(8)).PaddingTop(20).Element(ComposeTableObiectiveActuale);

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                    });
            });
            byte[] pdfBytes = documentEvaluare.GeneratePdf();

            return pdfBytes;
        }

        public byte[] GeneratePDFObiectiveIstoric(int idAngajat, int? anul = null)
        {
            PdfObiectiveModel = _pdfObiectiveDocumentsDataSource.GetPdfObIstoricData(idAngajat, anul);

            var documentEvaluare = Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri));

                        page.Header().Element(ComposeHeaderObiective);
                        page.Content().DefaultTextStyle(x => x.FontSize(8)).PaddingTop(20).Element(ComposeTableObiectiveIstoric);

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                    });
            });
            byte[] pdfBytes = documentEvaluare.GeneratePdf();

            return pdfBytes;
        }

        public byte[] GeneratePDFEvaluareSiConcluzii(int idAngajat, int? anul = null)
        {
            PdfEvaluareConcluziiModel = _pdfEvalareDocumentsConclzuieDataSource.GetPdfEvaluareSiConcluziiData(idAngajat, anul);

            var documentEvaluare = Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri));

                        page.Header().Element(ComposeHeaderEvaluareConcluzii);
                        page.Content().DefaultTextStyle(x => x.FontSize(8)).PaddingTop(20).Element(ComposeEvaluareSiConcluzii);

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                    });
            });
            byte[] pdfBytes = documentEvaluare.GeneratePdf();

            return pdfBytes;
        }

        public byte[] GeneratePDFPip(int idAngajat, int? anul = null)
        {
            PdfPipModel = _pdfPipDocumentsDataSource.GetPdfPipData(idAngajat, anul);

            var documentPip = Document.Create(container =>
            {
                container
                    .Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontFamily(Fonts.Calibri));

                        page.Header().Element(ComposeHeaderPip);
                        page.Content().DefaultTextStyle(x => x.FontSize(12)).PaddingTop(20).Element(ComposeRaportPip);

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                    });
            });
            byte[] pdfBytes = documentPip.GeneratePdf();

            return pdfBytes;
        }

        private void ComposeHeaderPip(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingRight(5).Text("Raport Plan de îmbunătățire a performanței profesionale a salariaților (PIP)" + PdfPipModel.Anul).FontSize(22);
            
                row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(10).Text("Date Angajat").SemiBold();

                    column.Item().Text("Matricola: " + PdfPipModel.Header.Matricola);
                    column.Item().Text("Nume si Prenume: " + PdfPipModel.Header.NumePrenume);
                    column.Item().Text("Post: " + PdfPipModel.Header.DenumirePost);
                    column.Item().Text("Locatie:  " + PdfPipModel.Header.Locatie);
                });
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Date Superior").SemiBold();

                    column.Item().Text("Matricola: " + PdfPipModel.Header.MatricolaSef);
                    column.Item().Text("Nume si Prenume: " + PdfPipModel.Header.NumePrenumeSef);
                    column.Item().Text("Post: " + PdfPipModel.Header.DenumirePostSupervizor);
                    column.Item().Text("Locatie:  " + PdfPipModel.Header.LocatieSupervizor);
                });
            });
        }

        private void ComposeHeaderEvaluare(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingRight(5).Text("Raport Evaluare " + PdfEvaluareModel.Anul).FontSize(26);

                row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(10).Text("Date Angajat").SemiBold();

                    column.Item().Text("Matricola: " + PdfEvaluareModel.Header.Matricola);
                    column.Item().Text("Nume si Prenume: " + PdfEvaluareModel.Header.NumePrenume);
                    column.Item().Text("Post: " + PdfEvaluareModel.Header.DenumirePost);
                    column.Item().Text("Locatie:  " + PdfEvaluareModel.Header.Locatie);
                });
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Date Superior").SemiBold();

                    column.Item().Text("Matricola: " + PdfEvaluareModel.Header.MatricolaSef);
                    column.Item().Text("Nume si Prenume: " + PdfEvaluareModel.Header.NumePrenumeSef);
                    column.Item().Text("Post: " + PdfEvaluareModel.Header.DenumirePostSupervizor);
                    column.Item().Text("Locatie:  " + PdfEvaluareModel.Header.LocatieSupervizor);
                });
            });
        }

        private void ComposeHeaderEvaluareConcluzii(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingRight(5).Text("Raport Evaluare " + PdfEvaluareConcluziiModel.Anul).FontSize(26);

                row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(10).Text("Date Angajat").SemiBold();

                    column.Item().Text("Matricola: " + PdfEvaluareConcluziiModel.Header.Matricola);
                    column.Item().Text("Nume si Prenume: " + PdfEvaluareConcluziiModel.Header.NumePrenume);
                    column.Item().Text("Post: " + PdfEvaluareConcluziiModel.Header.DenumirePost);
                    column.Item().Text("Locatie:  " + PdfEvaluareConcluziiModel.Header.Locatie);
                });
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Date Superior").SemiBold();

                    column.Item().Text("Matricola: " + PdfEvaluareConcluziiModel.Header.MatricolaSef);
                    column.Item().Text("Nume si Prenume: " + PdfEvaluareConcluziiModel.Header.NumePrenumeSef);
                    column.Item().Text("Post: " + PdfEvaluareConcluziiModel.Header.DenumirePostSupervizor);
                    column.Item().Text("Locatie:  " + PdfEvaluareConcluziiModel.Header.LocatieSupervizor);
                });
            });
        }

        private void ComposeHeaderObiective(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().PaddingRight(5).Text("Raport Obiective " + PdfObiectiveModel.Anul).FontSize(26);

                row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(10).Text("Date Angajat").SemiBold();

                    column.Item().Text("Matricola: " + PdfObiectiveModel.Header.Matricola);
                    column.Item().Text("Nume si Prenume: " + PdfObiectiveModel.Header.NumePrenume);
                    column.Item().Text("Post: " + PdfObiectiveModel.Header.DenumirePost);
                    column.Item().Text("Locatie:  " + PdfObiectiveModel.Header.Locatie);
                });
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Spacing(2);

                    column.Item().BorderBottom(1).PaddingBottom(5).Text("Date Superior").SemiBold();

                    column.Item().Text("Matricola: " + PdfObiectiveModel.Header.MatricolaSef);
                    column.Item().Text("Nume si Prenume: " + PdfObiectiveModel.Header.NumePrenumeSef);
                    column.Item().Text("Post: " + PdfObiectiveModel.Header.DenumirePostSupervizor);
                    column.Item().Text("Locatie:  " + PdfObiectiveModel.Header.LocatieSupervizor);
                });
            });
        }

        private void ComposeTableObiectiveActuale(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Tip");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Den.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Dt. Încep.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Frecv.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Dt. Sf.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. min.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus min.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. target");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus target");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. max.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus max.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Ob. Calitativ");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold()).BorderTop(1).BorderBottom(2).BorderColor(Colors.Black).Background(Colors.Grey.Lighten2);
                    }
                });

                foreach (var ob in PdfObiectiveModel.DateObiective)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Tip == "I" ? "Indiv." : ob.Tip == "D" ? "Dep." : "Corp.");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Denumire);
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.DataIn.HasValue ? ob.DataIn.Value.ToString("dd/MM/yyyy") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Frecventa);
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.DataSf.HasValue ? ob.DataSf.Value.ToString("dd/MM/yyyy") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValMin.HasValue ? ob.ValMin.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusMin.HasValue ? string.Concat(ob.BonusMin.Value.ToString(), ob.IsBonusProcentual == true ? " %" : " RON") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValTarget.HasValue ? ob.ValTarget.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusTarget.HasValue ? string.Concat(ob.BonusTarget.Value.ToString(), ob.IsBonusProcentual == true ? " %" : " RON") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValMax.HasValue ? ob.ValMax.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusMax.HasValue ? string.Concat(ob.BonusMax.Value.ToString(), ob.IsBonusProcentual == true ? " %" : " RON") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.IsFaraProcent == true ? "DA" : "NU");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                }
            });
        }

        private void ComposeTableObiectiveIstoric(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Tip");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Den.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Dt. Încep.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Frecv.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Dt. Sf.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. min.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus min.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. target");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus target");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Val. max.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Bonus max.");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Ob. Calitativ");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Real.");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold()).BorderTop(1).BorderBottom(2).BorderColor(Colors.Black).Background(Colors.Grey.Lighten2);
                    }
                });

                foreach (var ob in PdfObiectiveModel.DateObiective)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Tip == "I" ? "Indiv." : ob.Tip == "D" ? "Dep." : "Corp.");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Denumire);
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.DataIn.HasValue ? ob.DataIn.Value.ToString("dd/MM/yyyy") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.Frecventa);
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.DataSf.HasValue ? ob.DataSf.Value.ToString("dd/MM/yyyy") : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValMin.HasValue ? ob.ValMin.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusMin.HasValue ? ob.BonusMin.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValTarget.HasValue ? ob.ValTarget.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusTarget.HasValue ? ob.BonusTarget.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.ValMax.HasValue ? ob.ValMax.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.BonusMax.HasValue ? ob.BonusMax.Value.ToString() : "");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.IsFaraProcent == true ? "DA" : "NU");
                    table.Cell().Element(CellStyle).AlignLeft().Text(ob.IsRealizat == true ? "DA" : "NU");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                }
            });
        }

        private void ComposeTableEvaluare(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    // columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    // columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Competenta");
                    // header.Cell().Element(CellStyle).AlignLeft().Text("Detalii");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Autoevaluare");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Eval. Superior");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Eval. Finală");
                    // header.Cell().Element(CellStyle).AlignLeft().Text("Ideal");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Observatii");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold()).BorderTop(1).BorderBottom(2).BorderColor(Colors.Black).Background(Colors.Grey.Lighten2);
                    }
                });

                foreach (var eval in PdfEvaluareModel.DateEvaluare.DateEval)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.DenumireSkill);
                    // table.Cell().Element(CellStyle).AlignLeft().Text(eval.DetaliiSkill);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.ValIndiv.HasValue ? eval.ValIndiv.Value.ToString() : string.Empty);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.Val.HasValue ? eval.Val.Value.ToString() : string.Empty);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.ValFin.HasValue ? eval.ValFin.Value.ToString() : string.Empty);
                    // table.Cell().Element(CellStyle).AlignLeft().Text(eval.Ideal.ToString());
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.Obs);
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                }
            });
        }

        private void ComposeTableEvaluareSiConcluzii(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    // columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    // columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Competenta");
                    // header.Cell().Element(CellStyle).AlignLeft().Text("Detalii");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Autoevaluare");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Eval. Superior");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Eval. Finală");
                    // header.Cell().Element(CellStyle).AlignLeft().Text("Ideal");
                    header.Cell().Element(CellStyle).AlignLeft().Text("Observatii");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold()).BorderTop(1).BorderBottom(2).BorderColor(Colors.Black).Background(Colors.Grey.Lighten2);
                    }
                });

                foreach (var eval in PdfEvaluareConcluziiModel.DateEvaluare.DateEval)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.DenumireSkill);
                    // table.Cell().Element(CellStyle).AlignLeft().Text(eval.DetaliiSkill);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.ValIndiv.HasValue ? eval.ValIndiv.Value.ToString() : string.Empty);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.Val.HasValue ? eval.Val.Value.ToString() : string.Empty);
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.ValFin.HasValue ? eval.ValFin.Value.ToString() : string.Empty);
                    // table.Cell().Element(CellStyle).AlignLeft().Text(eval.Ideal.ToString());
                    table.Cell().Element(CellStyle).AlignLeft().Text(eval.Obs);
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1);
                    }
                }
            });
        }

        private void ComposeConluzii(IContainer container)
        {
            if (PdfEvaluareConcluziiModel.ConclzuieEvaluare != null)
            {
                var concluzii = PdfEvaluareConcluziiModel.ConclzuieEvaluare;

                container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
                {
                    column.Spacing(5);
                    column.Item().Text("Concluzii despre rezultatele evaluării competențelor și despre activitățile de dezvoltare profesională").FontSize(10).Bold();
                    column.Item().Text(!string.IsNullOrWhiteSpace(concluzii.ConcluziiEvalCompActDezProf) ? concluzii.ConcluziiEvalCompActDezProf.ToString() : "");
                    column.Spacing(5);
                    column.Item().Text("Concluzii despre evaluarea cantitativă a obiectivelor și despre setarea noilor obiective").FontSize(10).Bold();
                    column.Item().Text(!string.IsNullOrWhiteSpace(concluzii.ConcluziiEvalCantOb) ? concluzii.ConcluziiEvalCantOb.ToString() : "");
                    column.Spacing(5);
                    column.Item().Text("Așteptări angajat și aspecte generale").FontSize(10).Bold();
                    column.Item().Text(!string.IsNullOrWhiteSpace(concluzii.ConcluziiAspecteGen) ? concluzii.ConcluziiAspecteGen.ToString() : "");

                    var trainiguri = PdfEvaluareConcluziiModel.ListaTrainiguri;
                    if (trainiguri.Count() > 0)
                    {
                        column.Spacing(2);
                        column.Item().Text("Cursuri recomandate: ").FontSize(10).Bold();

                        foreach (var trainig in trainiguri)
                        {
                            column.Item().Text("Denumire: " + trainig.Denumire);
                            if (!string.IsNullOrWhiteSpace(trainig.Organizator))
                                column.Item().Text("Organizator: " + trainig.Organizator);

                            if (trainig.Pret.HasValue)
                                column.Item().Text("Pret: " + trainig.Pret.ToString());

                            if (!string.IsNullOrWhiteSpace(trainig.Locatie))
                                column.Item().Text("Locatie: " + trainig.Locatie);

                            if (!string.IsNullOrWhiteSpace(trainig.Link))
                                column.Item().Text("Link: " + trainig.Link);
                        }
                    }
                });
            }
        }

        private void ComposeEvaluareSiConcluzii(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);
                column.Item().Element(ComposeTableEvaluareSiConcluzii);

                if (PdfEvaluareConcluziiModel.ConclzuieEvaluare != null)
                {
                    column.Item().PaddingTop(25).Element(ComposeConluzii);
                }
            });
        }

        private void ComposeRaportPip(IContainer container)
        {
            if (PdfPipModel.PlanInbunatatirePerformante != null)
            {
                var pipData = PdfPipModel.PlanInbunatatirePerformante;

                container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
                {
                    column.Item().Text("Date Evaluare Competențe: ").SemiBold();

                    column.Spacing(4);

                    if (pipData.DataInceputEvaluare.HasValue && pipData.DataSfarsitEvaluare.HasValue)
                    {
                        column.Item().Text("Perioada de desfășurare a evaluării profesionale: "
                            + pipData.DataInceputEvaluare.Value.ToString("dd/MM/yyyy") + " - " + pipData.DataSfarsitEvaluare.Value.ToString("dd/MM/yyyy"));
                    }
                    column.Item().Text("Califiactivul obținut în urma evaluării profesionale: " + pipData.CalificativEvaluare);

                    column.Item().BorderBottom(1).PaddingBottom(5);

                    column.Item().Text("Date  Plan de îmbunătățire a performanței profesionale a salariaților: ").SemiBold();

                    column.Item().Text("Observații departament HR: ").SemiBold();
                    column.Item().Text(pipData.ObservatiiHr);

                    column.Item().Text("Data start PIP: " + pipData.DataInceputPip.ToString("dd/MM/yyyy")).SemiBold();
                    column.Item().Text("Data finalizare PIP: " + pipData.DataSfarsitPip.ToString("dd/MM/yyyy")).SemiBold();
                    column.Item().Text("Calificativ minim așteptat după finalizarea PIP: " + pipData.CalificativMinimPip).SemiBold();

                    column.Item().Text("Obiectivele de dezvoltare ale salariatului pentru îmbunătățirea performanței: ").SemiBold();
                    column.Item().Text(pipData.ObiectiveDezvoltare);
                    column.Item().PaddingBottom(2);

                    column.Item().Text("Acțiunile specifice pentru atingerea obiectivelor de dezvoltare: ").SemiBold();
                    column.Item().Text(pipData.ActiuniSalariat);
                    column.Item().PaddingBottom(2);

                    column.Item().Text("Suportul oferit de Managerul direct: ").SemiBold();
                    column.Item().Text(pipData.SuportManager);
                    column.Item().PaddingBottom(2);

                    column.Item().Text("Alte tipuri de suport oferit de Societate pentru dezvoltarea salariatului: ").SemiBold();
                    column.Item().Text(pipData.AltSuport);
                    column.Item().PaddingBottom(2);

                    column.Item().BorderBottom(1).PaddingBottom(8);
                    column.Item().Text("Evaluare finalizare PIP").SemiBold();
                    column.Item().PaddingBottom(2);

                    if (pipData.CalificativFinalPip.HasValue)
                        column.Item().Text("Calificativ obținut la finalizarea PIP: " + pipData.CalificativFinalPip.Value).SemiBold();
                    if (pipData.ObservatiFinalPip != null)
                        column.Item().Text("Observații Manager direct pe perioada PIP: " + pipData.ObservatiFinalPip).SemiBold();
                    if (pipData.DeczieFinalaManager != null)
                        column.Item().Text("Decizie finală Manager direct: " + pipData.DeczieFinalaManager).SemiBold();

                    if (pipData.DenumireStare != null) {
                        column.Item().Text("Stare: " + pipData.DenumireStare).SemiBold();
                    }
                });
            }
        }
    }
}
