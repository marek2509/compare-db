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
        //private const string _connectionStringBase = @"User=sysdba;Password=M@r3kwbg;Database= D:\Bazy\Scalenia\Milejczyce porównanie przed po\MILEJCZYCE 2021.FDB; DataSource=localhost; Port=3051;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
        private static string GetConnectionString(string path)
        {
            return $@"User=sysdba;Password=;Database={path}; DataSource=localhost; Port=3051;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
        }

        //public DbSet<Udzialy> Udzialy { get; set; }
        //public DbSet<Podmioty> Podmioty { get; set; }


        public MainDbContext(string path) : base(GetConnectionString(path))
        {
        }
        public DbSet<Jedn_rej> Jedn_Rejs { get; set; }


    }
}