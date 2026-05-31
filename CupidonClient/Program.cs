using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5100/cupidonHub", options =>
    {
        options.HttpMessageHandlerFactory = _ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    })
    .Build();

connection.On<string, string, int, string, string>("LetterArrived",
    async (username, town, age, phone, message) =>
    {
        Console.WriteLine("<3<3<3<3<3<3     LOVE LETTER ARRIVED!     <3<3<3<3<3<3");
        Console.WriteLine($"   from: {username}");
        Console.WriteLine($"   town: {town}");
        Console.WriteLine($"   age: {age}");
        if (phone != "hidden")
            Console.WriteLine($"   phone: {phone}");
        Console.WriteLine($"   message: {message}");
        Console.WriteLine("\npress ENTER to accept this letter...");

        Console.ReadLine();
        await connection.InvokeAsync("AcceptLetter");
        Console.WriteLine("letter accepted! you can now receive new letters\n");
    });

connection.On<string>("Registered", msg =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nOK: {msg}");
    Console.ResetColor();
});

connection.On<string>("Error", msg =>
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\nError: {msg}");
    Console.ResetColor();
});

connection.On<string>("Blocked", msg =>
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\nBlocked: {msg}");
    Console.ResetColor();
});

await connection.StartAsync();
//Console.WriteLine("connected to Cupidon server!\n");

Console.Write("username: ");
string username = Console.ReadLine() ?? "";
while (string.IsNullOrWhiteSpace(username))
{
    Console.Write("username cannot be empty. try again: ");
    username = Console.ReadLine() ?? "";
}

Console.Write("town: ");
string town = Console.ReadLine() ?? "";
while (string.IsNullOrWhiteSpace(town))
{
    Console.Write("town cannot be empty. try again: ");
    town = Console.ReadLine() ?? "";
}

int age = 0;
Console.Write("age: ");
while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
{
    Console.Write("age cannot be less than 1. try again: ");
}

Console.Write("phone number: ");
string phone = Console.ReadLine() ?? "";
while (string.IsNullOrWhiteSpace(phone) || !phone.All(char.IsDigit))
{
    Console.Write("phone cannot be empty and must containt only digits. try again: ");
    phone = Console.ReadLine() ?? "";
}

await connection.InvokeAsync("InitSinglePerson", username, town, age, phone);

Console.WriteLine("\nCommands:");
Console.WriteLine("   /block <username> - block a user");
Console.WriteLine("   /quit             - exit the application");
Console.WriteLine("\nwaiting for love letters...\n");

// Main input loop
while (true)
{
    var input = (Console.ReadLine() ?? "").Trim();

    if (input == "/quit")
    {
        Console.WriteLine("bye! :(");
        break;
    }
    else if (input.StartsWith("/block "))
    {
        var toBlock = input.Substring(7).Trim();
        if (string.IsNullOrWhiteSpace(toBlock))
        {
            Console.WriteLine("Usage: /block <username>");
        }
        else
        {
            await connection.InvokeAsync("Block", toBlock);
        }
    }
    else if (!string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Unknown command. Available commands: /block <username>, /quit");
    }
}