using BookBazar.DataAccess.Data;
using BookBazar.Models;
using BookBazar.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookBazar.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;

        public DBInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDBContext db
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "itsroshanksingh@gmail.com",
                    Email = "itsroshanksingh@gmail.com",
                    Name = "Roshan Singh",
                    PhoneNumber = "1234567890",
                    StreetAddress = "Test 123",
                    State = "Test State",
                    City = "Test City",
                    PostalCode = "123456"
                }, "Admin@123").GetAwaiter().GetResult();

                var user = _userManager.FindByEmailAsync("itsroshanksingh@gmail.com").GetAwaiter().GetResult();

                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}