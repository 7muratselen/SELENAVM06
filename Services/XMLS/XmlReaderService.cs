using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SELENAVM06.Models.XMLS;

namespace SELENAVM06.Services.XMLS
{
    public class XmlReaderService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        // Gümüş tedarikçi XML için ReadXml_XA metodu
        public async Task<UrunXmlElemet_XA.Products> ReadXml_XA(string pathOrUrl)
        {
            XmlSerializer serializer = new(typeof(UrunXmlElemet_XA.Products));
            using StreamReader reader = new(await GetStreamAsync(pathOrUrl));
            string xmlData = await reader.ReadToEndAsync();
            using StringReader stringReader = new(xmlData);
            var result = serializer.Deserialize(stringReader);
            return result as UrunXmlElemet_XA.Products ?? new();
        }
        // Bigpoint tedarikçisi XML için ReadXml_BI metodu
        public async Task<Urunler> ReadXml_BI(string pathOrUrl)
        {
            XmlSerializer serializer = new(typeof(Urunler));
            using StreamReader reader = new(await GetStreamAsync(pathOrUrl));
            string xmlData = await reader.ReadToEndAsync();
            using StringReader stringReader = new(xmlData);
            var result = serializer.Deserialize(stringReader);
            return result as Urunler ?? new();
        }
        // Genel Methot Yeni Gelen her XML için methodu yukarıya ekleyebilirsin.
        private async Task<Stream> GetStreamAsync(string pathOrUrl)
        {
            try
            {
                if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    return await _httpClient.GetStreamAsync(pathOrUrl);
                }
                else
                {
                    return File.OpenRead(pathOrUrl);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Dosya veya URL okunurken bir hata oluştu: {ex.Message}", ex);
            }
        }
    }
}


// Bu kod, GetStream adında bir yardımcı metot ekler. Bu metot, giriş parametresinin bir URL mi yoksa dosya yolu mu olduğunu kontrol eder ve buna göre bir Stream döndürür.
// Bu Stream, StreamReader'a giriş olarak verilir. Bu şekilde, ReadXml_XA ve ReadXml_BI metodlarınız hem dosya yolu hem de URL ile çalışabilir.
// Bu kod, XML dosyalarını okuyan ve belirli bir modeli döndüren iki metoda sahip bir
// XmlReaderService sınıfı tanımlar. Her iki metot da, belirli bir dosya yolu alır, bu dosyayı okur ve içeriğini belirli bir model nesnesine dönüştürür.
// Eğer deserialization işlemi başarısız olursa, yeni bir boş nesne döndürülür. Bu, null referans hatalarını önlemek için kullanışlı bir tekniktir.
