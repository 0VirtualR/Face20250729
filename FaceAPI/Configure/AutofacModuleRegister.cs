using Autofac;
using System.Reflection;

namespace FaceAPI.Configure
{
    public class AutofacModuleRegister:Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //base.Load(builder);


            //Assembly interfaceAssembly = Assembly.Load("Interface");
            //Assembly serviceAssembly = Assembly.Load("Service");
            //builder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();


            // 获取当前项目的程序集（假设接口和实现类都在当前程序集中）
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            // 通过命名空间或类型名称过滤接口和实现类
            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => t.Namespace != null && (
                    t.Namespace.EndsWith("Interface") || // 接口放在 Interface 文件夹/命名空间下
                    t.Namespace.EndsWith("Service")      // 实现类放在 Service 文件夹/命名空间下
                ))
                .AsImplementedInterfaces(); // 自动注册接口和实现的对应关系
        }
    }
}
