using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace SoPechExcel.Files
{
    public class ExcelReport
    {
        private string FileA { get; set; }
        private List<string> FilesB { get; set; }

        public ExcelReport(string fileA, List<string> filesB)
        {
            FileA = fileA;
            FilesB = filesB;
        }

        public void Proceed()
        {
            using (var package = new ExcelPackage(new FileInfo(FileA)))
            {
                string resultPath = FileA.Substring(0, FileA.LastIndexOf('.')) + "Success" + FileA.Substring(FileA.LastIndexOf('.'));
                package.SaveAs(new FileInfo(resultPath));
            }
        }
    }
}