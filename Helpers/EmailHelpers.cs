using System.Net;

namespace SELENAVM06.Helpers
{
    // apsetting içerisinde tanımlama yapıldı program.cs apssetting üzerinden bilgileri okuyor.
    // Bu sınıfın amacı, uygulama içerisinde kullanılacak olan e-posta bağlantı bilgilerini tutmaktır.program içinde şuan kullanımda değil.
    public class EmailConnectionInfo
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string MailServer { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public NetworkCredential NetworkCredentials { get; set; }

        public EmailConnectionInfo()
        {
            // Varsayılan değerlerin atanması
            FromEmail = string.Empty;
            ToEmail = string.Empty;
            MailServer = string.Empty;
            Port = 465;  // SMTP için genel bir varsayılan port
            EnableSsl = true;  // Çoğu SMTP sunucusu SSL gerektirir
            NetworkCredentials = new NetworkCredential("", "");  // Boş bir NetworkCredential oluşturuluyor
        }

        // Yapıcı metodu, parametre alarak özelleştirilmiş değerlerle nesne oluşturmayı sağlar
        public EmailConnectionInfo(string fromEmail, string toEmail, string mailServer, int port, bool enableSsl, NetworkCredential networkCredentials)
        {
            FromEmail = fromEmail;
            ToEmail = toEmail;
            MailServer = mailServer;
            Port = port;
            EnableSsl = enableSsl;
            NetworkCredentials = networkCredentials;
        }
    }
}

// Bu kod, EmailConnectionInfo adında bir sınıf tanımlar. Bu sınıf, bir e-posta bağlantısının detaylarını tutar.
// Bu detaylar arasında gönderenin e-postası (FromEmail), alıcının e-postası (ToEmail), mail sunucusu (MailServer),
// port numarası (Port), SSL'nin etkin olup olmadığı (EnableSsl) ve ağ kimlik bilgileri (NetworkCredentials) bulunur.
// Bu sınıf, bu bilgileri varsayılan değerlerle veya sağlanan değerlerle başlatmak için iki yapıcı metoda sahiptir.

