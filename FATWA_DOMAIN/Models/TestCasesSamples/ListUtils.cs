using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TestCasesSamples
{
    public static class ListUtils
    {
        //<Object> Diff
        //<Author Company='Mercurial Minds'> Aqeel Altaf </Author>
        //<Copyright> Mercurial Minds </Copyright>
        //<Description>return difference between two lists</Description>
        //<Parameter Name='list1' Direction='Input'>  </Parameter>
        //<Parameter Name='list2' Direction='Input'>  </Parameter>
        //</Object>
        public static List<string> Diff(this List<string> list1, List<string> list2)
        {
            List<string> inList1ButNotList2 = (from o in list1
                                               join p in list2 on o equals p into t
                                               from od in t.DefaultIfEmpty()
                                               where od == null
                                               select o).ToList<string>();
            return inList1ButNotList2;
        }

        //<Object> DiffSecondWay
        //<Author Company='Mercurial Minds'> Aqeel Altaf </Author>
        //<Copyright> Mercurial Minds </Copyright>
        //<Description>return difference between two lists</Description>
        //<Parameter Name='list1' Direction='Input'>  </Parameter>
        //<Parameter Name='list2' Direction='Input'>  </Parameter>
        //</Object>
        public static List<string> DiffSecondWay(this List<string> list1, List<string> list2)
        {
            var result = list1.Except(list2).ToList();
            return (List<string>)result;
        }
    }
}
