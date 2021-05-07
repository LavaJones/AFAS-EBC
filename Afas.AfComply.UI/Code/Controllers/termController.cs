using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for termController
/// </summary>
public static class termController
{
    /// <summary>
    /// 
    /// </summary>
    private static List<term> terms = new List<term>();

    public static List<term> getTerms()
    {
        if (terms.Count() < 1)
        {
            generateTerms();
            return terms;
        }
        else
        {
            return terms;
        }
    }

    public static void addTerms(term _term)
    {
        terms.Add(_term);
    }


    public static bool generateTerms()
    {
        if (terms.Count() < 1)
        {
            termShow ts = new termShow();
            bool terms_generated = false;

            terms_generated = ts.manufactureTermList();

            return terms_generated;
        }
        else
        {
            return true;
        }
    }
}