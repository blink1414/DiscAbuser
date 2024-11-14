using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DiscordBot.Services;

public class FileHandler : ModuleBase<SocketCommandContext>, IFileHandler
{
    private readonly string _encryptionKey;
    private readonly IConfiguration _configuration;
    private readonly DiscordSocketClient _client;

    public FileHandler(IConfiguration configuration, DiscordSocketClient client)
    {
        _client = client;
        _configuration = configuration;
        _encryptionKey = _configuration["EncriptionKey"] ?? throw new Exception("Encryption key not found.");
    }

    [Command("encryptfile")]
    public async Task EncryptAndSendFileAsync(IEnumerable<Attachment> discordFile, SocketCommandContext context)
    {
        // Verifica se há um anexo na mensagem
        var attachment = discordFile.FirstOrDefault();
        if (attachment == null)
        {
            await ReplyAsync("Por favor, envie um arquivo para encriptar.");
            return;
        }

        // Baixa o arquivo enviado
        string originalFilePath = Path.Combine(Path.GetTempPath(), attachment.Filename);
        using (var httpClient = new HttpClient())
        {
            var fileBytes = await httpClient.GetByteArrayAsync(attachment.Url);
            await File.WriteAllBytesAsync(originalFilePath, fileBytes);
        }

        // Encripta o arquivo
        string encryptedFilePath = Path.Combine(Path.GetTempPath(), "encrypted_" + attachment.Filename);
        EncryptFile(originalFilePath, encryptedFilePath);

        // Envia o arquivo encriptado de volta para o canal
        using (var fileStream = new FileStream(encryptedFilePath, FileMode.Open, FileAccess.Read))
        {
            var file = new FileAttachment(fileStream, "encrypted_" + attachment.Filename);
            await context.Channel.SendFileAsync(fileStream, "encrypted_" + attachment.Filename, "Heres's the file");
            Console.WriteLine("File encrypted and saved");
        }

        // Apaga os arquivos temporários
        File.Delete(originalFilePath);
        File.Delete(encryptedFilePath);
    }

    private void EncryptFile(string inputFilePath, string outputFilePath)
    {
        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
        using (FileStream fsOutput = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
        using (Aes aes = Aes.Create())
        {
            byte[] key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.Key = key;
            aes.IV = new byte[16]; // Vetor de inicialização (IV) padrão de 16 bytes (zeros)

            using (CryptoStream cryptoStream = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                fsInput.CopyTo(cryptoStream);
            }
        }
    }
}
