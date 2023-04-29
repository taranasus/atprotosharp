using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using atprotosharp;
using DevConsole;

var options = new JsonSerializerOptions
{
    WriteIndented = true
};

// See https://aka.ms/new-console-template for more information
var header = $@"
 ▄▄▄▄     ██████  ██ ▄█▀▓██   ██▓   ▄▄▄█████▓▓█████  ██▀███   ███▄ ▄███▓ ██▓ ███▄    █  ▄▄▄       ██▓    
▓█████▄ ▒██    ▒  ██▄█▒  ▒██  ██▒   ▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒▓██▒▀█▀ ██▒▓██▒ ██ ▀█   █ ▒████▄    ▓██▒    
▒██▒ ▄██░ ▓██▄   ▓███▄░   ▒██ ██░   ▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒▓██    ▓██░▒██▒▓██  ▀█ ██▒▒██  ▀█▄  ▒██░    
▒██░█▀    ▒   ██▒▓██ █▄   ░ ▐██▓░   ░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄  ▒██    ▒██ ░██░▓██▒  ▐▌██▒░██▄▄▄▄██ ▒██░    
░▓█  ▀█▓▒██████▒▒▒██▒ █▄  ░ ██▒▓░     ▒██▒ ░ ░▒████▒░██▓ ▒██▒▒██▒   ░██▒░██░▒██░   ▓██░ ▓█   ▓██▒░██████▒
░▒▓███▀▒▒ ▒▓▒ ▒ ░▒ ▒▒ ▓▒   ██▒▒▒      ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░░ ▒░   ░  ░░▓  ░ ▒░   ▒ ▒  ▒▒   ▓▒█░░ ▒░▓  ░
▒░▒   ░ ░ ░▒  ░ ░░ ░▒ ▒░ ▓██ ░▒░        ░     ░ ░  ░  ░▒ ░ ▒░░  ░      ░ ▒ ░░ ░░   ░ ▒░  ▒   ▒▒ ░░ ░ ▒  ░
 ░    ░ ░  ░  ░  ░ ░░ ░  ▒ ▒ ░░       ░         ░     ░░   ░ ░      ░    ▒ ░   ░   ░ ░   ░   ▒     ░ ░   
 ░            ░  ░  ░    ░ ░                    ░  ░   ░            ░    ░           ░       ░  ░    ░  ░
      ░                  ░ ░                                                                             
";
Console.ForegroundColor = ConsoleColor.White;
Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.WriteLine(header);
Console.ResetColor();

var interpretor = new Interpretor();

string returnedCommand = null;

while (true)
{
    if (string.IsNullOrWhiteSpace(returnedCommand))
        Console.WriteLine("");
    Console.Write(interpretor.GetUser() + "> ");
    if (!string.IsNullOrWhiteSpace(returnedCommand))
        Console.Write(returnedCommand);
    else
        returnedCommand = "";

    string command = returnedCommand + Console.ReadLine();
    Console.ForegroundColor = ConsoleColor.Green;
    var commandResut = await interpretor.ProcessCommand(command);
    returnedCommand = commandResut.commandContinuation;
    Console.WriteLine("");
    Console.WriteLine(commandResut.output);
    Console.ResetColor();
}


