using Epers.DataAccess;
using Epers.Models;
using Epers.Models.Nomenclatoare;
using Epers.Models.Users;

namespace EpersBackend.Services.Common
{
    public class DrodpwonRepository : IDrodpwonRepository
    {
        private readonly EpersContext _epersContext;


        public DrodpwonRepository(EpersContext epersContext)
        {
            _epersContext = epersContext;
        }

        public List<DropdownSelection> GetDDCompartimente()
        {
            var selectionCompartimente = from nCompartimente in _epersContext.NCompartimente
                                         join nLocatii in _epersContext.NLocatii on nCompartimente.Id_Locatie equals nLocatii.Id into locatiiJoin
                                         from nLocatii in locatiiJoin.DefaultIfEmpty()
                                         where nCompartimente.Data_sf == null || nCompartimente.Data_sf < DateTime.Now
                                         orderby nCompartimente.Denumire

                                         select new DropdownSelection
                                         {
                                             Id = nCompartimente.Id,
                                             Text = nCompartimente.Denumire + " - " + nLocatii.Denumire,
                                             Value = nCompartimente.Id.ToString(),
                                             IdFirma = nCompartimente.IdFirma
                                         };

            return selectionCompartimente.ToList();
        }

        public List<DropdownSelection> GetDDCursuri()
        {
            var selectionCursuri = from nCursuri in _epersContext.NCursuri
                                         orderby nCursuri.Denumire

                                         select new DropdownSelection
                                         {
                                             Id = nCursuri.Id,
                                             Text = nCursuri.Denumire,
                                             Value = nCursuri.Id.ToString(),
                                             IdFirma = nCursuri.IdFirma
                                         };

            return selectionCursuri.ToList();
        }

        public List<DropdownSelection> GetDDCompetente()
        {
            var selectionSkills = from nSkills in _epersContext.NSkills
                                  where nSkills.DataSf == null || nSkills.DataSf < DateTime.Now
                                  orderby nSkills.Denumire

                                  select new DropdownSelection
                                  {
                                      Id = nSkills.Id,
                                      Text = nSkills.Denumire,
                                      Value = nSkills.Id.ToString(),
                                      IdFirma = nSkills.IdFirma
                                  };

            return selectionSkills.ToList();
        }

        public List<DropdownSelection> GetDDDivizii()
        {
            var selectionDivizii = from nDivizii in _epersContext.NDivizii
                                   where nDivizii.DataSf == null || nDivizii.DataSf < DateTime.Now
                                   orderby nDivizii.Denumire
                                   select new DropdownSelection
                                   {
                                       Id = nDivizii.Id,
                                       Text = !string.IsNullOrWhiteSpace(nDivizii.Denumire) ? nDivizii.Denumire : "",
                                       Value = nDivizii.Id.ToString(),
                                       IdFirma = nDivizii.IdFirma
                                   };

            return selectionDivizii.ToList();
        }

        public List<DropdownSelection> GetDDLocatii()
        {
            var selectionLocatii = from nLocatii in _epersContext.NLocatii
                                   where nLocatii.DataSf == null || nLocatii.DataSf < DateTime.Now
                                   orderby nLocatii.Denumire
                                   select new DropdownSelection
                                   {
                                       Id = nLocatii.Id,
                                       Text = nLocatii.Denumire,
                                       Value = nLocatii.Id.ToString(),
                                       IdFirma = nLocatii.IdFirma

                                   };

            return selectionLocatii.ToList();
        }

        public List<DropdownSelection> GetDDPosturi()
        {
            var selectionPosturi = from nPosturi in _epersContext.NPosturi
                                   where nPosturi.DataSf == null || nPosturi.DataSf < DateTime.Now
                                   orderby nPosturi.Nume
                                   select new DropdownSelection
                                   {
                                       Id = nPosturi.Id,
                                       Text = string.Concat(nPosturi.Id, ": ", nPosturi.Nume, " ", nPosturi.COR),
                                       Value = nPosturi.Id.ToString(),
                                       IdFirma = nPosturi.IdFirma

                                   };

            return selectionPosturi.ToList();
        }

        public List<DropdownSelection> GetDDRoluri()
        {
            var selectionRoluri = from rol in _epersContext.Rol
                                  select new DropdownSelection
                                  {
                                      Id = rol.Id,
                                      Text = rol.Denumire,
                                      Value = rol.Id.ToString()
                                  };

            return selectionRoluri.ToList();
        }

        public List<DropdownSelection> GetDDUseri()
        {
            var selectionUserSup = from user in _epersContext.User
                                   orderby user.NumePrenume

                                   select new DropdownSelection
                                   {
                                       Id = user.Id,
                                       Text = string.Concat(user.Matricola, ": ", user.NumePrenume),
                                       Value = user.Id.ToString(),
                                       IdFirma = user.IdFirma
                                   };

            return selectionUserSup.ToList();
        }

        public NomenclatoareSelection GetNomenclatoareSelection()
        {
            return new NomenclatoareSelection
            {
                CompartimenteSelection = GetDDCompartimente(),
                LocatiiSelection = GetDDLocatii(),
                PosturiSelection = GetDDPosturi(),
                CompetenteSelection = GetDDCompetente()
            };
        }


        public List<DropdownSelection> GetDDObiective()
        {
            var selectionNOb = from nOb in _epersContext.NObiective
                               orderby nOb.Denumire
                               select new DropdownSelection
                               {
                                   Id = nOb.Id,
                                   Text = nOb.Denumire,
                                   Value = nOb.Id.ToString(),
                                   IdFirma = nOb.IdFirma
                               };

            return selectionNOb.ToList();
        }

        public AppDropdownSelections GetAllDropdownSelections()
        {
            return new AppDropdownSelections
            {
                DdCompartimente = GetDDCompartimente(),
                DdCompetente = GetDDCompetente(),
                DdDivizii = GetDDDivizii(),
                DdLocatii = GetDDLocatii(),
                DdObiective = GetDDObiective(),
                DdPosturi = GetDDPosturi(),
                DdRoluri = GetDDRoluri(),
                DdUseri = GetDDUseri(),
                DdCursuri = GetDDCursuri()
            };
        }
    }
}

