using SqlSugar;

namespace FaceAPI.Model.SugarEntity
{
    public class T_USER
    {
        [SugarColumn(IsNullable =false,IsIdentity =true,ColumnDescription ="用户ID",IsPrimaryKey =true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = false, ColumnDescription = "账号")]
        public string Account { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="用户名")]
        public string Username { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="密码")]
        public string Password { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "创建时间")]
        public string CreateTime { get; set; }
    }
}
