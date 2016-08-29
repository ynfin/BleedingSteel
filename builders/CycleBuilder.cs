using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace BleedingSteel
{
    public class CycleBuilder
    {
        // custom triggers
        public Trigger deliverytrigger { get; private set; }
        public Trigger handlerdowntrigger { get; private set; }
        public Trigger materialchangetrigger { get; private set; }
        public Trigger homepostrigger { get; private set; }
        public Trigger Firsthomepostrigger { get; private set; }

        public Trigger clnifsToDeliveryTrigger { get; private set; }
        public Trigger fillcarttrigger { get; private set; }
        public Trigger clncartToDeliveryTrigger { get; private set; }

        public Trigger fillIfsTrigger { get; private set; }
        public Trigger fillGateTrigger { get; private set; }
        public Trigger flushGateTrigger { get; private set; }
        public Trigger ftCleanTrigger { get; private set; }

        // cycles until delivery
        public List<Cycle> firstJobList { get; private set; }
        public List<Cycle> secondJobList { get; private set; }
        public List<Cycle> thirdJobList { get; private set; }

        public List<Cycle> handlerOutJobs { get; private set; }
        public List<Cycle> handlerDownJobs { get; private set; }
        public List<Cycle> materialChangeJobs { get; private set;}
        public List<Cycle> homeposJobs { get; private set; }

        public List<Cycle> GetCartJobs { get; private set; }
        public List<Cycle> PlaceCartJobs { get; private set; }
        public List<Cycle> homeToMcJobs { get; private set; }

        // cycles for specific modules
        public List<Cycle> fillCartJobs { get; private set; }
        public List<Cycle> clnIfsJobs { get; private set;}
        public List<Cycle> clnCartJobs { get; private set;}

        public List<Cycle> fillIfsJobs { get; private set; }
        public List<Cycle> fillGateJobs { get; private set; }
        public List<Cycle> flushGateJobs { get; private set; }
        public List<Cycle> ftCleanJobs { get; private set; }

        // storage for all jobs
        public List<Job> jobList { get; private set;}
        public List<Cycle> McToHomeJobs { get; private set; }
        public Trigger fillDcuTrigger { get; private set; }
        public List<Cycle> fillDcuJobs { get; private set; }

        public CycleBuilder(List<TimeSample> timeSampleList)
        {
            TimeSample cyclestart = timeSampleList[0];
            List<Trigger> TriggerStack = new List<Trigger>();
            jobList = new List<Job>();

            // build a trigger for batch job cycles, triggering on start of clnifs
            clnifsToDeliveryTrigger = new Trigger("clnifstrigger");
            clnifsToDeliveryTrigger.addCondition(new TriggerCondition("Debug_clnifs", 1, double.MaxValue));
            TriggerStack.Add(clnifsToDeliveryTrigger);

            // build a trigger for batch jobs, starting at clncart
            clncartToDeliveryTrigger = new Trigger("clncarttrigger");
            clncartToDeliveryTrigger.addCondition(new TriggerCondition("Debug_clncart", 1, double.MaxValue));
            TriggerStack.Add(clncartToDeliveryTrigger);

            // build a trigger for fill jobs
            fillcarttrigger = new Trigger("fillcarttrigger");
            fillcarttrigger.addCondition(new TriggerCondition("Debug_fillcart", 1, double.MaxValue));
            TriggerStack.Add(fillcarttrigger);

            // build a trigger for checking when delivery of cartrigde is ready
            deliverytrigger = new Trigger("deliverytrigger");
            deliverytrigger.addCondition(new TriggerCondition("H1AngleAxis", double.MinValue, 16));
            TriggerStack.Add(deliverytrigger);

            // build a trigger for checking when handler is down
            handlerdowntrigger = new Trigger("handlerDownTrigger");
            handlerdowntrigger.addCondition(new TriggerCondition("H1VertAxis", double.MinValue, 5));
            handlerdowntrigger.addCondition(new TriggerCondition("H1AngleAxis", 65, double.MaxValue));
            handlerdowntrigger.addCondition(new TriggerCondition("H1AngleAxis", double.MinValue,75));
            TriggerStack.Add(handlerdowntrigger);

            // build a trigger for checking when applicator is in materialchange position
            materialchangetrigger = new Trigger("materialchangepos");
            materialchangetrigger.addCondition(new TriggerCondition("MtrlChangePos", 1,double.MaxValue));
            TriggerStack.Add(materialchangetrigger);

            // homepos
            homepostrigger = new Trigger("homepos");
            homepostrigger.addCondition(new TriggerCondition("HomePos", 1, double.MaxValue));
            TriggerStack.Add(homepostrigger);

            // firsthomepos
            Firsthomepostrigger = new Trigger("firsthomepos");
            Firsthomepostrigger.addCondition(new TriggerCondition("HomePos", 1, double.MaxValue));
            Firsthomepostrigger.addCondition(new TriggerCondition("H1AngleAxis", double.MinValue, 16));
            TriggerStack.Add(Firsthomepostrigger);

            // length of fillifs
            fillIfsTrigger = new Trigger("fillIfsTrigger");
            fillIfsTrigger.addCondition(new TriggerCondition("Debug_fillifs", 1, double.MaxValue));
            TriggerStack.Add(fillIfsTrigger);

            // length of fillgate
            fillGateTrigger = new Trigger("fillGateTrigger");
            fillGateTrigger.addCondition(new TriggerCondition("Debug_fillgate", 1, double.MaxValue));
            TriggerStack.Add(fillGateTrigger);

            // length of fillifs
            flushGateTrigger = new Trigger("flushGateTrigger");
            flushGateTrigger.addCondition(new TriggerCondition("Debug_flushgate", 1, double.MaxValue));
            TriggerStack.Add(flushGateTrigger);

            // length of fillifs
            ftCleanTrigger = new Trigger("ftCleanTrigger");
            ftCleanTrigger.addCondition(new TriggerCondition("Debug_ftclean", 1, double.MaxValue));
            TriggerStack.Add(ftCleanTrigger);

            // length of filldcu
            fillDcuTrigger = new Trigger("fillDcuTrigger");
            fillDcuTrigger.addCondition(new TriggerCondition("Debug_filldcu", 1, double.MaxValue));
            TriggerStack.Add(fillDcuTrigger);


            // check all triggers
            foreach (var timesample in timeSampleList) {
                foreach (var trigger in TriggerStack) {
                    trigger.checktrigger(timesample);
                }

                //deliverytrigger.checktrigger(timesample);

                //filltrigger.checktrigger(timesample);
                //clnifstrigger.checktrigger(timesample);
                //clncarttrigger.checktrigger(timesample);


            }

            firstJobList = buildCyclesToDelivery(clnifsToDeliveryTrigger, deliverytrigger,timeSampleList);
            secondJobList = buildCyclesToDelivery(clncartToDeliveryTrigger, deliverytrigger,timeSampleList);
            thirdJobList = buildCyclesToDelivery(fillcarttrigger, deliverytrigger,timeSampleList);

            GetCartJobs = buildCustomCycles(materialchangetrigger.HighFlankList, handlerdowntrigger.HighFlankList, timeSampleList, "getcart");
            PlaceCartJobs = buildCustomCycles(ftCleanTrigger.LowFlankList, deliverytrigger.HighFlankList, timeSampleList,"placecart");
            homeToMcJobs = buildCustomCycles(Firsthomepostrigger.LowFlankList, materialchangetrigger.HighFlankList, timeSampleList, "homeToMc");
            McToHomeJobs = buildCustomCycles(materialchangetrigger.LowFlankList, homepostrigger.HighFlankList, timeSampleList, "McToHome");

            clnIfsJobs = buildCycles(clnifsToDeliveryTrigger, timeSampleList);
            clnCartJobs = buildCycles(clncartToDeliveryTrigger, timeSampleList);
            fillCartJobs = buildCycles(fillcarttrigger, timeSampleList);
            fillIfsJobs = buildCycles(fillIfsTrigger, timeSampleList);
            fillGateJobs = buildCycles(fillGateTrigger, timeSampleList);
            flushGateJobs = buildCycles(flushGateTrigger, timeSampleList);
            ftCleanJobs = buildCycles(ftCleanTrigger, timeSampleList);
            fillDcuJobs = buildCycles(fillDcuTrigger, timeSampleList);

            handlerOutJobs = buildCycles(deliverytrigger, timeSampleList);
            handlerDownJobs = buildCycles(handlerdowntrigger, timeSampleList);
            materialChangeJobs = buildCycles(materialchangetrigger, timeSampleList);
            homeposJobs = buildCycles(homepostrigger, timeSampleList);

            // [first job] from start of clnifs to ready for delivery
            Job firstjob = new Job("FirstJob", firstJobList);
            firstjob.whiteListSignal("H1Fluid");
            firstjob.whiteListSignal("DCU2");
            jobList.Add(firstjob);

            // [first job] time it takes to do cleanifs
            Job clnifsjob = new Job("clnifs", clnIfsJobs);
            jobList.Add(clnifsjob);

            // [first job] fillifs
            Job fillifsjob = new Job("fillifs", fillIfsJobs);
            jobList.Add(fillifsjob);

            // [second job] fillcart to delivery
            Job secondjob = new Job("Secondjob", secondJobList);
            secondjob.whiteListSignal("H1Fluid");
            secondjob.whiteListSignal("DCU2");
            jobList.Add(secondjob);

            // [second job] cleancart
            Job clncartjob = new Job("clncart", clnCartJobs);
            jobList.Add(clncartjob);

            // [second job] fillgate
            Job fillgatejob = new Job("fillgate", fillGateJobs);
            jobList.Add(fillgatejob);

            // [steady state jobs] fillcart to delivery
            Job thirdjob = new Job("Thirdjob", thirdJobList);
            jobList.Add(thirdjob);

            Job flushgatejob = new Job("flushgate", flushGateJobs);
            jobList.Add(flushgatejob);

            Job fillcartjob = new Job("Fillcartjob", fillCartJobs);
            jobList.Add(fillcartjob);

            Job ftcleanjob = new Job("ftclean", ftCleanJobs);
            jobList.Add(ftcleanjob);

            // misc jobs
            Job handleroutjob = new Job("handlerout", handlerOutJobs);
            jobList.Add(handleroutjob);

            Job handlerdownjob = new Job("handleronfillingcart", handlerDownJobs);
            jobList.Add(handlerdownjob);

            Job placecart = new Job("presentcart", PlaceCartJobs);
            jobList.Add(placecart);

            Job filldcujob = new Job("filldcu", fillDcuJobs);
            jobList.Add(filldcujob);

            Job hometomcjob = new Job("homeToMc", homeToMcJobs);
            jobList.Add(hometomcjob);

            Job materialchangejob = new Job("materialchangepos", materialChangeJobs);
            jobList.Add(materialchangejob);

            Job mctohomejob = new Job("McTohome", McToHomeJobs);
            jobList.Add(mctohomejob);

            Job homeposjob = new Job("homepos", homeposJobs);
            jobList.Add(homeposjob);

            Job getcartjob = new Job("getcart", GetCartJobs);
            jobList.Add(getcartjob);

            foreach (var job in jobList) {
                job.purgeObsoleteSignals();
            }

        }

        private List<Cycle> buildCustomCycles(List<TimeSample> highFlankList1, List<TimeSample> highFlankList2, List<TimeSample> timeSampleList,string name)
        {
            List<Cycle> returnlist = new List<Cycle>();

            foreach (var triggtime in highFlankList1)
            {
                TimeSpan cyclestartstamp = triggtime.timespanStamp;

                foreach (var deliverystamp in highFlankList2)
                {
                    if (cyclestartstamp.CompareTo(deliverystamp.timespanStamp) < 0)
                    {
                        var cycleendstamp = deliverystamp.timespanStamp;
                        List<TimeSample> samplerange = new List<TimeSample>();

                        foreach (var sample in timeSampleList)
                        {
                            if ((sample.timespanStamp.CompareTo(cyclestartstamp) > 0) && (sample.timespanStamp.CompareTo(cycleendstamp) < 0))
                            {
                                samplerange.Add(sample);
                            }
                        }
                        returnlist.Add(new Cycle(cyclestartstamp, cycleendstamp, name, samplerange));
                        break;
                    }
                }
            }
            foreach (var cycles in returnlist)
            {
                Console.WriteLine(cycles);
            }

            return returnlist;
        }

        private List<Cycle> buildCycles(Trigger intrigger, List<TimeSample> timeSampleList)
        {
            List<Cycle> returnlist = new List<Cycle>();

            foreach (var HighTriggtime in intrigger.HighFlankList) {
                TimeSpan cyclestartstamp = HighTriggtime.timespanStamp;

                foreach (var LowTriggtime in intrigger.LowFlankList) {
                    if (cyclestartstamp.CompareTo(LowTriggtime.timespanStamp) < 0) {
                        var cycleendstamp = LowTriggtime.timespanStamp;
                        List<TimeSample> samplerange = new List<TimeSample>();

                        foreach (var sample in timeSampleList) {
                            if ((sample.timespanStamp.CompareTo(cyclestartstamp) > 0) && (sample.timespanStamp.CompareTo(cycleendstamp) < 0)) {
                                samplerange.Add(sample);
                            }
                        }
                        returnlist.Add(new Cycle(cyclestartstamp, cycleendstamp, intrigger.triggername, samplerange));
                        break;
                    }
                }
            }
            foreach (var cycles in returnlist) {
                Console.WriteLine(cycles);
            }
            return returnlist;
        }

        private List<Cycle> buildCyclesToDelivery(Trigger startTrigger, Trigger deliveryTrigger, List<TimeSample> fullSampleList ){

            List<Cycle> returnlist = new List<Cycle>();
            List<TimeSample> inputFlankList = startTrigger.HighFlankList;
            List<TimeSample> deliveryFlankList = deliveryTrigger.HighFlankList;

            foreach (var triggtime in inputFlankList) {
                TimeSpan cyclestartstamp = triggtime.timespanStamp;

                foreach (var deliverystamp in deliveryFlankList) {
                    if (cyclestartstamp.CompareTo(deliverystamp.timespanStamp) < 0) {
                        var cycleendstamp = deliverystamp.timespanStamp;
                        List<TimeSample> samplerange = new List<TimeSample>();

                        foreach (var sample in fullSampleList) {
                            if ((sample.timespanStamp.CompareTo(cyclestartstamp) > 0) && (sample.timespanStamp.CompareTo(cycleendstamp) < 0)) {
                                samplerange.Add(sample);
                            }
                        }
                        returnlist.Add(new Cycle(cyclestartstamp, cycleendstamp, startTrigger.triggername, samplerange));
                        break;
                    }
                }
            }
            foreach (var cycles in returnlist) {
                Console.WriteLine(cycles);
            }

            return returnlist;

        }
    }
}

