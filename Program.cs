using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medusa
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = DataImporter.ImportKey();
            Console.WriteLine($"The Key has {key.Answers.Count} answers");

            var testResults = DataImporter.ImportTestData();

            var middleSchoolTestResults = testResults
                .Where(i => i.Grade == 6 || i.Grade == 7 || i.Grade == 8)
                .ToList();

            var middleSchoolList = new List<ReportItem>();

            for (var index = 0; index <= 40; index++)
            {
                var reportItem = new ReportItem
                {
                    Exactly = middleSchoolTestResults.Count(i => i.GetCorrectAnswers(key) == index),
                    AtLeast = middleSchoolTestResults.Count(i => i.GetCorrectAnswers(key) >= index)
                };
                middleSchoolList.Add(reportItem);
            }

            using (StreamWriter outputFile = new StreamWriter("C:\\Temp\\Medusa\\MiddleSchool.csv"))
            {
                var index = 0;
                var totalCount = middleSchoolTestResults.Count;
                outputFile.WriteLine("# Correct,# EX,# A.L.,PCT");
                foreach (var reportItem in middleSchoolList)
                {
                    decimal pct = (decimal)reportItem.AtLeast / (decimal)totalCount;
                    string percentage = pct.ToString("P1");
                    outputFile.WriteLine($"{index},{reportItem.Exactly},{reportItem.AtLeast},{percentage}");
                    index++;
                }
            }
        }
    }
}
