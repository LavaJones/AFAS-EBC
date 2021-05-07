using System;
using System.Collections.Generic;
using System.Linq;
using Afas.AfComply.Reporting.Domain.LegacyData;
using Afas.AfComply.Reporting.Core.Models;
using System.Diagnostics;
using log4net;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing.Response;
using System.Text;
using Afc.Core.Domain;
using Afas.AfComply.Domain;
using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.Domain.POCO;
using Afas.AfComply.Reporting.Core.Models;
using Afc.Marketing.Models;

namespace Afas.AfComply.Reporting.Domain.MonthlyDetails
{
    public static class _1094MonthlyDetails
    {
        private static ILog Log = LogManager.GetLogger(typeof(_1094MonthlyDetails));

        public static employer getEmployerById(int EmployerId, AcaEntities ctx)
        {
            employer Empr = ctx.employers.Where(emp => emp.employer_id == EmployerId).Single();
            return Empr;
        }
        public static Employer1094SummaryModel GetEmployee1094summaryModel(int EmployerId, int TaxYear)
        {
            using (var ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;

                var emp = _1094MonthlyDetails.getEmployerById(EmployerId, ctx);

                var part1 = _1094MonthlyDetails.getEmployerPart1(EmployerId, TaxYear, ctx);

                var part2 = _1094MonthlyDetails.getEmployerPart2(EmployerId, TaxYear, ctx);

                var part3 = _1094MonthlyDetails.getEmployerPart3(EmployerId, TaxYear, ctx);

                var part4 = _1094MonthlyDetails.getEmployerPart4(EmployerId, TaxYear, ctx);

                return CreateEmployer1094SummaryModel(part1, part2, part3, part4, ctx, EmployerId);

            }
        }

        private static Employer1094SummaryModel CreateEmployer1094SummaryModel(View_1094Part1 part1, View_1094Part2 part2, List<View_1094Part3> part3, List<View_1094Part4> part4, AcaEntities ctx, int EmployerId)
        {

            Employer1094SummaryModel Employer1094Details = new Employer1094SummaryModel();
            if (part1 !=null)
            { 
                MapPart1(Employer1094Details, part1);
                MapPart2(Employer1094Details, part2);
                MapPart3(Employer1094Details, part3, ctx);
                MapPart4(Employer1094Details, part4);
            }
            else
            {
                Log.Error(string.Format("Can't finalize because no records from view_1094Part1 for employer : {0}", EmployerId));

            }
            return Employer1094Details;

        }

        private static void MapPart4(Employer1094SummaryModel Employer1094Details, List<View_1094Part4> part4)
        {
            if (part4.Count != 0)
            {

                List<Employer1094detailsPart4Model> Part4OtherAleMembersOfAleGroup = new List<Employer1094detailsPart4Model>();
                foreach (var p4 in part4)
                {
                    Employer1094detailsPart4Model MapP4 = new Employer1094detailsPart4Model();
                    MapP4.EmployerName = p4.Name;
                    MapP4.EIN = p4.Ein;
                    Part4OtherAleMembersOfAleGroup.Add(MapP4);
                }
                Employer1094Details.Employer1094Part4s = Part4OtherAleMembersOfAleGroup;
            }

            Log.Info(string.Format("Mapped 1094 part4 details for employer :{0}", Employer1094Details.EmployerID));
        }

        private static void MapPart3(Employer1094SummaryModel Employer1094Details, List<View_1094Part3> part3, AcaEntities ctx)
        {
            if (part3.Count != 0)
            {
                List<Employer1094detailsPart3Model> P3 = new List<Employer1094detailsPart3Model>();

                for (int Month = 1; Month <= 12; Month++)
                {
                    Employer1094detailsPart3Model p3m = new Employer1094detailsPart3Model();
                    var part3MonthlyDetails = part3.Where(md => (md.MonthId == Month || md.MonthId == 0)&&md.Receiving1095C==true).ToList();

                    var FullTimeEmployesCount = from p in part3MonthlyDetails
                                                where p.Receiving1095C == true
                                                select p.NoOfEmployees;

                    var FullTimeEmployeeCount = part3MonthlyDetails.Where(p3 => p3.Receiving1095C == true).Sum(p => p.NoOfEmployees);
         
                    var FullTimeEmployeesOfferedInsurance = part3MonthlyDetails.Where(p3 => p3.Receiving1095C == true && p3.OfferedInsurance == true).Sum(p => p.NoOfEmployees);

                    float? MEC95 = 100f;
                    if (FullTimeEmployeeCount > 0)
                    {
                        MEC95 = (0.5f + ((100f * FullTimeEmployeesOfferedInsurance) / FullTimeEmployeeCount)); 
                    } 

                    p3m.MEC95 = Math.Floor(Convert.ToDouble(string.Format("{0:0.00}", MEC95)));
                    p3m.MinimumEssentialCoverageOfferIndicator = MEC95 >= 95 ? true : false;

                    if ((FullTimeEmployeeCount - FullTimeEmployeesOfferedInsurance) <= 5)
                    {
                        p3m.MinimumEssentialCoverageOfferIndicator = true;
                    }

                    p3m.FullTimeEmployeeCount = FullTimeEmployeeCount;
                    p3m.TotalEmployeeCount = GetTotalEmploeesCountForEmployerForPerticularMonth(Employer1094Details, Month, ctx);
                    p3m.MonthId = Month;
                    P3.Add(p3m);
                }
                Employer1094Details.Employer1094Part3s = P3;
            }
            Log.Info(string.Format("Mapped 1094 part3 details for employer :{0}", Employer1094Details.EmployerID));
        }

        private static int? GetTotalEmploeesCountForEmployerForPerticularMonth(Employer1094SummaryModel employer, int month, AcaEntities ctx)
        {
            var FirstDateOfTheMonth = GetFirstDateOfTheMonth(employer.TaxYearId, month);
            var LastDateOfTheMonth = GetLastDateOfTheMonth(employer.TaxYearId, month);

            var TotalEmoployees = ctx.employees.Where(emp => emp.employer_id == employer.EmployerID).ToList();

            var EmployeesInMonth = from emp in TotalEmoployees
                                   where (!emp.terminationDate.HasValue || emp.terminationDate >= FirstDateOfTheMonth) && emp.hireDate <= LastDateOfTheMonth
                                   select emp;
            return EmployeesInMonth.Count();


        }

        private static DateTime GetLastDateOfTheMonth(int taxYearId, int month)
        {
            DateTime firstDayOfTheMonth = new DateTime(taxYearId, month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        private static DateTime GetFirstDateOfTheMonth(int taxYearId, int month)
        {
            return new DateTime(taxYearId, month, 1);
        }

        private static void MapPart2(Employer1094SummaryModel Employer1094Details, View_1094Part2 part2)
        {
            if (part2 != null)
            {
                Employer1094Details.IsAggregatedAleGroup = part2.IsAle;
                Employer1094Details.Total1095Forms = part2.Form1095Count;
                Log.Info(string.Format("Mapped 1094 part2 details for employer :{0}", part2.employer_id));
            }
        }

        private static void MapPart1(Employer1094SummaryModel Employer1094Details, View_1094Part1 part1)
        {
            Employer1094Details.EmployerName = part1.EmployerName;
            Employer1094Details.EmployerID = part1.EmployerId;
            Employer1094Details.EmployerDBAName = part1.EmployerDBAName;
            Employer1094Details.TaxYearId = part1.TaxYearId;
            Employer1094Details.IsDge = part1.IsDge;
            Employer1094Details.EIN = part1.Ein;
            Employer1094Details.EmployerResourceId = part1.ResourceId;
            Employer1094Details.Address = part1.Address;
            Employer1094Details.City = part1.City;
            Employer1094Details.State = string.IsNullOrEmpty(part1.StateId.ToString()) ? "" : ((UsStateAbbreviationEnum)part1.StateId).ToString();
            Employer1094Details.StateId = part1.StateId;
            Employer1094Details.ZipCode = part1.ZipCode;
            Employer1094Details.IrsContactName = part1.IrsContactName;
            Employer1094Details.IrsContactPhone = part1.IrsContactPhoneNumber;
            Employer1094Details.TaxYearId = part1.TaxYearId;
            Employer1094Details.DgeName = part1.DgeName;
            Employer1094Details.DgeEIN = part1.DgeEin;
            Employer1094Details.DgeAddress = part1.DgeAddress;
            Employer1094Details.DgeCity = part1.DgeCity;
            Employer1094Details.DgeStateId = part1.DgeStateId;
            Employer1094Details.DgeState = string.IsNullOrEmpty(part1.DgeStateId.ToString()) ? "" : ((UsStateAbbreviationEnum)part1.DgeStateId).ToString();
            Employer1094Details.DgeZipCode = part1.DgeZipCode;
            Employer1094Details.DgeContactName = part1.DgeContactName;
            Employer1094Details.DgeContactPhone = part1.DgeContactName;
            Employer1094Details.TransmissionTotal1095Forms = part1.TransmissionTotal1095Forms;
            Employer1094Details.IsAuthoritiveTransmission = true;                  

            Log.Info(string.Format("Mapped 1094 part1 details for employer :{0}", part1.EmployerId));

        }

        private static List<View_1094Part3> getEmployerPart3(int employerId, int taxYear, AcaEntities ctx)
        {
            try
            {
                var part3 = ctx.View_1094Part3.Where(emp => emp.EmployerId == employerId && emp.TaxYear == taxYear).ToList();
                Log.Info(string.Format("Got 1094 part3 details for employer :{0} and 95% record count {1}", employerId, part3.Count));
                return part3;
            }
            catch (Exception ex)
            {
                Log.Error("Caught an exception in _1094MonthlyDetails -> getEmployerPart3.", ex);
                List<View_1094Part3> emptyPart3 = new List<View_1094Part3>();
                return emptyPart3;
            }

        }

        private static View_1094Part2 getEmployerPart2(int employerId, int taxYear, AcaEntities ctx)
        {
            try
            {
                var part2 = ctx.View_1094Part2.Where(emp => emp.employer_id == employerId && emp.tax_year == taxYear).SingleOrDefault();
                Log.Info(string.Format("Got 1094 part2 details for employer :", employerId));
                return part2;
            }
            catch (Exception ex)
            {
                Log.Error("Caught an exception in _1094MonthlyDetails -> getEmployerPart2.", ex);
                View_1094Part2 emptyPart2 = new View_1094Part2();
                return emptyPart2;
            }

        }

        private static View_1094Part1 getEmployerPart1(int employerId, int taxYear, AcaEntities ctx)
        {
            try
            {
                var part1 = ctx.View_1094Part1.Where(emp => emp.EmployerId == employerId && emp.TaxYearId == taxYear).SingleOrDefault();
                Log.Info(string.Format("Got 1094 part1 details for employer :{0}", employerId));
                return part1;
            }
            catch (Exception ex)
            {
                Log.Error("Caught an exception in _1094MonthlyDetails -> getEmployerPart1.", ex);
                View_1094Part1 emptyPart1 = new View_1094Part1();
                return emptyPart1;
            }
        }

        private static List<View_1094Part4> getEmployerPart4(int employerId, int taxYear, AcaEntities ctx)
        {
            try
            {
                var part4 = ctx.View_1094Part4.Where(emp => emp.EmployerId == employerId && emp.tax_year == taxYear).ToList();
                Log.Info(string.Format("Got 1094 part4 details for employer :{0} and ALE record count {1}", employerId, part4.Count));
                return part4;
            }
            catch (Exception ex)
            {
                Log.Error("Caught an exception in _1094MonthlyDetails -> getEmployerPart4.", ex);
                List<View_1094Part4> emptyPart4 = new List<View_1094Part4>();
                return emptyPart4;
            }
        }

        public static List<Core.Models.EmployerModel> getAllEmployers()
        {
            try
            {
                using (var ctx = new AcaEntities())
                {
                    ctx.Database.CommandTimeout = 180;

                    List<employer> employersList = ctx.employers.ToList();

                    List<Core.Models.EmployerModel> employers =  buildEmployerModel(employersList);
                    return employers;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Caught an exception in _1094MonthlyDetails -> getEmployerPart2.", ex);
                List<Core.Models.EmployerModel> EmptyEemployers = new List<Core.Models.EmployerModel>();
                return EmptyEemployers;
            }
        }

        private static List<Core.Models.EmployerModel> buildEmployerModel(List<employer> employers)
        {
            List<Core.Models.EmployerModel> Employers = new List<Core.Models.EmployerModel>();
            foreach (employer employer in employers)
            {
                Core.Models.EmployerModel emp = new Core.Models.EmployerModel();

                emp.EmployerId = employer.employer_id;
                emp.Name = employer.name;
                emp.DBAName = employer.DBAName;
                emp.Address = employer.address;
                emp.City = employer.city;
                emp.Zip = employer.zip;
                emp.StateId = employer.state_id;
                emp.EIN = employer.ein;
                emp.EmployerTypeId = employer.employer_type_id;
                emp.BillAddress = employer.bill_address;
                emp.BillCity = employer.bill_city;
                emp.BillZip = employer.bill_zip;
                emp.InitialMeasurementId = employer.initial_measurement_id;
                emp.ImportDemo = employer.import_demo;
                emp.ImportPayroll = employer.import_payroll;
                emp.IEI = employer.iei;
                emp.IEC = employer.iec;
                emp.FTPEI = employer.ftpei;
                emp.FTPEC = employer.ftpec;
                emp.IPI = employer.ipi;
                emp.IPC = employer.ipc;
                emp.FTPPC = employer.ftppc;
                emp.FTPPI = employer.ftppi;
                emp.ImportProcess = employer.importProcess;
                emp.VendorId = employer.vendor_id;
                emp.AutoUpload = employer.autoUpload;
                emp.AutoBill = employer.autoBill;
                emp.SuBilled = employer.suBilled;
                emp.ImportGP = employer.import_gp;
                emp.ImportHR = employer.import_hr;
                emp.ImportEC = employer.import_ec;
                emp.ImportIO = employer.import_io;
                emp.ImportIC = employer.import_ic;
                emp.ImportPayMod = employer.import_pay_mod;
                emp.ResourceId = employer.ResourceId;
                emp.IrsEnabled = employer.IrsEnabled;
                emp.Feeid = employer.fee_id;
                Employers.Add(emp);
            }
            return Employers;
        }

        public static void CheckAndChangeEmployerTaxYearTransmissionStatus(int EmployerId, int TaxYearId)
        {
            using (var ctx = new AcaEntities())
            {
                ctx.Database.CommandTimeout = 180;

                var EmployerTaxYearTransmissionStatus = (from TS in ctx.EmployerTaxYearTransmissionStatus 
                                                         join T in ctx.EmployerTaxYearTransmissions on TS.EmployerTaxYearTransmissionId equals T.EmployerTaxYearTransmissionId 
                                                         where T.EmployerId == EmployerId 
                                                            && T.TaxYearId== TaxYearId 
                                                            && TS.TransmissionStatusId== (int)TransmissionStatusEnum.Halt 
                                                         select TS).ToList();

                foreach (var rec in EmployerTaxYearTransmissionStatus)
                {
                    if (rec.TransmissionStatusId == (int)TransmissionStatusEnum.Halt)
                    {
                        rec.TransmissionStatusId = (int)TransmissionStatusEnum.Transmit;
                        ctx.EmployerTaxYearTransmissionStatus.Attach(rec);
                        ctx.Entry(rec).State = System.Data.Entity.EntityState.Modified;
                       
                    }
                }

                SaveContext(ctx);
            }
        }

        private static void SaveContext(AcaEntities ctx)
        {
            try
            {
                ctx.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                Log.Error("Errors during updating EmployerTaxYearTransmissionStatus", ex);

            }
        }
    }
}
