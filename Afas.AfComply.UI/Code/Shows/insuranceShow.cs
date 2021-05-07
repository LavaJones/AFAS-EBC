using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for insuranceShow
/// </summary>
public class insuranceShow
{
    /// <summary>
    /// This will returna list of Insurance Objects
    /// </summary>
    /// <param name="_employerID">Employer ID</param>
    /// <param name="_alertsOnly">
    ///     True = Return only Insurance Objects that are missing Insurance Type ID
    ///     False = Return all Insurance Objects that are currently Active.
    /// </param>
    /// <returns>List of Insurance Objects</returns>
    public List<insurance> getAllActiveInsurancePlans(int _employerID, bool _alertsOnly)
    {
        List<insurance> insList = new List<insurance>();
        List<PlanYear> pyList = PlanYear_Controller.getEmployerPlanYear(_employerID);

        foreach (PlanYear py in pyList)
        {
            List<insurance> tempList = insuranceController.manufactureInsuranceList(py.PLAN_YEAR_ID);
            foreach (insurance i in tempList)
            {
                if (_alertsOnly == true)
                {
                    if (i.INSURANCE_TYPE_ID < 1)
                    {
                        insList.Add(i);
                    }
                }
                else
                {
                    insList.Add(i);
                }
            }
        }

        return insList;
    }

    /// <summary>
    /// Get a list of all active insurance plans based on entity status. 
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <returns></returns>
    public List<insurance> getAllActiveInsurancePlansByPlanYear(int _planYearID)
    {
        List<insurance> insList = new List<insurance>();
        List<insurance> tempList = insuranceController.manufactureInsuranceList(_planYearID);

        foreach (insurance i in tempList)
        {
            if (i.INSURANCE_ENTITY_STATUS == 1)
            {
                insList.Add(i);
            }
        }

        return insList;
    }


    public insurance getSingleInsurancePlan(int _insuranceID, List<insurance> _tempList)
    {
        insurance currIns = null;

        foreach (insurance ins in _tempList)
        {
            if (ins.INSURANCE_ID == _insuranceID)
            {
                currIns = ins;
                break;
            }
        }

        return currIns;
    }

    

    public insuranceContribution getSingleInsuranceContribution(int _insContributionID, List<insuranceContribution> _tempList)
    {
        insuranceContribution currInsCont = null;

        foreach (insuranceContribution insCont in _tempList)
        {
            if (insCont.INS_CONT_ID == _insContributionID)
            {
                currInsCont = insCont;
                break;
            }
        }

        return currInsCont;
    }



    public void crossReferanceInsuranceCarrierImportData(int _employerID, DateTime _modOn, string _modBy)
    {
    }

    /// <summary>
    /// Find a single Insurance Coverage object from a list of them. 
    /// </summary>
    /// <param name="rowID"></param>
    /// <param name="templist"></param>
    /// <returns></returns>
    public insurance_coverage getSingleInsuranceCoverage(int rowID, List<insurance_coverage> templist)
    {
        insurance_coverage currInsuranceCoverage = null;

        foreach (insurance_coverage ic in templist)
        {
            if (ic.ROW_ID == rowID)
            {
                currInsuranceCoverage = ic;
                break;
            }
        }

        return currInsuranceCoverage;
    }

    
}