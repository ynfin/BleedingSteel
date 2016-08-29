using System;
using System.Collections.Generic;
using System.IO;

namespace BleedingSteel
{
    class MainClass
    {
        public static void Main(string [] args)
        {
            inout io = new inout();
            //LinSpacer linspacer = new LinSpacer();
            Logfile logfile;
            SignalBuilder signalbuilder;
            CycleBuilder cyclebuilder;

            List<TimeSample> timeSampleList;

            string[] files = System.IO.Directory.GetFiles(@"C:\!WORK\GapInProgress\_In Progress\GAP - RawData", "*.csv", SearchOption.AllDirectories);

            //foreach (var filepath in files)
            //{
                string filepath = @"/Users/yngve/Dropbox/500water9bar_1of5.csv";

                Console.WriteLine("----- working on: " + filepath);

                string outfile = filepath + "_timeline.html";

                // read file from local disk
                //string filepath = @"/Users/yngve/Dropbox/multisampletest.csv";
                //string filepath = @"/Users/yngve/Dropbox/500water9bar_1of5.csv";
                logfile = new Logfile(filepath);

                signalbuilder = new SignalBuilder(logfile.content);
                timeSampleList = signalbuilder.linearizeRawData(5);

                cyclebuilder = new CycleBuilder(timeSampleList);

                //io.printTimeLineXml(cyclebuilder.jobList,@"/Users/yngve/Dropbox/500water9bar_1of5_timeline.xml");
                //io.printXmlFromTimeSamples(cyclebuilder.jobList,@"/Users/yngve/Dropbox/500water9bar_1of5.xml");
                //io.printFullDataMatrix(signalbuilder.TimeList, @"/Users/yngve/Dropbox/500water9bar_1of5.output");

                //io.printTimelineHtml(cyclebuilder.jobList, @"/Users/yngve/Dropbox/500water9bar_1of5_timeline.html");
                io.printTimelineHtml(cyclebuilder.jobList, outfile);

                // linearize signal from log
                //List<TimeSample> linearSamples = linspacer.linearize(logfile.content, 50);

                // plot
                //Console.WriteLine("starting writing");
                //io.writeCsvFile(linearSamples, @"/Users/yngve/Dropbox/DEFAC_Logs/DEFAC2_water_fullCC_linearized.csv");
            //}




        }

    }
}
