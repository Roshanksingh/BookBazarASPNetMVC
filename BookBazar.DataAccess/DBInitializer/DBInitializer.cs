using BookBazar.DataAccess.Data;
using BookBazar.DataAccess.DBInitializer;
using BookBazar.Models;
using BookBazar.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DBInitializer : IDBInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDBContext _db;
    private readonly IConfiguration _configuration;

    public DBInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDBContext db,
        IConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
        _configuration = configuration;
    }

    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Any())
                _db.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }

        if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();

            var adminEmail = _configuration["SeedAdmin:Email"]
                ?? throw new InvalidOperationException("SeedAdmin:Email not configured.");
            var adminPassword = _configuration["SeedAdmin:Password"]
                ?? throw new InvalidOperationException("SeedAdmin:Password not configured.");

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin",
                PhoneNumber = "0000000000",
                StreetAddress = "",
                State = "",
                City = "",
                PostalCode = ""
            }, adminPassword).GetAwaiter().GetResult();

            var user = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
            if (user != null)
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }
    }
}