using System;
using System.Net;
using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Sincron;
using Newtonsoft.Json;

namespace EpersBackend.Services.Sincron
{

    public class SincronizareSincron : ISincronizareSincron
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<SincronizareSincron> _logger;

        public SincronizareSincron(EpersContext epersContext,
             ILogger<SincronizareSincron> logger)
        {
            _epersContext = epersContext;
            _logger = logger;
        }

        public async void AddSincronEmployeeToEpers(int id)
        {
            var sincronUrl = "https://sincron.biz/apis/employees/" + id;

            var httpClientHandler = new HttpClientHandler
            {
                PreAuthenticate = true,
                Credentials = CreateAuthenticationCredentials()
            };
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri(sincronUrl);

            var response = await httpClient.GetAsync(new Uri(sincronUrl)).Result.Content.ReadAsStringAsync();
            var employeesReceived = JsonConvert.DeserializeObject<List<EmployeesSincronRecievedModel>>(response);

            if (employeesReceived != null)
            {
                UpsertEmployeeSincronTable(employeesReceived);
            }
        }

        public async void AddSincronEmployeesToEpers()
        {
            var sincronUrl = "https://sincron.biz/apis/employees/";

            var httpClientHandler = new HttpClientHandler
            {
                PreAuthenticate = true,
                Credentials = CreateAuthenticationCredentials()
            };
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri(sincronUrl);

            var response = await httpClient.GetAsync(new Uri(sincronUrl)).Result.Content.ReadAsStringAsync();
            var employeesReceived = JsonConvert.DeserializeObject<List<EmployeesSincronRecievedModel>>(response);

            if (employeesReceived != null)
            {
                UpsertEmployeeSincronTable(employeesReceived);
            }
        }

        private void UpsertEmployeeSincronTable(List<EmployeesSincronRecievedModel> sincronEmplyoyees)
        {
            //var existingEmployeesInEpers = GetEpersExistingEmployees();

            foreach (var sincronEmp in sincronEmplyoyees)
            {
                var epersEmpl = Map(sincronEmp);

                //if (existingEmployeesInEpers.Any(emp => emp.marca == sincronEmp.marca))
                //    _unitOfWork.EmployeesSincronRep.Update(epersEmpl);

                //else
                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.EmployeesSincronDbModel.Add(epersEmpl);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Sincronizare Sincron: UpsertEmployeeSincronTable");
                    throw;
                }
            }
        }

        //private List<EmployeesSincronDbModel> GetEpersExistingEmployees()
        //{
        //    return _unitOfWork.EmployeesSincronRep.GetAll().ToList();
        //}

        private CredentialCache CreateAuthenticationCredentials()
        {
            var sincronUsername = "aquila";
            var sincronPass = "92127b4483830b255a7f0615711607cd";

            var credCache = new CredentialCache
            {
                {
                    new Uri("https://sincron.biz"),
                    "Digest",
                    new NetworkCredential(sincronUsername, sincronPass)
                }
            };

            return credCache;
        }

        private EmployeesSincronDbModel Map(EmployeesSincronRecievedModel recievedModel)
        {
            return new EmployeesSincronDbModel
            {
                adresa_resedinta = recievedModel.adresa_resedinta.tara + " " + recievedModel.adresa_resedinta.judet + " "
                    + recievedModel.adresa_resedinta.localitate + " " + recievedModel.adresa_resedinta.sector + " "
                    + recievedModel.adresa_resedinta.strada + " " + recievedModel.adresa_resedinta.numar + " "
                    + recievedModel.adresa_resedinta.bloc + " " + recievedModel.adresa_resedinta.scara + " "
                    + recievedModel.adresa_resedinta.etaj + " " + recievedModel.adresa_resedinta.apartament + " "
                    + recievedModel.adresa_resedinta.cod_postal + " " + recievedModel.adresa_resedinta.detalii_1 + " "
                    + recievedModel.adresa_resedinta.detalii_2,

                document_identitate = recievedModel.document_identitate.tip + " " + recievedModel.document_identitate.serie + " "
                    + recievedModel.document_identitate.numar + " " + recievedModel.document_identitate.data_emiterii + " "
                    + recievedModel.document_identitate.data_expirarii + " " + recievedModel.document_identitate.emis_de,

                id_employee = recievedModel.id_employee,
                nume = recievedModel.nume,
                prenume = recievedModel.prenume,
                cnp = recievedModel.cnp,
                sin = recievedModel.sin,
                data_nastere = recievedModel.data_nastere,
                marca = recievedModel.marca,
                email = recievedModel.email,
                id_card_pontaj = recievedModel.id_card_pontaj,
                tags = recievedModel.tags,
                senioritate = recievedModel.senioritate,
                categorie_angajat = recievedModel.categorie_angajat,
                phones = recievedModel.phones,
                status = recievedModel.status,
                id_status = recievedModel.id_status,
                data_incepere_activitate = recievedModel.data_incepere_activitate,
                rol_ro = recievedModel.rol_ro,
                rol_en = recievedModel.rol_en,
                companie = recievedModel.companie,
                abreviere_companie = recievedModel.abreviere_companie,
                punct_de_lucru = recievedModel.punct_de_lucru,
                abreviere_punct_de_lucru = recievedModel.abreviere_punct_de_lucru,
                tara_punct_de_lucru = recievedModel.tara_punct_de_lucru,
                cod_tara_punct_de_lucru = recievedModel.cod_tara_punct_de_lucru,
                moneda_punct_de_lucru = recievedModel.moneda_punct_de_lucru,
                abreviere_moneda_punct_de_lucru = recievedModel.abreviere_moneda_punct_de_lucru,
                tara_companie = recievedModel.tara_companie,
                cod_tara_companie = recievedModel.cod_tara_companie,
                moneda_companie = recievedModel.moneda_companie,
                abreviere_moneda_companie = recievedModel.abreviere_moneda_companie,
                departament_angajat_en = recievedModel.departament_angajat_en,
                departament_angajat_ro = recievedModel.departament_angajat_ro,
                centru_cost = recievedModel.centru_cost,
                grup_angajat = recievedModel.grup_angajat,
                name = recievedModel.name,
                manager_direct = recievedModel.manager_direct,
                norma_lucru = recievedModel.norma_lucru,
                cor = recievedModel.cor,
                cod_cor = recievedModel.cod_cor,
                program_munca = recievedModel.program_munca,
                program_munca_contract = recievedModel.program_munca_contract,
                departament_contract_ro = recievedModel.departament_contract_ro,
                departament_contract_en = recievedModel.departament_contract_en,
                zile_concediu = recievedModel.zile_concediu,
                contract_type = recievedModel.contract_type,
                etichete = recievedModel.etichete,
                rate_code = recievedModel.rate_code,
                guild = recievedModel.guild,
                seniority = recievedModel.seniority,
                legal_entity_name = recievedModel.legal_entity_name
            };
        }
    }
}
