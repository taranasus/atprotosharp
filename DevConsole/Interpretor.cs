using System;
using System.Text.Json;
using atprotosharp;

namespace DevConsole
{
    public class Interpretor
    {
        API _atApi;
        string _userHandle;

        public Interpretor()
        {
            _atApi = new API();
            _userHandle = "";
        }

        public string GetUser()
        {
            return _userHandle;
        }

        public async Task<(string output, string commandContinuation)> ProcessCommand(string command)
        {
            var commandWords = command.Split(' ');
            switch (commandWords[0].ToLower())
            {
                case "login":
                case "connect":
                    return await ConnectCommand(commandWords);
                    break;
                case "logout":
                case "disconnect":
                    await _atApi.LogOutAsync();
                    _userHandle = "";
                    return ("In a while aligator!", null);
                case "help":
                case "?":
                    return HelpCommand(commandWords);
                    break;
                default:
                    return (commandWords[0] + " not recognized as a command" + System.Environment.NewLine, null);
                    break;
            }

            return ("Something went tits up!" + System.Environment.NewLine, null);
        }

        private async Task<(string output, string commandContinuation)> ConnectCommand(string[] commandWords)
        {
            switch (commandWords.Length)
            {
                case 0:
                    return ("How the fuck did this happen???", null);
                case 1:
                    return ("Please type in your username: ", commandWords[0] + " ");
                case 2:
                    if (commandWords[1].ToLower() == "-f")
                    {
                        var credentials = ReadUserAndPasswordFromJsonFile("credentials/default.json");
                        return await ConnectCommand(new List<string>() { "connect", credentials.user, credentials.password }.ToArray());
                    }
                    return ("Please type in your password: ", commandWords[0] + " " + commandWords[1] + " ");
                default:
                    var response = await _atApi.ConnectAsync(commandWords[1], commandWords[2]);
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        _userHandle = _atApi.GetMyHandle();
                        return ("Connection Succesful! Welcome: " + _atApi.GetMyHandle(), null);
                    }
                    else
                        return (response, null);
            }
        }

        private (string output, string commandContinuation) HelpCommand(string[] commandWords)
        {
            string helpReturn =
@$"--------------------------------------------------------------------------
This application allows you to interact with the atProto in terminal form.

Most commands will guide you through how to use them after typing just the
first word.

Following is a list of all implemented commands in alphabetical order
--------------------------------------------------------------------------

connect    Establishes a connection with the bluesky server.
           Example: connect my@email.com myp4$$w0rd
           You can also just type ""connect"" and follow the prompts.

           -f   Will use credeintials stored in credentials/default.json
                instead of having to type them in, which is really fast
                and conveninet
disconnect Very complicated command. It logs you out of the current
           session.

?          This command. Displays this page. Same as ""help""

help       This command. Displays this page. Same as ""?""

login      same as ""connect""

logout     same as ""disconnect""
--------------------------------------------------------------------------";
            return (helpReturn, null);
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

