using System;
using System.Collections.Generic;

namespace Medusa.Models
{
    public class MedusaKey
    {
        public MedusaKey(List<string> answers)
        {
            Answers = answers;
        }
        public List<String> Answers { get; set; }
    }
}