using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Get
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

timeline - Gets your timeline of posts and displays it
";
                        return (commandOpstions, commandWords[0] + " ");
                    case 2:
                        switch (commandWords[1])
                        {
                            case "timeline":
                                var result = await api.GetTimeline("reverse-chronological");
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
        }
    }
}

