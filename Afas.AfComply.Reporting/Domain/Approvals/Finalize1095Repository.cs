using Afas.Domain;
using Afc.Framework.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public class Finalize1095Repository : BaseDomainRepository<Approved1095Final, IReportingDataContext>, IFinalize1095Repository
    {

        private readonly ILog Log = LogManager.GetLogger(typeof(Finalize1095Repository));

        public void DisableAutoDetectChanges()
        {

            ((DbContext)this.Context).Configuration.AutoDetectChangesEnabled = false;

        }

        public void EnableAutoDetectChanges()
        {

            ((DbContext)this.Context).Configuration.AutoDetectChangesEnabled = true;

        }

        public void DisableValidateOnSave()
        {

            ((DbContext)this.Context).Configuration.ValidateOnSaveEnabled = false;

        }

        public void EnableValidateOnSave()
        {

            ((DbContext)this.Context).Configuration.ValidateOnSaveEnabled = true;

        }

        private List<Approved1095Final> SqlBulkUpdateMain(IEnumerable<Approved1095Final> finals, int EmployerId, int TaxYear, string requestor, bool IsUpdate = false)
        {

            // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
            List<Approved1095Final> allFinals = finals.ToList();
            int bulkBatchSize = 100000;
            int iteration = 0;

            // Loop through everything as long as we have more data to save.
            while (allFinals.Count > bulkBatchSize * iteration)
            {
                // skip any rows we've already processed, then take the next batch worth
                finals = allFinals.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                // increment the counter so next loop we grab the next batch
                iteration++;



                DataTable table = new DataTable("Approved1095Final");

                table.Columns.Add("Approved1095FinalId", typeof(long));//[Approved1095FinalId] [bigint] IDENTITY(1,1) NOT NULL,
                table.Columns.Add("employerID", typeof(int));//[employerID] [int] NOT NULL,
                table.Columns.Add("employeeID", typeof(int));//[employeeID] [int] NOT NULL,
                table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,
                table.Columns.Add("FirstName", typeof(string));//[FirstName] [nvarchar](max) NOT NULL,
                table.Columns.Add("MiddleName", typeof(string));//[MiddleName] [nvarchar](max) NULL,
                table.Columns.Add("LastName", typeof(string));//[LastName] [nvarchar](max) NOT NULL,
                table.Columns.Add("SSN", typeof(string));//[SSN] [nvarchar](max) NOT NULL,
                table.Columns.Add("StreetAddress", typeof(string));//[StreetAddress] [nvarchar](max) NOT NULL,
                table.Columns.Add("City", typeof(string));//[City] [nvarchar](max) NOT NULL,
                table.Columns.Add("State", typeof(string));//[State] [nvarchar](max) NOT NULL,
                table.Columns.Add("Zip", typeof(string));//[Zip] [nvarchar](max) NOT NULL,
                table.Columns.Add("EmployeeResourceId", typeof(Guid)); //[EmployeeResourceId] [uniqueidentifier] NOT NULL,
                table.Columns.Add("SelfInsured", typeof(bool));//[SelfInsured] [bit] NOT NULL,
                table.Columns.Add("TaxYear", typeof(int));//[TaxYear] [int] NOT NULL,

                foreach (Approved1095Final final in finals)
                {
                    if (null == final.CreatedBy || string.Empty == final.CreatedBy)
                    {
                        final.CreatedBy = requestor;
                    }

                    if (0 == final.EntityStatus)
                    {
                        final.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                    }

                    final.ModifiedBy = requestor;
                    final.ModifiedDate = DateTime.Now;

                    DataRow row = table.NewRow();
                    row["Approved1095FinalId"] = final.ID.OrDBNull();

                    row["EmployeeResourceId"] = final.EmployeeResourceId.OrDBNull();
                    row["SelfInsured"] = final.SelfInsured.OrDBNull();
                    row["FirstName"] = final.FirstName.OrDBNull();
                    row["MiddleName"] = final.MiddleName.OrDBNull();
                    row["LastName"] = final.LastName.OrDBNull();
                    row["SSN"] = final.SSN.OrDBNull();
                    row["StreetAddress"] = final.StreetAddress.OrDBNull();
                    row["City"] = final.City.OrDBNull();
                    row["State"] = final.State.OrDBNull();
                    row["Zip"] = final.Zip.OrDBNull();

                    row["employeeID"] = final.EmployeeID.OrDBNull();
                    row["employerID"] = final.EmployerID.OrDBNull();
                    row["ResourceId"] = final.ResourceId.OrDBNull();
                    row["EntityStatusId"] = (int)final.EntityStatus.OrDBNull();
                    row["CreatedBy"] = final.CreatedBy.OrDBNull();
                    row["CreatedDate"] = final.CreatedDate.OrDBNull();
                    row["ModifiedBy"] = final.ModifiedBy.OrDBNull();
                    row["ModifiedDate"] = final.ModifiedDate.OrDBNull();
                    row["TaxYear"] = final.TaxYear.OrDBNull();

                    table.Rows.Add(row);

                }

                string connection = ((ReportingDataContext)this.Context).Database.Connection.ConnectionString;
                SqlBulkCopyOptions options = SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers;
                if (IsUpdate)
                {
                    options = options | SqlBulkCopyOptions.KeepIdentity;
                }
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, options))
                {

                    bulkCopy.DestinationTableName =
                        "aca.dbo.Approved1095Final";
                    bulkCopy.EnableStreaming = true;

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(ex.Message);
                    }

                }

            }


            List<Approved1095Final> results = new List<Approved1095Final>();

            //((DbContext)this.Context).Configuration.LazyLoadingEnabled = false;
            //((DbContext)this.Context).Configuration.AutoDetectChangesEnabled = false;
            //((DbContext)this.Context).Configuration.ProxyCreationEnabled = false;
            //((DbContext)this.Context).Configuration.UseDatabaseNullSemantics = true;

            List<Approved1095Final> fromDB = ((IFinalize1095Repository)this).GetApproved1095sForEmployerTaxYear(EmployerId, TaxYear);

            foreach (Approved1095Final final in finals)
            {
                Approved1095Final onefromDB = fromDB.Where(item => item.EmployeeID == final.EmployeeID).SingleOrDefault();

                if (null != onefromDB)
                {
                    onefromDB.part2s = final.part2s;
                    onefromDB.part3s = final.part3s;

                    results.Add(onefromDB);
                }
            }

            return results;

        }

        private void SqlBulkUpdatePart2(IEnumerable<Approved1095Final> finals, string requestor, bool IsUpdate = false)
        {

            // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
            List<Approved1095Final> allFinals = finals.ToList();
            int bulkBatchSize = 100000;
            int iteration = 0;

            // Loop through everything as long as we have more data to save.
            while (allFinals.Count > bulkBatchSize * iteration)
            {
                // skip any rows we've already processed, then take the next batch worth
                finals = allFinals.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                // increment the counter so next loop we grab the next batch
                iteration++;


                DataTable table = new DataTable("Approved1095FinalPart2");

                table.Columns.Add("Approved1095FinalPart2Id", typeof(long));//[Approved1095FinalPart2Id] [bigint] IDENTITY(1,1) NOT NULL,
                table.Columns.Add("employeeID", typeof(int));//[employeeID] [int] NOT NULL,
                table.Columns.Add("TaxYear", typeof(int));//[TaxYear] [int] NOT NULL,
                table.Columns.Add("MonthId", typeof(int));//[MonthId] [int] NOT NULL,
                table.Columns.Add("Line14", typeof(string));//[Line14] [nvarchar](max) NULL,
                table.Columns.Add("Line15", typeof(string));//[Line15] [nvarchar](max) NULL,
                table.Columns.Add("Line16", typeof(string));//[Line16] [nvarchar](max) NULL,
                table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,
                table.Columns.Add("Approved1095Final_ID", typeof(long));//[Approved1095Final_ID] [bigint] NULL,
                table.Columns.Add("Offered", typeof(bool));//[Offered] [bit] NOT NULL,
                table.Columns.Add("Receiving1095C", typeof(bool));//[Receiving1095C] [bit] NOT NULL,

                foreach (Approved1095Final final in finals)
                {

                    foreach (Approved1095FinalPart2 part2 in final.part2s)
                    {

                        if (null == part2.CreatedBy || string.Empty == part2.CreatedBy)
                        {
                            part2.CreatedBy = requestor;
                        }

                        if (0 == part2.EntityStatus)
                        {
                            part2.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                        }

                        part2.ModifiedBy = requestor;
                        part2.ModifiedDate = DateTime.Now;

                        DataRow row = table.NewRow();
                        row["Approved1095FinalPart2Id"] = part2.ID.OrDBNull();

                        row["MonthId"] = part2.MonthId.OrDBNull();
                        row["Line14"] = part2.Line14.OrDBNull();
                        row["Line15"] = part2.Line15.OrDBNull();
                        row["Line16"] = part2.Line16.OrDBNull();
                        row["Approved1095Final_ID"] = final.ID.OrDBNull();
                        row["Offered"] = part2.Offered.OrDBNull();
                        row["Receiving1095C"] = part2.Receiving1095C.OrDBNull();

                        row["employeeID"] = part2.employeeID.OrDBNull();
                        row["ResourceId"] = part2.ResourceId.OrDBNull();
                        row["EntityStatusId"] = (int)part2.EntityStatus.OrDBNull();
                        row["CreatedBy"] = part2.CreatedBy.OrDBNull();
                        row["CreatedDate"] = part2.CreatedDate.OrDBNull();
                        row["ModifiedBy"] = part2.ModifiedBy.OrDBNull();
                        row["ModifiedDate"] = part2.ModifiedDate.OrDBNull();
                        row["TaxYear"] = part2.TaxYear.OrDBNull();

                        table.Rows.Add(row);
                    }
                }

                string connection = ((ReportingDataContext)this.Context).Database.Connection.ConnectionString;
                SqlBulkCopyOptions options = SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers;
                if (IsUpdate)
                {
                    options = options | SqlBulkCopyOptions.KeepIdentity;
                }
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, options))
                {

                    bulkCopy.DestinationTableName =
                        "aca.dbo.Approved1095FinalPart2";
                    bulkCopy.EnableStreaming = true;

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(ex.Message);
                    }

                }

            }

        }

        private void SqlBulkUpdatePart3(IEnumerable<Approved1095Final> finals, string requestor, bool IsUpdate = false)
        {

            // These 3 values are used to batch the SQL Bulk calls so that we don't overload the DB
            List<Approved1095Final> allFinals = finals.ToList();
            int bulkBatchSize = 100000;
            int iteration = 0;

            // Loop through everything as long as we have more data to save.
            while (allFinals.Count > bulkBatchSize * iteration)
            {
                // skip any rows we've already processed, then take the next batch worth
                finals = allFinals.Skip(bulkBatchSize * iteration).Take(bulkBatchSize);
                // increment the counter so next loop we grab the next batch
                iteration++;



                DataTable table = new DataTable("Approved1095FinalPart3");

                table.Columns.Add("Approved1095FinalPart3Id", typeof(long));//[Approved1095FinalPart3Id] [bigint] IDENTITY(1,1) NOT NULL,
                table.Columns.Add("InsuranceCoverageRowID", typeof(int));//[InsuranceCoverageRowID] [int] NOT NULL,
                table.Columns.Add("EmployeeID", typeof(int));//[EmployeeID] [int] NOT NULL,
                table.Columns.Add("DependantID", typeof(int));//[DependantID] [int] NOT NULL,
                table.Columns.Add("TaxYear", typeof(int));//[TaxYear] [int] NOT NULL,
                table.Columns.Add("FirstName", typeof(string));//[FirstName] [nvarchar](max) NULL,
                table.Columns.Add("MiddleName", typeof(string));//[MiddleName] [nvarchar](max) NULL,
                table.Columns.Add("LastName", typeof(string));//[LastName] [nvarchar](max) NULL,
                table.Columns.Add("SSN", typeof(string));//[SSN] [nvarchar](max) NULL,
                table.Columns.Add("Dob", typeof(DateTime));//[Dob] [datetime] NOT NULL,
                table.Columns.Add("ResourceId", typeof(Guid)); //[ResourceId] [uniqueidentifier] NOT NULL,
                table.Columns.Add("EntityStatusId", typeof(int));//[EntityStatusId] [int] NOT NULL,
                table.Columns.Add("CreatedBy", typeof(string));//[CreatedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("CreatedDate", typeof(DateTime));//[CreatedDate] [datetime] NOT NULL,
                table.Columns.Add("ModifiedBy", typeof(string));//[ModifiedBy] [nvarchar](50) NOT NULL,
                table.Columns.Add("ModifiedDate", typeof(DateTime));//[ModifiedDate] [datetime] NOT NULL,
                table.Columns.Add("Approved1095Final_ID", typeof(long));//[Approved1095Final_ID] [bigint] NULL,
                table.Columns.Add("EnrolledJan", typeof(bool));//[EnrolledJan] [bit] NOT NULL
                table.Columns.Add("EnrolledFeb", typeof(bool));//[EnrolledFeb] [bit] NOT NULL
                table.Columns.Add("EnrolledMar", typeof(bool));//[EnrolledMar] [bit] NOT NULL
                table.Columns.Add("EnrolledApr", typeof(bool));//[EnrolledApr] [bit] NOT NULL
                table.Columns.Add("EnrolledMay", typeof(bool));//[EnrolledMay] [bit] NOT NULL
                table.Columns.Add("EnrolledJun", typeof(bool));//[EnrolledJun] [bit] NOT NULL
                table.Columns.Add("EnrolledJul", typeof(bool));//[EnrolledJul] [bit] NOT NULL
                table.Columns.Add("EnrolledAug", typeof(bool));//[EnrolledAug] [bit] NOT NULL
                table.Columns.Add("EnrolledSep", typeof(bool));//[EnrolledSep] [bit] NOT NULL
                table.Columns.Add("EnrolledOct", typeof(bool));//[EnrolledOct] [bit] NOT NULL
                table.Columns.Add("EnrolledNov", typeof(bool));//[EnrolledNov] [bit] NOT NULL
                table.Columns.Add("EnrolledDec", typeof(bool));//[EnrolledDec] [bit] NOT NULL


                foreach (Approved1095Final final in finals)
                {
                    foreach (Approved1095FinalPart3 part3 in final.part3s)
                    {

                        if (null == part3.CreatedBy || string.Empty == part3.CreatedBy)
                        {
                            part3.CreatedBy = requestor;
                        }

                        if (0 == part3.EntityStatus)
                        {
                            part3.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                        }

                        part3.ModifiedBy = requestor;
                        part3.ModifiedDate = DateTime.Now;

                        DataRow row = table.NewRow();
                        row["Approved1095FinalPart3Id"] = part3.ID.OrDBNull();

                        row["InsuranceCoverageRowID"] = part3.InsuranceCoverageRowID.OrDBNull();
                        row["DependantID"] = part3.DependantID.OrDBNull();
                        row["FirstName"] = part3.FirstName.OrDBNull();
                        row["MiddleName"] = part3.MiddleName.OrDBNull();
                        row["LastName"] = part3.LastName.OrDBNull();
                        row["SSN"] = part3.SSN.OrDBNull();
                        row["Dob"] = part3.Dob.OrDBNull();

                        row["Approved1095Final_ID"] = final.ID.OrDBNull();

                        row["EmployeeID"] = part3.EmployeeID.OrDBNull();
                        row["ResourceId"] = part3.ResourceId.OrDBNull();
                        row["EntityStatusId"] = (int)part3.EntityStatus.OrDBNull();
                        row["CreatedBy"] = part3.CreatedBy.OrDBNull();
                        row["CreatedDate"] = part3.CreatedDate.OrDBNull();
                        row["ModifiedBy"] = part3.ModifiedBy.OrDBNull();
                        row["ModifiedDate"] = part3.ModifiedDate.OrDBNull();
                        row["TaxYear"] = part3.TaxYear.OrDBNull();

                        row["EnrolledJan"] = part3.EnrolledJan.OrDBNull();
                        row["EnrolledFeb"] = part3.EnrolledFeb.OrDBNull();
                        row["EnrolledMar"] = part3.EnrolledMar.OrDBNull();
                        row["EnrolledApr"] = part3.EnrolledApr.OrDBNull();
                        row["EnrolledMay"] = part3.EnrolledMay.OrDBNull();
                        row["EnrolledJun"] = part3.EnrolledJun.OrDBNull();
                        row["EnrolledJul"] = part3.EnrolledJul.OrDBNull();
                        row["EnrolledAug"] = part3.EnrolledAug.OrDBNull();
                        row["EnrolledSep"] = part3.EnrolledSep.OrDBNull();
                        row["EnrolledOct"] = part3.EnrolledOct.OrDBNull();
                        row["EnrolledNov"] = part3.EnrolledNov.OrDBNull();
                        row["EnrolledDec"] = part3.EnrolledDec.OrDBNull();

                        table.Rows.Add(row);
                    }
                }

                string connection = ((ReportingDataContext)this.Context).Database.Connection.ConnectionString;
                SqlBulkCopyOptions options = SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.FireTriggers;
                if (IsUpdate)
                {
                    options = options | SqlBulkCopyOptions.KeepIdentity;
                }
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, options))
                {

                    bulkCopy.DestinationTableName =
                        "aca.dbo.Approved1095FinalPart3";
                    bulkCopy.EnableStreaming = true;

                    try
                    {
                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(table);
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(ex.Message);
                    }

                }

            }

        }

        void IFinalize1095Repository.SaveApproved1095(IList<Approved1095Final> Approved1095s, int EmployerId, int TaxYear, string requestor, bool IsUpdate = false)
        {

            List<Approved1095Final> added = this.SqlBulkUpdateMain(Approved1095s, EmployerId, TaxYear, requestor, IsUpdate);

            this.SqlBulkUpdatePart2(added, requestor, IsUpdate);

            this.SqlBulkUpdatePart3(added, requestor, IsUpdate);

        }

        List<Approved1095Final> IFinalize1095Repository.GetApproved1095sForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return this.Context.Set<Approved1095Final>()
                .FilterForActive()
                .FilterForEmployer(EmployerId)
                .FilterForTaxYear(TaxYear)
                .Include("part2s")
                .Include("part3s")
                .ToList();

        }


        List<int> IFinalize1095Repository.GetApproved1095sEmployeeIdsForEmployerTaxYear(int EmployerId, int TaxYear)
        {

            return (from
                        approved
                    in
                        this.Context.Set<Approved1095Final>()
                            .FilterForActive()
                            .FilterForEmployer(EmployerId)
                            .FilterForTaxYear(TaxYear)
                            .Distinct()
                    select approved.EmployeeID)
                .ToList();

        }
    }
}
