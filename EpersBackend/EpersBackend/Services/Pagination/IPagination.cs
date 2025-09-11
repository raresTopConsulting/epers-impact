using System;
using Epers.Models.Pagination;

namespace EpersBackend.Services.Pagination
{
	public interface IPagination
	{
        PaginationModel GetPages(int currentPage, int items, int itemsPerPage);
    }
}

