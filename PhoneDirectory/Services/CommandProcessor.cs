using System;
using System.Linq;

namespace PhoneDirectory
{
    public class CommandProcessor
    {
        private readonly PhoneSystem phoneSystem;
        private readonly CallManager callManager;

        public CommandProcessor(PhoneSystem phoneSystem)
        {
            this.phoneSystem = phoneSystem;
            this.callManager = new CallManager(phoneSystem);
        }

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

        public void HandleOnhook(string identifier)
        {
            var entry = phoneSystem.FindEntry(identifier);
            if (entry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // If phone is in a call, leave that call
            if (phoneSystem.IsPhoneInCall(entry.PhoneNumber))
            {
                Console.WriteLine($"{entry.Name} hung up.");
                phoneSystem.LeaveCall(entry.PhoneNumber);
                return;
            }

            var state = phoneSystem.GetPhoneState(entry.PhoneNumber);
            if (state == PhoneState.ONHOOK)
            {
                Console.WriteLine($"{entry.Name} is already onhook.");
                return;
            }

            phoneSystem.SetPhoneState(entry.PhoneNumber, PhoneState.ONHOOK);
            Console.WriteLine($"{entry.Name} is now onhook.");
        }

        public void HandleCall(string identifier)
        {
            // Find the caller (someone with OFFHOOK_DIALTONE)
            var caller = phoneSystem.PhoneBook.FirstOrDefault(p =>
                phoneSystem.GetPhoneState(p.PhoneNumber) == PhoneState.OFFHOOK_DIALTONE);

            if (caller == null)
            {
                Console.WriteLine("silence");
                return;
            }

            var target = phoneSystem.FindEntry(identifier);
            if (target == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Self-call check
            if (caller.PhoneNumber == target.PhoneNumber)
            {
                Console.WriteLine("denial");
                return;
            }

            // Target busy?
            if (phoneSystem.IsPhoneInCall(target.PhoneNumber))
            {
                Console.WriteLine("busy");
                return;
            }

            // Caller busy?
            if (phoneSystem.IsPhoneInCall(caller.PhoneNumber))
            {
                Console.WriteLine("busy");
                return;
            }

            // Target ONHOOK?
            if (phoneSystem.GetPhoneState(target.PhoneNumber) == PhoneState.ONHOOK)
            {
                Console.WriteLine("silence");
                return;
            }

            // Start the call
            phoneSystem.StartCall(caller.PhoneNumber, target.PhoneNumber);
            Console.WriteLine($"{caller.Name} is now in a call with {target.Name}.");
        }

        public void HandleTransfer(string identifier)
        {
            // Check if target exists
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Prompt for transferer
            Console.Write("Who is tranferring the call? ");
            string? transfererInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(transfererInput))
            {
                Console.WriteLine("denial");
                return;
            }

            // Check that transferer is in the call
            var transfererEntry = phoneSystem.FindEntry(transfererInput);
            if (transfererEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Prevent transferring call to self
            if (transfererEntry.PhoneNumber == targetEntry.PhoneNumber)
            {
                Console.WriteLine("denial");
                return;
            }

            // Ensure the call is a valid 2-way call
            var call = phoneSystem.GetCallForPhone(transfererEntry.PhoneNumber);
            if (call == null || call.Count != 2)
            {
                Console.WriteLine("denial");
                return;
            }

            // Prevent transferring a call to someone already in the same call
            if (call.Contains(targetEntry.PhoneNumber))
            {
                Console.WriteLine($"{targetEntry.Name} is already part of that call.");
                return;
            }

            // Attempt the transfer
            if (!phoneSystem.TryTransferCall(transfererEntry.PhoneNumber, targetEntry.PhoneNumber))
            {
                Console.WriteLine("denial");
                return;
            }
        }

        public void HandleConference(string identifier)
        {
            // Find who’s requesting conference
            var requester = phoneSystem.PhoneBook.FirstOrDefault(p =>
                phoneSystem.IsPhoneInCall(p.PhoneNumber));

            if (requester == null)
            {
                Console.WriteLine("silence");
                return;
            }

            var newParticipant = phoneSystem.FindEntry(identifier);
            if (newParticipant == null)
            {
                Console.WriteLine("denial");
                return;
            }

            // Self-conference
            if (requester.PhoneNumber == newParticipant.PhoneNumber)
            {
                Console.WriteLine("denial");
                return;
            }

            // If new participant is already in another call
            if (phoneSystem.IsPhoneInCall(newParticipant.PhoneNumber))
            {
                Console.WriteLine("busy");
                return;
            }

            // If new participant is ONHOOK → can’t join
            if (phoneSystem.GetPhoneState(newParticipant.PhoneNumber) == PhoneState.ONHOOK)
            {
                Console.WriteLine("silence");
                return;
            }

            // Try adding them to existing call
            if (phoneSystem.TryAddToCall(requester.PhoneNumber, newParticipant.PhoneNumber))
            {
                Console.WriteLine($"{newParticipant.Name} joined the conference.");
            }
            else
            {
                Console.WriteLine("busy (conference full).");
            }
        }

        /// <summary>
        /// Get the call manager for status display purposes
        /// </summary>
        public CallManager CallManager => callManager;
    }
}
