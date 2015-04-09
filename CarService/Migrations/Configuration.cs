namespace CarService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using CarService.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CarService.Models.CarServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CarService.Models.CarServiceContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Manufacturers.AddOrUpdate(x => x.Id,
        new Manufacturer() { Id = 1, Name = "Toyota" },
        new Manufacturer() { Id = 2, Name = "Nissan" },
        new Manufacturer() { Id = 3, Name = "Ford" }
        );

            context.Models.AddOrUpdate(x => x.Id,
                new Model()
                {
                    Id = 1,
                    Name = "Yaris",
                    Year = 2004,
                    ManufacturerId = 1,
                    Price = 6000,
                    FuelType = "Diesel"
                },
                new Model()
                {
                    Id = 2,
                    Name = "Aygo",
                    Year = 2006,
                    ManufacturerId = 1,
                    Price = 4000,
                    FuelType = "Gas"
                },
                new Model()
                {
                    Id = 3,
                    Name = "Qashqai",
                    Year = 2012,
                    ManufacturerId = 2,
                    Price = 19000,
                    FuelType = "Gas"
                },
                new Model()
                {
                    Id = 4,
                    Name = "Mondeo",
                    Year = 2000,
                    ManufacturerId = 3,
                    Price = 2000,
                    FuelType = "Gas"
                }
                );
        }
    }
}
