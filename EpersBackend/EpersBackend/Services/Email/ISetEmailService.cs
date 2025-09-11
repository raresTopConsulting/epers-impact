namespace EpersBackend.Services.Email
{
    public interface ISetEmailService
    {
        string SetEmailBodyEvaluare(string url, string nameMailTo, string mailBody);
        string SetEmailBodyObiective(string url, string nameMailTo, string mailBody);
        string SetEmailBodyPipFor(string url, string nameMailTo, string mailBody);
        string SetEmailBodyConcluzii(string url, string nameMailTo, string mailBody);
        string SetEmailBodyResetPassword(string url, string nameMailTo, string tempPassCode);
    }
}