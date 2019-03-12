using System.Collections.Generic;
using System.IO;
using System.Linq;
using Medusa.Models;

namespace Medusa.Reports
{

    public class SchoolCsvReport : BaseSchoolReport
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
            var filename = $"{baseReportPath}//{school}_MedusaReport.csv";

            using (StreamWriter sw = new StreamWriter(filename))
            {
                // Header
                sw.WriteLine("Student,Grade,Gender,Teacher,Raw Score,National Average,Award");

                var sorted = testResults
                    .OrderBy(i => i.Grade)
                    .ThenBy(i => i.GetCorrectAnswers(key))
                    .ThenBy(i => i.Name)
                    .ToList();

                foreach (var tr in sorted)
                {
                    var nationalAvg = NationalAverage[tr.Grade];
                    string award = "N/A";
                    sw.WriteLine($"{tr.Name},{tr.Grade},{tr.Gender},{tr.Teacher},{tr.GetCorrectAnswers(key)},{nationalAvg},{award}");
                }
            }
        }


    }
}