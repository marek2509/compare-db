using Porównator_Baz.Entities.Interface;
using Porównator_Baz.Middleware;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Porównator_Baz.Entities
{
    public class MainDbContext : DbContext
    {

        public MainDbContext(string path) : base(ConnectionString.GetConnectionString(path))
        {
        }
        public DbSet<Jedn_rej> Jedn_Rejs { get; set; }
    }
}