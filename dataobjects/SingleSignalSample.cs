using System;
namespace BleedingSteel
{
    public class SingleSignalSample
    {
        public string name { get; }
        public double value { get; }
        public DateTime realTimestamp { get; }

        // when building signal
        public SingleSignalSample(double value, DateTime timestamp,string name)
        {
            this.name = name;
            this.value = value;
            this.realTimestamp = timestamp;
        }

        // when building timeline
        public SingleSignalSample(double value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }
}

