using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using ARW.Admin.WebApi.Framework;
using Hei.Captcha;
using Infrastructure.Extensions;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Admin.WebApi.Middleware;
using ARW.Admin.WebApi.Hubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Infrastructure.WeChat.Login;
using Senparc.Weixin.RegisterServices;
using Senparc.CO2NET.RegisterServices;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Weixin.Entities;
using Senparc.Weixin;
using ARW.Model.System.Generate;
using Senparc.Weixin.WxOpen.Containers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    //忽略循环引用
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

    //不改变字段大小
    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//注入HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// 微信服务配置
builder.Services.AddMemoryCache();
builder.Services.AddSenparcGlobalServices(builder.Configuration)//Senparc.CO2NET 全局注册
                   .AddSenparcWeixinServices(builder.Configuration);
var senparcSetting = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<SenparcSetting>>();
IRegisterService register = RegisterService.Start(senparcSetting.Value).UseSenparcGlobal();// 启动 CO2NET 全局注册，必须！
var senparcWeixinSetting = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<SenparcWeixinSetting>>();
register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);//微信全局注册，必须！

// 小程序
await AccessTokenContainer.RegisterAsync("wx34436769371ceb7c", "8ded49f94c8e607de7cc4244639d4666");
// 公众号
await AccessTokenContainer.RegisterAsync("wx99caf571c9fb1cc5", "61100d9c4fff5acf97517f0c8799e2e2");

// 添加WeChat单例服务
builder.Services.AddSingleton<WeChatLogin>(new WeChatLogin("wxf3f7f286bfbb7dfa", "c2a02e478d9f0683d8dafec646c3d2ca"));

//配置跨域
builder.Services.AddCors(c =>
{
    c.AddPolicy("Policy", policy =>
    {
        policy.WithOrigins(builder.Configuration["corsUrls"].Split(',', StringSplitOptions.RemoveEmptyEntries))
        .AllowAnyHeader()//允许任意头
        .AllowCredentials()//允许cookie
        .AllowAnyMethod();//允许任意方法
    });
});

//注入SignalR实时通讯，默认用json传输
builder.Services.AddSignalR();
//消除Error unprotecting the session cookie警告
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
//普通验证码
builder.Services.AddHeiCaptcha();
//builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
//绑定整个对象到Model上
builder.Services.Configure<OptionsSetting>(builder.Configuration);

//jwt 认证
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie()
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = JwtUtil.ValidParameters();
});

//InternalApp.InternalServices = builder.Services;
builder.Services.AddAppService();
builder.Services.AddSingleton(new AppSettings(builder.Configuration));
//开启计划任务
builder.Services.AddTaskSchedulers();
//初始化db
DbExtension.AddDb(builder.Configuration);

//注册REDIS 服务
Task.Run(() =>
{
    //RedisServer.Initalize();
});
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(GlobalActionMonitor));//全局注册
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonConverterUtil.DateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonConverterUtil.DateTimeNullConverter());
});

builder.Services.AddSwaggerConfig();

var app = builder.Build();
if (builder.Configuration["InitDb"].ParseToBool() == true)
{
    InternalApp.ServiceProvider = app.Services;
    app.Services.InitDb();
}

app.UseSwagger();

//使可以多次多去body内容
app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    if (context.Request.Query.TryGetValue("access_token", out var token))
    {
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
    }
    return next();
});

//app.UseSenparcWeixin(app.Environment, null, null, register => { },
//    (register, weixinSetting) =>
//{
//    //注册公众号信息（可以执行多次，注册多个公众号）
//    register.RegisterMpAccount(weixinSetting, "【盛派网络小助手】公众号");
//});

//开启访问静态文件/wwwroot目录文件，要放在UseRouting前面
app.UseStaticFiles();
//开启路由访问
app.UseRouting();
app.UseCors("Policy");//要放在app.UseEndpoints前。
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//开启缓存
app.UseResponseCaching();
//恢复/启动任务
app.UseAddTaskSchedulers();
//使用全局异常中间件
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
    //设置socket连接
    endpoints.MapHub<MessageHub>("/msgHub");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.MapControllers();

//app.UseSenparcWeixin(app.Environment, null, null, register => { },
//    (register, weixinSetting) =>
//    {
//        //注册公众号信息（可以执行多次，注册多个公众号）
//        register.RegisterMpAccount(weixinSetting, "【盛派网络小助手】公众号");
//    });

app.Run();