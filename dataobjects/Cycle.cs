using System;
using System.Collections.Generic;
namespace BleedingSteel
{
    public class Cycle
    {
        TimeSpan startflank;
        TimeSpan endflank;
        public string cyclename { get; set;}


        public List<TimeSample> timeSampleList { get; private set;}

        public Cycle(TimeSpan startflank, TimeSpan endflank, string name, List<TimeSample> samplerange)
        {
            this.startflank = startflank;
            this.endflank = endflank;
            this.cyclename = name;
            this.timeSampleList = samplerange;
        }

        public TimeSpan getStartStamp(){
            return startflank;
        }

        public TimeSpan getEndStamp(){
            return endflank;
        }

        public TimeSpan getDuration(){
            return (endflank - startflank);
        }

        public override string ToString(){
            return "Cycle "+ cyclename +" start: " + startflank + " end: " + endflank + " duration: " + (endflank-startflank).TotalSeconds + " samples: " + timeSampleList.Count;
        }
    }
}

