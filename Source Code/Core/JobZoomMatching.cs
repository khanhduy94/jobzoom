using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Business.Entities;

namespace JobZoom.Core
{
    public class JobZoomMatching
    {
        public string SourceID { get; set; }
        public string TargetID { get; set; }
        public int RequirePoint { get; private set; }
        public int MatchingPoint { get; private set; }
        public IEnumerable<MatchingResult> Results { get; set; }

        public JobZoomMatching(string sourceID, string targetID)
        {
            SourceID = sourceID;
            TargetID = targetID;
        }

        public void Process()
        {
            JobZoomEntities db = new JobZoomEntities();
            var sourceTags = db.AttributeTags.Where(t => t.ObjectId == Guid.Parse(SourceID)).ToArray();
            var targetTags = db.AttributeTags.Where(t => t.ObjectId == Guid.Parse(TargetID)).ToArray();
            RequirePoint = targetTags.Where(i => i.Required == true).Sum(i => (i.Weight != null ? i.Weight.Value : 0) * (i.Level != null ? i.Level.Value : 0));
            Results = Matching(sourceTags, targetTags);
            MatchingPoint = Results.Sum(i => i.Point);
        }

        private IEnumerable<MatchingResult> Matching(Tag[] source, Tag[] target)
        {
            List<MatchingResult> matchingResults = new List<MatchingResult>();
            foreach (var item in target)
            {                
                var obj = source.FirstOrDefault(t => t.TagName.Equals(item.TagName) && t.ParentID.Equals(item.ParentID));
                if (obj != null)
                {
                    MatchingResult result = new MatchingResult();
                    result.TargetTagID = item.ID;
                    result.IsExists = true;
                    if (item.Required.GetValueOrDefault())
                    {
                        result.Point = (item.Weight * obj.Level).Value;
                    }

                    if (obj.Level >= item.Level)
                    {
                        result.IsMatch = true;
                    }
                    matchingResults.Add(result);
                }
            }
            return matchingResults;
        }
    }
}