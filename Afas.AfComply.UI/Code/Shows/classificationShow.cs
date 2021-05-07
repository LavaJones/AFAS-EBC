using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for classificationShow
/// </summary>
public class classificationShow
{
    public bool validClassification(int _classID, List<classification> tempList)
    {
        
        bool validData = false;

        foreach (classification cl in tempList)
        {
            if (cl.CLASS_ID == _classID)
            {
                validData = true;
                break;
            }
        }

        return validData;
    }


    public classification findClassification(int _classID, List<classification> tempList)
    {
        classification myClassification = null; 

        foreach (classification cl in tempList)
        {
            if (cl.CLASS_ID == _classID)
            {
                myClassification = cl;
                break;
            }
        }

        return myClassification;
    }

    public List<classification> getClassificationActiveOnly(List<classification> tempList)
    {
        List<classification> activeClassifications = new List<classification>();

        foreach (classification cl in tempList)
        {
            if (cl.CLASS_ENTITY_STATUS == 1)
            {
                activeClassifications.Add(cl);
            }
        }

        return activeClassifications;
    }
}