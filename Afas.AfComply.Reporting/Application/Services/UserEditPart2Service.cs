using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afas.AfComply.Reporting.Application.Services
{

    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public class UserEditPart2Service : ABaseCrudService<UserEditPart2>, IUserEditPart2Service
    {
        protected IUserEditPart2Repository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="repository">The Repository to get the .</param>
        public UserEditPart2Service(
            IUserEditPart2Repository repository) :
                base(repository)
        {

            this.Repository = repository;

        }

        /// <summary>
        /// Updates the database with the user Edits provided
        /// </summary>
        /// <param name="edits">The User Edits to store.</param>
        /// <param name="currentSystem">The current values in the system.</param>
        /// <param name="employeeId">The Id of the employee for this edit.</param>
        /// <param name="EmployerId">The Id of the employer for this edit.</param>
        /// <param name="TaxYear">The Id of the tax year for this edit.</param>
        /// <param name="requestor">The User making the edits.</param>
        void IUserEditPart2Service.UpdateWithEdits(IList<Employee1095detailsPart2Model> userSave, IDictionary<int, List<Employee1095detailsPart2Model>> currentSystem, int EmployerId, int TaxYear, string requestor)
        {
            // This code runs the Performance Timers and the `using` ensures that they get logged even if an exception is thrown or other exit happens before the the timers are logged
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(UserEditPart2Service), "UpdateWithEdits", SystemSettings.UsePerformanceLog))
            {

                methodTimer.StartTimer("Processing All Edits to be saved.");
                methodTimer.StartLapTimerPaused("Stupid Validation Checks");
                methodTimer.StartLapTimerPaused("Filter For MonthId");
                methodTimer.StartLapTimerPaused("Compare Edits and Prep to Save");

                // Build a list of all the edits that need to be saved
                List<UserEditPart2> editsToSave = new List<UserEditPart2>();

                // compare the current system value to the provided because we only want to update the differences
                foreach (Employee1095detailsPart2Model save in userSave)
                {
                    methodTimer.ResumeLap("Stupid Validation Checks");

                    // I'm so tired of weird data issues and other problems, I went overboard checking for stupid things that should not happen.

                    // Stupid double check  // should not be null
                    if (null == save)
                    {
                        // if this impossible happens, then throw an exception
                        throw new NullReferenceException("Value in userSave List was NULL, cannot be NULL.");

                    }

                    // Stupid double check  // should not be 0 or negative and month Id should not be > 12
                    if (0 >= save.EmployeeId || 0 >= save.MonthId || 12 < save.MonthId)
                    {
                        // if this impossible happens, then throw an exception
                        throw new ArgumentOutOfRangeException(string.Format("Value in userSave List was Invalid. EmployeeId: [{0}], MonthId: [{1}]", save.EmployeeId, save.MonthId));

                    }

                    // Stupid double check  // should match
                    if (false == currentSystem.ContainsKey(save.EmployeeId))
                    {
                        // This should be impossible! I think? Maybe with the ILBs?

                        throw new KeyNotFoundException(string.Format("Did not find the EmployeeId [{0}], in current system Values. System Values Count [{1}]", save.EmployeeId, currentSystem.Keys.Count));

                    }

                    methodTimer.ResumeLap("Filter For MonthId");

                    //Get the List of values by it's EmployeeId Key
                    IEnumerable<Employee1095detailsPart2Model> allOld = currentSystem[save.EmployeeId];
                    
                    // Stupid double check  // should return a non null value and the list should have items
                    if (null == allOld || 0 >= allOld.Count())
                    {
                        // If the list is somehow null or empty then we can't continue

                        throw new KeyNotFoundException(string.Format(
                            "Found Values for EmployeeId [{0}], in current system Values. But the Value was invalid. [{1}]",
                            save.EmployeeId,
                            allOld?.Count()
                            ));

                    }

                    //Get the specific value by it's month Id
                    IEnumerable<Employee1095detailsPart2Model> allOldForMonth = allOld.Where(x => x.MonthId == save.MonthId);

                    // Stupid double check  // There should only be one, and only one value for a specific month.
                    if (null == allOldForMonth || allOldForMonth.Count() <= 0 || allOldForMonth.Count() > 1)
                    {
                        // The month should be provided, if a Month ID is missing that implies bad data or ILB. 

                        throw new KeyNotFoundException(string.Format(
                            "Found Values for EmployeeId [{0}], but found no valid values for Month Id [{1}]. Values count was: [{2}]",
                            save.EmployeeId,
                            save.MonthId,
                            allOldForMonth?.Count()
                            ));

                    }

                    // Get the first Item or null
                    Employee1095detailsPart2Model old = allOldForMonth.FirstOrDefault();

                    methodTimer.Lap("Filter For MonthId");
                    methodTimer.PauseLap("Filter For MonthId");

                    // Stupid double check  // should not be null
                    if (null == old)
                    {

                        // if this impossible happens, then throw an exception
                        throw new NullReferenceException(string.Format("Value in currentSystem List with key [{0}] and month id [{1}] was NULL or was not found, cannot be NULL.", save.EmployeeId, save.MonthId));

                    }
                    else
                    {

                        // Stupid double check  // Check for bad Tax year Values
                        if (old.TaxYear != TaxYear
                            || save.TaxYear != TaxYear)
                        {
                            // this Should never be hit... 

                            throw new ArgumentException(string.Format("The Tax Years of the arguments did not match. TaxYear:[{0}], userSave.TaxYear:[{1}], currentSystem Item.TaxYear:[{2}], ", TaxYear, save.TaxYear, old.TaxYear));

                        }

                        methodTimer.Lap("Stupid Validation Checks");
                        methodTimer.PauseLap("Stupid Validation Checks");

                        methodTimer.ResumeLap("Compare Edits and Prep to Save");

                        // Remove it from the source List because we don't want to edit the same item multiple times.
                        currentSystem[save.EmployeeId].Remove(old);

                        // If the value to save is null, then set it to an empty string
                        if (null == save.Line14)
                        {
                            save.Line14 = string.Empty;
                        }

                        // line 14
                        if (null == old.Line14 // This is to force Null values to be over written
                            || (old.Line14 != save.Line14 // Don't create an edit if the new and old values are equal
                                && false == old.Line14.Trim().Equals(save.Line14.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // Add a new Edit to the list to save
                            editsToSave.Add(
                                new UserEditPart2()
                                {
                                    OldValue = old.Line14,
                                    NewValue = save.Line14.ToUpperInvariant().Trim(),
                                    MonthId = save.MonthId,
                                    LineId = 14,
                                    EmployeeId = save.EmployeeId,
                                    EmployerId = EmployerId,
                                    TaxYear = TaxYear
                                });
                        }

                        // If the value to save is null, then set it to an empty string
                        if (null == save.Line15)
                        {
                            save.Line15 = string.Empty;
                        }

                        // line 15
                        double parsed = 0.0;
                        if (null == old.Line15 // This is to force Null values to be over written
                            || (old.Line15 != save.Line15 // check that the values are not equal
                                && (save.Line15.Trim() == string.Empty // if the new value is not blank
                                    || // then compare on the value parsed to a double
                                    (true == double.TryParse(save.Line15, out parsed) //Note: parse failure will block the save
                                    && old.Line15 != parsed.ToString("F2"))))) // compare on the formatted text (2 decimal places)
                        {

                            // Check if this is a blank or a number
                            string newval = string.Empty;
                            if (save.Line15.Trim() != string.Empty)
                            {
                                newval = parsed.ToString("F2").ToUpperInvariant().Trim();
                            }

                            // Add a new Edit to the list to save
                            editsToSave.Add(
                                new UserEditPart2()
                                {
                                    OldValue = old.Line15,
                                    NewValue = newval,
                                    MonthId = save.MonthId,
                                    LineId = 15,
                                    EmployeeId = save.EmployeeId,
                                    EmployerId = EmployerId,
                                    TaxYear = TaxYear
                                });
                        }

                        // If the value to save is null, then set it to an empty string
                        if (null == save.Line16)
                        {
                            save.Line16 = string.Empty;
                        }

                        // line 16
                        if (null == old.Line16 // This is to force Null values to be over written
                            || (old.Line16 != save.Line16  // Don't create an edit if the new and old values are equal
                            && false == old.Line16.Trim().Equals(save.Line16.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // Add a new Edit to the list to save
                            editsToSave.Add(
                                new UserEditPart2()
                                {
                                    OldValue = old.Line16,
                                    NewValue = save.Line16.ToUpperInvariant().Trim(),
                                    MonthId = save.MonthId,
                                    LineId = 16,
                                    EmployeeId = save.EmployeeId,
                                    EmployerId = EmployerId,
                                    TaxYear = TaxYear
                                });
                        }

                        // Note: if the import did not specify Receiving1095C or not, then we don't count it as an edit                  
                        // Reciveing 1095
                        if (null != save.Receiving1095C && old.Receiving1095C != save.Receiving1095C)
                        {
                            // Add a new Edit to the list to save
                            editsToSave.Add(
                                new UserEditPart2()
                                {
                                    OldValue = old.Receiving1095C.ToString(),
                                    NewValue = save.Receiving1095C.ToString(),
                                    MonthId = save.MonthId,
                                    LineId = 0,
                                    EmployeeId = save.EmployeeId,
                                    EmployerId = EmployerId,
                                    TaxYear = TaxYear
                                });
                        }

                        methodTimer.Lap("Compare Edits and Prep to Save");
                        methodTimer.PauseLap("Compare Edits and Prep to Save");

                    }

                }

                methodTimer.LogTimeAndDispose("Processing All Edits to be saved.");
                methodTimer.LogAllLapsAndDispose("Stupid Validation Checks");
                methodTimer.LogAllLapsAndDispose("Filter For MonthId");
                methodTimer.LogAllLapsAndDispose("Compare Edits and Prep to Save");

                methodTimer.StartTimer("Saving All Edits with UpdateWithEditsMany");

                // Save all the Edits
                this.Repository.UpdateWithEditsMany(editsToSave, EmployerId, TaxYear, requestor);

                methodTimer.LogTimeAndDispose("Saving All Edits with UpdateWithEditsMany");

            }

        }

        /// <summary>
        /// Filters the Values for ones belonging to an employer. 
        /// </summary>
        /// <param name="employerId">The Employer Id.</param>
        /// <param name="taxYear">The Tax Year.</param>
        /// <returns>The filtered values</returns>
        Dictionary<int, Dictionary<int, List<UserEditPart2>>> IUserEditPart2Service.GetForEmployerTaxYear(int employerId, int taxYear)
        {

            return this.Repository.GetForEmployerTaxYear(employerId, taxYear);

        }

    }

}
