using DataLayer.DatabaseModel;

namespace DataLayer
{
    public static class BaseContext
    {
        public static StockEntities GetDbContext()
        {
            StockEntities context = new StockEntities();
            return context;
        }

        public static StockEntities GetDbContextForJSON()
        {
            StockEntities context = new StockEntities();
            context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.LazyLoadingEnabled = false;
            return context;
        }
    }
}
