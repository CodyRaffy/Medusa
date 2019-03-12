using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medusa.Models;
using Medusa.Reports;

namespace Medusa
{
    class Program
    {
        private static readonly string BaseReportPath = @"C:\Temp\\Medusa\";

        static void Main(string[] args)
        {
            var key = DataImporter.ImportKey();
            Console.WriteLine($"The Key has {key.Answers.Count} answers");

            var testResults = DataImporter.ImportTestData();

            SummaryReport.GenerateTestSummaryReport(BaseReportPath, key, testResults);

            //ISchoolReport schoolReport = new SchoolCsvReport();
            ISchoolReport schoolReport = new SchoolExcelReport();
            schoolReport.GenerateSchoolReports(BaseReportPath, testResults, key);
        }
    }
}
