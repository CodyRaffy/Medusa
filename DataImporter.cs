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
            string currentLine;
            string school = string.Empty;

            var lines = new List<string>();

            using (var file = new System.IO.StreamReader("./data/2018_TestResults.csv"))
            {
                while ((currentLine = file.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(currentLine)) continue;
                    lines.Add(currentLine);
                }

            }

            var convertedLines = new List<string>();
            foreach (var line in lines)
            {
                var noQuoteLine = line;
                var quoteIndex = noQuoteLine.IndexOf('"');

                while (quoteIndex != -1)
                {
                    var nextQuote = noQuoteLine.IndexOf('"', quoteIndex + 1);

                    var quoteSection = noQuoteLine.Substring(quoteIndex + 1, nextQuote - quoteIndex - 1)
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace(",", "|");


                    noQuoteLine = noQuoteLine.Substring(0, quoteIndex) + quoteSection + line.Substring(nextQuote + 1);
                    quoteIndex = noQuoteLine.IndexOf('"');
                }

                convertedLines.Add(noQuoteLine.Replace("|", " AND "));
            }


            foreach (var line in convertedLines)
            {
                var split = line.Split(",").Select(i => i.Trim()).ToList();
                testResults.Add(new MedusaTest(split));
            }

            return testResults;
        }

    }
}