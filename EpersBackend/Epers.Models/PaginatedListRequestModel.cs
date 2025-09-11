namespace Epers.Models
{
    public class PaginatedListRequestModel
    {
        public int CurrentPage {get;set; }
        public int ItemsPerPage { get; set; } 
        public int IdRol { get; set; }
        public int? IdFirmaLoggedInUser { get; set; }
        public string? Filter { get; set; } 
        public int? FilterFirma { get; set; }  
        public string? MatricolaLoggedInUser { get; set; }
    }
}