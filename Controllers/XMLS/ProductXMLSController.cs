using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SELENAVM06.Models.XMLS;
using SELENAVM06.Services.XMLS;
using SELENAVM06.Data;
using SELENAVM06.Entities;
using System.Diagnostics;
using Serilog;


namespace SELENAVM06.Controllers.XMLS
{
    public class ProductXMLSController : Controller
    {
        private readonly XmlReaderService _xmlReaderService;
        private readonly SelenavmDbContext _dbContext;
        private readonly ILogger<ProductXMLSController> _logger;

        public ProductXMLSController(XmlReaderService xmlReaderService, ILogger<ProductXMLSController> logger, SelenavmDbContext dbContext)
        {
            _xmlReaderService = xmlReaderService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<UrunlerXMLS> allUrunler = new List<UrunlerXMLS>();

            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                _logger.LogInformation("XML_XA dosyası okunuyor, lütfen bekleyin...");

                // XML_XA Gumush Tedarikçisinin XML dosyasını oku
                // Yerel dosyadan okuma kodu yorum satırı haline getirildi
                // UrunXmlElemet_XA.Products XML_XA = _xmlReaderService.ReadXml_XA(@"C:/Code/XML/XA_Ornek.xml");

                // URL'den okuma kodu eklendi
                UrunXmlElemet_XA.Products XML_XA = await _xmlReaderService.ReadXml_XA("https://www.gumush.com/xml/?R=6575&K=bfb9&AltUrun=1&TamLink=1&Dislink=1&Imgs=1&start=0&limit=99999&pass=8RRqo6ZF&nocache&currency=TL&Stok=1");

                _logger.LogInformation("XML_XA dosyası başarıyla okundu. Veritabanına kaydediliyor...");

                stopwatch.Stop();
                _logger.LogInformation("XML_XA dosyası okuma süresi: {ElapsedSeconds} saniye", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Reset();

                // Veri tabanı dbo.URUNLERXMLS Tüm verileri yazmadan önce sil.
                allUrunler = _dbContext.UrunlerXMLS.ToList();
                _dbContext.UrunlerXMLS.RemoveRange(allUrunler);
                _dbContext.SaveChanges();

                stopwatch.Start();

                // XML_XA Gumush Tedarikçisinin XML dosyasından gelen verileri işle ve veritabanına yaz
                foreach (var product in XML_XA.ProductList)
                {
                    // Create a new database product for the main product
                    var mainDbProduct = new UrunlerXMLS
                    {
                        UrunKodu = "XA_" + product.UrunKodu + "_" + product.UrunBarkodu,
                        UrunBarkodu = product.UrunBarkodu,
                        UrunAdi = product.UrunAdi ?? "Default Name",
                        UrunMarka = product.UrunMarka ?? "Default Brand",
                        UrunAdeti = product.UrunAdeti,
                        UrunFiyati = product.UrunFiyati,
                        ParentChild = true // Set ParentChild property to true for main products
                    };

                    // Add the main product to the database
                    _dbContext.UrunlerXMLS.Add(mainDbProduct);

                    // Check if the product has Subproducts
                    if (product.Subproducts?.SubproductList != null && product.Subproducts.SubproductList.Count > 0)
                    {
                        // If it does, iterate over the Subproduct list
                        foreach (var subproduct in product.Subproducts.SubproductList)
                        {
                            // Create a new database product for each subproduct
                            var dbProduct = new UrunlerXMLS
                            {
                                UrunKodu = "XA_" + product.UrunKodu + "_" + product.UrunBarkodu,
                                UrunBarkodu = product.UrunBarkodu,
                                UrunAdi = product.UrunAdi ?? "Default Name",
                                UrunMarka = product.UrunMarka ?? "Default Brand",
                                UrunAdeti = subproduct.VaryantAdeti,
                                UrunFiyati = subproduct.VaryantFiyati,
                                VaryantKodu = "XA_" + subproduct.VaryantKodu + "_" + product.UrunBarkodu + "_" + subproduct.UrunModeli,
                                UrunRengi = subproduct.UrunRengi ?? "Default Color",
                                UrunModeli = subproduct.UrunModeli ?? "Default Model",
                                ParentChild = false // Set ParentChild property to false for subproducts
                            };

                            // Add the subproduct to the database
                            _dbContext.UrunlerXMLS.Add(dbProduct);
                        }
                    }
                }

                // Save changes to the database
                _dbContext.SaveChanges();

                _logger.LogInformation("XML_XA dosyasındaki veriler başarıyla veritabanına kaydedildi.");

                stopwatch.Stop();
                _logger.LogInformation("XML_XA dosyası okuma süresi: {ElapsedSeconds} saniye", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Reset();

                stopwatch.Start();
                _logger.LogInformation("XML_BI dosyası okunuyor, lütfen bekleyin...");

                // XML_BI Bigpoint Tedarikçisinin XML dosyasını oku
                // Yerel dosyadan okuma kodu yorum satırı haline getirildi
                // Urunler XML_BI = _xmlReaderService.ReadXml_BI(@"C:/Code/XML/BI_Ornek.xml");

                // URL'den okuma kodu eklendi
                Urunler XML_BI = await _xmlReaderService.ReadXml_BI("https://www.bigpoint.com.tr/xml.php?bayi=a4v274a4&minumum=siparis");


                _logger.LogInformation("XML_BI dosyası başarıyla okundu. Veritabanına kaydediliyor...");

                // XML_BI Bigpoint Tedarikçisinin dosyasından gelen verileri işle ve veritabanına yaz
                foreach (var product in XML_BI.ProductList)
                {
                    // Veritabanı model nesnesini oluştur
                    var dbProduct = new UrunlerXMLS
                    {
                        UrunKodu = "BI_" + product.UrunKodu + "_" + product.UrunBarkodu,
                        UrunBarkodu = product.UrunBarkodu,
                        VaryantKodu = product.VaryantKodu,
                        UrunAdi = product.UrunAdi ?? "Default Name",
                        UrunMarka = product.UrunMarka ?? "Default Brand",
                        UrunRengi = product.UrunRengi ?? "Default Color",
                        UrunModeli = product.UrunModeli ?? "Default Model",
                        UrunAdeti = product.UrunAdeti,
                        UrunFiyati = product.UrunFiyati,
                        ParentChild = true // Set ParentChild property to true for main products
                    };

                    // Veritabanına ekle
                    _dbContext.UrunlerXMLS.Add(dbProduct);
                }

                // Değişiklikleri kaydet
                _dbContext.SaveChanges();

                _logger.LogInformation("XML_BI dosyasındaki veriler başarıyla veritabanına kaydedildi.");

                stopwatch.Stop();
                _logger.LogInformation("XML_BI dosyası okuma süresi: {ElapsedSeconds} saniye", stopwatch.Elapsed.TotalSeconds);
                stopwatch.Reset();
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda çalışacak kod
                _logger.LogError("An error occurred: {ErrorMessage}", ex.Message);
            }

            return View(allUrunler);
        }

    }
}

// ProductXMLSController sınıfınız, XML dosyalarını okuyan ve bu verileri veritabanına kaydeden bir ASP.NET Core MVC Controller'dır.
//Index metodu, iki XML dosyasını okur (XA_Ornek.xml ve BI_Ornek.xml), bu dosyalardan gelen verileri işler ve UrunlerXMLS nesnelerine dönüştürür. Bu nesneler daha sonra veritabanına eklenir.
//Her bir XML dosyası için, önce dosya okunur ve veriler deserialize edilir. Daha sonra, bu veriler üzerinde döngü başlatılır ve her bir ürün için bir UrunlerXMLS nesnesi oluşturulur. Bu nesne, ürünün özelliklerini temsil eder ve veritabanına eklenir.
//Eğer bir ürünün alt ürünleri varsa (yani Subproducts özelliği null değilse ve SubproductList özelliği boş değilse), bu alt ürünler üzerinde de döngü başlatılır ve her bir alt ürün için de bir UrunlerXMLS nesnesi oluşturulur ve veritabanına eklenir.
//Bu işlemler, her iki XML dosyası için de gerçekleştirilir. Her bir dosya işlemi tamamlandığında, işlemin ne kadar sürdüğüne dair bir log mesajı yazılır.
//Eğer bu süreç sırasında bir hata oluşursa, hata yakalanır ve hata mesajı loglanır.
//Son olarak, tüm UrunlerXMLS nesneleri bir liste olarak döndürülür ve bu liste, View'a gönderilir.