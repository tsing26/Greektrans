using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

using GreekTrans;

namespace NewTest
{
    public class NewlyTest
    {
        [Theory]
        [InlineData(1, "ἅβα*→haba")]
        [InlineData(2, "ἁβρός*→habros")]
        [InlineData(3, "ἌΓΑΜΑΙ*→AGAMAI")]
        [InlineData(4, "ἁγῑνέω*→hagineō")]
        [InlineData(5, "ἁγώ*→hagō")]
        [InlineData(6, "ᾅδας*→hadas")]//改了希腊字母α
        [InlineData(7, "ἃδοι*→hadoi")]
        [InlineData(8, "ἃζομαι*→hazomai")]
        [InlineData(9, "ἃζω*→hazō")]
        [InlineData(10, "ἈΘΎΡΩ*→ATHYRŌ")]
        [InlineData(11, "Ἄθῳος*→Athōos")]
        [InlineData(12, "αἱμακτός*→haimaktos")]
        [InlineData(13, "Ἄγγλος→Anglos")]
        [InlineData(14, "ἅγια→hagia")]
        [InlineData(15, "ἁγιογδύτης→hagiogdytēs")]
        [InlineData(16, "ἃγιος→hagios")]
        [InlineData(17, "ἅη→haē")]
        [InlineData(18, "ἀϊβασιλιάτικος→aivasiliatikos")]
        [InlineData(19, "αἰγίς→aigis")]
        [InlineData(20, "αἱματώνω→haimatōnō")]
        [InlineData(21, "αίρετός→airetos")]
        [InlineData(22, "ἀϊτός→aitos")]
        [InlineData(23, "ἁλιεία→halieia")]
        [InlineData(24, "ἁμαξωτός→hamaxōtos")]
        // 共 24 个
        public void test_issue_13_alpha(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "βᾰβαί*→babai")]
        [InlineData(2, "ΒΆΖΩ*→BAZŌ")]
        [InlineData(3, "βάϊν*→bain")]
        [InlineData(4, "βαλλαντιοτομέω*→ballantiotomeō")]
        [InlineData(5, "ΒᾸΡΎΣ*→BARYS")]
        [InlineData(6, "βᾰσᾰνιστέος*→basanisteos")]
        [InlineData(7, "ΒΑΥΚΌΣ*→BAUKOS")]
        [InlineData(8, "ΒΔΈΩ*→BDEŌ")]
        [InlineData(9, "ΒΈΜΒΙΞ*→BEMBIX")]
        [InlineData(10, "βησσήεις*→bēssēeis")]
        [InlineData(11, "βιάζω*→biazō")]
        [InlineData(12, "βίβλος*→biblos")]
        [InlineData(13, "ΒΊΟΣ*→BIOS")]
        [InlineData(14, "βιόω*→bioō")]
        [InlineData(15, "βαγένι→vageni")]
        [InlineData(16, "βάγια→vagia")]
        [InlineData(17, "βαγόνι→vagoni")]
        [InlineData(18, "βάδισμα→vadisma")]
        [InlineData(19, "βαθαίνω→vathainō")]
        [InlineData(20, "βαθμηδόν→vathmēdon")]
        [InlineData(21, "βαθμιαῖς→vathmiais")]
        [InlineData(22, "βαθμολογία→vathmologia")]
        [InlineData(23, "βαθμός→vathmos")]
        [InlineData(24, "βαθμοῦχος→vathmouchos")]
        [InlineData(25, "βαθυ→vathy")]//υ→y
        // 共 25 个
        public void test_issue_13_beta(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "Γάδειρα*→Gadeira")]
        [InlineData(2, "γάζα*→gaza")]
        [InlineData(3, "γάϊος*→gaios")]
        [InlineData(4, "ΓΑΊΩ*→GAIŌ")]
        [InlineData(5, "γᾰλᾰκτινος*→galaktinos")]//改了希腊字母ο
        [InlineData(6, "ΓΈΡΩΝ*→GERŌN")]
        [InlineData(7, "γεφῡρίζω*→gephyrizō")]
        [InlineData(8, "γύννις*→gynnis")]
        [InlineData(9, "γυμνασιαρχέω*→gymnasiarcheō")]//υ→y
        [InlineData(10, "ΓΌΟΣ*→GOOS")]
        [InlineData(11, "γάγγλιον→ganglion")]
        [InlineData(12, "γαζί→gazi")]
        [InlineData(13, "γαῖα→gaia")]
        [InlineData(14, "γαιάνθραξ→gaianthrax")]
        [InlineData(15, "γαϊδουράγκαθο→gaidourankatho")]
        [InlineData(16, "γαλῆ→galē")]
        [InlineData(17, "γαλιάνδρα→galiandra")]
        [InlineData(18, "Γάλλος→Gallos")]//Γ→G
        [InlineData(19, "γαλουχῶ→galouchō")]
        [InlineData(20, "γάμπαγαργαλ ιστικό→gampagargal istiko")]//①无法处理单独的变音符号，删除了标点符号΄。待补充标点符号规则。②转写词之间未空格。
        [InlineData(21, "γειά→geia")]
        // 共 21 个
        public void test_issue_13_gamma(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "δᾳδίον*→dadion")]
        [InlineData(2, "ΔᾹΉΡ*→DAĒR")]
        [InlineData(3, "δᾰνειστής*→daneistēs")]
        [InlineData(4, "ΔΆΝΟΣ*→DANOS")]
        [InlineData(5, "Δᾱπεικὸς*→Dapeikos")]
        [InlineData(6, "δαφνηφορέω*→daphnēphoreō")]
        [InlineData(7, "ΔΈ*→DE")]
        [InlineData(8, "δεῖγμα*→deigma")]
        [InlineData(9, "ΔΈΚᾸ*→DEKA")]
        [InlineData(10, "δεκαταῖος*→dekataios")]
        [InlineData(11, "δεύτερος*→deuteros")]
        [InlineData(12, "δήϊος*→dēios")]
        [InlineData(13, "δάγγειος→dangeios")]
        [InlineData(14, "δαίμονας→daimonas")]
        [InlineData(15, "γαιμόνιος→gaimonios")]
        [InlineData(16, "δακρυσμένος→dakrysmenos")]
        [InlineData(17, "δαλτωνισμός→daltōnismos")]
        [InlineData(18, "δανεικός→daneikos")]
        [InlineData(19, "δανειον→daneion")]
        [InlineData(20, "δασκαλεύω→daskaleuō")]
        [InlineData(21, "δασμολόθιον→dasmolothion")]
        [InlineData(22, "δάσος→dasos")]
        [InlineData(23, "δασόφυτος→dasophytos")]
        // 共 23 个
        public void test_issue_13_delta(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ἕᾱγα*→heaga")]
        [InlineData(2, "ἔαξα*→eaxa")]//ξ→x
        [InlineData(3, "ἘΑὨ*→EAŌ")]
        [InlineData(4, "ἐγγεγραμμένος*→engegrammenos")]
        [InlineData(5, "έγκωμιάζω*→enkōmiazō")]
        [InlineData(6, "ἔγχελυς*→enchelys")]//υ→y
        [InlineData(7, "ἑδράζω*→hedrazō")]
        [InlineData(8, "ἘΘΈΛΩ*→ETHELŌ")]
        [InlineData(9, "εἱᾰμενή*→heiamenē")]
        [InlineData(10, "ἑκασταχόθι*→hekastachothi")]
        [InlineData(11, "ἐλεφαίρομαι*→elephairomai")]
        [InlineData(12, "ἐάν→ean")]
        [InlineData(13, "ἑαθτός→heathtos")]
        [InlineData(14, "ἔβγα→evga")]
        [InlineData(15, "ἑβδομή→hevdomē")]
        [InlineData(16, "ἑβραϊκός→hevraikos")]
        [InlineData(17, "ἐγγαστρίμυθος→engastrimythos")]
        [InlineData(18, "Έγγλέζος→Englezos")]
        [InlineData(19, "ἔγκλειστος→enkleistos")]
        [InlineData(20, "έγχειρεσις→encheiresis")]
        [InlineData(21, "ἕδρα→hedra")]
        [InlineData(22, "ἑδώδιμος→hedōdimos")]//δ→d
        // 共 22 个
        public void test_issue_14_epsilon(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ζευγίτης*→zeugitēs")]
        [InlineData(2, "ζητήσιμος*→zētēsimos")]
        [InlineData(3, "ΖΩΜΌΣ*→ZŌMOS")]
        [InlineData(4, "ζωμεύω*→zōmeuō")]
        [InlineData(5, "ΖΩΜΌΣ*→ZŌMOS")]
        [InlineData(6, "ζορκάς*→zorkas")]
        [InlineData(7, "ζόω*→zoō")]
        [InlineData(8, "ΖῨΓΌΝ*→ZYGON")]
        [InlineData(9, "ζῷον*→zōon")]
        [InlineData(10, "ζῠγοστᾰτέω*→zygostateō")]//ῠ→y
        [InlineData(11, "ζαβάδα→zavada")]
        [InlineData(12, "ζαβλακώνω→zavlakōnō")]
        [InlineData(13, "ζαλίζω→zalizō")]
        [InlineData(14, "ζάπλουτος→zaploutos")]
        [InlineData(15, "ζαχαριέρα→zachariera")]
        [InlineData(16, "ζαχαροπλαστεῖον→zacharoplasteion")]
        [InlineData(17, "ζεϊμπέκικος→zeimpekikos")]
        [InlineData(18, "ζεωίθ→zeōith")]
        [InlineData(19, "ζέροεϋ→zeroeu")]
        [InlineData(20, "ζήλ→zēl")]
        [InlineData(21, "ζωώδης→zōōdēs")]
        // 共 21 个
        public void test_issue_14_zeta(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ἤ*→ē")]
        [InlineData(2, "ἧ*→hē")]
        [InlineData(3, "ἦ*→ē")]
        [InlineData(4, "ἠβαιός*→ēbaios")]
        [InlineData(5, "ἡβάσκω*→hēbaskō")]
        [InlineData(6, "ἭΒΗ*→HĒBĒ")]
        [InlineData(7, "ἡβητής*→hēbētēs")]
        [InlineData(8, "ἡγεμοωεύω*→hēgemoōeuō")]
        [InlineData(9, "ἤγγειλα*→ēngeila")]
        [InlineData(10, "ἤγγῐκα*→ēngika")]
        [InlineData(11, "ἡγεμόσυνα*→hēgemosyna")]
        [InlineData(12, "ἡγέομαι*→hēgeomai")]
        [InlineData(13, "ἡγέτης*→hēgetēs")]
        [InlineData(14, "ἥ→hē")]
        [InlineData(15, "ἢ→ē")]
        [InlineData(16, "ἡγεμ→hēgem")]
        [InlineData(17, "ἡγοῆμαι→hēgoēmai")]
        [InlineData(18, "ἡδη→hēdē")]
        [InlineData(19, "ἠθικολογῶ→ēthikologō")]
        [InlineData(20, "ἦθος→ēthos")]
        [InlineData(21, "ἥκιστα→hēkista")]
        [InlineData(22, "ἠλεκτροκίνητος→ēlektrokinētos")]
        [InlineData(23, "ἠλεκτροπληξία→ēlektroplēxia")]
        [InlineData(24, "ἡλιακός→hēliakos")]
        [InlineData(25, "ἡλικία→hēlikia")]
        [InlineData(26, "ἧλος→hēlos")]
        // 共 26 个
        public void test_issue_14_eta(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "θάβω*→thabō")]
        [InlineData(2, "θαλαμηπόλος*→thalamēpolos")]
        [InlineData(3, "θαλασσόβιος*→thalassobios")]
        [InlineData(4, "θαλασσοδάωειον*→thalassodaōeion")]
        [InlineData(5, "θαλερός*→thaleros")]
        [InlineData(6, "θαμπός*→thampos")]
        [InlineData(7, "θαμών*→thamōn")]
        [InlineData(8, "θανή*→thanē")]
        [InlineData(9, "θαρρῶ*→tharrō")]
        [InlineData(10, "θαῦμα*→thauma")]
        [InlineData(11, "θάβω→thavō")]
        [InlineData(12, "θαλαμηπόλος→thalamēpolos")]
        [InlineData(13, "θαλασοδέρνω→thalasodernō")]
        [InlineData(14, "θαλασσοπνίγομαι→thalassopnigomai")]
        [InlineData(15, "θαλερός→thaleros")]
        [InlineData(16, "θανατηφόρος→thanatēphoros")]
        [InlineData(17, "θανή→thanē")]
        [InlineData(18, "θαρρῶ→tharrō")]
        [InlineData(19, "θαυματουργός→thaumatourgos")]
        [InlineData(20, "θεία→theia")]
        [InlineData(21, "θειικός→theiikos")]
        // 共 21 个
        public void test_issue_14_theta(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ἰαίνω*→iainō")]
        [InlineData(2, "ἴαμβος*→iambos")]
        [InlineData(3, "Ιανουάριος*→Ianouarios")]//删除了单独的变音符号
        [InlineData(4, "ἰατρεύω*→iatreuō")]
        [InlineData(5, "ἰατροδικαστής*→iatrodikastēs")]
        [InlineData(6, "ἰαχή*→iachē")]
        [InlineData(7, "ἰδέα*→idea")]
        [InlineData(8, "ἰδιαιτέρως*→idiaiterōs")]//经核实，需删除issue8中相关规则，即删除Ι⑤：ΙΔΡ→HID、Ιδ→Hid；ι⑤：ιδ→hid。
        [InlineData(9, "Ιδιοποιοῦμαι*→Idiopoioumai")]//经核实，需删除issue8中相关规则，即删除Ι⑤：ΙΔΡ→HID、Ιδ→Hid；ι⑤：ιδ→hid。
        [InlineData(10, "Ιδίωμα*→Idiōma")]//转写无误；经核实，需删除issue8中相关规则，即删除Ι⑤：ΙΔΡ→HID、Ιδ→Hid；ι⑤：ιδ→hid。
        [InlineData(11, "ἰαματικός→iamatikos")]
        [InlineData(12, "λανουάριος→lanouarios")]//删除了单独的变音符号
        [InlineData(13, "λάπων→lapōn")]//删除了单独的变音符号
        [InlineData(14, "ἰατρεῖον→iatreion")]
        [InlineData(15, "ἰατροδικαστής→iatrodikastēs")]
        [InlineData(16, "ἰαχή→iachē")]
        [InlineData(17, "ἴδε→ide")]
        [InlineData(18, "ἰδιαιτέρως→idiaiterōs")]
        [InlineData(19, "ἱέραξ→hierax")]
        [InlineData(20, "ἱεραπόστολος→hierapostolos")]
        // 共 20 个
        public void test_issue_15_iota(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "καβαλιέρος*→kabalieros")]
        [InlineData(2, "καβάλλα*→kaballa")]
        [InlineData(3, "καβαλλικεύω*→kaballikeuō")]
        [InlineData(4, "καγκελόπορτα*→kankeloporta")]//ε→e
        [InlineData(5, "καγχάζω*→kanchazō")]
        [InlineData(6, "καήλα*→kaēla")]
        [InlineData(7, "καημός*→kaēmos")]
        [InlineData(8, "καθαίρεσις*→kathairesis")]
        [InlineData(9, "καθαρεύουσα*→kathareuousa")]
        [InlineData(10, "καθαρίζω*→katharizō")]
        [InlineData(11, "καβαλλικεύω→kavallikeuō")]
        [InlineData(12, "κάγκελο→kankelo")]
        [InlineData(13, "καημένος→kaēmenos")]
        [InlineData(14, "καθαίπεσις→kathaipesis")]
        [InlineData(15, "καθαρίζω→katharizō")]
        [InlineData(16, "καθούμενος→kathoumenos")]
        [InlineData(17, "καίριος→kairios")]
        [InlineData(18, "κακομελετῶ→kakomeletō")]
        [InlineData(19, "κακοφτιαγμένος→kakophtiagmenos")]
        [InlineData(20, "καλίϊ→kalii")]
        // 共 20 个
        public void test_issue_15_kappa(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "λάβαρον*→labaron")]
        [InlineData(2, "λαβεῖν*→labein")]
        [InlineData(3, "λαβή*→labē")]
        [InlineData(4, "λαβομάνο*→labomano")]
        [InlineData(5, "λάβρα*→labra")]
        [InlineData(6, "λαγοκοιμοῦμαι*→lagokoimoumai")]
        [InlineData(7, "λαγωνικό*→lagōniko")]
        [InlineData(8, "λάδι*→ladi")]
        [InlineData(9, "λαδομπογιά*→ladompogia")]
        [InlineData(10, "λάθ*→lath")]
        [InlineData(11, "λαβύρινθος→lavyrinthos")]
        [InlineData(12, "λαγκεμένος→lankemenos")]
        [InlineData(13, "λαγούνες→lagounes")]
        [InlineData(14, "λακκούβα→lakkouva")]
        [InlineData(15, "λακκάκι→lakkaki")]
        [InlineData(16, "λαλούμενα→laloumena")]
        [InlineData(17, "λαλῶ→lalō")]
        [InlineData(18, "λαντζιέρης→lantzierēs")]
        [InlineData(19, "λαρυγγισμός→laryngismos")]
        [InlineData(20, "λάσπη→laspē")]
        // 共 20 个
        public void test_issue_15_lambda(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "μαβής*→mabēs")]
        [InlineData(2, "μαγγανεία*→manganeia")]
        [InlineData(3, "μαγγώνω*→mangōnō")]
        [InlineData(4, "μαγειρίτσα*→mageiritsa")]
        [InlineData(5, "μαγερειό*→magereio")]
        [InlineData(6, "μαγευτικός*→mageutikos")]
        [InlineData(7, "μαγνητόφωνον*→magnētophōnon")]
        [InlineData(8, "μαέστρος*→maestros")]
        [InlineData(9, "μαζί*→mazi")]
        [InlineData(10, "μαθέ*→mathe")]
        [InlineData(11, "μαῖα*→maia")]
        [InlineData(12, "μαίανδρος*→maiandros")]
        [InlineData(13, "μαγγανοπήγαδο→manganopēgado")]
        [InlineData(14, "μαγεία→mageia")]
        [InlineData(15, "μαγειπεῖον→mageipeion")]
        [InlineData(16, "μαγιό→magio")]
        [InlineData(17, "μαγκούφης→mankouphēs")]
        [InlineData(18, "μαθαίνω→mathainō")]
        [InlineData(19, "μαίανδρος→maiandros")]
        [InlineData(20, "μαϊμοῦ→maimou")]
        [InlineData(21, "Μάϊος→Maios")]
        [InlineData(22, "μακρ→makr")]
        [InlineData(23, "μακρινός→makrinos")]
        [InlineData(24, "μπάτης→batēs")]
        // 共 24 个
        public void test_issue_15_mu(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ναϊάς*→naias")]
        [InlineData(2, "ναργιλές*→nargiles")]
        [InlineData(3, "νάρθηξ*→narthēx")]
        [InlineData(4, "ναρκλιεθτικόν*→narkliethtikon")]
        [InlineData(5, "ναρκοθέτις*→narkothetis")]
        [InlineData(6, "ναῦληρος*→naulēros")]
        [InlineData(7, "ναθλώνω*→nathlōnō")]
        [InlineData(8, "ναθμαχία*→nathmachia")]//χ→ch
        [InlineData(9, "ναυσιπλοΐα*→nausiploia")]
        [InlineData(10, "ναυτικόν*→nautikon")]
        [InlineData(11, "νάζι→nazi")]
        [InlineData(12, "νάϊλον→nailon")]
        [InlineData(13, "ναρκαλιευτικόν→narkalieutikon")]
        [InlineData(14, "ναύαρχος→nauarchos")]
        [InlineData(15, "ναυμαχία→naumachia")]
        [InlineData(16, "ναυπηγεῖον→naupēgeion")]
        [InlineData(17, "ναυτόπαις→nautopais")]
        [InlineData(18, "νεάζω→neazō")]
        [InlineData(19, "νειᾶτα→neiata")]
        [InlineData(20, "νεκροφάνεια→nekrophaneia")]
        [InlineData(21, "νενέ→nene")]
        [InlineData(22, "νεο-→neo-")]
        [InlineData(23, "νεογέννητος→neogennētos")]
        // 共 23 个
        public void test_issue_16_nu(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ξάγναντο*→xagnanto")]
        [InlineData(2, "ξαγοράρης*→xagorarēs")]
        [InlineData(3, "ξαδέρφη*→xaderphē")]
        [InlineData(4, "ξαλαφρώνω*→xalaphrōnō")]
        [InlineData(5, "ξαναγεννιέμαι*→xanagenniemai")]
        [InlineData(6, "ξανάστροφη*→xanastrophē")]
        [InlineData(7, "ξάωοιγμα*→xaōoigma")]
        [InlineData(8, "ξαποσταίνω*→xapostainō")]
        [InlineData(9, "ξαφνικό*→xaphniko")]
        [InlineData(10, "ξαφωικός*→xaphōikos")]
        [InlineData(11, "ξάγναντο→xagnanto")]//ξ→x
        [InlineData(12, "ξαδέρφη→xaderphē")]//ξ→x
        [InlineData(13, "ξαίρω→xairō")]//ξ→x
        [InlineData(14, "ξαφνιάζω→xaphniazō")]//ξ→x
        [InlineData(15, "ξεβάφω→xevaphō")]//ξ→x
        [InlineData(16, "ξεκαρζίζομαι→xekarzizomai")]//ξ→x
        [InlineData(17, "ξελαρυγγίζομαι→xelaryngizomai")]//ξ→x
        [InlineData(18, "ξενίζω→xenizō")]//ξ→x
        [InlineData(19, "ξένος→xenos")]//ξ→x
        [InlineData(20, "ξεραΐλα→xeraila")]//ξ→x
        // 共 20 个
        public void test_issue_16_xi(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ὅ*→ho")]
        [InlineData(2, "Ὁβριός*→Hobrios")]//转写后首母未大写
        [InlineData(3, "ὀγκόλιθος*→onkolithos")]//γκ→nk
        [InlineData(4, "ὀδαλίσκη*→odaliskē")]
        [InlineData(5, "ὁδογέφυρα*→hodogephyra")]
        [InlineData(6, "ὁδοιπορικά*→hodoiporika")]
        [InlineData(7, "ὀδονταλγία*→odontalgia")]
        [InlineData(8, "ὀδοντόβουτσα*→odontoboutsa")]
        [InlineData(9, "ὁδοστρωτήρ*→hodostrōtēr")]
        [InlineData(10, "όδοντογλυφίδα*→odontoglyphida")]
        [InlineData(11, "ὀβελίσκος→oveliskos")]
        [InlineData(12, "Ὁβριός→Hovrios")]//转写后首字母未大写
        [InlineData(13, "ὄγδοος→ogdoos")]
        [InlineData(14, "ὅδε→hode")]
        [InlineData(15, "ὁδηγία→hodēgia")]
        [InlineData(16, "ὁδοκαθαριστής→hodokatharistēs")]//σ→s
        [InlineData(17, "ὁδός→hodos")]
        [InlineData(18, "ὀδυρμός→odyrmos")]
        [InlineData(19, "ὄδύσσεια→odysseia")]
        [InlineData(20, "ὄδω→odō")]
        // 共 20 个
        public void test_issue_16_omicron(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "παγάνα*→pagana")]
        [InlineData(2, "παγερός*→pageros")]
        [InlineData(3, "παγετών*→pagetōn")]
        [InlineData(4, "παγκόσμιος*→pankosmios")]
        [InlineData(5, "παγωνιέρα*→pagōniera")]
        [InlineData(6, "παιγνιόχαρτον*→paigniocharton")]
        [InlineData(7, "παίνεμα*→painema")]
        [InlineData(8, "παίξιμο*→paiximo")]
        [InlineData(9, "παλαιοπώλης*→palaiopōlēs")]
        [InlineData(10, "παλαμάκια*→palamakia")]
        [InlineData(11, "παγερός→pageros")]
        [InlineData(12, "παγίδι→pagidi")]
        [InlineData(13, "πάγκος→pankos")]
        [InlineData(14, "παγοπέδιλον→pagopedilon")]
        [InlineData(15, "παγωνιέρα→pagōniera")]
        [InlineData(16, "παθιασμένος→pathiasmenos")]
        [InlineData(17, "παιγνιόχαρτον→paigniocharton")]
        [InlineData(18, "παιδομάζωμα→paidomazōma")]
        [InlineData(19, "παλεύω→paleuō")]
        [InlineData(20, "πάλι→pali")]
        // 共 20 个
        public void test_issue_16_pi(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ῥά*→rha")]
        [InlineData(2, "ῥαββίον*→rhabbion")]
        [InlineData(3, "ῥαβδονομέω*→rhabdonomeō")]
        [InlineData(4, "ῥαβδονχέω*→rhabdoncheō")]
        [InlineData(5, "ῥαγδαῖος*→rhagdaios")]
        [InlineData(6, "ῬᾸΔΝΌΣ*→RHADNOS")]//删除多余拉丁字母I
        [InlineData(7, "ῥᾴδιος*→rhadios")]
        [InlineData(8, "ῥᾳδιούργέω*→rhadiourgeō")]
        [InlineData(9, "ῥᾳθῡμία*→rhathymia")]
        [InlineData(10, "ῬΑΊΝΩ*→RHAINŌ")]
        [InlineData(11, "ῥᾰκιο-συρραπτάδης*→rhakio-syrraptadēs")]//增加未转写的首个拉丁字母a
        [InlineData(12, "ῥαπτός*→rhaptos")]
        [InlineData(13, "ραβαΐσι→ravaisi")]
        [InlineData(14, "ράγια→ragia")]
        [InlineData(15, "ραδιογραφία→radiographia")]
        [InlineData(16, "ραιβοσκελές→raivoskeles")]
        [InlineData(17, "ραπτομηχανή→raptomēchanē")]
        [InlineData(18, "ραχοκόκκαλο→rachokokkalo")]
        [InlineData(19, "πέκλάμα→peklama")]
        [InlineData(20, "πετσινόλαδο→petsinolado")]
        [InlineData(21, "ρεύω→reuō")]
        [InlineData(22, "ρῆζις→rēzis")]
        // 共 22 个
        public void test_issue_17_rho(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "Σᾰβάζιος*→Sabazios")]
        [InlineData(2, "Σαββᾰτίζω*→Sabbatizō")]
        [InlineData(3, "σαγή*→sagē")]
        [InlineData(4, "Σαδδουκαῖοι*→Saddoukaioi")]
        [InlineData(5, "ΣΆΓΟΣ*→SAGOS")]
        [InlineData(6, "ΣΑΊΡΩ*→SAIRŌ")]
        [InlineData(7, "Σᾰλᾰμῑν-ᾰφέτης*→Salamin-aphetēs")]
        [InlineData(8, "σάλασσα*→salassa")]
        [InlineData(9, "Σάρδεις*→Sardeis")]
        [InlineData(10, "σάρκῐκός*→sarkikos")]
        [InlineData(11, "σάβανον→savanon")]
        [InlineData(12, "σαγή→sagē")]
        [InlineData(13, "σακαράκα→sakaraka")]
        [InlineData(14, "σακκάκι→sakkaki")]
        [InlineData(15, "σαλαμάνδρα→salamandra")]
        [InlineData(16, "σάλιαγκας→saliankas")]
        [InlineData(17, "σαλιάρης→saliarēs")]
        [InlineData(18, "σαλιώνω→saliōnō")]
        [InlineData(19, "σαλτιμπάγκος→saltimpankos")]
        [InlineData(20, "σαμπάνια→sampania")]
        // 共 20 个
        public void test_issue_17_sigma(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }

        [Theory]
        [InlineData(1, "ταβλιόπη*→tabliopē")]
        [InlineData(2, "τἀγαμέμνονος*→tagamemnonos")]//增加未转写的第二个拉丁字母o
        [InlineData(3, "τᾰγεύω*→tageuō")]
        [InlineData(4, "τἀγχέλεια*→tancheleia")]//γχ→nch
        [InlineData(5, "Ταίνᾰρος*→Tainaros")]
        [InlineData(6, "τακτικός*→taktikos")]
        [InlineData(7, "τᾰλαιπωρέω*→talaipōreō")]
        [InlineData(8, "τᾰλαιπωρία*→talaipōria")]
        [InlineData(9, "τάλατον*→talaton")]
        [InlineData(10, "ΤΆΛᾸΡΟΣ*→TALAROS")]
        [InlineData(11, "ταβέρνα→taverna")]
        [InlineData(12, "ταγή→tagē")]
        [InlineData(13, "ταγματάρχης→tagmatarchēs")]
        [InlineData(14, "τακτοποι→taktopoi")]
        [InlineData(15, "ταλαίπωρος→talaipōros")]
        [InlineData(16, "ταμιευτήριον→tamieutērion")]
        [InlineData(17, "ταμποθρᾶς→tampothras")]
        [InlineData(18, "ταζιδεύω→tazideuō")]
        [InlineData(19, "ταπετσαρία→tapetsaria")]
        [InlineData(20, "τασάκι→tasaki")]
        // 共 20 个
        public void test_issue_17_tau(int number, string source, string? target = null, bool acient = false)
        {
            test_string(number, source, target, acient);
        }



        #region 内部实现

        // https://xunit.net/docs/capturing-output
        private readonly ITestOutputHelper output;

        public NewlyTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        // parameters:
        //      source  可以为单纯的希腊文字符串。如果最后一个字符为 '*'，表示这是古代风格
        //              还可以为 xxxx→yyyy 形态，表示 → 以后其实是 target 内容
        //              还可以为 xxxx*→yyyy 形态，星号表示 xxxx 部分是古代风格
        void test_string(
            int number,
            string source,
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

            StringBuilder debugInfo = new StringBuilder();
            var result = GreekTransliter.TransliterString(source, ancient, debugInfo);
            output.WriteLine($"{source} --> {target} ({(target == result ? "OK" : "expect:" + result)})");
            output.WriteLine(debugInfo.ToString());
            StableTest.Compare(target, result);
            Assert.Equal(target, result);
        }

        #endregion
    }
}
