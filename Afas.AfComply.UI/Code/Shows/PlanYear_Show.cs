using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PlanYear_Show
/// </summary>
public class PlanYear_Show
{
    public PlanYear findPlanYear(int _planID, int _employerID)
    {
        List<PlanYear> tempList = PlanYear_Controller.getEmployerPlanYear(_employerID);
        PlanYear plan = null;

        foreach (PlanYear py in tempList)
        {
            if (py.PLAN_YEAR_ID == _planID)
            {
                plan = py;
                break;
            }
        }

        return plan;
    }

    public List<PlanYear> filterFuturePlanYears(List<PlanYear> allPY)
    {
        List<PlanYear> tempList = new List<PlanYear>();

        foreach (PlanYear py in allPY)
        {
            if (py.PLAN_YEAR_START < System.DateTime.Now)
            {
                tempList.Add(py);
            }
        }

        return tempList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_currentPlanYears"></param>
    /// <param name="_hireDate"></param>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public List<PlanYear> findNewHirePlanYear(List<PlanYear> _currentPlanYears, DateTime _hireDate, int _employerID)
    {
        List<PlanYear> tempList = new List<PlanYear>();
        List<Measurement> tempMeasurementList = new List<Measurement>();


        tempMeasurementList = measurementController.manufactureMeasurementList(_employerID);

        foreach (PlanYear py in _currentPlanYears)
        {
            foreach (Measurement meas in tempMeasurementList)
            {
                if (py.PLAN_YEAR_ID == meas.MEASUREMENT_PLAN_ID)
                {
                    if (_hireDate >= meas.MEASUREMENT_START && _hireDate <= meas.MEASUREMENT_END)
                    {
                        tempList.Add(py);
                        break;              
                    }
                }
            }
        }

        return tempList;
    }

    /// <summary>
    /// Check to see if the Employee's Hire Date is Greater than all Measurement Period End Dates. 
    /// </summary>
    /// <param name="_currentPlanYears"></param>
    /// <param name="_hireDate"></param>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public bool validateNewHire(List<PlanYear> _currentPlanYears, DateTime _hireDate, int _employerID)
    {
        bool validNewHire = false;
        List<Measurement> tempMeasurementList = new List<Measurement>();

        tempMeasurementList = measurementController.manufactureMeasurementList(_employerID);

        foreach (PlanYear py in _currentPlanYears)
        {
            foreach (Measurement meas in tempMeasurementList)
            {
                if (py.PLAN_YEAR_ID == meas.MEASUREMENT_PLAN_ID)
                {
                    if (_hireDate <= meas.MEASUREMENT_END)
                    {
                        validNewHire = true;
                        break;
                    }
                }
            }

            if (validNewHire == true)
            {
                break;
            }
        }

        return validNewHire;
    }


}