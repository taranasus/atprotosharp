using System.Text.Json;
using atprotosharp;

var options = new JsonSerializerOptions
{
    WriteIndented = true
};

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var atApi = new API(new HttpRequestHandler());

Console.WriteLine("Get Server Details: ");

Console.WriteLine(JsonSerializer.Serialize(await atApi.GetServerParameters(), options));
Console.WriteLine("");
Console.WriteLine("Get Authentication token");

var userCredentials = ReadUserAndPasswordFromJsonFile("credentials/default.json");

// Looks like different threads don't share the same global variables. Which means that each request has to be contained within itself.
// This would probably be easier synchronously
await atApi.Connect(userCredentials.user, userCredentials.password);

Console.WriteLine("Finished connecting! Awaiting User input");
//Console.ReadLine();

var sessionRefereshResult = await atApi.GetSession();

Console.WriteLine("SESSION REFERESH: " + JsonSerializer.Serialize(sessionRefereshResult, options));

Console.ReadLine();

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