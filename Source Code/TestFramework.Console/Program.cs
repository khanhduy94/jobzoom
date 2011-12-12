using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobZoom.Core.Framework;

namespace TestFramework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            JobZoomEntities db = new JobZoomEntities();
            db.SimilarityTerms.Add(new SimilarityTerm {ID= "test", Keyword1= "kw1", Keyword2="kw2", Rate=20 });
            db.SaveChanges();
        }
    }
}
