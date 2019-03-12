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

            GenerateTestSummaryReport(key, testResults);

            SchoolReport.GenerateSchoolReports(BaseReportPath, testResults, key);
        }

        private static void GenerateTestSummaryReport(MedusaKey key, List<MedusaTest> testResults)
        {
            var perfecto = testResults.Where(i => i.GetCorrectAnswers(key) >= 39);

            foreach (var person in perfecto)
            {
                Console.WriteLine($"{person.Name} - {person.Gender} - {person.School}: {person.GetCorrectAnswers(key)} - Q9: {person.Answers[8]} - Q30: {person.Answers[29]}");
            }

            var middleSchoolTestResults = testResults
                .Where(i => i.Grade == 6 || i.Grade == 7 || i.Grade == 8)
                .ToList();

            var ninthGradeTestResults = testResults.Where(i => i.Grade == 9).ToList();
            var tenthGradeTestResults = testResults.Where(i => i.Grade == 10).ToList();
            var eleventhGradeTestResults = testResults.Where(i => i.Grade == 11).ToList();
            var twelvthGradeTestResults = testResults.Where(i => i.Grade == 12).ToList();

            var middleSchoolReportList = CreateReportItemList(middleSchoolTestResults, key);
            var ninthGradeReportList = CreateReportItemList(ninthGradeTestResults, key);
            var tenthGradeReportList = CreateReportItemList(tenthGradeTestResults, key);
            var eleventhGradeReportList = CreateReportItemList(eleventhGradeTestResults.ToList(), key);
            var twelvthGradeReportList = CreateReportItemList(twelvthGradeTestResults.ToList(), key);
            var allReportList = CreateReportItemList(testResults, key);

            var all = CreateReportItemList(testResults, key);

            using (StreamWriter sw = new StreamWriter($"{BaseReportPath}\\Distribution.csv"))
            {
                var totalCount = middleSchoolReportList.Count;
                var header = "# EX,# A.L.,PCT";
                sw.WriteLine($"# Correct,{header},{header},{header},{header},{header},{header}");

                for (var i = 0; i < totalCount; i++)
                {
                    sw.Write($"{i},");
                    WriteItemFromList(sw, middleSchoolReportList, i, middleSchoolTestResults.Count);
                    WriteItemFromList(sw, ninthGradeReportList, i, ninthGradeTestResults.Count);
                    WriteItemFromList(sw, tenthGradeReportList, i, tenthGradeTestResults.Count);
                    WriteItemFromList(sw, eleventhGradeReportList, i, eleventhGradeTestResults.Count);
                    WriteItemFromList(sw, twelvthGradeReportList, i, twelvthGradeTestResults.Count);
                    WriteItemFromList(sw, allReportList, i, testResults.Count);
                    sw.WriteLine("");
                }

                // Write Totals
                sw.Write($",,Total,{middleSchoolTestResults.Count},,");
                sw.Write($"Total,{ninthGradeTestResults.Count},,");
                sw.Write($"Total,{tenthGradeTestResults.Count},,");
                sw.Write($"Total,{eleventhGradeTestResults.Count},,");
                sw.Write($"Total,{twelvthGradeTestResults.Count},,");
                sw.Write($"Total,{testResults.Count}");
                sw.WriteLine("");

                // Write Averages
                sw.Write($",,Average,{Math.Round(middleSchoolTestResults.Average(i => i.GetCorrectAnswers(key)), 1)},,");
                sw.Write($"Average,{Math.Round(ninthGradeTestResults.Average(i => i.GetCorrectAnswers(key)), 1)},,");
                sw.Write($"Average,{Math.Round(tenthGradeTestResults.Average(i => i.GetCorrectAnswers(key)), 1)},,");
                sw.Write($"Average,{Math.Round(eleventhGradeTestResults.Average(i => i.GetCorrectAnswers(key)), 1)},,");
                sw.Write($"Average,{Math.Round(twelvthGradeTestResults.Average(i => i.GetCorrectAnswers(key)), 1)},,");
                sw.Write($"Average,{Math.Round(testResults.Average(i => i.GetCorrectAnswers(key)), 1)}");
                sw.WriteLine("");

                // Item Analysis
                //Question	Correct	    Percent	    Correct
                //Number	Responses	Correct	    Answer
                sw.WriteLine("Question,Correct,Percent,Correct");
                sw.WriteLine("Number,Responses,Correct,Answer");
                for (var i = 0; i < 40; i++)
                {
                    var numberCorrect = testResults.Count(test => test.IsAnswerCorrect(key, i));
                    var percentCorrect = ((decimal)numberCorrect / (decimal)testResults.Count).ToString("P1");
                    sw.WriteLine($"{i + 1},{numberCorrect},{percentCorrect},{key.Answers[i]}");
                }
            }
        }

        private static void WriteItemFromList(StreamWriter sw, List<ReportItem> list, int index, int totalCount)
        {
            var reportItem = list[index];
            WriteSection(sw, reportItem, index, totalCount);
        }

        private static void WriteSection(StreamWriter sw, ReportItem reportItem, int index, int totalCount)
        {
            decimal pct = (decimal)reportItem.AtLeast / (decimal)totalCount;
            string percentage = pct.ToString("P1");
            sw.Write($"{reportItem.Exactly},{reportItem.AtLeast},{percentage},");
        }

        private static List<ReportItem> CreateReportItemList(List<MedusaTest> list, MedusaKey key)
        {
            var reportItemList = new List<ReportItem>();

            for (var index = 0; index <= 40; index++)
            {
                var reportItem = new ReportItem
                {
                    Exactly = list.Count(i => i.GetCorrectAnswers(key) == index),
                    AtLeast = list.Count(i => i.GetCorrectAnswers(key) >= index)
                };
                reportItemList.Add(reportItem);
            }

            return reportItemList;
        }
    }
}
