using SELENAVM06.Models.XMLS;
using System.Xml.Serialization;

namespace SELENAVM06.Services.XMLS
{
  
 
public class XmlReaderService
{
        public UrunXmlElemet_XA.Products ReadXml_XA(string filePath)
        {
            XmlSerializer serializer = new(typeof(UrunXmlElemet_XA.Products));
            using StreamReader reader = new(filePath);
            var result = serializer.Deserialize(reader);
            return result as UrunXmlElemet_XA.Products ?? new();
        }

        public Urunler ReadXml_BI(string filePath)
        {
            XmlSerializer serializer = new(typeof(Urunler));
            using StreamReader reader = new(filePath);
            var result = serializer.Deserialize(reader);
            return result as Urunler ?? new();
        }

    }
}

// Bu kod, XML dosyalarını okuyan ve belirli bir modeli döndüren iki metoda sahip bir
// XmlReaderService sınıfı tanımlar. Her iki metot da, belirli bir dosya yolu alır, bu dosyayı okur ve içeriğini belirli bir model nesnesine dönüştürür.
// Eğer deserialization işlemi başarısız olursa, yeni bir boş nesne döndürülür. Bu, null referans hatalarını önlemek için kullanışlı bir tekniktir.
