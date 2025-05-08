using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using FishSpotApi.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class MailService(IConfiguration config, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));

    public void SendRecoverPasswordMail(string email, string name, string code)
    {
        var smtpClient = new SmtpClient()
        {
            Port = int.Parse(config["MailSettings:Port"] ?? "0"),
            Host = config["MailSettings:Host"] ?? "",
            EnableSsl = bool.Parse(config["MailSettings:EnableSsl"] ?? "false"),
            Timeout = int.Parse(config["MailSettings:Timeout"] ?? "5000"),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new System.Net.NetworkCredential(config["MailSettings:UserName"], config["MailSettings:Password"]),
        };

        var mailMessage = new MailMessage()
        {
            From = new MailAddress(config["MailSettings:From"] ?? "", config["MailSettings:DisplayName"]),
            Subject = _localizer["mail_subject"],
            Body = GetMailBody(code),
            IsBodyHtml = true
        };

        mailMessage.To.Add(new MailAddress(email, name));
        smtpClient.Send(mailMessage);
    }

    private string GetMailBody(string code)
    {
        var title = _localizer["mail_title"];
        var description = _localizer["mail_description"];
        var codeDescription = _localizer["mail_code_description"];
        var warningDescription = _localizer["mail_warning_description"];

        var codes = string.Empty;
        var codeArray = code.ToCharArray();
        for (var i = 0; i < codeArray.Length; i++)
        {
            codes += $"<li> {codeArray[i]} </li>";
        }

        return "<!DOCTYPE html>" +
            "<html lang='en'>" +
            "<head>" +
                "<meta charset='UTF-8' />" +
                "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0' />" +
                "<title>Document</title>" +
                "<style>" +
                ".card {" +
                    "display: block;" +
                    "margin: auto;" +
                    "width: 30em;" +
                    "border: 1px solid rgb(170, 170, 170);" +
                    "box-shadow: 0px 0px 5px rgb(170, 170, 170);" +
                    "border-radius: 5px;" +
                "}" +
                ".card-header {" +
                    "padding: 1rem;" +
                    "background-color: #68f76d;" +
                    "color: white;" +
                    "font-size: 1.5em;" +
                    "font-weight: bold;" +
                    "border-radius: 5px 5px 0px 0px;" +
                "}" +
                ".card-content {" +
                    "padding: 1rem;" +
                    "background-color: #ffffff;" +
                    "border-radius: 0px 0px 5px 5px;" +
                "}" +
                ".nav {" +
                   "padding: 0px;" +
                   "text-align: center;" +
                "}" +
                "ul.nav > li {" +
                    "display: inline-block;" +
                    "font-weight: bold;" +
                    "font-size: 2em;" +
                "}" +
                ".card-content p {" +
                    "font-size: large;" +
                    "font-family: 'Times New Roman', Times, serif;" +
                    "text-align: inherit;" +
                "}" +
                "</style>" +
            "</head>" +
            "<body>" +
                "<div class='card'>" +
                    "<div class='card-header'>" +
                        $"<h2 style='margin: 0px'>{title}</h2>" +
                    "</div>" +
                    "<div class='card-content'>" +
                        $"<p>{description}</p>" +
                        $"<ul class='nav'>{codes}</ul>" +
                        $"<p>{codeDescription}</p>" +
                        $"<p>{warningDescription}</p>" +
                    "</div>" +
                "</div>" +
            "</body>" +
            "</html>";
    }
}