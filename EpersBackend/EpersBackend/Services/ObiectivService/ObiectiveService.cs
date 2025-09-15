using AutoMapper;
using Epers.DataAccess;
using Epers.Models.Evaluare;
using Epers.Models.Obiectiv;
using Epers.Models.Pagination;
using Epers.Models.Users;
using EpersBackend.Services.Email;
using EpersBackend.Services.Pagination;
using EpersBackend.Services.Salesforce;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.ObiectivService
{
    public class ObiectiveService : IObiectiveService
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<Evaluare_competente> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPagination _paginationService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSendService _emailSendService;
        private readonly IEfAgentMetricsRepository _agentMetricsRepo;

        public ObiectiveService(EpersContext epersContext,
            ILogger<Evaluare_competente> logger,
            IMapper mapper,
            IUserService userService,
            IPagination paginationService,
            IConfiguration configuration,
            IEfAgentMetricsRepository agentMetricsRepo,
            IEmailSendService emailSendService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _paginationService = paginationService;
            _configuration = configuration;
            _emailSendService = emailSendService;
            _agentMetricsRepo = agentMetricsRepo;
        }

        public void Add(SetareObiective setareObiective, int[]? idAngajatiSelectati = null)
        {
            var obObject = new Obiective();

            if (idAngajatiSelectati != null)
            {
                foreach (var idAngSelectat in idAngajatiSelectati)
                {
                    var angajat = _userService.Get(idAngSelectat);

                    foreach (var obTemplate in setareObiective.ObiectivTemplate)
                    {
                        obTemplate.DataSf = CalcuateDataSfarsit(obTemplate.DataIn, obTemplate.Frecventa);

                        var obieciv = _mapper.Map(obTemplate, obObject);

                        obieciv.IdAngajat = angajat.Id.ToString();
                        obieciv.MatricolaAngajat = angajat.Matricola;
                        obieciv.IdSuperior = angajat.IdSuperior.HasValue ? angajat.IdSuperior.Value.ToString() : "";
                        obieciv.MatricolaSuperior = !string.IsNullOrWhiteSpace(angajat.MatricolaSuperior) ? angajat.MatricolaSuperior : "";
                        obieciv.IdCompartiment = angajat.IdCompartiment;
                        obieciv.IdPost = angajat.IdPost;
                        obieciv.IsActive = true;
                        obieciv.IdFirma = angajat.IdFirma;

                        obieciv.UpdateId = setareObiective.UpdateId;
                        obieciv.UpdateDate = DateTime.Now;
                        obieciv.Id = 0;

                        InsertObiectiv(obieciv);
                    }
                }

                if (idAngajatiSelectati == null)
                {
                    foreach (var obTemplate in setareObiective.ObiectivTemplate)
                    {
                        var obieciv = _mapper.Map(obTemplate, obObject);

                        obieciv.IdAngajat = setareObiective.IdAngajat;
                        obieciv.MatricolaAngajat = setareObiective.MatricolaAngajat;
                        obieciv.IdSuperior = setareObiective.IdSuperior;
                        obieciv.MatricolaSuperior = setareObiective.MatricolaSuperior;
                        obieciv.IdCompartiment = setareObiective.IdCompartiment;
                        obieciv.IdPost = setareObiective.IdPost;
                        obieciv.UpdateId = setareObiective.UpdateId;
                        obieciv.UpdateDate = DateTime.Now;
                        obieciv.Id = 0;

                        InsertObiectiv(obieciv);
                    }
                }
            }
        }

        private static DateTime? CalcuateDataSfarsit(DateTime? dataIn, string frecventa)
        {
            if (dataIn == null) return null;
            return frecventa switch
            {
                "Saptamanal" => dataIn.Value.AddDays(7 - 1),
                "28 de zile" => dataIn.Value.AddDays(28),
                "Lunar" => dataIn.Value.AddMonths(1).AddDays(-1),
                "4 luni" => dataIn.Value.AddMonths(4).AddDays(-1),
                "Semestrial" => dataIn.Value.AddMonths(6).AddDays(-1),
                "Anual" => dataIn.Value.AddYears(1).AddDays(-1),
                _ => null,
            };
        }

        public Obiective[] GetObiectiveActuale(int idAngajat, string? filter = null)
        {
            var obActive = new List<Obiective>();

            if (string.IsNullOrWhiteSpace(filter))
                obActive = _epersContext.Obiective.Where(ob => ob.IdAngajat == idAngajat.ToString()
                    && ob.IsActive == true).OrderBy(ob => ob.DataIn).ToList();
            else
                obActive = _epersContext.Obiective.Where(ob => ob.IdAngajat == idAngajat.ToString()
                    && ob.IsActive == true && ob.Denumire.Contains(filter)).OrderBy(ob => ob.DataIn).ToList();

            return obActive.ToArray();
        }

        public Obiective[] GetIstoricObiective(int idAngajat, string? filter = null)
        {
            var istoricOb = new List<Obiective>();

            if (string.IsNullOrWhiteSpace(filter))
                istoricOb = _epersContext.Obiective.Where(ob => ob.IdAngajat == idAngajat.ToString()
                    && (ob.IsActive.HasValue && ob.IsActive.Value == false)).OrderBy(ob => ob.DataIn).ToList();
            else
                istoricOb = _epersContext.Obiective.Where(ob => ob.IdAngajat == idAngajat.ToString()
                    && (ob.IsActive.HasValue && ob.IsActive.Value == false)
                    && ob.Denumire.Contains(filter)).OrderBy(ob => ob.DataIn).ToList();

            return istoricOb.ToArray();
        }

        public void Evaluare(Obiective[] obiective)
        {
            foreach (var ob in obiective)
            {
                if (!string.IsNullOrWhiteSpace(ob.ValoareRealizata))
                {
                    decimal.TryParse(ob.ValoareRealizata, out decimal valRealizata);

                    if (valRealizata >= ob.ValTarget)
                        ob.IsRealizat = true;
                    else
                        ob.IsRealizat = false;
                }
                ob.IsActive = false;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.Obiective.Update(ob);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Obiective: Evaluare");
                    throw;
                }
            }
        }

        public ObiectiveListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniObi = Array.Empty<SubalterniDropdown>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.IdFirma == idFirmaFilter
                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter
                        && (us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.IdFirma == idFirmaFilter
                                          && (user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter))
                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter)

                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new ObiectiveListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniObi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ObiectiveListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaLoggedInUser = null, string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniObi = Array.Empty<SubalterniDropdown>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter && us.MatricolaSuperior == matricolaLoggedInUser);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaLoggedInUser

                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == matricolaLoggedInUser && us.IdFirma == idFirmaFilter
                        && (us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaLoggedInUser
                                          && (user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter))
                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == matricolaLoggedInUser);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.MatricolaSuperior == matricolaLoggedInUser

                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == us.Matricola
                        && (us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniObi = (from user in _epersContext.User
                                          join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                          from post in postJoin.DefaultIfEmpty()
                                          join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                          from pip in pipJoin.DefaultIfEmpty()
                                          orderby user.NumePrenume
                                          where user.MatricolaSuperior == matricolaLoggedInUser
                                          && (user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter))
                                          select new SubalterniDropdown
                                          {
                                              IdAngajat = user.Id,
                                              NumePrenume = user.NumePrenume,
                                              MatricolaAngajat = user.Matricola,
                                              COR = post.COR != null ? post.COR : "",
                                              PostAngajat = post.Nume,
                                              HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                          }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new ObiectiveListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniObi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ObiectiveListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null)
        {
            var listaSubalterniObi = Array.Empty<SubalterniDropdown>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();
            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniObi = (from user in _epersContext.User
                                      join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                      from post in postJoin.DefaultIfEmpty()
                                      join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                      from pip in pipJoin.DefaultIfEmpty()
                                      orderby user.NumePrenume
                                      where user.IdFirma == loggedInUserFirma
                                      select new SubalterniDropdown
                                      {
                                          IdAngajat = user.Id,
                                          NumePrenume = user.NumePrenume,
                                          MatricolaAngajat = user.Matricola,
                                          COR = post.COR != null ? post.COR : "",
                                          PostAngajat = post.Nume,
                                          HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                      }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma
                    && (us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter)));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniObi = (from user in _epersContext.User
                                      join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                      from post in postJoin.DefaultIfEmpty()
                                      join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                      from pip in pipJoin.DefaultIfEmpty()
                                      orderby user.NumePrenume
                                      where user.IdFirma == loggedInUserFirma
                                      && (user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter))
                                      select new SubalterniDropdown
                                      {
                                          IdAngajat = user.Id,
                                          NumePrenume = user.NumePrenume,
                                          MatricolaAngajat = user.Matricola,
                                          COR = post.COR != null ? post.COR : "",
                                          PostAngajat = post.Nume,
                                          HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                      }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new ObiectiveListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniObi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ObiectiveListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? matricolaLoggedInUser = null, string? filter = null)
        {
            var listaSubalterniObi = Array.Empty<SubalterniDropdown>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma && us.MatricolaSuperior == matricolaLoggedInUser);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniObi = (from user in _epersContext.User
                                      join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                      from post in postJoin.DefaultIfEmpty()
                                      join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                      from pip in pipJoin.DefaultIfEmpty()
                                      orderby user.NumePrenume
                                      where user.IdFirma == loggedInUserFirma && user.MatricolaSuperior == matricolaLoggedInUser
                                      select new SubalterniDropdown
                                      {
                                          IdAngajat = user.Id,
                                          NumePrenume = user.NumePrenume,
                                          MatricolaAngajat = user.Matricola,
                                          COR = post.COR != null ? post.COR : "",
                                          PostAngajat = post.Nume,
                                          HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                      }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == matricolaLoggedInUser && us.IdFirma == loggedInUserFirma
                    && (us.Matricola == filter || us.NumePrenume.Contains(filter) || us.Username.Contains(filter)));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniObi = (from user in _epersContext.User
                                      join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                      from post in postJoin.DefaultIfEmpty()
                                      join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
                                      from pip in pipJoin.DefaultIfEmpty()
                                      orderby user.NumePrenume
                                      where user.IdFirma == loggedInUserFirma && user.MatricolaSuperior == matricolaLoggedInUser
                                      && (user.Matricola == filter || user.NumePrenume.Contains(filter) || user.Username.Contains(filter))
                                      select new SubalterniDropdown
                                      {
                                          IdAngajat = user.Id,
                                          NumePrenume = user.NumePrenume,
                                          MatricolaAngajat = user.Matricola,
                                          COR = post.COR != null ? post.COR : "",
                                          PostAngajat = post.Nume,
                                          HideStartPip = pip.IdAngajat == user.Id && (pip.DataInceputPip.Year == DateTime.Now.Year || pip.DataSfarsitPip.Year == DateTime.Now.Year)
                                      }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new ObiectiveListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniObi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public void InsertObiectiv(Obiective obieciv)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Obiective.Add(obieciv);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Obiective: Add");
                throw;
            }
        }

        public void UpdateObiectiv(Obiective obieciv)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Obiective.Update(obieciv);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Obiective: Add");
                throw;
            }
        }

        public void InsertOrUpdateObiectiv(Obiective obieciv)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    var existingObiectiv = _epersContext.Obiective.FirstOrDefault(ob => ob.Id == obieciv.Id);
                    if (existingObiectiv != null)
                    {
                        // Update existing record
                        _epersContext.Entry(existingObiectiv).CurrentValues.SetValues(obieciv);
                    }
                    else
                    {
                        // Insert new record
                        _epersContext.Obiective.Add(obieciv);
                    }

                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Obiective: InsertOrUpdate");
                throw;
            }
        }

    }
}

