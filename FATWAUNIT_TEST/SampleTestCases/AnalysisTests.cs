using FATWA_DOMAIN.Models.TestCasesSamples;
using System;
using System.Collections.Generic;
using System.Text;

namespace FATWAUNIT_TEST.SampleTestCases
{
    public class AnalysisTests
    {
        [Fact]
        public void GetStatsTest()
        {
            List<decimal> list1 = new List<decimal>();
            list1.Add((decimal)1.2);
            list1.Add((decimal)2.4);
            list1.Add((decimal)3.5);
            list1.Add((decimal)0.8);
            var result = Analysis.GetStats(list1);
            Assert.NotNull(result);
            Console.WriteLine(result);
        }
    }
}