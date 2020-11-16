using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using SoPechExcel.Constatnts;
using Microsoft.Office.Interop.Excel;

namespace SoPechExcel.Files
{
    public class ExcelReport
    {
        private string FileA { get; set; }
        private List<string> FilesB { get; set; }
        private int SelectedYear { get; set; }
        private int SelectedMonth { get; set; }

        public ExcelReport(string fileA, List<string> filesB, int year, int month)
        {
            FileA = fileA;
            FilesB = filesB;
            SelectedYear = year;
            SelectedMonth = month;
        }

        public void Proceed()
        {
            using (var excelPackageA = new ExcelPackage(new FileInfo(FileA)))
            {
                ExcelWorkbook excelWorkBookA = excelPackageA.Workbook;
                ExcelWorksheet excelWorksheetA = excelWorkBookA.Worksheets[Month.ShortMonth[SelectedMonth - 1]];

                if (excelWorksheetA == null)
                {
                    throw new Exception($"Can not get sheet name '{Month.ShortMonth[SelectedMonth - 1]}'");
                }
                int startRow = 7;
                int endRow = 27;

                foreach (var fileB in FilesB)
                {
                    ConvertToXlsx(fileB);
                    string fileBName = fileB + "x";

                    using (var excelPackageB = new ExcelPackage(new FileInfo(fileBName)))
                    {
                        ExcelWorkbook excelWorkBookB = excelPackageB.Workbook;
                        ExcelWorksheet excelWorksheetB = excelWorkBookB.Worksheets.First();

                        if (!excelWorksheetB.Cells[2, 1].Value.ToString().Contains($"{SelectedMonth}.{SelectedYear}"))
                        {
                            throw new Exception($"'{fileBName}' is not '{SelectedMonth}.{SelectedYear}'");
                        }
                        int startIndex = FileA.LastIndexOf('(') + 1;
                        int endIndex = FileA.LastIndexOf(')');

                        if (excelWorksheetB.Cells[6, 4].Value != null && !excelWorksheetB.Cells[6, 4].Value.ToString().Contains(FileA.Substring(startIndex, endIndex - startIndex)))
                        {
                            throw new Exception($"'{fileBName}' is not '{FileA.Substring(startIndex, endIndex - startIndex)}'");
                        }
                        else if (excelWorksheetB.Cells[7, 4].Value != null && !excelWorksheetB.Cells[7, 4].Value.ToString().Contains(FileA.Substring(startIndex, endIndex - startIndex)))
                        {
                            throw new Exception($"'{fileBName}' is not '{FileA.Substring(startIndex, endIndex - startIndex)}'");
                        }

                        excelWorksheetB.Cells[excelWorksheetB.Cells[6, 4].Value != null ? 11 : 12, 3, excelWorksheetB.Cells[6, 4].Value != null ? 31 : 32, 3].Copy(excelWorksheetA.Cells[startRow, 21, endRow, 21]); // COPY C12-C32 To U7-U27 and ...
                        excelWorksheetB.Cells[excelWorksheetB.Cells[6, 4].Value != null ? 11 : 12, 8, excelWorksheetB.Cells[6, 4].Value != null ? 31 : 32, 8].Copy(excelWorksheetA.Cells[startRow, 22, endRow, 22]); // COPY H12-H32 To V7-V27 and ...
                        excelWorksheetB.Cells[excelWorksheetB.Cells[6, 4].Value != null ? 11 : 12, 14, excelWorksheetB.Cells[6, 4].Value != null ? 31 : 32, 14].Copy(excelWorksheetA.Cells[startRow, 23, endRow, 23]); // COPY N12-N32 To W7-W27 and ...
                    }
                    startRow = endRow + 8;
                    endRow = startRow + 20;
                    File.Delete(fileBName);
                }

                string resultPath = FileA.Substring(0, FileA.LastIndexOf('.')) + "Success" + FileA.Substring(FileA.LastIndexOf('.'));
                excelPackageA.SaveAs(new FileInfo(resultPath));

            }
        }

        public void ConvertToXlsx(String path)
        {
            var app = new Application();
            var wb = app.Workbooks.Open(path);
            wb.SaveAs(Filename: path + "x", FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            wb.Close();
            app.Quit();
        }
    }
}