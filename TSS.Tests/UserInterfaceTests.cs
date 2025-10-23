using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneDirectory;
using System.Collections.Generic;

namespace TSS.Tests
{
    [TestClass]
    public class UserInterfaceTests
    {
        private System.IO.TextWriter? _originalOut;
        private System.IO.TextReader? _originalIn;

        [TestInitialize]
        public void TestInit()
        {
            _originalOut = System.Console.Out;
            _originalIn = System.Console.In;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_originalOut != null) System.Console.SetOut(_originalOut);
            if (_originalIn != null) System.Console.SetIn(_originalIn);
        }

        [TestMethod]
        public void PrintHelp_WritesExpectedCommands()
        {
            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            UserInterface.PrintHelp();

            var output = sw.ToString();
            if (!output.Contains("Available commands:")) Assert.Fail($"Missing 'Available commands:' in output: {output}");
            if (!output.Contains("offhook <phone|name>")) Assert.Fail($"Missing 'offhook <phone|name>' in output: {output}");
            if (!output.Contains("onhook <phone|name>")) Assert.Fail($"Missing 'onhook <phone|name>' in output: {output}");
            if (!output.Contains("call <target>")) Assert.Fail($"Missing 'call <target>' in output: {output}");
            if (!output.Contains("conference <target>")) Assert.Fail($"Missing 'conference <target>' in output: {output}");
            if (!output.Contains("transfer <target>")) Assert.Fail($"Missing 'transfer <target>' in output: {output}");
            if (!output.Contains("status")) Assert.Fail($"Missing 'status' in output: {output}");
            if (!output.Contains("show calls")) Assert.Fail($"Missing 'show calls' in output: {output}");
            if (!output.Contains("help")) Assert.Fail($"Missing 'help' in output: {output}");
            if (!output.Contains("quit/exit")) Assert.Fail($"Missing 'quit/exit' in output: {output}");
        }

        [TestMethod]
        public void ShowExitMessage_DisplaysCommandCount()
        {
            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            UserInterface.ShowExitMessage(42);

            var output = sw.ToString();
            if (!output.Contains("Total commands processed: 42"))
                Assert.Fail($"Missing 'Total commands processed: 42' in output: {output}");
        }

        [TestMethod]
        public void GetUserInput_TruncatesLongInput()
        {
            var input = new System.IO.StringReader(new string('a', 60) + "\n");
            System.Console.SetIn(input);
            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            var result = UserInterface.GetUserInput();

            Assert.AreEqual(new string('a', 50), result);
            Assert.IsTrue(sw.ToString().Contains("Warning: command truncated"));
        }

        [TestMethod]
        public void GetUserInput_ReturnsNullOnWhitespace()
        {
            var input = new System.IO.StringReader("   \n");
            System.Console.SetIn(input);
            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            var result = UserInterface.GetUserInput();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PrintStatus_DisplaysPhoneStatesAndCalls()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "123", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "456", Name = "Bob" }
            };

            var system = new PhoneSystem(entries);
            system.SetPhoneState("123", PhoneState.TALKING_2WAY);
            system.SetPhoneState("456", PhoneState.TALKING_2WAY);
            system.StartCall("123", "456");

            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            UserInterface.PrintStatus(system);

            var output = sw.ToString();
            Assert.IsTrue(output.Contains("System Status:"));
            Assert.IsTrue(output.Contains("TALKING_2WAY"));
            Assert.IsTrue(output.Contains("Active Calls: 1"));
        }

        [TestMethod]
        public void ShowActiveCalls_DisplaysActiveCalls()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "123", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "456", Name = "Bob" }
            };

            var system = new PhoneSystem(entries);
            system.StartCall("123", "456");

            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            UserInterface.ShowActiveCalls(system);

            var output = sw.ToString();
            if (!output.Contains("Active Calls:")) Assert.Fail($"Missing 'Active Calls:' in output: {output}");
            if (!output.Contains("Alice")) Assert.Fail($"Missing 'Alice' in output: {output}");
            if (!output.Contains("Bob")) Assert.Fail($"Missing 'Bob' in output: {output}");
        }

        [TestMethod]
        public void ShowActiveCalls_NoCalls()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "123", Name = "Alice" }
            };

            var system = new PhoneSystem(entries);

            var sw = new System.IO.StringWriter();
            System.Console.SetOut(sw);

            UserInterface.ShowActiveCalls(system);

            var output = sw.ToString();
            if (!output.Contains("No active calls."))
                Assert.Fail($"Missing 'No active calls.' in output: {output}");
        }
    }
}
