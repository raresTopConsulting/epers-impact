
using Epers.Models.Obiectiv;

namespace EpersBackend.Services.ObiectivService
{
    public interface IObiectiveSalesforceIntegrationService
    {
        int GetSalesforceDataInObiective();
        Obiective? GetObActiveForSalesforce(int idAngajat, DateTime dataIn, string? denumire = null);
    }

}