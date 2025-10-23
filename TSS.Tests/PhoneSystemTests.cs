using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneDirectory;
using System.Collections.Generic;

namespace TSS.Test.Unit
{
    [TestClass]
    public class PhoneSystemTests
    {
        [TestMethod]
        public void WB_PS_001_InitializesPhoneStatesToOnhook()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" }
            };
            var system = new PhoneSystem(entries);
            Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("12345"));
            Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("23456"));
        }

        [TestMethod]
        public void WB_PS_101_StartCallCreatesCallAndUpdatesStates()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" }
            };
            var system = new PhoneSystem(entries);
            system.StartCall("12345", "23456");
            Assert.IsTrue(system.IsPhoneInCall("12345"));
            Assert.IsTrue(system.IsPhoneInCall("23456"));
            Assert.AreEqual(PhoneState.TALKING_2WAY, system.GetPhoneState("12345"));
            Assert.AreEqual(PhoneState.TALKING_2WAY, system.GetPhoneState("23456"));
        }

        [TestMethod]
        public void WB_PS_202_LeaveCall_2Way_RemovesCallAndUpdatesState()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" }
            };
            var system = new PhoneSystem(entries);
            system.StartCall("12345", "23456");
            var sw = new StringWriter();
            var originalOut = System.Console.Out;
            System.Console.SetOut(sw);
            try
            {
                system.LeaveCall("12345");
                Assert.IsFalse(system.IsPhoneInCall("12345"));
                Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("12345"));
                Assert.AreEqual(PhoneState.OFFHOOK_DIALTONE, system.GetPhoneState("23456"));
            }
            finally
            {
                System.Console.SetOut(originalOut);
                sw.Dispose();
            }
        }

        [TestMethod]
        public void WB_PS_203_LeaveCall_3Way_UpdatesStates()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" },
                new PhoneEntry { PhoneNumber = "34567", Name = "Carol" }
            };
            var system = new PhoneSystem(entries);
            system.StartCall("12345", "23456");
            system.TryAddToCall("12345", "34567");
            system.LeaveCall("34567");
            Assert.IsFalse(system.IsPhoneInCall("34567"));
            Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("34567"));
            Assert.AreEqual(PhoneState.TALKING_2WAY, system.GetPhoneState("12345"));
            Assert.AreEqual(PhoneState.TALKING_2WAY, system.GetPhoneState("23456"));
        }

        [TestMethod]
        public void WB_PS_107_TryAddToCall_Success()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" },
                new PhoneEntry { PhoneNumber = "34567", Name = "Carol" }
            };
            var system = new PhoneSystem(entries);
            system.StartCall("12345", "23456");
            var result = system.TryAddToCall("12345", "34567");
            Assert.IsTrue(result);
            Assert.AreEqual(PhoneState.TALKING_3WAY, system.GetPhoneState("12345"));
            Assert.AreEqual(PhoneState.TALKING_3WAY, system.GetPhoneState("23456"));
            Assert.AreEqual(PhoneState.TALKING_3WAY, system.GetPhoneState("34567"));
        }

        [TestMethod]
        public void WB_PS_109_TryAddToCall_FailsIfCallFull()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" },
                new PhoneEntry { PhoneNumber = "34567", Name = "Carol" },
                new PhoneEntry { PhoneNumber = "45678", Name = "Dave" }
            };
            var system = new PhoneSystem(entries);
            system.StartCall("12345", "23456");
            system.TryAddToCall("12345", "34567");
            var result = system.TryAddToCall("12345", "45678");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WB_PS_003_SetPhoneState_NotExists_DoesNothing()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" }
            };
            var system = new PhoneSystem(entries);
            system.SetPhoneState("99999", PhoneState.OFFHOOK_DIALTONE);
            Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("12345"));
        }

        [TestMethod]
        public void WB_PS_004_GetPhoneState_NotExists_ReturnsOnhook()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" }
            };
            var system = new PhoneSystem(entries);
            Assert.AreEqual(PhoneState.ONHOOK, system.GetPhoneState("99999"));
        }

        [TestMethod]
        public void WB_PS_006_FindEntry_ByPhone_ReturnsEntry()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" }
            };
            var system = new PhoneSystem(entries);
            var entry = system.FindEntry("12345");
            Assert.IsNotNull(entry);
            Assert.AreEqual("Alice", entry.Name);
        }

        [TestMethod]
        public void WB_PS_007_FindEntry_ByName_ReturnsEntry()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" }
            };
            var system = new PhoneSystem(entries);
            var entry = system.FindEntry("Alice");
            Assert.IsNotNull(entry);
            Assert.AreEqual("12345", entry.PhoneNumber);
        }

        [TestMethod]
        public void WB_PS_008_FindEntry_NotFound_ReturnsNull()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" }
            };
            var system = new PhoneSystem(entries);
            var entry = system.FindEntry("Bob");
            Assert.IsNull(entry);
        }

        [TestMethod]
        public void WB_PS_106_GetCallForPhone_NotFound_ReturnsNull()
        {
            var entries = new List<PhoneEntry>
            {
                new PhoneEntry { PhoneNumber = "12345", Name = "Alice" },
                new PhoneEntry { PhoneNumber = "23456", Name = "Bob" }
            };
            var system = new PhoneSystem(entries);
            var call = system.GetCallForPhone("99999");
            Assert.IsNull(call);
        }
    }
}
