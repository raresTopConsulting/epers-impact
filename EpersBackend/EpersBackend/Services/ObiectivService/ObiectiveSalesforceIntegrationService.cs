
using Epers.DataAccess;
using Epers.Models.Obiectiv;
using Epers.Models.Salesforce;
using Epers.Models.Users;
using EpersBackend.Services.Salesforce;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.ObiectivService
{
    public class ObiectiveSalesforceIntegrationService : IObiectiveSalesforceIntegrationService
    {
        private readonly IEfAgentMetricsRepository _agentMetricsRepo;
        private readonly IUserService _userService;
        private readonly IObiectiveService _obiectiveService;
        private readonly EpersContext _epersContext;

        public ObiectiveSalesforceIntegrationService(IEfAgentMetricsRepository agentMetricsRepo,
                    IUserService userService,
                    IObiectiveService obiectiveService,
                    EpersContext epersContext)
        {
            _agentMetricsRepo = agentMetricsRepo;
            _userService = userService;
            _obiectiveService = obiectiveService;
            _epersContext = epersContext;
        }

        public int GetSalesforceDataInObiective()
        {
            var agentMetrics = _agentMetricsRepo.GetAll();
            int countObAddedOrUpdated = 0;

            foreach (var agentMetric in agentMetrics)
            {
                var user = _userService.Get(agentMetric.Email);

                if (user != null)
                {
                    var obiectiveLeadRamase = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, "Salesforce: Leaduri Ramase");
                    var obiectiveLeadTotal = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, "Salesforce: Leaduri Total");
                    var obiectiveTelefoane = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, "Salesforce: Telefoane");
                    var obiectiveMesaje = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, "Salesforce: Mesaje");
                    var obiectiveIntalniri = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, "Salesforce: Intalniri");

                    if (obiectiveLeadRamase != null)
                    {
                        _obiectiveService.UpdateObiectiv(obiectiveLeadRamase);
                        countObAddedOrUpdated++;
                    }
                    else if (obiectiveLeadRamase == null)
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);
                        newObiectiv.Denumire = "Salesforce: Leaduri Ramase";
                        newObiectiv.ValTarget = agentMetric.LeaduriRamase;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                    if (obiectiveLeadTotal != null)
                    {
                        _obiectiveService.UpdateObiectiv(obiectiveLeadTotal);
                        countObAddedOrUpdated++;
                    }
                    else if (obiectiveLeadTotal == null)
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);
                        newObiectiv.Denumire = "Salesfore: Leaduri Total";
                        newObiectiv.ValTarget = agentMetric.LeaduriTotal;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                    if (obiectiveTelefoane != null)
                    {
                        _obiectiveService.UpdateObiectiv(obiectiveTelefoane);
                        countObAddedOrUpdated++;
                    }
                    else if (obiectiveTelefoane == null)
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);
                        newObiectiv.Denumire = "Salesforce: Telefoane";
                        newObiectiv.ValTarget = agentMetric.Telefoane;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                    if (obiectiveMesaje != null)
                    {
                        _obiectiveService.UpdateObiectiv(obiectiveMesaje);
                        countObAddedOrUpdated++;
                    }
                    else if (obiectiveMesaje == null && agentMetric.Mesaje > 0)
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);

                        newObiectiv.Denumire = "Salesforce: Mesaje";
                        newObiectiv.ValTarget = agentMetric.Mesaje;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                    if (obiectiveIntalniri != null)
                    {
                        _obiectiveService.UpdateObiectiv(obiectiveIntalniri);
                        countObAddedOrUpdated++;
                    }
                    else if (obiectiveIntalniri == null && agentMetric.Intalniri > 0)
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);

                        newObiectiv.Denumire = "Salesforce: Intalniri";
                        newObiectiv.ValTarget = agentMetric.Intalniri;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                }

                else
                {
                    throw new Exception($"Nu s-a gasit angajat cu adresa de Email: {agentMetric.Email} din Salesforce in Epers.");
                }
            }
            return countObAddedOrUpdated;
        }

        public Obiective? GetObActiveForSalesforce(int idAngajat, DateTime dataIn, string? denumire = null)
        {
            Obiective? obiectiv = null;

            obiectiv = _epersContext.Obiective.FirstOrDefault(ob => ob.IdAngajat == idAngajat.ToString()
                && ob.DataIn.HasValue && ob.DataIn.Value.Date == dataIn.Date
                && ob.Denumire == denumire
                && ob.IsActive == true);

            return obiectiv;
        }

        private Obiective BuildObiectiveBase(User user, AgentMetrics agentMetric)
        {
            return new Obiective
            {
                IdAngajat = user.Id.ToString(),
                MatricolaAngajat = user.Matricola,
                DataIn = agentMetric.StartDate,
                DataSf = agentMetric.EndDate,
                ValoareRealizata = "0",
                IsRealizat = false,
                IsActive = true,
                IdCompartiment = user.IdCompartiment,
                IdPost = user.IdPost,
                IdSuperior = user.IdSuperior.HasValue
                                     ? user.IdSuperior.Value.ToString()
                                     : string.Empty,
                MatricolaSuperior = string.IsNullOrWhiteSpace(user.MatricolaSuperior)
                                       ? string.Empty
                                       : user.MatricolaSuperior,
                IdFirma = user.IdFirma,
                UpdateDate = DateTime.UtcNow // use UTC everywhere
            };
        }

    }
}