using AutoMapper;
using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using Epers.Models.PIP;
using Epers.Models.Users;
using EpersBackend.Services.Email;
using EpersBackend.Services.Evaluare;
using EpersBackend.Services.Nomenclatoare;
using EpersBackend.Services.Pagination;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.PIP
{
	public class PIPService : IPIPService
	{
		private readonly IEfPIPRepository _pipRepository;
		private readonly IUserService _userService;
		private readonly IEvaluareService _evaluareService;
		private readonly EpersContext _epersContext;
		private readonly IEfPosturiRepository _posturiRepository;
		private readonly IMapper _mapper;
		private readonly IPagination _paginationService;
		private readonly IEmailSendService _emailSendService;

		public PIPService(IEfPIPRepository pipRepository,
			IUserService userService,
			IEvaluareService evaluareService,
			EpersContext epersContext,
			IEfPosturiRepository posturiRepository,
			IMapper mapper,
			IPagination paginationService,
			IEmailSendService emailSendService)
		{
			_pipRepository = pipRepository;
			_userService = userService;
			_evaluareService = evaluareService;
			_epersContext = epersContext;
			_posturiRepository = posturiRepository;
			_mapper = mapper;
			_paginationService = paginationService;
			_emailSendService = emailSendService;
		}

		public PipDisplayAddEditModel CreateInitial(int idAngajat)
		{
			User subaltern = _userService.Get(idAngajat);
			User superior = _userService.GetSuperior(idAngajat);
			var evalPipData = _evaluareService.GetDateEvaluareForPip(idAngajat);

			var pip = new PipDisplayAddEditModel()
			{
				IdAngajat = idAngajat,
				NumePrenumeAngajat = subaltern.NumePrenume,
				Matricola = subaltern.Matricola,
				IdSuperior = superior.Id,
				NumePrenumeSuperior = superior.NumePrenume,
				MatricolaSuperior = superior.MatricolaSuperior,
				IdPost = subaltern.IdPost,
				IdPostSuperior = superior.IdPost,
				PostAngajat = subaltern.IdPost.HasValue ? _posturiRepository.Get(subaltern.IdPost.Value).Nume : "",
				PostSuperior = superior.IdPost.HasValue ? _posturiRepository.Get(superior.IdPost.Value).Nume : "",
				DataInceputEvaluare = evalPipData.DataInceputEvaluare,
				DataSfarsitEvaluare = evalPipData.DataSfarsitEvaluare,
				CalificativEvaluare = evalPipData.CalificativEvaluare,
				DataInceputPip = DateTime.Now,
				DataSfarsitPip = DateTime.Now.AddMonths(3),
				Anul = DateTime.Now.Year,
				CalificativMinimPip = 0,
				ObiectiveDezvoltare = string.Empty,
				ActiuniSalariat = string.Empty,
				SuportManager = string.Empty,
				AltSuport = string.Empty,
				CalificativFinalPip = 0,
				ObservatiFinalPip = string.Empty,
				DeczieFinalaManager = string.Empty,
				IdStare = 1,
				ObservatiiHr = string.Empty,
			};

			return pip;
		}

		public PipDisplayAddEditModel Get(int idAngajat, int? anul = null)
		{
			if (!anul.HasValue)
			{
				anul = DateTime.Now.Year;
			}

			var pipDisplayData = from pip in _epersContext.PlanInbunatatirePerformante
								 join angajat in _epersContext.User on pip.IdAngajat equals angajat.Id into angajatJoin
								 from angajat in angajatJoin.DefaultIfEmpty()
								 join superior in _epersContext.User on pip.IdSuperior equals superior.Id into superiorJoin
								 from superior in superiorJoin.DefaultIfEmpty()
								 join postAng in _epersContext.NPosturi on pip.IdPost equals postAng.Id into postAngJoin
								 from postAng in postAngJoin.DefaultIfEmpty()
								 join postSup in _epersContext.NPosturi on pip.IdPostSuperior equals postSup.Id into postSupJoin
								 from postSup in postSupJoin.DefaultIfEmpty()
								 join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
								 from nsPip in nsPipJoin.DefaultIfEmpty()

								 where pip.IdAngajat == idAngajat
								 && (pip.DataInceputPip.Year == anul.Value || pip.DataSfarsitPip.Year == anul.Value)

								 select new PipDisplayAddEditModel
								 {
									 Id = pip.Id,
									 Anul = pip.Anul,
									 IdAngajat = pip.IdAngajat,
									 NumePrenumeAngajat = angajat.NumePrenume,
									 Matricola = pip.Matricola,
									 IdSuperior = pip.IdSuperior,
									 NumePrenumeSuperior = superior != null ? superior.NumePrenume : "",
									 MatricolaSuperior = pip.MatricolaSuperior,
									 IdPost = pip.IdPost,
									 IdPostSuperior = pip.IdPostSuperior,
									 PostAngajat = postAng.Nume,
									 PostSuperior = pip.IdPostSuperior.HasValue ? postSup.Nume : "",
									 DataInceputEvaluare = pip.DataInceputEvaluare,
									 DataSfarsitEvaluare = pip.DataSfarsitEvaluare,
									 CalificativEvaluare = pip.CalificativEvaluare,
									 DataInceputPip = pip.DataInceputPip,
									 DataSfarsitPip = pip.DataSfarsitPip,
									 CalificativMinimPip = pip.CalificativMinimPip,
									 ObiectiveDezvoltare = pip.ObiectiveDezvoltare,
									 ActiuniSalariat = pip.ActiuniSalariat,
									 SuportManager = pip.SuportManager,
									 AltSuport = pip.AltSuport,
									 CalificativFinalPip = pip.CalificativFinalPip,
									 ObservatiFinalPip = pip.ObservatiFinalPip,
									 DeczieFinalaManager = pip.DeczieFinalaManager,
									 ObservatiiHr = pip.ObservatiiHr,
									 IdStare = pip.IdStare,
									 DenumireStare = nsPip.Denumire
								 };

			return pipDisplayData.Any() ? pipDisplayData.First() : throw new Exception("PlanInbunatatirePerformante cu idAngajat " + idAngajat + " nu a putut fi gasit");
		}

		public void Add(PipDisplayAddEditModel pipDisplayAddEditModel)
		{
			var pip = _mapper.Map<PlanInbunatatirePerformante>(pipDisplayAddEditModel);
			_pipRepository.Add(pip);
		}

		public void Update(PipDisplayAddEditModel pipDisplayAddEditModel)
		{
			var pip = _mapper.Map<PlanInbunatatirePerformante>(pipDisplayAddEditModel);
			_pipRepository.Update(pip);
		}

		public bool HasPip(int idAngajat, int? anul = null)
		{
			if (!anul.HasValue)
				return _epersContext.PlanInbunatatirePerformante.Any(p => p.IdAngajat == idAngajat);
			else
				return _epersContext.PlanInbunatatirePerformante.Any(p => p.IdAngajat == idAngajat && (p.DataInceputPip.Year == anul.Value || p.DataSfarsitPip.Year == anul.Value));
		}

		public bool HasPipNefinalizat(int idAngajat, int? anul = null)
		{
			if (!anul.HasValue)
				return _epersContext.PlanInbunatatirePerformante.Any(p => p.IdAngajat == idAngajat
					&& p.DataInceputPip != null && (p.CalificativFinalPip == null || p.CalificativFinalPip == 0));
			else
				return _epersContext.PlanInbunatatirePerformante.Any(p => p.IdAngajat == idAngajat
					&& (p.DataInceputPip.Year == anul.Value || p.DataSfarsitPip.Year == anul.Value) && (p.CalificativFinalPip == null || p.CalificativFinalPip == 0));
		}

		public SubalterniDropdown[] SubalterniThatHavePIP(string? matricolaSuperior = null)
		{
			var ddSubalterni = from user in _epersContext.User
							   join post in _epersContext.NPosturi on user.IdPost
							   equals post.Id into postJoin
							   from post in postJoin.DefaultIfEmpty()

							   join pip in _epersContext.PlanInbunatatirePerformante on user.Id
							   equals pip.IdAngajat into pipJoin
							   from pip in pipJoin.DefaultIfEmpty()

							   where pip.IdAngajat == user.Id

							   select new SubalterniDropdown
							   {
								   IdAngajat = user.Id,
								   NumePrenume = user.NumePrenume,
								   MatricolaAngajat = user.Matricola,
								   COR = post.COR != null ? post.COR : "",
								   PostAngajat = post.Nume
							   };

			return ddSubalterni.ToArray();
		}

		public ListaSubalterniPipDisplayModel GetListaSubalterniOngoingPipForAdmin(int currentPage, int itemsPerPage,
			int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}


		public string ActualizareListaSublaterniThatNeedPip(int? anul = null)
		{
			if (!anul.HasValue)
				anul = DateTime.Now.Year;

			var result = _evaluareService.UpdateCalificativFinalEvaluare();

			return result;
		}

		public bool AngajatAreMediaPtPip(int idAngajat, int? anul = null)
		{
			if (!anul.HasValue)
				anul = DateTime.Now.Year;

			var hasMediaForPip = _epersContext.Evaluare_competente
				.Any(e => e.Id_angajat == idAngajat.ToString() && e.Anul == anul && e.CalificativFinalEvaluare.HasValue
				 && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT);
			
			return hasMediaForPip;
		}

		public NStariPIP GetStareActualaPip(int idAngajat, int? anul = null)
		{
			if (!anul.HasValue)
				anul = DateTime.Now.Year;

			if (!HasPip(idAngajat, anul.Value)) return new NStariPIP();

			return _epersContext.PlanInbunatatirePerformante
				.Where(p => p.IdAngajat == idAngajat && p.Anul == anul)
				.Join(_epersContext.NStariPIP, pip => pip.IdStare, stare => stare.Id, (pip, stare) => stare)
				.Select(stare => stare).First();
		}

		public ListaSubalterniPipDisplayModel GetListaSubalterniOngoingPip(string matricolaSuperior, int currentPage, int itemsPerPage,
			int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior
										&& pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
								   && pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
										&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
								 	&& pip.Anul == anul && pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && pip.Anul == anul && user.MatricolaSuperior == matricolaSuperior
										&& pip.IdStare >= 2 && pip.IdStare <= 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}

		public ListaSubalterniPipDisplayModel GetListaSubalterniIstoricPipForAdmin(int currentPage, int itemsPerPage,
			int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare > 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare > 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter
										&& pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare > 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare > 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}

		public ListaSubalterniPipDisplayModel GetListaSubalterniIstoricPip(string matricolaSuperior, int currentPage, int itemsPerPage,
			int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
								   && pip.Anul == anul && pip.IdStare > 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
										&& pip.Anul == anul && pip.IdStare > 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && pip.IdAngajat == user.Id
								 	&& pip.Anul == anul && pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior
										&& pip.IdAngajat == user.Id && pip.Anul == anul
										&& pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
								   && pip.Anul == anul && pip.IdStare > 3
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare
									equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
										&& pip.Anul == anul && pip.IdStare > 3
									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id
								 equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.IdAngajat == user.Id && user.MatricolaSuperior == matricolaSuperior
								 	&& pip.Anul == anul && pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()

									join pip in _epersContext.PlanInbunatatirePerformante on user.Id
									equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()

									join nsPip in _epersContext.NStariPIP on pip.IdStare equals nsPip.Id into nsPipJoin
									from nsPip in nsPipJoin.DefaultIfEmpty()

									where pip.IdAngajat == user.Id && pip.Anul == anul && user.MatricolaSuperior == matricolaSuperior
										&& pip.IdStare > 3
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniPipModel
									{
										IdAngajat = user.Id,
										IdPip = pip.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										IdStarePIP = pip.IdStare,
										DenumireStarePIP = nsPip.Denumire,
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}

		public ListaSubalterniCalificatiPipDisplayModel GetListaSublaterniCalaificatiPipForAdmin(int currentPage, int itemsPerPage,
			int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniCalificatiPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
								 where user.IdFirma == idFirmaFilter
								 && evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where user.IdFirma == idFirmaFilter
									&& evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
								 where user.IdFirma == idFirmaFilter
								 && evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
								 && (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where user.IdFirma == idFirmaFilter
									&& evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
									&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
								 where evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
								 where evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
								 && (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where evalJoin.Any(e => e.Anul == anul && e.CalificativFinalEvaluare.HasValue && e.CalificativFinalEvaluare <= NoteMinimePip.CALIFICATIV_MINIM_NECESITA_EFORT)
								 	&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)

									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniCalificatiPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}

		public ListaSubalterniCalificatiPipDisplayModel GetListaSubalterniPentruAprobarePip(int currentPage, int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null)
		{
			var usersPipList = Array.Empty<ListaSubalterniCalificatiPipModel>();
			var totalRows = 0;
			var pageSettings = new PaginationModel();

			if (idFirmaFilter.HasValue)
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join post in _epersContext.NPosturi on user.IdPost
								 equals post.Id into postJoin
								 from post in postJoin.DefaultIfEmpty()
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where user.IdFirma == idFirmaFilter && pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join post in _epersContext.NPosturi on user.IdPost
								 equals post.Id into postJoin
								 from post in postJoin.DefaultIfEmpty()
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where user.IdFirma == idFirmaFilter && pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
									 && (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where user.IdFirma == idFirmaFilter && pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
										&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(filter))
				{
					totalRows = (from user in _epersContext.User
								 join post in _epersContext.NPosturi on user.IdPost
								 equals post.Id into postJoin
								 from post in postJoin.DefaultIfEmpty()
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
				else
				{
					totalRows = (from user in _epersContext.User
								 join post in _epersContext.NPosturi on user.IdPost
								 equals post.Id into postJoin
								 from post in postJoin.DefaultIfEmpty()
								 join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
								 from pip in pipJoin.DefaultIfEmpty()
								 where pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
									 && (user.NumePrenume.Contains(filter) || user.Matricola == filter)
								 select user.Id).Count();

					pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

					usersPipList = (from user in _epersContext.User
									join post in _epersContext.NPosturi on user.IdPost
									equals post.Id into postJoin
									from post in postJoin.DefaultIfEmpty()
									join pip in _epersContext.PlanInbunatatirePerformante on user.Id equals pip.IdAngajat into pipJoin
									from pip in pipJoin.DefaultIfEmpty()
									join nStPip in _epersContext.NStariPIP on pip.IdStare equals nStPip.Id into nStPipJoin
									from nStPip in nStPipJoin.DefaultIfEmpty()
									where pip.Anul == anul && (pip.IdStare == 1 || pip.IdStare == 3)
										&& (user.NumePrenume.Contains(filter) || user.Matricola == filter)
									orderby user.NumePrenume

									select new ListaSubalterniCalificatiPipModel
									{
										IdAngajat = user.Id,
										IdSuperior = user.IdSuperior,
										MatricolaSuperior = user.MatricolaSuperior,
										Matricola = user.Matricola,
										NumePrenume = user.NumePrenume,
										Username = user.Username,
										PostAngajat = post.Nume,
										Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
										HasPipAprobat = pip.Anul == anul && (pip.IdStare == 2 || pip.IdStare == 4 || pip.IdStare == 5),
										StarePip = nStPip.Denumire
									}).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
				}
			}
			return new ListaSubalterniCalificatiPipDisplayModel
			{
				AngajatiPip = usersPipList,
				CurrentPage = currentPage,
				Pages = pageSettings.Pages
			};
		}
	}
}

