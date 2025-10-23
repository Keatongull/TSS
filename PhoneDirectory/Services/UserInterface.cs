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
            Console.WriteLine("4. conference <target>      - Add another phone to a call (prompts for selection if multiple calls are active)");
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

            var callsField = typeof(PhoneSystem)
                .GetField("activeCalls", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var activeCalls = callsField != null
                ? (List<HashSet<string>>)callsField.GetValue(phoneSystem)!
                : new List<HashSet<string>>();

            foreach (var entry in phoneSystem.PhoneBook)
            {
                var state = phoneSystem.GetPhoneState(entry.PhoneNumber);

                // Check if this phone is in any active call
                var call = activeCalls.FirstOrDefault(c => c.Contains(entry.PhoneNumber));

                if (call == null)
                {
                    Console.WriteLine($"{entry.PhoneNumber} ({entry.Name}): {state}");
                }
                else
                {
                    string status;
                    if (call.Count == 2)
                    {
                        // Find the other participant
                        var otherNumber = call.First(n => n != entry.PhoneNumber);
                        var otherEntry = phoneSystem.FindEntry(otherNumber);
                        status = $"TALKING_2WAY with {otherEntry?.PhoneNumber} ({otherEntry?.Name})";
                    }
                    else if (call.Count == 3)
                    {
                        var others = call.Where(n => n != entry.PhoneNumber)
                                        .Select(n =>
                                        {
                                            var e = phoneSystem.FindEntry(n);
                                            return $"{e?.PhoneNumber} ({e?.Name})";
                                        });
                        status = $"TALKING_3WAY with {string.Join(", ", others)}";
                    }
                    else
                    {
                        status = state.ToString();
                    }

                    Console.WriteLine($"{entry.PhoneNumber} ({entry.Name}): {status}");
                }
            }

             Console.WriteLine($"\nActive Calls: {activeCalls.Count}");

            int callIndex = 1;
            foreach (var call in activeCalls)
            {
                // Convert to Name <-> Name <-> Name format
                var participants = call.Select(num =>
                {
                    var entry = phoneSystem.FindEntry(num);
                    return entry != null ? entry.Name : num;
                });

                Console.WriteLine($"    - Call {callIndex++}: {string.Join(" <-> ", participants)}");
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
