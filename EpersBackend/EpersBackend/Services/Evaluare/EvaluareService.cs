using System.Data;
using Epers.DataAccess;
using Epers.Models.Afisare;
using Epers.Models.Competente;
using Epers.Models.Evaluare;
using Epers.Models.Pagination;
using Epers.Models.Users;
using EpersBackend.Services.Common;
using EpersBackend.Services.Competente;
using EpersBackend.Services.Pagination;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.Evaluare
{
    public class EvaluareService : IEvaluareService
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<Evaluare_competente> _logger;
        private readonly ICompetenteService _competenteService;
        private readonly IUserService _userService;
        private readonly IPagination _paginationService;
        private readonly IConversionHelper _conversionHelper;

        public EvaluareService(EpersContext epersContext,
            ILogger<Evaluare_competente> logger,
            ICompetenteService competenteService,
            IUserService userService,
            IPagination paginationService,
            IConversionHelper conversionHelper)
        {
            _epersContext = epersContext;
            _logger = logger;
            _competenteService = competenteService;
            _userService = userService;
            _paginationService = paginationService;
            _conversionHelper = conversionHelper;
        }

        private List<Evaluare_competente> GetEvaluariNefinalizate(string matricola, int? anul = null)
        {
            var userEvalList = new List<Evaluare_competente>();

            userEvalList = !anul.HasValue ? _epersContext.Evaluare_competente
                    .Where(eval => eval.Matricola == matricola && eval.Flag_finalizat == 0).ToList()
                    : _epersContext.Evaluare_competente
                    .Where(eval => eval.Matricola == matricola && eval.Flag_finalizat == 0 && eval.Anul == anul).ToList();

            return userEvalList.Any() ? userEvalList : new List<Evaluare_competente>();
        }

        private List<Evaluare_competente> GetEvaluari(string matricola, int? anul = null)
        {
            var userEvalList = new List<Evaluare_competente>();

            userEvalList = !anul.HasValue ? _epersContext.Evaluare_competente
                    .Where(eval => eval.Matricola == matricola).Select(eval => eval).ToList()
                    : _epersContext.Evaluare_competente
                    .Where(eval => eval.Matricola == matricola && eval.Anul == anul).Select(eval => eval).ToList();

            return userEvalList.Any() ? userEvalList : new List<Evaluare_competente>();
        }

        public AfisareSkillsEvalModel GetEvalTemplate(int idAngajat, int? anul = null)
        {
            AfisareSkillsEvalModel afisareEval = new();
            User user = _userService.Get(idAngajat);
            List<RelevantSkillsModel> userRelevantSkills = _competenteService.Get(user.IdPost);
            List<Evaluare_competente> evaluari = GetEvaluari(user.Matricola, anul);
            // List<Evaluare_competente> evaluariNefinalizate = GetEvaluariNefinalizate(user.Matricola, anul);

            afisareEval.IdPost = user.IdPost.HasValue ? user.IdPost.Value : 0;
            afisareEval.FlagFinalizat = evaluari.Any(x => x.Flag_finalizat == 1);

            int indxEval = 0;
            foreach (var item in userRelevantSkills)
            {
                afisareEval.DateEval.Add(new AfisareSkillsEvalModel.EvalArray()
                {
                    IdSkill = item.Id_Skill,
                    DenumireSkill = item.Denumire,
                    DetaliiSkill = item.Detalii != null ? item.Detalii : "",
                    Ideal = item.Ideal
                });

                if (evaluari != null && evaluari.Count > 0)
                {
                    int valoareIndiv = 0;
                    int valoare = 0;
                    int valoareFinala = 0;

                    bool convertValIndiv = int.TryParse(evaluari[indxEval].Valoare_indiv, out valoareIndiv);
                    bool convertVal = int.TryParse(evaluari[indxEval].Valoare, out valoare);
                    bool convertValFinala = int.TryParse(evaluari[indxEval].Valoare_fin, out valoareFinala);

                    afisareEval.DateEval[indxEval].ValIndiv = convertValIndiv == true ? valoareIndiv : null;
                    afisareEval.DateEval[indxEval].Val = convertVal == true ? valoare : null;
                    afisareEval.DateEval[indxEval].ValFin = convertValFinala == true ? valoareFinala : null;

                    afisareEval.DateEval[indxEval].Obs = evaluari[indxEval].Observatie != null ?
                        evaluari[indxEval].Observatie : "";

                    afisareEval.DateEval[indxEval].DataEvalFinala = evaluari[indxEval].Data_EvalFinala;
                }

                indxEval++;
            }

            return afisareEval;
        }

        public AfisareSkillsEvalModel GetIstoricEvalTemplate(int idAngajat, int? anul = null)
        {
            AfisareSkillsEvalModel afisareEval = new();
            User user = _userService.Get(idAngajat);
            List<Evaluare_competente> evaluariAnulSelectat = GetEvaluari(user.Matricola, anul);

            afisareEval.IdPost = user.IdPost.HasValue ? user.IdPost.Value : 0;

            int indxEval = 0;
            int sumValFin = 0;

            if (evaluariAnulSelectat != null && evaluariAnulSelectat.Count > 0)
            {
                foreach (var eval in evaluariAnulSelectat)
                {
                    if (eval.Id_post != null)
                    {
                        var competentaPost = _competenteService.GetDisplaySkill(eval.Id_skill, eval.Id_post.Value);

                        afisareEval.DateEval.Add(new AfisareSkillsEvalModel.EvalArray()
                        {
                            IdSkill = eval.Id_skill,
                            DenumireSkill = competentaPost.Denumire,
                            DetaliiSkill = competentaPost.Detalii != null ? competentaPost.Detalii : "",
                            Ideal = competentaPost.Ideal
                        });

                        int valoareIndiv = 0;
                        int valoare = 0;
                        int valoareFinala = 0;

                        bool convertValIndiv = int.TryParse(eval.Valoare_indiv, out valoareIndiv);
                        bool convertVal = int.TryParse(eval.Valoare, out valoare);
                        bool convertValFinala = int.TryParse(eval.Valoare_fin, out valoareFinala);

                        afisareEval.DateEval[indxEval].ValIndiv = convertValIndiv == true ? valoareIndiv : null;
                        afisareEval.DateEval[indxEval].Val = convertVal == true ? valoare : null;
                        afisareEval.DateEval[indxEval].ValFin = convertValFinala == true ? valoareFinala : null;

                        afisareEval.DateEval[indxEval].Obs = eval.Observatie != null ?
                            eval.Observatie : "";

                        if (valoareFinala != 0)
                            sumValFin += valoareFinala;
                    }
                    indxEval++;
                }

                var idTraining = evaluariAnulSelectat.FirstOrDefault(ev => !string.IsNullOrWhiteSpace(ev.Id_training))?.Id_training;

                if (idTraining != null)
                    afisareEval.DisplayIdsTraining = _conversionHelper.ConvertStringWithCommaSeparatorToIntArray(idTraining);

                if (sumValFin != 0)
                {
                    decimal calificativFinal = (decimal)sumValFin / evaluariAnulSelectat.Count;
                    afisareEval.CalificativFinal = Math.Round(calificativFinal, 1);
                    afisareEval.IncadrareCalificativFinal = MapCalificativFinal(afisareEval.CalificativFinal.Value);
                }
            }

            return afisareEval;
        }

        private bool HasAnyEvaluariNefinalizateAnulCurent(int idAngajat)
        {
            int currentYear = DateTime.Now.Year;

            return _epersContext.Evaluare_competente.Any(eval => eval.Id_angajat == idAngajat.ToString()
                && (eval.Flag_finalizat == 0) && eval.Anul == currentYear);
        }

        public void UpsertEvaluare(EvaluareTemplate evalInput)
        {
            List<RelevantSkillsModel> userRelevantSkills = _competenteService.Get(evalInput.IdPost);

            User subaltern = _userService.Get(evalInput.IdAngajat);
            User superior = _userService.GetSuperior(evalInput.IdAngajat);

            int nrCompIntorduseValSiValIndiv = evalInput.DateEval.Where(ev => (ev.Val.HasValue || ev.ValIndiv.HasValue)).Count();
            int nrCompIntroduseValFin = evalInput.DateEval.Count(ev => ev.ValFin.HasValue);

            bool userHasEvaluariNefinalizate = HasAnyEvaluariNefinalizateAnulCurent(evalInput.IdAngajat);

            List<Evaluare_competente> evaluariNefinalizateAnulCurent = GetEvaluariNefinalizate(subaltern.Matricola, DateTime.Now.Year);

            if (nrCompIntorduseValSiValIndiv != userRelevantSkills.Count && evalInput.TipEvaluare != 3)
                throw new Exception("Nu toate competentele au nota! Introduce-ti o nota pentru fiecare!");

            if ((nrCompIntroduseValFin != userRelevantSkills.Count) && evalInput.TipEvaluare == 3)
                throw new Exception("Nu toate competentele au nota! Introduce-ti o nota pentru fiecare!");


            for (int i = 0; i < userRelevantSkills.Count; i++)
            {
                if (!AreEqual(userRelevantSkills[i].Id_Skill, evalInput.DateEval[i].IdSkill))
                    throw new Exception("Competentele intorduse nu coincid cu cele din nomenclator!");

                if (userHasEvaluariNefinalizate)
                {
                    // ToDO: check how to improve this evalCurenta logic
                    Evaluare_competente evalCurenta = evaluariNefinalizateAnulCurent[i];

                    // autoevaluare
                    if (evalInput.TipEvaluare == 1)
                    {
                        UpdateAutoEvaluare(subaltern, evalCurenta, evalInput, i);
                    }

                    // evaluare sef
                    if (evalInput.TipEvaluare == 2)
                    {
                        UpdateEvaluareSubaltern(subaltern, superior, evalCurenta, evalInput, i);
                    }

                    // evaluare finala
                    if (evalInput.TipEvaluare == 3)
                    {
                        UpdateEvaluareFinalaSubaltern(subaltern, superior, evalCurenta, evalInput, i, userRelevantSkills.Count);
                    }
                }
                else
                {
                    if (evalInput.TipEvaluare == 1)
                    {
                        AddAutoEvaluare(subaltern, evalInput, i);
                    }

                    if (evalInput.TipEvaluare == 2)
                    {
                        AddEvaluareSubaltern(subaltern, superior, evalInput, i);
                    }
                }
            }
        }

        private void AddAutoEvaluare(User user, EvaluareTemplate evalTemplate, int index)
        {
            var eval = new Evaluare_competente
            {
                Anul = evalTemplate.Anul,
                Id_angajat = user.Id.ToString(),
                Id_sef = user.IdSuperior.HasValue ? user.IdSuperior.Value.ToString() : string.Empty,
                Matricola = user.Matricola,
                Matricola_Sef = !string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? user.MatricolaSuperior : string.Empty,
                Data_AutoEval = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateId = user.Matricola,
                Valoare_indiv = evalTemplate.DateEval[index].ValIndiv.HasValue ? evalTemplate.DateEval[index].ValIndiv.ToString() : string.Empty,
                Id_skill = evalTemplate.DateEval[index].IdSkill,
                Flag_finalizat = 0,
                Id_post = evalTemplate.IdPost,
                IdFirma = user.IdFirma,
                Observatie = evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(DateTime.Now.ToString("dd/MM/yyyy"), " ", user.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : ""
            };


            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Add(eval);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Add - Autoevaluare");
                throw;
            }
        }

        private void UpdateAutoEvaluare(User user, Evaluare_competente evalCurenta,
            EvaluareTemplate evalTemplate, int index)
        {
            var eval = evalCurenta;

            eval.Anul = evalTemplate.Anul;
            eval.Id_angajat = user.Id.ToString();
            eval.Id_sef = user.IdSuperior.HasValue ? user.IdSuperior.Value.ToString() : string.Empty;
            eval.Matricola = user.Matricola;
            eval.Matricola_Sef = !string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? user.MatricolaSuperior : string.Empty;
            eval.Data_AutoEval = DateTime.Now;
            eval.Valoare_indiv = evalTemplate.DateEval[index].ValIndiv.ToString();
            eval.Id_skill = evalTemplate.DateEval[index].IdSkill;
            eval.Flag_finalizat = 0;
            eval.IdFirma = user.IdFirma;
            eval.Id_post = evalTemplate.IdPost;

            if (evalCurenta.Observatie != null)
            {
                eval.Observatie += evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(" ", DateTime.Now.ToString("dd/MM/yyyy"), " ", user.NumePrenume,
                    ": ", evalTemplate.DateEval[index].Obs) : "";
            }
            else
            {
                eval.Observatie = evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(DateTime.Now.ToString("dd/MM/yyyy"), " ", user.NumePrenume,
                    ": ", evalTemplate.DateEval[index].Obs) : "";
            }

            eval.UpdateDate = DateTime.Now;
            eval.UpdateId = user.Matricola;

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Update(eval);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Update - Autoevaluare");
                throw;
            }
        }

        private void UpdateEvaluareSubaltern(User subaltern, User superior,
            Evaluare_competente evalCurenta, EvaluareTemplate evalTemplate, int index)
        {
            var eval = evalCurenta;

            eval.Anul = evalTemplate.Anul;
            eval.Id_angajat = subaltern.Id.ToString();
            eval.Id_sef = subaltern.IdSuperior.HasValue ? subaltern.IdSuperior.Value.ToString() : string.Empty;
            eval.Matricola = subaltern.Matricola;
            eval.Matricola_Sef = !string.IsNullOrWhiteSpace(subaltern.MatricolaSuperior) ? subaltern.MatricolaSuperior : string.Empty;
            eval.Data_EvalSef = DateTime.Now;
            eval.Valoare = evalTemplate.DateEval[index].Val.ToString();
            eval.Id_skill = evalTemplate.DateEval[index].IdSkill;
            eval.Flag_finalizat = 0;
            eval.IdFirma = subaltern.IdFirma;
            eval.Id_post = evalTemplate.IdPost;

            if (evalCurenta.Observatie != null)
            {
                eval.Observatie += evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(" ", DateTime.Now.ToString("dd/MM/yyyy"), " ",
                    superior.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : "";
            }
            else
            {
                eval.Observatie = evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(DateTime.Now.ToString("dd/MM/yyyy"), " ",
                    superior.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : "";
            }

            eval.UpdateDate = DateTime.Now;
            eval.UpdateId = subaltern.MatricolaSuperior;

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Update(eval);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Update - Evaluare Subaltern");
                throw;
            }
        }

        private void AddEvaluareSubaltern(User subaltern, User superior,
            EvaluareTemplate evalTemplate, int index)
        {
            var eval = new Evaluare_competente
            {
                Anul = evalTemplate.Anul,
                Id_angajat = subaltern.Id.ToString(),
                Id_sef = subaltern.IdSuperior.HasValue ? subaltern.IdSuperior.Value.ToString() : string.Empty,
                Matricola = subaltern.Matricola,
                Matricola_Sef = !string.IsNullOrWhiteSpace(subaltern.MatricolaSuperior) ? subaltern.MatricolaSuperior : string.Empty,
                UpdateDate = DateTime.Now,
                UpdateId = subaltern.MatricolaSuperior,
                Data_EvalSef = DateTime.Now,
                Valoare = evalTemplate.DateEval[index].Val.ToString(),
                Id_skill = evalTemplate.DateEval[index].IdSkill,
                Flag_finalizat = 0,
                IdFirma = subaltern.IdFirma,
                Id_post = evalTemplate.IdPost,
                Observatie = evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(DateTime.Now.ToString("dd/MM/yyyy"), " ", superior.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : ""
            };

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Add(eval);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Add - Evaluare Subaltern");
                throw;
            }
        }

        public Evaluare_competente Get(long id)
        {
            return _epersContext.Evaluare_competente.Single(ev => ev.Id == id);
        }

        public List<Evaluare_competente> GetAllForUser(string idAngajat)
        {
            return _epersContext.Evaluare_competente.Where(ev => ev.Id_angajat == idAngajat).Select(ev => ev).ToList();
        }

        private void UpdateEvaluareFinalaSubaltern(User subaltern, User superior,
            Evaluare_competente evalCurenta, EvaluareTemplate evalTemplate, int index, int totalRelevantSkills)
        {
            var eval = evalCurenta;

            eval.Anul = evalTemplate.Anul;
            eval.Id_angajat = subaltern.Id.ToString();
            eval.Id_sef = subaltern.IdSuperior.HasValue ? subaltern.IdSuperior.Value.ToString() : "";
            eval.Matricola = subaltern.Matricola;
            eval.Matricola_Sef = !string.IsNullOrWhiteSpace(subaltern.MatricolaSuperior) ? subaltern.MatricolaSuperior : string.Empty;
            eval.UpdateDate = DateTime.Now;
            eval.UpdateId = subaltern.MatricolaSuperior;
            eval.Data_EvalFinala = DateTime.Now;
            eval.Valoare_fin = evalTemplate.DateEval[index].ValFin.ToString();
            eval.Id_skill = evalTemplate.DateEval[index].IdSkill;
            eval.Flag_finalizat = 1;
            eval.Id_post = evalTemplate.IdPost;
            eval.IdFirma = subaltern.IdFirma;
            eval.CalificativFinalEvaluare = ComputeCalificativFinalEvaluare(evalTemplate.DateEval, totalRelevantSkills);

            if (evalCurenta.Observatie != null)
            {
                eval.Observatie += evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(" ", DateTime.Now.ToString("dd/MM/yyyy"), " ",
                    superior.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : "";
            }
            else
            {
                eval.Observatie = evalTemplate.DateEval[index].Obs != null ?
                    string.Concat(DateTime.Now.ToString("dd/MM/yyyy"), " ",
                    superior.NumePrenume, ": ", evalTemplate.DateEval[index].Obs) : "";
            }

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Update(eval);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Update - Evaluare Finala Subaltern");
                throw;
            }
        }

        private decimal ComputeCalificativFinalEvaluare(List<EvaluareCreateModel> evaluareList, int totalRelevantSkills)
        {
            var sum = 0;

            if (evaluareList.Select(ev => ev.ValFin.HasValue).Count() != totalRelevantSkills)
            {
                _logger.LogError("Nu au fost evaluate toate competențele, media finala nu poate fi calculată");
                return 0;
            }

            foreach (var eval in evaluareList)
            {
                if (eval.ValFin.HasValue)
                    sum += eval.ValFin.Value;
            }

            return Math.Round((decimal)sum / totalRelevantSkills, 2);
        }

        private bool AreEqual(int val1, int val2)
        {
            if (val1 == val2)
                return true;
            else
                return false;
        }

        private string MapCalificativFinal(decimal calificativ)
        {
            if (calificativ >= 1 && calificativ <= 1.5m)
                return "Insuficient";
            if (calificativ >= 1.6m && calificativ <= 2.5m)
                return "Necesită efort susținut pentru dezvoltare";
            if (calificativ >= 2.6m && calificativ <= 3.5m)
                return "Necesită dezvoltare";
            if (calificativ >= 3.6m && calificativ <= 4.5m)
                return "Exemplar";
            if (calificativ >= 4.6m && calificativ <= 5)
                return "Excepțional";
            return string.Empty;
        }

        public void ContestareEvaluare(int idAngajat, int? anul = null)
        {
            User user = _userService.Get(idAngajat);
            var evaluari = GetEvaluari(user.Matricola, anul);

            foreach (var eval in evaluari)
            {
                eval.Flag_finalizat = 0;
                eval.Valoare_fin = null;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.Evaluare_competente.Update(eval);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "EvaluareCompetente: Update - Contestare");
                    throw;
                }
            }
        }

        public EvaluarePipData GetDateEvaluareForPip(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
                anul = DateTime.Now.Year;

            var evaluari = _epersContext.Evaluare_competente
                .Where(ev => ev.Id_angajat == idAngajat.ToString() && ev.Anul == anul && ev.Flag_finalizat == 1)
                .ToList();

            if (!evaluari.Any())
            {
                // lasam gol pentru ca putem incepe pip is doar din obiective
                // throw new Exception("Nu exista evaluari pentru anul " + anul);
                return new EvaluarePipData();
            }

            var sumaEvaluari = 0;
            foreach (var eval in evaluari)
            {
                int valoareFinala = 0;
                int.TryParse(eval.Valoare_fin, out valoareFinala);

                sumaEvaluari += valoareFinala;
            }

            decimal calificativEvaluare = (decimal)sumaEvaluari / evaluari.Count;
            var dataInceputAutoevaluare = evaluari.Any() ? evaluari.Min(ev => ev.Data_AutoEval) : DateTime.MinValue;
            var dataInceputEvaluareSef = evaluari.Any() ? evaluari.Min(ev => ev.Data_EvalSef) : DateTime.MinValue;
            var dataInceput = dataInceputAutoevaluare <= dataInceputEvaluareSef ? dataInceputAutoevaluare : dataInceputEvaluareSef;
            var dataSfarsit = evaluari.Any() ? evaluari.Max(ev => ev.Data_EvalFinala) : DateTime.MinValue;

            if (!dataInceput.HasValue || !dataSfarsit.HasValue)
                throw new Exception("Nu exista date de inceput sau sfarsit");

            return new EvaluarePipData
            {
                IdEvaluari = evaluari.Select(ev => ev.Id).ToArray(),
                DataInceputEvaluare = dataInceput.Value,
                DataSfarsitEvaluare = dataSfarsit.Value,
                CalificativEvaluare = calificativEvaluare
            };

        }

        public AfisareEvalCalificativFinal GetIstoricEvalCalificativFinal(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            AfisareEvalCalificativFinal afisareCalificativFinal = new();
            User user = _userService.Get(idAngajat);
            List<Evaluare_competente> evaluariAnulSelectat = GetEvaluari(user.Matricola, anul);

            int sumValFin = 0;

            if (evaluariAnulSelectat != null && evaluariAnulSelectat.Count > 0)
            {
                foreach (var eval in evaluariAnulSelectat)
                {
                    int valoareFinala = 0;
                    bool convertValFinala = int.TryParse(eval.Valoare_fin, out valoareFinala);

                    if (valoareFinala != 0)
                        sumValFin += valoareFinala;
                }

                if (sumValFin != 0)
                {
                    decimal calificativFinal = (decimal)sumValFin / evaluariAnulSelectat.Count;
                    afisareCalificativFinal.CalificativFinal = Math.Round(calificativFinal, 1);
                    afisareCalificativFinal.IncadrareCalificativFinal = MapCalificativFinal(afisareCalificativFinal.CalificativFinal.Value);
                }
            }

            return afisareCalificativFinal;
        }

        public string UpdateCalificativFinalEvaluare(int? anul = null)
        {
            decimal calificativFinal = 0;
            int countUpdatedData = 0;

            if (!anul.HasValue)
                anul = DateTime.Now.Year;

            var listaMatricoleEvaluariFinalizate = _epersContext.Evaluare_competente
                .Where(ev => ev.Anul == anul && ev.Flag_finalizat == 1 && ev.CalificativFinalEvaluare == null)
                .Select(ev => ev.Matricola).Distinct().ToList();

            if (listaMatricoleEvaluariFinalizate.Any())
            {
                foreach (var matricolaEvalFinalizata in listaMatricoleEvaluariFinalizate)
                {
                    var evaluariAnulSelectat = GetEvaluari(matricolaEvalFinalizata, anul);
                    int sumValFin = 0;

                    if (evaluariAnulSelectat != null && evaluariAnulSelectat.Count > 0)
                    {
                        foreach (var eval in evaluariAnulSelectat)
                        {
                            int valoareFinala = 0;
                            bool convertValFinala = int.TryParse(eval.Valoare_fin, out valoareFinala);

                            if (valoareFinala != 0)
                                sumValFin += valoareFinala;
                        }

                        if (sumValFin != 0)
                        {
                            calificativFinal = Math.Round((decimal)sumValFin / evaluariAnulSelectat.Count, 2);
                        }

                        foreach (var eval in evaluariAnulSelectat)
                        {
                            eval.CalificativFinalEvaluare = calificativFinal;

                            using (var dbTransaction = _epersContext.Database.BeginTransaction())
                            {
                                _epersContext.Evaluare_competente.Update(eval);
                                _epersContext.SaveChanges();
                                dbTransaction.Commit();
                            }
                            countUpdatedData++;
                        }
                    }
                }
            }

            if (countUpdatedData == 0)
                return "Nu s-a actualizat nicio înregistare pentru Planul de Înbunătățire al Performanțelor (PIP)";

            return "S-au actualizat " + countUpdatedData + " înregistrări pentru Planul de Înbunătățire al Performanțelor (PIP)";
        }

        public bool UserHasEvaluareFinalizata(int idAngajat, int anul)
        {
            return _epersContext.Evaluare_competente.Any(eval => eval.Id_angajat == idAngajat.ToString()
                && (eval.Flag_finalizat == 1) && eval.Anul == anul);
        }

        public EvaluareListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniEvaluare = Array.Empty<EvaluareListaSubalteni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    // Get all users with their latest evaluation
                    var usersWithEvaluations = _epersContext.User
                        .Where(user => user.IdFirma == idFirmaFilter)
                        .OrderBy(user => user.NumePrenume)
                        .Select(user => new
                        {
                            User = user,
                            LatestEvaluation = _epersContext.Evaluare_competente
                                .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                                .OrderByDescending(e => e.Anul)
                                .FirstOrDefault(),
                            Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                        })
                        .Skip(pageSettings.ItemBeginIndex)
                        .Take(pageSettings.DisplayedItems)
                        .ToList();

                    listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                    {
                        MatricolaAngajat = x.User.Matricola,
                        NumePrenume = x.User.NumePrenume,
                        IdAngajat = x.User.Id,
                        PostAngajat = x.Post?.Nume ?? string.Empty,
                        COR = x.Post?.COR ?? string.Empty,
                        DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                        Concluzii = (x.LatestEvaluation != null
                            ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                            : string.Empty),
                        FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                        FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                             x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                    }).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter && (us.NumePrenume.Contains(filter) || us.Matricola == filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    // Get all users with their latest evaluation
                    var usersWithEvaluations = _epersContext.User
                        .Where(user => user.IdFirma == idFirmaFilter && (user.NumePrenume.Contains(filter) || user.Matricola == filter))
                        .OrderBy(user => user.NumePrenume)
                        .Select(user => new
                        {
                            User = user,
                            LatestEvaluation = _epersContext.Evaluare_competente
                                .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                                .OrderByDescending(e => e.Anul)
                                .FirstOrDefault(),
                            Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                        })
                        .Skip(pageSettings.ItemBeginIndex)
                        .Take(pageSettings.DisplayedItems)
                        .ToList();

                    listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                    {
                        MatricolaAngajat = x.User.Matricola,
                        NumePrenume = x.User.NumePrenume,
                        IdAngajat = x.User.Id,
                        PostAngajat = x.Post?.Nume ?? string.Empty,
                        COR = x.Post?.COR ?? string.Empty,
                        DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                        Concluzii = (x.LatestEvaluation != null
                            ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                            : string.Empty),
                        FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                        FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                             x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                    }).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniEvaluare = (from user in _epersContext.User
                                               join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                               from post in postJoin.DefaultIfEmpty()
                                               join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
                                               from latestEval in evalJoin.OrderByDescending(ev => ev.Anul).Take(1).DefaultIfEmpty()

                                               orderby user.NumePrenume ascending

                                               select new EvaluareListaSubalteni
                                               {
                                                   MatricolaAngajat = user.Matricola,
                                                   NumePrenume = user.NumePrenume,
                                                   IdAngajat = user.Id,
                                                   PostAngajat = post.Nume,
                                                   COR = post.COR != null ? post.COR : "",
                                                   DataAutoEval = latestEval.Data_AutoEval.HasValue ? latestEval.Data_AutoEval.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalSef = latestEval.Data_EvalSef.HasValue ? latestEval.Data_EvalSef.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalFin = latestEval.Data_EvalFinala.HasValue ? latestEval.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",
                                                   DataUltimaEval = latestEval.UpdateDate.HasValue ? latestEval.UpdateDate.Value.ToString("dd/MM/yyyy") : "",
                                                   Concluzii = latestEval.ConcluziiAspecteGen + latestEval.ConcluziiEvalCantOb + latestEval.ConcluziiEvalCompActDezProf,
                                                   FlagFinalizat = latestEval.Flag_finalizat == 1,
                                                   FinaliztAnulCurent = latestEval.Flag_finalizat == 1
                                                    && latestEval.UpdateDate.HasValue && (latestEval.UpdateDate.Value.Year == DateTime.Now.Year)
                                               }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.NumePrenume.Contains(filter) || us.Matricola == filter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniEvaluare = (from user in _epersContext.User
                                               join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                               from post in postJoin.DefaultIfEmpty()
                                               join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
                                               from latestEval in evalJoin.OrderByDescending(ev => ev.Anul).Take(1).DefaultIfEmpty()

                                               orderby user.NumePrenume ascending
                                               where user.NumePrenume.Contains(filter) || user.Matricola == filter

                                               select new EvaluareListaSubalteni
                                               {
                                                   MatricolaAngajat = user.Matricola,
                                                   NumePrenume = user.NumePrenume,
                                                   IdAngajat = user.Id,
                                                   PostAngajat = post.Nume,
                                                   COR = post.COR != null ? post.COR : "",
                                                   DataAutoEval = latestEval.Data_AutoEval.HasValue ? latestEval.Data_AutoEval.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalSef = latestEval.Data_EvalSef.HasValue ? latestEval.Data_EvalSef.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalFin = latestEval.Data_EvalFinala.HasValue ? latestEval.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",
                                                   DataUltimaEval = latestEval.UpdateDate.HasValue ? latestEval.UpdateDate.Value.ToString("dd/MM/yyyy") : "",
                                                   Concluzii = latestEval.ConcluziiAspecteGen + latestEval.ConcluziiEvalCantOb + latestEval.ConcluziiEvalCompActDezProf,
                                                   FlagFinalizat = latestEval.Flag_finalizat == 1,
                                                   FinaliztAnulCurent = latestEval.Flag_finalizat == 1
                                                    && latestEval.UpdateDate.HasValue && (latestEval.UpdateDate.Value.Year == DateTime.Now.Year)
                                               }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new EvaluareListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniEvaluare,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public EvaluareListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var listaSubalterniEvaluare = Array.Empty<EvaluareListaSubalteni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                // Get all users with their latest evaluation
                var usersWithEvaluations = _epersContext.User
                    .Where(user => user.IdFirma == loggedInUserFirma)
                    .OrderBy(user => user.NumePrenume)
                    .Select(user => new
                    {
                        User = user,
                        LatestEvaluation = _epersContext.Evaluare_competente
                            .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                            .OrderByDescending(e => e.Anul)
                            .FirstOrDefault(),
                        Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems)
                    .ToList();

                listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                {
                    MatricolaAngajat = x.User.Matricola,
                    NumePrenume = x.User.NumePrenume,
                    IdAngajat = x.User.Id,
                    PostAngajat = x.Post?.Nume ?? string.Empty,
                    COR = x.Post?.COR ?? string.Empty,
                    DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Concluzii = (x.LatestEvaluation != null
                        ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                        : string.Empty),
                    FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                    FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                         x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                }).ToArray();
            }
            else
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma && (us.NumePrenume.Contains(filter) || us.Matricola == filter));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                // Get all users with their latest evaluation
                var usersWithEvaluations = _epersContext.User
                    .Where(user => user.IdFirma == loggedInUserFirma && (user.NumePrenume.Contains(filter) || user.Matricola == filter))
                    .OrderBy(user => user.NumePrenume)
                    .Select(user => new
                    {
                        User = user,
                        LatestEvaluation = _epersContext.Evaluare_competente
                            .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                            .OrderByDescending(e => e.Anul)
                            .FirstOrDefault(),
                        Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems)
                    .ToList();

                listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                {
                    MatricolaAngajat = x.User.Matricola,
                    NumePrenume = x.User.NumePrenume,
                    IdAngajat = x.User.Id,
                    PostAngajat = x.Post?.Nume ?? string.Empty,
                    COR = x.Post?.COR ?? string.Empty,
                    DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Concluzii = (x.LatestEvaluation != null
                        ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                        : string.Empty),
                    FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                    FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                         x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                }).ToArray();
            }

            return new EvaluareListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniEvaluare,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public EvaluareListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaSuperior = null, string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniEvaluare = Array.Empty<EvaluareListaSubalteni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter && us.MatricolaSuperior == matricolaSuperior);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    // Get all users with their latest evaluation
                    var usersWithEvaluations = _epersContext.User
                        .Where(user => user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior)
                        .OrderBy(user => user.NumePrenume)
                        .Select(user => new
                        {
                            User = user,
                            LatestEvaluation = _epersContext.Evaluare_competente
                                .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                                .OrderByDescending(e => e.Anul)
                                .FirstOrDefault(),
                            Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                        })
                        .Skip(pageSettings.ItemBeginIndex)
                        .Take(pageSettings.DisplayedItems)
                        .ToList();

                    listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                    {
                        MatricolaAngajat = x.User.Matricola,
                        NumePrenume = x.User.NumePrenume,
                        IdAngajat = x.User.Id,
                        PostAngajat = x.Post?.Nume ?? string.Empty,
                        COR = x.Post?.COR ?? string.Empty,
                        DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                        Concluzii = (x.LatestEvaluation != null
                            ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                            : string.Empty),
                        FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                        FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                             x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                    }).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter && us.MatricolaSuperior == matricolaSuperior
                                && (us.NumePrenume.Contains(filter) || us.Matricola == filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var usersWithEvaluations = _epersContext.User
                    .Where(user => user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior
                     && (user.NumePrenume.Contains(filter) || user.Matricola == filter))
                    .OrderBy(user => user.NumePrenume)
                    .Select(user => new
                    {
                        User = user,
                        LatestEvaluation = _epersContext.Evaluare_competente
                            .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                            .OrderByDescending(e => e.Anul)
                            .FirstOrDefault(),
                        Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems)
                    .ToList();

                    listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                    {
                        MatricolaAngajat = x.User.Matricola,
                        NumePrenume = x.User.NumePrenume,
                        IdAngajat = x.User.Id,
                        PostAngajat = x.Post?.Nume ?? string.Empty,
                        COR = x.Post?.COR ?? string.Empty,
                        DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                        DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                        Concluzii = (x.LatestEvaluation != null
                            ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                              (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                            : string.Empty),
                        FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                        FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                             x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                    }).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == matricolaSuperior);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniEvaluare = (from user in _epersContext.User
                                               join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                               from post in postJoin.DefaultIfEmpty()
                                               join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
                                               from latestEval in evalJoin.OrderByDescending(ev => ev.Anul).Take(1).DefaultIfEmpty()

                                               orderby user.NumePrenume ascending
                                               where user.MatricolaSuperior == matricolaSuperior

                                               select new EvaluareListaSubalteni
                                               {
                                                   MatricolaAngajat = user.Matricola,
                                                   NumePrenume = user.NumePrenume,
                                                   IdAngajat = user.Id,
                                                   PostAngajat = post.Nume,
                                                   COR = post.COR != null ? post.COR : "",
                                                   DataAutoEval = latestEval.Data_AutoEval.HasValue ? latestEval.Data_AutoEval.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalSef = latestEval.Data_EvalSef.HasValue ? latestEval.Data_EvalSef.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalFin = latestEval.Data_EvalFinala.HasValue ? latestEval.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",
                                                   DataUltimaEval = latestEval.UpdateDate.HasValue ? latestEval.UpdateDate.Value.ToString("dd/MM/yyyy") : "",
                                                   Concluzii = latestEval.ConcluziiAspecteGen + latestEval.ConcluziiEvalCantOb + latestEval.ConcluziiEvalCompActDezProf,
                                                   FlagFinalizat = latestEval.Flag_finalizat == 1,
                                                   FinaliztAnulCurent = latestEval.Flag_finalizat == 1
                                                    && latestEval.UpdateDate.HasValue && (latestEval.UpdateDate.Value.Year == DateTime.Now.Year)
                                               }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.User.Count(us => us.MatricolaSuperior == matricolaSuperior && (us.NumePrenume.Contains(filter) || us.Matricola == filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniEvaluare = (from user in _epersContext.User
                                               join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                               from post in postJoin.DefaultIfEmpty()
                                               join evaluare in _epersContext.Evaluare_competente on user.Matricola equals evaluare.Matricola into evalJoin
                                               from latestEval in evalJoin.OrderByDescending(ev => ev.Anul).Take(1).DefaultIfEmpty()

                                               orderby user.NumePrenume ascending
                                               where user.MatricolaSuperior == matricolaSuperior && user.NumePrenume.Contains(filter) || user.Matricola == filter

                                               select new EvaluareListaSubalteni
                                               {
                                                   MatricolaAngajat = user.Matricola,
                                                   NumePrenume = user.NumePrenume,
                                                   IdAngajat = user.Id,
                                                   PostAngajat = post.Nume,
                                                   COR = post.COR != null ? post.COR : "",
                                                   DataAutoEval = latestEval.Data_AutoEval.HasValue ? latestEval.Data_AutoEval.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalSef = latestEval.Data_EvalSef.HasValue ? latestEval.Data_EvalSef.Value.ToString("dd/MM/yyyy") : "",
                                                   DataEvalFin = latestEval.Data_EvalFinala.HasValue ? latestEval.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",
                                                   DataUltimaEval = latestEval.UpdateDate.HasValue ? latestEval.UpdateDate.Value.ToString("dd/MM/yyyy") : "",
                                                   Concluzii = latestEval.ConcluziiAspecteGen + latestEval.ConcluziiEvalCantOb + latestEval.ConcluziiEvalCompActDezProf,
                                                   FlagFinalizat = latestEval.Flag_finalizat == 1,
                                                   FinaliztAnulCurent = latestEval.Flag_finalizat == 1
                                                    && latestEval.UpdateDate.HasValue && (latestEval.UpdateDate.Value.Year == DateTime.Now.Year)
                                               }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new EvaluareListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniEvaluare,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public EvaluareListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? matricolaSuperior = null, string? filter = null)
        {
            var listaSubalterniEvaluare = Array.Empty<EvaluareListaSubalteni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();


            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma && us.MatricolaSuperior == matricolaSuperior);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                // Get all users with their latest evaluation
                var usersWithEvaluations = _epersContext.User
                    .Where(user => user.IdFirma == loggedInUserFirma && user.MatricolaSuperior == matricolaSuperior)
                    .OrderBy(user => user.NumePrenume)
                    .Select(user => new
                    {
                        User = user,
                        LatestEvaluation = _epersContext.Evaluare_competente
                            .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                            .OrderByDescending(e => e.Anul)
                            .FirstOrDefault(),
                        Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems)
                    .ToList();

                listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                {
                    MatricolaAngajat = x.User.Matricola,
                    NumePrenume = x.User.NumePrenume,
                    IdAngajat = x.User.Id,
                    PostAngajat = x.Post?.Nume ?? string.Empty,
                    COR = x.Post?.COR ?? string.Empty,
                    DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Concluzii = (x.LatestEvaluation != null
                        ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                        : string.Empty),
                    FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                    FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                         x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                }).ToArray();

                return new EvaluareListaSubalterniDisplayModel
                {
                    ListaSubalterni = listaSubalterniEvaluare,
                    CurrentPage = pageSettings.CurrentPage,
                    Pages = pageSettings.Pages
                };
            }
            else
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma && us.MatricolaSuperior == matricolaSuperior
                            && (us.NumePrenume.Contains(filter) || us.Matricola == filter));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var usersWithEvaluations = _epersContext.User
                    .Where(user => user.IdFirma == loggedInUserFirma && user.MatricolaSuperior == matricolaSuperior
                    && (user.NumePrenume.Contains(filter) || user.Matricola == filter))
                    .OrderBy(user => user.NumePrenume)
                    .Select(user => new
                    {
                        User = user,
                        LatestEvaluation = _epersContext.Evaluare_competente
                            .Where(e => e.Matricola == user.Matricola && e.IdFirma == user.IdFirma)
                            .OrderByDescending(e => e.Anul)
                            .FirstOrDefault(),
                        Post = _epersContext.NPosturi.FirstOrDefault(p => p.Id == user.IdPost)
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems)
                    .ToList();

                listaSubalterniEvaluare = usersWithEvaluations.Select(x => new EvaluareListaSubalteni
                {
                    MatricolaAngajat = x.User.Matricola,
                    NumePrenume = x.User.NumePrenume,
                    IdAngajat = x.User.Id,
                    PostAngajat = x.Post?.Nume ?? string.Empty,
                    COR = x.Post?.COR ?? string.Empty,
                    DataAutoEval = x.LatestEvaluation?.Data_AutoEval?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalSef = x.LatestEvaluation?.Data_EvalSef?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataEvalFin = x.LatestEvaluation?.Data_EvalFinala?.ToString("dd/MM/yyyy") ?? string.Empty,
                    DataUltimaEval = x.LatestEvaluation?.UpdateDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Concluzii = (x.LatestEvaluation != null
                        ? (x.LatestEvaluation.ConcluziiAspecteGen ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCantOb ?? string.Empty) +
                          (x.LatestEvaluation.ConcluziiEvalCompActDezProf ?? string.Empty)
                        : string.Empty),
                    FlagFinalizat = x.LatestEvaluation?.Flag_finalizat == 1,
                    FinaliztAnulCurent = x.LatestEvaluation?.Flag_finalizat == 1 &&
                                         x.LatestEvaluation?.UpdateDate?.Year == DateTime.Now.Year
                }).ToArray();

                return new EvaluareListaSubalterniDisplayModel
                {
                    ListaSubalterni = listaSubalterniEvaluare,
                    CurrentPage = pageSettings.CurrentPage,
                    Pages = pageSettings.Pages
                };
            }
        }

        public void AddIdFrima(Evaluare_competente evalCompetente)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Evaluare_competente.Update(evalCompetente);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EvaluareCompetente: Add IdFrima");
                throw;
            }
        }
    }
}