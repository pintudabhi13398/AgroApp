using AgroApp.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Repository
{
    public class BaseContext
    {
        public static AgroAppEntities GetDbContext()
        {
            AgroAppEntities context = new AgroAppEntities();
            context.Configuration.ProxyCreationEnabled = false;
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            objectContext.CommandTimeout = int.MaxValue;
            return context;
        }
    }
    
}
