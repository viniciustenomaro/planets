using TesteApi.DataAccess.Infraestructure;
using System.Collections.Generic;

namespace TesteApi.DataAccess
{
    public class DataAccessDataConfigurations : IDataConfigurations
    {
        public static DataAccessDataConfigurations Instance { get; private set; } = new DataAccessDataConfigurations();

        private DataAccessDataConfigurations()
        {
        }

        public HashSet<dynamic> Configurations()
        {
            var config = new HashSet<dynamic>()
            {
            };

            return config;
        }
    }
}
