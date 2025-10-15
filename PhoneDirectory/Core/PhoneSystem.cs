using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoneDirectory
{
    public class PhoneSystem
    {
        private readonly List<PhoneEntry> phoneBook;
        private readonly Dictionary<string, PhoneState> phoneStates;
        private readonly List<HashSet<string>> activeCalls = new();

        public PhoneSystem(List<PhoneEntry> phoneBook)
        {
            this.phoneBook = phoneBook;
            phoneStates = new Dictionary<string, PhoneState>();

            foreach (var entry in phoneBook)
            {
                phoneStates[entry.PhoneNumber] = PhoneState.ONHOOK;
            }
        }

        public List<PhoneEntry> PhoneBook => phoneBook;
        public Dictionary<string, PhoneState> PhoneStates => phoneStates;

        public PhoneEntry? FindEntry(string identifier)
        {
            foreach (var entry in phoneBook)
            {
                if (entry.PhoneNumber == identifier)
                    return entry;
            }
            foreach (var entry in phoneBook)
            {
                if (entry.Name.Equals(identifier, StringComparison.OrdinalIgnoreCase))
                    return entry;
            }
            return null;
        }

        public PhoneState GetPhoneState(string phoneNumber)
        {
            return phoneStates.TryGetValue(phoneNumber, out var state) ? state : PhoneState.ONHOOK;
        }

        public void SetPhoneState(string phoneNumber, PhoneState state)
        {
            if (phoneStates.ContainsKey(phoneNumber))
                phoneStates[phoneNumber] = state;
        }

        // === Call Management ===
        public bool IsPhoneInCall(string phoneNumber) => activeCalls.Any(c => c.Contains(phoneNumber));

        public HashSet<string>? GetCallForPhone(string phoneNumber) =>
            activeCalls.FirstOrDefault(c => c.Contains(phoneNumber));

        public void StartCall(string phone1, string phone2)
        {
            var call = new HashSet<string> { phone1, phone2 };
            activeCalls.Add(call);

            SetPhoneState(phone1, PhoneState.TALKING_2WAY);
            SetPhoneState(phone2, PhoneState.TALKING_2WAY);
        }

        public bool TryAddToCall(string existingPhone, string newPhone)
        {
            var call = GetCallForPhone(existingPhone);
            if (call == null || call.Count >= 3)
                return false;

            call.Add(newPhone);
            foreach (var num in call)
                SetPhoneState(num, PhoneState.TALKING_3WAY);

            return true;
        }

        public void LeaveCall(string phoneNumber)
        {
            var call = GetCallForPhone(phoneNumber);
            if (call == null) return;

            call.Remove(phoneNumber);
            SetPhoneState(phoneNumber, PhoneState.ONHOOK);

            if (call.Count == 1)
            {
                var remaining = call.First();
                Console.WriteLine($"{FindEntry(remaining)?.Name} hears silence.");
                activeCalls.Remove(call);
                SetPhoneState(remaining, PhoneState.OFFHOOK_DIALTONE);
            }
            else if (call.Count == 2)
            {
                foreach (var num in call)
                    SetPhoneState(num, PhoneState.TALKING_2WAY);
            }
            else if (call.Count == 0)
            {
                activeCalls.Remove(call);
            }
        }

        public bool TryTransferCall(string transferer, string target)
        {
            // Allow only valid, 2-way calls
            var call = GetCallForPhone(transferer);
            if (call == null || call.Count != 2)
            {
                return false;
            }

            // Deny if the target is already in a call
            if (IsPhoneInCall(target))
            {
                return false;
            }
                
            // Check if target exists in phoneStates and is ONHOOK (available to receive the transfer)
            if (!phoneStates.TryGetValue(target, out var targetState) || targetState != PhoneState.ONHOOK)
            {
                return false;
            }

            var other = call.First(p => p != transferer);

            // Remove the transferer from the call
            call.Remove(transferer);
            SetPhoneState(transferer, PhoneState.ONHOOK);
            Console.WriteLine($"{FindEntry(transferer)?.Name} hears silence.");

            // Simulate ringing
            Console.WriteLine($"{FindEntry(target)?.Name} is ringing...");
            Console.Write("Answer call? (y/n): ");
            string? response = Console.ReadLine()?.Trim();

            // Target answers
            if (response == "y")
            {
                call.Add(target);
                foreach (var num in call)
                    SetPhoneState(num, PhoneState.TALKING_2WAY);
                Console.WriteLine($"{FindEntry(target)?.Name} joined the call with {FindEntry(other)?.Name}.");
                return true;
            }
            //  Target declines
            else
            {
                call.Add(transferer);
                foreach (var num in call)
                    SetPhoneState(num, PhoneState.TALKING_2WAY);
                Console.WriteLine($"Transfer failed. Original call between {FindEntry(transferer)?.Name} and {FindEntry(other)?.Name} resumed.");
                return false;
            }

        }
    }
}
