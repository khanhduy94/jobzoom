using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core;
using JobZoom.Core.Framework;
using JobZoom.Core.Framework.DataMining;
using JobZoom.Core.Entities;


namespace JobZoom.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            //string MainServerConnectionString = "Data Source=TRUNGHIEU-PC; Initial Catalog=JobZoom; Integrated Security=SSPI;";
            //string TempServerConnectionString = "Data Source=TRUNGHIEU-PC; Initial Catalog=JobZoom; Integrated Security=SSPI;";
            //string AnalysisServerConnectionString = "Data Source=TRUNGHIEU-PC; Provider=msolap;";


            //Console.WriteLine("Export View...");
            //ExportViewToMining.Export(MainServerConnectionString, TempServerConnectionString, null, "GetPivotProfile", "PF");
            //ExportViewToMining.Export(MainServerConnectionString, TempServerConnectionString, null, "GetPivotJob", "JB");

            //Console.WriteLine("Build Mining Database...");
            //MiningDatabaseGenerator.BuildMiningDatabase_Console(MainServerConnectionString, AnalysisServerConnectionString, TempServerConnectionString, "PF", new DecisionTreeAlgorithmParameters());


            string[] att = new string[] { };
            string[] ex_att = new string[] {  };
            List<DecisionTreeAnalysisResult> results = new List<DecisionTreeAnalysisResult>();
            //results = getAnalysisResults("Developer Evangelist", CompareType.GreaterThanOrEqualTo, 0.5);
            results = DecisionTreeAnalysis.getAnalysisResults(DecisionTreeAnalysis.convertJobTitleNameToModelName("Developer Evangelist", "PF"), att, ex_att, CompareType.GreaterThanOrEqualTo, 0.5);
            foreach (var result in results)
            {
                Console.WriteLine("NODE_ID = " + result.Node.NODE_ID);
                foreach (var caption in result.NodeDescription.NodeCaptions)
                {
                    Console.WriteLine(caption.Name + " = " + caption.Value + " with Probability = " + result.getDetailProbability());
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
