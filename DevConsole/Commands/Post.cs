using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Post
        {
            public static async Task<(string? output, string? commandContinuation)> Command(string[] commandWords, string fullCommand, API api)
            {
                switch (commandWords.Length)
                {
                    case 0:
                        return ("How the fuck did this happen???", null);
                    case 1:
                        return ("Type in the text of your post between |your post goes here| and then the full filepath to any files you wish to attach to the post separated by space ", commandWords[0] + " ");
                    default:
                        var commandInfo = ParsePostCommandData(fullCommand);
                        var result = await api.CreatePost(commandInfo.Item1, commandInfo.Item2);
                        if (!(bool)result.success)
                            return (result.error, null);
                        return ("Post Created! " + result.uri + System.Environment.NewLine + "Run a get timeline to see it!", null);
                }
            }

            public static (string, string[]) ParsePostCommandData(string input)
            {
                // Find the start and end indices of the text between | |
                int startIndex = input.IndexOf('|') + 1;
                int endIndex = input.LastIndexOf('|');

                // Extract the text between | |
                string text = input.Substring(startIndex, endIndex - startIndex).Trim();

                // Extract the remaining part of the input after the text
                string remainingInput = input.Substring(endIndex + 1).Trim();

                // Split the remaining input by spaces to get the file paths
                string[] filePaths = remainingInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Return the extracted text and file paths as a tuple
                return (text, filePaths);
            }
        }
    }
}

