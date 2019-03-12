using System.Collections.Generic;
using System.Drawing;
using Medusa.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Linq;

namespace Medusa.Reports
{
    public class SchoolExcelReport : BaseSchoolReport
    {
        public override void GenerateSchoolReports(string baseReportPath, List<MedusaTest> testResults, MedusaKey key)
        {
            SetNationalAverages(testResults, key);

            var schoolGroups = testResults.GroupBy(i => i.School);
            foreach (var schoolGroup in schoolGroups)
            {
                GenerateSchoolReport(baseReportPath, key, schoolGroup.Key, schoolGroup.ToList());
            }
        }

        private void GenerateSchoolReport(string baseReportPath, MedusaKey key, string school, List<MedusaTest> testResults)
        {
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(school);
                //Add the headers
                worksheet.Cells[1, 1].Value = "Student";
                worksheet.Cells[1, 2].Value = "Grade";
                //worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 3].Value = "Teacher";
                worksheet.Cells[1, 4].Value = "Raw Score";
                worksheet.Cells[1, 4].Style.WrapText = true;
                worksheet.Cells[1, 5].Value = "National Average";
                worksheet.Cells[1, 5].Style.WrapText = true;
                worksheet.Cells[1, 6].Value = "Award";

                //Add some items...
                var sorted = testResults
                   .OrderBy(i => i.Grade)
                   .ThenBy(i => i.GetCorrectAnswers(key))
                   .ThenBy(i => i.Name)
                   .ToList();

                var row = 2;
                foreach (var tr in sorted)
                {

                    var nationalAvg = NationalAverage[tr.Grade];

                    worksheet.Cells[$"A{row}"].Value = tr.Name;
                    worksheet.Cells[$"B{row}"].Value = tr.Grade;
                    //worksheet.Cells[$"C{row}"].Value = tr.Gender;
                    worksheet.Cells[$"C{row}"].Value = tr.Teacher;
                    worksheet.Cells[$"D{row}"].Value = tr.GetCorrectAnswers(key);
                    worksheet.Cells[$"E{row}"].Value = nationalAvg;
                    worksheet.Cells[$"F{row}"].Value = tr.GetAward(key);

                    row++;
                }

                worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                using (ExcelRange r = worksheet.Cells["A1:F1"])
                {
                    //r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
                    r.Style.Font.Bold = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                using (var r = worksheet.Cells[""])

                    worksheet.Cells.AutoFitColumns(8);  //Autofit columns for all cells

                // lets set the header text 
                worksheet.HeaderFooter.OddHeader.CenteredText = $"&24&U&\"Arial,Regular Bold\" {school}";
                // add the page number to the footer plus the total number of pages
                worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                // add the sheet name to the footer
                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                // add the file path to the footer
                //worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FileName;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:1"];
                worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

                // Change the sheet view to show it in page layout mode
                worksheet.View.PageLayoutView = true;

                // set some document properties
                package.Workbook.Properties.Title = $"School Report: {school}";
                package.Workbook.Properties.Author = "Melissa Raffensperger";
                package.Workbook.Properties.Comments = "Medusa 2018 Test";

                // set some extended property values
                package.Workbook.Properties.Company = "Raffy6Pack";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "Cody Raffensperger");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");

                var filename = $"{baseReportPath}//{school}_MedusaReport.xlsx";
                var xlFile = GetFileInfo(filename);
                // save our new workbook in the output directory and we are done!
                package.SaveAs(xlFile);
            }
        }


        private static FileInfo GetFileInfo(string file, bool deleteIfExists = true)
        {
            var fi = new FileInfo(file);
            if (deleteIfExists && fi.Exists)
            {
                fi.Delete();  // ensures we create a new workbook
            }
            return fi;
        }
    }
}