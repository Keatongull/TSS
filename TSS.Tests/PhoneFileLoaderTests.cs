using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneDirectory;
using System.Collections.Generic;
using System.IO;

namespace TSS.Test.Unit
{
    [TestClass]
    public class PhoneFileLoaderTests
    {
        [TestMethod]
        public void WB_FL_001_FileNotFound_ReturnsNull()
        {
            var loader = new PhoneFileLoader();
            var result = loader.LoadPhoneEntries("nonexistent.txt", 20);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void WB_FL_005_SingleField_ProducesError()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            // Should skip invalid line, so result should be null or empty
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_011_ValidEntry_AddedToList()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("12345", result[0].PhoneNumber);
            Assert.AreEqual("Alice", result[0].Name);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_101_EmptyFile_ReturnsNull()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsNull(result);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_102_CommentLine_Ignored()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "# This is a comment\n12345 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_103_InvalidPhoneNumber_TooShort()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "1234 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_104_InvalidPhoneNumber_TooLong()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "123456 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_108_SingleField_Only_Warning()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_109_DuplicatePhoneNumber_WarningAndFirstUsed()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n12345 Bob\n");
            var sw = new StringWriter();
            var originalOut = System.Console.Out;
            System.Console.SetOut(sw);
            try
            {
                var result = loader.LoadPhoneEntries(tempFile, 20);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Alice", result[0].Name);
            }
            finally
            {
                System.Console.SetOut(originalOut);
                sw.Dispose();
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void WB_FL_110_DuplicateName_WarningAndFirstUsed()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n23456 ALICE\n");
            var sw = new StringWriter();
            var originalOut = System.Console.Out;
            System.Console.SetOut(sw);
            try
            {
                var result = loader.LoadPhoneEntries(tempFile, 20);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Alice", result[0].Name);
            }
            finally
            {
                System.Console.SetOut(originalOut);
                sw.Dispose();
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void WB_FL_106_NameWithNumbers_Invalid()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice123\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_105_NonNumericPhone_Invalid()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12A45 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_111_ExceedMaxEntries_OnlyFirst20Loaded()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            var lines = new List<string>();
            string[] validNames = { "Alice", "Bob", "Carol", "Dave", "Eve", "Frank", "Grace", "Heidi", "Ivan", "Judy", "Mallory", "Niaj", "Olivia", "Peggy", "Quentin", "Rupert", "Sybil", "Trent", "Uma", "Victor", "Wendy", "Xander", "Yvonne", "Zack", "Oscar" };
            for (int i = 0; i < 25; i++)
                lines.Add($"{10000 + i} {validNames[i % validNames.Length]}");
            File.WriteAllLines(tempFile, lines);
            var sw = new StringWriter();
            var originalOut = System.Console.Out;
            System.Console.SetOut(sw);
            try
            {
                var result = loader.LoadPhoneEntries(tempFile, 20);
                Assert.IsNotNull(result);
                Assert.AreEqual(20, result.Count);
            }
            finally
            {
                System.Console.SetOut(originalOut);
                sw.Dispose();
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void WB_FL_101_PhoneLength_Zero_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_102_PhoneLength_One_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "1" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_103_PhoneLength_Four_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "1234" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_104_PhoneLength_Five_True()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "12345" });
            Assert.IsTrue(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_105_PhoneLength_Six_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "123456" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_106_NameLength_Zero_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_107_NameLength_One_True()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "A" });
            Assert.IsTrue(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_108_NameLength_Twelve_True()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "AliceAndBobs" });
            Assert.IsTrue(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_109_NameLength_Thirteen_False()
        {
            var loader = new PhoneFileLoader();
            var method = typeof(PhoneFileLoader).GetMethod("IsValidName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(method);
            var result = method.Invoke(loader, new object[] { "AliceAndBobsy" });
            Assert.IsFalse(result is bool b && b);
        }

        [TestMethod]
        public void WB_FL_201_UsedPhoneNumbers_Populated()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n");
            loader.LoadPhoneEntries(tempFile, 20);
            var field = typeof(PhoneFileLoader).GetField("usedPhoneNumbers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field);
            var set = field.GetValue(loader) as HashSet<string>;
            Assert.IsNotNull(set);
            Assert.IsTrue(set.Contains("12345"));
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_202_UsedNames_CaseInsensitive()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n23456 ALICE\n");
            loader.LoadPhoneEntries(tempFile, 20);
            var field = typeof(PhoneFileLoader).GetField("usedNames", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field);
            var set = field.GetValue(loader) as HashSet<string>;
            Assert.IsNotNull(set);
            Assert.IsTrue(set.Contains("alice"));
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_203_HashSet_PreventsDuplicates()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n12345 Bob\n");
            loader.LoadPhoneEntries(tempFile, 20);
            var field = typeof(PhoneFileLoader).GetField("usedPhoneNumbers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field);
            var set = field.GetValue(loader) as HashSet<string>;
            Assert.IsNotNull(set);
            Assert.AreEqual(1, set.Count);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_204_ListOrder_Preserved()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice\n23456 Bob\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsNotNull(result);
            Assert.AreEqual("12345", result[0].PhoneNumber);
            Assert.AreEqual("23456", result[1].PhoneNumber);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_205_EmptyOrCommentLine_Skipped()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "\n#comment\n12345 Alice\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            File.Delete(tempFile);
        }

        [TestMethod]
        public void WB_FL_206_InvalidNameCharacters()
        {
            var loader = new PhoneFileLoader();
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "12345 Alice123\n");
            var result = loader.LoadPhoneEntries(tempFile, 20);
            Assert.IsTrue(result == null || result.Count == 0);
            File.Delete(tempFile);
        }
    }
}
