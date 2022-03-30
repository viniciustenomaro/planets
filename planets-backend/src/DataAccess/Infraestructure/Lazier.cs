using System;
using Microsoft.Extensions.DependencyInjection;

namespace TesteApi.DataAccess.Infraestructure
{
    public class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        {
        }
    }
}
