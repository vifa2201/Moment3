using Microsoft.EntityFrameworkCore;
using Moment3.Models;

namespace Moment3.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

    }
public DbSet<BookModel> Books { get; set; }

public DbSet<CategoryModel> Categories { get; set; }

public DbSet<AuthorModel> Authors { get; set;}

public DbSet<LoanModel> Loans {get; set;}

public DbSet<UserModel> Users {get; set;}
}