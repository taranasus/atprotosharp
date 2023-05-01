using atprotosharp;

namespace DevConsole
{
    public partial class Interpretor
    {
        public static class Upload
        {
            public static async Task<(string? output, string? commandContinuation)> Command(string[] commandWords, API api)
            {
                switch (commandWords.Length)
                {
                    case 0:
                        return ("How the fuck did this happen???", null);
                    case 1:
                        return ("Please type in the filepath to the file you wish to upload ", commandWords[0] + " ");
                    default:
                        var result = await api.UploadMedia(commandWords[1]);
                        if (!(bool)result.success)
                            return (result.error, null);
                        string type = result.blob["$type"];
                        string link = result.blob["ref"]["$link"];
                        string mimeType = result.blob["mimeType"];
                        int size = result.blob["size"];
                        return (@$"File Uploaded Succesfully!
Type: {type}
Link: {link}
MimeType: {mimeType}
Size: {size.ToString()}", null);
                }
            }
        }
    }
}

