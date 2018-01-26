
namespace EfCoreCodeFirstSQLite
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Simple demonstration of using code-first EntityFrameworkCore on a Sqlite database.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new PetContext())
            {
                // Create database if it does not exist.  (Don't used for migrations)
                db.Database.EnsureCreated();

                // Get a pet.
                string[] listOfNames = { "Cat", "Dog", "Fish", "Bird" };
                string petName = listOfNames[db.Pets.Count() % listOfNames.Length];

                // Add a pet.
                Pet onePet = new Pet { Name = petName };
                db.Add(onePet);
                db.SaveChanges();

                // List the pets.
                db.Pets
                    .Select(pet => pet.Name)
                    .ToList()
                    .ForEach(name => { Console.WriteLine(name); });
            }

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// This describes a table row.
    /// </summary>
    [Table("Pet")]
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// This class is use to access the database.
    /// </summary>
    public class PetContext : DbContext
    {
        // Each table refinance goes here.
        public DbSet<Pet> Pets { get; set; }

        // Gets call once when opening the database.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // The database will show up in the VS Solution Explorer after its created.
            optionsBuilder.UseSqlite("Data Source=Pet.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
