using Epers.Models.Competente;

namespace EpersBackend.Services.Competente
{
	public interface ICompetenteService
	{
        List<RelevantSkillsModel> Get(int? idPost = null);
        RelevantSkillsModel GetDisplaySkill(int idSkill, int idPost);
    }
}

