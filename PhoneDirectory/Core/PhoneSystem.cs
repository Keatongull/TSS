using System;
using System.Collections.Generic;

namespace PhoneDirectory
{
    /// <summary>
    /// Manages the telephone system state and operations
    /// </summary>
    public class PhoneSystem
    {
        private readonly List<PhoneEntry> phoneBook;
        private readonly Dictionary<string, PhoneState> phoneStates;

        public PhoneSystem(List<PhoneEntry> phoneBook)
        {
            this.phoneBook = phoneBook;
            this.phoneStates = new Dictionary<string, PhoneState>();
            
            // Initialize all phones to ONHOOK state
            foreach (var entry in phoneBook)
            {
                phoneStates[entry.PhoneNumber] = PhoneState.ONHOOK;
            }
        }

        public List<PhoneEntry> PhoneBook => phoneBook;
        public Dictionary<string, PhoneState> PhoneStates => phoneStates;

        /// <summary>
        /// Find a phone entry by phone number or name (case-insensitive)
        /// </summary>
        public PhoneEntry? FindEntry(string identifier)
        {
            // Try phone number first
            foreach (var entry in phoneBook)
            {
                if (entry.PhoneNumber == identifier)
                    return entry;
            }
            // Try name (case-insensitive)
            foreach (var entry in phoneBook)
            {
                if (entry.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase))
                    return entry;
            }
            return null;
        }

        /// <summary>
        /// Get the current state of a phone
        /// </summary>
        public PhoneState GetPhoneState(string phoneNumber)
        {
            return phoneStates.TryGetValue(phoneNumber, out var state) ? state : PhoneState.ONHOOK;
        }

        /// <summary>
        /// Set the state of a phone
        /// </summary>
        public void SetPhoneState(string phoneNumber, PhoneState state)
        {
            if (phoneStates.ContainsKey(phoneNumber))
            {
                phoneStates[phoneNumber] = state;
            }
        }
    }
}