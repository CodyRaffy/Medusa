using System;
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

            foreach (var testResult in testResults.OrderBy(i => i.Grade))
            {
                if (testResult.School == "ZZJAEC")
                    Console.WriteLine($"{testResult.Name}, {testResult.Gender} - Grade {testResult.Grade} got {testResult.GetCorrectAnswers(key)} questions correct");
            }
        }
    }
}
