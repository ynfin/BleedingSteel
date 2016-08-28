using System;
using System.IO;

namespace BleedingSteel
{
    public class Logfile
    {
        public string filepath { get; }
        public DateTime creationtime { get; }
        public string[] content { get; }

        public Logfile(string path)
        {
            content = File.ReadAllLines(path);
            filepath = path;
            creationtime = File.GetCreationTime(path);
        }
    }
}

