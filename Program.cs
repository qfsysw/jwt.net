
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using webapijwttest.Models;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
#region JWT服务
// 注册JWT服务
builder.Services.AddSingleton(new JwtHelper2(builder.Configuration));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //是否验证Issuer
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //发行人Issuer
        ValidateAudience = false, //是否验证Audience      
        ValidateIssuerSigningKey = true, //是否验证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecKey"])), //SecurityKey
        ValidateLifetime = true, //是否验证失效时间
        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
        RequireExpirationTime = true,
    };
}
);
#endregion


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
    //开启注释
    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);//需要 生成 目录生成XML
    c.IncludeXmlComments(xmlPath, true);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
        Name = "Authorization",//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
        Type = SecuritySchemeType.ApiKey
    });
    //认证方式，此方式为全局添加
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference()
                    {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                    }
                    }, Array.Empty<string>() }
                    });



    // 配置 JWT Bearer 授权
    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    Description = "JWT Authorization header using the Bearer scheme",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "bearer"
    //});
    //var securityScheme = new OpenApiSecurityScheme
    //{
    //    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    //};
    //var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, new string[] { } } };
    //c.AddSecurityRequirement(securityRequirement);




});


var app = builder.Build();


//启用验证中间件
app.UseAuthentication();
app.UseAuthorization();



app.UseSwagger();

app.UseSwaggerUI(options =>
{

    options.SwaggerEndpoint("/swagger/v1/swagger.json", " api v1 ");
    //  options.RoutePrefix = string.Empty;
    options.DocumentTitle = "IM API DOC v1";

});



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts(); //https


}
//app.UseHttpsRedirection(); //https

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
