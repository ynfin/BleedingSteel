using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BleedingSteel
{
    public class SignalObject
    {

        public string displayname { get; private set; }
        public string signalname { get; private set; }
        public string source { get; private set; }
        public string unit { get; private set; }
        public string signaltype { get; private set; }
        public bool initialized { get; private set; }
        public DateTime startTime { get; private set;}
        public DateTime endTime { get; private set; }
        public int samples { get; private set; }
        public int lastSampledIndex { get; set;}


        public Sample holdSample { get; set;}
        public List<Sample> sampleList = new List<Sample>();

        public SignalObject(){
            initialized = false;
            lastSampledIndex = 0;
        }

        // signalobject will only be created on info lines, and will use all data in line to build info
        public void Init(string line)
        {
            if (line.Contains("DisplayName:")) {
                displayname = getInfoFromLine(line, "DisplayName:");
                signalname = getInfoFromLine(line, "SignalName:");
                source = getInfoFromLine(line, "Source:");
                unit = getInfoFromLine(line,"Unit:");
                signaltype = getInfoFromLine(line,"SignalType:");
                initialized = true;
            }
        }

        public void addDataLine(string line){
            if (line != "")
                sampleList.Add(new Sample(getDataFromLine(line), getTimestampFromLine(line),signalname));
        }

        private string getInfoFromLine(string line, string descriptor){
            int startindex = line.IndexOf(descriptor, StringComparison.InvariantCulture) + descriptor.Length;
            int endindex = line.IndexOf(',',startindex);
            if (endindex < 0)
                endindex = line.Length;
            return line.Substring(startindex, endindex-startindex);
        }

        private double getDataFromLine(string line){
            return float.Parse(line.Substring(line.IndexOf(',') + 1));
        }

        private DateTime getTimestampFromLine(string line){
            string timestring = line.Substring(0, line.IndexOf(','));
            return DateTime.ParseExact(timestring, "dd:MM:yyyy HH:mm:ss:fff", CultureInfo.InvariantCulture);
        }

        public void wrapUpSignal(){
            startTime = sampleList.First().timestamp;
            endTime = sampleList.Last().timestamp;
            samples = sampleList.Count;
            holdSample = sampleList[0];
        }

        public void printInfo()
        {
            Console.WriteLine("signal members: ");
            Console.WriteLine("displayname:    " + displayname);
            Console.WriteLine("signalname:     " + signalname);
            Console.WriteLine("source:         " + source);
            Console.WriteLine("unit:           " + unit);
            Console.WriteLine("signaltype:     " + signaltype);
            Console.WriteLine();
            Console.WriteLine("samples         " + sampleList.Count);

            Console.WriteLine("");
        }

    }
}

