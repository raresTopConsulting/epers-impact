using Epers.Models;
using Epers.Models.Nomenclatoare;
using Epers.Models.Users;

namespace EpersBackend.Services.Common
{
    public interface IDrodpwonRepository
    {
        List<DropdownSelection> GetDDUseri();
        List<DropdownSelection> GetDDPosturi();
        List<DropdownSelection> GetDDCompartimente();
        List<DropdownSelection> GetDDCompetente();
        List<DropdownSelection> GetDDDivizii();
        List<DropdownSelection> GetDDLocatii();
        List<DropdownSelection> GetDDRoluri();
        NomenclatoareSelection GetNomenclatoareSelection();
        List<DropdownSelection> GetDDObiective();
        List<DropdownSelection> GetDDCursuri();
        AppDropdownSelections GetAllDropdownSelections();
    }
}

