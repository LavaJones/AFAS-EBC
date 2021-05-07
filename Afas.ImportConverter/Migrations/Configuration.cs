namespace Afas.ImportConverter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Afas.ImportConverter.Domain.ImportConverterDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Afas.ImportConverter.Domain.ImportConverterDataContext context)
        {
        }
    }
}
