using System;
using System.Text.Json;
using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        API _api;
        string _userHandle;

        public Interpretor()
        {
            _api = new API();
            _userHandle = "";
        }

        public string GetUser()
        {
            return _userHandle;
        }

        public async Task<(string? output, string? commandContinuation)> ProcessCommand(string command)
        {
            var commandWords = command.Split(' ');
            switch (commandWords[0].ToLower())
            {
                case "get":
                    return await Get.Command(commandWords, _api);
                case "connect":
                    return await Connect.Command(commandWords, _api);
                case "server":
                    return await Server.Command(commandWords, _api);
                case "login":
                    var loginResult = await Login.Command(commandWords, _api);
                    _userHandle = loginResult.userHandle ?? "";
                    return (loginResult.output, loginResult.commandContinuation);
                case "logout":
                case "disconnect":
                    _api.Logout();
                    _userHandle = "";
                    return ("In a while aligator!", null);
                case "help":
                case "?":
                    return HelpCommand(commandWords);
                case "upload":
                    return await Upload.Command(commandWords, _api);
                case "post":
                    return await Post.Command(commandWords, command, _api);
                default:
                    return (commandWords[0] + " not recognized as a command" + System.Environment.NewLine, null);
            }
        }
        private (string? output, string? commandContinuation) HelpCommand(string[] commandWords)
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

upload     Uploads a file to the server. Type upload and then the full 
           filepath on your machine to the file you wish to upload.

--------------------------------------------------------------------------";
            return (helpReturn, null);
        }


    }
}

