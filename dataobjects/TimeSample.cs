using System;
using System.Collections.Generic;
using System.Linq;

namespace BleedingSteel
{
    public class TimeSample
    {
        public List<Sample> sampleList { get; }
        public TimeSpan timespanStamp { get; }

        public TimeSample(TimeSpan timespanStamp)
        {
            this.timespanStamp = timespanStamp;
            this.sampleList = new List<Sample>();
        }

        public void addSample(Sample sample)
        {
            sampleList.Add(sample);
        }

        public List<double> getAllSampleValues(){
            List<double> valueList = new List<double>();
            foreach (var sample in sampleList) {
                valueList.Add(sample.value);
            }
            return valueList;
        }

        public bool hasDuplicateSamples(){
            List<string> sampleNameList = new List<string>();
            foreach (var sample in sampleList) {
                if (!sampleNameList.Contains(sample.signalname)){
                    sampleNameList.Add(sample.signalname);
                }
            }
            if (sampleList.Count != sampleNameList.Distinct().Count()) {
                return true;
            }
            return false;
        }

    }
}

