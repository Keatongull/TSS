using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoneDirectory
{
    /// <summary>
    /// Manages all call operations including 2-way calls, 3-way calls, transfers, and conferences
    /// </summary>
    public class CallManager
    {
        private readonly PhoneSystem phoneSystem;
        private readonly List<Call> activeCalls;
        private int nextCallId;

        public CallManager(PhoneSystem phoneSystem)
        {
            this.phoneSystem = phoneSystem;
            this.activeCalls = new List<Call>();
            this.nextCallId = 1;
        }

        public List<Call> ActiveCalls => activeCalls;

        /// <summary>
        /// Initiate a call from caller to target
        /// </summary>
        public string InitiateCall(PhoneEntry caller, PhoneEntry target)
        {
            // Check if caller is in proper state
            var callerState = phoneSystem.GetPhoneState(caller.PhoneNumber);
            if (callerState != PhoneState.OFFHOOK_DIALTONE)
            {
                return "silence";
            }

            // Check for self-call
            if (caller.PhoneNumber == target.PhoneNumber)
            {
                return "denial";
            }

            // Check if target is available
            var targetState = phoneSystem.GetPhoneState(target.PhoneNumber);
            if (targetState != PhoneState.ONHOOK)
            {
                return "busy";
            }

            // Check call limits (max 10 simultaneous calls)
            if (activeCalls.Count >= 10)
            {
                return "busy";
            }

            // Create new call
            var call = new Call(nextCallId++, new List<PhoneEntry> { caller, target });
            activeCalls.Add(call);

            // Update phone states
            phoneSystem.SetPhoneState(caller.PhoneNumber, PhoneState.CALLING);
            phoneSystem.SetPhoneState(target.PhoneNumber, PhoneState.RINGING);

            // Simulate target answering automatically for simplicity
            // In a real system, this would wait for the target to answer
            phoneSystem.SetPhoneState(caller.PhoneNumber, PhoneState.TALKING_2WAY);
            phoneSystem.SetPhoneState(target.PhoneNumber, PhoneState.TALKING_2WAY);

            return $"{caller.Name} is calling {target.Name}. Call connected.";
        }

        /// <summary>
        /// Answer an incoming call
        /// </summary>
        public string AnswerCall(PhoneEntry phone)
        {
            var call = FindCallByParticipant(phone.PhoneNumber);
            if (call == null) return "denial";

            // Update all participants to talking state
            foreach (var participant in call.Participants)
            {
                var state = call.Participants.Count == 2 ? PhoneState.TALKING_2WAY : PhoneState.TALKING_3WAY;
                phoneSystem.SetPhoneState(participant.PhoneNumber, state);
            }

            return $"{phone.Name} answered the call.";
        }

        /// <summary>
        /// Handle phone going onhook (hanging up)
        /// </summary>
        public string HandleOnhook(PhoneEntry phone)
        {
            var call = FindCallByParticipant(phone.PhoneNumber);
            if (call == null)
            {
                // Phone not in a call, just set to onhook
                phoneSystem.SetPhoneState(phone.PhoneNumber, PhoneState.ONHOOK);
                return $"{phone.Name} is now onhook.";
            }

            // Remove phone from call
            call.Participants.Remove(phone);
            phoneSystem.SetPhoneState(phone.PhoneNumber, PhoneState.ONHOOK);

            if (call.Participants.Count == 0)
            {
                // No participants left, end call
                activeCalls.Remove(call);
            }
            else if (call.Participants.Count == 1)
            {
                // Only one participant left, they hear silence
                var remainingPhone = call.Participants[0];
                phoneSystem.SetPhoneState(remainingPhone.PhoneNumber, PhoneState.OFFHOOK_DIALTONE);
                activeCalls.Remove(call);
                return $"{phone.Name} is now onhook. {remainingPhone.Name} hears silence.";
            }
            else if (call.Participants.Count == 2)
            {
                // Two participants left, continue as 2-way call
                foreach (var participant in call.Participants)
                {
                    phoneSystem.SetPhoneState(participant.PhoneNumber, PhoneState.TALKING_2WAY);
                }
            }

            return $"{phone.Name} is now onhook.";
        }

        /// <summary>
        /// Initiate a conference call
        /// </summary>
        public string InitiateConference(PhoneEntry initiator, PhoneEntry target)
        {
            // Check if initiator is in a 2-way call
            var initiatorCall = FindCallByParticipant(initiator.PhoneNumber);
            if (initiatorCall == null || initiatorCall.Participants.Count != 2)
            {
                return "denial";
            }

            var initiatorState = phoneSystem.GetPhoneState(initiator.PhoneNumber);
            if (initiatorState != PhoneState.TALKING_2WAY)
            {
                return "denial";
            }

            // Check if target is available
            var targetState = phoneSystem.GetPhoneState(target.PhoneNumber);
            if (targetState != PhoneState.ONHOOK)
            {
                return "busy";
            }

            // Check if target is already in the same call
            if (initiatorCall.Participants.Any(p => p.PhoneNumber == target.PhoneNumber))
            {
                return $"{target.Name} is already part of that call.";
            }

            // Check if there's already a 3-way call active
            if (activeCalls.Any(c => c.Participants.Count == 3))
            {
                return "busy"; // Only one 3-way call allowed at a time
            }

            // Add target to the call
            initiatorCall.Participants.Add(target);
            
            // Update states
            phoneSystem.SetPhoneState(initiator.PhoneNumber, PhoneState.CONFERENCING);
            phoneSystem.SetPhoneState(target.PhoneNumber, PhoneState.RINGING);

            // Simulate target answering the conference
            foreach (var participant in initiatorCall.Participants)
            {
                phoneSystem.SetPhoneState(participant.PhoneNumber, PhoneState.TALKING_3WAY);
            }

            return $"Conference established: {string.Join(", ", initiatorCall.Participants.Select(p => p.Name))}";
        }

        /// <summary>
        /// Initiate a call transfer
        /// </summary>
        public string InitiateTransfer(PhoneEntry initiator, PhoneEntry target)
        {
            // Check if initiator is in a 2-way call
            var initiatorCall = FindCallByParticipant(initiator.PhoneNumber);
            if (initiatorCall == null || initiatorCall.Participants.Count != 2)
            {
                return "denial";
            }

            var initiatorState = phoneSystem.GetPhoneState(initiator.PhoneNumber);
            if (initiatorState != PhoneState.TALKING_2WAY)
            {
                return "denial";
            }

            // Check if target is available
            var targetState = phoneSystem.GetPhoneState(target.PhoneNumber);
            if (targetState != PhoneState.ONHOOK)
            {
                return "busy";
            }

            // Check if target is already in the same call
            if (initiatorCall.Participants.Any(p => p.PhoneNumber == target.PhoneNumber))
            {
                return "denial";
            }

            // Get the other participant in the call
            var otherParticipant = initiatorCall.Participants.FirstOrDefault(p => p.PhoneNumber != initiator.PhoneNumber);
            if (otherParticipant == null) return "denial";

            // Remove initiator from call and set to transferring state
            initiatorCall.Participants.Remove(initiator);
            phoneSystem.SetPhoneState(initiator.PhoneNumber, PhoneState.TRANSFERRING);

            // Add target to the call
            initiatorCall.Participants.Add(target);
            phoneSystem.SetPhoneState(target.PhoneNumber, PhoneState.RINGING);

            // Simulate target answering the transfer
            phoneSystem.SetPhoneState(otherParticipant.PhoneNumber, PhoneState.TALKING_2WAY);
            phoneSystem.SetPhoneState(target.PhoneNumber, PhoneState.TALKING_2WAY);
            
            // Initiator hears silence (transfer complete)
            phoneSystem.SetPhoneState(initiator.PhoneNumber, PhoneState.OFFHOOK_DIALTONE);

            return $"Transfer completed: {otherParticipant.Name} is now talking to {target.Name}. {initiator.Name} hears silence.";
        }

        /// <summary>
        /// Find a call that includes the specified phone number
        /// </summary>
        public Call? FindCallByParticipant(string phoneNumber)
        {
            return activeCalls.FirstOrDefault(call => 
                call.Participants.Any(p => p.PhoneNumber == phoneNumber));
        }

        /// <summary>
        /// Get call information for status display
        /// </summary>
        public string GetCallInfo(PhoneEntry phone)
        {
            var call = FindCallByParticipant(phone.PhoneNumber);
            if (call == null) return "";

            var otherParticipants = call.Participants
                .Where(p => p.PhoneNumber != phone.PhoneNumber)
                .Select(p => $"{p.PhoneNumber} ({p.Name})")
                .ToList();

            if (otherParticipants.Count == 0) return "";

            var state = phoneSystem.GetPhoneState(phone.PhoneNumber);
            string prefix = state == PhoneState.TALKING_3WAY ? " with " : " with ";
            
            return prefix + string.Join(", ", otherParticipants);
        }

        /// <summary>
        /// Get summary of all active calls for status display
        /// </summary>
        public List<string> GetActiveCallsSummary()
        {
            var summary = new List<string>();
            
            for (int i = 0; i < activeCalls.Count; i++)
            {
                var call = activeCalls[i];
                var participants = string.Join(" <-> ", call.Participants.Select(p => p.Name));
                summary.Add($"  - Call {i + 1}: {participants}");
            }

            return summary;
        }
    }

    /// <summary>
    /// Represents an active call with multiple participants
    /// </summary>
    public class Call
    {
        public int CallId { get; }
        public List<PhoneEntry> Participants { get; }

        public Call(int callId, List<PhoneEntry> participants)
        {
            CallId = callId;
            Participants = participants;
        }
    }
}