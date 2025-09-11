using Epers.DataAccess;
using Epers.Models.Competente;

namespace EpersBackend.Services.Competente
{
    public class CompetenteService : ICompetenteService
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<RelevantSkillsModel> _logger;

        public CompetenteService(EpersContext epersContext,
            ILogger<RelevantSkillsModel> logger)
        {
            _epersContext = epersContext;
            _logger = logger;
        }

        public List<RelevantSkillsModel> Get(int? idPost = null)
        {
            if (idPost.HasValue && idPost.Value != 0)
            {
                var relevantSkills = from nSkills in _epersContext.NSkills
                                     join setareProfil in _epersContext.SetareProfil
                                     on nSkills.Id equals setareProfil.Id_Skill
                                     where setareProfil.Id_Post == idPost

                                     select new RelevantSkillsModel
                                     {
                                         Denumire = nSkills.Denumire,
                                         DataIn = nSkills.DataIn,
                                         DataSf = nSkills.DataSf,
                                         Id_Skill = nSkills.Id,
                                         IdPost = setareProfil.Id_Post.HasValue ? setareProfil.Id_Post.Value : 0,
                                         Detalii = nSkills.Detalii,
                                         Ideal = setareProfil.Ideal.HasValue ? setareProfil.Ideal.Value : 0
                                     };

                return relevantSkills.ToList();
            }
            else
            {
                var emptyList = new List<RelevantSkillsModel>();
                emptyList.Add(new RelevantSkillsModel()
                {
                    DataIn = null,
                    DataSf = null,
                    Denumire = string.Empty,
                    Detalii = null,
                    Id_Skill = 0,
                    Ideal = 0,
                    IdPost = 0
                });
                return emptyList;
            }
        }

        public RelevantSkillsModel GetDisplaySkill(int idSkill, int idPost)
        {
            var skillDisplay = from nSkills in _epersContext.NSkills
                               join setareProfil in _epersContext.SetareProfil
                               on nSkills.Id equals setareProfil.Id_Skill
                               where nSkills.Id == idSkill && setareProfil.Id_Post == idPost

                               select new RelevantSkillsModel
                               {
                                   Denumire = nSkills.Denumire,
                                   DataIn = nSkills.DataIn,
                                   DataSf = nSkills.DataSf,
                                   Id_Skill = nSkills.Id,
                                   IdPost = setareProfil.Id_Post.HasValue ? setareProfil.Id_Post.Value : 0,
                                   Detalii = nSkills.Detalii,
                                   Ideal = setareProfil.Ideal.HasValue ? setareProfil.Ideal.Value : 0
                               };

            return skillDisplay.Any() ? skillDisplay.First() : new RelevantSkillsModel();
        }
    }
}

