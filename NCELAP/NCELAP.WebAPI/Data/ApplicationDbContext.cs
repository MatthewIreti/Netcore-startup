using System;
using Microsoft.EntityFrameworkCore;
using NCELAP.WebAPI.Models.Entities.Support;

namespace NCELAP.WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<SupportTickets> SupportTickets { get; set; }

        public DbSet<SupportTicketsComment> SupportComments { get; set; }

    }
}
