using System.Collections.Generic;
using Medusa.Models;

namespace Medusa.Reports
{
    public interface ISchoolReport
    {
        void GenerateSchoolReports(string baseReportPath, List<MedusaTest> testResults, MedusaKey key);
    }
}