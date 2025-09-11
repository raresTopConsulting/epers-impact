using Epers.DataAccess;
using Epers.Models.Afisare;
using Epers.Models.Nomenclatoare;
using Epers.Models.Users;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.Header
{
	public class HeaderService: IHeaderService
	{
        private readonly EpersContext _epersContext;
        private readonly IUserService _userService;

		public HeaderService(EpersContext epersContext,
            IUserService userService)
		{
            _epersContext = epersContext;
            _userService = userService;
		}

        public AfisareHeaderModel GetHeader(int idAngajat)
        {
            NPosturi postSubaltern = new NPosturi();
            NPosturi postSuperior = new NPosturi();

            NLocatii locatieSubaltern = new NLocatii();
            NLocatii locatieSuperior = new NLocatii();

            NCompartimente compartimentSubaltern = new NCompartimente();
            NCompartimente compartimentSuperior = new NCompartimente();


            User subaltern = _userService.Get(idAngajat);
            User superior = _userService.GetSuperior(idAngajat);

            var header = new AfisareHeaderModel();

            if (subaltern.IdPost.HasValue && subaltern.IdPost != 0)
                postSubaltern = _epersContext.NPosturi.Single(post => post.Id == subaltern.IdPost.Value);

            if (superior.IdPost.HasValue && superior.IdPost != 0)
                postSuperior = _epersContext.NPosturi.Single(post => post.Id == superior.IdPost.Value);

            if (subaltern.IdLocatie.HasValue && subaltern.IdLocatie != 0)
                locatieSubaltern = _epersContext.NLocatii.Single(loc => loc.Id == subaltern.IdLocatie.Value);

            if (superior.IdLocatie.HasValue && superior.IdLocatie != 0)
                locatieSuperior = _epersContext.NLocatii.Single(loc => loc.Id == superior.IdLocatie.Value);

            //if (subaltern.IdCompartiment.HasValue && subaltern.IdCompartiment != 0)
            //    compartimentSubaltern = _epersContext.NCompartimente.Single(comp => comp.Id == subaltern.IdCompartiment.Value);

            //if (superior.IdCompartiment.HasValue && superior.IdCompartiment != 0)
            //    compartimentSuperior = _epersContext.NCompartimente.Single(comp => comp.Id == superior.IdCompartiment.Value);

            header.IdSuperior = superior.Id;
            header.IdSubaltern = subaltern.Id;
            header.NumePrenume = subaltern.NumePrenume;
            header.Matricola = subaltern.Matricola;
            header.MatricolaSef = superior.Matricola;
            header.NumePrenumeSef = superior.NumePrenume;
            header.DenumirePost = postSubaltern.Nume;
            header.COR = postSubaltern.COR;
            header.Compartiment = compartimentSubaltern.Denumire;
            header.Locatie = locatieSubaltern.Denumire;
            header.DenumirePostSupervizor = postSuperior.Nume;
            header.CORSupervizor = postSuperior.COR;
            header.CompartimentSupervizor = compartimentSuperior.Denumire;
            header.LocatieSupervizor = locatieSuperior.Denumire;
            header.IdCompartiment = subaltern.IdCompartiment;
            header.IdCompartimentSuperior = superior.IdCompartiment;
            header.IdPost = subaltern.IdPost;
            header.IdPostSuperior = superior.IdPost;
            header.IdLocatie = subaltern.IdLocatie;
            header.IdLocatieSupervizor = superior.IdLocatie;

            return header;
        }

        public AfisareUserDetails GetUserDetails(int id)
        {
            string numeSuperior = _userService.GetSuperior(id).NumePrenume;

            var userdetails = from user in _epersContext.User
                              join post in _epersContext.NPosturi
                              on user.IdPost equals post.Id into postJoin
                              from post in postJoin.DefaultIfEmpty()
                              
                              join locatie in _epersContext.NLocatii
                              on user.IdLocatie equals locatie.Id into locatieJoin
                              from locatie in locatieJoin.DefaultIfEmpty()

                              join compartiment in _epersContext.NCompartimente
                              on user.IdCompartiment equals compartiment.Id into compartimentJoin
                              from compartiment in compartimentJoin.DefaultIfEmpty()

                              join rol in _epersContext.Rol
                              on user.IdRol equals rol.Id

                              where user.Id == id

                              select new AfisareUserDetails
                              {
                                  Id = user.Id,
                                  Matricola = user.Matricola,
                                  NumePrenume = user.NumePrenume,
                                  Username = user.Username,
                                  Rol = rol.Denumire,
                                  DenumirePost = post.Nume,
                                  Cor = post.COR ?? "",
                                  OrganizatieBaza = compartiment.Denumire,
                                  OrganizatieIntermediara = compartiment.SubCompartiment ?? "",
                                  Locatie = locatie.Denumire,
                                  NumeSuperior = numeSuperior
                              };

            return userdetails.FirstOrDefault() ?? new AfisareUserDetails();
        }
    }
}

