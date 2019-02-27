using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medusa
{
    public static class DataImporter
    {
        public static MedusaKey ImportKey()
        {
            const string path = "./data/2018_AnswerKey.csv";
            string keyText = File.ReadAllText(path);
            var answers = keyText.Split("\r\n").ToList();
            return new MedusaKey(answers);
        }

        public static List<MedusaTest> ImportTestData()
        {
            List<MedusaTest> testResults = new List<MedusaTest>();
            string line;
            string school = string.Empty;

            var file = new System.IO.StreamReader("./data/2018_TestResults.csv");
            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var split = line.Split(",").Select(i => i.Trim()).ToList();
                testResults.Add(new MedusaTest(split));
            }

            return testResults;
        }

    }
}