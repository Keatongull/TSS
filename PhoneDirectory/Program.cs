using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PhoneDirectory
{
    /// <summary>
    /// Main program entry point for the Telephone Switching System (TSS)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Get the directory where the executable is located
            string? exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Navigate up from bin/Debug/net9.0 to the project root and then to Data folder
            string projectRoot = Path.GetFullPath(Path.Combine(exeDirectory!, "..", "..", ".."));
            string fileName = Path.Combine(projectRoot, "Data", "phones.txt");
            const int maxEntries = 20;

            // Load phone entries using the file loader
            PhoneFileLoader loader = new PhoneFileLoader();
            List<PhoneEntry>? phoneBook = loader.LoadPhoneEntries(fileName, maxEntries);

            // Check if loading was successful
            if (phoneBook == null || phoneBook.Count == 0)
            {
                // Error already displayed by loader
                return;
            }

            Console.WriteLine($"Successfully loaded {phoneBook.Count} phone entries.");

            // Initialize the phone system and command processor
            PhoneSystem phoneSystem = new PhoneSystem(phoneBook);
            CommandProcessor commandProcessor = new CommandProcessor(phoneSystem);

            // Main command loop
            int commandCount = 0;
            while (true)
            {
                string? input = UserInterface.GetUserInput();
                if (input == null) break;

                commandCount++;
                
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string cmd = parts[0].ToLower();

                // Process the command
                switch (cmd)
                {
                    case "help":
                    case "7":
                        UserInterface.PrintHelp();
                        break;
                    case "quit":
                    case "exit":
                    case "8":
                        UserInterface.ShowExitMessage(commandCount);
                        return;
                    case "status":
                    case "6":
                        UserInterface.PrintStatus(phoneSystem, commandProcessor.CallManager);
                        break;
                    case "offhook":
                    case "1":
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Invalid command syntax.");
                            break;
                        }
                        commandProcessor.HandleOffhook(parts[1]);
                        break;
                    case "onhook":
                    case "2":
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Invalid command syntax.");
                            break;
                        }
                        commandProcessor.HandleOnhook(parts[1]);
                        break;
                    case "call":
                    case "3":
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Invalid command syntax.");
                            break;
                        }
                        commandProcessor.HandleCall(parts[1]);
                        break;
                    case "transfer":
                    case "4":
                        commandProcessor.HandleTransfer(parts);
                        break;
                    case "conference":
                    case "5":
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Invalid command syntax.");
                            break;
                        }
                        commandProcessor.HandleConference(parts[1]);
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }
    }
}