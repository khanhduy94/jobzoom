using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matching
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid jobId = Guid.Parse("B121BDDF-7A43-4DE4-9048-7FF1C90EAD9B");
            Guid profileId = Guid.Parse("F2CC887E-E5F5-4183-A23C-84FE3FDCA068");
            
            JobZoomMatching matching = new JobZoomMatching(profileId, jobId);
            matching.Process();

            Console.WriteLine("Require Point: {0}", matching.RequirePoint);
            Console.WriteLine("Match Point: {0}", matching.MatchingPoint);
            Console.WriteLine("Detail results");
            foreach (var item in matching.Results)
            {
                Console.WriteLine(item.TargetTagID);
                Console.WriteLine(item.IsExists);
                Console.WriteLine(item.IsMatch);
                Console.WriteLine(item.Point);
            }
            Console.ReadKey();

        }
    }
}
