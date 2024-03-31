namespace SELENAVM06.Entities
{
    public class UrunlerXMLS
    {
        public int Id { get; set; }
        public string UrunKodu { get; set; } = string.Empty;
        public string UrunBarkodu { get; set; } = string.Empty;
        public string VaryantKodu { get; set; } = string.Empty;
        public bool ParentChild { get; set; }
        public string? UrunAdi { get; set; } // UrunAdi property is now nullable
        public string? UrunMarka { get; set; } // UrunMarka property is now nullable
        public string? UrunRengi { get; set; } // UrunRengi property is now nullable
        public string? UrunModeli { get; set; } // UrunModeli property is now nullable
        public int? UrunAdeti { get; set; }
        public decimal? UrunFiyati { get; set; }
    }

    public class Logs
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; } // Level property is now nullable
        public string? Message { get; set; } // Message property is now nullable
        public string? Source { get; set; } // Source property is now nullable
    }

}
