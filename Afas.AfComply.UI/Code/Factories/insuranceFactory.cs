using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using log4net;
using Afas.AfComply.Domain;
using Afas.Domain;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain.POCO;

/// <summary>
/// Summary description for insuranceFactory
/// </summary>
public class insuranceFactory
{
    private ILog Log = LogManager.GetLogger(typeof(insuranceFactory));
    

    private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

	public insuranceFactory()
	{
	
	}

    public List<insurance> manufactureInsuranceList(int _planyearID)
    {
        List<insurance> tempList = new List<insurance>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_plan_year_insurance_plan", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planyearID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object planyearID = null;
                object desc = null;
                object monthlyCost = null;
                object minValue = null;
                object offSpouse = null;
                object offDependent = null;
                object modOn = null;
                object modBy = null;
                object history = null;
                object insuranceTypeID = null;
                object resourceID = null;
                object spouseConditional = null;
                object mec = null;
                object createdBy = null;
                object createdOn = null;
                object entityStatus = null;
                object fullyAndSelf = null;

                id = rdr[0] as object ?? default(object);
                planyearID = rdr[1] as object ?? default(object);
                desc = rdr[2] as object ?? default(object);
                monthlyCost = rdr[3] as object ?? default(object);
                minValue = rdr[4] as object ?? default(object);
                offSpouse = rdr[5] as object ?? default(object);
                offDependent = rdr[6] as object ?? default(object);
                modOn = rdr[7] as object ?? default(object);
                modBy = rdr[8] as object ?? default(object);
                history = rdr[9] as object ?? default(object);
                insuranceTypeID = rdr[10] as object ?? default(object);
                resourceID = rdr[11] as object ?? default(object);
                spouseConditional = rdr[12] as object ?? default(object);
                mec = rdr[13] as object ?? default(object);
                createdBy = rdr[14] as object ?? default(object);
                createdOn = rdr[15] as object ?? default(object);
                entityStatus = rdr[16] as object ?? default(object);
                fullyAndSelf = rdr[17] as object ?? default(object);

                int _id = id.checkIntNull();
                int _pyID = (int)planyearID.checkIntNull();
                string _desc = (string)desc.checkStringNull();
                decimal _monthlyCost = monthlyCost.checkDecimalNull();
                bool _minValue = minValue.checkBoolNull();
                bool _offSpouse = offSpouse.checkBoolNull();
                bool _offDependent = offDependent.checkBoolNull();
                DateTime? _modOn = modOn.checkDateNull();
                string _modBy = (string)modBy.checkStringNull();
                string _history = (string)history.checkStringNull();
                int _insuranceTypeID = insuranceTypeID.checkIntNull();
                string _resourceID = resourceID.checkStringNull();
                bool SpouseConditional = spouseConditional.checkBoolNull();
                bool _mec = mec.checkBoolNull();
                DateTime? _createdOn = createdOn.checkDateNull();
                string _createdBy = createdBy.checkStringNull();
                int _entityStatusID = entityStatus.checkIntNull();
                bool _fullyAndSelf = fullyAndSelf.checkBoolNull();

                insurance newI = new insurance(_id, _pyID, _desc, _monthlyCost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, _createdBy, _createdOn, _entityStatusID, _fullyAndSelf);
                tempList.Add(newI);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.INSURANCE_NAME).ToList();

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    public List<insuranceContribution> manufactureInsuranceContributionList(int _insuranceID)
    {
        List<insuranceContribution> tempList = new List<insuranceContribution>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_insurance_contributions", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object insuranceID = null;
                object contributionID = null;
                object classID = null;
                object amount = null;
                object modOn = null;
                object modBy = null;
                object history = null;
                object name = null;

                id = rdr[0] as object ?? default(object);
                insuranceID = rdr[1] as object ?? default(object);
                contributionID = rdr[2] as object ?? default(object);
                classID = rdr[3] as object ?? default(object);
                amount = rdr[4] as object ?? default(object); 
                modBy = rdr[5] as object ?? default(object);
                modOn = rdr[6] as object ?? default(object);
                history = rdr[7] as object ?? default(object);
                name = rdr[8] as object ?? default(object);

                int _id = id.checkIntNull();
                int _insuranceID2 = (int)insuranceID.checkIntNull();
                string _contributionID = contributionID.checkStringNull();
                int _classID = classID.checkIntNull();
                double _amount = amount.checkDoubleNull();
                DateTime? _modOn = modOn.checkDateNull();
                string _modBy = (string)modBy.checkStringNull();
                string _history = (string)history.checkStringNull();
                string _name = (string)name.checkStringNull();

                insuranceContribution newIC = new insuranceContribution(_id, _insuranceID, _contributionID, _classID, _amount, _modBy, _modOn, _history, _name);
                tempList.Add(newIC);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.INS_CONT_CLASSNAME).ToList();

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insuranceContribution>();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return tempList;
    }

    public List<contribution> manufactureContributionList()
    {
        List<contribution> tempList = new List<contribution>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_contribution_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                string _id = id.checkStringNull();
                string _name = name.checkStringNull();

                contribution newC = new contribution(_id, _name);
                tempList.Add(newC);
            }

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    public List<carrier> manufactureInsuranceCarriers()
    {
        List<carrier> tempList = new List<carrier>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_all_insurance_carriers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object name = null;
                object approved = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);
                approved = rdr[2] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();
                bool _approved = approved.checkBoolNull();

                carrier newCarrier = new carrier(_id, _name, _approved);
                tempList.Add(newCarrier);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.CARRIER_NAME).ToList();

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception); 
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }

    public insurance manufactureInsurancePlan(int _planyearID, string _name, decimal _cost, bool _minValue, bool _offSpouse, bool SpouseConditional, bool _offDependent, string _modBy, DateTime? _modOn, string _history, int _insuranceTypeID, bool _mec, bool _fullyPlusSelfInsured)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_insurance_plan", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planyearID.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();
            cmd.Parameters.AddWithValue("@monthlycost", SqlDbType.Money).Value = _cost.checkForDBNull();
            cmd.Parameters.AddWithValue("@minValue", SqlDbType.Bit).Value = _minValue.checkForDBNull();
            cmd.Parameters.AddWithValue("@offSpouse", SqlDbType.Bit).Value = _offSpouse.checkForDBNull();
            cmd.Parameters.AddWithValue("@SpouseConditional", SqlDbType.Bit).Value = SpouseConditional.checkForDBNull();            
            cmd.Parameters.AddWithValue("@offDependent", SqlDbType.Bit).Value = _offDependent.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@insuranceTypeID", SqlDbType.Int).Value = _insuranceTypeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@mec", SqlDbType.Bit).Value = _mec.checkForDBNull();
            cmd.Parameters.AddWithValue("@fullyPlusSelf", SqlDbType.Bit).Value = _fullyPlusSelfInsured.checkBoolDBNull();

            cmd.Parameters.Add("@insuranceid", SqlDbType.Int);
            cmd.Parameters["@insuranceid"].Direction = ParameterDirection.Output;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                int _id = (int)cmd.Parameters["@insuranceid"].Value;

                insurance newIns = new insurance(_id, _planyearID, _name, _cost, _minValue, _offSpouse, SpouseConditional, _offDependent, _modBy, _modOn, _history, _insuranceTypeID, _mec, _modBy, _modOn, 1, _fullyPlusSelfInsured);

                return newIns;
            }
            else
            {
                return null;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public insuranceContribution manufactureInsuranceContribution(int _insuranceID, string _contributionID, int _classID, double _amount, string _modBy, DateTime? _modOn, string _history)
    {
        insuranceContribution newContribution = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_insurance_contribution", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkForDBNull();
            cmd.Parameters.AddWithValue("@contributionID", SqlDbType.VarChar).Value = _contributionID.checkForDBNull();
            cmd.Parameters.AddWithValue("@classID", SqlDbType.Int).Value = _classID.checkForDBNull();
            cmd.Parameters.AddWithValue("@amount", SqlDbType.Money).Value = _amount.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();

            cmd.Parameters.Add("@newID", SqlDbType.Int);
            cmd.Parameters["@newID"].Direction = ParameterDirection.Output;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                int _id = (int)cmd.Parameters["@newID"].Value;

                newContribution = new insuranceContribution(_id, _insuranceID, _contributionID, _classID, _amount, _modBy, _modOn, _history, null);
            }
            else
            {
                newContribution = null;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            newContribution = null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return newContribution;
    }

    public bool updateInsurancePlan(int _insuranceID, int _planyearID, string _name, decimal _cost, bool _minValue, bool _offSpouse, bool SpouseConditional, bool _offDependent, string _modBy, DateTime? _modOn, string _history, int _insuranceTypeID, bool _mec, bool _fullyPlusSelfInsured)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_insurance_plan", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkForDBNull();
            cmd.Parameters.AddWithValue("@planyearID", SqlDbType.Int).Value = _planyearID.checkForDBNull();
            cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = _name.checkForDBNull();
            cmd.Parameters.AddWithValue("@cost", SqlDbType.Money).Value = _cost.checkForDBNull();
            cmd.Parameters.AddWithValue("@minValue", SqlDbType.Bit).Value = _minValue.checkForDBNull();
            cmd.Parameters.AddWithValue("@offSpouse", SqlDbType.Bit).Value = _offSpouse.checkForDBNull();
            cmd.Parameters.AddWithValue("@SpouseConditional", SqlDbType.Bit).Value = SpouseConditional.checkForDBNull();            
            cmd.Parameters.AddWithValue("@offDependent", SqlDbType.Bit).Value = _offDependent.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@insuranceTypeID", SqlDbType.Int).Value = _insuranceTypeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@mec", SqlDbType.Bit).Value = _mec.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@fullyPlusSelf", SqlDbType.Bit).Value = _fullyPlusSelfInsured.checkBoolDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool InsertNewInsuranceCoverage(int _employerID, int _employeeID, int _planYearID, double _monthlyAvg, string _modBy, DateTime? _modOn, string _history)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_insurance_offer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkForDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkForDBNull();
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID.checkForDBNull();
            cmd.Parameters.AddWithValue("@monthlyAvg", SqlDbType.Decimal).Value = _monthlyAvg.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;     
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return false;
    }

    public bool updateInsuranceOffer(int _rowID, int? _insuranceID, int? _contributionID, double? _avgHours, bool? _offered, DateTime? _offeredDate, bool? _accepted, DateTime? _acceptedDate, DateTime _modOn, string _modBy, string _notes, string _history, DateTime? _effDate, double _hraFlex)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_insurance_offer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@contributionID", SqlDbType.Int).Value = _contributionID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@avgHours", SqlDbType.Decimal).Value = _avgHours.checkDoubleDBNull();
            cmd.Parameters.AddWithValue("@offered", SqlDbType.Bit).Value = _offered.checkForDBNull();
            cmd.Parameters.AddWithValue("@offeredOn", SqlDbType.DateTime).Value = _offeredDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@accepted", SqlDbType.Bit).Value = _accepted.checkForDBNull();
            cmd.Parameters.AddWithValue("@acceptedOn", SqlDbType.DateTime).Value = _acceptedDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@effDate", SqlDbType.DateTime).Value = _effDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@hraFlex", SqlDbType.Decimal).Value = _hraFlex.checkDoubleNull();
            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    public Boolean TransferInsuranceChangeEvent(
            int _rowID, 
            int? _insuranceID, 
            int? _contributionID, 
            double? _avgHours, 
            bool? _offered, 
            DateTime? _offeredDate, 
            Boolean? _accepted, 
            DateTime? _acceptedDate, 
            DateTime _modOn, 
            String _modBy, 
            String _notes, 
            String _history, 
            DateTime? _effDate, 
            double _hraFlex
        )
    {

        Boolean validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("TRANSFER_insurance_change_event", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@contributionID", SqlDbType.Int).Value = _contributionID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@avgHours", SqlDbType.Decimal).Value = _avgHours.checkDoubleDBNull();
            cmd.Parameters.AddWithValue("@offered", SqlDbType.Bit).Value = _offered.checkForDBNull();
            cmd.Parameters.AddWithValue("@offeredOn", SqlDbType.DateTime).Value = _offeredDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@accepted", SqlDbType.Bit).Value = _accepted.checkForDBNull();
            cmd.Parameters.AddWithValue("@acceptedOn", SqlDbType.DateTime).Value = _acceptedDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@notes", SqlDbType.VarChar).Value = _notes.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@effDate", SqlDbType.DateTime).Value = _effDate.checkDateDBNull();
            cmd.Parameters.AddWithValue("@hraFlex", SqlDbType.Decimal).Value = _hraFlex.checkDoubleDBNull();
            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 2)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }

        }
        catch (Exception exception)
        {
            
            Log.Warn("Suppressing errors.", exception);
            
            validTransaction = false;
        
        }
        finally
        {
        
            if (conn != null)
            {
                conn.Close();
            }
        
        }

        return validTransaction;
    
    }

    public string generateInsuranceAlertFile(PlanYear plan, employer employer)
    {
        
        List<Employee> employees = EmployeeController.manufactureEmployeeList(employer.EMPLOYER_ID);
        List<PlanYear> planYears = PlanYear_Controller.getEmployerPlanYear(employer.EMPLOYER_ID);

        employees = (from Employee emp in employees where emp.EMPLOYEE_TERM_DATE >= plan.PLAN_YEAR_START || emp.EMPLOYEE_TERM_DATE == null select emp).ToList();
        employees = (from Employee emp in employees where emp.EMPLOYEE_HIRE_DATE <= plan.PLAN_YEAR_END select emp).ToList();
        planYears = (from PlanYear year in planYears where year.PlanYearGroupId == plan.PlanYearGroupId select year).ToList();
        employees = (from Employee emp in employees where planYears.Any(junk => junk.PLAN_YEAR_ID == emp.EMPLOYEE_PLAN_YEAR_ID_MEAS) select emp).ToList();

        return generateInsuranceAlertFile(employees, plan, employer); 
    }

    public string generateInsuranceAlertFile(List<alert_insurance> alerts, PlanYear plan, employer employer)
    {
        List<Employee> employees = EmployeeController.manufactureEmployeeList(employer.EMPLOYER_ID);

        employees = (from Employee emp in employees where alerts.Any(junk => junk.IALERT_EMPLOYEEID == emp.EMPLOYEE_ID) select emp).ToList();

        return generateInsuranceAlertFile(employees, plan, employer);
    }

    public string generateInsuranceAlertFile(List<Employee> employees, PlanYear plan, employer employer)
    {
        List<classification> classes = classificationController.ManufactureEmployerClassificationList(employer.EMPLOYER_ID, false);
        List<Measurement> measurements = measurementController.manufactureMeasurementList(employer.EMPLOYER_ID);

        DataTable export = new DataTable();

        export.Columns.Add("FEIN", typeof(string));
        export.Columns.Add("STABILITY PERIOD", typeof(string));
        export.Columns.Add("SSN", typeof(string));

        export.Columns.Add("FIRST NAME", typeof(string));
        export.Columns.Add("MIDDLE NAME", typeof(string));
        export.Columns.Add("LAST NAME", typeof(string));
        export.Columns.Add("SUFFIX", typeof(string));

        export.Columns.Add("OFFERED", typeof(string));
        export.Columns.Add("ACCEPTED", typeof(string));
        export.Columns.Add("EFFECTIVE DATE", typeof(string));
        export.Columns.Add("MEDICAL PLAN", typeof(string));

        export.Columns.Add("DATA PREFILLED", typeof(string));
        export.Columns.Add("EMPLOYEE CLASS", typeof(string));
        export.Columns.Add("AVG HOURS", typeof(string));
        export.Columns.Add("ACA STATUS", typeof(string));
        export.Columns.Add("HIRE DATE", typeof(string));
        export.Columns.Add("TERM DATE", typeof(string));
        export.Columns.Add("EMPLOYEE ID", typeof(string));

        foreach (Employee employee in employees)
        {
            if (null != employee.EMPLOYEE_TERM_DATE && employee.EMPLOYEE_TERM_DATE < plan.PLAN_YEAR_START)
            {
                continue;         
            }

            if (null == employee.EMPLOYEE_HIRE_DATE || employee.EMPLOYEE_HIRE_DATE > plan.PLAN_YEAR_END)
            {
                continue;         
            }

            List<offered_Insurance> allOffers = insuranceController.findAllInsurancesofferedToEmployee(employee.EMPLOYEE_ID);
            List<offered_Insurance> currentOffers = (
                    from 
                        offered_Insurance offer in allOffers
                    where 
                        offer.PlanYearDescryption == plan.PLAN_YEAR_DESCRIPTION 
                        && offer.effectiveDate != null
                    orderby 
                        offer.effectiveDate ascending
                    select offer).ToList();

            if (currentOffers.Count <= 0)
            {

                offered_Insurance lastOffer = (
                    from 
                        offered_Insurance off in allOffers
                    where 
                        off.effectiveDate != null 
                        && off.offeredbool == true
                    orderby 
                        off.effectiveDate descending
                    select off).FirstOrDefault();

                currentOffers.Add(lastOffer);

            }

            foreach (offered_Insurance oldOffer in currentOffers)
            {
            DataRow row = export.NewRow();

                row["FEIN"] = employer.EMPLOYER_EIN;

                row["STABILITY PERIOD"] = plan.PLAN_YEAR_DESCRIPTION;

            row["SSN"] = employee.Employee_SSN_Visible;

                row["FIRST NAME"] = employee.EMPLOYEE_FIRST_NAME;
                row["MIDDLE NAME"] = employee.EMPLOYEE_MIDDLE_NAME;
                row["LAST NAME"] = employee.EMPLOYEE_LAST_NAME;
                row["SUFFIX"] = "";

                classification classy = (from classification cla in classes where cla.CLASS_ID == employee.EMPLOYEE_CLASS_ID select cla).Single();
                row["EMPLOYEE CLASS"] = classy.CLASS_DESC;

                Measurement measurement = (from Measurement meas in measurements where meas.MEASUREMENT_EMPLOYEE_TYPE_ID == employee.EMPLOYEE_TYPE_ID && meas.MEASUREMENT_PLAN_ID == plan.PLAN_YEAR_ID select meas).Single();
                
                AverageHours avg = AverageHoursFactory.GetAverageHoursForEmployeeMeasurement(employee.EMPLOYEE_ID, measurement.MEASUREMENT_ID, false);
            if (null != avg)
                {
                    row["AVG HOURS"] = string.Format("{0:N2}", Math.Round(avg.MonthlyAverageHours, 2));
                }

            ACAStatusEnum status = (ACAStatusEnum)employee.EMPLOYEE_ACT_STATUS_ID;

            row["ACA STATUS"] = status.ToString();

            if (null != employee.EMPLOYEE_HIRE_DATE)
                {
                row["HIRE DATE"] = ((DateTime)employee.EMPLOYEE_HIRE_DATE).ToShortDateString();
            }
            if (null != employee.EMPLOYEE_TERM_DATE)
            {
                row["TERM DATE"] = ((DateTime)employee.EMPLOYEE_TERM_DATE).ToShortDateString();
            }
                row["EMPLOYEE ID"] = employee.EMPLOYEE_EXT_ID;

                if (((DateTime)plan.PLAN_YEAR_START) > employee.EMPLOYEE_HIRE_DATE)
                {
                    row["EFFECTIVE DATE"] = ((DateTime)plan.PLAN_YEAR_START).ToShortDateString();
                }

            if (null != oldOffer)
                    {
                    if (oldOffer.offeredbool)
                {
                        row["OFFERED"] = oldOffer.offeredbool ? "Yes":"No";
                        row["ACCEPTED"] = oldOffer.accepted ? "Yes" : "No";

                        if (oldOffer.PlanYearDescryption == plan.PLAN_YEAR_DESCRIPTION)
                        {

                    row["EFFECTIVE DATE"] = oldOffer.effectiveDatePart;

                        }
                        else
                    {

                            row["DATA PREFILLED"] = "Prefilled From " + oldOffer.PlanYearDescryption;

                    }
                }

                try
                {
                    if (false == oldOffer.InsuranceDescription.IsNullOrEmpty())
                    {
                        List<insurance> insurances = insuranceController.manufactureInsuranceList(plan.PLAN_YEAR_ID);
                        insurance insurance = (from insurance ins in insurances where ins.INSURANCE_NAME == oldOffer.InsuranceDescription select ins).SingleOrDefault();

                        if (null != insurance)
                            {
                            List<insuranceContribution> contributions = insuranceController.manufactureInsuranceContributionList(insurance.INSURANCE_ID);
                    
                                row["MEDICAL PLAN"] = oldOffer.InsuranceDescription;

                            }
                        }
                    }
                catch (InvalidOperationException ex)
                {
                        Log.Warn("Logging Exception then rethrowing, Found Multiple Insurances with the same name: [" + oldOffer.InsuranceDescription + "] EmployerId: [" + employer.EMPLOYER_ID + "]", ex);

                    throw;
                    }
            }

            export.Rows.Add(row);
        }
        }

        return export.GetAsCsv();

    }

    public string generateInsuranceAlertFileHRAFlex(List<alert_insurance> _tempList, employer _employer)
    {
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        int count = _tempList.Count();
        bool success = false;
        string fullFilePath = null;
        List<insurance> insuranceList = null;
        int _employerID2 = _employer.EMPLOYER_ID;
        int _planYearID = _tempList[0].IALERT_PLANYEARID;
        fileName = Branding.ProductName.ToUpper() + "_EXPORT_" + _employer.EMPLOYER_IMPORT_IO + "_" + count + "_" + currDate + ".csv";
        fullFilePath = HttpContext.Current.Server.MapPath("..\\ftps\\export\\") + fileName;
        string message1 = "***** Please follow these instructions *****";
        string message2 = "Section 1 is all of your medical plans.";
        string message3 = "Section 2 is all of your Insurance Contributions.";
        string exampleOne = "INSTRUCTIONS";
        string exampleTwo = "***** Only change the values in the following columns ***** ";
        string message4 = "     5) Insurance ID: This value will come from Section 1 above.";
        string message5 = "     6) Contribution ID: This value will come from Section 2 above.";
        string message6 = "     1) Offered: This will either be true or false.";
        string message7 = "     2) Offered On: The date that the plan was offered to the employee. Use the following format: mm/dd/yyyy";
        string message8 = "     3) Accepted: This will either be true or false.";
        string message9 = "     4) Accpeted/DeclinedOn: The date that the Employee accepted/declined the offer. User the following format: mm/dd/yyyy";
        string message10 = "    7) Effective Date: The date this insurance would be effective regardless of whether they accepted or not this value needs to be filled in. Use the following format: mm/dd/yyyy";
        string message11 = "ROW ID,EMPLOYEE ID,EMPLOYER ID,PLAN YEAR ID,PAYROLL ID,NAME,CLASS ID,AVG HOURS,OFFERED,OFFERED ON,ACCEPTED,ACCEPTED/DECLINED ON,INSURANCE ID,CONTRIBUTION ID,EFFECTIVE DATE,HRA-FLEX";

        try
        {
            insuranceList = insuranceController.manufactureInsuranceList(_planYearID);
            using (StreamWriter sw = File.CreateText(fullFilePath))
            {
                sw.WriteLine(message1);
                sw.WriteLine(message2);
                sw.WriteLine("Insurance ID,Insurance Name,Cost");

                foreach (insurance ai in insuranceList)
                {
                    sw.WriteLine(ai.INSURANCE_ID + "," + ai.INSURANCE_NAME + "," + ai.INSURANCE_COST);
                    List<insuranceContribution> contributionList = insuranceController.manufactureInsuranceContributionList(ai.INSURANCE_ID);
                }

                sw.WriteLine(message3);
                sw.WriteLine("Insurance ID,Contribution ID,Contribution Name,Contribution,Employee Class Name,Employee Class ID");
                foreach (insurance ai in insuranceList)
                {
                    List<insuranceContribution> contributionList = insuranceController.manufactureInsuranceContributionList(ai.INSURANCE_ID);

                    foreach (insuranceContribution cl in contributionList)
                    {
                        sw.WriteLine(ai.INSURANCE_ID + "," + cl.INS_CONT_ID + "," + cl.INS_DESC + "," + cl.INS_CONT_AMOUNT + "," + cl.INS_CONT_CLASSNAME + "," + cl.INS_CONT_CLASSID);
                    }
                }

                sw.WriteLine(exampleOne);
                sw.WriteLine(exampleTwo);
                sw.WriteLine(message6);
                sw.WriteLine(message7);
                sw.WriteLine(message8);
                sw.WriteLine(message9);
                sw.WriteLine(message4);
                sw.WriteLine(message5);
                sw.WriteLine(message10);
                sw.WriteLine(message11);

                foreach (alert_insurance ai in _tempList)
                {
                    string line = ai.ROW_ID + ",";
                    line += ai.IALERT_EMPLOYEEID + ",";
                    line += ai.IALERT_EMPLOYERID + ",";
                    line += ai.IALERT_PLANYEARID + ",";
                    line += ai.EMPLOYEE_EXT_ID + ",";
                    line += ai.EMPLOYEE_FULL_NAME.Replace(',', ' ') + ",";
                    line += ai.IALERT_CLASS_ID + ",";
                    line += ai.EMPLOYEE_AVG_HOURS + ",";

                    line += ai.IALERT_OFFERED.ToString() + ",";
                    line += ai.IALERT_OFFERED_ON.ToString() + ",";
                    line += ai.IALERT_ACCEPTED.ToString() + ",";
                    line += ai.IALERT_ACCEPTEDDATE.ToString() + ",";
                    line += ai.IALERT_INSURANCE_ID.ToString() + ",";
                    line += ai.IALERT_CONTRIBUTION_ID.ToString() + ",";
                    line += ai.IALERT_EFFECTIVE_DATE.ToString() + ",";
                    line += " ";
                    sw.WriteLine(line);
                }

            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            fileName = null;
        }

        return fileName;
    }

    public bool deleteInsurancePlan(int _insuranceID)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool deleteInsuranceContribution(int _contributionID)
    {
        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_contribution", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@contID", SqlDbType.Int).Value = _contributionID.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }

    public bool updateInsuranceContribution(int _contID, int _insuranceID, string _contributionID, int _classID, double _amount, string _modBy, DateTime? _modOn, string _history)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_insurance_contribution", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@contID", SqlDbType.Int).Value = _contID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@insuranceID", SqlDbType.Int).Value = _insuranceID.checkForDBNull();
            cmd.Parameters.AddWithValue("@contributionID", SqlDbType.VarChar).Value = _contributionID.checkForDBNull();
            cmd.Parameters.AddWithValue("@classificationID", SqlDbType.Int).Value = _classID.checkForDBNull();
            cmd.Parameters.AddWithValue("@cost", SqlDbType.Money).Value = _amount.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

     /// <summary>
    /// This will generate a list of all Insurance Alerts. 
    /// </summary>
    /// <returns></returns>
    public List<alert_insurance> manufactureEmployerInsuranceAlertList(int _employerID)
    {
        List<alert_insurance> tempList = new List<alert_insurance>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_insurance_alerts";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();
            tempList = manufactureInsuranceAlertList(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<alert_insurance>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Generate a single Insurance Alert Offer. 
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public alert_insurance findSingleInsuranceOffer(int _planYearID, int _employeeID)
    {
        List<alert_insurance> tempList = new List<alert_insurance>();
        alert_insurance tempIO = null;

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_insurance_offer";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;

            rdr = cmd.ExecuteReader();

            tempList = manufactureInsuranceAlertList(rdr);
            
            tempIO = tempList.FirstOrDefault();                   
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempIO = null;
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempIO;
    }

    /// <summary>
    /// This will generate a List of All insurance offers on a per employee basis.  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<alert_insurance> findEmployeeInsuranceOffers(int _employeeID)
    {
        List<alert_insurance> tempList = new List<alert_insurance>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_insurance_offer_all";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            tempList = manufactureInsuranceAlertList(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }


    /// <summary>
    /// This will generate a List of All insurances offered to employee from both "employee_insurance_offer" and "employee_insurance_offer_archive" tables(current and past).  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<offered_Insurance> findAllInsurancesofferedToEmployee(int _employeeID)
    {
        List<offered_Insurance> tempList = new List<offered_Insurance>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_EmployeeInsuranceOfferAndEmployeeInsuranceOfferArchive_ForEmployee";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(rdr);
            tempList = dataTable.ToCollection<offered_Insurance>();
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }


    /// <summary>
    /// This will generate a List of All insurance offers on a per employee basis.  
    /// </summary>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<alert_insurance> findEmployeeInsuranceOfferChangeEvents(int _employeeID)
    {
        List<alert_insurance> tempList = new List<alert_insurance>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_insurance_offer_change_events_all";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            tempList = manufactureInsuranceAlertList(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// This will generate a List of All insurance offers and change events for an employer's planyear.  
    /// </summary>
    /// <param name="_employeeID">ID of the employer to use.</param>
    /// <param name="_planYearID">Id of the Plan Year to use.</param>
    /// <returns>A list of all Offers and Change Events</returns>
    public List<alert_insurance> getAllInsuranceForEmployerPlanYear(int _employerID, int _planYearID)
    {
        List<alert_insurance> tempList = new List<alert_insurance>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "[SELECT_employer_planyear_insurance_offer_all]";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@planYearID", SqlDbType.Int).Value = _planYearID;
            
            rdr = cmd.ExecuteReader();

            tempList = manufactureInsuranceAlertList(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }


    /// <summary>
    /// This will generate the list of Alert_Insurance objects from the rdr object. 
    /// </summary>
    /// <returns></returns>
    public List<alert_insurance> manufactureInsuranceAlertList(SqlDataReader rdr)
    {
        List<alert_insurance> aiList = new List<alert_insurance>();


        while (rdr.Read())
        {
            alert_insurance tempIO = null;
            object rowID = null;
            object employeeID = null;
            object planYearID = null;
            object employerID = null;
            object avgHours = null;
            object offeredOn = null;
            object accepted = null;
            object acceptedOn = null;
            object modOn = null;
            object modBy = null;
            object notes = null;
            object history = null;
            object extID = null;
            object fname = null;
            object lname = null;
            object effectiveDate = null;
            object offered = null;
            object contributionID = null;
            object insuranceID = null;
            object classID = null;
            object hrStatusID = null;
            object adminPlanYearID = null;
            object adminAvgHours = null;
            object nhAvgHours = null;
            object flexHra = null;

            rowID = rdr[0] as object ?? default(object);
            employeeID = rdr[1] as object ?? default(object);
            planYearID = rdr[2] as object ?? default(object);
            employerID = rdr[3] as object ?? default(object);
            avgHours = rdr[4] as object ?? default(object);
            offered = rdr[5] as object ?? default(object);
            accepted = rdr[6] as object ?? default(object);
            acceptedOn = rdr[7] as object ?? default(object);
            modOn = rdr[8] as object ?? default(object);
            modBy = rdr[9] as object ?? default(object);
            notes = rdr[10] as object ?? default(object);
            history = rdr[11] as object ?? default(object);
            extID = rdr[12] as object ?? default(object);
            fname = rdr[13] as object ?? default(object);
            lname = rdr[14] as object ?? default(object);
            effectiveDate = rdr[15] as object ?? default(object);
            offeredOn = rdr[16] as object ?? default(object);
            contributionID = rdr[17] as object ?? default(object);
            insuranceID = rdr[18] as object ?? default(object);
            hrStatusID = rdr[19] as object ?? default(object);
            adminPlanYearID = rdr[20] as object ?? default(object);
            adminAvgHours = rdr[21] as object ?? default(object);
            nhAvgHours = rdr[22] as object ?? default(object);
            classID = rdr[23] as object ?? default(object);
            flexHra = rdr[24] as object ?? default(object);

            int _rowID = rowID.checkIntNull();
            int _employeeID2 = employeeID.checkIntNull();
            int _planYearID2 = planYearID.checkIntNull();
            int _employerID2 = employerID.checkIntNull();
            double _avgHours = avgHours.checkDoubleNull();
            bool? _offered = offered.checkBoolNull2();
            DateTime? _offeredOn = offeredOn.checkDateNull();
            bool _accepted = accepted.checkBoolNull();
            DateTime? _acceptedOn = acceptedOn.checkDateNull();
            DateTime _modOn = (DateTime)modOn;
            string _modBy = modBy.checkStringNull();
            string _notes = notes.checkStringNull();
            string _history = history.checkStringNull();
            string _extID = extID.checkStringNull();
            string _fname = fname.checkStringNull();
            string _lname = lname.checkStringNull();
            DateTime? _effectiveDate = effectiveDate.checkDateNull();
            int? _insuranceID = insuranceID.checkIntNull();
            int? _contributionID = contributionID.checkIntNull();
            int? _hrStatusID = hrStatusID.checkIntNull();
            int? _classID = classID.checkIntNull();
            int? _adminPlanYearID = adminPlanYearID.checkIntNull();
            double? _adminAvgHours = adminAvgHours.checkDoubleNull();
            double? _nhAvgHours = nhAvgHours.checkDoubleNull();
            double? _flexHra = flexHra.checkDoubleNull();

            tempIO = new alert_insurance(_rowID, _employeeID2, _planYearID2, _employerID2, _offered, _offeredOn, _accepted, _acceptedOn, _modOn, _modBy, _notes, _history, _extID, _fname, _lname, _avgHours, _effectiveDate, _contributionID, _insuranceID, _classID, _hrStatusID, _adminPlanYearID, _flexHra);

            aiList.Add(tempIO);
        }

        return aiList;
    }

    /// <summary>
    /// 
    /// </summary>
    public insurance_coverage_template manufactureInsuranceCoverageTemplate(int _carrierID)
    {
       insurance_coverage_template ict = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_insurance_coverage_template", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@carrierID", SqlDbType.Int).Value = _carrierID;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object templateID = null;
                object carrierID = 0;
                object columns = 0;
                object employeeDependentLink = 0;
                object fname = 0;
                object mname = 0;
                object lname = 0;
                object ssn = 0;
                object dob = 0;
                object all12 = 0;
                object jan = 0;
                object feb = 0;
                object mar = 0;
                object apr = 0;
                object may = 0;
                object jun = 0;
                object jul = 0;
                object aug = 0;
                object sep = 0;
                object oct = 0;
                object nov = 0;
                object dec = 0;
                object subscriber = 0;
                object tFormat = null;
                object nFormat = null;
                object all12tFormat = null;
                object subscriberFormat = null;
                object address = null;
                object city = null;
                object state = null;
                object zip = null;

                templateID = rdr[0] as object ?? default(object);
                carrierID = rdr[1] as object ?? default(object);
                columns = rdr[2] as object ?? default(object);
                employeeDependentLink = rdr[3] as object ?? default(object);
                fname = rdr[4] as object ?? default(object);
                mname = rdr[5] as object ?? default(object);
                lname = rdr[6] as object ?? default(object);
                ssn = rdr[7] as object ?? default(object);
                dob = rdr[8] as object ?? default(object);
                all12 = rdr[9] as object ?? default(object);
                jan = rdr[10] as object ?? default(object);
                feb = rdr[11] as object ?? default(object);
                mar = rdr[12] as object ?? default(object);
                apr = rdr[13] as object ?? default(object);
                may = rdr[14] as object ?? default(object);
                jun = rdr[15] as object ?? default(object);
                jul = rdr[16] as object ?? default(object);
                aug = rdr[17] as object ?? default(object);
                sep = rdr[18] as object ?? default(object);
                oct = rdr[19] as object ?? default(object);
                nov = rdr[20] as object ?? default(object);
                dec = rdr[21] as object ?? default(object);
                subscriber = rdr[22] as object ?? default(object);
                tFormat = rdr[23] as object ?? default(object);
                nFormat = rdr[24] as object ?? default(object);
                all12tFormat = rdr[25] as object ?? default(object);
                subscriberFormat = rdr[26] as object ?? default(object);
                address = rdr[27] as object ?? default(object);
                city = rdr[28] as object ?? default(object);
                state = rdr[29] as object ?? default(object);
                zip = rdr[30] as object ?? default(object);

                int _templateID = templateID.checkIntNull();
                int _carrierID2 = carrierID.checkIntNull(); 
                int _columns = columns.checkIntNull();
                int _empDepLink = employeeDependentLink.checkIntNull();
                int _fname = fname.checkIntNull();
                int _mname = mname.checkIntNull();
                int _lname = lname.checkIntNull();
                int _ssn = ssn.checkIntNull();
                int _dob = dob.checkIntNull();
                int _all12 = all12.checkIntNull();
                int _jan = jan.checkIntNull();
                int _feb = feb.checkIntNull();
                int _march = mar.checkIntNull();
                int _april = apr.checkIntNull();
                int _may = may.checkIntNull();
                int _june = jun.checkIntNull();
                int _july = jul.checkIntNull();
                int _august = aug.checkIntNull();
                int _sep = sep.checkIntNull();
                int _oct = oct.checkIntNull();
                int _nov = nov.checkIntNull();
                int _dec = dec.checkIntNull();
                int _subscriber = subscriber.checkIntNull();
                string _nFormat = nFormat.checkStringNull();
                string _tFormat = tFormat.checkStringNull();
                string _all12tFormat = all12tFormat.checkStringNull();
                string _subscriberFormat = subscriberFormat.checkStringNull();
                int _address = address.checkIntNull();
                int _city = city.checkIntNull();
                int _state = state.checkIntNull();
                int _zip = zip.checkIntNull();

                ict = new insurance_coverage_template(_templateID, _carrierID, _columns, _empDepLink, _fname, _mname, _lname, _ssn, _dob, _all12, _jan, _feb, _march, _april, _may, _june, _july, _august, _sep, _oct, _nov, _dec, _tFormat, _nFormat, _all12tFormat, _subscriber, _subscriberFormat, _address, _city, _state, _zip);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            ict = null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return ict;
    }

    /// <summary>
    /// Retrieves the different types of insurance from the ACA database.
    /// </summary>
    public List<insuranceType> ManufactureInsuranceTypeList()
    {

        List<insuranceType> tempList = new List<insuranceType>();

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_all_insurance_types", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object id = 0;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = id.checkIntNull();
                string _name = name.checkStringNull();

                insuranceType it = new insuranceType(_id, _name);
                tempList.Add(it);

            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            return new List<insuranceType>();

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }
            
            if (rdr != null)
            {
                rdr.Close();
            }
        
        }

        return tempList;

    }

    public void transferInsuranceCarrierImportData(int _employerID, string _modBy)
    {
        List<insurance_coverage_I> currIC = insuranceController.manufactureInsuranceCoverageAlerts(_employerID);
        foreach (insurance_coverage_I ici in currIC)
        {
            if (ici.IC_EMPLOYEE_ID > 0 || ici.IC_DEPENDENT_ID > 0)
            {
                string history = ici.IC_FIRST_NAME + " , " + ici.IC_LAST_NAME + " | ";
                history += "SUBSCRIBER:" + ici.IC_SUBSCRIBER_I + " | ";
                history += "JAN:" + ici.IC_JAN_I + " | ";
                history += "FEB:" + ici.IC_FEB_I + " | ";
                history += "MAR:" + ici.IC_MAR_I + " | ";
                history += "APR:" + ici.IC_APR_I + " | ";
                history += "MAY:" + ici.IC_MAY_I + " | ";
                history += "JUN:" + ici.IC_JUN_I + " | ";
                history += "JUL:" + ici.IC_JUL_I + " | ";
                history += "AUG:" + ici.IC_AUG_I + " | ";
                history += "SEP:" + ici.IC_SEP_I + " | ";
                history += "OCT:" + ici.IC_OCT_I + " | ";
                history += "NOV:" + ici.IC_NOV_I + " | ";
                history += "DEC:" + ici.IC_DEC_I + " | ";
                history += "ALL12:" + ici.IC_ALL_12_I + Environment.NewLine;

                int _dependentID = ici.IC_DEPENDENT_ID.checkIntNull();

                transferInsuranceCarrierImportRow(ici.ROW_ID, ici.IC_BatchID, ici.IC_TAX_YEAR, ici.IC_CARRIER_ID, ici.IC_EMPLOYEE_ID, _dependentID, ici.IC_ALL_12, ici.IC_JAN, ici.IC_FEB, ici.IC_MAR, ici.IC_APR, ici.IC_MAY, ici.IC_JUN, ici.IC_JUL, ici.IC_AUG, ici.IC_SEP, ici.IC_OCT, ici.IC_NOV, ici.IC_DEC, history, _modBy);
            }
        }
    }

    private bool transferInsuranceCarrierImportRow(int _rowID, int _batchID, int _taxYear, int _carrierID, int _employeeID, int _dependentID, bool _all12, bool _jan, bool _feb, bool _mar, bool _apr, bool _may, bool _jun, bool _jul, bool _aug, bool _sep, bool _oct, bool _nov, bool _dec, string _history, string _modBy)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("TRANSFER_import_existing_insurance_carrier_imports", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@carrierID", SqlDbType.Int).Value = _carrierID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.VarChar).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dependentID", SqlDbType.VarChar).Value = _dependentID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@all12", SqlDbType.Bit).Value = _all12.checkBoolNull();
            cmd.Parameters.AddWithValue("@jan", SqlDbType.Bit).Value = _jan.checkBoolNull();
            cmd.Parameters.AddWithValue("@feb", SqlDbType.Bit).Value = _feb.checkBoolNull();
            cmd.Parameters.AddWithValue("@mar", SqlDbType.Bit).Value = _mar.checkBoolNull();
            cmd.Parameters.AddWithValue("@apr", SqlDbType.Bit).Value = _apr.checkBoolNull();
            cmd.Parameters.AddWithValue("@may", SqlDbType.Bit).Value = _may.checkBoolNull();
            cmd.Parameters.AddWithValue("@jun", SqlDbType.Bit).Value = _jun.checkBoolNull();
            cmd.Parameters.AddWithValue("@jul", SqlDbType.Bit).Value = _jul.checkBoolNull();
            cmd.Parameters.AddWithValue("@aug", SqlDbType.Bit).Value = _aug.checkBoolNull();
            cmd.Parameters.AddWithValue("@sep", SqlDbType.Bit).Value = _sep.checkBoolNull();
            cmd.Parameters.AddWithValue("@oct", SqlDbType.Bit).Value = _oct.checkBoolNull();
            cmd.Parameters.AddWithValue("@nov", SqlDbType.Bit).Value = _nov.checkBoolNull();
            cmd.Parameters.AddWithValue("@dec", SqlDbType.Bit).Value = _dec.checkBoolNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();
            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID.checkForDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    public bool updateInsuranceCoverageRow(int _rowID, bool _jan, bool _feb, bool _mar, bool _apr, bool _may, bool _jun, bool _jul, bool _aug, bool _sep, bool _oct, bool _nov, bool _dec, string _history)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_employee_LINEIII_Months", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@row_id", SqlDbType.Int).Value = _rowID.checkIntNull();
            cmd.Parameters.AddWithValue("@jan", SqlDbType.Bit).Value = _jan.checkBoolNull();
            cmd.Parameters.AddWithValue("@feb", SqlDbType.Bit).Value = _feb.checkBoolNull();
            cmd.Parameters.AddWithValue("@mar", SqlDbType.Bit).Value = _mar.checkBoolNull();
            cmd.Parameters.AddWithValue("@apr", SqlDbType.Bit).Value = _apr.checkBoolNull();
            cmd.Parameters.AddWithValue("@may", SqlDbType.Bit).Value = _may.checkBoolNull();
            cmd.Parameters.AddWithValue("@jun", SqlDbType.Bit).Value = _jun.checkBoolNull();
            cmd.Parameters.AddWithValue("@jul", SqlDbType.Bit).Value = _jul.checkBoolNull();
            cmd.Parameters.AddWithValue("@aug", SqlDbType.Bit).Value = _aug.checkBoolNull();
            cmd.Parameters.AddWithValue("@sep", SqlDbType.Bit).Value = _sep.checkBoolNull();
            cmd.Parameters.AddWithValue("@oct", SqlDbType.Bit).Value = _oct.checkBoolNull();
            cmd.Parameters.AddWithValue("@nov", SqlDbType.Bit).Value = _nov.checkBoolNull();
            cmd.Parameters.AddWithValue("@dec", SqlDbType.Bit).Value = _dec.checkBoolNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Exception updateInsuranceCoverageRow.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// 
    /// </summary>
    public Boolean insertNewInsuranceCarrierImportRow(
            int _batchID,
            int _taxYear,
            int _employerID,
            String _dependentLink,
            String _fname,
            String _mname,
            String _lname,
            String _ssn,
            DateTime? _dob,
            Boolean jan,
            Boolean feb,
            Boolean mar,
            Boolean apr,
            Boolean may,
            Boolean jun,
            Boolean jul,
            Boolean aug,
            Boolean sep,
            Boolean oct,
            Boolean nov,
            Boolean dec,
            Boolean all12,
            Boolean subscriber,
            String _jan,
            String _feb,
            String _mar,
            String _apr,
            String _may,
            String _jun,
            String _jul,
            String _aug,
            String _sep,
            String _oct,
            String _nov,
            String _dec,
            String _all12,
            String _subscriberi,
            String _address,
            String _city,
            String _state,
            String _zip,
            int _carrierID
        )
    {

        Boolean validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_import_insurance_carrier_report", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID.checkIntNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkForDBNull();
            cmd.Parameters.AddWithValue("@dependentLink", SqlDbType.VarChar).Value = _dependentLink.checkForDBNull();
            cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@mname", SqlDbType.VarChar).Value = _mname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.VarChar).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.DateTime).Value = _dob.checkForDBNull();
            cmd.Parameters.AddWithValue("@jan", SqlDbType.Bit).Value = jan.checkBoolNull();
            cmd.Parameters.AddWithValue("@feb", SqlDbType.Bit).Value = feb.checkBoolNull();
            cmd.Parameters.AddWithValue("@mar", SqlDbType.Bit).Value = mar.checkBoolNull();
            cmd.Parameters.AddWithValue("@apr", SqlDbType.Bit).Value = apr.checkBoolNull();
            cmd.Parameters.AddWithValue("@may", SqlDbType.Bit).Value = may.checkBoolNull();
            cmd.Parameters.AddWithValue("@jun", SqlDbType.Bit).Value = jun.checkBoolNull();
            cmd.Parameters.AddWithValue("@jul", SqlDbType.Bit).Value = jul.checkBoolNull();
            cmd.Parameters.AddWithValue("@aug", SqlDbType.Bit).Value = aug.checkBoolNull();
            cmd.Parameters.AddWithValue("@sep", SqlDbType.Bit).Value = sep.checkBoolNull();
            cmd.Parameters.AddWithValue("@oct", SqlDbType.Bit).Value = oct.checkBoolNull();
            cmd.Parameters.AddWithValue("@nov", SqlDbType.Bit).Value = nov.checkBoolNull();
            cmd.Parameters.AddWithValue("@dec", SqlDbType.Bit).Value = dec.checkBoolNull();
            cmd.Parameters.AddWithValue("@subscriber", SqlDbType.Bit).Value = subscriber.checkBoolNull();
            cmd.Parameters.AddWithValue("@all12", SqlDbType.Bit).Value = all12.checkBoolNull();
            cmd.Parameters.AddWithValue("@jani", SqlDbType.VarChar).Value = _jan.checkForDBNull();
            cmd.Parameters.AddWithValue("@febi", SqlDbType.VarChar).Value = _feb.checkForDBNull();
            cmd.Parameters.AddWithValue("@mari", SqlDbType.VarChar).Value = _mar.checkForDBNull();
            cmd.Parameters.AddWithValue("@apri", SqlDbType.VarChar).Value = _apr.checkForDBNull();
            cmd.Parameters.AddWithValue("@mayi", SqlDbType.VarChar).Value = _may.checkForDBNull();
            cmd.Parameters.AddWithValue("@juni", SqlDbType.VarChar).Value = _jun.checkForDBNull();
            cmd.Parameters.AddWithValue("@juli", SqlDbType.VarChar).Value = _jul.checkForDBNull();
            cmd.Parameters.AddWithValue("@augi", SqlDbType.VarChar).Value = _aug.checkForDBNull();
            cmd.Parameters.AddWithValue("@sepi", SqlDbType.VarChar).Value = _sep.checkForDBNull();
            cmd.Parameters.AddWithValue("@octi", SqlDbType.VarChar).Value = _oct.checkForDBNull();
            cmd.Parameters.AddWithValue("@novi", SqlDbType.VarChar).Value = _nov.checkForDBNull();
            cmd.Parameters.AddWithValue("@deci", SqlDbType.VarChar).Value = _dec.checkForDBNull();
            cmd.Parameters.AddWithValue("@all12i", SqlDbType.VarChar).Value = _all12.checkForDBNull();
            cmd.Parameters.AddWithValue("@subscriberi", SqlDbType.VarChar).Value = _subscriberi.checkForDBNull();
            cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = _address.checkForDBNull();
            cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = _city.checkForDBNull();
            cmd.Parameters.AddWithValue("@state", SqlDbType.VarChar).Value = _state.checkForDBNull();
            cmd.Parameters.AddWithValue("@zip", SqlDbType.VarChar).Value = _zip.checkForDBNull();
            cmd.Parameters.AddWithValue("@carrierID", SqlDbType.Int).Value = _carrierID.checkForDBNull();


            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// This will remove all records pertaining to a single Batch ID.
    /// </summary>
    public bool DeleteInsuranceCarrierBatch(int _batchID)
    {

        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_carrier_batch_import", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID.checkIntNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// Generate a single Insurance Alert Offer. 
    /// </summary>
    public List<insurance_coverage_I> manufactureInsuranceCoverageAlerts(int _employerID)
    {
        List<insurance_coverage_I> tempList = new List<insurance_coverage_I>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employer_insurance_coverage_import_alerts";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            rdr = cmd.ExecuteReader();

            tempList = manufactureInsuranceCoverageI(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage_I>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all dependent insurance coverage for a specific employee and tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<insurance_coverage> manufactureDependentInsuranceCoverage(int _employeeID, int _taxYear)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_dependent_coverage";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = manufactureDependentInsuranceCoverage(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all insurance coverage for a specific employee and tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<insurance_coverage> manufactureAllInsuranceCoverage(int _employeeID, int _taxYear)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_all_individual_coverage";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = manufactureDependentInsuranceCoverage(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all insurance coverage for a specific employee regardless of the tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<insurance_coverage> manufactureAllInsuranceCoverageForEmployee(int _employeeID)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_individual_coverage_all_tax_years";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;


            rdr = cmd.ExecuteReader();

            tempList = manufactureDependentInsuranceCoverage(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all insurance coverage for a specific employee's dependents regardless of the tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<insurance_coverage> manufactureAllInsuranceCoverageForEmployeeDependents(int _employeeID)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_dependent_coverage_all_tax_years";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;


            rdr = cmd.ExecuteReader();

            tempList = manufactureDependentInsuranceCoverage(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all insurance coverage for a specific employee and tax year.
    /// </summary>
    /// <param name="_planYearID"></param>
    /// <param name="_employeeID"></param>
    /// <returns></returns>
    public List<insurance_coverage> manufactureAllEditableInsuranceCoverage(int _employeeID, int _taxYear)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_editable_individual_coverage";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = manufactureEditableInsuranceCoverage(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in manufactureAllEditableInsuranceCoverage", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return tempList;
    }

    /// <summary>
    /// Get all dependent insurance coverage for a specific employee and tax year. 
    /// </summary>
    public List<insurance_coverage> ManufactureEmployeeInsuranceCoverage(int _employeeID, int _taxYear)
    {

        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "SELECT_employee_coverage";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            tempList = manufactureDependentInsuranceCoverage(rdr);

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        
        }
        finally
        {
            
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            
            if (cmd != null)
            {
                cmd.Dispose();
            }
            
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        
        }

        return tempList;
    
    }

    /// <summary>
    /// Get all dependent insurance coverage for a specific employee and tax year. 
    /// </summary>
    public IList<InsuranceCoverageEditable> ManufactureInsuranceCoverageEditable(int _employeeID, int _taxYear)
    {

        IList<InsuranceCoverageEditable> insuranceCoverageEditables = new List<InsuranceCoverageEditable>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {

            conn.Open();

            cmd.CommandText = "[dbo].[SELECT_employee_editable_individual_coverage]";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            insuranceCoverageEditables = ManufactureInsuranceCoverageEditable(rdr);

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            insuranceCoverageEditables = new List<InsuranceCoverageEditable>();
        
        }
        finally
        {
            
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            
            if (cmd != null)
            {
                cmd.Dispose();
            }
            
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        
        }

        return insuranceCoverageEditables;
    }

    /// <summary>
    /// Get a list of dependent and employee editable coverage non tax year specific. 
    /// </summary>
    public List<insurance_coverage> ManufactureInsuranceCoverageEditableWithNames(int _employeeID)
    {
        List<insurance_coverage> tempList = new List<insurance_coverage>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rdr = null;

        try
        {
            conn.Open();

            cmd.CommandText = "[dbo].[SELECT_employee_editable_individual_coverage_with_names]";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            tempList = ManufactureInsuranceCoverageEditableWithNames(rdr);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            tempList = new List<insurance_coverage>();
        }
        finally
        {

            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }

            if (cmd != null)
            {
                cmd.Dispose();
            }

            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }

        }

        return tempList;
    }


    /// <summary>
    /// This will generate the list of Alert_Insurance objects from the rdr object. 
    /// </summary>
    /// <returns></returns>
    private List<insurance_coverage_I> manufactureInsuranceCoverageI(SqlDataReader rdr)
    {

        List<insurance_coverage_I> iciList = new List<insurance_coverage_I>();

        while (rdr.Read())
        {
            insurance_coverage_I newICI = null;
            object rowID = null;
            object batchID = null;
            object employerID = null;
            object taxYear = null;
            object employeeID = null;
            object dependentLink = null;
            object dependentID = null;
            object fname = null;
            object mname = null;
            object lname = null;
            object ssn = null;
            object dob = null;
            object jan = null;
            object feb = null;
            object mar = null;
            object apr = null;
            object may = null;
            object jun = null;
            object jul = null;
            object aug = null;
            object sep = null;
            object oct = null;
            object nov = null;
            object dec = null;
            object subscriber = null;
            object all12 = null;
            object stateID = null;
            object zip = null;
            object jani = null;
            object febi = null;
            object mari = null;
            object apri = null;
            object mayi = null;
            object juni= null;
            object juli = null;
            object augi = null;
            object sepi = null;
            object octi = null;
            object novi = null;
            object deci = null;
            object subscriberi = null;
            object all12i = null;
            object addressi = null;
            object cityi = null;
            object statei = null;
            object zipi = null;
            object carrierID = null;

            rowID = rdr[0] as object ?? default(object);
            batchID = rdr[1] as object ?? default(object);
            employerID = rdr[2] as object ?? default(object);
            taxYear = rdr[3] as object ?? default(object);
            employeeID = rdr[4] as object ?? default(object);
            dependentLink = rdr[5] as object ?? default(object);
            dependentID = rdr[6] as object ?? default(object);
            fname = rdr[7] as object ?? default(object);
            lname = rdr[8] as object ?? default(object);
            mname = rdr[9] as object ?? default(object);
            ssn = rdr[10] as object ?? default(object);
            dob = rdr[11] as object ?? default(object);
            jan = rdr[12] as object ?? default(object);
            feb = rdr[13] as object ?? default(object);
            mar = rdr[14] as object ?? default(object);
            apr = rdr[15] as object ?? default(object);
            may = rdr[16] as object ?? default(object);
            jun = rdr[17] as object ?? default(object);
            jul = rdr[18] as object ?? default(object);
            aug = rdr[19] as object ?? default(object);
            sep = rdr[20] as object ?? default(object);
            oct = rdr[21] as object ?? default(object);
            nov = rdr[22] as object ?? default(object);
            dec = rdr[23] as object ?? default(object);
            subscriber = rdr[24] as object ?? default(object);
            all12 = rdr[25] as object ?? default(object);
            stateID = rdr[26] as object ?? default(object);
            zip = rdr[27] as object ?? default(object);
            jani = rdr[28] as object ?? default(object);
            febi = rdr[29] as object ?? default(object);
            mari = rdr[30] as object ?? default(object);
            apri = rdr[31] as object ?? default(object);
            mayi = rdr[32] as object ?? default(object);
            juni = rdr[33] as object ?? default(object);
            juli = rdr[34] as object ?? default(object);
            augi = rdr[35] as object ?? default(object);
            sepi = rdr[36] as object ?? default(object);
            octi = rdr[37] as object ?? default(object);
            novi = rdr[38] as object ?? default(object);
            deci = rdr[39] as object ?? default(object);
            all12i = rdr[40] as object ?? default(object);
            subscriberi = rdr[41] as object ?? default(object);
            addressi = rdr[42] as object ?? default(object);
            cityi = rdr[43] as object ?? default(object);
            statei = rdr[44] as object ?? default(object);
            zipi = rdr[45] as object ?? default(object);
            carrierID = rdr[46] as object ?? default(object);
            
            int _rowID = rowID.checkIntNull();
            int _batchID = batchID.checkIntNull();
            int _employerID2 = employerID.checkIntNull();
            int _taxYear = taxYear.checkIntNull();
            int _employeeID = employeeID.checkIntNull();
            string _dependentLink = dependentLink.checkStringNull();
            int? _dependentID = dependentID.checkIntNull();
            string _fname = fname.checkStringNull();
            string _mname = mname.checkStringNull();
            string _lname = lname.checkStringNull();
            string _ssn = ssn.checkStringNull();
            DateTime? _dob = dob.checkDateNull();
            bool _jan = jan.checkBoolNull();
            bool _feb = feb.checkBoolNull();
            bool _mar = mar.checkBoolNull();
            bool _apr = apr.checkBoolNull();
            bool _may = may.checkBoolNull();
            bool _jun = jun.checkBoolNull();
            bool _jul = jul.checkBoolNull();
            bool _aug = aug.checkBoolNull();
            bool _sep = sep.checkBoolNull();
            bool _oct = oct.checkBoolNull();
            bool _nov = nov.checkBoolNull();
            bool _dec = dec.checkBoolNull();
            bool _subscriber = subscriber.checkBoolNull();
            bool _all12 = all12.checkBoolNull();
            int _stateID = stateID.checkIntNull();
            int _zip = zip.checkIntNull();
            string _jani = jani.checkStringNull();
            string _febi = febi.checkStringNull();
            string _mari = mari.checkStringNull();
            string _apri = apri.checkStringNull();
            string _mayi = mayi.checkStringNull();
            string _juni = juni.checkStringNull();
            string _juli = juli.checkStringNull();
            string _augi = augi.checkStringNull();
            string _sepi = sepi.checkStringNull();
            string _octi = octi.checkStringNull();
            string _novi = novi.checkStringNull();
            string _deci = deci.checkStringNull();
            string _subscriberi = subscriberi.checkStringNull();
            string _all12i = all12i.checkStringNull();
            string _addressi = addressi.checkStringNull();
            string _cityi = cityi.checkStringNull();
            string _statei = statei.checkStringNull();
            string _zipi = zipi.checkStringNull();
            int _carrierID = carrierID.checkIntNull();

            if (_ssn != null)
            {
                _ssn = AesEncryption.Decrypt(_ssn);
            }

            newICI = new insurance_coverage_I(_rowID, _batchID, _employerID2, _employeeID, _taxYear, _dependentLink, _dependentID, _fname, _mname, _lname, _ssn, _dob, _all12, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, _all12i, _subscriber, _jani, _febi, _mari, _apri, _mayi, _juni, _juli, _augi, _sepi, _octi, _novi, _deci, _subscriberi, _addressi, _cityi, _statei, _stateID, _zipi, _zip, _carrierID);
            iciList.Add(newICI);
        }

        return iciList;
    }

    /// <summary>
    /// This will generate the list of Dependent Insurance Coverage objects from the rdr object. 
    /// </summary>
    /// <returns></returns>
    private List<insurance_coverage> manufactureDependentInsuranceCoverage(SqlDataReader rdr)
    {

        List<insurance_coverage> icList = new List<insurance_coverage>();

        while (rdr.Read())
        {
            insurance_coverage newIC = null;
            object batchID = null;
            object dependentID = null;
            object employeeID = null;
            object fname = null;
            object mname = null;
            object lname = null;
            object ssn = null;
            object dob = null;
            object rowID = null;
            object taxYear = null;
            object carrierID = null;
            object all12 = null;
            object jan = null;
            object feb = null;
            object mar = null;
            object apr = null;
            object may = null;
            object jun = null;
            object jul = null;
            object aug = null;
            object sep = null;
            object oct = null;
            object nov = null;
            object dec = null;
            object history = null;
            object resourceID = null;

            dependentID = rdr[0] as object ?? default(object);
            employeeID = rdr[1] as object ?? default(object);
            fname = rdr[2] as object ?? default(object);
            mname = rdr[3] as object ?? default(object);
            lname = rdr[4] as object ?? default(object);
            ssn = rdr[5] as object ?? default(object);
            dob = rdr[6] as object ?? default(object);
            rowID = rdr[7] as object ?? default(object);
            taxYear = rdr[8] as object ?? default(object);
            carrierID = rdr[9] as object ?? default(object);
            all12 = rdr[10] as object ?? default(object);
            jan = rdr[11] as object ?? default(object);
            feb = rdr[12] as object ?? default(object);
            mar = rdr[13] as object ?? default(object);
            apr = rdr[14] as object ?? default(object);
            may = rdr[15] as object ?? default(object);
            jun = rdr[16] as object ?? default(object);
            jul = rdr[17] as object ?? default(object);
            aug = rdr[18] as object ?? default(object);
            sep = rdr[19] as object ?? default(object);
            oct = rdr[20] as object ?? default(object);
            nov = rdr[21] as object ?? default(object);
            dec = rdr[22] as object ?? default(object);
            history = String.Empty;         
            resourceID = rdr[24] as object ?? default(object);
            batchID = rdr[25] as object ?? default(object);

            int _dependentID = dependentID.checkIntNull();
            int _employeeID = employeeID.checkIntNull();
            string _fname = fname.checkStringNull();
            string _mname = mname.checkStringNull();
            string _lname = lname.checkStringNull();
            string _ssn = ssn.checkStringNull();
            DateTime? _dob = dob.checkDateNull();
            int _rowID = rowID.checkIntNull();
            int _taxYear = taxYear.checkIntNull();
            int _carrierID = carrierID.checkIntNull();
            bool _all12 = all12.checkBoolNull();
            bool _jan = jan.checkBoolNull();
            bool _feb = feb.checkBoolNull();
            bool _mar = mar.checkBoolNull();
            bool _apr = apr.checkBoolNull();
            bool _may = may.checkBoolNull();
            bool _jun = jun.checkBoolNull();
            bool _jul = jul.checkBoolNull();
            bool _aug = aug.checkBoolNull();
            bool _sep = sep.checkBoolNull();
            bool _oct = oct.checkBoolNull();
            bool _nov = nov.checkBoolNull();
            bool _dec = dec.checkBoolNull();
            string _history = history.checkStringNull();
            int _batchID = batchID.checkIntNull();

            if (_ssn != null)
            {
                _ssn = AesEncryption.Decrypt(_ssn);
            }

            newIC = new insurance_coverage(_rowID, _batchID, 0, _employeeID, _taxYear, null, _dependentID, _fname, _mname, _lname, _ssn, _dob, _all12, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, false, _carrierID, _history);
            icList.Add(newIC);
        }

        return icList;
    }

    /// <summary>
    /// This will generate the list of Dependent Insurance Coverage objects from the rdr object. 
    /// </summary>
    /// <returns></returns>
    private List<insurance_coverage> manufactureEditableInsuranceCoverage(SqlDataReader rdr)
    {

        List<insurance_coverage> icList = new List<insurance_coverage>();

        while (rdr.Read())
        {
            insurance_coverage newIC = null;
            object dependentID = null;
            object employeeID = null;
            object employerID = null;
            object fname = null;
            object mname = null;
            object lname = null;
            object ssn = null;
            object dob = null;
            object rowID = null;
            object taxYear = null;
            object carrierID = null;
            object all12 = null;
            object jan = null;
            object feb = null;
            object mar = null;
            object apr = null;
            object may = null;
            object jun = null;
            object jul = null;
            object aug = null;
            object sep = null;
            object oct = null;
            object nov = null;
            object dec = null;
            object history = null;

            rowID = rdr[0] as object ?? default(object);
            employeeID = rdr[1] as object ?? default(object);
            employerID = rdr[2] as object ?? default(object);
            dependentID = rdr[3] as object ?? default(object);
            taxYear = rdr[4] as object ?? default(object);                      
            jan = rdr[5] as object ?? default(object);
            feb = rdr[6] as object ?? default(object);
            mar = rdr[7] as object ?? default(object);
            apr = rdr[8] as object ?? default(object);
            may = rdr[9] as object ?? default(object);
            jun = rdr[10] as object ?? default(object);
            jul = rdr[11] as object ?? default(object);
            aug = rdr[12] as object ?? default(object);
            sep = rdr[13] as object ?? default(object);
            oct = rdr[14] as object ?? default(object);
            nov = rdr[15] as object ?? default(object);
            dec = rdr[16] as object ?? default(object);
            history = String.Empty;         

            int _dependentID = dependentID.checkIntNull();
            int _employeeID = employeeID.checkIntNull();
            string _fname = null;
            string _mname = null;
            string _lname = null;
            string _ssn = null;
            DateTime? _dob = null;
            int _rowID = rowID.checkIntNull();
            int _taxYear = taxYear.checkIntNull();
            bool _all12 = false;
            bool _jan = jan.checkBoolNull();
            bool _feb = feb.checkBoolNull();
            bool _mar = mar.checkBoolNull();
            bool _apr = apr.checkBoolNull();
            bool _may = may.checkBoolNull();
            bool _jun = jun.checkBoolNull();
            bool _jul = jul.checkBoolNull();
            bool _aug = aug.checkBoolNull();
            bool _sep = sep.checkBoolNull();
            bool _oct = oct.checkBoolNull();
            bool _nov = nov.checkBoolNull();
            bool _dec = dec.checkBoolNull();
            string _history = history.checkStringNull();

            if (_ssn != null)
            {
                _ssn = AesEncryption.Decrypt(_ssn);
            }

            newIC = new insurance_coverage(_rowID, 0, 0, _employeeID, _taxYear, null, _dependentID, _fname, _mname, _lname, _ssn, _dob, _all12, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, false, 0, _history);
            icList.Add(newIC);
        }

        return icList;
    }

    /// <summary>
    /// Pulls out the editable coverage objects from the SqlDataReader. 
    /// </summary>
    private IList<InsuranceCoverageEditable> ManufactureInsuranceCoverageEditable(SqlDataReader rdr)
    {

        IList<InsuranceCoverageEditable> insuranceCoverageEditables = new List<InsuranceCoverageEditable>();

        while (rdr.Read())
        {

            InsuranceCoverageEditable insuranceCoverageEditable = null;

            Object rowID = null;
            Object employeeID = null;
            Object employerID = null;
            Object dependentID = null;
            Object taxYear = null;
            Object jan = null;
            Object feb = null;
            Object mar = null;
            Object apr = null;
            Object may = null;
            Object jun = null;
            Object jul = null;
            Object aug = null;
            Object sep = null;
            Object oct = null;
            Object nov = null;
            Object dec = null;
            Object resourceId = null;

            rowID = rdr[0] as Object ?? default(Object);
            employeeID = rdr[1] as Object ?? default(Object);
            employerID = rdr[2] as Object ?? default(Object);
            dependentID = rdr[3] as Object ?? default(Object);
            taxYear = rdr[4] as Object ?? default(Object);
            jan = rdr[5] as Object ?? default(Object);
            feb = rdr[6] as Object ?? default(Object);
            mar = rdr[7] as Object ?? default(Object);
            apr = rdr[8] as Object ?? default(Object);
            may = rdr[9] as Object ?? default(Object);
            jun = rdr[10] as Object ?? default(Object);
            jul = rdr[11] as Object ?? default(Object);
            aug = rdr[12] as Object ?? default(Object);
            sep = rdr[13] as Object ?? default(Object);
            oct = rdr[14] as Object ?? default(Object);
            nov = rdr[15] as Object ?? default(Object);
            dec = rdr[16] as Object ?? default(Object);
            resourceId = rdr[17] as Object ?? default(Object);

            int _rowID = rowID.checkIntNull();
            int _employeeID = employeeID.checkIntNull();
            int _employerID = employerID.checkIntNull();
            int? _dependentID = dependentID.checkIntNull();
            int _taxYear = taxYear.checkIntNull();
            Boolean _jan = jan.checkBoolNull();
            Boolean _feb = feb.checkBoolNull();
            Boolean _mar = mar.checkBoolNull();
            Boolean _apr = apr.checkBoolNull();
            Boolean _may = may.checkBoolNull();
            Boolean _jun = jun.checkBoolNull();
            Boolean _jul = jul.checkBoolNull();
            Boolean _aug = aug.checkBoolNull();
            Boolean _sep = sep.checkBoolNull();
            Boolean _oct = oct.checkBoolNull();
            Boolean _nov = nov.checkBoolNull();
            Boolean _dec = dec.checkBoolNull();
            Guid _resourceId = resourceId.checkGuidNull();

            insuranceCoverageEditable = new InsuranceCoverageEditable(
                    _rowID,
                    _employeeID,
                    _employerID,
                    _dependentID,
                    _taxYear,
                    _jan,
                    _feb,
                    _mar,
                    _apr,
                    _may,
                    _jun,
                    _jul,
                    _aug,
                    _sep,
                    _oct,
                    _nov,
                    _dec,
                    _resourceId
                );

            insuranceCoverageEditables.Add(insuranceCoverageEditable);
        
        }

        return insuranceCoverageEditables;
    
    }

    /// <summary>
    /// Pulls out the editable coverage objects from the SqlDataReader. 
    /// </summary>
    private List<insurance_coverage> ManufactureInsuranceCoverageEditableWithNames(SqlDataReader rdr)
    {
        List<insurance_coverage> icList = new List<insurance_coverage>();

        while (rdr.Read())
        {
            Object rowID = null;
            Object employeeID = null;
            Object employerID = null;
            Object dependentID = null;
            Object taxYear = null;
            Object jan = null;
            Object feb = null;
            Object mar = null;
            Object apr = null;
            Object may = null;
            Object jun = null;
            Object jul = null;
            Object aug = null;
            Object sep = null;
            Object oct = null;
            Object nov = null;
            Object dec = null;
            Object fname = null;
            Object mname = null;
            Object lname = null;
            Object ssn = null;
            Object dob = null;

            rowID = rdr[0] as Object ?? default(Object);
            employeeID = rdr[1] as Object ?? default(Object);
            employerID = rdr[2] as Object ?? default(Object);
            dependentID = rdr[3] as Object ?? default(Object);
            taxYear = rdr[4] as Object ?? default(Object);
            jan = rdr[5] as Object ?? default(Object);
            feb = rdr[6] as Object ?? default(Object);
            mar = rdr[7] as Object ?? default(Object);
            apr = rdr[8] as Object ?? default(Object);
            may = rdr[9] as Object ?? default(Object);
            jun = rdr[10] as Object ?? default(Object);
            jul = rdr[11] as Object ?? default(Object);
            aug = rdr[12] as Object ?? default(Object);
            sep = rdr[13] as Object ?? default(Object);
            oct = rdr[14] as Object ?? default(Object);
            nov = rdr[15] as Object ?? default(Object);
            dec = rdr[16] as Object ?? default(Object);
            fname = rdr[17] as Object ?? default(Object);
            mname = rdr[18] as Object ?? default(Object);
            lname = rdr[19] as Object ?? default(Object);

            int _rowID = rowID.checkIntNull();
            int _employeeID = employeeID.checkIntNull();
            int _employerID = employerID.checkIntNull();
            int? _dependentID = dependentID.checkIntNull();
            int _taxYear = taxYear.checkIntNull();
            Boolean _jan = jan.checkBoolNull();
            Boolean _feb = feb.checkBoolNull();
            Boolean _mar = mar.checkBoolNull();
            Boolean _apr = apr.checkBoolNull();
            Boolean _may = may.checkBoolNull();
            Boolean _jun = jun.checkBoolNull();
            Boolean _jul = jul.checkBoolNull();
            Boolean _aug = aug.checkBoolNull();
            Boolean _sep = sep.checkBoolNull();
            Boolean _oct = oct.checkBoolNull();
            Boolean _nov = nov.checkBoolNull();
            Boolean _dec = dec.checkBoolNull();
            string _fname = fname.checkStringNull();
            string _mname = mname.checkStringNull();
            string _lname = lname.checkStringNull();

            insurance_coverage ic = new insurance_coverage(_rowID, 0, _employerID, _employeeID, _taxYear, null, _dependentID, _fname, _mname, _lname, null, null, false, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec, false, 0, null);
            icList.Add(ic);
        }

        return icList;
    }

    /// <summary>
    /// This will update the EmployeeID and DependentID.
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool updateInsuranceCoverageAlert(int _rowID, int _employeeID, int? _dependentID, string _ssn, DateTime? _dob, string _fname, string _lname, bool _subscriber)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE_insurance_coverage_import", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (_ssn != null)
            {
                _ssn = AesEncryption.Encrypt(_ssn);
            }

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dependentID", SqlDbType.Int).Value = _dependentID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@fname", SqlDbType.Int).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@lname", SqlDbType.Int).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.Int).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.Int).Value = _dob.checkDateDBNull();
            cmd.Parameters.AddWithValue("@subscriber", SqlDbType.Bit).Value = _subscriber.checkBoolNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// This will update the EmployeeID and DependentID.
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool deleteInsuranceCoverageAlert(int _rowID, string _modBy, DateTime _modOn)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_carrier_import_row", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 2)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// Pass in Employer ID, Loop through all of the Insurance Coverage Import Records and validate any 
    /// existing employees. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public bool ValidateCurrentEmployee(int _employerID)
    {

        List<insurance_coverage_I> currIC = insuranceController.manufactureInsuranceCoverageAlerts(_employerID);
        List<Employee> currEmployees = EmployeeController.manufactureEmployeeList(_employerID);

        try
        {

            foreach (insurance_coverage_I ici in currIC)
            {

                try
                {
                    
                    String ssn = ici.IC_SSN;
                    
                    foreach (Employee emp in currEmployees)
                    {
                        
                        if (String.Compare(ssn, emp.Employee_SSN_Visible, true) == 0 && ici.IC_SUBSCRIBER == true)
                        {
                            
                            ici.IC_EMPLOYEE_ID = emp.EMPLOYEE_ID;
                            
                            break;
                        
                        }
                    
                    }

                }
                catch (Exception exception)
                {
                    
                    this.Log.Warn(String.Format("Errors processing insurance carrier alert {0} during employee mapping.", ici.ROW_ID), exception);

                }

            }

            insuranceFactory inf = new insuranceFactory();

            foreach (insurance_coverage_I ici in currIC)
            {

                if (false == inf.updateInsuranceCoverageAlert(
                        ici.ROW_ID,
                        ici.IC_EMPLOYEE_ID,
                        ici.IC_DEPENDENT_ID,
                        ici.IC_SSN,
                        ici.IC_DOB,
                        ici.IC_FIRST_NAME,
                        ici.IC_LAST_NAME,
                        ici.IC_SUBSCRIBER
                    )) 
                {
                    Log.Warn("Failed to update Insurance Coverage for Row_Id: " + ici.ROW_ID);
                }
            
            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Failure while doing Validate Current Employee.", exception);

            return false;
        }
        return true;
    }

    /// <summary>
    /// Pass in Employer ID, Loop through all of the Insurance Coverage Import Records and validate any 
    /// existing Employee Dependents. 
    /// </summary>
    /// <param name="_employerID"></param>
    /// <returns></returns>
    public bool validateCurrentEmployeeDependents(int _employerID, string modBy)
    {

        List<insurance_coverage_I> currIC = insuranceController.manufactureInsuranceCoverageAlerts(_employerID);

        insuranceFactory inf = new insuranceFactory();

        try
        {
            int i = 0;
            foreach (insurance_coverage_I ici in currIC)
            {

                i += 1;    

                if (ici.IC_SUBSCRIBER == true && ici.IC_EMPLOYEE_ID > 0)
                {
                
                    foreach (insurance_coverage_I ici2 in currIC)
                    {
                    
                        Dependent newDependent = null;
                        if ((String.Compare(ici2.IC_DEPENDENT_EMPLOYEE_LINK, ici.IC_DEPENDENT_EMPLOYEE_LINK) == 0) && ici2.IC_SUBSCRIBER == false)
                        {

                            int depID = ici.IC_DEPENDENT_ID.checkIntNull();
                            
                            newDependent = EmployeeController.updateEmployeeDependent(
                                    depID,
                                    ici.IC_EMPLOYEE_ID,
                                    ici2.IC_FIRST_NAME,
                                    ici2.IC_MIDDLE_NAME,
                                    ici2.IC_LAST_NAME,
                                    ici2.IC_SSN,
                                    ici2.IC_DOB,
                                    modBy,
                                    1
                                );

                            if (newDependent == null) 
                            {           

                                Log.Info("Failed to update Dependent with DepId: " + depID);

                                continue;
                            }

                            ici2.IC_DEPENDENT_ID = newDependent.DEPENDENT_ID;
                            
                            inf.updateInsuranceCoverageAlert(
                                    ici2.ROW_ID,
                                    ici.IC_EMPLOYEE_ID,
                                    ici2.IC_DEPENDENT_ID,
                                    ici2.IC_SSN,
                                    ici2.IC_DOB,
                                    ici2.IC_FIRST_NAME,
                                    ici2.IC_LAST_NAME,
                                    ici2.IC_SUBSCRIBER
                                );
                        
                        }

                    }

                }

            }

        }
        catch (Exception exception)
        {

            this.Log.Error("Failure while validate Current Employee Dependents", exception);
            return false;
        }

        return true;
    }


    public void insertUpdateDependent(int rowID, int employeeID, string ssn, DateTime? dob)
    {
        
    }

    /// <summary>
    /// Moves all Employees that are select as the Subscriber and do not have an Employee ID.
    /// </summary>
    /// <param name="_employerID"></param>
    public void createAlertsForMissingEmployees(int _employerID, DateTime _modOn, string _modBy)
    {
        List<insurance_coverage_I> currIC = insuranceController.manufactureInsuranceCoverageAlerts(_employerID);
        int batchID = EmployeeController.manufactureBatchID(_employerID, _modOn, _modBy);
        string _ssn = null;

        foreach (insurance_coverage_I ici in currIC)
        {
            if (ici.IC_DEPENDENT_ID == 0 && ici.IC_EMPLOYEE_ID == 0 && ici.IC_SUBSCRIBER == true)
            {
                 string formattedDOB = null;

                 try
                 {
                     if (ici.IC_DOB != null)
                     {
                         formattedDOB = errorChecking.convertShortDate(((DateTime)ici.IC_DOB).ToShortDateString());
                     }
                 }
                 catch (Exception exception)
                 {
                     Log.Warn("Suppressing errors.", exception);
                 }

                 if (_ssn != null)
                 {
                     _ssn = AesEncryption.Encrypt(ici.IC_SSN);
                 }
                 else
                 {
                     _ssn = null;
                 }


                EmployeeController.ImportEmployee(batchID, null, null, _employerID, ici.IC_FIRST_NAME, ici.IC_MIDDLE_NAME, ici.IC_LAST_NAME, ici.IC_ADDRESS_I, ici.IC_CITY_I, ici.IC_STATE_I, ici.IC_ZIP_I, null, null, _ssn, null, null, formattedDOB);
            }
        }
    }


    public Boolean InsertNewInsuranceCoverageRow(
            int _taxYear,
            int _employeeID,
            int _employerID,
            int _dependentID,
            Boolean jan,
            Boolean feb,
            Boolean mar,
            Boolean apr,
            Boolean may,
            Boolean jun,
            Boolean jul,
            Boolean aug,
            Boolean sep,
            Boolean oct,
            Boolean nov,
            Boolean dec
        )
    {

        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_new_editable_insurance_coverage", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dependentID", SqlDbType.Int).Value = _dependentID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@jan", SqlDbType.Bit).Value = jan.checkBoolNull();
            cmd.Parameters.AddWithValue("@feb", SqlDbType.Bit).Value = feb.checkBoolNull();
            cmd.Parameters.AddWithValue("@mar", SqlDbType.Bit).Value = mar.checkBoolNull();
            cmd.Parameters.AddWithValue("@apr", SqlDbType.Bit).Value = apr.checkBoolNull();
            cmd.Parameters.AddWithValue("@may", SqlDbType.Bit).Value = may.checkBoolNull();
            cmd.Parameters.AddWithValue("@jun", SqlDbType.Bit).Value = jun.checkBoolNull();
            cmd.Parameters.AddWithValue("@jul", SqlDbType.Bit).Value = jul.checkBoolNull();
            cmd.Parameters.AddWithValue("@aug", SqlDbType.Bit).Value = aug.checkBoolNull();
            cmd.Parameters.AddWithValue("@sep", SqlDbType.Bit).Value = sep.checkBoolNull();
            cmd.Parameters.AddWithValue("@oct", SqlDbType.Bit).Value = oct.checkBoolNull();
            cmd.Parameters.AddWithValue("@nov", SqlDbType.Bit).Value = nov.checkBoolNull();
            cmd.Parameters.AddWithValue("@dec", SqlDbType.Bit).Value = dec.checkBoolNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    public CoveredIndividual InsertCoveredIndividual(
          int taxYear,
          int employeeID,
          int employerID,
          int dependentID,
          Boolean annual_coverage_indicator
      )
    {

        DataTable dtCoveredIndividual = new DataTable();

        using (var conn = new SqlConnection(connString))
        {
            using (var cmd = new SqlCommand("INSERT_covered_individual", conn))
            {

                cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = employeeID;
                cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerID;
                cmd.Parameters.AddWithValue("@dependentID", SqlDbType.Int).Value = dependentID;
                cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYear;
                cmd.Parameters.AddWithValue("@annual_coverage_indicator", SqlDbType.Bit).Value = annual_coverage_indicator;

                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtCoveredIndividual);
                }
            }
        }

        return dtCoveredIndividual.DataTableToObject<CoveredIndividual>();

    }

    public Boolean InsertUpdateCoveredIndividuaMonthlyDetail(
            int coveredIndividualID,
            int employeeID,
            int employerID,
            int taxYear,
            int month,
            Boolean coveredIndicator
        )
    {

        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT_UPDATE_covered_individual_monthly_detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@coveredIndividualID", SqlDbType.Int).Value = coveredIndividualID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@month", SqlDbType.Int).Value = month.checkIntDBNull();
            cmd.Parameters.AddWithValue("@coveredIndicator", SqlDbType.Bit).Value = coveredIndicator.checkBoolNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Exception in InsertUpdateCoveredIndividuaMonthlyDetail.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }

    public bool deleteInsuranceCoverageRow(int _rowid)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_coverage_editable_row", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowid.checkIntDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql == 1)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return validTransaction;
    }
   

    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~insuranceFactory() 
    {
        if (null != rdr && false == rdr.IsClosed) 
        {
            rdr.Close();
        }
        if (null != conn) 
        {
            conn.Dispose();
        }        
    }

    /// <summary>
    /// This will migrate all insurance offers and insurance offer change events related to one employee and move them over to the correct employee.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employerID">Employer ID</param>
    /// <param name="_employeeIDold">Employee ID of the incorrect Employee</param>
    /// <param name="_employeeIDnew">Employee ID of the correct Employee</param>
    /// <returns></returns>
    public bool migrateInsuranceOffers(int _rowID, int _employerID, int _employeeIDold, int _employeeIDnew, string _modBy, DateTime _modOn, string _history)
    {
        bool succesfulTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("MIGRATE_insurance_offers", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDnew", SqlDbType.Int).Value = _employeeIDnew.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDold", SqlDbType.Int).Value = _employeeIDold.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Decimal).Value = _employerID.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkDateDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history.checkForDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }


    /// <summary>
    /// This will delete all insurance offers & Insurance Offer Change Events related to one employee.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employerID">Employer ID</param>
    /// <param name="_employee">Employee ID</param>
    /// <returns></returns>
    public bool deleteInsuranceOffers(int _rowID, int _employerID, int _employeeID)
    {
        bool succesfulTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_offer_single", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Decimal).Value = _employerID.checkDecimalDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }

    /// <summary>
    /// This will delete a single row in the insurance coverage table based on the row id passed in.
    /// </summary>
    /// <param name="_rowID">Record ID</param>
    /// <param name="_employee">Employee ID</param>
    /// <returns></returns>
    public bool deleteInsuranceCoverageSingleRow(int _rowID, int _employeeID)
    {
        bool succesfulTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE_insurance_coverage_row", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }


    /// <summary>
    /// This will transfer a single insurance coverage row over to the correct employee. 
    /// This is used on the merge employee screen. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employeeIDnew"></param>
    /// <param name="_employeeIDold"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public bool updateInsuranceCoverageRow(int _rowID, int _employeeIDnew, int _employeeIDold, string _history)
    {
        bool succesfulTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("MIGRATE_insurance_coverage_row", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDold", SqlDbType.Int).Value = _employeeIDold.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDnew", SqlDbType.Int).Value = _employeeIDnew.checkIntDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }

    /// <summary>
    /// This will transfer all insurance coverage rows for a dependent of an employee over to the correct dependent/employee. 
    /// This is used on the merge employee screen. 
    /// </summary>
    /// <param name="_rowID"></param>
    /// <param name="_employeeIDnew"></param>
    /// <param name="_employeeIDold"></param>
    /// <param name="_history"></param>
    /// <returns></returns>
    public bool updateInsuranceCoverageRowsDependent(int _rowID, int _employeeIDnew, int _employeeIDold, int _dependentIDnew, int _dependentIDold, string _history)
    {
        bool succesfulTransaction = false;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("MIGRATE_insurance_coverage_row_dependent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowID", SqlDbType.Int).Value = _rowID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDold", SqlDbType.Int).Value = _employeeIDold.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeIDnew", SqlDbType.Int).Value = _employeeIDnew.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dependentIDold", SqlDbType.Int).Value = _dependentIDold.checkIntDBNull();
            cmd.Parameters.AddWithValue("@dependentIDnew", SqlDbType.Int).Value = _dependentIDnew.checkIntDBNull();
            cmd.Parameters.AddWithValue("@history", SqlDbType.VarChar).Value = _history;

            int tsql = 0;
            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                succesfulTransaction = true;
            }
            else
            {
                succesfulTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return succesfulTransaction;
    }

    /// <summary>
    /// Builds a list of Waiting Period Objects. 
    /// </summary>
    /// <returns></returns>
    public List<waitingPeriod> manufactureWaitingPeriod()
    {
        List<waitingPeriod> tempList = new List<waitingPeriod>();

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT_Waiting_Periods", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;
                object desc = null;
                object guid = null;
                object modOn = null;
                object modBy = null;
                object createdOn = null;
                object createdBy = null;
                object entityStatus = null;

                id = rdr[0] as object ?? default(object);
                desc = rdr[1] as object ?? default(object);
                guid = rdr[2] as object ?? default(object);
                entityStatus = rdr[3] as object ?? default(object);
                modOn = rdr[4] as object ?? default(object);
                modBy = rdr[5] as object ?? default(object);
                createdOn = rdr[6] as object ?? default(object);
                createdBy = rdr[7] as object ?? default(object);

                int _id = id.checkIntNull();
                string _desc = desc.checkStringNull();
                string _guid = guid.checkStringNull();
                int _entityStatusID = entityStatus.checkIntNull();
                DateTime? _modOn = modOn.checkDateNull();
                string _modBy = modBy.checkStringNull();
                DateTime? _createdOn = createdOn.checkDateNull();
                string _createdBy = createdBy.checkStringNull();

                waitingPeriod wp = new waitingPeriod();
                wp.id = _id;
                wp.description = _desc;
                wp.guid = _guid;
                wp.entityStatusID = _entityStatusID;
                wp.modifiedOn = _modOn;
                wp.modifiedBy = _modBy;
                wp.createdOn = _createdOn;
                wp.createdBy = _createdBy;

                tempList.Add(wp);
            }

            tempList = tempList.OrderBy(sortVal => sortVal.description).ToList();

            return tempList;
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return tempList;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }


    public Boolean BulkImportOffer( DataTable data)
    {

        this.Log.Info("Starting BulkImportOffer.");

        try
        {

            conn = new SqlConnection(connString);
            conn.Open();
            
            SqlCommand cmd = new SqlCommand("InsuranceOfferUpload", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@insuranceOffer", SqlDbType.Structured).Value = data;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {

                this.Log.Info(String.Format("InsuranceOfferUpload returned {0}.", tsql));

                return true;

            }
            else
            {

                this.Log.Warn(String.Format("InsuranceOfferUpload returned {0}.",  tsql));

                return false;

            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Failure to Bulk Import Employees.", exception);

            return false;

        }
        finally
        {

            if (conn != null)
            {
                conn.Close();
            }

        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_batchID"></param>
    /// <returns></returns>
    public bool deleteImportedInsuranceCarrierBatch(int _batchID, string _modBy, int _employerID)
    {
        bool validTransaction = false;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        try
        {
            conn = new SqlConnection(connString);
            conn.Open();

            cmd = new SqlCommand("DELETE_insurance_carrier_import", conn);
            cmd.CommandType = CommandType.StoredProcedure; ;

            cmd.Parameters.AddWithValue("@batchID", SqlDbType.Int).Value = _batchID;
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;

            int tsql = 0;

            tsql = cmd.ExecuteNonQuery();

            if (tsql > 0)
            {
                validTransaction = true;
            }
            else
            {
                validTransaction = false;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (rdr != null)
            {
                rdr.Close();
                rdr.Dispose();
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        return validTransaction;
    }
}