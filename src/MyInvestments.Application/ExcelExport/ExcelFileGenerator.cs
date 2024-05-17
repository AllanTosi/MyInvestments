using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MyInvestments.Ativos;
using MyInvestments.ClasseAtivos;
using MyInvestments.Operacoes;
using MyInvestments.Setores;
using MyInvestments.TipoTransacoes;

namespace MyInvestments.ExcelExport
{
    public static class ExcelFileGenerator
    {
        public static byte[] GenerateExcelFile()
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var row1 = new Row();
            row1.AppendChild(
                new Cell
                {
                    CellValue = new CellValue("Abp Framework"),
                    DataType = CellValues.String
                }

            );

            sheetData?.AppendChild(row1);

            var row2 = new Row();
            row2.AppendChild(
                new Cell
                {
                    CellValue = new CellValue("Open Source"),
                    DataType = CellValues.String
                }
            );
            sheetData?.AppendChild(row2);

            var row3 = new Row();
            row3.AppendChild(
                new Cell
                {
                    CellValue = new CellValue("WEB APPLICATION FRAMEWORK"),
                    DataType = CellValues.String
                }
            );
            sheetData?.AppendChild(row3);

            document.Save();

            return memoryStream.ToArray();
        }

        public static byte[] GenerateExcelFileAtivos(List<AtivoDto> lAtivo)
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Adicionar cabeçalhos (se necessário)
            var headerRow = new Row();

            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Ticker"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Nome"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Descricao"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Setor"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Classe Ativo"),
                DataType = CellValues.String
            });

            sheetData.AppendChild(headerRow);

            foreach (var ativo in lAtivo)
            {
                var row = new Row();

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Ticker),
                        DataType = CellValues.String
                    });

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Nome),
                        DataType = CellValues.String
                    });

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Descricao),
                        DataType = CellValues.String
                    });

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Setor.Descricao),
                        DataType = CellValues.String
                    });

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.ClasseAtivo.Nome),
                        DataType = CellValues.String
                    });

                sheetData?.AppendChild(row);
            }

            document.Save();

            return memoryStream.ToArray();
        }

        public static byte[] GenerateExcelFileTipoTransacoes(List<TipoTransacaoDto> lTipoTransacao)
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Adicionar cabeçalhos (se necessário)
            var headerRow = new Row();

            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Descricao"),
                DataType = CellValues.String
            });

            sheetData.AppendChild(headerRow);

            foreach (var ativo in lTipoTransacao)
            {
                var row = new Row();

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Descricao),
                        DataType = CellValues.String
                    });

                sheetData?.AppendChild(row);
            }

            document.Save();

            return memoryStream.ToArray();
        }

        public static byte[] GenerateExcelFileSetores(List<SetorDto> lSetores)
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Adicionar cabeçalhos (se necessário)
            var headerRow = new Row();

            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Descricao"),
                DataType = CellValues.String
            });

            sheetData.AppendChild(headerRow);

            foreach (var ativo in lSetores)
            {
                var row = new Row();

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Descricao),
                        DataType = CellValues.String
                    });

                sheetData?.AppendChild(row);
            }

            document.Save();

            return memoryStream.ToArray();
        }

        public static byte[] GenerateExcelFileClasseAtivos(List<ClasseAtivoDto> lClasseAtivo)
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Adicionar cabeçalhos (se necessário)
            var headerRow = new Row();

            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Nome"),
                DataType = CellValues.String
            });

            sheetData.AppendChild(headerRow);

            foreach (var ativo in lClasseAtivo)
            {
                var row = new Row();

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(ativo.Nome),
                        DataType = CellValues.String
                    });

                sheetData?.AppendChild(row);
            }

            document.Save();

            return memoryStream.ToArray();
        }

        public static byte[] GenerateExcelFileOperacoes(List<OperacaoDto> lOperacoes)
        {
            var memoryStream = new MemoryStream();

            using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());

            sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet 1"
            });

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Adicionar cabeçalhos (se necessário)
            var headerRow = new Row();

            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("DataOperacao"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Quantidade"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("Preco"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("ValorEmulumento"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("ValorIrpf"),
                DataType = CellValues.String
            });
            headerRow.AppendChild(new Cell
            {
                CellValue = new CellValue("ValorCorretagem"),
                DataType = CellValues.String
            });

            sheetData.AppendChild(headerRow);

            foreach (var item in lOperacoes)
            {
                var row = new Row();

                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.DataOperacao),
                        DataType = CellValues.Date
                    });
                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.Quantidade),
                        DataType = CellValues.Number
                    });
                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.Preco),
                        DataType = CellValues.Number
                    });
                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.ValorEmulumento),
                        DataType = CellValues.Number
                    });
                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.ValorIrpf),
                        DataType = CellValues.Number
                    });
                row.AppendChild(
                    new Cell
                    {
                        CellValue = new CellValue(item.ValorCorretagem),
                        DataType = CellValues.Number
                    });

                sheetData?.AppendChild(row);
            }

            document.Save();

            return memoryStream.ToArray();
        }
    }
}