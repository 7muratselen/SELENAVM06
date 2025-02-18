using Microsoft.EntityFrameworkCore;
using Serilog;
using SELENAVM06.Data;  // DB context'inizin olduğu namespace
using SELENAVM06.Services.XMLS;  // XmlReaderService'in olduğu namespace

namespace SELENAVM06
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Uygulama hizmetlerini ekleyin
            builder.Services.AddControllersWithViews();

            // SelenavmDbContext için DbContext servisini ekleyin
            builder.Services.AddDbContext<SelenavmDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLSelenavmConnection")));

            // XmlReaderService için servisi ekleyin
            builder.Services.AddScoped<XmlReaderService>();

            var app = builder.Build();

            // HTTP istek hattını yapılandırın
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=ProductXMLS}/{action=Index}/{id?}");

            try
            {
                Log.Information("Application is starting");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application could not start properly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}






//appsettings.json dosyasında ve Program.cs dosyasında e-posta ayarlarınızı tanımladığınızı görüyorum. Ancak, bu iki tanımlama birbirini tamamlar nitelikte ve mükerrer değil.
//appsettings.json dosyasında, Serilog'un e-posta sink'i için gerekli olan ayarları tanımlıyorsunuz. Bu ayarlar, e-postaların nereden gönderileceği (fromEmail), nereye gönderileceği (toEmail), hangi mail sunucusunun kullanılacağı (mailServer), hangi portun kullanılacağı (port), SSL'nin kullanılıp kullanılmayacağı (useSsl) ve ağ kimlik bilgileri (networkCredential) gibi bilgileri içerir.
//Program.cs dosyasında ise, bu ayarları appsettings.json dosyasından okuyor ve bir EmailConnectionInfo nesnesi oluşturuyorsunuz. Bu nesne, e-posta ayarlarınızı bir yerde toplar ve bu ayarları programınızın diğer kısımlarında kullanmanızı sağlar.
//Bu iki tanımlama, birbirini tamamlar nitelikte. appsettings.json dosyasındaki ayarlar, uygulamanızın yapılandırmasını belirlerken, Program.cs dosyasındaki kod, bu yapılandırmayı okur ve kullanır. Bu nedenle, bu iki tanımlama mükerrer değil, aksine birbirini tamamlar niteliktedir.
//Ancak, Program.cs dosyasında oluşturduğunuz EmailConnectionInfo nesnesini Serilog yapılandırmanızda kullanmanız gerekiyor. Bu nesneyi oluşturmanızın bir anlamı olması için, bu nesneyi bir yerde kullanmanız gerekiyor. Bu nesneyi bir Serilog e-posta sink'i için kullanmayı planlıyorsanız, bu sink'i Serilog yapılandırmanıza eklemeniz gerekiyor. Bu, log mesajlarını belirttiğiniz e-posta adresine göndermenizi sağlar.