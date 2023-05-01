using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Connect
        {
            public static async Task<(string? output, string? commandContinuation)> Command(string[] commandWords, API api)
            {
                switch (commandWords.Length)
                {
                    case 0:
                        return ("How the fuck did this happen???", null);
                    case 1:
                        return ("Please type in the server URL you wish to use: ", commandWords[0] + " ");
                    default:
                        var switchResult = api.SwitchServer(commandWords[1]);
                        if (!switchResult.success)
                            return (switchResult.error + " Try agin!", commandWords[0] + " ");
                        var result = await api.GetServerParameters();
                        if (!(bool)result.success)
                            return ($"Invalid Server, try again!", commandWords[0] + " ");
                        return ($"Disconnected from previous server. Now connected to: {string.Join(" ", result.availableUserDomains)}", null);
                }
            }
        }
    }
}

