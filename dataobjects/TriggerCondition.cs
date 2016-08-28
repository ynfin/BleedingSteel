using System;
namespace BleedingSteel
{
    public class TriggerCondition
    {
        public string triggersignal { get; private set; }
        public double lowrange { get; private set; }
        public double highrange { get; private set; }

        public TriggerCondition(string signalname, double lowrange, double highrange )
        {
            this.triggersignal = signalname;
            this.lowrange = lowrange;
            this.highrange = highrange;
        }

        public bool check(TimeSample timesample){
            foreach (var sample in timesample.sampleList) {
                if (sample.signalname.Contains(triggersignal)) {
                    return ((sample.value < highrange) && (sample.value >= lowrange));
                }
            }
            return false;
        }
    }
}

