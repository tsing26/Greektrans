using GreekTrans;
using System.Text;

namespace UnitTestTrans
{
    [TestClass]
    public class UnitTest1
    {
        #region word

        [TestMethod]
        public void Test_word_01()
        {
            string source = "Ηλιακοπουλος";
            string target = "Ēliakopoulos";
            Assert.AreEqual(true, GreekTransliter.IsAllGreekChar(source));

            var result = GreekTransliter.TransliterWord(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        public static void Compare(string s1, string s2)
        {
            if (s1.Length != s2.Length)
                throw new Exception($"字符串 \r\n'{s1}' 和 \r\n'{s2}' 长度不同。({s1.Length} 和 {s2.Length}) \r\n'{GetCode(s1)}' \r\n'{GetCode(s2)}'");
            for (int i = 0; i < s1.Length; i++)
            {
                char c1 = s1[i];
                char c2 = s2[i];
                if (c1 != c2)
                    throw new Exception($"偏移 {i} 处的字符 '{c1}' 和 '{c2}' 不同(内码为 {GetHex(c1)} 和 {GetHex(c2)})");
            }
        }

        static string GetHex(char ch)
        {
            return "0x" + Convert.ToString((int)ch, 16);
        }

        static string GetCode(string s)
        {
            StringBuilder text = new StringBuilder();
            foreach (char c in s)
            {
                text.Append(c.ToString() + "(" + GetHex(c) + ")");
            }
            return text.ToString();
        }

        [TestMethod]
        public void Test_word_02()
        {
            string source = "Βασιλης";
            string target = "Vasilēs";
            Assert.AreEqual(true, GreekTransliter.IsAllGreekChar(source));
            var result = GreekTransliter.TransliterWord(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 从 Test_string_11() 抽取出来
        [TestMethod]
        public void Test_word_03()
        {
            string source = "Μουσειο";
            string target = "Mouseion";
            Assert.AreEqual(true, GreekTransliter.IsAllGreekChar(source));
            var result = GreekTransliter.TransliterWord(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }


        #endregion

        #region string

        [TestMethod]
        public void Test_string_01()
        {
            string source = "Ηλιακοπουλος, Βασιλης";    // 0392 (Β)
            string target = "Ēliakopoulos, Vasilēs";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_02()
        {
            string source = "Ηλιαδη, Αμαλια Κ., 1967-";
            string target = "Ēliadē, Amalia K., 1967-";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_03()
        {
            string source = "Η.Π.Α.";
            string target = "Ē.P.A.";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

#if REMOVED
        [TestMethod]
        public void Test_string_04()
        {
            string source = "Ηιpparchos, active 190 B.C.-127 B.C.";
            string target = "Hipparchus, active 190 B.C.-127 B.C.";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }
#endif

#if REMOVED
        [TestMethod]
        public void Test_string_05()
        {
            string source = "Ηιρραsοs"; // ο
            string target = "Hippasus";
             
            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }
#endif

        // 	
        [TestMethod]
        public void Test_string_06()
        {
            string source = "Ηλιοπουλος, Ντινος, 1915-2001";
            string target = "Ēliopoulos, Ḏinos, 1915-2001";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 	
        [TestMethod]
        public void Test_string_07()
        {
            string source = "Θ. Δ. Φ.";
            string target = "Th. D. Ph.";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 	
        [TestMethod]
        public void Test_string_08()
        {
            string source = "Θανατος (Greek deity)";
            string target = "Thanatos (Greek deity)";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 
        [TestMethod]
        public void Test_string_09()
        {
            string source = "Θανος, Γιωργος, 1984-";
            string target = "Thanos, Giōrgos, 1984-";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 	
        [TestMethod]
        public void Test_string_10()
        {
            string source = "Εθνικη Επιτροπη Βιοηθικης";
            string target = "Ethnikē Epitropē Vioēthikēs";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 
        // 
        [TestMethod]
        public void Test_string_11()
        {
            string source = "Εθνικη Πινακοθηκη, Μουσειο Αλεξανδρου Σουτζου";
            string target = "Ethnikē Pinakothēkē, Mouseion Alexandrou Soutsou";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        // 	
        // 
        [TestMethod]
        public void Test_string_12()
        {
            string source = "Εθνικη Στατιστικη Υπηρεσια της Ελλαδος";
            string target = "Ethnikē Statistikē Hypēresia tēs Hellados";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_100()
        {
            string source = "Ελβετικη";
            string target = "Helvetikē";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_101()
        {
            string source = "ελλαδος";
            string target = "hellados";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_102()
        {
            string source = "έλικών";
            string target = "helikōn";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_103()
        {
            string source = "επαναστατικά";
            string target = "epanastatika";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

        [TestMethod]
        public void Test_string_104()
        {
            string source = "Έλευσίνιος";
            string target = "Eleusinios";   // "Eleusinios";

            var result = GreekTransliter.TransliterString(source, false);
            Compare(target, result);
            Assert.AreEqual(target, result);
        }

#endregion
    }

}