using System;
namespace BleedingSteel
{
    public class Sample
    {
        public DateTime timestamp { get; }
        public TimeSpan zeroTimeSpan { get; set;}
        public TimeSpan sampledTimeSpan { get; set;}
        public double value { get; set;}
        public bool sampled { get; set; }
        public string signalname { get; }
        public bool isHoldSample { get; set;}

        public Sample(double value, DateTime timestamp, string signalname)
        {
            this.value = value;
            this.timestamp = timestamp;
            this.signalname = signalname;
            sampled = false;
        }

        public string getShortSignalName(){
            if (signalname.Split('.')[0].Equals("IO")) {
                return signalname.Split('.')[2];
            }
            return signalname.Split('.')[1];
        }
    }
}

