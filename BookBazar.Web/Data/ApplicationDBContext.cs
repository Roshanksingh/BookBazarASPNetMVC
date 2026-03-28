using Microsoft.EntityFrameworkCore;

namespace BookBazar.Web.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
    }
}
