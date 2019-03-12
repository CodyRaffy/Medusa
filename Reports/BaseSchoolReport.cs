using System.Collections.Generic;
using Medusa.Models;
using System.Linq;

namespace Medusa.Reports
{
    public abstract class BaseSchoolReport : ISchoolReport
    {
        protected readonly Dictionary<int, string> NationalAverage = new Dictionary<int, string>();

        public abstract void GenerateSchoolReports(string baseReportPath, List<MedusaTest> testResults, MedusaKey key);

        protected void SetNationalAverages(List<MedusaTest> testResults, MedusaKey key)
        {
            var gradeGroups = testResults.GroupBy(i => i.Grade);
            foreach (var gradeGroup in gradeGroups)
            {
                NationalAverage.Add(gradeGroup.Key, gradeGroup.Average(i => i.GetCorrectAnswers(key)).ToString("F1"));
            }
        }
    }
}