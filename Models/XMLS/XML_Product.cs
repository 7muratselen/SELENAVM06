using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SELENAVM06.Models.XMLS
{
    public class UrunXmlElemet_XA
    {
        [XmlRoot("products")]
        public class Products
        {
            [XmlElement("product")]
            public List<Product> ProductList { get; set; } = new List<Product>();
        }


        public class Product
        {
            [XmlElement("code", IsNullable = false)]
            [StringLength(50)]
            public string UrunKodu { get; set; } = string.Empty;

            [XmlElement("barcode", IsNullable = false)]
            [StringLength(50)]
            public string UrunBarkodu { get; set; } = string.Empty;

            [XmlElement("name", IsNullable = true)]
            [StringLength(255)]
            public string? UrunAdi { get; set; }

            [XmlElement("brand", IsNullable = true)]
            [StringLength(50)]
            public string? UrunMarka { get; set; }

            [XmlElement("stock", IsNullable = true)]
            public int? UrunAdeti { get; set; }

            [XmlElement("price_list", IsNullable = true)]
            public decimal? UrunFiyati { get; set; }

            [XmlElement("subproducts")]
            public Subproducts? Subproducts { get; set; }
        }

        public class Subproducts
        {
            [XmlElement("subproduct")]
            public List<Subproduct> SubproductList { get; set; } = new List<Subproduct>();
        }

        public class Subproduct
        {
            [XmlElement("code", IsNullable = false)]
            [StringLength(50)]
            public string VaryantKodu { get; set; } = string.Empty;

            [XmlElement("type1", IsNullable = true)]
            [StringLength(25)]
            public string? UrunModeli { get; set; }

            [XmlElement("type2", IsNullable = true)]
            [StringLength(25)]
            public string? UrunRengi { get; set; }

            [XmlElement("stock", IsNullable = true)]
            public int? VaryantAdeti { get; set; }

            [XmlElement("price_list", IsNullable = true)]
            public decimal? VaryantFiyati { get; set; }
        }
    }

    public class UrunXmlElemet_BI
    {
        [XmlElement("urun_kodu", IsNullable = false)]
        [StringLength(50)]
        public string UrunKodu { get; set; } = string.Empty;

        [XmlElement("minumum_siparis_barkodu", IsNullable = false)]
        [StringLength(50)]
        public string UrunBarkodu { get; set; } = string.Empty;

        [XmlElement("varyant_kodu", IsNullable = false)]
        [StringLength(50)]
        public string VaryantKodu { get; set; } = string.Empty;

        [XmlElement("urun_adi", IsNullable = true)]
        [StringLength(255)]
        public string? UrunAdi { get; set; }

        [XmlElement("marka", IsNullable = true)]
        [StringLength(50)]
        public string? UrunMarka { get; set; }

        [XmlElement("urun_rengi", IsNullable = true)]
        [StringLength(25)]
        public string? UrunRengi { get; set; }

        [XmlElement("urun_modeli", IsNullable = true)]
        [StringLength(25)]
        public string? UrunModeli { get; set; }

        [XmlElement("mevcut_stok", IsNullable = true)]
        public int? UrunAdeti { get; set; }

        [XmlElement("kdvli_brutfiyati", IsNullable = true)]
        public decimal? UrunFiyati { get; set; }
    }

    [XmlRoot("Urunler")]
    public class Urunler
    {
        [XmlElement("Urun")]
        public List<UrunXmlElemet_BI> ProductList { get; set; } = new List<UrunXmlElemet_BI>();
    }
}
