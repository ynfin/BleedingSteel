using System;
using System.Collections.Generic;
using System.IO;

namespace BleedingSteel
{
    public class HtmlWriter
    {
        public HtmlWriter()
        {
            // copy modified template to output path
        }

        //[ 'FillCart', 'FillCart (11.211s)', new Date(2016,1,23,22,22,17,178), new Date(2016,1,23,22,22,28,389) ],

        public void printTimeLineXml(List<Job> inputjob, string outputpath)
        {
            string[] readText = File.ReadAllLines("io/googleTimelineTemplate.html");
            List<string> injectionString = new List<string>();
                
            foreach (var job in inputjob) {

                foreach (var cycle in job.triggeredCycles) {

                    string tempstring = "[";

                    tempstring = tempstring + "'" + job.jobName + "',";

                    tempstring = tempstring + "'" + job.jobName + " (" + cycle.getDuration().TotalSeconds + ")',";
                    tempstring = tempstring + "new Date(0,0,"+cycle.getStartStamp().Days +","
                                                                     + cycle.getStartStamp().Hours + ","
                                                                     + cycle.getStartStamp().Minutes + ","
                                                                     + cycle.getStartStamp().Seconds + ","
                                                                     + cycle.getStartStamp().Milliseconds + "),";

                    tempstring = tempstring + "new Date(0,0," + cycle.getEndStamp().Days + ","
                                                                     + cycle.getEndStamp().Hours + ","
                                                                     + cycle.getEndStamp().Minutes + ","
                                                                     + cycle.getEndStamp().Seconds + ","
                                                                     + cycle.getEndStamp().Milliseconds + ")";
                    tempstring = tempstring + "],";
                    Console.WriteLine(tempstring);
                    injectionString.Add(tempstring);
                }
            }

            using (StreamWriter file = new StreamWriter(outputpath)) {
                foreach (var line in readText) {
                    if (line.Contains("<insertionpoint timelinecycles>")) {
                        foreach (var injectionline in injectionString) {
                            file.WriteLine(injectionline);
                        }
                    } else {
                        file.WriteLine(line);
                    }
                }
            }
        }

    }
}

