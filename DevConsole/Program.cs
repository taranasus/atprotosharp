using System.Text.Json;
using atprotosharp;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var atApi = new API(new HttpRequestHandler());

Console.WriteLine("Get Server Details: ");

Console.WriteLine(JsonSerializer.Serialize(atApi.GetServerParameters()));
Console.WriteLine("");
Console.WriteLine("Get Authentication token");

Console.WriteLine(JsonSerializer.Serialize(atApi.Authenticate("taranode@outlook.com", "NotTellingYou")));

Console.ReadLine();