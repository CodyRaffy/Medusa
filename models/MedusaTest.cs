using System;
using System.Collections.Generic;

namespace Medusa
{
    public class MedusaTest
    {
        public MedusaTest(string school, List<string> inputLine)
        {
            School = school;
            Gender = inputLine[0];
            Name = inputLine[1];
            GradeLevel = inputLine[2];
            Subject = inputLine[3];
            Answers = new List<string>();

            for (var i = 4; i < inputLine.Count; i++)
            {
                Answers.Add(inputLine[i]);
            }
        }

        public string School { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string GradeLevel { get; set; }

        public List<String> Answers { get; set; }

        public int GetCorrectAnswers(MedusaKey key)
        {
            int correct = 0;
            for (var i = 0; i < Answers.Count; i++)
            {
                if (IsAnswerCorrect(key, i))
                {
                    correct++;
                }
            }
            return correct;
        }

        public bool IsAnswerCorrect(MedusaKey key, int questionIndex)
        {
            if (Answers.Count <= questionIndex || key.Answers.Count <= questionIndex) return false;
            return key.Answers[questionIndex] == "All" || key.Answers[questionIndex] == Answers[questionIndex];
        }
    }
}