using FaceAPI.Model.SugarEntity;
using SqlSugar;

namespace FaceAPI.Configure
{
    public class DatabaseMigrationService
    {
        private readonly ISqlSugarClient client;

     public   DatabaseMigrationService(ISqlSugarClient client)
        {
            this.client = client;
        }
        public void Migrate()
        {
            client.DbMaintenance.CreateDatabase();
            client.CodeFirst.InitTables(typeof(T_USER));  
            client.CodeFirst.InitTables(typeof(T_FACE));  
        }
    }
}
