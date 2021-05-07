using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for employer_typeController
/// </summary>
public class employer_typeController
{
    /// <summary>
    /// 
    /// </summary>
    private static List<employer_type> empTypes = new List<employer_type>();

    public static List<employer_type> getEmployerTypes()
    {
        if (empTypes.Count() < 1)
        {
            generateEmployerTypes();
            return empTypes;
        }
        else
        {
            return empTypes;
        }
    }

    public static void addEmployerType(employer_type _empType)
    {
        empTypes.Add(_empType);
    }


    public static bool generateEmployerTypes()
    {
        if (empTypes.Count() < 1)
        {
            employer_typeFactory ef = new employer_typeFactory();
            bool types_generated = false;

            types_generated = ef.manufactureEmployerType();

            return types_generated;
        }
        else
        {
            return true;
        }
    }
}