using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matching
{
    public class JobZoomMatching
    {
        public Guid SourceID { get; set; }
        public Guid TargetID { get; set; }
        public int RequirePoint { get; private set; }
        public int MatchingPoint { get; private set; }
        public IEnumerable<MatchingResult> Results { get; set; }
        public const float SimilarityRate = 0.75f;

        public JobZoomMatching(Guid sourceID, Guid targetID)
        {
            SourceID = sourceID;
            TargetID = targetID;
        }

        public void Process()
        {
            JobZoomEntities db = new JobZoomEntities();
            var sourceTags = db.TagAttributes.Where(t => t.ObjectId == SourceID).ToArray();
            var targetTags = db.TagAttributes.Where(t => t.ObjectId == TargetID).ToArray();
            RequirePoint = targetTags.Where(i => i.Required == true).Sum(i => (i.Weight != null ? i.Weight.Value : 0) * (i.Level != null ? i.Level.Value : 0));
            Results = Matching(sourceTags, targetTags);
            MatchingPoint = Results.Sum(i => i.Point);
        }

        private IEnumerable<MatchingResult> Matching(TagAttribute[] source, TagAttribute[] target)
        {
            JobZoomEntities db = new JobZoomEntities();
            List<MatchingResult> matchingResults = new List<MatchingResult>();
            foreach (var item in target)
            {
                var obj = source.FirstOrDefault(t => t.TagName.Equals(item.TagName) && t.ParentId.Equals(item.ParentId));
                // If not match exactly
                if (obj == null)
                {
                    // Find similar keywords of source TagName
                    string[] term = FindSimilarityTerm(item.TagName);

                    // Matching again with similar keywords
                    var similarObj = source.Where(t => term.Contains(t.TagName) && t.ParentId.Equals(item.ParentId)).OrderByDescending(o => o.Level).FirstOrDefault();
                    if (similarObj != null)
                    {
                        MatchingResult result = new MatchingResult();
                        result.TargetTagID = item.TagId.ToString();
                        result.IsExists = true;

                        // If this is require criteria
                        if (item.Required.GetValueOrDefault())
                        {
                            // Calculate the point
                            result.Point = (item.Weight * similarObj.Level).Value;
                        }

                        // If equal or greater than require level
                        if (similarObj.Level >= item.Level)
                        {
                            result.IsMatch = true;
                        }
                        matchingResults.Add(result);
                    }
                }
                // If Match
                else
                {
                    MatchingResult result = new MatchingResult();
                    result.TargetTagID = item.TagId.ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string[] FindSimilarityTerm(string keyword)
        {
            JobZoomEntities db = new JobZoomEntities();
            string[] term = db.SimilarityTerms.Where(t => t.Keyword1.Equals(keyword) && t.Rate >= SimilarityRate).Select(t => t.Keyword2).ToArray();
            string[] term2 = db.SimilarityTerms.Where(t => t.Keyword2.Equals(keyword) && t.Rate >= SimilarityRate).Select(t => t.Keyword1).ToArray();
            term = term.Concat(term2).ToArray();
            return term;
        }
    }
}