using System;
using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Sincron
{
	public class EmployeesSincronDbModel
	{
        [Key]
        public string marca { get; set; } = string.Empty;

        public string id_employee { get; set; } = string.Empty;
        public string nume { get; set; } = string.Empty;
        public string prenume { get; set; } = string.Empty;
        public string cnp { get; set; } = string.Empty;
        public string sin { get; set; } = string.Empty;
        public string data_nastere { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string id_card_pontaj { get; set; } = string.Empty;
        public string tags { get; set; } = string.Empty;
        public string senioritate { get; set; } = string.Empty;
        public string categorie_angajat { get; set; } = string.Empty;
        public string phones { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string id_status { get; set; } = string.Empty;
        public string data_incepere_activitate { get; set; } = string.Empty;
        public string rol_ro { get; set; } = string.Empty;
        public string rol_en { get; set; } = string.Empty;
        public string companie { get; set; } = string.Empty;
        public string abreviere_companie { get; set; } = string.Empty;
        public string punct_de_lucru { get; set; } = string.Empty;
        public string abreviere_punct_de_lucru { get; set; } = string.Empty;
        public string tara_punct_de_lucru { get; set; } = string.Empty;
        public string cod_tara_punct_de_lucru { get; set; } = string.Empty;
        public string moneda_punct_de_lucru { get; set; } = string.Empty;
        public string abreviere_moneda_punct_de_lucru { get; set; } = string.Empty;
        public string tara_companie { get; set; } = string.Empty;
        public string cod_tara_companie { get; set; } = string.Empty;
        public string moneda_companie { get; set; } = string.Empty;
        public string abreviere_moneda_companie { get; set; } = string.Empty;
        public string departament_angajat_en { get; set; } = string.Empty;
        public string departament_angajat_ro { get; set; } = string.Empty;
        public string centru_cost { get; set; } = string.Empty;
        public string grup_angajat { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string manager_direct { get; set; } = string.Empty;
        public string norma_lucru { get; set; } = string.Empty;
        public string cor { get; set; } = string.Empty;
        public string cod_cor { get; set; } = string.Empty;
        public string program_munca { get; set; } = string.Empty;
        public string program_munca_contract { get; set; } = string.Empty;
        public string departament_contract_ro { get; set; } = string.Empty;
        public string departament_contract_en { get; set; } = string.Empty;
        public string zile_concediu { get; set; } = string.Empty;
        public string contract_type { get; set; } = string.Empty;
        public string adresa_resedinta { get; set; } = string.Empty;
        public string document_identitate { get; set; } = string.Empty;
        public string etichete { get; set; } = string.Empty;
        public string rate_code { get; set; } = string.Empty;
        public string guild { get; set; } = string.Empty;
        public string seniority { get; set; } = string.Empty;
        public string legal_entity_name { get; set; } = string.Empty;
    }
}

