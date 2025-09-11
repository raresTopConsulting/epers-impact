namespace EpersBackend.Services.Email
{
    public class SetEmailService : ISetEmailService
    {
        private readonly IConfiguration _configuration;

        public SetEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string SetEmailBodyEvaluare(string url, string nameMailTo, string mailBody)
        {
            var dateContactEmailSuport = _configuration["EmailSettings:ContactEmailSuport"];
            var dateContactTelefonSuport = _configuration["EmailSettings:ContactTelefonSuport"];

            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #0D6EFD;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 20px;
                            text-align: center;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            font-size: 16px;
                        }
                        .content p {
                            margin: 15px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin: 20px 0;
                        }
                        .button {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 10px 20px;
                            text-decoration: none;
                            font-size: 16px;
                            border-radius: 5px;
                            display: inline-block;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #0D6EFD;
                            background-color: #f1f1f1;
                        }
                        .footer a {
                            color: #0D6EFD;
                            text-decoration: none;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            Notificare Aplicație Evaluare Personal
                        </div>
                        <div class='content'>
                            <p>Stimate/Stimată " + nameMailTo + @",</p>
                            <p>" + mailBody + @"</p>
                            <p> Pentru a consulta detalile evaluării și feedback-ul oferit, vă rugăm să accesați aplicația noastră de evaluare.</p>
                            <p>Puteți vizualiza detalile evaluării făcând click pe butonul de mai jos:</p>
                            <div class='button-container'>
                                <a href='" + url + "'" + @" class='button'>Deschideți Aplicația</a>
                            </div>
                            <p>Vă încurajăm să analizați feedback-ul primit și să adresați eventualele întrebări managerului dumneavoastră, dacă este necesar.</p>
                        </div>
                        <div class='footer'>
                            &copy; 2022 Evaluare Personal (Epers) - TopConsulting S.R.L. 
                            <br>
                            <a adresa email:" + dateContactEmailSuport + ">" + dateContactEmailSuport + @"</a>
                            <a nr. mobil:" + dateContactTelefonSuport + ">" + dateContactTelefonSuport + @"</a>
                        </div>
                    </div>
                </body>
            </html>";
        }

        public string SetEmailBodyObiective(string url, string nameMailTo, string mailBody)
        {
            var dateContactEmailSuport = _configuration["EmailSettings:ContactEmailSuport"];
            var dateContactTelefonSuport = _configuration["EmailSettings:ContactTelefonSuport"];

            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #0D6EFD;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 20px;
                            text-align: center;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            font-size: 16px;
                        }
                        .content p {
                            margin: 15px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin: 20px 0;
                        }
                        .button {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 10px 20px;
                            text-decoration: none;
                            font-size: 16px;
                            border-radius: 5px;
                            display: inline-block;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #0D6EFD;
                            background-color: #f1f1f1;
                        }
                        .footer a {
                            color: #0D6EFD;
                            text-decoration: none;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            Notificare Aplicație Evaluare Personal
                        </div>
                        <div class='content'>
                            <p>Stimate/Stimată " + nameMailTo + @",</p>
                            <p>" + mailBody + @"</p>
                            <p> Pentru a consulta detalile privind obiectivele, vă rugăm să accesați aplicația noastră de evaluare, secțiunea Obiective.</p>
                            <p>Puteți vizualiza detalile privind obiectivele făcând click pe butonul de mai jos:</p>
                            <div class='button-container'>
                                <a href='" + url + "'" + @" class='button'>Deschideți Aplicația</a>
                            </div>
                            <p>Vă încurajăm să analizați datele și să adresați eventualele întrebări managerului dumneavoastră, dacă este necesar.</p>
                        </div>
                        <div class='footer'>
                            &copy; 2022 Evaluare Personal (Epers) - TopConsulting S.R.L. 
                            <br>
                            <a adresa email:" + dateContactEmailSuport + ">" + dateContactEmailSuport + @"</a>
                            <a nr. mobil:" + dateContactTelefonSuport + ">" + dateContactTelefonSuport + @"</a>
                        </div>
                    </div>
                </body>
            </html>";
        }

        public string SetEmailBodyPipFor(string url, string nameMailTo, string mailBody)
        {
            var dateContactEmailSuport = _configuration["EmailSettings:ContactEmailSuport"];
            var dateContactTelefonSuport = _configuration["EmailSettings:ContactTelefonSuport"];

            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #0D6EFD;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 20px;
                            text-align: center;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            font-size: 16px;
                        }
                        .content p {
                            margin: 15px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin: 20px 0;
                        }
                        .button {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 10px 20px;
                            text-decoration: none;
                            font-size: 16px;
                            border-radius: 5px;
                            display: inline-block;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #0D6EFD;
                            background-color: #f1f1f1;
                        }
                        .footer a {
                            color: #0D6EFD;
                            text-decoration: none;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            Notificare Aplicație Evaluare Personal
                        </div>
                        <div class='content'>
                            <p>Stimate/Stimată " + nameMailTo + @",</p>
                            <p>" + mailBody + @"</p>
                            <p> Pentru a consulta detalile privind planul de îmbunătățire a performanțelor, vă rugăm să accesați aplicația noastră, secțiunea Concluzii și sețiunea PIP.</p>
                            <p>Puteți vizualiza detalile privind planul de îmbunătățire a performanțelor făcând click pe butonul de mai jos:</p>
                            <div class='button-container'>
                                <a href='" + url + "'" + @" class='button'>Deschideți Aplicația</a>
                            </div>
                            <p>Vă încurajăm să analizați datele și să adresați eventualele întrebări managerului dumneavoastră sau departamentului de Resurse Umane, dacă este necesar.</p>
                        </div>
                        <div class='footer'>
                            &copy; 2022 Evaluare Personal (Epers) - TopConsulting S.R.L. 
                            <br>
                            <a adresa email:" + dateContactEmailSuport + ">" + dateContactEmailSuport + @"</a>
                            <a nr. mobil:" + dateContactTelefonSuport + ">" + dateContactTelefonSuport + @"</a>
                        </div>
                    </div>
                </body>
            </html>";
        }

        public string SetEmailBodyConcluzii(string url, string nameMailTo, string mailBody)
        {
            var dateContactEmailSuport = _configuration["EmailSettings:ContactEmailSuport"];
            var dateContactTelefonSuport = _configuration["EmailSettings:ContactTelefonSuport"];

            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #0D6EFD;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 20px;
                            text-align: center;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            font-size: 16px;
                        }
                        .content p {
                            margin: 15px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin: 20px 0;
                        }
                        .button {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 10px 20px;
                            text-decoration: none;
                            font-size: 16px;
                            border-radius: 5px;
                            display: inline-block;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #0D6EFD;
                            background-color: #f1f1f1;
                        }
                        .footer a {
                            color: #0D6EFD;
                            text-decoration: none;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            Notificare Aplicație Evaluare Personal
                        </div>
                        <div class='content'>
                            <p>Stimate/Stimată " + nameMailTo + @",</p>
                            <p>" + mailBody + @"</p>
                            <p> Pentru a consulta rubrica de concluzii și feedback-ul oferit, vă rugăm să accesați aplicația noastră de evaluare.</p>
                            <p>Puteți vizualiza rubrica de concluzii făcând click pe butonul de mai jos:</p>
                            <div class='button-container'>
                                <a href='" + url + "'" + @" class='button'>Deschideți Aplicația</a>
                            </div>
                            <p>Vă încurajăm să analizați feedback-ul primit și să adresați eventualele întrebări managerului dumneavoastră, dacă este necesar.</p>
                        </div>
                        <div class='footer'>
                            &copy; 2022 Evaluare Personal (Epers) - TopConsulting S.R.L. 
                            <br>
                            <a adresa email:" + dateContactEmailSuport + ">" + dateContactEmailSuport + @"</a>
                            <a nr. mobil:" + dateContactTelefonSuport + ">" + dateContactTelefonSuport + @"</a>
                        </div>
                    </div>
                </body>
            </html>";
        }

        public string SetEmailBodyResetPassword(string url, string nameMailTo, string tempPassCode)
        {
            var dateContactEmailSuport = _configuration["EmailSettings:ContactEmailSuport"];
            var dateContactTelefonSuport = _configuration["EmailSettings:ContactTelefonSuport"];

            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            background-color: #0D6EFD;
                            margin: 0;
                            padding: 0;
                        }
                        .email-container {
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            border-radius: 8px;
                            overflow: hidden;
                            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 20px;
                            text-align: center;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            font-size: 16px;
                        }
                        .content p {
                            margin: 15px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin: 20px 0;
                        }
                        .button {
                            background-color: #0D6EFD;
                            color: white !important;
                            padding: 10px 20px;
                            text-decoration: none;
                            font-size: 16px;
                            border-radius: 5px;
                            display: inline-block;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #0D6EFD;
                            background-color: #f1f1f1;
                        }
                        .footer a {
                            color: #0D6EFD;
                            text-decoration: none;
                        }
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            Resetare parolă Aplicație Evaluare Personal
                        </div>
                        <div class='content'>
                            <p>Stimate/Stimată " + nameMailTo + @",</p>

                            <p>Am primit o cerere de resetare a parolei pentru contul dumeanvoastră. Pentru a continua procesul, folosiți codul unic de mai jos pentru autentificare:</p>
                            <h4>Codul tău unic: " + tempPassCode + @"</h4>
                            <p>Acest cod este valabil timp de 2 ore.</p>
                            <p>Dacă nu ați solicitat această acțiune, vă rugăm să ignorați acest mesaj.</p>
                            <p>După ce vă autentificați folosind codul de mai sus, schimbați parola accesând următorul link:</p>
                            <div class='button-container'>
                                <a href='" + url + "'" + @" class='button'>Resetare parolă</a>
                            </div>
                        </div>
                        <div class='footer'>
                            &copy; 2022 Evaluare Personal (Epers) - TopConsulting S.R.L. 
                            <br>
                            <a adresa email:" + dateContactEmailSuport + ">" + dateContactEmailSuport + @"</a>
                            <a nr. mobil:" + dateContactTelefonSuport + ">" + dateContactTelefonSuport + @"</a>
                        </div>
                    </div>
                </body>
            </html>";
        }
    }
}