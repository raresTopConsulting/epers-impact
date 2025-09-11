using System;
using Epers.Models.Pagination;

namespace EpersBackend.Services.Pagination
{
	public class Pagination: IPagination
	{
        public Pagination()
        {
        }

        public PaginationModel GetPages(int currentPage,int items, int itemsPerPage)
		{
			return new PaginationModel
			{
				Pages = (int)Math.Ceiling(items / (decimal)itemsPerPage),
				CurrentPage = currentPage,
				ItemBeginIndex = (currentPage - 1) * itemsPerPage,
				ItemEndIndex = currentPage * itemsPerPage,
				DisplayedItems = (currentPage * itemsPerPage) - ((currentPage - 1) * itemsPerPage)
            };
        }
	}
}
