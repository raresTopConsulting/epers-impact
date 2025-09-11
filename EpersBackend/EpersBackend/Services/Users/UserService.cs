using AutoMapper;
using Epers.DataAccess;
using Epers.Models.Pagination;
using Epers.Models.Users;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Users
{
    public class UserService : IUserService
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<User> _logger;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IPagination _paginationService;

        public UserService(EpersContext epersContext,
            ILogger<User> logger,
            IMapper mapper,
            IPasswordService passwordService,
            IPagination paginationService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _mapper = mapper;
            _passwordService = passwordService;
            _paginationService = paginationService;
        }

        public void Delete(int id)
        {
            try
            {
                var userToDel = Get(id);

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.User.Remove(userToDel);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserService: Delete by id");
                throw;
            }
        }

        public void Delete(string matricola)
        {
            try
            {
                var userToDel = Get(matricola);

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.User.Remove(userToDel);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserService: Delete by matricola");
                throw;
            }
        }

        public List<User> GetListaSubordonati(int id)
        {
            return _epersContext.User.Where(sub => sub.IdSuperior == id).ToList();
        }

        public ListaUtilisatoriDisplayModel GetListaUtilizatoriAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var users = Array.Empty<ListaUtilizatori>();

            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count(us => us.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var userList = from user in _epersContext.User
                                   join post in _epersContext.NPosturi on user.IdPost
                                   equals post.Id into postJoin
                                   from post in postJoin.DefaultIfEmpty()

                                   join locatie in _epersContext.NLocatii on user.IdLocatie
                                   equals locatie.Id into locatieJoin
                                   from locatie in locatieJoin.DefaultIfEmpty()

                                   join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                   equals compartiment.Id into compartimentJoin
                                   from compartiment in compartimentJoin.DefaultIfEmpty()

                                   join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                                   from rol in rolJoin.DefaultIfEmpty()

                                   join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                                   from firma in firmaJoin.DefaultIfEmpty()

                                   orderby user.NumePrenume

                                   where user.IdFirma == idFirmaFilter

                                   select new ListaUtilizatori
                                   {
                                       Id = user.Id,
                                       Matricola = user.Matricola,
                                       Nume_Prenume = user.NumePrenume,
                                       Username = user.Username,
                                       Rol = rol.Denumire,
                                       IdFirma = user.IdFirma,
                                       DenumirePost = post.Nume,
                                       Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                       OrganizatieBaza = compartiment.Denumire,
                                       OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                       Locatie = locatie.Denumire,
                                       Firma = firma.Denumire
                                   };
                    users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi on user.IdPost
                                 equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()

                                 join locatie in _epersContext.NLocatii on user.IdLocatie
                                 equals locatie.Id into locatieJoin
                                 from locatie in locatieJoin.DefaultIfEmpty()

                                 join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                 equals compartiment.Id into compartimentJoin
                                 from compartiment in compartimentJoin.DefaultIfEmpty()

                                 where user.IdFirma == idFirmaFilter && (user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                    || post.Nume.Contains(filter)
                                    || locatie.Denumire.Contains(filter)
                                    || compartiment.Denumire.Contains(filter))

                                 select user).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var userList = from user in _epersContext.User
                                   join post in _epersContext.NPosturi on user.IdPost
                                   equals post.Id into postJoin
                                   from post in postJoin.DefaultIfEmpty()

                                   join locatie in _epersContext.NLocatii on user.IdLocatie
                                   equals locatie.Id into locatieJoin
                                   from locatie in locatieJoin.DefaultIfEmpty()

                                   join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                   equals compartiment.Id into compartimentJoin
                                   from compartiment in compartimentJoin.DefaultIfEmpty()

                                   join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                                   from rol in rolJoin.DefaultIfEmpty()

                                   join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                                   from firma in firmaJoin.DefaultIfEmpty()

                                   orderby user.NumePrenume

                                   where user.IdFirma == idFirmaFilter && (user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                      || post.Nume.Contains(filter)
                                      || locatie.Denumire.Contains(filter)
                                      || compartiment.Denumire.Contains(filter))

                                   select new ListaUtilizatori
                                   {
                                       Id = user.Id,
                                       Matricola = user.Matricola,
                                       Nume_Prenume = user.NumePrenume,
                                       Username = user.Username,
                                       Rol = rol.Denumire,
                                       IdFirma = user.IdFirma,
                                       DenumirePost = post.Nume,
                                       Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                       OrganizatieBaza = compartiment.Denumire,
                                       OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                       Locatie = locatie.Denumire,
                                       Firma = firma.Denumire
                                   };
                    users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.User.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var userList = from user in _epersContext.User
                                   join post in _epersContext.NPosturi on user.IdPost
                                   equals post.Id into postJoin
                                   from post in postJoin.DefaultIfEmpty()

                                   join locatie in _epersContext.NLocatii on user.IdLocatie
                                   equals locatie.Id into locatieJoin
                                   from locatie in locatieJoin.DefaultIfEmpty()

                                   join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                   equals compartiment.Id into compartimentJoin
                                   from compartiment in compartimentJoin.DefaultIfEmpty()

                                   join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                                   from rol in rolJoin.DefaultIfEmpty()

                                   join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                                   from firma in firmaJoin.DefaultIfEmpty()

                                   orderby user.NumePrenume

                                   select new ListaUtilizatori
                                   {
                                       Id = user.Id,
                                       Matricola = user.Matricola,
                                       Nume_Prenume = user.NumePrenume,
                                       Username = user.Username,
                                       Rol = rol.Denumire,
                                       IdFirma = user.IdFirma,
                                       DenumirePost = post.Nume,
                                       Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                       OrganizatieBaza = compartiment.Denumire,
                                       OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                       Locatie = locatie.Denumire,
                                       Firma = firma.Denumire
                                   };
                    users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi on user.IdPost
                                 equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()

                                 join locatie in _epersContext.NLocatii on user.IdLocatie
                                 equals locatie.Id into locatieJoin
                                 from locatie in locatieJoin.DefaultIfEmpty()

                                 join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                 equals compartiment.Id into compartimentJoin
                                 from compartiment in compartimentJoin.DefaultIfEmpty()

                                 where user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                    || post.Nume.Contains(filter)
                                    || locatie.Denumire.Contains(filter)
                                    || compartiment.Denumire.Contains(filter)

                                 select user).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var userList = from user in _epersContext.User
                                   join post in _epersContext.NPosturi on user.IdPost
                                   equals post.Id into postJoin
                                   from post in postJoin.DefaultIfEmpty()

                                   join locatie in _epersContext.NLocatii on user.IdLocatie
                                   equals locatie.Id into locatieJoin
                                   from locatie in locatieJoin.DefaultIfEmpty()

                                   join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                                   equals compartiment.Id into compartimentJoin
                                   from compartiment in compartimentJoin.DefaultIfEmpty()

                                   join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                                   from rol in rolJoin.DefaultIfEmpty()

                                   join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                                   from firma in firmaJoin.DefaultIfEmpty()

                                   orderby user.NumePrenume

                                   where user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                    || post.Nume.Contains(filter)
                                    || locatie.Denumire.Contains(filter)
                                    || compartiment.Denumire.Contains(filter)

                                   select new ListaUtilizatori
                                   {
                                       Id = user.Id,
                                       Matricola = user.Matricola,
                                       Nume_Prenume = user.NumePrenume,
                                       Username = user.Username,
                                       Rol = rol.Denumire,
                                       IdFirma = user.IdFirma,
                                       DenumirePost = post.Nume,
                                       Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                       OrganizatieBaza = compartiment.Denumire,
                                       OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                       Locatie = locatie.Denumire,
                                       Firma = firma.Denumire
                                   };
                    users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            return new ListaUtilisatoriDisplayModel
            {
                Utilizatori = users,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };
        }


        public ListaUtilisatoriDisplayModel GetListaUtilizatoriPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null)
        {
            var users = Array.Empty<ListaUtilizatori>();

            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.User.Count(us => us.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var userList = from user in _epersContext.User
                               join post in _epersContext.NPosturi on user.IdPost
                               equals post.Id into postJoin
                               from post in postJoin.DefaultIfEmpty()

                               join locatie in _epersContext.NLocatii on user.IdLocatie
                               equals locatie.Id into locatieJoin
                               from locatie in locatieJoin.DefaultIfEmpty()

                               join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                               equals compartiment.Id into compartimentJoin
                               from compartiment in compartimentJoin.DefaultIfEmpty()

                               join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                               from rol in rolJoin.DefaultIfEmpty()

                               join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                               from firma in firmaJoin.DefaultIfEmpty()

                               orderby user.NumePrenume

                               where user.IdFirma == loggedInUserFirma

                               select new ListaUtilizatori
                               {
                                   Id = user.Id,
                                   Matricola = user.Matricola,
                                   Nume_Prenume = user.NumePrenume,
                                   Username = user.Username,
                                   Rol = rol.Denumire,
                                   IdFirma = user.IdFirma,
                                   DenumirePost = post.Nume,
                                   Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                   OrganizatieBaza = compartiment.Denumire,
                                   OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                   Locatie = locatie.Denumire,
                                   Firma = firma.Denumire
                               };
                users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = (from user in _epersContext.User
                             join post in _epersContext.NPosturi on user.IdPost
                             equals post.Id into postJoin
                             from post in postJoin.DefaultIfEmpty()

                             join locatie in _epersContext.NLocatii on user.IdLocatie
                             equals locatie.Id into locatieJoin
                             from locatie in locatieJoin.DefaultIfEmpty()

                             join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                             equals compartiment.Id into compartimentJoin
                             from compartiment in compartimentJoin.DefaultIfEmpty()

                             join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                             from rol in rolJoin.DefaultIfEmpty()

                             join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                             from firma in firmaJoin.DefaultIfEmpty()

                             where user.IdFirma == loggedInUserFirma && (user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                || post.Nume.Contains(filter)
                                || locatie.Denumire.Contains(filter)
                                || compartiment.Denumire.Contains(filter))

                             select user).Count();

                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var userList = from user in _epersContext.User
                               join post in _epersContext.NPosturi on user.IdPost
                               equals post.Id into postJoin
                               from post in postJoin.DefaultIfEmpty()

                               join locatie in _epersContext.NLocatii on user.IdLocatie
                               equals locatie.Id into locatieJoin
                               from locatie in locatieJoin.DefaultIfEmpty()

                               join compartiment in _epersContext.NCompartimente on user.IdCompartiment
                               equals compartiment.Id into compartimentJoin
                               from compartiment in compartimentJoin.DefaultIfEmpty()

                               join rol in _epersContext.Rol on user.IdRol equals rol.Id into rolJoin
                               from rol in rolJoin.DefaultIfEmpty()

                               join firma in _epersContext.NFirme on user.IdFirma equals firma.Id into firmaJoin
                               from firma in firmaJoin.DefaultIfEmpty()

                               orderby user.NumePrenume

                               where user.IdFirma == loggedInUserFirma && (user.Username.Contains(filter) || user.NumePrenume.Contains(filter) || user.Matricola == filter
                                  || post.Nume.Contains(filter)
                                  || locatie.Denumire.Contains(filter)
                                  || compartiment.Denumire.Contains(filter))

                               select new ListaUtilizatori
                               {
                                   Id = user.Id,
                                   Matricola = user.Matricola,
                                   Nume_Prenume = user.NumePrenume,
                                   Username = user.Username,
                                   Rol = rol.Denumire,
                                   IdFirma = user.IdFirma,
                                   DenumirePost = post.Nume,
                                   Cor = !string.IsNullOrWhiteSpace(post.COR) ? post.COR : string.Empty,
                                   OrganizatieBaza = compartiment.Denumire,
                                   OrganizatieIntermediara = !string.IsNullOrWhiteSpace(compartiment.SubCompartiment) ? compartiment.SubCompartiment : "",
                                   Locatie = locatie.Denumire,
                                   Firma = firma.Denumire
                               };
                users = userList.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new ListaUtilisatoriDisplayModel
            {
                Utilizatori = users,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };
        }


        public User GetLoggedInSuperiorUserData(string username)
        {
            var subordonat = _epersContext.User.SingleOrDefault(us => us.Username == username);

            if (subordonat != null && subordonat.IdSuperior.HasValue)
            {
                GetLoggedInSuperiorUserData(subordonat.IdSuperior.Value);
            }
            if (subordonat != null && !subordonat.IdSuperior.HasValue)
            {
                return subordonat;
            }
            throw new("Utilizatorul nu exita!");
        }

        public User GetLoggedInSuperiorUserData(int id)
        {
            return _epersContext.User.SingleOrDefault(us => us.IdSuperior == id) ?? new User();
        }

        public User Get(string username)
        {
            return _epersContext.User.SingleOrDefault(us => us.Username == username) ?? new User();
        }

        public User Get(int id)
        {
            return _epersContext.User.SingleOrDefault(us => us.Id == id) ?? new User();
        }

        public User GetUserByMatricola(string matricola)
        {
            return _epersContext.User.SingleOrDefault(us => us.Matricola == matricola) ?? new User();
        }

        public string GetRol(int idRol)
        {
            return _epersContext.Rol.Any(rl => rl.Id == idRol) ?
                _epersContext.Rol.Single(rl => rl.Id == idRol).Denumire : string.Empty;
        }

        public string GetUsername()
        {
            throw new NotImplementedException();
        }

        public void Register(UserCreateModel registerUser)
        {
            if (registerUser.IdSuperior.HasValue)
            {
                User superior = Get(registerUser.IdSuperior.Value);
                registerUser.MatricolaSuperior = superior.Matricola;
            }
            if (!string.IsNullOrWhiteSpace(registerUser.MatricolaSuperior))
            {
                var existaSup = _epersContext.User.Any(us => us.Matricola == us.MatricolaSuperior);
                if (existaSup == true)
                {
                    var superior = GetUserByMatricola(registerUser.MatricolaSuperior);
                    registerUser.IdSuperior = superior.Id;
                }
            }

            try
            {
                _passwordService.CreatePasswordHash(registerUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = _mapper.Map<UserCreateModel, User>(registerUser);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.VerifiedAt = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.User.Add(user);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserService: Register");
                throw;
            }
        }

        public bool UserHasRight(string username, string jud)
        {
            throw new NotImplementedException();
        }

        public User GetSuperior(int idAngajat)
        {
            int? idSuperior = Get(idAngajat).IdSuperior;

            if (idSuperior.HasValue)
            {
                User superior = Get(idSuperior.Value);
                return superior;
            }

            return new User();
        }

        public void Update(UserEditModel userEdit)
        {

            User currentUserData = Get(userEdit.Id);

            if (currentUserData != null)
            {
                var updateUserData = _mapper.Map(userEdit, currentUserData);
                updateUserData.MatricolaSuperior = updateUserData.IdSuperior.HasValue ?
                    Get(updateUserData.IdSuperior.Value).Matricola : "";

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.User.Update(updateUserData);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UserService: Update User");
                    throw;
                }
            }

        }

        public List<UserEditModel> GetAll()
        {
            var userList = from user in _epersContext.User
                           select new UserEditModel
                           {
                               Id = user.Id,
                               IdCompartiment = user.IdCompartiment.HasValue ? user.IdCompartiment : null,
                               IdLocatie = user.IdLocatie.HasValue ? user.IdLocatie : null,
                               IdPost = user.IdPost.HasValue ? user.IdPost : null,
                               IdRol = user.IdRol,
                               Matricola = user.Matricola,
                               MatricolaSuperior = string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? string.Empty : user.MatricolaSuperior,
                               NumePrenume = user.NumePrenume,
                               Username = user.Username,
                               IdFirma = user.IdFirma
                           };

            return userList.Any() ? userList.ToList() : new List<UserEditModel>();
        }

        public List<UserEditModel> GetAllUsersWithIdFirma()
        {
            var usersWithIdFrima = _epersContext.User.Where(user => user.IdFirma.HasValue)
                        .Select(user => new UserEditModel
                        {
                            Id = user.Id,
                            IdCompartiment = user.IdCompartiment.HasValue ? user.IdCompartiment : null,
                            IdLocatie = user.IdLocatie.HasValue ? user.IdLocatie : null,
                            IdPost = user.IdPost.HasValue ? user.IdPost : null,
                            IdRol = user.IdRol,
                            Matricola = user.Matricola,
                            MatricolaSuperior = string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? string.Empty : user.MatricolaSuperior,
                            NumePrenume = user.NumePrenume,
                            Username = user.Username,
                            IdFirma = user.IdFirma
                        }).ToList();

            return usersWithIdFrima;
        }

        public List<UserEditModel> GetAllBetween(int idAStart, int idStop)
        {
            var userList = from user in _epersContext.User
                           where user.Id >= idAStart && user.Id <= idStop
                           select new UserEditModel
                           {
                               Id = user.Id,
                               IdCompartiment = user.IdCompartiment.HasValue ? user.IdCompartiment : null,
                               IdLocatie = user.IdLocatie.HasValue ? user.IdLocatie : null,
                               IdPost = user.IdPost.HasValue ? user.IdPost : null,
                               IdRol = user.IdRol,
                               Matricola = user.Matricola,
                               MatricolaSuperior = string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? string.Empty : user.MatricolaSuperior,
                               NumePrenume = user.NumePrenume,
                               Username = user.Username,
                               IdFirma = user.IdFirma
                           };

            return userList.Any() ? userList.ToList() : new List<UserEditModel>();
        }

        public List<UserEditModel> GetAllIncompleteUsers()
        {
            var usersMatricolaNoSuperior = new string[] { "6773", "44432", "1234", "13649", "200000" };

            var userList = from user in _epersContext.User
                           where user.IdSuperior == null
                           && (user.MatricolaSuperior != null && user.MatricolaSuperior != "0")
                           && !usersMatricolaNoSuperior.Contains(user.Matricola)

                           select new UserEditModel
                           {
                               Id = user.Id,
                               IdCompartiment = user.IdCompartiment.HasValue ? user.IdCompartiment : null,
                               IdLocatie = user.IdLocatie.HasValue ? user.IdLocatie : null,
                               IdPost = user.IdPost.HasValue ? user.IdPost : null,
                               IdRol = user.IdRol,
                               Matricola = user.Matricola,
                               MatricolaSuperior = string.IsNullOrWhiteSpace(user.MatricolaSuperior) ? string.Empty : user.MatricolaSuperior,
                               NumePrenume = user.NumePrenume,
                               Username = user.Username,
                               IdFirma = user.IdFirma
                           };

            return userList.Any() ? userList.ToList() : new List<UserEditModel>();

        }



        public SubalterniDropdown[] GetListaSubalterniDropdown(string matricolaSuperior)
        {
            var listaSubalterni = from user in _epersContext.User
                                  join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                  from post in postJoin.DefaultIfEmpty()

                                  where user.MatricolaSuperior == matricolaSuperior
                                  orderby user.NumePrenume

                                  select new SubalterniDropdown
                                  {
                                      IdAngajat = user.Id,
                                      NumePrenume = user.NumePrenume,
                                      MatricolaAngajat = user.Matricola,
                                      COR = post.COR != null ? post.COR : "",
                                      PostAngajat = post.Nume
                                  };

            return listaSubalterni.ToArray();
        }

        public SubalterniDropdown[] GetListaAngajatiForAdminDropdown()
        {
            var listaSubalterni = from user in _epersContext.User
                                  join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                  from post in postJoin.DefaultIfEmpty()

                                  orderby user.NumePrenume

                                  select new SubalterniDropdown
                                  {
                                      IdAngajat = user.Id,
                                      NumePrenume = user.NumePrenume,
                                      MatricolaAngajat = user.Matricola,
                                      COR = post.COR != null ? post.COR : "",
                                      PostAngajat = post.Nume
                                  };

            return listaSubalterni.ToArray();
        }

        public List<User> GetUseriHR(int? idFirma = null)
        {
            if (idFirma != null)
                return _epersContext.User.Where(us => us.IdFirma == idFirma && us.IdRol == 4).ToList();
            else
                return _epersContext.User.Where(us => us.IdRol == 4).ToList();
        }

        public void SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate)
        {
            var user = _epersContext.User.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpires = expiryDate;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.User.Update(user);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UserService: UpsertRrefreshToken");
                    throw;
                }
            }
        }

        public User? ValidateRefreshToken(string refreshToken)
        {
            return _epersContext.User.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpires > DateTime.UtcNow);
        }

        public void ReplaceRefreshToken(int userId, string oldRefreshToken, string newRefreshToken, DateTime expiryDate)
        {
            var user = _epersContext.User.FirstOrDefault(u => u.Id == userId);
            var oldRefreshTokenExists = _epersContext.User.Any(u => u.RefreshToken == oldRefreshToken && u.Id == userId);

            if (!oldRefreshTokenExists)
            {
                _logger.LogError("UserService: ReplaceRefreshToken old token not matching");
                throw new UnauthorizedAccessException("");
            }

            if (user != null)
            {
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpires = expiryDate;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.User.Update(user);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UserService: UpsertRrefreshToken error update");
                    throw;
                }
            }
        }

        public DateTime? GetRefreshTokenExpiry(string refreshToken)
        {
            return _epersContext.User.FirstOrDefault(u => u.RefreshToken == refreshToken).RefreshTokenExpires;
        }

        public void UpsertResetPasswordToken(int userId, string resetPasswordToken, DateTime expiryDate)
        {
            var user = _epersContext.User.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.ResetPasswordToken = resetPasswordToken;
                user.ResetPasswordTokenExpires = expiryDate;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.User.Update(user);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UserService: UpsertResetPasswordToken error update");
                    throw;
                }
            }
        }
    }
}

