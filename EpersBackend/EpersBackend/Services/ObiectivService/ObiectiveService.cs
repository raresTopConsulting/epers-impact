using AutoMapper;
using Epers.DataAccess;
using Epers.Models.Evaluare;
using Epers.Models.Obiectiv;
using Epers.Models.Pagination;
using Epers.Models.Users;
using EpersBackend.Services.Email;
using EpersBackend.Services.Pagination;
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

        public ObiectiveService(EpersContext epersContext,
            ILogger<Evaluare_competente> logger,
            IMapper mapper,
            IUserService userService,
            IPagination paginationService,
            IConfiguration configuration,
            IEmailSendService emailSendService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _paginationService = paginationService;
            _configuration = configuration;
            _emailSendService = emailSendService;
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

//         public async Task SyncImpactSalesForceWithEpers()
//         {
//             var impactSalesForceEndpoint = _config["Impact:SalesForceApiUrl"] ?? throw new ArgumentNullException("Impact:SalesForceApiUrl not configured");
//             var impactSalesForceToken = _config["Impact:SalesForceApiToken"] ?? throw new InvalidOperationException("Impact:SalesForceApiToken not configured");

//             _httpClient.BaseAddress = new Uri(impactSalesForceEndpoint);
//             _httpClient.DefaultRequestHeaders.Authorization =
//           new AuthenticationHeaderValue("Bearer", impactSalesForceToken);

//             try
//             {
//                 var salesForceResp = await _httpClient.GetAsync(impactSalesForceEndpoint).ConfigureAwait(true);
//                 salesForceResp.EnsureSuccessStatusCode();
//                 var salesForceContent = await salesForceResp.Content.ReadAsStringAsync().ConfigureAwait(true);
//                 var salesForceData = System.Text.Json.JsonSerializer.Deserialize<List<SalesForceImpactModel>>(salesForceContent);

//                 var obiectiveList = new List<Obiective>();

//                 // 1. Prepare the mapping between metric names and NObiective IDs
//                 var metricMap = new Dictionary<string, (int Id, Func<Metrics, int> ValueSelector)>
// {
//     { "leaduri", (3007, m => m.LeaduriTotal) },
//     { "telefoane", (3008, m => m.Telefoane) },
//     { "mesaje", (3009, m => m.Mesaje) },
//     { "întâlniri", (3010, m => m.Intânlniri) },
//     { "revizionari", (3011, m => m.Revizionări) },
//     { "semnariNoi", (3012, m => m.SemnăriNoi) },
//     { "valoareSemnariNoi", (3013, m => m.ValoareSemnăriNoi) },
//     { "cvcCount", (3014, m => m.CvcCount) },
//     { "cvcValue", (3016, m => m.CvcValue) }
// };

//                 // 2. Iterate through each agent and metric
//                 foreach (var agent in salesForceData)
//                 {
//                     foreach (var metric in metricMap)
//                     {
//                         var value = metric.Value.ValueSelector(agent.Metrics);

//                         var obiectiv = new Obiective
//                         {
//                             Denumire = metric.Key,
//                             IdAngajat = agent.AgentId,
//                             DataIn = agent.Start,
//                             DataSf = agent.End,
//                             ValoareRealizata = value.ToString(),
//                             UpdateDate = agent.SyncedAt.UtcDateTime,
//                             IdPost = null, // leave empty if not available
//                             IdCompartiment = null, // leave empty if not available
//                             IdFirma = null, // or set if you have it
//                             IsActive = true,
//                             // Set other properties as needed
//                         };

//                         // Insert obiectiv into your database context
//                         dbContext.Obiective.Add(obiectiv);
//                     }
//                 }

//                 // 3. Save changes to the database
//                 dbContext.SaveChanges(

//             }
//             catch (HttpRequestException httpEx)
//             {
//                 _logger.LogError(httpEx, "Error connecting to SalesForce Impact API");
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "General error in SyncImpactSalesForceWithEpers");
//                 throw;
//             }

//         }
    }
}

