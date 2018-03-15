using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bai_APP.Entity
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataConnection") { }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AllowedMessage> AllowedMessages { get; set; }
    }
}
