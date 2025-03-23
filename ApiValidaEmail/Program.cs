using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using DnsClient;
using ApiValidaEmail;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API de validação de e-mail está funcionando!");

app.MapPost("/validar-email", async (EmailRequest request) =>
{
    if (!IsValidEmail(request.Email))
    {
        return Results.BadRequest("Formato de e-mail inválido.");
    }

    var emailParts = request.Email.Split('@');
    var dominio = emailParts[emailParts.Length - 1];

    if (!HasMxRecord(dominio))
    {
        return Results.BadRequest("O domínio não possui registro MX.");
    }

    string body = "<h1>E-mail de Teste</h1><br /><p>Este é um disparo automático da API de validação.</p>";

    var sent = await SendEmail(request.Email, "Validação concluída com sucesso!", body);

    if (!sent)
    {
        return Results.StatusCode(500);
    }

    return Results.Ok("E-mail válido, domínio com MX e e-mail enviado com sucesso.");
});

bool IsValidEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email)) return false;

    var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    return regex.IsMatch(email);
}

bool HasMxRecord(string dominio)
{
    var lookup = new LookupClient();
    var result = lookup.Query(dominio, QueryType.MX);
    return result.Answers.MxRecords().Any();
}

async Task<bool> SendEmail(string destino, string assunto, string corpoHtml)
{
    var mensagem = new MimeMessage();
    mensagem.From.Add(new MailboxAddress("Sistema de Validação", "noreply@suaempresa.com"));
    mensagem.To.Add(MailboxAddress.Parse(destino));
    mensagem.Subject = assunto;

    var bodyBuilder = new BodyBuilder
    {
        HtmlBody = corpoHtml
    };
    mensagem.Body = bodyBuilder.ToMessageBody();

    try
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.messagecenter.com.br", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("apikey", "MC.1B4270ca-C24a-47eC-9De8-ed8b8e4b4953-F3eFaE2A-48b3-476B-8b03-a8C35b85a978");
        await smtp.SendAsync(mensagem);
        await smtp.DisconnectAsync(true);
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
        return false;
    }
}

await app.RunAsync();

