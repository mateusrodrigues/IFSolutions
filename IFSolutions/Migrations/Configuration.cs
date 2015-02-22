namespace IFSolutions.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using IFSolutions.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<IFSolutions.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IFSolutions.Models.ApplicationDbContext context)
        {

        }
    }
}
