using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BleedingSteel
{
    public class SignalBuilder
    {

        public DateTime absoluteStart { get; private set;}
        public DateTime absoluteEnd { get; private set; }
        public List<TimeSample> TimeList { get; private set;}


        Stopwatch watch = new Stopwatch();

        private List<SignalObject> RawList = new List<SignalObject>();
        private List<SignalObject> LinList = new List<SignalObject>();

        public SignalBuilder(string[] filedata) // will read all data and store in rawlist
        {
            TimeList = new List<TimeSample>();
            Console.WriteLine(" ---- // Loading filedata into signal lists // ----");
            watch.Restart();
            // start with reading the raw signals
            SignalObject tempSignal = new SignalObject();
            foreach (var line in filedata) {
                //Console.WriteLine(line);
                if (line.Contains("EventLog"))
                    continue;
                       

                if ((line.Contains("DisplayName:")) || (line == filedata.Last())) {

                    if (tempSignal.initialized){
                        tempSignal.wrapUpSignal();
                        RawList.Add(tempSignal);
                        Console.WriteLine(" - [" + tempSignal.samples + "] \t"+tempSignal.displayname);
                    }

                    tempSignal = new SignalObject();
                    tempSignal.Init(line);
                }
                else {
                    if (tempSignal.initialized) {
                        tempSignal.addDataLine(line);
                    }
                }
            }
            Console.WriteLine(" - Total Signals in list: " + RawList.Count);
            Console.WriteLine(" - [Processing time: " + watch.Elapsed.TotalSeconds+ "]");
        }

        public List<TimeSample> linearizeRawData(int msecintervals){

            watch.Restart();
            Console.WriteLine("\n ---- // Initializing Linearization // ----");
            double lastPercent = 0;
            absoluteStart = RawList.Min(item => item.startTime);
            absoluteEnd = RawList.Max(item => item.endTime);
            TimeSpan duration = absoluteEnd - absoluteStart;
            TimeSpan lastsample = new TimeSpan(0);

            Console.WriteLine(" - Offsetting samples from zero");
            foreach (var signal in RawList) {
                foreach (var sample in signal.sampleList) {
                    sample.zeroTimeSpan = sample.timestamp - absoluteStart;
                    lastsample = sample.zeroTimeSpan;
                }
            }

            // add the first sample of each signal to use as sample-hold...
            List<Sample> sampleHoldList = new List<Sample>();
            foreach (var signal in RawList) {
                sampleHoldList.Add(signal.sampleList[0]);
            }


            Console.WriteLine(" - Assigning samples to a linear timescale with "+ msecintervals+ " ms sample intervals");
            TimeSpan sampletime = new TimeSpan(0);
            while (sampletime < duration){
                

                // Also create timestamplist
                TimeSample tempTimeSample = new TimeSample(sampletime);

                foreach (var signal in RawList) {
                    //Console.WriteLine("    - Processing sample "+ sampletime +" signal "+ signal.signalname);
                    for (int i = signal.lastSampledIndex; i < signal.sampleList.Count; i++) {
                        var sample = signal.sampleList[i];
                        if (sample.zeroTimeSpan.CompareTo(sampletime) >= 0 && sample.zeroTimeSpan.CompareTo(sampletime.Add(TimeSpan.FromMilliseconds(msecintervals))) < 0) {
                            sample.sampledTimeSpan = sampletime;
                            signal.lastSampledIndex = i;
                            tempTimeSample.addSample(sample);

                            signal.holdSample = sample;

                        }

                        if (sample.zeroTimeSpan.CompareTo(sampletime.Add(TimeSpan.FromMilliseconds(msecintervals))) == 1){
                            if (signal.holdSample.sampledTimeSpan.CompareTo(sampletime) < 0) {
                                // add previous sample to temptimesample instead of current, as it is empty...
                                tempTimeSample.addSample(signal.holdSample);
                            }
                            break;
                        }

                    }
                }
                //Console.WriteLine(sampletime);
                // done with this sample... increase
                TimeList.Add(tempTimeSample);
                sampletime = sampletime.Add(TimeSpan.FromMilliseconds(msecintervals));
                double percent = (Divide(sampletime, duration) * 100);
                if (percent > lastPercent +10) {
                    Console.Write(" - " + percent.ToString("0")+"% ");
                    lastPercent = percent;
                }
            }
            Console.WriteLine(" - 100%");
            Console.WriteLine("\n - abs start: " + absoluteStart);
            Console.WriteLine(" - abs end: " + absoluteEnd);
            Console.WriteLine(" - total span: " + duration);
            Console.WriteLine(" - [Processing time: " + watch.Elapsed.TotalSeconds + "]");

            return TimeList;
        }

        public DateTime Truncate(DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static double Divide(TimeSpan dividend, TimeSpan divisor)
        {
            return (double)dividend.Ticks / (double)divisor.Ticks;
        }

    }
}