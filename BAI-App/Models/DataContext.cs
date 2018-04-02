using System.Data.Entity;
using Bai_APP.Models;

namespace Bai_APP.Entity
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataConnection") { }

        public DbSet<Message> Messages { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AllowedMessage> AllowedMessages { get; set; }

        public DbSet<AnonymousUser> AnonymousUsers { get; set; }
    }
}
