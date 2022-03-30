using System.Collections.Generic;

namespace TesteApi.DataAccess.Infraestructure
{
    public interface IDataConfigurations
    {
        HashSet<dynamic> Configurations();
    }
}
