using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneDirectory;

namespace TSS.Tests
{
    [TestClass]
    public class CommandProcessorTests
    {
        // White-box tests for CommandProcessor
        // Uses a mock PhoneSystem to verify command logic and error handling


        private class LoggingPhoneSystem : PhoneDirectory.PhoneSystem
        {
            public List<string> Log = new();
            public LoggingPhoneSystem(List<PhoneDirectory.PhoneEntry> entries) : base(entries) { }
            public new PhoneDirectory.PhoneEntry? FindEntry(string identifier)
            {
                Log.Add($"FindEntry:{identifier}");
                return base.FindEntry(identifier);
            }
            public new PhoneDirectory.PhoneState GetPhoneState(string phoneNumber)
            {
                Log.Add($"GetPhoneState:{phoneNumber}");
                return base.GetPhoneState(phoneNumber);
            }
            public new void SetPhoneState(string phoneNumber, PhoneDirectory.PhoneState state)
            {
                Log.Add($"SetPhoneState:{phoneNumber}:{state}");
                base.SetPhoneState(phoneNumber, state);
            }
            public new bool IsPhoneInCall(string phoneNumber)
            {
                Log.Add($"IsPhoneInCall:{phoneNumber}");
                return base.IsPhoneInCall(phoneNumber);
            }
            public new void LeaveCall(string phoneNumber)
            {
                Log.Add($"LeaveCall:{phoneNumber}");
                base.LeaveCall(phoneNumber);
            }
            public new void StartCall(string phone1, string phone2)
            {
                Log.Add($"StartCall:{phone1}:{phone2}");
                base.StartCall(phone1, phone2);
            }
        }

        [TestMethod]
        public void HandleOffhook_DenialIfEntryNotFound()
        {
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry>());
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOffhook("notfound");
            Assert.IsTrue(sw.ToString().Contains("denial"));
        }

        [TestMethod]
        public void HandleOffhook_AlreadyOffhook()
        {
            var entry = new PhoneDirectory.PhoneEntry { PhoneNumber = "123", Name = "Alice" };
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry> { entry });
            system.SetPhoneState("123", PhoneDirectory.PhoneState.TALKING_2WAY);
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOffhook("123");
            var output = sw.ToString();
            Assert.IsTrue(output.Contains("already offhook"), $"Actual output: {output}");
        }

        [TestMethod]
        public void HandleOffhook_Success()
        {
            var entry = new PhoneDirectory.PhoneEntry { PhoneNumber = "123", Name = "Alice" };
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry> { entry });
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOffhook("123");
            Assert.IsTrue(sw.ToString().Contains("is now offhook"));
        }

        [TestMethod]
        public void HandleOnhook_DenialIfEntryNotFound()
        {
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry>());
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOnhook("notfound");
            Assert.IsTrue(sw.ToString().Contains("denial"));
        }

        [TestMethod]
        public void HandleOnhook_AlreadyOnhook()
        {
            var entry = new PhoneDirectory.PhoneEntry { PhoneNumber = "123", Name = "Alice" };
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry> { entry });
            system.SetPhoneState("123", PhoneDirectory.PhoneState.ONHOOK);
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOnhook("123");
            var output = sw.ToString();
            if (!output.Contains("already onhook"))
            {
                Assert.Fail($"Expected output to contain 'already onhook' but got: {output}");
            }
        }

        [TestMethod]
        public void HandleOnhook_LeaveCall()
        {
            var entry = new PhoneDirectory.PhoneEntry { PhoneNumber = "123", Name = "Alice" };
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry> { entry });
            system.StartCall("123", "123");
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOnhook("123");
            Assert.IsTrue(sw.ToString().Contains("hung up"));
        }

        [TestMethod]
        public void HandleOnhook_Success()
        {
            var entry = new PhoneDirectory.PhoneEntry { PhoneNumber = "123", Name = "Alice" };
            var system = new LoggingPhoneSystem(new List<PhoneDirectory.PhoneEntry> { entry });
            system.SetPhoneState("123", PhoneDirectory.PhoneState.OFFHOOK_DIALTONE);
            var processor = new PhoneDirectory.CommandProcessor(system);
            using var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);
            processor.HandleOnhook("123");
            Assert.IsTrue(sw.ToString().Contains("is now onhook"));
        }
        // ...existing code...
    }
}
