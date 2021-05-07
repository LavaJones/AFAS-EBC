using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;

using Afc.Core;

namespace Afas.ImportConverter.Domain
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomSqlCodeGenerator : CSharpMigrationCodeGenerator
    {

        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {

            SharedUtilities.VerifyObjectParameter(createTableOperation, "CreateTableOperation");
            SharedUtilities.VerifyObjectParameter(writer, "Writer");

            createTableOperation.PrimaryKey.Name = CreatePrivateKeyName(createTableOperation.PrimaryKey);

            base.Generate(createTableOperation, writer);

        }

        protected override void GenerateInline(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {

            SharedUtilities.VerifyObjectParameter(addForeignKeyOperation, "AddForeignKeyOperation");
            SharedUtilities.VerifyObjectParameter(writer, "Writer");

            addForeignKeyOperation.Name = CreateForeignKeyName(addForeignKeyOperation);

            writer.WriteLine();
            writer.Write(".ForeignKey(" + Quote(addForeignKeyOperation.PrincipalTable) + ", ");
            Generate(addForeignKeyOperation.DependentColumns, writer);

            if (addForeignKeyOperation.CascadeDelete)
            {
                writer.Write(", cascadeDelete: true");
            }

            if (!addForeignKeyOperation.HasDefaultName)
            {

                writer.Write(", name: ");

                writer.Write(Quote(addForeignKeyOperation.Name));

            }

            writer.Write(")");

        }

        protected override void GenerateInline(CreateIndexOperation createIndexOperation, IndentedTextWriter writer)
        {

            SharedUtilities.VerifyObjectParameter(createIndexOperation, "CreateIndexOperation");
            SharedUtilities.VerifyObjectParameter(writer, "Writer");

            createIndexOperation.Name = CreateIndexName(createIndexOperation);

            writer.WriteLine();
            writer.Write(".Index(");
            Generate(createIndexOperation.Columns, writer);

            if (!createIndexOperation.HasDefaultName)
            {

                writer.Write(", name: ");
                writer.Write(Quote(createIndexOperation.Name));

            }

            if (createIndexOperation.IsUnique)
            {
                writer.Write(", unique: true");
            }

            writer.Write(")");

        }

        protected virtual string CreateForeignKeyName(AddForeignKeyOperation addForeignKeyOperation)
        {

            SharedUtilities.VerifyObjectParameter(addForeignKeyOperation, "AddForeignKeyOperation");

            StringBuilder formattedName = new StringBuilder();
            formattedName.Append("FK_");
            formattedName.Append(StripDbo(addForeignKeyOperation.PrincipalTable));
            formattedName.Append("_");
            formattedName.Append(StripDbo(addForeignKeyOperation.DependentTable));

            foreach (String columnName in addForeignKeyOperation.DependentColumns)
            {

                formattedName.Append("_");
                formattedName.Append(columnName);

            }

            return formattedName.ToString();

        }

        protected virtual String CreateIndexName(CreateIndexOperation createIndexOperation)
        {

            SharedUtilities.VerifyObjectParameter(createIndexOperation, "CreateIndexOperation");

            StringBuilder formattedName = new StringBuilder();

            if (createIndexOperation.IsUnique && createIndexOperation.Columns.Count == 1 && createIndexOperation.Columns.Contains("ResourceID"))
            {
                formattedName.Append("UDX_");
            }
            else
            {
                formattedName.Append("IDX_");
            }

            formattedName.Append(StripDbo(createIndexOperation.Table));

            foreach (String columnName in createIndexOperation.Columns)
            {
                formattedName.Append("_");
                formattedName.Append(columnName);
            }

            return formattedName.ToString();

        }

        protected virtual String CreatePrivateKeyName(AddPrimaryKeyOperation addPrimaryKeyOperation)
        {

            SharedUtilities.VerifyObjectParameter(addPrimaryKeyOperation, "AddPrimaryKeyOperation");

            StringBuilder formattedName = new StringBuilder();
            formattedName.Append("PK_");
            formattedName.Append(StripDbo(addPrimaryKeyOperation.Table));

            foreach (String columnName in addPrimaryKeyOperation.Columns)
            {

                formattedName.Append("_");
                formattedName.Append(columnName);

            }

            return formattedName.ToString();

        }

        protected String StripDbo(String table)
        {
            if (table.StartsWith("dbo."))
            {
                return table.Substring(4);
            }

            return table;

        }

    }

}
