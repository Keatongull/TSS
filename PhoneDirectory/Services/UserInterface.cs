using System;
using System.Collections.Generic;
using System.Linq;

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
            Console.WriteLine("1. offhook <phone|name>     - Take a phone offhook (begin dialing)");
            Console.WriteLine("2. onhook <phone|name>      - Hang up or return phone to onhook");
            Console.WriteLine("3. call <target>            - Call another phone or name");
            Console.WriteLine("4. conference <target>      - Add another phone to the current call (max 3)");
            Console.WriteLine("5. transfer <target>        - (future) Transfer a current call");
            Console.WriteLine("6. status                   - Show all phone states");
            Console.WriteLine("7. show calls               - Display all active calls (debugging)");
            Console.WriteLine("8. help                     - Display this help message");
            Console.WriteLine("9. quit/exit                - Exit the system");
        }

        /// <summary>
        /// Display the current system status showing all phones and their states
        /// </summary>
        public static void PrintStatus(PhoneSystem phoneSystem)
        {
            Console.WriteLine("\nSystem Status:");
            Console.WriteLine("-------------");

            foreach (var entry in phoneSystem.PhoneBook)
            {
                var state = phoneSystem.GetPhoneState(entry.PhoneNumber);
                Console.WriteLine($"{entry.PhoneNumber} ({entry.Name}): {state}");
            }
        }

        /// <summary>
        /// Display all active calls (for debugging or testing)
        /// </summary>
        public static void ShowActiveCalls(PhoneSystem phoneSystem)
        {
            var callsField = typeof(PhoneSystem)
                .GetField("activeCalls", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (callsField == null)
            {
                Console.WriteLine("Unable to access active calls.");
                return;
            }

            var activeCalls = (List<HashSet<string>>)callsField.GetValue(phoneSystem)!;

            if (activeCalls.Count == 0)
            {
                Console.WriteLine("\nNo active calls.");
                return;
            }

            Console.WriteLine("\nActive Calls:");
            Console.WriteLine("-------------");

            int index = 1;
            foreach (var call in activeCalls)
            {
                var participants = call.Select(num =>
                {
                    var entry = phoneSystem.FindEntry(num);
                    return entry != null ? $"{entry.Name} ({num})" : num;
                });

                Console.WriteLine($"{index++}. {string.Join(", ", participants)}");
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
            Console.WriteLine($"\nTotal commands processed: {commandCount}");
        }
    }
}
