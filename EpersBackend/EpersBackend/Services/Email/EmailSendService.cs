using Epers.Models.Users;
using EpersBackend.Services.Users;
using MailKit.Net.Smtp;
using MimeKit;

namespace EpersBackend.Services.Email
{
    public class EmailSendService : IEmailSendService
    {
        private readonly IConfiguration _configuration;
        private readonly ISetEmailService _setEmailService;
        private readonly IUserService _userService;

        public EmailSendService(IConfiguration configuration,
            ISetEmailService setEmailService,
            IUserService userService)
        {
            _configuration = configuration;
            _setEmailService = setEmailService;
            _userService = userService;
        }

        public void SendEmailAutoevaluare(int idEvaluat)
        {
            var userEvaluat = _userService.Get(idEvaluat);
            var manager = _userService.GetSuperior(idEvaluat);

            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/evaluare/evaluareSubaltern/" + idEvaluat;
            var mesajEvalauare = "Angajatului/agajata " + userEvaluat.NumePrenume + " a completat pasul 'Autoevaluare' din cadrul procesului de evaluare.";
            var emailBody = _setEmailService.SetEmailBodyEvaluare(url, manager.NumePrenume, mesajEvalauare);
            var emailSubject = "Epers: Autoevaluare: " + userEvaluat.NumePrenume;

            if (manager != null)
            {
                SendEmail(manager.Username, emailSubject, emailBody);
            }
        }

        public void SendEmailEvaluareSubaltern(int idEvaluat)
        {
            var userEvaluat = _userService.Get(idEvaluat);
            var manager = _userService.GetSuperior(idEvaluat);

            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/evaluare/istoricEvPersonala";

            if (manager != null)
            {
                var mesajEvalauare = "Ați fost evaluat/evaluată de către managerul dumneavoastră direct, " + manager.NumePrenume + ".";
                var emailSubject = "Epers: Evaluare de către: " + manager.NumePrenume;
                var emailBody = _setEmailService.SetEmailBodyEvaluare(url, userEvaluat.NumePrenume, mesajEvalauare);

                SendEmail(userEvaluat.Username, emailSubject, emailBody);
            }
            else
            {
                var mesajEvalauare = "Ați fost evaluat/evaluată de către managerul dumneavoastră direct.";
                var emailSubject = "Epers: Evaluare de către managerul direct.";
                var emailBody = _setEmailService.SetEmailBodyEvaluare(url, "Angajat", mesajEvalauare);

                SendEmail(userEvaluat.Username, emailSubject, emailBody);
            }
        }

        public void SendEmailEvaluareFinalaSubaltern(int idEvaluat)
        {
            var userEvaluat = _userService.Get(idEvaluat);
            var manager = _userService.GetSuperior(idEvaluat);

            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/evaluare/istoricEvPersonala";

            if (manager != null)
            {
                var mesajEvalauare = "Evaluarea finală a fost realizată de către managerul dumneavoastră direct, " + manager.NumePrenume + ".";
                var emailSubject = "Epers: Evaluare finală de către: " + manager.NumePrenume;
                var emailBody = _setEmailService.SetEmailBodyEvaluare(url, userEvaluat.NumePrenume, mesajEvalauare);

                SendEmail(userEvaluat.Username, emailSubject, emailBody);
            }
            else
            {
                var mesajEvalauare = "Evaluarea finală a fost realizată de către managerul dumneavoastră direct.";
                var emailSubject = "Epers: Evaluare finală de către managerul direct.";
                var emailBody = _setEmailService.SetEmailBodyEvaluare(url, "Angajat", mesajEvalauare);

                SendEmail(userEvaluat.Username, emailSubject, emailBody);
            }
        }

        public void SendEmailObiectiveSetate(string numeEvaluat, string mailAngajat, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/obiective/actuale/" + idAngajat;
            var mesajEvalauare = "Obiectivele dumneavoastră au fost setate de către managerul dumneavoastră direct.";

            var emailBody = _setEmailService.SetEmailBodyObiective(url, numeEvaluat, mesajEvalauare);
            var emailSubject = "Epers: Obiective Setate";

            SendEmail(mailAngajat, emailSubject, emailBody);
        }

        public void SendEmailObiectiveEvaluate(string numeEvaluat, string mailAngajat, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/obiective/istoric/" + idAngajat;
            var mesajEvalauare = "Obiectivele dumneavoastră au fost evaluate de către managerul dumneavoastră direct.";

            var emailBody = _setEmailService.SetEmailBodyObiective(url, numeEvaluat, mesajEvalauare);
            var emailSubject = "Epers: Obiective evaluate";

            SendEmail(mailAngajat, emailSubject, emailBody);
        }

        public void SendEmailCalificatPIPToAngajat(string numeAngajatPip, string mailAngajat, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/concluzii/istoric/" + idAngajat;

            var mesajPip = @"Din păcate, vă anunțăm că rezultatele obținute în urma procesuluii de Evaluare indică un nivel insuficient, conform criteriilor stabilite.
                    Ca urmare, este necesar să urmați un plan personalizat de îmbunătățire a performanței, pentru a vă sprijini în dezvoltarea competențelor și atingerea standardelor așteptate.
                    În contunare, veți primi un e-mail sau veți fi înștințat de către managerul dumneavoastră direct de îndată ce planul de îmbunătățire a performanței a fost aprobat de către departamentul de resurse umane.
                    Vă mulțumim pentru înțelegere și implicare! Echipa noastră este aici pentru a vă susține în acest proces.";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeAngajatPip, mesajPip);
            var emailSubject = "Epers: Calificare pentru Planulu de Îmbunătățire a Performanței";

            SendEmail(mailAngajat, emailSubject, emailBody);
        }

        public void SendEmailCalificatPIPToHR(string numeAngajatPip, string mailHr, string numeHr)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/lista-subalterni-calificati-pip";

            var mesajPip = "Angajatul " + numeAngajatPip + @" a obținut un calificativ insuficient în urma procesului de Evaluare.
            Puteți verifica și aproba Planulu de Îmbunătățire a Performanței din aplicație sau dând click pe butonul de mai jos.";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeHr, mesajPip);
            var emailSubject = "Epers: " + numeAngajatPip + " a fost calificat pentru a urma Planulu de Îmbunătățire a Performanțelor";

            SendEmail(mailHr, emailSubject, emailBody);
        }

        public void SendEmailAprobarePIPTOAngajat(string numeAngajatPip, string mailAngajat, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/concluzii/istoric/" + idAngajat;
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor a fost aprobat de departamentul Resurse Umane și este acum disponibil în aplicație.
                Vă încurajăm să accesați secțiunea dedicată pentru a consulta detaliile planului și pentru a începe pașii necesari în acest proces.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeAngajatPip, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost aprobat";

            SendEmail(mailAngajat, emailSubject, emailBody);
        }

        public void SendEmailAprobarePIPToManager(string numeAngajatPip, string mailManager, string numeManager)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/lista-subalterni-with-pip";
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor pentru " + numeAngajatPip + @"a fost aprobat de departamentul Resurse Umane și este acum disponibil în aplicație.
                Vă încurajăm să accesați secțiunea dedicată pentru a consulta detaliile planului și pentru a continua pașii necesari în acest proces.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeManager, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost aprobat pentru " + numeAngajatPip;

            SendEmail(mailManager, emailSubject, emailBody);
        }

        public void SendEmailPIPRespinsToManager(string mailManager, string numeManager, string numeAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/lista-subalterni-calificati-pip";
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor pentru " + numeAngajat + @" nu a fost aprobat de către departamentul de Resurse Umane.
                Puteți ajusta și verifica Planulu de Îmbunătățire a Performanței conform cerințelor departamentului de Resurse Umane din aplicație sau dând click pe butonul de mai jos.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeManager, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost respins.";

            SendEmail(mailManager, emailSubject, emailBody);
        }

        public void SendEmailPIPRespinsToAngajat(string mailAngajat, string numeAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/lista-subalterni-calificati-pip";
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor nu a fost aprobat de către departamentul de Resurse Umane.
                Acesta va trebuii să fie revizuit de către managerul dumneavoastră direct și trimis din nou la departamentul de resurse umane pentru aprobare.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeAngajat, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost respins.";

            SendEmail(mailAngajat, emailSubject, emailBody);
        }

        public void SendEmailPIPUpdateToHr(string numeAngajatPip, string mailHr, string numeHr)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/lista-subalterni-with-pip";
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor pentur angajatul/angajata " + numeAngajatPip + @"a fost actualizat și este acum disponibil în aplicație.
                Vă încurajăm să accesați secțiunea dedicată pentru a consulta actualizările planului și pentru a oferii suport în acest proces.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeHr, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost actualizat pentru " + numeAngajatPip;

            SendEmail(mailHr, emailSubject, emailBody);
        }

        public void SendEmailPipUpdateToAngajat(string nameMailTo, string mailAddress, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/concluzii/istoric/" + idAngajat;
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor a fost actualizat pentru dumneavoastră și este acum disponibil în aplicație.
                Vă încurajăm să accesați secțiunea dedicată pentru a consulta actualizările planului.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, nameMailTo, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost actualizat.";

            SendEmail(mailAddress, emailSubject, emailBody);
        }

        public void SendEmailPIPIncheiatAngajat(string numeEvaluat, string mailAddress, int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/concluzii/istoric/" + idAngajat;
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor a fost încheiat și este acum disponibil în aplicație.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, numeEvaluat, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost încheiat.";

            SendEmail(mailAddress, emailSubject, emailBody);
        }

        public void SendEmailPIPIncheiatManagerAndHr(string nameMailTo, string mailAddress, string numeAngajatPip)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/pip/istoric-lista-subalterni-with-pip";
            var mesajPip = @"Vă informăm că planul de îmbunătățire a performanțelor a fost încheiat pentru angjatul/angajata " + numeAngajatPip + @" și este acum disponibil în aplicație.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyPipFor(url, nameMailTo, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost încheiat.";

            SendEmail(mailAddress, emailSubject, emailBody);
        }

        public void SenEmailConcluziiEvaluare(int idAngajat)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            var angajat = _userService.Get(idAngajat);

            url += "/concluzii/istoric/" + idAngajat;
            var mesajPip = @"Conclzuiile finale ale evaluării calitative și cantitative au fost efectuate și sunt acum disponibil în aplicație.
                Vă încurajăm să accesați secțiunea dedicată pentru a consulta secțiunea de concluzii.
                Dacă aveți întrebări sau nevoie de suport, echipa noastră vă stă la dispoziție.
                Vă mulțumim pentru implicare și vă dorim succes!";

            var emailBody = _setEmailService.SetEmailBodyConcluzii(url, angajat.NumePrenume, mesajPip);
            var emailSubject = "Epers: Planul de Îmbunătățire a Performanțelor a fost actualizat.";

            SendEmail(angajat.Username, emailSubject, emailBody);
        }

        public void SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Epers", _configuration["EmailSettings:SenderEmail"]));
                email.To.Add(MailboxAddress.Parse(recipientEmail));
                email.Subject = subject;

                email.Body = new TextPart("html") { Text = body };

                using var smtpClient = new SmtpClient();
                smtpClient.Connect(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:Password"]);
                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendEmailWithTemporaryPassword(User user, string tempPassCode)
        {
            var url = _configuration["EmailSettings:AppBaseLink"];
            url += "/utilizatori/changePassword/" + user.Id;

            var emailBody = _setEmailService.SetEmailBodyResetPassword(url, user.NumePrenume, tempPassCode);
            var emailSubject = "Epers: Resetare parolă";

            SendEmail(user.Username, emailSubject, emailBody);
        }
    }
}