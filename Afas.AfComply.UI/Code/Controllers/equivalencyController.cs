using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for equivalencyController
/// </summary>
public static class equivalencyController
{
    private static List<position> positions = new List<position>();
    private static List<activity> activities = new List<activity>();
    private static List<detail> details = new List<detail>();

    /********************************************************************************
     * Equivalency Position Objects *************************************************
     *******************************************************************************/
    public static void addPosition(position p)
    {
        positions.Add(p);
    }

    public static List<position> getPositions()
    {
        if (positions.Count() < 1)
        {
            generatePositions();
            return positions;
        }
        else
        {
            return positions;
        }
    }

    private static void generatePositions()
    {
        positions.Clear();
        equivalencyFactory ef = new equivalencyFactory();
        ef.manufacturePositionList();
    }

    /********************************************************************************
     * Equivalency Activity Objects *************************************************
     *******************************************************************************/
    public static void addActivity(activity a)
    {
        activities.Add(a);
    }

    public static List<activity> getActivities()
    {
        if (activities.Count() < 1)
        {
            generateActivities();
            return activities;
        }
        else
        {
            return activities;
        }
    }

    private static void generateActivities()
    {
        activities.Clear();
        equivalencyFactory ef = new equivalencyFactory();
        ef.manufactureActivityList();
    }

    public static List<activity> getEbcActivities(int _positionID)
    {
        equivalencyShow es = new equivalencyShow();
        return es.getEbcActivities(_positionID);
    }


    /********************************************************************************
     * Equivalency Detail Objects *************************************************
     *******************************************************************************/
    public static void addDetail(detail d)
    {
        details.Add(d);
    }

    public static List<detail> getDetails()
    {
        if (details.Count() < 1)
        {
            generateDetails();
            return details;
        }
        else
        {
            return details;
        }
    }

    private static void generateDetails()
    {
        details.Clear();
        equivalencyFactory ef = new equivalencyFactory();
        ef.manufactureDetailList();
    }

    public static List<detail> getEbcDetails(int _activityID)
    {
        equivalencyShow es = new equivalencyShow();
        return es.getEbcDetails(_activityID);
    }

    public static detail getEbcSingleDetail(int _detailID)
    {
        equivalencyShow es = new equivalencyShow();
        return es.getEbcSingleDetail(_detailID);
    }

    /********************************************************************************
     * Equivalency *************************************************
     *******************************************************************************/
    public static equivalency manufactureEquivalency(int _empID, string _name, int _gpID, decimal _every, int _unitID, decimal _credit, DateTime? _sdate, DateTime? _edate, string _notes, string _modBy, DateTime _modOn, string _history, bool _active, int _typeID, string _typeName, string _unitName, int _positionID, int _activityID, int _detailID)
    {
        equivalencyFactory ef = new equivalencyFactory();
        return ef.manufactureEquivalency(_empID, _name, _gpID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
    }

    public static equivalency updateEquivalency(int _equivID, int _empID, string _name, string _extID, decimal _every, int _unitID, decimal _credit, DateTime? _sdate, DateTime? _edate, string _notes, string _modBy, DateTime _modOn, string _history, bool _active, int _typeID, string _typeName, string _unitName, int _positionID, int _activityID, int _detailID)
    {
        equivalencyFactory ef = new equivalencyFactory();
        return ef.updateEquivalency(_equivID, _empID, _name, _extID, _every, _unitID, _credit, _sdate, _edate, _notes, _modBy, _modOn, _history, _active, _typeID, _typeName, _unitName, _positionID, _activityID, _detailID);
    }

    public static List<equivalency> manufactureEquivalencyList(int _employerID)
    {
         equivalencyFactory ef = new equivalencyFactory();
         return ef.manufactureEquivalencyList(_employerID);
    }

    public static equivalency getSingleEquivalency(int _gpID, List<equivalency> _tempList)
    {
        equivalencyShow es = new equivalencyShow();
        return es.getSingleEquivalency(_gpID, _tempList);
    }

    /// <summary>
    /// This will DELETE a specific Eqvuivalency from the database. 
    /// </summary>
    /// <param name="_equivID"></param>
    /// <returns></returns>
    public static bool deleteEquivalency(int _equivID)
    {
        equivalencyFactory ef = new equivalencyFactory();
        return ef.deleteEquivalency(_equivID);
    }
}