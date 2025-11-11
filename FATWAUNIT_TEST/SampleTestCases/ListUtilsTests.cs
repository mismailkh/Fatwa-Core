using FATWA_DOMAIN.Models.TestCasesSamples;
using System;
using System.Collections.Generic;
using System.Text;

namespace FATWAUNIT_TEST.SampleTestCases
{
    public class ListUtilsTests
    {
        [Fact]
        public void DiffTest()
        {
            //First input
            List<string> list1 = new List<string>() { "1", "2", "3" };
            List<string> list2 = new List<string>() { "2" };
            int ExpectedResultCount = 2;
            List<string> result = ListUtils.Diff(list1, list2);
            Assert.Equal(ExpectedResultCount, result.Count);
            Console.Write(result);
            //Second input
            List<string> list1SecondInput = new List<string>() { "a", "b"};
            List<string> list2SecondInput = new List<string>() { "c" };
            int ExpectedResultCount2 = 2;
            List<string> result2 = ListUtils.Diff(list1SecondInput, list2SecondInput);
            Assert.Equal(ExpectedResultCount2, result2.Count);
            Console.Write(result2);
            //Third input
            List<string> list1ThirdInput = new List<string>() { "1", "2", "2","3" };
            List<string> list2ThirdInput = new List<string>() { "2" };
            int ExpectedResultCount3 = 2;
            List<string> result3 = ListUtils.Diff(list1ThirdInput, list2ThirdInput);
            Assert.Equal(ExpectedResultCount3, result3.Count);
            Console.Write(result3);
        }
        [Fact]
        public void DiffSecondWayTest()
        {
            List<string> list1 = new List<string>() { "1", "2", "2", "3" };
            List<string> list2 = new List<string>() { "2" };
            int ExpectedResultCount = 2;
            List<string> result = ListUtils.DiffSecondWay(list1, list2);
            Assert.Equal(ExpectedResultCount, result.Count);
            Console.Write(result);
        }
    }
}