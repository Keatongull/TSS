using System;

namespace PhoneDirectory
{
    /// <summary>
    /// Processes and handles all telephone system commands
    /// </summary>
    public class CommandProcessor
    {
        private readonly PhoneSystem phoneSystem;

        public CommandProcessor(PhoneSystem phoneSystem)
        {
            this.phoneSystem = phoneSystem;
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
            
            phoneSystem.SetPhoneState(entry.PhoneNumber, PhoneState.ONHOOK);
            Console.WriteLine($"{entry.Name} is now onhook.");
        }

        /// <summary>
        /// Handle the call command (placeholder for steps 1-3)
        /// </summary>
        public void HandleCall(string identifier)
        {
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }
            
            // For steps 1-3, we just implement basic validation
            // Commands on ONHOOK phones (other than offhook) result in "silence"
            // This will be fully implemented in step 4 with proper call logic
            Console.WriteLine("silence");
        }

        /// <summary>
        /// Handle the transfer command (placeholder for steps 1-3)
        /// </summary>
        public void HandleTransfer(string identifier)
        {
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }
            
            // Commands on ONHOOK phones (other than offhook) result in "silence"
            // This will be fully implemented in step 5 with proper transfer logic
            Console.WriteLine("silence");
        }

        /// <summary>
        /// Handle the conference command (placeholder for steps 1-3)
        /// </summary>
        public void HandleConference(string identifier)
        {
            var targetEntry = phoneSystem.FindEntry(identifier);
            if (targetEntry == null)
            {
                Console.WriteLine("denial");
                return;
            }
            
            // Commands on ONHOOK phones (other than offhook) result in "silence"
            // This will be fully implemented in step 5 with proper conference logic
            Console.WriteLine("silence");
        }
    }
}