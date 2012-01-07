using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobZoom.Core.Entities;
using JobZoom.Core.Taxonomy;

namespace JobZoom.Core.Matching
{
    public class MatchingTool
    {
        public double RequirePoint { get; private set; }
        public double MatchingPoint { get; private set; }
        public IEnumerable<MatchingResult> Results { get; private set; }       

        public void Match(Guid sourceId, Guid targetId)
        {
            try
            {
                JobZoomCoreEntities db = new JobZoomCoreEntities();
                List<TagAttribute> sourceTag = db.TagAttributes.Where(i => i.ObjectId == sourceId).ToList();
                List<TagAttribute> targetTag = db.TagAttributes.Where(i => i.ObjectId == targetId).ToList();

                RequirePoint = targetTag.Sum(i => (i.Weight != null ? i.Weight.Value : 0));
                Results = Process(sourceTag, targetTag);
                MatchingPoint = Results.Where(i => i.IsMatchAbsolute == true).Sum(i => (i.TagMatch.Weight != null ? i.TagMatch.Weight.Value : 0));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<MatchingResult> Process(List<TagAttribute> sourceTag, List<TagAttribute> targetTag)
        {
            List<MatchingResult> matchingResults = new List<MatchingResult>();
            foreach (var target in targetTag.Where(i => i.ObjectDeepLevel == 3))
            {
                var obj = sourceTag.FirstOrDefault(i => i.TagName.Equals(target.TagName) && i.ParentName.Equals(target.ParentName));
                if (obj == null)
                {
                    // Find similarity term
                    string term = FindSimilarityTerm(target.TagName, sourceTag.Where(i => i.ObjectDeepLevel == 3).Select(i => i.TagName).ToArray());
                    if (!string.IsNullOrEmpty(term))
                    {
                        obj = sourceTag.FirstOrDefault(t => t.TagName.Equals(term) && t.ParentName.Equals(target.ParentName));
                    }
                }

                if (obj != null)
                {
                    MatchingResult result = new MatchingResult();
                    result.TagMatch = target;

                    // Matching attribute value
                    switch (target.ValueType)
                    {
                        case "text":
                            if (target.TagValue.Trim().ToLower() == obj.TagValue.Trim().ToLower())
                            {
                                result.IsMatchAbsolute = true;
                            }
                            break;

                        case "numberic":
                            double targetValue = double.Parse(target.TagValue);
                            double sourceValue = double.Parse(obj.TagValue);
                            switch (target.Criteria)
                            {
                                case "LT":
                                    if (sourceValue < targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                case "LE":
                                    if (sourceValue <= targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                case "EQ":
                                    if (sourceValue == targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                case "NE":
                                    if (sourceValue != targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                case "GE":
                                    if (sourceValue >= targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                case "GT":
                                    if (sourceValue > targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                                default:
                                    if (sourceValue > targetValue)
                                    {
                                        result.IsMatchAbsolute = true;
                                    }
                                    break;
                            }
                            break;
                    }

                    matchingResults.Add(result);
                }

            }

            return matchingResults;
        }

        private string FindSimilarityTerm(string keyword, string[] tagName)
        {
            JobZoomCoreEntities db = new JobZoomCoreEntities();
            SimilarityTerm term1 = db.SimilarityTerms.Where(t => t.Keyword2.Equals(keyword) && tagName.Contains(t.Keyword1)).OrderByDescending(t => t.Rate).FirstOrDefault();
            SimilarityTerm term2 = db.SimilarityTerms.Where(t => t.Keyword1.Equals(keyword) && tagName.Contains(t.Keyword2)).OrderByDescending(t => t.Rate).FirstOrDefault();

            if (term1 != null && term2 != null)
            {
                if (term1.Rate >= term2.Rate)
                {
                    return term1.Keyword1;
                }
                else
                {
                    return term2.Keyword2;
                }
            }

            if (term1 != null && term2 == null)
            {
                return term1.Keyword1;
            }
            else if (term1 == null && term2 != null)
            {
                return term2.Keyword2;
            }
            else
                return null;
        }
    }
}