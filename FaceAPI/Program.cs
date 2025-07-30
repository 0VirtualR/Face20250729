
#region 1.����WebApplication������
using FaceAPI.Configure;
using FaceAPI.Middlewares;
using Serilog;
using Serilog.Events;
using static System.Net.WebRequestMethods;



/*.MinimumLevel.Information()                 // ȫ�������־����
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)  // ����Microsoft�����ռ����־����
.MinimumLevel.Override("System", LogEventLevel.Warning)     // ����System�����ռ����־����
����˵��
���÷���	����	ʾ��Ӱ��
.MinimumLevel.Information()	ȫ�������־����
ֻ��¼ Information �����ϼ���Info, Warning, Error, Fatal��	Debug ��־�ᱻ����
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)	���� Microsoft. ��ͷ�������־����
ֻ��¼ Warning �����ϼ���	���˵� ASP.NET Core ��ܵĵͼ�����־����·��ƥ�䡢�м�������ȣ�
.MinimumLevel.Override("System", LogEventLevel.Warning)	���� System. ��ͷ�������־����
ֻ��¼ Warning �����ϼ���	���˵� .NET �ײ�ϵͳ����ĵͼ�����־
*/

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning).
    MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console().
    WriteTo.File("LogFile/webapi-.log", 
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit:7,
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    ).
    CreateLogger();

try
{
    Log.Information("Starting web host...");
    var builder = WebApplication.CreateBuilder(args);
    //��������ʼ��һ��WebApplicationBuilderʵ��
    //args���������������д���Ĳ���
    //����Զ�����appsettings.json����������������
    #endregion

    builder.Host.UseSerilog();      //ʹ��Serilog������־��ӡ

    #region 2.��ӷ�������
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();//���API Explorer����
                                               //ΪSwagger�ȹ����ṩAPIԪ����

    builder.Services.AddSwaggerGen();//���Swagger����������
                                     //�����Զ�����API�ĵ�
    #endregion

    builder.Register();


    //3.����Ӧ�ó���
    var app = builder.Build();      //ʹ��֮ǰ���õķ��񹹽�WebApplicationʵ��

    app.UseMiddleware<ExceptionMiddlewareSerilog>();    //ʹ��Serilog��־�м��

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


    app.UseAuthentication();    //��Ȩ�м��
    app.UseAuthorization();//�����Ȩ�м��        ���û��ڽ�ɫ����Ȩ����

    app.UseCors("CorsPolicy");

    app.MapControllers();//ӳ������·�ɿ�����        ���������е�·������Ӧ�õ�Ӧ�ó���

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!"); // ���������쳣
}
finally
{
    Log.CloseAndFlush();
}
/*  Serilog��ʹ��
 * ��Ȼ�Ѿ��� try-catch ����ȫ���쳣��Ϊʲô����Ҫ ExceptionMiddlewareSerilog��
���ߵ����ò�ͬ
����  ���÷�Χ ������쳣   ������;
try-catch��Main ������	���������׶� CreateHostBuilder().Build().Run() ���쳣
����˿ڳ�ͻ�����ô���	��¼������־�������˳�
ExceptionMiddlewareSerilog  HTTP ����׶�   ���������м���ȴ�������ʱ���쳣 ���� JSON ������Ӧ����¼����������
Ϊʲô���߶���Ҫ��
Main �� try-catch �޷����� HTTP �����쳣

��ֻ���� Host ����ʱ���쳣���� app.Run() ֮ǰ�����⣩��

һ�� app.Run() ִ�У������� HTTP �����쳣���м������

ExceptionMiddlewareSerilog �ṩ���Ѻõ� API ������Ӧ

ֱ�ӷ��� JSON ��ʽ�Ĵ�����Ϣ���� { "Error": "xxx", "RequestId": "123" }����

��¼���������ģ��� URL��HTTP ���������� ID����

ʾ������
Main �� try-catch ����

���ݿ�����ʧ�ܡ������ļ�ȱʧ���˿ڱ�ռ�á�

ExceptionMiddlewareSerilog ����

UserController.GetById(999) �׳� NullReferenceException��*/