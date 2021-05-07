using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for measurementController
/// </summary>
public static class measurementController
{
    private static List<measurementType> measurementTypes = new List<measurementType>();
    private static List<measurementInitial> initialMeasurements = new List<measurementInitial>();

    public static List<measurementType> getMeasurementTypes()
    {
        if (measurementTypes.Count > 0)
        {
            return measurementTypes;
        }
        else
        {
            measurementFactory mf = new measurementFactory();
            mf.manufactureMeasurementTypeList();
            return measurementTypes;
        }
    }

    public static List<measurementInitial> getInitialMeasurements()
    {
        if (initialMeasurements.Count > 0)
        {
            return initialMeasurements;
        }
        else
        {
            measurementFactory mf = new measurementFactory();
            mf.manufactureInitialMeasurementList();
            return initialMeasurements;
        }
    }

    public static void addInitialMeasurement(measurementInitial mi)
    {
        initialMeasurements.Add(mi);
    }

    public static void addMeasurementType(measurementType mt)
    {
        measurementTypes.Add(mt);
    }

    public static int manufactureNewMeasurement(int _employerID, int _planID, int _employeeTypeID, int _measurementTypeID, DateTime _meas_start, DateTime _meas_end, DateTime _admin_start, DateTime _admin_end, DateTime _open_start, DateTime _open_end, DateTime _stab_start, DateTime _stab_end, string _notes, string _modBy, DateTime _modOn, string _history, DateTime? _swStart, DateTime? _swEnd, DateTime? _swStart2, DateTime? _swEnd2)
    {
        measurementFactory mf = new measurementFactory();
        return mf.manufactureNewMeasurement(_employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd,  _swStart2, _swEnd2);
    }

    public static List<Measurement> manufactureMeasurementList(int _employerID, int _planID, int _employeeTypeID, int _measTypeID)
    {
        measurementFactory mf = new measurementFactory();
        return mf.manufactureMeasurementList(_employerID, _planID, _employeeTypeID, _measTypeID);
    }

    public static Measurement getPlanYearMeasurement(int _employerID, int _planyearID, int _employeeTypeID)
    {
        measurementFactory mf = new measurementFactory();
        return mf.getMeasurement(_employerID, _planyearID, _employeeTypeID);
    }


    public static bool updateMeasurement(int _measID, DateTime _meas_start, DateTime _meas_end, DateTime _admin_start, DateTime _admin_end, DateTime _open_start, DateTime _open_end, DateTime _stab_start, DateTime _stab_end, string _notes, string _modBy, DateTime _modOn, string _history, DateTime? _swStart, DateTime? _swEnd, DateTime? _swStart2, DateTime? _swEnd2)
    {
        measurementFactory mf = new measurementFactory();
        return mf.updateMeasurement(_measID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);
    }

    public static bool updateInitialMeasurement(int _employerID, int _measID)
    {
        measurementFactory mf = new measurementFactory();
        return mf.updateInitialMeasurement(_employerID, _measID);
    }

    public static List<Measurement> manufactureMeasurementList(int _employerID)
    {
        measurementFactory mf = new measurementFactory();
        return mf.manufactureMeasurementList(_employerID);
    }

    public static List<Measurement> manufactureMeasurementList()
    {
        measurementFactory mf = new measurementFactory();
        return mf.manufactureMeasurementList();
    }

    public static int getInitialMeasurementLength(int _id)
    {
        measurementShow ms = new measurementShow();
        return ms.getInitialMeasurementLength(_id);
    }
}