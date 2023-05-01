using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Server
        {
            public static async Task<(string? output, string? commandContinuation)> Command(string[] commandWords, API api)
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
                                var result = await api.GetServerParameters();
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
        }
    }
}

