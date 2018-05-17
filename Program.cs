using System;

namespace Medusa
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = DataImporter.ImportKey();
            Console.WriteLine($"The Key has {key.Answers.Count} answers");
        }
    }
}
