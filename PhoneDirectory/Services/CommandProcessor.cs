using System;

namespace PhoneDirectory
{
    /// <summary>
    /// Processes and handles all telephone system commands
    /// </summary>
    public class CommandProcessor
    {
        private readonly PhoneSystem phoneSystem;
        private readonly CallManager callManager;

        public CommandProcessor(PhoneSystem phoneSystem)
        {
            this.phoneSystem = phoneSystem;
            this.callManager = new CallManager(phoneSystem);
        }

        /// <summary>
        /// Handle the offhook command
        /// </summary>
        public void HandleOffhook(string identifier)
        {
            var entry = phoneSystem.FindEntry(identifier);
            if (entry == null)
            {
                Console.WriteLine("denial");
                return;
            }
            
            var state = phoneSystem.GetPhoneState(entry.PhoneNumber);
            if (state != PhoneState.ONHOOK)
            {
                Console.WriteLine($"{entry.Name} is already offhook.");
                return;
            }
            
            phoneSystem.SetPhoneState(entry.PhoneNumber, PhoneState.OFFHOOK_DIALTONE);
            Console.WriteLine($"{entry.Name} is now offhook (dialtone).");
        }

        /// <summary>
        /// Handle the onhook command
        /// </summary>
        public void HandleOnhook(string identifier)
        {
            var entry = phoneSystem.FindEntry(identifier);
            if (entry == null)
            {
                Console.WriteLine("denial");
                return;
            }
            
            var state = phoneSystem.GetPhoneState(entry.PhoneNumber);
            if (state == PhoneState.ONHOOK)
            {
                Console.WriteLine($"{entry.Name} is already onhook.");
                return;
            }
            
            string result = callManager.HandleOnhook(entry);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Handle the call command
        /// </summary>
        public void HandleCall(string identifier)
        {
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Find the calling phone (phone in OFFHOOK_DIALTONE state)
            PhoneEntry? caller = null;
            foreach (var entry in phoneSystem.PhoneBook)
            {
                if (phoneSystem.GetPhoneState(entry.PhoneNumber) == PhoneState.OFFHOOK_DIALTONE)
                {
                    caller = entry;
                    break;
                }
            }

            if (caller == null)
            {
                Console.WriteLine("silence");
                return;
            }
            
            string result = callManager.InitiateCall(caller, targetEntry);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Handle the transfer command
        /// Format: transfer <initiator> <target>
        /// </summary>
        public void HandleTransfer(string[] parts)
        {
            // Check if we have both initiator and target
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid command syntax. Use: transfer <initiator> <target>");
                return;
            }

            var initiatorEntry = phoneSystem.FindEntry(parts[1]);
            if (initiatorEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            var targetEntry = phoneSystem.FindEntry(parts[2]);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Check initiator is in TALKING_2WAY state
            if (phoneSystem.GetPhoneState(initiatorEntry.PhoneNumber) != PhoneState.TALKING_2WAY)
            {
                Console.WriteLine("silence");
                return;
            }
            
            string result = callManager.InitiateTransfer(initiatorEntry, targetEntry);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Handle the conference command
        /// </summary>
        public void HandleConference(string identifier)
        {
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Find the phone trying to conference (must be in TALKING_2WAY state)
            PhoneEntry? initiator = null;
            foreach (var entry in phoneSystem.PhoneBook)
            {
                if (phoneSystem.GetPhoneState(entry.PhoneNumber) == PhoneState.TALKING_2WAY)
                {
                    initiator = entry;
                    break;
                }
            }

            if (initiator == null)
            {
                Console.WriteLine("silence");
                return;
            }
            
            string result = callManager.InitiateConference(initiator, targetEntry);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Get the call manager for status display purposes
        /// </summary>
        public CallManager CallManager => callManager;
    }
}