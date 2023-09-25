using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeopleManager.Model;

namespace PeopleManager.Core
{
    public class PeopleManagerDbContext : IdentityDbContext
    {

        public PeopleManagerDbContext(DbContextOptions<PeopleManagerDbContext> options) : base(options)
        {

        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.ResponsiblePerson)
                .WithMany(p => p.ResponsibleForVehicles)
                .HasForeignKey(v => v.ResponsiblePersonId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            AddDefaultRoles();
            AddDefaultUser();

            var bavoPerson = new Person
            {
                FirstName = "Bavo",
                LastName = "Ketels",
                Email = "bavo.ketels@vives.be",
                Description = "Lector"
            };
            var wimPerson = new Person
            {
                FirstName = "Wim",
                LastName = "Engelen",
                Email = "wim.engelen@vives.be",
                Description = "Opleidingshoofd"
            };

            People.AddRange(new List<Person>
            {
                bavoPerson,
                new Person{FirstName = "Isabelle", LastName = "Vandoorne", Email = "isabelle.vandoorne@vives.be" },
                wimPerson,
                new Person{FirstName = "Ebe", LastName = "Deketelaere", Email = "ebe.deketelaere@vives.be" }
            });

            Vehicles.AddRange(new[] {
                new Vehicle{LicensePlate = "1-ABC-123", ResponsiblePerson = bavoPerson},
                new Vehicle{LicensePlate = "THE_BOSS", Brand= "Ferrari", Type="448", ResponsiblePerson = wimPerson},
                new Vehicle{LicensePlate = "SALES_GUY_1", Brand= "Audi", Type="e-tron", ResponsiblePerson = bavoPerson},
                new Vehicle{LicensePlate = "DESK_1", Brand= "Fiat", Type="Punto", ResponsiblePerson = bavoPerson}});

            SaveChanges();
        }

        private void AddDefaultUser()
        {
            var managerRole = Roles.SingleOrDefault(r => r.Name == "Manager");
            if (managerRole is null)
            {
                return;
            }

            var username = "manager@test.be";

            var user = new IdentityUser
            {
                AccessFailedCount = 0,
                EmailConfirmed = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                Email = username,
                NormalizedEmail = username.ToUpper(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAENoVftXbReMcFkOwzlfLFgFkx1tZ2PVoFwKaz/7UP6r5rlrymlHMFjMgowJCZl+6YA==" //Test123$
            };

            Users.Add(user);

            var normalUsername = "normal@test.be";
            var normalUser = new IdentityUser
            {
                AccessFailedCount = 0,
                EmailConfirmed = false,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                Email = normalUsername,
                NormalizedEmail = normalUsername.ToUpper(),
                UserName = normalUsername,
                NormalizedUserName = normalUsername.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAENoVftXbReMcFkOwzlfLFgFkx1tZ2PVoFwKaz/7UP6r5rlrymlHMFjMgowJCZl+6YA==" //Test123$
            };

            Users.Add(normalUser);
            SaveChanges();

            UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = managerRole.Id, 
                UserId = user.Id
            });
            SaveChanges();
        }

        private void AddDefaultRoles()
        {
            Roles.Add(new IdentityRole("Manager"));
            Roles.Add(new IdentityRole("Moderator"));

            SaveChanges();
        }
    }
}
