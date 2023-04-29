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
                case "get":
                    return await GetCommand(commandWords);
                case "connect":
                    return await ConnectCommand(commandWords);
                case "server":
                    return await ServerCommand(commandWords);
                case "login":
                    return await LoginCommand(commandWords);
                case "logout":
                case "disconnect":
                    await _atApi.LogoutAsync();
                    _userHandle = "";
                    return ("In a while aligator!", null);
                case "help":
                case "?":
                    return HelpCommand(commandWords);
                default:
                    return (commandWords[0] + " not recognized as a command" + System.Environment.NewLine, null);
            }

            return ("Something went tits up!" + System.Environment.NewLine, null);
        }

        private async Task<(string output, string commandContinuation)> ServerCommand(string[] commandWords)
        {
            switch (commandWords.Length)
            {
                case 0:
                    return ("How the fuck did this happen???", null);
                case 1:
                    string commandOpstions =
@"Here are your possible commands

info - Gets available information from this server
";
                    return (commandOpstions, commandWords[0] + " ");
                case 2:
                    switch (commandWords[1])
                    {
                        case "info":
                            var result = await _atApi.GetServerParameters();
                            if (!(bool)result.success)
                                return ($"Invalid Server, try again!", commandWords[0] + " ");
                            var formattedResult =
$@"Server URL: {result.serverUrl}
Available User Domains: {string.Join(" | ", result.availableUserDomains)}
Invitation code Required: {result.inviteCodeRequired}
Privacy Policy: {result.links.privacyPolicy}
Terms of Service: {result.links.termsOfService}";
                            return (formattedResult, null);
                        default:
                            return ("Invalid command", null);

                    }
                default:
                    return ("Invalid command", null);
            }
        }

        private async Task<(string output, string commandContinuation)> GetCommand(string[] commandWords)
        {
            switch (commandWords.Length)
            {
                case 0:
                    return ("How the fuck did this happen???", null);
                case 1:
                    string commandOpstions =
@"Here are your possible commands

timeline - Gets your timeline of posts and displays it
";
                    return (commandOpstions, commandWords[0] + " ");
                case 2:
                    switch (commandWords[1])
                    {
                        case "timeline":
                            var result = await _atApi.GetTimeline("reverse-chronological");
                            if (!(bool)result.success)
                                return (result.error, null);

                            string posts = "";
                            foreach (var item in result.feed)
                            {
                                posts += $"--- [{item.post.indexedAt}][{item.post.author.displayName}][{item.post.author.handle}]" + ((bool)item.post.author.viewer.muted ? "[MUTED]" : "") + ((bool)item.post.author.viewer.blockedBy ? "[BLOCKED]" : "") + System.Environment.NewLine +
                                    (item.post.author.labels.Count > 0 ? ("[LABELS: " + string.Join(" | " + item.post.author.labels) + System.Environment.NewLine) : "") +
                                    item.post.record.text + System.Environment.NewLine + System.Environment.NewLine;
                            }

                            return (posts, null);
                        default:
                            return ("Invalid command", null);

                    }
                default:
                    return ("Invalid command", null);
            }
        }

        private async Task<(string output, string commandContinuation)> ConnectCommand(string[] commandWords)
        {
            switch (commandWords.Length)
            {
                case 0:
                    return ("How the fuck did this happen???", null);
                case 1:
                    return ("Please type in the server URL you wish to use: ", commandWords[0] + " ");
                default:
                    var switchResult = await _atApi.SwitchServer(commandWords[1]);
                    if (!switchResult.success)
                        return (switchResult.error + " Try agin!", commandWords[0] + " ");
                    var result = await _atApi.GetServerParameters();
                    if (!(bool)result.success)
                        return ($"Invalid Server, try again!", commandWords[0] + " ");
                    return ($"Disconnected from previous server. Now connected to: {string.Join(" ", result.availableUserDomains)}", null);
            }
        }

        private async Task<(string output, string commandContinuation)> LoginCommand(string[] commandWords)
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
                        return await LoginCommand(new List<string>() { "connect", credentials.user, credentials.password }.ToArray());
                    }
                    return ("Please type in your password: ", commandWords[0] + " " + commandWords[1] + " ");
                default:
                    if (commandWords[1].ToLower() == "-f")
                    {
                        var credentials = ReadUserAndPasswordFromJsonFile($"credentials/{commandWords[2]}.json");
                        return await LoginCommand(new List<string>() { "connect", credentials.user, credentials.password }.ToArray());
                    }
                    var response = await _atApi.LoginAsync(commandWords[1], commandWords[2]);
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

?          This command. Displays this page. Same as ""help""

connect    allows you to specify which atProt server you want to
           communicate with

disconnect Very complicated command. It logs you out of the current
           session.

help       This command. Displays this page. Same as ""?""

login      Establishes a connection with the bluesky server.
           Example: connect my@email.com myp4$$w0rd
           You can also just type ""connect"" and follow the prompts.

           -f   Will use credeintials stored in credentials/default.json
                instead of having to type them in, which is really fast
                and conveninet

logout     same as ""disconnect""

server     Allows you to do various actions intended for the server.
           Type server and hit enter to get more information on available
           sub-commands.

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

