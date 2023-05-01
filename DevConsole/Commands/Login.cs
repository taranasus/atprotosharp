using System.Text.Json;
using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Login
        {
            public static async Task<(string? output, string? commandContinuation, string? userHandle)> Command(string[] commandWords, API api)
            {
                switch (commandWords.Length)
                {
                    case 0:
                        return ("How the fuck did this happen???", null, null);
                    case 1:
                        return ("Please type in your username: ", commandWords[0] + " ", null);
                    case 2:
                        if (commandWords[1].ToLower() == "-f")
                        {
                            var credentials = ReadUserAndPasswordFromJsonFile("credentials/default.json");
                            return await Command(new List<string>() { "connect", credentials.user, credentials.password }.ToArray(), api);
                        }
                        return ("Please type in your password: ", commandWords[0] + " " + commandWords[1] + " ", null);
                    default:
                        if (commandWords[1].ToLower() == "-f")
                        {
                            var credentials = ReadUserAndPasswordFromJsonFile($"credentials/{commandWords[2]}.json");
                            return await Command(new List<string>() { "connect", credentials.user, credentials.password }.ToArray(), api);
                        }
                        var response = await api.LoginAsync(commandWords[1], commandWords[2]);
                        if (string.IsNullOrWhiteSpace(response))
                        {
                            return ("Connection Succesful! Welcome: " + api.GetMyHandle(), null, api.GetMyHandle());
                        }
                        else
                            return (response, null, null);
                }
            }
            static (string user, string password) ReadUserAndPasswordFromJsonFile(string filePath)
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    var json = streamReader.ReadToEnd();
                    var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

                    var user = jsonObject.GetProperty("user").GetString();
                    var password = jsonObject.GetProperty("password").GetString();

                    return (user!, password!);
                }
            }
        }
    }
}

