using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.DataProvider.Models
{
    [DbConfigurationType(typeof(DRADBContextConfig))]
    public partial class DRAContext : DbContext
    {
        public DRAContext(string connectionString) : base(connectionString) { }
    }

    public class DRADBContextConfig : DbConfiguration
    {
        public DRADBContextConfig()
        {
            SetProviderServices("System.Data.EntityClient",
            SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }
}
