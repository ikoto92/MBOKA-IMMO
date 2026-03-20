using MBOKA_IMMO.src.MbokaImmo.Domain.Enums;

namespace MBOKA_IMMO.src.MbokaImmo.Domain.Entities
{
    public class Photo
    {
        public int IdPhoto { get; set; }
        public int EntiteId { get; set; }
        public EntiteTypeEnum EntiteType { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Ordre { get; set; } = 0;
        public DateTime DateUpload { get; set; } = DateTime.UtcNow;
    }
}
