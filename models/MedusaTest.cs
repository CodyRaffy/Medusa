using System;
using System.Collections.Generic;

namespace Medusa
{
    public class MedusaTest
    {
        public MedusaTest(List<string> inputLine)
        {
            Name = inputLine[0];
            Gender = inputLine[1];
            GradeLevel = inputLine[2];
            Subject = inputLine[3];
            School = inputLine[4];
            Teacher = inputLine[5];

            Answers = new List<string>();

            for (var i = 6; i < inputLine.Count; i++)
            {
                Answers.Add(inputLine[i]);
            }
        }

        public string School { get; set; }

        public string Subject { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string GradeLevel { get; set; }

        public string Teacher { get; set; }
        public int Grade
        {
            get
            {
                int grade = 0;
                int.TryParse(GradeLevel, out grade);
                return grade;
            }
        }

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