using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhoneDirectory
{
    public class PhoneFileLoader
    {
        private HashSet<string> usedPhoneNumbers;
        private HashSet<string> usedNames;

        public PhoneFileLoader()
        {
            usedPhoneNumbers = new HashSet<string>();
            usedNames = new HashSet<string>();
        }

    public List<PhoneEntry>? LoadPhoneEntries(string fileName, int maxEntries)
        {
            // Check if file exists
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Error: phones.txt not found");
                return null;
            }

            List<PhoneEntry> entries = new List<PhoneEntry>();
            int lineNumber = 0;
            int validEntriesLoaded = 0;
            bool maxEntriesWarningShown = false;

            try
            {
                string[] lines = File.ReadAllLines(fileName);

                foreach (string line in lines)
                {
                    lineNumber++;

                    // Check if we've already loaded the maximum entries
                    if (validEntriesLoaded >= maxEntries)
                    {
                        if (!maxEntriesWarningShown)
                        {
                            Console.WriteLine("Warning: additional lines skipped.");
                            maxEntriesWarningShown = true;
                        }
                        continue;
                    }

                    // Process the line
                    PhoneEntry? entry = ProcessLine(line, lineNumber);
                    if (entry != null)
                    {
                        entries.Add(entry);
                        validEntriesLoaded++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }

            // Check if any valid entries were loaded
            if (entries.Count == 0)
            {
                Console.WriteLine("Error: No valid phone entries found");
                return null;
            }

            return entries;
        }

        private PhoneEntry? ProcessLine(string line, int lineNumber)
        {
            // Skip empty lines and comments
            if (IsEmptyOrComment(line))
            {
                return null;
            }

            // Parse the line
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Check for correct number of fields
            if (parts.Length != 2)
            {
                Console.WriteLine($"Unable to read line {lineNumber}: '{line}' – reason: incorrect number of fields.");
                return null;
            }

            string phoneNumber = parts[0];
            string name = parts[1];

            // Validate phone number and name
            if (!IsValidPhoneNumber(phoneNumber) || !IsValidName(name))
            {
                Console.WriteLine($"Warning: invalid format on line {lineNumber}, skipping.");
                return null;
            }

            // Check for duplicate phone number
            if (IsDuplicatePhoneNumber(phoneNumber))
            {
                Console.WriteLine($"Warning: duplicate phone number on line {lineNumber}, using first occurrence.");
                return null;
            }

            // Check for duplicate name
            if (IsDuplicateName(name))
            {
                Console.WriteLine($"Warning: duplicate name on line {lineNumber}, using first occurrence.");
                return null;
            }

            // Add to tracking sets
            usedPhoneNumbers.Add(phoneNumber);
            usedNames.Add(name.ToLower());

            // Return valid entry
            return new PhoneEntry { PhoneNumber = phoneNumber, Name = name };
        }

        private bool IsEmptyOrComment(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Must be exactly 5 digits
            if (phoneNumber.Length != 5)
                return false;

            // All characters must be digits
            return phoneNumber.All(char.IsDigit);
        }

        private bool IsValidName(string name)
        {
            // Must be 1-12 characters
            if (name.Length < 1 || name.Length > 12)
                return false;

            // All characters must be alphabetic
            return name.All(char.IsLetter);
        }

        private bool IsDuplicatePhoneNumber(string phoneNumber)
        {
            return usedPhoneNumbers.Contains(phoneNumber);
        }

        private bool IsDuplicateName(string name)
        {
            return usedNames.Contains(name.ToLower());
        }
    }
}