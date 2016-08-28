using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
namespace BleedingSteel
{
    public class LinSpacer
    {
       /* public LinSpacer()
        {
        }

        //DisplayName: ObjectModel.H1VertAxis.ActPos @IPS_10.47.89.189, SignalName: ObjectModel.H1VertAxis.Signal.ActPos @IPS_10.47.89.189, Source: C522 5500 CBSII,Unit: mm,SignalType: Analog
        //15:06:2016 09:44:46:711,500

        public List<TimeSample> linearize(string[] csvLines, int sampleinterval)
        {

            List<Signal> signalList = gatherSignalData(csvLines);

            List<TimeSample> timeSampleList = new List<TimeSample>();

            DateTime globalStart = new DateTime();
            globalStart = DateTime.MaxValue;

            DateTime globalEnd = new DateTime();
            globalEnd = DateTime.MinValue;

            // set globalstart
            foreach (var signal in signalList) {
                if (signal.getStartTime() < globalStart)
                    globalStart = signal.getStartTime();
            }

            DateTime roundedStart = Truncate(globalStart, TimeSpan.FromSeconds(1));

            // set globalend
            foreach (var signal in signalList) {
                if (signal.getEndTime() > globalEnd)
                    globalEnd = signal.getEndTime();
            }

            DateTime currentTimestamp = roundedStart;
            TimeSample currentTimeSample;

            double totalsamples = ((globalEnd - globalStart).TotalSeconds / (sampleinterval / 1000.0));

            Console.Write("Linearizing between: " + globalStart + " and " + globalEnd);
            Console.WriteLine(" -- > [ " + (globalEnd - globalStart).Minutes + " min, "  + (globalEnd - globalStart).Seconds + " seconds ]");
            Console.WriteLine("Sampling at every " + sampleinterval + "ms gives " + (int)totalsamples + " samples:");

            int currenttick = 0;

            while (true){

                // increment time by sampleinterval
                currentTimeSample = new TimeSpan(currentTimestamp);
                currentTimestamp = currentTimestamp + TimeSpan.FromMilliseconds(sampleinterval);

                currenttick++;

                Console.Write("\r{0}% complete", (int)(currenttick / totalsamples * 100));

                if (currentTimestamp > globalEnd) {
                    break;
                }

                foreach (var signal in signalList) {

                    for (int i = signal.samplemark; i < signal.sampleValues.Count; i++) {

                        if (signal.sampleValues[i].realTimestamp > currentTimestamp) {

                            signal.samplemark = signal.sampleValues.IndexOf(signal.sampleValues[i]);

                            currentTimeSample.addNewSample(signal.sampleValues[i].value, signal.name);
                            timeSampleList.Add(currentTimeSample);
                            break;
                        }
                    }
                }
            }

            return timeSampleList;
        }


        public DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }


        private List<Signal> gatherSignalData(string[] csvLines){

            Signal tempSignal = new Signal("init");
            List<Signal> signalList = new List<Signal>();

            // build signal objects to store signal data for each signal
            foreach (var line in csvLines) {

                if (line.Contains("EventLog")){
                    if (tempSignal.sampleValues.Count > 0)
                        signalList.Add(tempSignal);

                    break;
                }

                if (line.Contains("DisplayName: ")){
                    if (tempSignal.sampleValues.Count > 0){
                        signalList.Add(tempSignal);
                        Console.WriteLine("Adding signal: " + tempSignal.sampleValues.Count + " - " + tempSignal.name);
                    }
                      
                    tempSignal = new Signal(getSignalName(line));
                }

                if ((!line.Contains("DisplayName: ")) && (!line.Equals(""))){
                    tempSignal.addNewSample(getValue(line), getTimestamp(line));
                }
            }
            return signalList;
        } 


        private double getValue(string linestring){
            string valuestring = linestring.Substring(linestring.IndexOf(',')+1);
            return Convert.ToDouble(valuestring);
        }

        private DateTime getTimestamp(string linestring){
            string substring = linestring.Substring(0, linestring.IndexOf(','));
            DateTime timestampstring = DateTime.ParseExact(substring, "dd:MM:yyyy HH:mm:ss:fff", CultureInfo.InvariantCulture);
            return timestampstring;
        }

        private string getSignalName(string linestring){
            int startindex = linestring.IndexOf("DisplayName: ", StringComparison.InvariantCulture) + "DisplayName: ".Length;
            int endindex = linestring.IndexOf('@');

            string substring = linestring.Substring(startindex, endindex-startindex);

            // remove irrelevent name data
            if (substring.Contains("ObjectModel."))
                substring = substring.Replace("ObjectModel.", "");
            
            return substring;
        }

*/
    }
}

