using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using log4net;
using Afas.AfComply.Domain;
using System.Web.UI;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for airFactory
/// </summary>
public class airFactory
{
    private ILog Log = LogManager.GetLogger(typeof(airFactory));

    private static string connStringACA = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;
    private static string connStringAir = System.Configuration.ConfigurationManager.ConnectionStrings["AIR_Conn"].ConnectionString;
    private SqlConnection conn = null;
    private SqlDataReader rdr = null;

    /// <summary>
    /// AIR - Returns the Offer Of Coverage Codes.
    /// </summary>
    public List<ooc> ManufactureOOCList()
    {

        List<ooc> tempList = new List<ooc>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_mec_codes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object id = 0;
                object desc = null;

                id = rdr[0] as object ?? default(object);
                desc = rdr[1] as object ?? default(object);

                string _id = id.checkStringNull();
                string _desc = desc.checkStringNull();

                ooc newOOC = new ooc(_id, _desc);
                tempList.Add(newOOC);

            }

            return tempList;

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            return new List<ooc>();

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

    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public List<status> ManufactureStatusList()
    {

        List<status> tempList = new List<status>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_status_codes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object id = 0;
                object desc = null;

                id = rdr[0] as object ?? default(object);
                desc = rdr[1] as object ?? default(object);

                int _id = id.checkIntNull();
                string _desc = desc.checkStringNull();

                status newStatus = new status(_id, _desc);
                tempList.Add(newStatus);

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

    /// <summary>
    /// AIR - Return the list of the Affordable Safe Harbor codes from the database.
    /// </summary>
    public List<ash> ManufactureASHList()
    {

        List<ash> tempList = new List<ash>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_4980H_codes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object id = 0;
                object desc = null;

                id = rdr[0] as object ?? default(object);
                desc = rdr[1] as object ?? default(object);

                String _id = id.checkStringNull();
                String _desc = desc.checkStringNull();

                ash newOOC = new ash(_id, _desc);
                tempList.Add(newOOC);

            }

            return tempList;

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            return new List<ash>();

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


    /// <summary>
    /// AIR - Connect to AIR Database and pull time frames based on a year.
    /// </summary>
    /// <returns></returns>
    public List<int> manufactureTimeFrameList(int _taxYear)
    {
        List<int> tempList = new List<int>();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_Time_Frame_Months", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = 0;

                id = rdr[0] as object ?? default(object);

                int _id = id.checkIntNull();

                tempList.Add(_id);
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

    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public List<monthlyDetail> ManufactureEmployeeMonthlyDetailList(int _employeeID)
    {

        List<monthlyDetail> tempList = new List<monthlyDetail>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_monthly_detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object employeeID = null;
                object timeFrameID = null;
                object employerID = null;
                object hours = null;
                object ooc = null;
                object mec = null;
                object lcmp = null;
                object ashCode = null;
                object enrolled = null;
                object monthlyStatusID = null;
                object insuranceTypeID = null;
                object modBy = null;
                object modOn = null;
                object finalized = null;

                employeeID = rdr[0] as object ?? default(object);
                timeFrameID = rdr[1] as object ?? default(object);
                employerID = rdr[2] as object ?? default(object);
                hours = rdr[3] as object ?? default(object);
                ooc = rdr[4] as object ?? default(object);
                mec = rdr[5] as object ?? default(object);
                lcmp = rdr[6] as object ?? default(object);
                ashCode = rdr[7] as object ?? default(object);
                enrolled = rdr[8] as object ?? default(object);
                monthlyStatusID = rdr[9] as object ?? default(object);
                insuranceTypeID = rdr[10] as object ?? default(object);
                modOn = rdr[12] as object ?? default(object);
                modBy = rdr[13] as object ?? default(object);
                int _employeeID2 = employeeID.checkIntNull();
                int _timeFrameID = timeFrameID.checkIntNull();
                int _employerID = employerID.checkIntNull();
                decimal _hours = hours.checkDecimalNull();
                string _ooc = ooc.checkStringNull();
                bool? _mec = mec.checkBoolNull2();
                decimal? _lcmp = lcmp.checkDecimalNull2();
                string _ashCode = ashCode.checkStringNull();
                bool _enrolled = enrolled.checkBoolNull();
                int _monthlyStatusID = monthlyStatusID.checkIntNull();
                int _insuranceTypeID = insuranceTypeID.checkIntNull();
                string _modBy = modBy.checkStringNull();
                DateTime? _modOn = modOn.checkDateNull();
                bool _finalized = finalized.checkBoolNull();

                monthlyDetail md = new monthlyDetail(_employeeID2, _timeFrameID, _employerID, _hours, _ooc, _mec, _lcmp, _ashCode, _enrolled, _monthlyStatusID, _insuranceTypeID, _modBy, _modOn, _finalized);

                tempList.Add(md);

            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<monthlyDetail>();

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

    public int manufactureEmployer1094CorrectionId(int _employerID, int _year)
    {
        int number = 0;
        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("dbo.SELECT_1094CCorrectionCount", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _year;

            return (int)cmd.ExecuteScalar();


        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        return number;
    }

    public int manufactureEmployerCorrectionId(int _employerID, int _year)
    {
        int number = 0;
        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("dbo.SELECT_CorrectionCount", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _year;

            return (int)cmd.ExecuteScalar();


        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
        return number;
    }

    /// <summary>
    /// Recheck the 1095 status for a particular employee.
    /// </summary>
    public Boolean Recalculate1095Status(int employerId, int employeeId, int taxYear)
    {

        Boolean validTransaction = false;

        try
        {

            conn = new SqlConnection(connStringAir);

            conn.Open();

            SqlCommand cmd = new SqlCommand("[appr].[spUpdate_1095C_status_for_employee]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 240;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = employerId.checkIntDBNull();
            cmd.Parameters.AddWithValue("@year_id", SqlDbType.Int).Value = taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = employeeId.checkIntDBNull();

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
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

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return validTransaction;

    }

    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public Boolean UpdateEmployeeMonthlyDetailList(
            int _employeeID,
            int _timeFrameID,
            int _employerID,
            String _ooc,
            Decimal? _lcmp,
            String _ash,
            String _modBy,
            DateTime _modOn,
            Decimal _hours,
            Boolean _enrolled,
            Boolean? _mec,
            int _monthlyStatusID,
            int _insuranceTypeID,
            Boolean _corrected
        )
    {

        Boolean validTransaction = false;

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_UPDATE_approved_monthly_detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@timeFrameID", SqlDbType.Int).Value = _timeFrameID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@hours", SqlDbType.Decimal).Value = _hours.checkDecimalDBNull();
            cmd.Parameters.AddWithValue("@ooc", SqlDbType.Int).Value = _ooc.checkForDBNull();
            cmd.Parameters.AddWithValue("@mec", SqlDbType.Bit).Value = _mec.checkBoolDBNull();
            cmd.Parameters.AddWithValue("@lcmp", SqlDbType.Decimal).Value = _lcmp.checkDecimalDBNull2();
            cmd.Parameters.AddWithValue("@ash", SqlDbType.Int).Value = _ash.checkForDBNull();
            cmd.Parameters.AddWithValue("@enrolled", SqlDbType.Bit).Value = _enrolled.checkBoolNull();
            cmd.Parameters.AddWithValue("@monthlyStatusID", SqlDbType.Int).Value = _monthlyStatusID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@insuranceTypeID", SqlDbType.Int).Value = _insuranceTypeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();

            int rows = cmd.ExecuteNonQuery();
            if (_corrected)
            {
                validTransaction = true;
            }
            else if (rows > 0)
            {
                validTransaction = true;
            }
            else
            {
                Log.Warn(
                    String.Format(@"Failed to Update Approved Monthly Detail for Employee Id: [{0}], 
Time Frame Id: [{1}],
Employer Id: [{2}],
Hours: [{3}],
OOC: [{4}],
MEC: [{5}],
LCMP: [{6}],
ASH: [{7}],
Enrolled: [{8}],
Monthly Status Id: [{9}],
Insurnace Type Id: [{10}],
Mod By: [{11}],
Mod On: [{12}].",
                _employeeID,
                _timeFrameID,
                _employerID,
                _hours,
                _ooc,
                _mec,
                _lcmp,
                _ash,
                _enrolled,
                _monthlyStatusID,
                _insuranceTypeID,
                _modBy,
                _modOn));

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

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return validTransaction;

    }

    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    public void ApproveEmployeeMonthlyDetail(int _employeeID, int _timeFrameID, string _modBy, DateTime _modOn)
    {

        List<monthlyDetail> tempList = new List<monthlyDetail>();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_UPDATE_approve_monthly_detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@timeFrameID", SqlDbType.Int).Value = _timeFrameID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@modBy", SqlDbType.VarChar).Value = _modBy.checkForDBNull();
            cmd.Parameters.AddWithValue("@modOn", SqlDbType.DateTime).Value = _modOn.checkForDBNull();

            int rows = cmd.ExecuteNonQuery();

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<monthlyDetail>();

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

    /// <summary>
    /// Pulls a list of Employee ID's that are flagged to get 1095C forms.
    /// </summary>
    public List<int> GetEmployeesReceiving1095(int _employerID, int _taxYear)
    {

        List<int> tempList = new List<int>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_employer_employees_in_yearly_detail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                object id = rdr[0] as object ?? default(object);
                int _id = id.checkIntNull();

                tempList.Add(_id);
            }

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<int>();

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

    /// <summary>
    /// Pulls a list of part III coverage (aka carrier file imports) for an employee
    /// </summary>
    public List<airCoverage> GetAirCoverage(int _employeeID)
    {

        List<airCoverage> tempList = new List<airCoverage>();

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_employee_LINE3_coverage", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID;

            rdr = cmd.ExecuteReader();

            tempList = manufactureAirCoverageList(rdr);

        }
        catch (Exception exception)
        {

            Log.Warn("Suppressing errors.", exception);

            tempList = new List<airCoverage>();

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

    /// <summary>
    /// This will generate the list of Alert_Insurance objects from the rdr object. 
    /// </summary>
    /// <returns></returns>
    private List<airCoverage> manufactureAirCoverageList(SqlDataReader rdr)
    {

        List<airCoverage> airCiList = new List<airCoverage>();

        while (rdr.Read())
        {
            airCoverage airCi = null;
            object employeeID = null;
            object fname = null;
            object lname = null;
            object ssn = null;
            object dob = null;
            object covIndID = null;
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

            employeeID = rdr[0] as object ?? default(object);
            fname = rdr[1] as object ?? default(object);
            lname = rdr[2] as object ?? default(object);
            ssn = rdr[3] as object ?? default(object);
            dob = rdr[4] as object ?? default(object);
            covIndID = rdr[5] as object ?? default(object);
            jan = rdr[6] as object ?? default(object);
            feb = rdr[7] as object ?? default(object);
            mar = rdr[8] as object ?? default(object);
            apr = rdr[9] as object ?? default(object);
            may = rdr[10] as object ?? default(object);
            jun = rdr[11] as object ?? default(object);
            jul = rdr[12] as object ?? default(object);
            aug = rdr[13] as object ?? default(object);
            sep = rdr[14] as object ?? default(object);
            oct = rdr[15] as object ?? default(object);
            nov = rdr[16] as object ?? default(object);
            dec = rdr[17] as object ?? default(object);

            int _employeeID = employeeID.checkIntNull();
            string _fname = fname.checkStringNull();
            string _lname = lname.checkStringNull();
            string _ssn = ssn.checkStringNull();
            DateTime? _dob = dob.checkDateNull();
            int _coverdIndID = covIndID.checkIntNull();
            bool _jan = jan.checkBoolStringNull();
            bool _feb = feb.checkBoolStringNull();
            bool _mar = mar.checkBoolStringNull();
            bool _apr = apr.checkBoolStringNull();
            bool _may = may.checkBoolStringNull();
            bool _jun = jun.checkBoolStringNull();
            bool _jul = jul.checkBoolStringNull();
            bool _aug = aug.checkBoolStringNull();
            bool _sep = sep.checkBoolStringNull();
            bool _oct = oct.checkBoolStringNull();
            bool _nov = nov.checkBoolStringNull();
            bool _dec = dec.checkBoolStringNull();

            if (_ssn != null)
            {
                _ssn = AesEncryption.Decrypt(_ssn);
            }

            airCi = new airCoverage(_coverdIndID, _employeeID, _fname, _lname, _ssn, _dob, _jan, _feb, _mar, _apr, _may, _jun, _jul, _aug, _sep, _oct, _nov, _dec);

            airCiList.Add(airCi);
        }

        return airCiList;

    }

    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public Boolean UpdateIndividualCoverage(int _individualCoverageID, string _fname, string _lname, string _ssn, DateTime? _dob)
    {

        bool validTransaction = false;

        try
        {

            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_UPDATE_covered_individual", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (_ssn != null)
            {
                _ssn = AesEncryption.Encrypt(_ssn);
            }

            cmd.Parameters.AddWithValue("@covIndID", SqlDbType.Int).Value = _individualCoverageID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@first_name", SqlDbType.Int).Value = _fname.checkForDBNull();
            cmd.Parameters.AddWithValue("@last_name", SqlDbType.Int).Value = _lname.checkForDBNull();
            cmd.Parameters.AddWithValue("@ssn", SqlDbType.Decimal).Value = _ssn.checkForDBNull();
            cmd.Parameters.AddWithValue("@dob", SqlDbType.Int).Value = _dob.checkDateDBNull();

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
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

            Log.Warn("Exception in UpdateIndividualCoverag.", exception);

            validTransaction = false;

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

        return validTransaction;

    }


    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public bool runETL_ShortBuild(int _employerID, int _employeeID, int _taxYear)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("sp_AIR_ETL_ShortBuild", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 240;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = _employeeID.checkIntDBNull();

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
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
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return validTransaction;
    }


    /// <summary>
    /// AIR - Connect to AIR Database.
    /// </summary>
    /// <returns></returns>
    public bool runETL_Build_MissingEmployee(int _employerID, int _employeeID, int _taxYear)
    {
        bool validTransaction = false;

        try
        {
            conn = new SqlConnection(connStringAir);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[etl].[spETL_Build_MissingEmployee]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 240;
            cmd.Parameters.AddWithValue("@employer_id", SqlDbType.Int).Value = _employerID.checkIntDBNull();
            cmd.Parameters.AddWithValue("@year_id", SqlDbType.Int).Value = _taxYear.checkIntDBNull();
            cmd.Parameters.AddWithValue("@employee_id", SqlDbType.Int).Value = _employeeID.checkIntDBNull();

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
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
            if (rdr != null)
            {
                rdr.Close();
            }
        }

        return validTransaction;
    }

    /// <summary>
    /// Removed the covered individual  from the Part III coverage.
    /// </summary>
    /// <returns></returns>
    public Boolean AddCoveredIndividual(int rowId)
    {

        Boolean validTransaction = false;

        try
        {

            conn = new SqlConnection(connStringAir);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[dbo].[spInsertNewCoveredIndividual]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowId", SqlDbType.Int).Value = rowId.checkIntDBNull();

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
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

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return validTransaction;

    }

    /// <summary>
    /// Removed the covered individual  from the Part III coverage.
    /// </summary>
    /// <returns></returns>
    public Boolean RemoveCoveredIndividual(int rowId)
    {

        Boolean validTransaction = false;

        try
        {

            conn = new SqlConnection(connStringAir);
            conn.Open();

            SqlCommand cmd = new SqlCommand("[appr].[spDeleteCoveredIndividual]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rowId", SqlDbType.Int).Value = rowId.checkIntDBNull();

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
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

            if (rdr != null)
            {
                rdr.Close();
            }

        }

        return validTransaction;

    }

    /// <summary>
    /// Distructor to be sure all resources are released
    /// </summary>
    ~airFactory()
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
    /// 2/17/2017: This will validate that an employee exists in the AIR tables for the selected Tax Year. 
    /// </summary>
    public Boolean validateEmployeeTaxYearInAIR(int employeeId, int taxYear)
    {
        Boolean validTransaction = false;
        try
        {
            conn = new SqlConnection(connStringAir);
            conn.Open();

            SqlCommand cmd = new SqlCommand("spSelect_does_employee_exist_taxyear", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 240;
            cmd.Parameters.AddWithValue("@employeeID", SqlDbType.Int).Value = employeeId.checkIntDBNull();
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = taxYear.checkIntDBNull();

            cmd.Parameters.Add("@rowCount", SqlDbType.Int);
            cmd.Parameters["@rowCount"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            int rows = (int)cmd.Parameters["@rowCount"].Value;

            if (rows == 1) { validTransaction = true; }
            else { validTransaction = false; }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validTransaction = false;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
            if (rdr != null) { rdr.Close(); }
        }

        return validTransaction;
    }


    /// <summary>
    /// 7/20/2017: New, this will need to be converted over to Entity Framework Select. 
    /// Create a list of taxYearEmployerTransmission Objects for a specific employer and tax year.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_year"></param>
    /// <returns></returns>
    public List<taxYearEmployerTransmission> manufactureEmployerTransmissions(int _employerID, int _year)
    {
        List<taxYearEmployerTransmission> tempList = new List<taxYearEmployerTransmission>();
        NullHelper nh = new NullHelper();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_tax_year_submissions_employer", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employerID", SqlDbType.Int).Value = _employerID;
            cmd.Parameters.AddWithValue("@taxYear", SqlDbType.Int).Value = _year;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object tyetID = rdr[0] as object ?? default(object);
                object taxyear = rdr[1] as object ?? default(object);
                object employerID = rdr[2] as object ?? default(object);
                object resID = rdr[3] as object ?? default(object);
                object tt = rdr[4] as object ?? default(object);
                object utID = rdr[5] as object ?? default(object);
                object receiptID = rdr[6] as object ?? default(object);
                object usID = rdr[7] as object ?? default(object);
                object tscID = rdr[8] as object ?? default(object);
                object originalReceiptID = rdr[9] as object ?? default(object);
                object originalusID = rdr[10] as object ?? default(object);
                object entityStatusID = rdr[11] as object ?? default(object);
                object createdBy = rdr[12] as object ?? default(object);
                object createdOn = rdr[13] as object ?? default(object);
                object modBy = rdr[14] as object ?? default(object);
                object modOn = rdr[15] as object ?? default(object);
                object bulk10941095filename = rdr[16] as object ?? default(object);
                object manifestFilename = rdr[17] as object ?? default(object);
                object ackFilename = rdr[18] as object ?? default(object);
                object form_count = rdr[19] as object ?? default(object);


                int _tyetID = nh.checkIntNull(tyetID);
                int _taxyear = nh.checkIntNull(taxyear);
                int _employerID2 = nh.checkIntNull(employerID);
                Guid _resID = (Guid)resID;
                string _tt = nh.checkStringNull(tt);
                string _utID = nh.checkStringNull(utID);
                string _receiptID = nh.checkStringNull(receiptID);
                string _usID = nh.checkStringNull(usID);
                int _tscID = nh.checkIntNull(tscID);
                string _originalReceiptID = nh.checkStringNull(originalReceiptID);
                string _originalusID = nh.checkStringNull(originalusID);
                int _entityStatusID = nh.checkIntNull(entityStatusID);
                string _createdBy = nh.checkStringNull(createdBy);
                DateTime? _createdOn = nh.checkDateNull(createdOn);
                string _modBy = nh.checkStringNull(modBy);
                DateTime? _modOn = nh.checkDateNull(modOn);
                string _bulk10941095filename = nh.checkStringNull(bulk10941095filename);
                string _manifestFilename = nh.checkStringNull(manifestFilename);
                string _ackFilename = nh.checkStringNull(ackFilename);
                int _formCount = nh.checkIntNull(form_count);


                taxYearEmployerTransmission tyet = new taxYearEmployerTransmission()
                {
                    tax_year_employer_transmissionID = _tyetID,
                    tax_year = _taxyear,
                    employee_id = 0,
                    employer_id = _employerID2,
                    ResourceId = _resID,
                    TransmissionType = _tt,
                    UniqueTransmissionId = _utID,
                    ReceiptID = _receiptID,
                    UniqueSubmissionId = _usID,
                    RecordID = 0,
                    transmission_status_code_id = _tscID,
                    OriginalReceiptId = _originalReceiptID,
                    OriginalUniqueSubmissionId = _originalusID,
                    EntityStatusId = _entityStatusID,
                    createdBy = _createdBy,
                    createdOn = (DateTime)_createdOn,
                    ModifiedBy = _modBy,
                    ModifiedDate = (DateTime)_modOn,
                    BulkFile = _bulk10941095filename,
                    manifestFile = _manifestFilename,
                    AckFile = _ackFilename,
                    FormCount = _formCount
                };

                tempList.Add(tyet);
            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            tempList = new List<taxYearEmployerTransmission>();
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


    /// <summary>
    /// 7/20/2017: New, this will need to be converted over to Entity Framework Select. 
    /// Create a list of taxYearEmployeeTransmission Objects for a specific employee and tax year.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_year"></param>
    /// <returns></returns>
    public List<taxYearEmployeeTransmission> manufactureEmployeeTransmissions(int _transmissionID)
    {
        List<taxYearEmployeeTransmission> tempList = new List<taxYearEmployeeTransmission>();
        NullHelper nh = new NullHelper();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_tax_year_submission_errant_employees", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@transmissionID", SqlDbType.Int).Value = _transmissionID;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object tyetID = rdr[0] as object ?? default(object);
                object tyetID2 = rdr[1] as object ?? default(object);
                object taxyear = rdr[2] as object ?? default(object);
                object employeeID = rdr[3] as object ?? default(object);
                object employerID = rdr[4] as object ?? default(object);
                object resID = rdr[5] as object ?? default(object);
                object recordID = rdr[6] as object ?? default(object);
                object tscID = rdr[7] as object ?? default(object);
                object entityStatusID = rdr[8] as object ?? default(object);
                object createdBy = rdr[9] as object ?? default(object);
                object createdOn = rdr[10] as object ?? default(object);
                object modBy = rdr[11] as object ?? default(object);
                object modOn = rdr[12] as object ?? default(object);

                int _tyetID = nh.checkIntNull(tyetID);
                int _tyetID2 = nh.checkIntNull(tyetID2);
                int _taxyear = nh.checkIntNull(taxyear);
                int _employeeID = nh.checkIntNull(employeeID);
                int _employerID = nh.checkIntNull(employerID);
                Guid _resID = (Guid)resID;
                int _recordID = nh.checkIntNull(recordID);
                int _tscID = nh.checkIntNull(tscID);
                int _entityStatusID = nh.checkIntNull(entityStatusID);
                string _createdBy = nh.checkStringNull(createdBy);
                DateTime? _createdOn = nh.checkDateNull(createdOn);
                string _modBy = nh.checkStringNull(modBy);
                DateTime? _modOn = nh.checkDateNull(modOn);

                taxYearEmployeeTransmission tyet = new taxYearEmployeeTransmission()
                {
                    tax_year_employee_transmissionID = _tyetID,
                    tax_year_employer_transmissionID = _tyetID2,
                    tax_year = _taxyear,
                    employee_id = _employeeID,
                    employer_id = _employerID,
                    ResourceId = _resID,
                    RecordID = _recordID,
                    transmission_status_code_id = _tscID,
                    EntityStatusId = _entityStatusID,
                    createdBy = _createdBy,
                    createdOn = (DateTime)_createdOn,
                    ModifiedBy = _modBy,
                    ModifiedDate = (DateTime)_modOn
                };

                tempList.Add(tyet);
            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            tempList = new List<taxYearEmployeeTransmission>();
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

    /// <summary>
    /// 7/20/2017: New, this will need to be converted over to Entity Framework Select. 
    /// Create a list of taxYearEmployeeTransmission Objects for a specific employee and tax year.
    /// </summary>
    /// <param name="_employerID"></param>
    /// <param name="_year"></param>
    /// <returns></returns>
    public List<taxYearEmployeeTransmission> manufactureAllEmployeeTransmissions(int _transmissionID)
    {
        List<taxYearEmployeeTransmission> tempList = new List<taxYearEmployeeTransmission>();
        NullHelper nh = new NullHelper();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT_tax_year_submission_all_employees", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@transmissionID", SqlDbType.Int).Value = _transmissionID;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object tyetID = rdr[0] as object ?? default(object);
                object tyetID2 = rdr[1] as object ?? default(object);
                object taxyear = rdr[2] as object ?? default(object);
                object employeeID = rdr[3] as object ?? default(object);
                object employerID = rdr[4] as object ?? default(object);
                object resID = rdr[5] as object ?? default(object);
                object recordID = rdr[6] as object ?? default(object);
                object tscID = rdr[7] as object ?? default(object);
                object entityStatusID = rdr[8] as object ?? default(object);
                object createdBy = rdr[9] as object ?? default(object);
                object createdOn = rdr[10] as object ?? default(object);
                object modBy = rdr[11] as object ?? default(object);
                object modOn = rdr[12] as object ?? default(object);

                int _tyetID = nh.checkIntNull(tyetID);
                int _tyetID2 = nh.checkIntNull(tyetID2);
                int _taxyear = nh.checkIntNull(taxyear);
                int _employeeID = nh.checkIntNull(employeeID);
                int _employerID = nh.checkIntNull(employerID);
                Guid _resID = (Guid)resID;
                int _recordID = nh.checkIntNull(recordID);
                int _tscID = nh.checkIntNull(tscID);
                int _entityStatusID = nh.checkIntNull(entityStatusID);
                string _createdBy = nh.checkStringNull(createdBy);
                DateTime? _createdOn = nh.checkDateNull(createdOn);
                string _modBy = nh.checkStringNull(modBy);
                DateTime? _modOn = nh.checkDateNull(modOn);

                taxYearEmployeeTransmission tyet = new taxYearEmployeeTransmission()
                {
                    tax_year_employee_transmissionID = _tyetID,
                    tax_year_employer_transmissionID = _tyetID2,
                    tax_year = _taxyear,
                    employee_id = _employeeID,
                    employer_id = _employerID,
                    ResourceId = _resID,
                    RecordID = _recordID,
                    transmission_status_code_id = _tscID,
                    EntityStatusId = _entityStatusID,
                    createdBy = _createdBy,
                    createdOn = (DateTime)_createdOn,
                    ModifiedBy = _modBy,
                    ModifiedDate = (DateTime)_modOn
                };

                tempList.Add(tyet);
            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            tempList = new List<taxYearEmployeeTransmission>();
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




    /// 7-20-2017: This now pulls the Submission Status Codes from the ACA database.
    /// </summary>
    /// <returns></returns>
    public List<airSubmissionStatus> manufactureSubmissionStatuses()
    {
        List<airSubmissionStatus> tempList = new List<airSubmissionStatus>();

        try
        {
            conn = new SqlConnection(connStringACA);
            conn.Open();

            NullHelper nh = new NullHelper();

            SqlCommand cmd = new SqlCommand("SELECT_submission_status_codes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object id = null;
                object name = null;

                id = rdr[0] as object ?? default(object);
                name = rdr[1] as object ?? default(object);

                int _id = nh.checkIntNull(id);
                string _name = nh.checkStringNull(name);

                airSubmissionStatus airss = new airSubmissionStatus(_id, _name);
                tempList.Add(airss);
            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            tempList = new List<airSubmissionStatus>();
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


    /// 7-21-2017: Process the Ack file to display error code and message.
    /// </summary>
    /// <returns></returns>
    public List<airRequestError> manufactureAckErrorFiles(airStatusRequest asr, int _employerID, string _uniqueTransmissionID)
    {
        List<airRequestError> tempList = new List<airRequestError>();
        List<Employee> employeeList = EmployeeController.manufactureEmployeeList(_employerID);

        string breakpoint = null;
        try
        {
            int errorID = 0;
            string uniqueSubmissionID = null;

            int _1095cLineError = 0;
            string errorCode = null;
            string errorText = null;
            string xPath = null;

            string xmlString = asr.SR_ACK_XML;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    uniqueSubmissionID = null;
                    errorText = null;
                    errorCode = null;
                    xPath = null;

                    if (node2.Name == "TransmitterErrorDetailGrp")
                    {
                        bool usID = false;
                        bool urID = false;
                        bool emD = false;

                        string originalUniqueSubmissionId = null;
                        string correctedUniqueSubmissionId = null;
                        string correctedUniqueRecordId = null;

                        foreach (XmlNode node4 in node2.ChildNodes)
                        {
                            switch (node4.Name)
                            {
                                case "UniqueSubmissionId":
                                    uniqueSubmissionID = node4.InnerText;
                                    originalUniqueSubmissionId = node4.InnerText;
                                    string[] subID = uniqueSubmissionID.Split('|');
                                    errorID = int.Parse(subID[1]);
                                    usID = true;
                                    foreach (XmlNode node5 in node4.ChildNodes)
                                    {
                                        switch (node5.Name)
                                        {
                                            case "ns2:ErrorMessageCd":
                                                errorCode = node5.InnerText;
                                                break;
                                            case "ns2:ErrorMessageTxt":
                                                errorText = node5.InnerText;
                                                break;
                                            case "ns2:SubmissionLevelStatusCd":
                                                break;
                                        }
                                    }
                                    break;
                                case "SubmissionLevelStatusCd":
                                    errorText = node4.InnerText;
                                    break;
                                case "UniqueRecordId":
                                    uniqueSubmissionID = node4.InnerText;
                                    correctedUniqueRecordId = node4.InnerText;
                                    string[] subID2 = uniqueSubmissionID.Split('|');
                                    originalUniqueSubmissionId = subID2[0] + "|" + subID2[1];
                                    var correctedSubmissionId = int.Parse(subID2[1]) + 1;
                                    correctedUniqueSubmissionId = subID2[0] + "|" + correctedSubmissionId.ToString();
                                    errorID = int.Parse(subID2[2]);
                                    urID = true;
                                    break;
                                case "ns2:ErrorMessageDetail":
                                    foreach (XmlNode node5 in node4.ChildNodes)
                                    {
                                        switch (node5.Name)
                                        {
                                            case "ns2:ErrorMessageCd":
                                                errorCode = node5.InnerText;
                                                break;
                                            case "ns2:ErrorMessageTxt":
                                                errorText = node5.InnerText;
                                                break;
                                            case "":
                                                xPath = "n/a";
                                                break;
                                        }
                                        emD = true;
                                    }
                                    break;
                            }
                        }
                        if (usID == true || urID == false)
                        {
                            airRequestError temp = new airRequestError(errorID, "test", errorID, errorID, errorCode, errorText);
                            temp.RE_ERROR_FIRST_NAME = "Employer";
                            temp.RE_ERROR_LAST_NAME = "Specific";
                            tempList.Add(temp);
                        }

                        if (urID == true && emD == true)
                        {
                            airRequestError temp = new airRequestError(errorID, "test", errorID, errorID, errorCode, errorText, originalUniqueSubmissionId, correctedUniqueSubmissionId, correctedUniqueRecordId);

                            int employeeID = getRecordID(_uniqueTransmissionID, errorID);
                            if (employeeID == 0)
                            {
                                temp.RE_ERROR_FIRST_NAME = "No Match";
                                temp.RE_ERROR_LAST_NAME = "No Match";
                            }
                            else
                            {
                                Employee emp = EmployeeController.findEmployee(employeeList, employeeID);
                                temp.RE_ERROR_FIRST_NAME = emp.EMPLOYEE_FIRST_NAME;
                                temp.RE_ERROR_LAST_NAME = emp.EMPLOYEE_LAST_NAME;
                                temp.EMPLOYEE_ID = employeeID;
                                temp.EMPLOYER_ID = _employerID;
                            }

                            if (EmployeeController.CheckIfTaxYear1095cCorrectionRecord(employeeID, 2016) == false)
                            {
                                tempList.Add(temp);
                            }
                        }
                    }
                }
            }
            return tempList;
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            return tempList;
        }
    }


    /// <summary>
    /// Get the Employee ID for the Error Code in Transmission. 
    /// </summary>
    /// <param name="_uniqueTransmissionID"></param>
    /// <param name="recordID"></param>
    /// <returns></returns>
    private int getRecordID(string _uniqueTransmissionID, int _recordID)
    {
        int employeeID = 0;

        try
        {
            conn = new SqlConnection(connStringAir);
            conn.Open();

            NullHelper nh = new NullHelper();

            SqlCommand cmd = new SqlCommand("sp_AIR_SELECT_employeeID_status_error", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@utID", SqlDbType.VarChar).Value = nh.checkForDBNull(_uniqueTransmissionID);
            cmd.Parameters.AddWithValue("@recordID", SqlDbType.Int).Value = nh.checkIntDBNull(_recordID);

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object empID = null;
                empID = rdr[0] as object ?? default(object);
                employeeID = nh.checkIntNull(empID);
            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            employeeID = 0;
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

        return employeeID;
    }



    public string generateIrsSubmissionErrorFile(List<taxYearEmployeeTransmission> _tempList, employer _employer, int etytID)
    {
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        string fullFilePath = null;
        int _employerID2 = _employer.EMPLOYER_ID;
        List<Employee> employeeList = EmployeeController.manufactureEmployeeList(_employerID2);

        fileName = "ACT_ERROR_EXPORT_" + _employer.EMPLOYER_ID.ToString() + "_" + currDate + ".csv";
        fullFilePath = HttpContext.Current.Server.MapPath("..\\ftps\\export\\") + fileName;
        try
        {
            using (StreamWriter sw = File.CreateText(fullFilePath))
            {
                sw.WriteLine("Row ID, First Name, Last Name, Error Code, Error Message");

                foreach (taxYearEmployeeTransmission tyet in _tempList)
                {
                    Employee currEmp = EmployeeController.findEmployee(employeeList, tyet.employee_id);
                    string[] messages = null;
                    string message1 = "n/a";
                    string message2 = "n/a";
                    AckFileXmlReader ackFile = new AckFileXmlReader();
                    string errorCodeMessage = ackFile.findEmployeeErrorMessageInXML(tyet.RecordID, tyet.employer_id, tyet.tax_year, etytID);
                    if (errorCodeMessage != null)
                    {
                        messages = errorCodeMessage.Split('|');
                        message1 = messages[0];
                        message2 = messages[1];
                    }

                    string line = tyet.RecordID + ",";
                    line += currEmp.EMPLOYEE_FIRST_NAME + ",";
                    line += currEmp.EMPLOYEE_LAST_NAME + ",";
                    line += message1 + ",";
                    line += message2;
                    sw.WriteLine(line);
                }

            }
        }
        catch (Exception ex)
        {
            this.Log.Error(string.Format("Exception ex {0}", ex.Message));
            fileName = null;
        }

        return fileName;
    }


    /// 
    ///              
    /// 
    /// 


}