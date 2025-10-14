using System;
using System.Collections.Generic;

namespace PhoneDirectory
{
    /// <summary>
    /// Handles all user interface operations for the telephone system
    /// </summary>
    public class UserInterface
    {
        /// <summary>
        /// Display the help information with all available commands
        /// </summary>
        public static void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("1. offhook <phone|name>");
            Console.WriteLine("2. onhook <phone|name>");
            Console.WriteLine("3. call <target>");
            Console.WriteLine("4. transfer <target>");
            Console.WriteLine("5. conference <target>");
            Console.WriteLine("6. status");
            Console.WriteLine("7. help");
            Console.WriteLine("8. quit/exit");
        }

        /// <summary>
        /// Display the current system status showing all phones and their states
        /// </summary>
        public static void PrintStatus(PhoneSystem phoneSystem, CallManager? callManager = null)
        {
            Console.WriteLine("System Status:");
            Console.WriteLine("-------------");
            foreach (var entry in phoneSystem.PhoneBook)
            {
                var state = phoneSystem.GetPhoneState(entry.PhoneNumber);
                string callInfo = callManager?.GetCallInfo(entry) ?? "";
                Console.WriteLine($"{entry.PhoneNumber} ({entry.Name}): {state}{callInfo}");
            }

            if (callManager != null)
            {
                var activeCallsSummary = callManager.GetActiveCallsSummary();
                if (activeCallsSummary.Count > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Active Calls: {activeCallsSummary.Count}");
                    foreach (var callSummary in activeCallsSummary)
                    {
                        Console.WriteLine(callSummary);
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Active Calls: 0");
                }
            }
        }

        /// <summary>
        /// Get and validate user input, applying length restrictions and trimming
        /// </summary>
        public static string? GetUserInput()
        {
            Console.Write("\nEnter command (help for list): ");
            string? input = Console.ReadLine();
            if (input == null) return null;

            input = input.Trim();
            if (input.Length > 50)
            {
                input = input.Substring(0, 50);
                Console.WriteLine("Warning: command truncated to 50 characters.");
            }

            return string.IsNullOrWhiteSpace(input) ? null : input;
        }

        /// <summary>
        /// Display the total number of commands processed when exiting
        /// </summary>
        public static void ShowExitMessage(int commandCount)
        {
            Console.WriteLine($"Total commands processed: {commandCount}");
        }
    }
}