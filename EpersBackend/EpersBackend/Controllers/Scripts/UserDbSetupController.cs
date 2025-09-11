//using AutoMapper;
//using Epers.DataAccess;
//using Epers.Models.Users;
//using EpersBackend.Services.Evaluare;
//using EpersBackend.Services.Users;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;

//namespace EpersBackend.Controllers.Scripts
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class UserDbSetupController : ControllerBase
//    {
//        private readonly EpersContext _epersContext;
//        private readonly IMapper _mapper;
//        private readonly IUserService _userService;
//        private readonly IPasswordManagement _passwordManagement;
//        private readonly IConfiguration _configuration;
//        private readonly IEvaluareService _evaluareService;

//        public UserDbSetupController(EpersContext epersContext,
//            IUserService userService, IMapper mapper,
//            IPasswordManagement passwordManagement,
//            IEvaluareService evaluareService,
//            IConfiguration configuration)
//        {
//            _epersContext = epersContext;
//            _userService = userService;
//            _mapper = mapper;
//            _passwordManagement = passwordManagement;
//            _configuration = configuration;
//            _evaluareService = evaluareService;
//        }

//        // script to read username and matricola and sef from epers-aquila
//        // register user in new usertable in epers-demo


//        [HttpGet("addAdmin")]
//        public IActionResult RegisterAdmin()
//        {
//            var admin = new UserCreateModel
//            {
//                Username = "admin@mail.com",
//                Matricola = "1234",
//                Password = "abcd",
//                ConfirmPassword = "abcd",
//                IdRol = 1
//            };

//            _userService.Register(admin);

//            return Ok();
//        }

//        [HttpGet("copyDataFromOldTable")]
//        public IActionResult MapUserTables()
//        {
//            var createUserList = new List<UserCreateModel>();

//            // read data from sql
//            //string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["OldDbConnection"].ToString();
//            var connectionString = _configuration.GetConnectionString("DefaultConnection");

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                // Open the connection
//                connection.Open();

//                // Execute SQL queries here
//                string sqlQuery = "SELECT " +
//                    "UserName," +
//                    "Nume_Prenume, " +
//                    "Matricola," +
//                    "Id_Post," +
//                    "Id_Locatie, " +
//                    "Id_Compartiment," +
//                    "Matricola_Superior" +
//                    "  FROM AspNetUsers";

//                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
//                {
//                    // Execute the query and obtain a SqlDataReader
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        // Process the returned data
//                        while (reader.Read())
//                        {
//                            if (!reader.GetString(0).Contains("admin"))
//                            {
//                                // Access data from the current row
//                                int? idPost = reader["Id_Post"] == DBNull.Value ? null : (int)reader["Id_Post"];
//                                int? idLocatie = reader["Id_Locatie"] == DBNull.Value ? null : (int)reader["Id_Locatie"];
//                                int? idCompartiment = reader["Id_Compartiment"] == DBNull.Value ? null : (int)reader["Id_Compartiment"];
//                                string? matricolaSup = reader["Matricola_Superior"] == DBNull.Value ? null : (string)reader["Matricola_Superior"];

//                                createUserList.Add(new UserCreateModel
//                                {
//                                    Username = reader.GetString(0),
//                                    NumePrenume = reader.GetString(1),
//                                    Matricola = reader.GetString(2),
//                                    IdPost = idPost,
//                                    IdLocatie = idLocatie,
//                                    IdCompartiment = idCompartiment,
//                                    MatricolaSuperior = !string.IsNullOrWhiteSpace(matricolaSup) ? matricolaSup : "",
//                                    IdRol = 2,
//                                    Password = reader.GetString(2),
//                                    ConfirmPassword = reader.GetString(2)
//                                });
//                            }
//                        }
//                    }
//                    connection.Close();
//                }
//            }

//            foreach (var user in createUserList)
//            {
//                _userService.Register(user);
//            }

//            return Ok();
//        }



//        [HttpGet("setHierarchy")]
//        public IActionResult SetUserHierarchy()
//        {
//            // code for impact
//            var usersMatricolaNoSuperior = new string[] { "ID_7628", "ID_TUDO2464" };

//            //var usersIDNoSuperior = new int[] { 1, 106, 235, 251, 289, 584,585,
//            //                      638,727,757,808,819,917,952,1029,1045,1055,
//            //                      1140,1243,1252,1322,1355,1414,1446,1463,1517,
//            //                      1581,1596,1798,1829,1885,2166,2192,2222,2280,
//            //                      2370,2492,2588,2681,2686,2706 };

//            // code for crevedia
//            //var usersMatricolaNoSuperior = new string[] { "1" };

//            var usersList = _userService.GetAll();

//            foreach (var user in usersList)
//            {
//                var userSuperior = usersList.FirstOrDefault(u => u.Matricola == user.MatricolaSuperior);
//                if (userSuperior != null)
//                {
//                    var userToUpdate = new Epers.Models.Users.User();

//                    if (!user.IdSuperior.HasValue && !string.IsNullOrWhiteSpace(user.MatricolaSuperior))
//                    {
//                        userToUpdate.IdSuperior = userSuperior.Id;

//                        var updUser = _mapper.Map(user, userToUpdate);
//                        updUser.IdSuperior = userSuperior.Id;

//                        using (var dbTransaction = _epersContext.Database.BeginTransaction())
//                        {
//                            _epersContext.User.Update(updUser);
//                            _epersContext.SaveChanges();
//                            dbTransaction.Commit();
//                        }
//                    }
//                }
//                if (!usersMatricolaNoSuperior.Any(us => us == user.Matricola) && !string.IsNullOrWhiteSpace(user.MatricolaSuperior))
//                    _passwordManagement.ChangePassword(user.Matricola, user.Id);

//            }

//            return Ok();
//        }

//        [HttpGet("setHierarchyIncompleteUsers")]
//        public IActionResult SetUserHierarchyIncompleteUsers()
//        {
//            // code for aquila
//            var usersMatricolaNoSuperior = new string[] { "6773", "44432", "1234", "13649", "200000" };
//            var allUsersList = _userService.GetAll();
//            var allIncompleteUserList = _userService.GetAllIncompleteUsers();

//            foreach (var incompleteUser in allIncompleteUserList)
//            {
//                var userSuperior = allUsersList.FirstOrDefault(us => us.Matricola == incompleteUser.MatricolaSuperior);

//                if (userSuperior != null)
//                {
//                    var userToUpdate = new Epers.Models.Users.User();
//                    if (!incompleteUser.IdSuperior.HasValue && !string.IsNullOrWhiteSpace(incompleteUser.MatricolaSuperior))
//                    {
//                        userToUpdate.IdSuperior = userSuperior.Id;

//                        var updUser = _mapper.Map(incompleteUser, userToUpdate);
//                        updUser.IdSuperior = userSuperior.Id;

//                        using (var dbTransaction = _epersContext.Database.BeginTransaction())
//                        {
//                            _epersContext.User.Update(updUser);
//                            _epersContext.SaveChanges();
//                            dbTransaction.Commit();
//                        }
//                    }

//                }
//                if (!string.IsNullOrWhiteSpace(incompleteUser.MatricolaSuperior))
//                    _passwordManagement.ChangePassword(incompleteUser.Matricola, incompleteUser.Id);
//            }

//            return Ok("Ierarhia a fost setata!");
//        }



//        [HttpGet("import24")]
//        public IActionResult Import24()
//        {
//            var createUserList = new List<UserCreateModel>();
//            var connectionString = _configuration.GetConnectionString("DefaultConnection");

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    // Open the connection
//                    connection.Open();

//                    // Execute SQL queries here
//                    string sqlQuery = "SELECT " +
//                        "Username, " +
//                        "NumePrenume, " +
//                        "Matricola," +
//                        "IdPost," +
//                        "IdLocatie, " +
//                        "IdCompartiment, " +
//                        "MatricolaSuperior, " +
//                        "IdRol, " +
//                        "IdFirma " +
//                        "  FROM import_24";

//                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
//                    {
//                        // Execute the query and obtain a SqlDataReader
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            // Process the returned data
//                            while (reader.Read())
//                            {
//                                // Access data from the current row
//                                // int? idPost = reader["IdPost"] == DBNull.Value ? null : (int)reader["IdPost"];
//                                // int? idLocatie = reader["IdLocatie"] == DBNull.Value ? null : (int)reader["IdLocatie"];
//                                // int? idCompartiment = reader["IdCompartiment"] == DBNull.Value ? null : (int)reader["IdCompartiment"];
//                                // string? matricolaSup = reader["MatricolaSuperior"] == DBNull.Value ? null : (string)reader["MatricolaSuperior"];
//                                int? idFirma = reader["IdFirma"] == DBNull.Value ? null : (int)reader["IdFirma"];

//                                createUserList.Add(new UserCreateModel
//                                {
//                                    Username = reader.GetString(0),
//                                    NumePrenume = reader.GetString(1),
//                                    Matricola = reader.GetString(2),
//                                    IdPost = (int)reader["IdPost"],
//                                    IdLocatie = (int)reader["IdLocatie"],
//                                    IdCompartiment = (int)reader["IdCompartiment"],
//                                    MatricolaSuperior = (string)reader["MatricolaSuperior"],
//                                    IdRol = 2,
//                                    Password = reader.GetString(2),
//                                    ConfirmPassword = reader.GetString(2),
//                                    IdFirma = idFirma,
//                                    IdSuperior = null
//                                });
//                            }
//                            connection.Close();
//                        }
//                    }

//                    foreach (var user in createUserList)
//                    {
//                        _userService.Register(user);
//                    }
//                }
//                catch (Exception)
//                {
//                    connection.Close();
//                    throw;
//                }

//            }

//            return Ok("Utilizatorii au fost importati cu succes");
//        }


//        [HttpGet("addIdFirmaToEvaluareCompetente")]
//        public IActionResult AddIdFirmaToEvaluare()
//        {
//            var usersList = _userService.GetAllUsersWithIdFirma();
            
//            foreach(var user in usersList)
//            {
//                var evaluariAngajat = _evaluareService.GetAllForUser(user.Id.ToString());

//                foreach(var evalAngajat in evaluariAngajat) 
//                {
//                    _evaluareService.AddIdFrima(evalAngajat);
//                }
//            }

//            return Ok(new { message = "IdFirma a fost adagugat cu succes in tabela Evaluare_Competente" });
//        }

//        //[HttpGet("UpdatePasswords")]

//        // [HttpGet("Update/Evaluare/CalificativFinal")]
//        // public IActionResult UpdateEvaluareCalificativFinal()
//        // {
//        //     var result = _evaluareService.UpdateCalificativFinalEvaluare();

//        //     return Ok(result);
//        // }

//        public class OldUserModel
//        {
//            public int Id { get; set; }
//            public string NumePrenume { get; set; } = string.Empty;
//            public string Username { get; set; } = string.Empty;
//            public string Matricola { get; set; } = string.Empty;
//            public string MatricolaSuperior { get; set; } = string.Empty;
//            public int? IdPost { get; set; }
//            public int? IdLocatie { get; set; }
//            public int? IdCompartiment { get; set; }
//        }
//    }
//}
