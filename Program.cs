using Microsoft.EntityFrameworkCore;
using Serilog;
using SELENAVM06.Data;  // DB context'inizin oldu�u namespace
using SELENAVM06.Services.XMLS;  // XmlReaderService'in oldu�u namespace

namespace SELENAVM06
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Uygulama hizmetlerini ekleyin
            builder.Services.AddControllersWithViews();

            // SelenavmDbContext i�in DbContext servisini ekleyin
            builder.Services.AddDbContext<SelenavmDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQLSelenavmConnection")));

            // XmlReaderService i�in servisi ekleyin
            builder.Services.AddScoped<XmlReaderService>();

            var app = builder.Build();

            // HTTP istek hatt�n� yap�land�r�n
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






//appsettings.json dosyas�nda ve Program.cs dosyas�nda e-posta ayarlar�n�z� tan�mlad���n�z� g�r�yorum. Ancak, bu iki tan�mlama birbirini tamamlar nitelikte ve m�kerrer de�il.
//appsettings.json dosyas�nda, Serilog'un e-posta sink'i i�in gerekli olan ayarlar� tan�ml�yorsunuz. Bu ayarlar, e-postalar�n nereden g�nderilece�i (fromEmail), nereye g�nderilece�i (toEmail), hangi mail sunucusunun kullan�laca�� (mailServer), hangi portun kullan�laca�� (port), SSL'nin kullan�l�p kullan�lmayaca�� (useSsl) ve a� kimlik bilgileri (networkCredential) gibi bilgileri i�erir.
//Program.cs dosyas�nda ise, bu ayarlar� appsettings.json dosyas�ndan okuyor ve bir EmailConnectionInfo nesnesi olu�turuyorsunuz. Bu nesne, e-posta ayarlar�n�z� bir yerde toplar ve bu ayarlar� program�n�z�n di�er k�s�mlar�nda kullanman�z� sa�lar.
//Bu iki tan�mlama, birbirini tamamlar nitelikte. appsettings.json dosyas�ndaki ayarlar, uygulaman�z�n yap�land�rmas�n� belirlerken, Program.cs dosyas�ndaki kod, bu yap�land�rmay� okur ve kullan�r. Bu nedenle, bu iki tan�mlama m�kerrer de�il, aksine birbirini tamamlar niteliktedir.
//Ancak, Program.cs dosyas�nda olu�turdu�unuz EmailConnectionInfo nesnesini Serilog yap�land�rman�zda kullanman�z gerekiyor. Bu nesneyi olu�turman�z�n bir anlam� olmas� i�in, bu nesneyi bir yerde kullanman�z gerekiyor. Bu nesneyi bir Serilog e-posta sink'i i�in kullanmay� planl�yorsan�z, bu sink'i Serilog yap�land�rman�za eklemeniz gerekiyor. Bu, log mesajlar�n� belirtti�iniz e-posta adresine g�ndermenizi sa�lar.