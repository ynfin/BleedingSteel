using System;
using System.Collections.Generic;
using System.Linq;

namespace BleedingSteel
{
    public class Job
    {
        public string jobName { get; private set; }
        public List<Cycle> triggeredCycles { get; set;}
        public List<string> whiteListedSignals { get; private set;}

        // this will hold all data for the created cycles resulting in a successfull trigg!
        public Job(string jobName, List<Cycle> triggeredCycles)
        {
            this.jobName = jobName;
            this.triggeredCycles = triggeredCycles;
            whiteListedSignals = new List<string>();
        }

        internal void purgeObsoleteSignals()
        {
            if (whiteListedSignals.Count == 0)
                return;
            
            foreach (var cycle in triggeredCycles) {
                foreach (var timesample in cycle.timeSampleList) {
                    List<Sample> sceduledForRemoval = new List<Sample>();
                    foreach (var sample in timesample.sampleList) {
                        bool b = whiteListedSignals.Any(s => sample.signalname.Contains(s));
                        if (!b)
                            sceduledForRemoval.Add(sample);
                    }
                    foreach (var removal in sceduledForRemoval) {
                        timesample.sampleList.Remove(removal);
                    }
                }
            }
        }

        public void whiteListSignal(string signalname){
            whiteListedSignals.Add(signalname);
        }

    }
}

