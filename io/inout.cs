using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using BleedingSteel;

namespace BleedingSteel
{
    public class inout
    {
        public inout ()
        {
        }


        public string[] readSignalAnalyzerFile(string filepath)
        {
            string[] linearray = File.ReadAllLines(filepath);
            return linearray;
        }

        public void printTimelineHtml(List<Job> inputjob, string outpath){
            HtmlWriter HtmlWrite = new HtmlWriter();
            HtmlWrite.printTimeLineXml(inputjob, outpath);
        }

        public void printTimeLineXml(List<Job> inputjob,string outputpath){
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("timelinexml"));

            foreach (var job in inputjob) {
                XmlElement jobEl = (XmlElement)el.AppendChild(doc.CreateElement("Jobnode"));
                jobEl.SetAttribute("jobname", job.jobName);

                foreach (var cycle in job.triggeredCycles) {
                    XmlElement cycleEl = (XmlElement)jobEl.AppendChild(doc.CreateElement("cyclenode"));
                    cycleEl.SetAttribute("cyclename", cycle.cyclename);
                    cycleEl.SetAttribute("start", cycle.getStartStamp().TotalSeconds.ToString());
                    cycleEl.SetAttribute("end", cycle.getEndStamp().TotalSeconds.ToString());
                    cycleEl.SetAttribute("duration", cycle.getDuration().TotalSeconds.ToString());
                }
            }

            using (StreamWriter file = new StreamWriter(outputpath))
                file.Write(Beautify(doc));
        }


        public void printXmlFromTimeSamples(List<Job> inputjob, string outputpath){

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("jobxml"));
            foreach (var job in inputjob) {
                XmlElement jobEl = (XmlElement)el.AppendChild(doc.CreateElement(job.jobName));
                Console.WriteLine(" --- XML > dumping " + job.jobName + " to xml file");

                foreach (var cycle in job.triggeredCycles) {
                    XmlElement cycleEl = (XmlElement)jobEl.AppendChild(doc.CreateElement(cycle.cyclename));
                    Console.WriteLine("    - cycle " + (job.triggeredCycles.IndexOf(cycle)+1) + ", " + cycle.cyclename);

                    foreach (var timesample in cycle.timeSampleList) {
                        XmlElement timeSampleEl = (XmlElement)cycleEl.AppendChild(doc.CreateElement("timesample"));
                        timeSampleEl.SetAttribute("timestamp", timesample.timespanStamp.ToString());
                        timeSampleEl.SetAttribute("delta", timesample.timespanStamp.TotalSeconds.ToString());

                        foreach (var sample in timesample.sampleList) {
                            XmlElement sampleEl = (XmlElement)timeSampleEl.AppendChild(doc.CreateElement("sample"));
                            sampleEl.SetAttribute("name",sample.signalname);
                            sampleEl.SetAttribute("value", sample.value.ToString());
                            sampleEl.SetAttribute("seconds",sample.sampledTimeSpan.TotalSeconds.ToString());
                        }
                    }
                }

                using (StreamWriter file = new StreamWriter(outputpath))
                    file.Write(Beautify(doc));
            }
        }

        private string Beautify(XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings)) {
                doc.Save(writer);
            }
            return sb.ToString();
        }


        public void printFullDataMatrix(List<TimeSample> timeSampleList, string outpath ){
            List<string> uniquesignals = new List<string>();

            Console.WriteLine("\n --- // Building header // --- ");
            foreach (var timesample in timeSampleList) {
                foreach (Sample sample in timesample.sampleList) {
                    if (!uniquesignals.Contains(sample.signalname)) {
                        uniquesignals.Add(sample.signalname);
                    }
                }
            }

            Console.WriteLine(" --- // dumping to file // ---");
            using (StreamWriter file = new StreamWriter(outpath)) {
                // write header
                file.Write("timestamp,");
                foreach (var signalname in uniquesignals) {
                    file.Write(","+signalname);
                }

                // start writing signals
                for (int i = 1; i < timeSampleList.Count; i++) {
                    var timesample = timeSampleList[i];
                    file.Write("\n" + timesample.timespanStamp.TotalSeconds + ",");
                    foreach (var uniquesignal in uniquesignals) {
                        foreach (var sample in timesample.sampleList) {
                            if (sample.signalname.Equals(uniquesignal)) {
                                file.Write(sample.value + ",");
                                break;
                            }
                        }
                    }
                }
            }

            /*
            foreach (var timesample in timeSampleList) {
                
                Console.Write(timesample.timespanStamp+ " -> ");
                foreach (var sample in timesample.sampleList) {
                    Console.Write(sample.getShortSignalName()+ " , ");
                }
                Console.WriteLine();
                if (timesample.hasDuplicateSamples()) {
                    Console.WriteLine("---------------- MULTISAMPLE ---------------");
                }
            }
            */



        }




        /*public void writeCsvFile(List<TimeSample> timesamples,string outputfile){

            using (StreamWriter w = File.AppendText(outputfile)) {

                // write signalnames in header
                string headerstring = "timestamp";
                foreach (var sample in timesamples[0].sampleValues)
                    headerstring += "," + sample.name;
                w.WriteLine(headerstring);

                foreach (var timesample in timesamples) {
                    string datastring = "";
                    datastring += timesample.timestamp;
                    foreach (var doublevalue in timesample.getAllSampleValues()) {
                        datastring += "," + doublevalue;
                    }
                    w.WriteLine(datastring);
                }
            }
        }*/
    }
}

