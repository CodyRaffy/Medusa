using System;
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
    }
}
