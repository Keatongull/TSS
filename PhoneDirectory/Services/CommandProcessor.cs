using System;
using System.Linq;

namespace PhoneDirectory
{
    public class CommandProcessor
    {
        private readonly PhoneSystem phoneSystem;

        public CommandProcessor(PhoneSystem phoneSystem)
        {
            this.phoneSystem = phoneSystem;
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
            var twoWayCalls = phoneSystem.GetTwoWayCalls();

            if (twoWayCalls.Count == 0)
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

            // If new participant is already in another call
            if (phoneSystem.IsPhoneInCall(newParticipant.PhoneNumber))
            {
                Console.WriteLine("busy");
                return;
            }

            // If new participant is ONHOOK → can't join
            if (phoneSystem.GetPhoneState(newParticipant.PhoneNumber) == PhoneState.ONHOOK)
            {
                Console.WriteLine("silence");
                return;
            }

            HashSet<string> selectedCall;
            PhoneEntry? requester = null;

            if (twoWayCalls.Count == 1)
            {
                // Single call - use existing logic
                selectedCall = twoWayCalls[0];
                requester = phoneSystem.FindEntry(selectedCall.First());
            }
            else
            {
                // Multiple calls - show menu and let user choose
                Console.WriteLine("Multiple active calls found:");
                for (int i = 0; i < twoWayCalls.Count; i++)
                {
                    var call = twoWayCalls[i];
                    var participants = call.Select(num => phoneSystem.FindEntry(num)?.Name ?? num).ToList();
                    Console.WriteLine($"{i + 1}. {participants[0]} <-> {participants[1]}");
                }

                Console.Write($"Which call should {newParticipant.Name} join? (1-{twoWayCalls.Count}): ");
                string? response = Console.ReadLine()?.Trim();

                if (int.TryParse(response, out int choice) && choice >= 1 && choice <= twoWayCalls.Count)
                {
                    selectedCall = twoWayCalls[choice - 1];
                    requester = phoneSystem.FindEntry(selectedCall.First());
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                    return;
                }
            }

            // Check for self-conference
            if (selectedCall.Contains(newParticipant.PhoneNumber))
            {
                Console.WriteLine($"{newParticipant.Name} is already part of that call.");
                return;
            }

            // Try adding them to the selected call
            if (phoneSystem.TryAddToCall(requester!.PhoneNumber, newParticipant.PhoneNumber))
            {
                Console.WriteLine($"{newParticipant.Name} joined the conference.");
            }
            else
            {
                Console.WriteLine("busy (conference full).");
            }
        }
    }
}
