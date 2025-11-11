using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TestCasesSamples
{
    public static class StringUtils
    {
        //<Object> ToAcronym
        //<Author Company='Mercurial Minds'> Aqeel Altaf </Author>
        //<Copyright> Mercurial Minds </Copyright>
        //<Description>function will return acronym from any string</Description>
        //<Parameter Name='inputstr' Direction='Input'>  </Parameter>
        //</Object>
        public static string ToAcronym(this string inputstr)
        {
            if (string.IsNullOrWhiteSpace(inputstr))
                return "";

            return string.Concat(Regex
              .Matches(inputstr, @"\p{L}+")
              .Cast<Match>()
              .Select(match => match.Value[0])).ToUpper();
        }

        //<Object> ToAcronymAlternateWay
        //<Author Company='Mercurial Minds'> Aqeel Altaf </Author>
        //<Copyright> Mercurial Minds </Copyright>
        //<Description>function will return acronym from any string Here is the alternate way of above function</Description>
        //<Parameter Name='inputstr' Direction='Input'>  </Parameter>
        //</Object>
        public static string ToAcronymAlternateWay(this string inputstr)
        {
            string accronym = "";
            int i = 0;
            accronym += inputstr[0];
            for (i = 0; i < inputstr.Length - 1; i++)
            {
                if (inputstr[i] == ' ' || inputstr[i] == '\t' ||
                    inputstr[i] == '\n')
                {
                    accronym += inputstr[i + 1];
                }
            }
            accronym = accronym.ToUpper();
            return accronym;
        }
    }

}
