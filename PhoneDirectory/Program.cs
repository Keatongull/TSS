using System;
using System.Collections.Generic;

namespace PhoneDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "phones.txt";
            const int maxEntries = 20;

            // Load phone entries using the file loader
            PhoneFileLoader loader = new PhoneFileLoader();
            List<PhoneEntry> phoneBook = loader.LoadPhoneEntries(fileName, maxEntries);

            // Check if loading was successful
            if (phoneBook == null)
            {
                // Error already displayed by loader
                return;
            }

            Console.WriteLine($"Successfully loaded {phoneBook.Count} phone entries.");

            // TODO: Add main program logic here (menu, search, etc.)
        }
    }

    // Simple data structure to hold phone entries
    class PhoneEntry
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
    }
}