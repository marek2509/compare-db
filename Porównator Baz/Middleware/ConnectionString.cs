using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porównator_Baz.Middleware
{
    public static class ConnectionString
    {
        public static string Name { get; set; } = "SYSDBA";
        public static string Password { private get; set; }


        public static string GetConnectionString(string path)
        {
            SetPassword(Properties.Settings.Default.Password, Properties.Settings.Default.Login);
            return $@"User={Name};Password={Password};Database={path}; DataSource=localhost; Port=3051;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
        }

        static void SetPassword(string password, string login)
        {
            Password = password;
            Name = login;
        }

    }
}
