using System;
namespace Epers.Models.Pagination
{
	public class PaginationModel
	{
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
		public int ItemBeginIndex { get; set; }
        public int ItemEndIndex { get; set; }
		public int DisplayedItems { get; set; }
    }
}