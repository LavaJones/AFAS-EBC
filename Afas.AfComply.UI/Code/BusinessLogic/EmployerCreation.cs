using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using Afas.Domain;

public class EmployerCreation
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployerCreation));

        private List<employer> employers;

        public string CreateEmployer(int? employerTypeID, 
            string employerName, string address, string city,
            string stateName, string zip, string EIN,
            string firstName, string lastName, string email,
            string phoneNumber, string username, string password, string dbaName)
        {
            string results = string.Empty;

            try
            {
                if (employers == null || employers.Count <= 0)
                {
                    employers = employerController.getAllEmployers();
                }

                var existing = (from emp in employers where emp.EMPLOYER_EIN.Equals(EIN) select emp).FirstOrDefault();

                if (existing == null)
                {   

                    Registration re = validateRegistrationData(employerTypeID, employerName, address, city, stateName, zip, EIN,
                        firstName, lastName, email, phoneNumber, username, password, dbaName);

                    if (null != re)
                    {
                        int _employerID = 0;

                        _employerID = employerController.newRegistration(re);

                        if (_employerID > 0)
                        {
                            employer employer = employerController.getEmployer(_employerID);

                            SetImportIds(employer);

                            bool succesfull = employerController.updateEmployerSetup(employer);

                            if (false == succesfull)
                            {
                                Log.Info("Unable to auto generate Import Id's for employer Id:" + _employerID);
                                results += "Failed to Update Data for EmployerId: [" + _employerID + "];\n";
                            }
                            else
                            {
                                Log.Info("Import succeeded for employer Id:" + _employerID);
                            }
                        }
                        else
                        {
                            Log.Info("Problem while saving data to database for EIN: " + re.REG_EIN);
                            results += "Unable to create employer for EIN: [" + re.REG_EIN + "];\n";
                        }
                    }
                    else
                    {
                        Log.Info(string.Format("Validation failed while Importing data [{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}]", 
                            employerName, address, city, stateName, zip, EIN, firstName, 
                            lastName, email, phoneNumber, username, password));
                        results += string.Format("Validation failed while Importing data [{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}]\n",
                            employerName, address, city, stateName, zip, EIN, firstName,
                            lastName, email, phoneNumber, username, password);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info(string.Format("Exception while Importing row [{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}]",
                            employerName, address, city, stateName, zip, EIN, firstName,
                            lastName, email, phoneNumber, username, password), ex);
                results += string.Format("failed to Import row [{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}]\n",
                            employerName, address, city, stateName, zip, EIN, firstName,
                            lastName, email, phoneNumber, username, password);
            }

            return results;
        }

        private void SetImportIds(employer employer)
        {
            employer.EMPLOYER_IMPORT_EMPLOYEE = "Dem" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_EC = "Ec" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_GP = "Gp" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_HR = "Hr" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_IC = "Ic" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_IO = "Io" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_PAY_MOD = "Mod" + employer.EMPLOYER_ID + "_";
            employer.EMPLOYER_IMPORT_PAYROLL = "Pay" + employer.EMPLOYER_ID + "_";

            employer.EMPLOYER_VENDOR_ID = 6;                
        }

        private List<State> states;                

        /// <summary>
        /// Error check all required data. If requirement fails, store data in temp object to relaod for user. 
        /// </summary>
        private Registration validateRegistrationData(int? employerTypeID,
            string employerName, string address, string city, 
            string stateName, string zip, string EIN,
            string firstName, string lastName, string email,
            string phoneNumber, string username, string password, string dbaName)
        {
            bool validData = true;

            int _employerTypeID = employerTypeID ?? 3;    

            string _employerName = employerName.Replace("\"", "");  
            string _employerAdd = address.Replace("\"", "");
            string _employerCity = city.Replace("\"", "");

            if (states == null || states.Count <= 0)
            {
                states = StateController.getStates();
            }
            int _employerState = 0;
            foreach (State state in states)
            {
                if (state.State_Name.ToLower().Equals(stateName.ToLower()) || state.State_Abbr.ToLower().Equals(stateName.ToLower()))
                {
                    _employerState = state.State_ID;
                    break;
                }
            }
            Log.Debug(string.Format("State Name [{0}] State Id [{1}]", stateName, _employerState));


            string _employerZip = zip;
            string _employerEIN = EIN;

            string _renewalDesc = "Default";
            int _renewalDate = 1;
            string _renewalDesc2 = "";
            bool _multiplePlans = false;
            int _renewalDate2 = 0;

            User adminUser = null;
            string _userfname = firstName;
            string _userlname = lastName;
            string _useremail = email;
            _useremail = _useremail.Replace("\"", "");
            if (_useremail.Contains(';'))
            {
             
                _useremail = _useremail.Split(';')[0].Trim();
            }
            if (_useremail.Contains(','))
            {
               
                _useremail = _useremail.Split(',')[0].Trim();
            }
            if (_useremail.Contains(':'))
            {
              
                _useremail = _useremail.Split(':')[0].Trim();
            }

            string _userphone = phoneNumber;
            string _userName = username.ToLower();
            if (_userName == null || _userName.Equals(string.Empty))
            {
               
                _userName = "AFC" + _employerName.Replace(" ", "").Substring(0, 3) + _employerEIN.Substring(_employerEIN.Length - 3, 3);
                _userName = _userName.ToLower();
            }

           
            string _password = password;

            try
            {
                validData = validData && false == _employerName.IsNullOrEmpty();
                validData = validData && false == _employerAdd.IsNullOrEmpty();
                validData = validData && false == _employerCity.IsNullOrEmpty();
                validData = validData && false == _employerZip.IsNullOrEmpty();
                validData = validData && _employerEIN.IsValidFedId();

                validData = validData && false == _userfname.IsNullOrEmpty();
                validData = validData && false == _userlname.IsNullOrEmpty();
                validData = validData && _userphone.IsValidPhoneNumber();
                validData = validData && _useremail.IsValidEmail();
              
            }
            catch (Exception exception)
            {
                Log.Warn("Error During Validation.", exception);
                validData = false;
            }

            if (validData)
            {
       
                adminUser = new User(0, _userfname, _userlname, _useremail, _userphone, _userName, _password, 0, true, true, DateTime.Now, "registration", false, true, false, false);

                Registration roe = new Registration(_employerTypeID, _employerName, _employerEIN, _employerAdd, _employerCity, _employerState, _employerZip, _renewalDesc, _renewalDate, _multiplePlans, _renewalDesc2, _renewalDate2, "", "", 36, "", adminUser, null, dbaName);   

                return roe;
            }
            else
            {
                Log.Info("Validation Failed for EIN: " + _employerEIN);
                return null;
            }
        }

        public string CreateNewClassification(string ein, string description, int? waitingPeriodID, string ooc, string safeHarbor)
        {
            string results = string.Empty;

        try
        {
            if (employers == null || employers.Count <= 0)
            {
                employers = employerController.getAllEmployers();
            }

            var existing = (from emp in employers where emp.EMPLOYER_EIN.Equals(ein) select emp).FirstOrDefault();

            if (existing != null)
            {         
                int _employerID = existing.EMPLOYER_ID;
                DateTime _modOn = DateTime.Now;
                string _modBy = "BulkImport";
                string _history = null;
                bool validTransaction = false;
                if (false == description.IsNullOrEmpty())
                {
                    _history = "Created on " + _modOn.ToString() + " by " + _modBy + System.Environment.NewLine + description;
                    _history += Environment.NewLine + "Code: " + safeHarbor;
                    validTransaction = classificationController.ManufactureEmployeeClassification(_employerID, description, safeHarbor, _modOn, _modBy, _history, waitingPeriodID, ooc);

                    if (false == validTransaction)
                    {
                        Log.Info("Import Employee Classification Failed: EIN: " + ein + " ASH Code: " + safeHarbor + " Description: " + description);
                        results += "Failed Classification Insert: EIN: [" + ein + "] ASH Code: [" + safeHarbor + "] Description: [" + description + "];\n";
                    }
                }
                else
                {
                    Log.Info("Validation Failed Employee Classification: EIN: " + ein + " ASH Code: " + safeHarbor + " Description: " + description);
                    results += "Validation Failed Classification: EIN: [" + ein + "] ASH Code: [" + safeHarbor + "] Description: [" + description + "];\n";
                }
            }
            else
            {
                Log.Info("Tried to import Employee Classification for employer that didn't exist with EIN: " + ein);
                results += "No Employer found with EIN: [" + ein + "];\n";
            }
        }
        catch (Exception ex)
        {
            Log.Info(string.Format("Exception while Importing row [{0}, {1}, {2}]", ein, description, safeHarbor), ex);
            results += string.Format("failed to Import row [{0}, {1}, {2}];\n", ein, description, safeHarbor);
        }

        return results;
        }
    }
