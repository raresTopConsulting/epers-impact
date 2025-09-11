using Epers.DataAccess;
using Epers.Models.Afisare;
using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public class EfSetareProfilPostRepository: IEfSetareProfilPostRepository
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<SetareProfil> _logger;

        public EfSetareProfilPostRepository(EpersContext epersContext,
            ILogger<SetareProfil> logger)
		{
            _epersContext = epersContext;
            _logger = logger;
		}

        public void Add(SetareProfil setareProfil)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.SetareProfil.Add(setareProfil);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfSetareProfilPostRepository: Add");
                throw;
            }
        }

        public void Delete(int idProfilPost)
        {
            try
            {
                var profilPostToDelete = _epersContext.SetareProfil.SingleOrDefault(pfp => pfp.Id == idProfilPost);

                if (profilPostToDelete != null )
                {
                    _epersContext.SetareProfil.Remove(profilPostToDelete);
                    _epersContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "EfSetareProfilPostRepository: Delete Profil Post");
                throw;
            }
        }

        public List<TableSetareProfil> Get(int idPost)
        {
            var profilulPostului = from setareProfil in _epersContext.SetareProfil
                                   join nSkills in _epersContext.NSkills
                                   on setareProfil.Id_Skill equals nSkills.Id
                                   where idPost == setareProfil.Id_Post
                                   select new TableSetareProfil
                                   {
                                       SetareProfil = setareProfil,
                                       DenumireSkill = nSkills.Denumire,
                                       DescriereSkill = nSkills.Descriere
                                   };

            return profilulPostului.ToList();
        }

        public void Update(SetareProfil setareProfil)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.SetareProfil.Update(setareProfil);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfSetareProfilPostRepository: Update");
                throw;
            }
        }

        //public void Upsert(SetareProfil[] setareProfilArray)
        //{
        //    try
        //    {
        //        foreach (var profil in setareProfilArray)
        //        {
        //            var existingProfil = _epersContext.SetareProfil.FirstOrDefault(pp => pp.Id == profil.Id);

        //            if (existingProfil != null)
        //            {
        //                existingProfil.Ideal = profil.Ideal;
        //                existingProfil.Id_Post = profil.Id_Post;
        //                existingProfil.Id_Skill = profil.Id_Skill;

        //                _epersContext.SetareProfil.Update(existingProfil);
        //                _epersContext.SaveChanges();
        //            }
        //            else
        //            {
        //                var newProfil = new SetareProfil
        //                {
        //                    Ideal = profil.Ideal,
        //                    Id_Post = profil.Id_Post,
        //                    Id_Skill = profil.Id_Skill
        //                };
        //                _epersContext.SetareProfil.Add(newProfil);
        //                _epersContext.SaveChanges();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "EfSetareProfilPostRepository: Upsert");
        //        throw;
        //    }
        //}
    }
}

