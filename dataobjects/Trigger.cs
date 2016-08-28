using System;
using System.Collections.Generic;
namespace BleedingSteel
{
    public class Trigger
    {
        private List<TriggerCondition> conditionList = new List<TriggerCondition>();
        public string triggername { get; }
        private List<TimeSample> triggerTrueTimeStamps { get; }

        public List<TimeSample> HighFlankList { get; }
        public List<TimeSample> LowFlankList { get; }
        public bool lastCheckValue = false;


        public Trigger(string triggername)
        {
            this.triggername = triggername;
            HighFlankList = new List<TimeSample>();
            LowFlankList = new List<TimeSample>();
        }

        public void addCondition(TriggerCondition condition){
            conditionList.Add(condition);
        }

        public void printAllTriggers(){
            Console.WriteLine("-- triggers in " + triggername + " --");
            foreach (var triggertime in HighFlankList) {
                Console.WriteLine(triggertime.timespanStamp);
            }
            Console.WriteLine("\n");
        }


        public bool checktrigger(TimeSample timesample){
            int requiredconditions = conditionList.Count;
            int satisfiedconditions = 0;

            foreach (var condition in conditionList) {
                if (condition.check(timesample)) {
                    satisfiedconditions++;
                }
            }
            if (satisfiedconditions == requiredconditions) {
                if (!lastCheckValue) {
                    HighFlankList.Add(timesample);
                }
                lastCheckValue = true;
                return true;
            }

            // nothing triggered, all is false
            if (lastCheckValue) {
                LowFlankList.Add(timesample);
                lastCheckValue = false;
            }
            return false;
        }

    }
}

