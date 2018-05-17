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
            const string path = "./data/MedusaKey.txt";
            string keyText = File.ReadAllText(path);
            var answers = keyText.Split("\r\n").ToList();
            return new MedusaKey(answers);
        }

        public static List<MedusaTest> ImportTestData()
        {
            List<MedusaTest> testResults = new List<MedusaTest>();
            string line;
            string school = string.Empty;

            var file = new System.IO.StreamReader("./data/Medusa17.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var split = line.Split(",").Select(i => i.Trim()).ToList();
                if (string.IsNullOrWhiteSpace(split[0]) && split[2] == "~")
                {
                    school = split[1];
                    continue;
                }

                testResults.Add(new MedusaTest(school, split));
            }

            return testResults;
        }

    }
}