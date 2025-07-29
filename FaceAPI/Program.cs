
#region 1.����WebApplication������
var builder = WebApplication.CreateBuilder(args);
//��������ʼ��һ��WebApplicationBuilderʵ��
//args���������������д���Ĳ���
//����Զ�����appsettings.json����������������
#endregion

#region 2.��ӷ�������
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();//���API Explorer����
//ΪSwagger�ȹ����ṩAPIԪ����

builder.Services.AddSwaggerGen();//���Swagger����������
//�����Զ�����API�ĵ�
#endregion

//3.����Ӧ�ó���
var app = builder.Build();      //ʹ��֮ǰ���õķ��񹹽�WebApplicationʵ��

// 4. ����HTTP����ܵ�
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //���Swagger�м������ʹע�͵��˻�����飬Ҳ��ʼ�����ã�

    //UseSwagger()������Swagger JSON�ĵ�

    //UseSwaggerUI()���ṩSwagger UI����
}

app.UseHttpsRedirection();//����HTTPS�ض����м�� 
/*���ã�������HTTP�����ض���HTTPS

ʵ��Ч����

����û����� http://example.com/api/values

�Զ��ض��� https://example.com/api/values

��Ҫ�ԣ���ȫ���ʵ�����ر�����������*/


app.UseAuthorization();//�����Ȩ�м��        ���û��ڽ�ɫ����Ȩ����

app.MapControllers();//ӳ������·�ɿ�����        ���������е�·������Ӧ�õ�Ӧ�ó���

app.Run();
