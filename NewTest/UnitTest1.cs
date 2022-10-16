using GreekTrans;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace NewTest
{
    public class UnitTest1
    {
        // https://xunit.net/docs/capturing-output
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        // 测试基本字母规则
        [Theory]
        // 规则里面带的测试案例
        [InlineData("Α→A")]
        [InlineData("Ἁ→HA")]
        [InlineData("Ἃ→HA")]
        [InlineData("Ἇ→HA")]
        [InlineData("ᾉ→HA")]
        [InlineData("ᾋ→HA")]
        [InlineData("ᾍ→HA")] // 1f8d
        [InlineData("ᾏ→HA")]
        [InlineData("Ἀ→A")]
        [InlineData("Ἂ→A")]
        [InlineData("ᾈ→A")]
        [InlineData("ᾌ→A")]

        // ΑἹ→HAI、ΑἽ→HAI、ΑἻ→HAI、ΑἿ→HAI、ΑὙ→HAU、ΑὝ→HAU、ΑὛ→HAU、ΑὟ→HAU
        [InlineData("ΑἹ", "HAI")]
        [InlineData("ΑἽ", "HAI")]
        [InlineData("ΑἻ", "HAI")]
        [InlineData("ΑἿ", "HAI")]
        [InlineData("ΑὙ", "HAU")]
        [InlineData("ΑὝ", "HAU")]
        [InlineData("ΑὛ", "HAU")]
        [InlineData("ΑὟ", "HAU")]

        /*
        α→a、
        ἁ→ha、
        ἃ →ha、
        ἅ→ha、
        ἇ→ha、
        ᾁ→ha、
        ᾃ→ha、
        ᾅ→ha、
        ᾇ→ha、
        ἀ→a、
        ἆ→a、
        ᾄ→a、
        αἱ→hai、
        αἵ→hai、
        αἳ→hai、
        αἷ→hai、
        αὑ→hau、
        αὕ→hau、
        αὓ→hau、
        αὗ→hau。
         * */
        [InlineData("α", "a")]
        [InlineData("ἁ", "ha")]
        [InlineData("ἃ", "ha")]
        [InlineData("ἅ", "ha")]
        [InlineData("ἇ", "ha")]
        [InlineData("ᾁ", "ha")]
        [InlineData("ᾃ", "ha")]
        [InlineData("ᾅ", "ha")]
        [InlineData("ᾇ", "ha")]
        [InlineData("ἀ", "a")]
        [InlineData("ἆ", "a")]
        [InlineData("ᾄ", "a")]
        [InlineData("αἱ", "hai")]
        [InlineData("αἵ", "hai")]
        [InlineData("αἳ", "hai")]
        [InlineData("αἷ", "hai")]
        [InlineData("αὑ", "hau")]
        [InlineData("αὕ", "hau")]
        [InlineData("αὓ", "hau")]
        [InlineData("αὗ", "hau")]
        public void test_alpha(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Β", "B", true)]
        [InlineData("Β", "V", false)]
        [InlineData("β", "b", true)]
        [InlineData("β", "v", false)]
        public void test_beta(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Γ", "G")]
        [InlineData("ΓΓ", "NG")]
        [InlineData("γ", "g")]
        [InlineData("γγ", "ng")]
        [InlineData("Γκ", "Gk")]
        [InlineData("Γκαρσόν", "Gkarson")]
        [InlineData("Γκαζόζα", "Gkazoza")]
        [InlineData("Γκάζι", "Gkazi")]
        [InlineData("ΓΚ", "GK")]
        [InlineData("ΓΚΑΡΣΌΝ", "GKARSON")]

        [InlineData("ΓΚΑΖΌΖΑ", "GKAZOZA")]
        [InlineData("ΓΚΆΖΙ", "GKAZI")]
        [InlineData("ΑΓΚ", "AGK")]
        [InlineData("ΡΓΚ", "RGK")]
        [InlineData("ΣΟΡΌΓΚ", "SOROGK")]
        [InlineData("ΣΥΓΚΕΝΤΡΏΣΕΩΝ", "SYNKENTRŌSEŌN")]  // 词中 反例
        [InlineData("ΠΆΓΚΑΛΟΝ", "PANKALON")]    // 反例
        [InlineData("ΖΙΟΥΓΚΆΝΟΦ", "ZIOUNKANOPH")]   // 反例
        // [InlineData("ΖΙΟΥΓΚΆΝΟΦ", "ZIOUNKANOPH")]   // 反例
        [InlineData("γκ", "gk")]
        [InlineData("γκαρσόν", "gkarson")]
        [InlineData("γκαζόζα", "gkazoza")]

        [InlineData("γκάζι", "gkazi")]

        [InlineData("αγκ", "agk")]
        [InlineData("ργκ", "rgk")]
        [InlineData("ρόγκ", "rogk")]
        [InlineData("Συγκεντρώσεων", "Synkentrōseōn")]  // 词中 反例
        [InlineData("πάγκαλον", "pankalon")]
        [InlineData("Ζιουγκάνοφ", "Ziounkanoph")]
        [InlineData("γξ", "nx")]
        [InlineData("λυγξ", "lynx")]
        [InlineData("ἄγξις", "anxis")]
        [InlineData("γξθ", "nxth")]
        [InlineData("ΓΞ", "NX")]
        [InlineData("γχ", "nch")]
        [InlineData("γχκ", "nchk")]
        [InlineData("γχν", "nchn")]
        [InlineData("γχς", "nchs")]

        [InlineData("Ελέγχου", "Helenchou")] // Elenchou 疑似错误 
        [InlineData("ΓΧ", "NCH")]
        // [InlineData("")]
        public void test_gamma(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("δᾳδίον*→dadion")]
        [InlineData("ΔᾹΉΡ*→DAĒR")]
        [InlineData("δᾰνειστής*→daneistēs")]
        [InlineData("ΔΆΝΟΣ*→DANOS")]
        [InlineData("Δᾱπεικὸς*→Dapeikos")]  // dapeikos 第一个字符应该大写
        [InlineData("δαφνηφορέω*→daphnēphoreō")]
        [InlineData("ΔΈ*→DE")]
        [InlineData("δεῖγμα*→deigma")]
        [InlineData("ΔΈΚᾸ*→KEKA")]  // KEKA 疑似错误？
        [InlineData("δεκαταῖος*→dekataios")]
        [InlineData("δεύτερος*→deuteros")]
        [InlineData("δήϊος*→dēios")]
        [InlineData("δάγγειος→dangeios")] // 
        [InlineData("δαίμονας→daimonas")]
        [InlineData("γαιμόνιος→gaimonios")]
        [InlineData("δακρυσμένος→dakrysmenos")]
        [InlineData("δαλτωνισμός→daltōnismos")]
        [InlineData("δανεικός→daneikos")]
        [InlineData("δανειον→daneion")]
        [InlineData("δασκαλεύω→daskaleuō")]
        [InlineData("δασμολόθιον→dasmolothion")]
        [InlineData("δάσος→dasos")]
        [InlineData("δασόφυτος→dasophutos")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_delta(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ε", "E")]
        [InlineData("Ἓ", "HE")]
        [InlineData("Ἕ", "HE")]
        [InlineData("Ἑ", "HE")]
        [InlineData("Ὲ", "E")]
        //[InlineData("Ἓ", "HE")]   // 重复了
        [InlineData("Έ", "E")]
        [InlineData("ΕἹ", "HEI")]
        [InlineData("ΕἽ", "HEI")]
        [InlineData("ΕἻ", "HEI")]
        [InlineData("ΕἿ", "HEI")]
        [InlineData("ΕὙ", "HEU")]
        [InlineData("ΕὝ", "HEU")]
        [InlineData("ΕὛ", "HEU")]
        [InlineData("ΕὟ", "HEU")]
        [InlineData("ε", "e")]
        [InlineData("ἑ", "he")]
        [InlineData("ἕ", "he")]
        [InlineData("ἓ", "he")]
        [InlineData("ἔ", "e")]
        [InlineData("ἐ", "e")]
        [InlineData("εἱ", "hei")]
        [InlineData("εἵ", "hei")]
        [InlineData("εἳ", "hei")]
        [InlineData("εἷ", "hei")]
        [InlineData("εὑ", "heu")]
        [InlineData("εὕ", "heu")]
        [InlineData("εὓ", "heu")]
        [InlineData("εὗ", "heu")]
        public void test_epsilon(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ζ", "Z")]
        [InlineData("ζ", "z")]
        [InlineData("ζευγίτης", "zeugitēs")]
        [InlineData("ζητήσιμος", "zētēsimos")]
        // ΖΟ΄ΦΟΣ*→ZOPHOS
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_zeta(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Θ", "TH")]
        [InlineData("θ", "th")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_theta(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ι", "I")]
        [InlineData("Ἱ", "HI")]
        [InlineData("Ἵ", "HI")]
        [InlineData("Ἳ", "HI")]
        [InlineData("Ἷ", "HI")]
        [InlineData("ΑἹ", "HAI")]
        [InlineData("ΕἹ", "HEI")]
        [InlineData("ΥἹ", "HUI")]
        [InlineData("ΟἹ", "HOI")]
        [InlineData("Ἲ", "I")]
        [InlineData("Ἰ", "I")]
        [InlineData("ι", "i")]
        [InlineData("ἱ", "hi")]
        [InlineData("ἵ", "hi")]
        [InlineData("ἳ", "hi")]
        [InlineData("ἷ", "hi")]
        [InlineData("αἱ", "hai")]
        [InlineData("εἱ", "hei")]
        [InlineData("υἱ", "hui")]
        [InlineData("οἱ", "hoi")]
        [InlineData("ἲ", "i")]
        [InlineData("ῗ", "i")]
        // [InlineData("")]
        public void test_iota(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Κ", "K")]
        [InlineData("κ", "k")]
        // [InlineData("")]
        public void test_kappa(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Λ", "L")]
        [InlineData("λ", "l")]
        // [InlineData("")]
        public void test_lambda(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Μ", "M")]
        [InlineData("μ", "m")]
        [InlineData("μπ", "b")]  //（词首）
        [InlineData("μπασκετ", "basket")]
        [InlineData("μπαμ", "bam")]
        [InlineData("μπιφτέκια", "biphtekia")]
        [InlineData("πέμπω", "pempō")]
        [InlineData("μπαρμπι", "barmpi")]
        [InlineData("αμπελι", "ampeli")]
        [InlineData("συμπ", "symp")]
        [InlineData("υμπ", "ymp")]
        [InlineData("βμπ", "bmp", true)]  //
        [InlineData("ψμπ", "psmp")]
        [InlineData("ρμπνμπ", "rmpnmp")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]

        public void test_mu(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("ν→n")]
        // [InlineData("ντ→ḏ")]
        [InlineData("Ντ→Ḏ")]
        [InlineData("Ντοτ→Ḏot")]
        [InlineData("Ντιανα→Ḏiana")]
        [InlineData("Ντοκουμεντο→Ḏokoumento")]
        [InlineData("ΝΤ→Ḏ")]
        [InlineData("ΝΤΟΤ→ḎOT")]
        [InlineData("ΝΤΑΝΑ→ḎANA")]
        [InlineData("ΝΤΟΚΟΥΜΕΝΤΟ→ḎOKOUMENTO")]
        [InlineData("αντέννα→antenna")]
        [InlineData("μεντα→menta")]
        [InlineData("πενταποσταγμα→pentapostagma")]
        [InlineData("ΛΙΝΤΛ→LINTL")]
        [InlineData("ΖΝΤΟΒΤΣ*→ZNTOBTS")]
        [InlineData("ΖΝΤΑΝΟΨ→ZNTANOPS")]
        [InlineData("ΝΤΟΝΤ→ḎONT")]
        [InlineData("ΤΝΤ→TNT")]  // 原来右侧是 ḎT，疑似错误了
        [InlineData("ΜΝΤ→MNT")]
        [InlineData("ντ→ḏ")]
        [InlineData("ντινα→ḏina")]
        [InlineData("νταλκας→ḏalkas")]
        [InlineData("ντσιαφέρης→ḏsiapherēs")]
        [InlineData("λιντλ→lintl")]
        [InlineData("ζντοβτς*→zntobts")]
        [InlineData("ζντανοφ→zntanoph")]
        [InlineData("ντοντ→ḏont")]
        [InlineData("τντ→tnt")]
        [InlineData("μντ→mnt")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]

        public void test_nu(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }


        [Theory]
        [InlineData("Ξ→X")]
        [InlineData("ξ→x")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]

        public void test_xi(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ο→O")]
        [InlineData("Ὁ→HO")]
        [InlineData("Ὅ→HO")]
        [InlineData("Ὃ→HO")]
        [InlineData("ΟἹ→HOI")]
        [InlineData("ΟἽ→HOI")]
        [InlineData("ΟἻ→HOI")]
        [InlineData("ΟἿ→HOI")]
        [InlineData("ΟὙ→HOU")]
        [InlineData("ΟὝ→HOU")]
        [InlineData("ΟὛ→HOU")]
        [InlineData("ΟὟ→HOU")]
        [InlineData("ο→o")]
        [InlineData("ὁ→ho")]
        [InlineData("ὅ→ho")]
        [InlineData("ὃ→ho")]
        [InlineData("οἱ→hoi")]
        [InlineData("οἵ→hoi")]
        [InlineData("οἳ→hoi")]
        [InlineData("οἷ→hoi")]
        [InlineData("οὑ→hou")]
        [InlineData("οὕ→hou")]
        [InlineData("οὓ→hou")]
        [InlineData("οὗ→hou")]
        [InlineData("ου→ou")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]

        public void test_omicron(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Π→P")]
        [InlineData("π→p")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_pi(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ρ→R")]
        [InlineData("Ῥ→RH")]
        [InlineData("ρ→r")]
        [InlineData("ῥ→rh")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_rho(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Σ→S")]
        [InlineData("Ϲ→S")]
        [InlineData("σ→s")]
        [InlineData("ϲ→s")]
        [InlineData("ΣΕΙΣΜΟΣ→SEISMOS")]
        [InlineData("ΣΑΡΩΣΗ→SARŌSĒ")]
        [InlineData("ΣΟΛΟΝ→SOLON")]
        [InlineData("Σουβλάκι*→Soublaki")]
        [InlineData("σουβλάκι*→soublaki")]   // 加了星号
        [InlineData("σμαρτμεδισυ→smartmedisy")]
        [InlineData("σχολεια→scholeia")]
        [InlineData("μος→mos")]
        [InlineData("ψος→psos")]    // phos 疑似错误
        [InlineData("εος→eos")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_sigma(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Τ→T")]
        [InlineData("τ→t")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_tao(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Υ→Y")]
        [InlineData("ΑΥ→AU")]
        [InlineData("ΕΥ→EU")]
        [InlineData("ΗΥ→ĒU")]
        [InlineData("ΟΥ→OU")]
        [InlineData("ΥΙ→UI")]
        [InlineData("Ὑ→HY")]
        [InlineData("Ὕ→HY")]
        [InlineData("Ὓ→HY")]
        [InlineData("Ὗ→HY")]
        [InlineData("ὙΙ→HUI")]
        [InlineData("ὝΙ→HUI")]
        [InlineData("ὛΙ→HUI")]
        [InlineData("ὟΙ→HUI")]
        [InlineData("αυ→au")]
        [InlineData("ευ→eu")]
        [InlineData("ηυ→ēu")]
        [InlineData("ου→ou")]
        [InlineData("υι→ui")]
        [InlineData("ὑ→hy")]
        [InlineData("ὕ→hy")]
        [InlineData("ὓ→hy")]
        [InlineData("ὗ→hy")]
        [InlineData("ὑι→hui")]
        [InlineData("ὕι→hui")]
        [InlineData("ὓι→hui")]
        [InlineData("ὗι→hui")]
        [InlineData("Ῥ→RH")]
        [InlineData("ῬΎΓΧΟΣ→RHYNCHOS")] // RHYGCHOS 疑似错误
        [InlineData("ῬΟΜΨΑΊΑ→RHOMPSAIA")]   // RHOMPHAIA 疑似错误
        [InlineData("ῬΗΤΟΡΙΚΗ*→RHĒTORIKĒ")]  // RHTORIKĒ 疑似错误
        [InlineData("ῥ→rh")]
        [InlineData("ῥῆμα→rhēma")]
        [InlineData("ῥάβδος*→rhabdos")]
        [InlineData("ῥητορική→rhētorikē")]
        [InlineData("ΕΥΧΑΡΙΣΤΏ→EUCHARISTŌ")]    // EYCHARISTŌ 疑似错误
        [InlineData("ΠΕΥΚΟΧΩΡΙ→PEUKOCHŌRI")]    // PEUKOCHŌPI 疑似错误
        [InlineData("ΗΥΘΝΔΑΙ→ĒUTHNDAI")]    // 
        [InlineData("ΜΟΥΣΙΚΟΣ→MOUSIKOS")]
        [InlineData("ΥΙΟΥΤΙΟΥΜΠ→UIOUTIOUMP")]
        [InlineData("ΖΩΥΦΙΑ→ZŌUPHIA")]  // TODO: 需要增补 ΩΥ 双元音转写规则
        [InlineData("μαυρικιος→maurikios")]
        [InlineData("πευκοχωρι→peukochōri")]
        [InlineData("τηυρογεν→tēurogen")]
        [InlineData("μουσικος→mousikos")]
        [InlineData("ουιπεστ→ouipest")]
        [InlineData("ζωυφια→zōuphia")]
        [InlineData("υεκα→yeka")]
        [InlineData("υοθτυβε*→yothtybe")]   // 加了 *
        [InlineData("υμαιλ→ymail")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_upsilon(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Φ→PH")]
        [InlineData("φ→ph")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_phi(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Χ→CH")]
        [InlineData("χ→ch")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_chi(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ψ→PS")]
        [InlineData("ψ→ps")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_psi(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ω→Ō")]
        [InlineData("Ὡ→HŌ")]
        [InlineData("Ὥ→HŌ")]
        [InlineData("Ὣ→HŌ")]
        [InlineData("ᾯ→HŌ")]
        [InlineData("Ὧ→HŌ")]
        [InlineData("ᾩ→HŌ")]
        [InlineData("ᾭ→HŌ")]
        [InlineData("ᾫ→HŌ")]
        [InlineData("ω→ō")]
        [InlineData("ὡ→hō")]
        [InlineData("ὥ→hō")]
        [InlineData("ὣ→hō")]
        [InlineData("ᾧ→hō")]
        [InlineData("ὧ→hō")]
        [InlineData("ᾡ→hō")]
        [InlineData("ᾥ→hō")]
        [InlineData("ᾣ→hō")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_omega(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ϝ→W")]
        [InlineData("ϝ→w")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_digamma(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ϙ→Ḳ")]
        [InlineData("ϙ→ḳ")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        // [InlineData("")]
        public void test_qoppa(string source, string? target = null, bool acient = false)
        {
            test(source, target, acient);
        }

        [Theory]
        [InlineData("Ηλιακοπουλος", "Ēliakopoulos")]

        public void test_string(string source, string target)
        {
            Assert.True(GreekTransliter.IsAllGreekChar(source));

            var result = GreekTransliter.TransliterWord(source, false);
            Compare(target, result);
            Assert.Equal(target, result);
        }

        // parameters:
        //      source  可以为单纯的希腊文字符串。如果最后一个字符为 '*'，表示这是古代风格
        //              还可以为 xxxx→yyyy 形态，表示 → 以后其实是 target 内容
        //              还可以为 xxxx*→yyyy 形态，星号表示 xxxx 部分是古代风格
        void test(string source, 
            string? target = null,
            bool ancient = false)
        {
            if (target == null)
            {
                GreekTransliter.ParseTwoPart(source,
                    "→",
                    out source,
                    out target);
            }

            if (source.EndsWith("*"))
            {
                source = source.Substring(0, source.Length - 1);
                ancient = true;
            }

            Assert.True(GreekTransliter.IsAllGreekChar(source));

            StringBuilder debugInfo = new StringBuilder();
            var result = GreekTransliter.TransliterWord(source, ancient, debugInfo);
            output.WriteLine($"{source} --> {target} ({(target == result ? "OK" : "expect:"+result)})");
            output.WriteLine(debugInfo.ToString());
            Compare(target, result);
            Assert.Equal(target, result);
        }

        static void Compare(string s1, string s2)
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

    }
}