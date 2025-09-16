
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
                if (user == null)
                {
                    throw new Exception($"Nu s-a gasit angajat cu adresa de Email: {agentMetric.Email} din Salesforce in Epers.");
                }

                var obiectivTypes = new List<(string Denumire, decimal? Value)>
                {
                    ("Salesforce: Leaduri Ramase", agentMetric.LeaduriRamase),
                    ("Salesforce: Leaduri Total", agentMetric.LeaduriTotal),
                    ("Salesforce: Telefoane", agentMetric.Telefoane),
                    ("Salesforce: Mesaje", agentMetric.Mesaje),
                    ("Salesforce: Intalniri", agentMetric.Intalniri),
                    ("Salesforce: Semnari Noi", agentMetric.SemnariNoi),
                    ("Salesforce: Valoare Semnari Noi", agentMetric.ValoareSemnariNoi),
                    ("Salesforce: Cvc Count", agentMetric.CvcCount),
                    ("Salesforce: Cvc Value", agentMetric.CvcValue)
                };

                foreach (var (denumire, value) in obiectivTypes)
                {
                    var existingObiectiv = GetObActiveForSalesforce(user.Id, agentMetric.StartDate, denumire);
                    if (existingObiectiv != null)
                    {
                        existingObiectiv.ValTarget = (decimal?)value ?? null;
                        _obiectiveService.UpdateObiectiv(existingObiectiv);
                        countObAddedOrUpdated++;
                    }
                    else
                    {
                        var newObiectiv = BuildObiectiveBase(user, agentMetric);
                        newObiectiv.Denumire = denumire;
                        newObiectiv.ValTarget = (decimal?)value ?? null;
                        _obiectiveService.InsertObiectiv(newObiectiv);
                        countObAddedOrUpdated++;
                    }
                }
            }
            return countObAddedOrUpdated;
        }

        public Obiective? GetObActiveForSalesforce(int idAngajat, DateTime dataIn, string? denumire = null)
        {
            Obiective? obiectiv = null;

            obiectiv = _epersContext.Obiective.FirstOrDefault(ob => ob.IdAngajat == idAngajat.ToString()
                && ob.DataIn.HasValue && ob.DataIn.Value.Date.Day == dataIn.Date.Day && ob.DataIn.Value.Date.Month == dataIn.Date.Month && ob.DataIn.Value.Date.Year == dataIn.Date.Year
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