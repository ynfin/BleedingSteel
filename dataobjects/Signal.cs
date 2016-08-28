using System;
using System.Collections.Generic;
using System.Linq;

namespace BleedingSteel
{
    public class Signal
    {

        public string name { get; }
        public List<SingleSignalSample> sampleValues { get; }
        public int samplemark = 0;

        public Signal(string name)
        {
            this.sampleValues = new List<SingleSignalSample>();
            this.name = name;
        }

        public void addNewSample(double value, DateTime timestamp){
            sampleValues.Add(new SingleSignalSample(value, timestamp,name));
        }

        public DateTime getStartTime(){
            return sampleValues.First().realTimestamp;
        }

        public DateTime getEndTime(){
            return sampleValues.Last().realTimestamp;
        }
    
    }
}

