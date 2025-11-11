using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FATWA_DOMAIN.Models.TestCasesSamples;

namespace FATWAUNIT_TEST.SampleTestCases
{
    public class StringUtilsTests
    {
        [Fact]
        public void ToAcronymTest()
        {
            string[] Input = new string[] {
                "Mobile network operator",
                "Example 1",
                "A quick brown fox jumps over the lazy dog",
                "National Aeronautics, Space Administration",
                "a b c"
             };
            string AcronymResult = string.Join(Environment.NewLine, Input
             .Select(Input => $"{Input,-25} => {Input.ToAcronym()}"));
            Console.Write(AcronymResult);
        }

        [Fact]
        public void ToAcronymAlternateWayTest()
        {
           string actualOutput = string.Empty;
           string input = "Mobile network operator";
           string expectedOutput1 = "MNO";

           actualOutput = input.ToAcronymAlternateWay();
           Assert.Equal(expectedOutput1, actualOutput);

           string input2 = "Example 1";
           string expectedOutput2 = "E1";
           actualOutput = input2.ToAcronymAlternateWay();
           Assert.Equal(expectedOutput2, actualOutput);

           string input3 = "a b c";
           string expectedOutput3 = "ABC";
           actualOutput = input3.ToAcronymAlternateWay();
           Assert.Equal(expectedOutput3, actualOutput);           
            
        }
    }
}