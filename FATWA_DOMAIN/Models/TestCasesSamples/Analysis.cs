using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TestCasesSamples
{
    public struct Averages
    {
        public decimal Min;
        public decimal Max;
        public decimal Avg;
    };
    public static class Analysis
    {
        //<Object> GetStats
        //<Author Company='Mercurial Minds'> Aqeel Altaf </Author>
        //<Copyright> Mercurial Minds </Copyright>
        //<Description>return difference between two lists</Description>
        //<Parameter Name='list1' Direction='Input'>  </Parameter>
        //<Parameter Name='list2' Direction='Input'>  </Parameter>
        //</Object>
        public static Averages GetStats(this List<decimal> list1)
        {
            Averages average = new Averages();
            average.Avg = list1.Count() > 0 ? list1.Average() : 0;
            average.Max = list1.Count() > 0 ? list1.Max() : 0;
            average.Min = list1.Count() > 0 ? list1.Min() : 0;
            return average;
        }
    }
}
