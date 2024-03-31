using Microsoft.EntityFrameworkCore;
using Serilog;
using SELENAVM06.Data;  // DB context'inizin olduðu namespace
using SELENAVM06.Services.XMLS;  // XmlReaderService'in olduðu namespace

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

            // HTTP istek hattýný yapýlandýrýn
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






//appsettings.json dosyasýnda ve Program.cs dosyasýnda e-posta ayarlarýnýzý tanýmladýðýnýzý görüyorum. Ancak, bu iki tanýmlama birbirini tamamlar nitelikte ve mükerrer deðil.
//appsettings.json dosyasýnda, Serilog'un e-posta sink'i için gerekli olan ayarlarý tanýmlýyorsunuz. Bu ayarlar, e-postalarýn nereden gönderileceði (fromEmail), nereye gönderileceði (toEmail), hangi mail sunucusunun kullanýlacaðý (mailServer), hangi portun kullanýlacaðý (port), SSL'nin kullanýlýp kullanýlmayacaðý (useSsl) ve að kimlik bilgileri (networkCredential) gibi bilgileri içerir.
//Program.cs dosyasýnda ise, bu ayarlarý appsettings.json dosyasýndan okuyor ve bir EmailConnectionInfo nesnesi oluþturuyorsunuz. Bu nesne, e-posta ayarlarýnýzý bir yerde toplar ve bu ayarlarý programýnýzýn diðer kýsýmlarýnda kullanmanýzý saðlar.
//Bu iki tanýmlama, birbirini tamamlar nitelikte. appsettings.json dosyasýndaki ayarlar, uygulamanýzýn yapýlandýrmasýný belirlerken, Program.cs dosyasýndaki kod, bu yapýlandýrmayý okur ve kullanýr. Bu nedenle, bu iki tanýmlama mükerrer deðil, aksine birbirini tamamlar niteliktedir.
//Ancak, Program.cs dosyasýnda oluþturduðunuz EmailConnectionInfo nesnesini Serilog yapýlandýrmanýzda kullanmanýz gerekiyor. Bu nesneyi oluþturmanýzýn bir anlamý olmasý için, bu nesneyi bir yerde kullanmanýz gerekiyor. Bu nesneyi bir Serilog e-posta sink'i için kullanmayý planlýyorsanýz, bu sink'i Serilog yapýlandýrmanýza eklemeniz gerekiyor. Bu, log mesajlarýný belirttiðiniz e-posta adresine göndermenizi saðlar.