using Epers.Models.Users;

namespace EpersBackend.Services.Email
{
    public interface IEmailSendService
    {
        void SendEmail(string recipientEmail, string subject, string body);
        void SendEmailAutoevaluare(int idEvaluat);
        void SendEmailEvaluareSubaltern(int idEvaluat);
        void SendEmailEvaluareFinalaSubaltern(int idEvaluat);
        void SenEmailConcluziiEvaluare(int idAngajat);
        void SendEmailObiectiveSetate(string numeEvaluat, string mailAngajat, int idAngajat);
        void SendEmailObiectiveEvaluate(string numeEvaluat, string mailAngajat, int idAngajat);
        public void SendEmailCalificatPIPToHR(string numeAngajatPip, string mailHr, string numeHr);
        void SendEmailCalificatPIPToAngajat(string numeAngajatPip, string mailAngajat, int idAngajat);
        void SendEmailAprobarePIPTOAngajat(string numeAngajatPip, string mailAngajat, int idAngajat);
        void SendEmailAprobarePIPToManager(string numeAngajatPip, string mailManager, string numeManager);
        void SendEmailPIPRespinsToAngajat(string mailAngajat, string numeAngajat);
        void SendEmailPIPRespinsToManager(string mailManager, string numeManager, string numeAngajat);
        void SendEmailPIPUpdateToHr(string numeAngajatPip, string mailHr, string numeHr);
        void SendEmailPipUpdateToAngajat(string nameMailTo, string mailAddress, int idAngajat);
        void SendEmailPIPIncheiatAngajat(string nameMailTo, string mailAddress, int idAngajat);
        void SendEmailPIPIncheiatManagerAndHr(string nameMailTo, string mailAddress, string numeAngajatPip);

        void SendEmailWithTemporaryPassword(User user, string tempPassCode);
    }
}