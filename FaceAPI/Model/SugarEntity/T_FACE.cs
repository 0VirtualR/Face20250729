using SqlSugar;

namespace FaceAPI.Model.SugarEntity
{
    public class T_FACE
    {
        [SugarColumn(IsNullable =false,IsIdentity =true,ColumnDescription ="人脸信息ID",IsPrimaryKey =true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="姓名")]
        public string Name { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="性别")]
        public string Sex {  get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="工号")]
        public string WorkId { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="工作")]
        public string WorkName { get; set; }
        [SugarColumn(IsNullable =false,ColumnDescription ="创建时间")]
        public DateTime CreateDate {  get; set; }
    }
}
