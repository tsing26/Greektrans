using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreekTrans
{
    // 希腊文转写
    public static class GreekTransliter
    {
        public class GetTokenResult
        {
            public string? Token { get; set; }

            public string? Type { get; set; }   // word delimiter finish

            // 从字符串中获得下一个 token
            public static GetTokenResult GetToken(string text,
                ref int offset)
            {
                StringBuilder delimeters = new StringBuilder();
                StringBuilder word = new StringBuilder();
                for (int i = offset; i < text.Length; i++)
                {
                    char ch = text[i];
                    if (IsDelimiter(ch))
                    {
                        if (word.Length > 0)
                        {
                            offset = i;
                            return new GetTokenResult
                            {
                                Token = word.ToString(),
                                Type = "word"
                            };
                        }
                        delimeters.Append(ch);
                    }
                    else
                    {
                        if (delimeters.Length > 0)
                        {
                            offset = i;
                            return new GetTokenResult
                            {
                                Token = delimeters.ToString(),
                                Type = "delimiter"
                            };
                        }
                        word.Append(ch);
                    }
                }

                if (word.Length > 0)
                {
                    offset = text.Length;
                    return new GetTokenResult
                    {
                        Token = word.ToString(),
                        Type = "word"
                    };
                }

                if (delimeters.Length > 0)
                {
                    offset = text.Length;
                    return new GetTokenResult
                    {
                        Token = delimeters.ToString(),
                        Type = "delimiter"
                    };
                }

                return new GetTokenResult { Type = "finish" };
            }
            static bool IsDelimiter(char ch)
            {
                if (char.IsControl(ch))
                    return true;
                if (ch == ' ' || ch == ',' || ch == ';' || ch == '.'
                    || ch == ':' || ch == '\'' || ch == '\t'
                    || ch == '-')
                    return true;
                if (char.IsLetter(ch))
                    return false;
                return false;
            }
        }

        public static string TransliterString(string source,
            bool ancient = false,
            StringBuilder? debugInfo = null)
        {
            StringBuilder text = new StringBuilder();
            int offset = 0;
            while (true)
            {
                var result = GetTokenResult.GetToken(source,
                    ref offset);
                if (result.Type == "finish")
                    break;
                if (result.Type == "delimiter")
                {
                    text.Append(result.Token);
                    debugInfo?.Append(result.Token);
                    continue;
                }

                if (string.IsNullOrEmpty(result.Token))
                    continue;
                if (IsGreekWord(result.Token) == false)
                {
                    text.Append(result.Token);
                    debugInfo?.Append("[" + result.Token + "]");
                    continue;
                }
                if (IsAllGreekChar(result.Token, out string error) == false)
                    throw new TransliterException($"警告: 单词 '{result.Token}' 中包含希腊文以外的字符: {error}");
                var word = TransliterWord(result.Token, ancient, debugInfo);
                text.Append(word);
            }

            return text.ToString();
        }

        // 是否为希腊文单词？(只要有一个字母是希腊文字母，就算希腊文单词)
        public static bool IsGreekWord(string word)
        {
            foreach (var ch in word)
            {
                if (ch >= 0x037e && ch <= 0x03fb)
                    return true;
                if (ch >= 0x1f00 && ch <= 0x1ffe)
                    return true;
            }

            return false;
        }

        public static bool IsAllGreekChar(string word)
        {
            return IsAllGreekChar(word, out string _);
        }

        // 是否全部为希腊文字符？
        public static bool IsAllGreekChar(string word, out string error)
        {
            int i = 0;
            foreach (var ch in word)
            {
                if (!(ch >= 0x037e && ch <= 0x03fb)
                    && !(ch >= 0x1f00 && ch <= 0x1ffe)
                    )
                {
                    error = $"字符 '{ch}' (offset={i}, 0x{Convert.ToString((int)ch, 16)})";
                    return false;
                }
                i++;
            }
            error = "";
            return true;
        }


        // 转写一个单词
        // 每次先取两个字符进行搜寻，如果搜不到再缩减为一个字符进行搜寻
        public static string TransliterWord(string source,
            bool ancient = false,
            StringBuilder? debugInfo = null)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            Debug.Assert(source.Length > 0);

            try
            {

                StringBuilder text = new StringBuilder();

                int length = source.Length;
                for (int i = 0; i < length; i++)
                {
                    LookupStyle style = ancient == true ? LookupStyle.ancient : LookupStyle.modern;

                    // 先尝试三个字符
                    if (i + 3 <= length)
                    {
                        string three = source.Substring(i, 3);

                        if (i == 0)
                            style |= LookupStyle.head;

                        if (i + 3 == length)
                            style |= LookupStyle.tail;

                        if (i > 0 && i + 3 < length)
                            style |= LookupStyle.middle;

                        string? result = LookupChar(three, style, out int step);
                        if (result != null)
                        {
                            Debug.Assert(IsGreekWord(result) == false, "按理说转换以后不应该存在希腊文字符了");

                            debugInfo?.Append($"{GetStepSegment(three, step)}->{result} ");

                            text.Append(result);
                            i += step - 1;
                            continue;
                        }
                    }

                    // 先尝试两个字符
                    if (i + 2 <= length)
                    {
                        string two = source.Substring(i, 2);

                        if (i == 0)
                            style |= LookupStyle.head;

                        if (i + 2 == length)
                            style |= LookupStyle.tail;

                        if (i > 0 && i + 2 < length)
                            style |= LookupStyle.middle;

                        string? result = LookupChar(two, style, out int step);
                        if (result != null)
                        {
                            Debug.Assert(IsGreekWord(result) == false, "按理说转换以后不应该存在希腊文字符了");

                            debugInfo?.Append($"{GetStepSegment(two, step)}->{result} ");

                            text.Append(result);
                            i += step - 1;
                            continue;
                        }
                    }

                    // 然后尝试一个字符
                    {
                        string one = source.Substring(i, 1);
                        if (i == 0)
                            style |= LookupStyle.head;

                        if (i + 1 == length)
                            style |= LookupStyle.tail;

                        if (i > 0 && i + 1 < length)
                            style |= LookupStyle.middle;

                        string? result = LookupChar(one, style, out int step);
                        if (result != null)
                        {
                            debugInfo?.Append($"{GetStepSegment(one, step)}->{result} ");

                            text.Append(result);
                            i += step - 1;
                        }
                        else
                            throw new TransliterException($"字符 '{one}' (偏移 {i}) 没有找到匹配的罗马化字符");
                    }
                }

                return text.ToString();
            }
            catch (TransliterException ex)
            {
                throw new TransliterException(ex.Message, source);
            }
        }

        static string GetStepSegment(string segment, int step)
        {
            if (segment.Length == step)
                return segment;
            return $"{segment}(step={step})";
        }

        // 双元音处理
        public static string? LookupDoubleChar(string greek,
    LookupStyle style,
    out int step)
        {
            Debug.Assert(greek.Length == 2);

            step = greek.Length;

            string s1 = greek.Substring(0, 1);
            string s2 = greek.Substring(1, 1);

            // *** Alpha Iota 相关 ***
            if (upper_alpha_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合②+集合⑤→HAI；
                // upper_alpha_without_dasia + upper_iota_with_dasia
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HAI";

                // 集合②+集合⑥→AI；
                // upper_alpha_without_dasia + upper_iota_without_dasia
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "AI";

                // 集合②+集合⑦→Hai；
                // upper_alpha_without_dasia + lower_iota_with_dasia
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hai";

                // 集合②+集合⑧→Ai；
                // upper_alpha_without_dasia + lower_iota_without_dasia
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Ai";
            }

            if (lower_alpha_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合④+集合⑦→hai；
                // lower_alpha_without_dasia + lower_iota_with_dasia
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hai";

                // 集合④+集合⑧→ai。
                // lower_alpha_without_dasia + lower_iota_without_dasia
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ai";
            }

            // *** Alpha Upsilon 相关 ***
            if (upper_alpha_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_alpha_without_dasia）+ 集合（upper_upsilon_with_dasia）→HAU；
                if (upper_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "HAU";

                // 集合（upper_alpha_without_dasia）+ 集合（upper_upsilon_without_dasia）→AU；
                if (upper_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "AU";

                // 集合（upper_alpha_without_dasia）+ 集合（lower_upsilon_with_dasia）→Hau；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "Hau";

                // 集合（upper_alpha_without_dasia）+ 集合（lower_upsilon_without_dasia）→Au；
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "Au";
            }

            if (lower_alpha_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_alpha_without_dasia）+ 集合（lower_upsilon_with_dasia）→hau；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "hau";

                // 集合（lower_alpha_without_dasia）+ 集合（lower_upsilon_without_dasia）→au。
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "au";
            }

            // *** Epsilon Iota 相关 ***

            if (upper_epsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_epsilon_without_dasia）+集合（upper_iota_with_dasia）→HEI；
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HEI";
                // 集合（upper_epsilon_without_dasia）+集合（upper_iota_without_dasia）→EI；
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "EI";
                // 集合（upper_epsilon_without_dasia）+集合（lower_iota_with_dasia）→Hei；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hei";
                // 集合（upper_epsilon_without_dasia）+集合（lower_iota_without_dasia）→Ei；
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Ei";
            }

            if (lower_epsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_epsilon_without_dasia）+集合（lower_iota_with_dasia）→hei；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hei";
                // 集合（lower_epsilon_without_dasia）+集合（lower_iota_without_dasia）→ei。
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ei";
            }

            // *** Epsilon Upsilon 相关 ***
            if (upper_epsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_epsilon_without_dasia）+集合（upper_upsilon_with_dasia）→HEU；
                if (upper_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "HEU";
                // 集合（upper_epsilon_without_dasia）+集合（upper_upsilon_without_dasia）→EU；
                if (upper_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "EU";
                // 集合（upper_epsilon_without_dasia）+集合（lower_upsilon_with_dasia）→Heu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "Heu";
                // 集合（upper_epsilon_without_dasia）+集合（lower_upsilon_without_dasia）→Eu；
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "Eu";
            }

            if (lower_epsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_epsilon_without_dasia）+集合（lower_upsilon_with_dasia）→heu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "heu";
                // 集合（lower_epsilon_without_dasia）+集合（lower_upsilon_without_dasia）→eu。
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "eu";
            }

            // *** Eta Iota 相关 ***

            if (upper_eta_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_eta_without_dasia）+集合（upper_iota_with_dasia）→HĒI；
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HĒI";
                // 集合（upper_eta_without_dasia）+集合（upper_iota_without_dasia）→ĒI；
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ĒI";
                // 集合（upper_eta_without_dasia）+集合（lower_iota_with_dasia）→Hēi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hēi";
                // 集合（upper_eta_without_dasia）+集合（lower_iota_without_dasia）→Ēi；
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Ēi";
            }

            if (lower_eta_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_eta_without_dasia）+集合（lower_iota_with_dasia）→hēi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hēi";
                // 集合（lower_eta_without_dasia）+集合（lower_iota_without_dasia）→ēi。
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ēi";
            }

            // *** Eta Upsilon 相关 ***

            if (upper_eta_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_eta_without_dasia）+集合（upper_upsilon_with_dasia）→HĒU；
                if (upper_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "HĒU";
                // 集合（upper_eta_without_dasia）+集合（upper_upsilon_without_dasia）→ĒU；
                if (upper_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "ĒU";
                // 集合（upper_eta_without_dasia）+集合（lower_upsilon_with_dasia）→Hēu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "Hēu";
                // 集合（upper_eta_without_dasia）+集合（lower_upsilon_without_dasia）→Ēu；
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "Ēu";
            }

            if (lower_eta_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_eta_without_dasia）+集合（lower_upsilon_with_dasia）→hēu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "hēu";
                // 集合（lower_eta_without_dasia）+集合（lower_upsilon_without_dasia）→ēu。
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "ēu";
            }

            // *** Omega Iota 相关 ***

            if (upper_omega_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_omega_without_dasia）+集合（upper_iota_with_dasia）→HŌI；
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HŌI";
                // 集合（upper_omega_without_dasia）+集合（upper_iota_without_dasia）→ŌI；
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ŌI";
                // 集合（upper_omega_without_dasia）+集合（lower_iota_with_dasia）→Hōi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hōi";
                // 集合（upper_omega_without_dasia）+集合（lower_iota_without_dasia）→Ōi；
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Ōi";
            }

            if (lower_omega_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_omega_without_dasia）+集合（lower_iota_with_dasia）→hōi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hōi";
                // 集合（lower_omega_without_dasia）+集合（lower_iota_without_dasia）→ōi。
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ōi";
            }

            // *** Omega Upsilon 相关 ***

            if (upper_omega_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_omega_without_dasia）+集合（upper_upsilon_with_dasia）→HŌU；
                if (upper_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "HŌU";
                // 集合（upper_omega_without_dasia）+集合（upper_upsilon_without_dasia）→ŌU；
                if (upper_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "ŌU";
                // 集合（upper_omega_without_dasia）+集合（lower_upsilon_with_dasia）→Hōu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "Hōu";
                // 集合（upper_omega_without_dasia）+集合（lower_upsilon_without_dasia）→Ōu；
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "Ōu";
            }

            if (lower_omega_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_omega_without_dasia）+集合（lower_upsilon_with_dasia）→hōu；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "hōu";
                // 集合（lower_omega_without_dasia）+集合（lower_upsilon_without_dasia）→ōu。
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "ōu";
            }

            // *** Omicron Iota 相关 ***

            if (upper_omicron_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_omicron_without_dasia）+集合（upper_iota_with_dasia）→HOI；
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HOI";
                // 集合（upper_omicron_without_dasia）+集合（upper_iota_without_dasia）→OI；
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "OI";
                // 集合（upper_omicron_without_dasia）+集合（lower_iota_with_dasia）→Hoi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hoi";
                // 集合（upper_omicron_without_dasia）+集合（lower_iota_without_dasia）→Oi；
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Oi";
            }

            if (lower_omicron_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_omicron_without_dasia）+集合（lower_iota_with_dasia）→hoi；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hoi";
                // 集合（lower_omicron_without_dasia）+集合（lower_iota_without_dasia）→oi。
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "oi";
            }

            // *** Omicron Upsilon 相关 ***

            if (upper_omicron_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_omicron_without_dasia）+集合（upper_upsilon_with_dasia）→HOU；
                if (upper_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "HOU";
                // 集合（upper_omicron_without_dasia）+集合（upper_upsilon_without_dasia）→OU；
                if (upper_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "OU";
                // 集合（upper_omicron_without_dasia）+集合（lower_upsilon_with_dasia）→Hou；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "Hou";
                // 集合（upper_omicron_without_dasia）+集合（lower_upsilon_without_dasia）→Ou；
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "Ou";
            }

            if (lower_omicron_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_omicron_without_dasia）+集合（lower_upsilon_with_dasia）→hou；
                if (lower_upsilon_with_dasia.Where(x => s2 == x).Any())
                    return "hou";
                // 集合（lower_omicron_without_dasia）+集合（lower_upsilon_without_dasia）→ou。
                if (lower_upsilon_without_dasia.Where(x => s2 == x).Any())
                    return "ou";
            }

            // *** Upsilon Iota 相关 ***

            if (upper_upsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（upper_upsilon_without_dasia）+集合（upper_iota_with_dasia）→HUI；
                if (upper_iota_with_dasia.Where(x => s2 == x).Any())
                    return "HUI";
                // 集合（upper_upsilon_without_dasia）+集合（upper_iota_without_dasia）→UI；
                if (upper_iota_without_dasia.Where(x => s2 == x).Any())
                    return "UI";
                // 集合（upper_upsilon_without_dasia）+集合（lower_iota_with_dasia）→Hui；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "Hui";
                // 集合（upper_upsilon_without_dasia）+集合（lower_iota_without_dasia）→Ui；
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "Ui";
            }

            if (lower_upsilon_without_dasia.Where(x => s1 == x).Any())
            {
                // 集合（lower_upsilon_without_dasia）+集合（lower_iota_with_dasia）→hui；
                if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                    return "hui";
                // 集合（lower_upsilon_without_dasia）+集合（lower_iota_without_dasia）→ui。
                if (lower_iota_without_dasia.Where(x => s2 == x).Any())
                    return "ui";
            }


            return null;
        }

        // 查找希腊文字符对应的罗马化形态
        public static string? LookupChar(string greek,
        // string prev_char,   // greek 的倒退一个位置的字符
        LookupStyle style,
        out int step)
        {
            if (greek.Length == 2)
            {
                var temp = LookupDoubleChar(greek, style, out step);
                if (temp != null)
                    return temp;
            }

            step = greek.Length;

            // ancient modern

            // ****************
            // *** 大写部分 ***

            // =============
            // === alpha ===
            // 双元音，并且在词首
            if (Array.IndexOf(upper_alpha_diphthong_ais, greek) != -1
                && (style & LookupStyle.head) != 0)
                return "HAI";
            if (Array.IndexOf(upper_alpha_diphthong_aus, greek) != -1
                && (style & LookupStyle.head) != 0)
                return "HAU";

            if (IsUpperAlphaDasia(greek))
                return "HA";
            if (IsUpperAlpha(greek))
                return "A";

            // ============
            // === beta ===
            if (greek == "Β")     // 0392 (Β)
            {
                if ((style & LookupStyle.ancient) != 0)
                    return "B";  // (古代和中世纪)
                Debug.Assert((style & LookupStyle.modern) != 0);
                return "V";  // (现代)
            }

            // =============
            // === gamma ===
            // Γ→G
            if (greek == "Γ")
                return "G";
            // ΓΓ→NG
            if (greek == "ΓΓ")
                return "NG";
            // Γκ→Gk （词首和词尾）
            if (greek == "Γκ")
            {
                if ((style & LookupStyle.head) != 0
                    || (style & LookupStyle.tail) != 0)
                    return "Gk";    // (词首和词尾)
                else if ((style & LookupStyle.middle) != 0)
                    return "Nk";    // (词中)
                else
                    throw new TransliterException("style 必须包含 head tail middle 之一");
            }
            // ΓΚ→GK（词首和词尾）ΓΚ→NK（词中）
            if (greek == "ΓΚ")
            {
                if ((style & LookupStyle.head) != 0
                    || (style & LookupStyle.tail) != 0)
                    return "GK";    // (词首和词尾)
                else if ((style & LookupStyle.middle) != 0)
                    return "NK";    // (词中)
                else
                    throw new TransliterException("style 必须包含 head tail middle 之一");
            }
            // ΓΞ→NX
            if (greek == "ΓΞ")
                return "NX";
            // ΓΧ→NCH。
            if (greek == "ΓΧ")
                return "NCH";

            // =============
            // === delta ===
            if (greek == "Δ")
                return "D";

            // ===============
            // === epsilon ===

            {
                // 2022/8/29
                // Ελβετικη→Helvetikē；ελλαδος→hellados
                if (greek == "Ελλ"
                    && (style & LookupStyle.head) != 0)
                    return "Hell";
                if (greek == "Ελ"
                    && (style & LookupStyle.head) != 0)
                    return "Hel";
            }

            // ①Ε→E；
            if (greek == "Ε" /*|| greek == "Έ"*/)
                return "E";
            // ②Ἑ→HE、Ἕ→HE、Ἓ→HE；
            if (greek == "Ἑ"
                || greek == "Ἕ"
                || greek == "Ἓ")
                return "HE";
            // ③有气号（Dasia）以外其它变音符号（Έ Έ Ὲ Ἐ Ἔ Ἒ）不转换；
            if (upper_epsilon_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "E";
            // ④双元音：ΕἹ→HEI、ΕἽ→HEI、ΕἻ→HEI、ΕἿ→HEI、 ΕὙ→HEU、ΕὝ→HEU、ΕὛ→HEU、ΕὟ→HEU
            if (greek.Length == 2)
            {
                if (greek == "ΕἹ"
    || greek == "ΕἽ"
    || greek == "ΕἻ"
    || greek == "ΕἿ")
                    return "HEI";
                if (greek == "ΕὙ"
                    || greek == "ΕὝ"
                    || greek == "ΕὛ"
                    || greek == "ΕὟ")
                    return "HEU";
            }

            // ============
            // === zeta ===

            // Ζ：Ζ→Z
            if (greek == "Ζ")
                return "Z";

            // ===========
            // === eta ===
            // ①Η→Ē；
            if (greek == "Η")
                return "Ē";
            // ②Ἡ→HĒ、Ἥ→HĒ、Ἣ→HĒ、ᾟ→HĒ、Ἧ→HĒ、ᾙ→HĒ、ᾝ→HĒ、ᾛ→HĒ；
            if (IsUpperEtaWithDasia(greek))
                return "HĒ";
            // ③有气号（Dasia）以外其它变音符号（Ή Ή Ὴ Ἠ Ἤ Ἢ ᾞ ῌ Ἦ ᾘ ᾜ ᾚ）不转换
            if (upper_eta_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "Ē";

            // ④双元音：ΗΥ→ĒU、ΗὙ→HĒU、ΗὝ→HĒU、ΗὛ→HĒU、ΗὟ→HĒU
            if (greek.Length == 2)
            {
                if (greek == "ΗΥ")
                    return "ĒU";
                if (greek == "ΗὙ"
                    || greek == "ΗὝ"
                    || greek == "ΗὛ"
                    || greek == "ΗὟ")
                    return "HĒU";
            }


            // =============
            // === theta ===

            // Θ→Th
            /*
            if (greek == "Θ")
                return "TH";
            */
            if (greek[0] == 'Θ')
            {
                step = 1;
                return AdjustUpperLower("TH", IsSecondCharUpperCase(greek, true));
            }

            // ============
            // === iota ===

            // ①单个Ι→I；
            if (greek == "Ι")
                return "I";
            // ②Ἱ→HI、Ἵ→HI、Ἳ→HI、Ἷ→HI；
            if (greek == "Ἱ"
    || greek == "Ἵ"
    || greek == "Ἳ"
    || greek == "Ἷ")
                return "HI";
            // ③有气号以外的其它变音符号（Ί Ί Ὶ Ϊ Ῐ Ῑ Ἰ Ἲ Ἴ）不转换；
            if (upper_iota_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "I";

            // ④双元音：ΑἹ→HAI、ΕἹ→HEI、ΥἹ→HUI、ΟἹ→HOI
            if (greek.Length == 2)
            {
                if (greek == "ΑἹ")
                    return "HAI";
                if (greek == "ΕἹ")
                    return "HEI";
                if (greek == "ΥἹ")
                    return "HUI";
                if (greek == "ΟἹ")
                    return "HOI";
            }

#if REMOVED
            // ⑤Ι位于词首，其后紧随ΔΡ或δρ且紧随带变音符号的元音时，（20220919）ΙΔ→Hid、Ιδ→Hid
            if ((style & LookupStyle.head) != 0)
            {
                if (greek == "ΙΔ")
                    return "HID";
                if (greek == "Ιδ")
                    return "Hid";
            }
#endif

            // =============
            // === kappa ===
            // Κ→K
            if (greek == "Κ")
                return "K";

            // ==============
            // === lambda ===
            // Λ→L
            if (greek == "Λ")
                return "L";

            // ==========
            // === mu ===
            // ①Μ→M；
            if (greek == "Μ")
                return "M";
            // ②ΜΠ→B（词首）、 Μπ→B（词首）
            if ((style & LookupStyle.head) != 0
                && (greek == "ΜΠ" || greek == "Μπ")
                )
                return "B";
            // ③ΜΠ→MP（词中和词尾）、 Μπ→Mp（词中和词尾）。
            if ((style & LookupStyle.middle) != 0
                || (style & LookupStyle.tail) != 0)
            {
                if (greek == "ΜΠ")
                    return "MP";
                if (greek == "Μπ")
                    return "Mp";
            }

            // ==========
            // === nu ===

            // ①Ν→N；
            if (greek == "Ν")
                return "N";
            // ②Ντ→Ḏ（词首）、ΝΤ→Ḏ（词首）；
            if ((style & LookupStyle.head) != 0
                && (greek == "Ντ" || greek == "ΝΤ")
                )
                return "Ḏ";
            // ③ΝΤ→nt（词中和词尾）
            if ((style & LookupStyle.middle) != 0
    || (style & LookupStyle.tail) != 0)
            {
                if (greek == "ΝΤ")
                    return "NT";    // ?? nt
            }

            // ==========
            // === xi ===

            // Ξ→X
            if (greek == "Ξ")
                return "X";

            // ===============
            // === omicron ===

            // ①Ο→O；
            if (greek == "Ο")
                return "O";
            // ②Ὁ→HO、Ὅ→HO、Ὃ→HO；
            /*
            if (greek == "Ὁ" || greek == "Ὅ" || greek == "Ὃ")
                return "HO";
            */
            if (greek[0] == 'Ὁ' || greek[0] == 'Ὅ' || greek[0] == 'Ὃ')
            {
                step = 1;
                return AdjustUpperLower("HO", IsSecondCharUpperCase(greek, true));
            }


            // ③有气号（Dasia）以外其它变音符号（Ό Ό Ὸ Ὀ Ὄ Ὂ）不转换；
            if (upper_omicron_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "O";
            // ④双元音：ΟἹ→HOI、ΟἽ→HOI、ΟἻ→HOI、ΟἿ→HOI、ΟὙ→HOU、ΟὝ→HOU、ΟὛ→HOU、ΟὟ→HOU
            if (greek.Length == 2)
            {
                if (greek == "ΟἹ"
                    || greek == "ΟἽ"
                    || greek == "ΟἻ"
                    || greek == "ΟἿ")
                    return "HOI";
                if (greek == "ΟὙ"
                    || greek == "ΟὝ"
                    || greek == "ΟὛ"
                    || greek == "ΟὟ")
                    return "HOU";
            }
            // ⑤ΟΥ→OU
            if (greek == "ΟΥ")
                return "OU";

            // ==========
            // === pi ===

            // Π→P
            if (greek == "Π")
                return "P";

            // ===========
            // === rho ===
            // ①Ρ→R；
            if (greek == "Ρ")
                return "R";
            // ②Ῥ→RH。
            /*
            if (greek == "Ῥ")  // （注：῾是希腊文Dasia，有气号）
                return "RH"; //（参照注释）有气号（῾）罗马化为英文字母h。当有气号与元音或双元音同时出现时，字母h需前置于被罗马化的元音或双元音；当有气号和rho(῾Ρ,ῥ)一起出现时，字母h需后置于被罗马化的rho(Rh,rh)。当希腊语文本未出现有气号时，根据实际需要添加字母h（如文本全部是大写形式或现代希腊语文本都是单调正字法）。但是，只有现代希腊语文本出现有气号时，῾Ρ/ῥ被罗马化为Rh/rh。
            */
            if (greek[0] == 'Ῥ')
            {
                step = 1;
                return AdjustUpperLower("RH", IsSecondCharUpperCase(greek, true));
            }

            // =============
            // === sigma ===
            // Σ：Σ→S。
            if (greek == "Σ")
                return "S";
            // Ϲ：Ϲ→S。
            if (greek == "Ϲ")
                return "S";

            // ===========
            // === tau ===
            // Τ→T。
            if (greek == "Τ")
                return "T";

            // ===============
            // === upsilon ===
            // ①Υ→Y；
            if (greek == "Υ")
                return "Y";
            // ②双元音：ΑΥ、ΕΥ、ΗΥ、ΟΥ、ΥΙ，Υ→U，即ΑΥ→AU、ΕΥ→EU、ΗΥ→ĒU、ΟΥ→OU、ΥΙ→UI；
            if (greek.Length == 2)
            {
                if (greek == "ΑΥ")
                    return "AU";
                if (greek == "ΕΥ")
                    return "EU";
                if (greek == "ΗΥ")
                    return "ĒU";
                if (greek == "ΟΥ")
                    return "OU";
                if (greek == "ΥΙ")
                    return "UI";
            }
            // 临时增加规则(2022/10/13): ΩΥ→ŌU
            if (greek == "ΩΥ")
                return "ŌU";
            // ③有气号（Dasia）以外其它变音符号（Ύ Ύ Ὺ Ϋ Ῠ Ῡ ϒ ϓ ϔ）不转换；
            if (upper_upsilon_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "Y";
            // ⑤Ὑ、Ὕ、Ὓ、Ὗ位于词首且与元音Ι紧邻，执行②，且有气号转换为拉丁字母H位于词首。如ὙΙ→HUI、ὝΙ→HUI、ὛΙ→HUI、ὟΙ→HUI
            if ((style & LookupStyle.head) != 0
                && (greek == "ὙΙ" || greek == "ὝΙ" || greek == "ὛΙ" || greek == "ὟΙ")
                )
                return "HUI";
            // 注意: 步骤(4) 故意放在步骤 (5) 后面的
            // ④Ὑ、Ὕ、Ὓ、Ὗ位于词首且不与元音Ι紧邻，Ὑ→HY、Ὕ→HY、Ὓ→HY、Ὗ→HY；
            bool is_tail = (greek.Length == 1 && (style & LookupStyle.tail) != 0);
            if ((style & LookupStyle.head) != 0
                && (greek.Length == 2 || is_tail)
                && (greek[0] == 'Ὑ'/*1f59*/ || greek[0] == 'Ὕ'/*1f5d*/
                || greek[0] == 'Ὓ'/*1f5b*/ || greek[0] == 'Ὗ'/*1f5f*/)
                )
            {
                if (is_tail == false)
                    step--; // 退回最后字母 greek[1] 让下次继续处理

                return AdjustUpperLower("HY", IsSecondCharUpperCase(greek, true));
            }

            // ===========
            // === phi ===
            // Φ→Ph
            /*
            if (greek == "Φ")
                return "PH";
            */

            if (greek[0] == 'Φ')
            {
                step = 1;
                return AdjustUpperLower("PH", IsSecondCharUpperCase(greek, true));
            }


            // ===========
            // === chi ===
            // Χ→Ch
            /*
            if (greek == "Χ")
            {
                return "CH";
            }
            */
            if (greek[0] == 'Χ')
            {
                step = 1;
                return AdjustUpperLower("CH", IsSecondCharUpperCase(greek, true));
            }

            // ===========
            // === psi ===
            // Ψ→Ps
            if (greek == "Ψ")
                return "PS";

            // === omega
            // ①Ω→Ō；
            if (greek == "Ω")
                return "Ō";

            // ②Ὡ→HŌ、Ὥ→HŌ、Ὣ→HŌ、ᾯ→HŌ、Ὧ→HŌ、ᾩ→HŌ、ᾭ→HŌ、ᾫ→HŌ；
            if (IsUpperOmegaWithDasia(greek))
                return "HŌ";

            // ③有气号（Dasia）以外其它变音符号（Ώ Ώ Ὼ Ὠ Ὤ Ὢ ᾮ Ὦ ᾨ ᾬ ᾪ ῼ）不转换。
            if (upper_omega_without_dasia/*upper_omega_diacritics_exclude_dasia*/.Where(x => greek == x).Any())
                return "Ō";

            // === digamma ===
            // Ϝ→W
            if (greek == "Ϝ")
                return "W";

            // === Qoppa ===
            // Ϙ→Ḳ
            if (greek == "Ϙ")
                return "Ḳ";

            // ***************
            // *** 小写部分 ***

            // === alpha
            /*
            // 双元音，并且在词首
            if (Array.IndexOf(lower_alpha_diphthong_ais, greek) != -1
                && (style & LookupStyle.head) != 0)
                return "hai";
            */
#if REMOVED // 已经集中处理
            // 双元音αι 集合1+集合2→hai；集合1+集合3→ai
            if (greek.Length == 2)
            {
                string s1 = greek.Substring(0, 1);
                string s2 = greek.Substring(1, 1);
                if (lower_alpha_chars.Where(x => s1 == x).Any())
                {
                    if (lower_iota_with_dasia.Where(x => s2 == x).Any())
                        return "hai";
                    if (lower_iota_diacritics_exclude_dasia.Where(x => s2 == x).Any())
                        return "ai";
                }
            }
#endif

            if (Array.IndexOf(lower_alpha_diphthong_aus, greek) != -1
                && (style & LookupStyle.head) != 0)
                return "hau";

            if (IsLowerAlphaDasia(greek))
                return "ha";
            if (IsLowerAlpha(greek))
                return "a";
            /*
            if (greek == "α")
                return "a";
            // 2022/8/29
            if (greek == "ά")
                return "a";
            */
            // === beta
            if (greek == "β")
            {
                if ((style & LookupStyle.ancient) != 0)
                    return "b"; // (古代和中世纪）
                Debug.Assert((style & LookupStyle.modern) != 0);
                return "v"; // (现代)
            }

            // === gamma
            // γ→g
            if (greek == "γ")
                return "g";
            // γγ→ng
            if (greek == "γγ")
                return "ng";
            // γκ→gk（词首和词尾）；②γκ→nk（词中）；③。
            if (greek == "γκ")
            {
                if ((style & LookupStyle.head) != 0
                    || (style & LookupStyle.tail) != 0)
                    return "gk";    // (词首和词尾)
                else if ((style & LookupStyle.middle) != 0)
                    return "nk";    // (词中)
                else
                    throw new TransliterException("style 必须包含 head tail middle 之一");
            }

            // γξ→nx
            if (greek == "γξ")
                return "nx";
            // γχ→nch
            if (greek == "γχ")
                return "nch";

            // === delta
            if (greek == "δ")
                return "d";

            // === epsilon
            {
                // 2022/8/29
                // Ελβετικη→Helvetikē；ελλαδος→hellados
                if ((greek == "ελλ" || greek == "έλλ")
                    && (style & LookupStyle.head) != 0)
                    return "hell";
                if ((greek == "ελ" || greek == "έλ")
                    && (style & LookupStyle.head) != 0)
                    return "hel";
            }

            // ①ε→e；
            if (greek == "ε" /*|| greek == "έ"*/)
                return "e";
            // ②ἑ→he、ἕ→he、ἓ→he、ἕ→he；
            if (greek == "ἑ"
                || greek == "ἕ"
                || greek == "ἓ"
                || greek == "ἕ")
                return "he";
            // ③有气号（Dasia）以外其它变音符号（έ έ ὲ ἐ ἔ ἒ）不转换；
            if (lower_epsilon_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "e";
            // ④双元音：εἱ→hei、 εἵ→hei、εἳ→hei、εἷ→hei、εὑ→heu、εὕ→heu、εὓ→heu、εὗ→heu
            if (greek.Length == 2)
            {
                if (greek == "εἱ"
    || greek == "εἵ"
    || greek == "εἳ"
    || greek == "εἷ")
                    return "hei";
                if (greek == "εὑ"
                    || greek == "εὕ"
                    || greek == "εὓ"
                    || greek == "εὗ")
                    return "heu";
            }

            // === zeta

            // ζ：ζ→z
            if (greek == "ζ")
                return "z";

            // === eta
            // ①η→ē
            if (greek == "η" /*|| greek == "ή"*/)
                return "ē";
            // ②ἡ→hē、ἥ→hē、ἣ→hē、ᾗ→hē、ἧ→hē、ᾑ→hē、ᾕ→hē、ᾓ→hē；
            if (IsLowerEtaWithDasia(greek))
                return "hē";
            // ③有气号（Dasia）以外其它变音符号（ή ή ὴ ῇ ἠ ἤ ἢ ᾖ ῆ ῃ ῄ ῂ ἦ ᾐ ᾔ ᾒ）不转换。
            if (lower_eta_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "ē";
            // ④双元音：ηυ→ēu、ηὑ→hēu、ηὕ→hēu、ηὓ→hēu、ηὗ→hēu
            if (greek.Length == 2)
            {
                if (greek == "ηυ")
                    return "ēu";
                if (greek == "ηὑ"
                    || greek == "ηὕ"
                    || greek == "ηὓ"
                    || greek == "ηὗ")
                    return "hēu";
            }

            // === theta

            // θ→th
            if (greek == "θ")
                return "th";

            // === iota
            // ①单个ι→i；
            if (greek == "ι" /*|| greek == "ί"*/)
                return "i";
            // ②ἱ→hi、ἵ→hi、ἳ→hi、ἷ→hi；
            if (greek == "ἱ"
                || greek == "ἵ"
                || greek == "ἳ"
                || greek == "ἷ")
                return "hi";
            // ③有气号以外的其它变音符号（ί ί ὶ ϊ ῐ ῑ ΐ ΐ ῒ ἰ ἴ ἲ ῖ ῗ ἶ）不转换；
            if (lower_iota_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "i";
            // ④双元音：αἱ→hai、εἱ→hei、υἱ→hui、οἱ→hoi；
            if (greek.Length == 2)
            {
                if (greek == "αἱ")
                    return "hai";
                if (greek == "εἱ")
                    return "hei";
                if (greek == "υἱ")
                    return "hui";
                if (greek == "οἱ")
                    return "hoi";
            }

#if REMOVED
            // ⑤ι位于词首，其后紧随δρ且紧随带变音符号的元音时，（20220919），ιδ→hid。
            if (greek == "ιδ"
                && (style & LookupStyle.head) != 0)
                return "hid";
#endif

            // === kappa
            // κ→k
            if (greek == "κ")
                return "k";

            // === lambda
            // λ→l
            if (greek == "λ")
                return "l";

            // === mu
            // ①μ→m；
            if (greek == "μ")
                return "m";
            // ②μπ→b（词首）；
            if (greek == "μπ"
                && (style & LookupStyle.head) != 0)
                return "b";
            // ③μπ→mp（词中和词尾）。
            if ((style & LookupStyle.middle) != 0 || (style & LookupStyle.tail) != 0)
            {
                if (greek == "μπ")
                    return "mp";  // (词中和词尾)
            }

            // === nu
            // ①ν→n；
            if (greek == "ν")
                return "n";
            // ②ντ→ḏ（词首）；
            // ③ντ→nt（词中和词尾）。
            if (greek == "ντ")
            {
                if ((style & LookupStyle.head) != 0)
                    return "ḏ";  // (词首)
                else if ((style & LookupStyle.middle) != 0
                    || (style & LookupStyle.tail) != 0)
                    return "nt";  // (词中和词尾)
                else
                    throw new TransliterException("???");
            }

            // ==========
            // === xi ===

            // ξ→x
            if (greek == "ξ")
                return "x";

            // ===============
            // === omicron ===

            // ①ο→o；
            if (greek == "ο"/* || greek == "ό"*/)
                return "o";
            // ②ὁ→ho、ὅ→ho、ὃ→ho；
            if (greek == "ὁ" || greek == "ὅ" || greek == "ὃ")
                return "ho";
            // ③有气号（Dasia）以外其它变音符号（ό ό ὸ ὀ ὄ ὂ）不转换；
            if (lower_omicron_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "o";
            // ④双元音：οἱ→hoi、οἱ→hoi、οἵ→hoi、οἳ→hoi、οἷ→hoi、οἱ→hoi、οὑ→hou、οὕ→hou、οὓ→hou、οὗ→hou；
            if (greek.Length == 2)
            {
                // ④双元音：→、→hoi、→hoi、→hoi、→hoi、→hoi、
                if (greek == "οἱ"
                    || greek == "οἱ"
                    || greek == "οἵ"
                    || greek == "οἳ"
                    || greek == "οἷ"
                    || greek == "οἱ")   // ? 重复了
                    return "hoi";
                if (greek == "οὑ"
                    || greek == "οὕ"
                    || greek == "οὓ"
                    || greek == "οὗ")
                    return "hou";
            }
            // ⑤ου→ou
            if (greek == "ου")
                return "ou";

            // ==========
            // === pi ===

            // π→p
            if (greek == "π")
                return "p";

            // ===========
            // === rho ===
            // ①ρ→r；
            if (greek == "ρ")
                return "r";
            // ②ῥ→rh。
            if (greek == "ῥ")   //（注：希腊小写字母ρ带Dasia有气号）
                return "rh";    //  	(参照注释)

            // =============
            // === sigma ===
            // σ：σ→s。
            if (greek == "σ")
                return "s";
            // ϲ：ϲ→s。
            if (greek == "ϲ")
                return "s";
            // ς：ς→s（词尾）。
            if (greek == "ς")
            {
                if ((style & LookupStyle.tail) != 0)
                    return "s";// (词尾)
                else
                {
                    // TODO: 继续向后搜索，还是抛出异常?
                    throw new TransliterException("字母 ς 不应该出现在非词尾的位置");
                }
            }

            // ===========
            // === tau ===
            // τ→t
            if (greek == "τ")
                return "t";

            // ===============
            // === upsilon ===

            // ①υ→y；
            if (greek == "υ" /*|| greek == "ύ"*/)
                return "y";
            // ②双元音：αυ、ευ、ηυ 、ου、 υι，υ→u，即αυ→au、ευ→eu、ηυ→ēu 、ου→ou、υι→ui；
            // 2022/8/27
            // (双元音αυ, ευ, ηυ, ου, υι和ωυ为u) 意思是说要两个一起看
            // 即αυ→au、ευ→eu、ηυ→ēu 、ου→ou、υι→ui；
            if (greek == "αυ")
                return "au";
            if (greek == "ευ")
                return "eu";
            if (greek == "ηυ")
                return "ēu";
            if (greek == "ου")
                return "ou";
            if (greek == "υι")
                return "ui";

            // 临时增加的规则: ωυ→ōu;
            if (greek == "ωυ")
                return "ōu";

            // ③有气号（Dasia）以外其它变音符号（ύ ύ ὺ ϋ ῠ ῡ ΰ ΰ ῢ ὐ ὔ ὒ ῦ ῧ ὖ）不转换；
            if (lower_upsilon_diacritics_exclude_dasia.Where(x => greek == x).Any())
                return "y";
            // ⑤ὑ、ὕ、ὓ、ὗ位于词首且与其它元音ι紧邻，执行②，且有气号转换为拉丁字母h位于词首。如ὑι→hui、ὕι→hui、ὓι→hui、ὗι→hui。
            if ((style & LookupStyle.head) != 0
    && (greek == "ὑι" || greek == "ὕι" || greek == "ὓι" || greek == "ὗι")
    )
                return "hui";
            // 注意: 步骤(4) 故意放在步骤 (5) 后面的
            // ④ὑ、ὕ、ὓ、ὗ位于词首且不与元音ι紧邻，ὑ→hy、ὕ→hy、ὓ→hy、ὗ→hy；
            if ((style & LookupStyle.head) != 0
    && (greek.Length == 2 || is_tail)
    && (greek[0] == 'ὑ' || greek[0] == 'ὕ'
    || greek[0] == 'ὓ' || greek[0] == 'ὗ')
    )
            {
                if (is_tail == false)
                    step--; // 退回最后字母 greek[1] 让下次继续处理
                return "hy";
            }

            // ===========
            // === phi ===
            // φ→ph
            if (greek == "φ")
            {
                // return "ph";
                step = 1;
                return AdjustUpperLower("ph", IsSecondCharUpperCase(greek, false));
            }

            // ===========
            // === chi ===
            // χ→ch
            if (greek == "χ")
            {
                // return "ch";
                step = 1;
                return AdjustUpperLower("ch", IsSecondCharUpperCase(greek, false));
            }

            // ===========
            // === psi ===
            // ψ→ps
            if (greek == "ψ")
            {
                // return "ps";
                step = 1;
                return AdjustUpperLower("ps", IsSecondCharUpperCase(greek, false));
            }

            // === omega
            // ①ω→ō；
            if (greek == "ω" || greek == "ώ")
                return "ō";
            // ②ὡ→hō、ὥ→hō、ὣ→hō、ᾧ→hō、ὧ→hō、ᾡ→hō、ᾥ→hō、ᾣ→hō；
            if (IsLowerOmegaWithDasia(greek))
            {
                // return "hō";
                step = 1;
                return AdjustUpperLower("hō", IsSecondCharUpperCase(greek, false));
            }
            // ③有气号（Dasia）以外其它变音符号（ώ ώ ὼ ῷ ὠ ὤ ὢ ᾦ ῶ ῳ ῴ ῲ ὦ ᾠ ᾤ ᾢ）不转换。
            if (lower_omega_without_dasia/*lower_omega_diacritics_exclude_dasia*/.Where(x => greek == x).Any())
                return "ō";

            // *** 古体字母

            // === digamma ===
            // ϝ→w
            if (greek == "ϝ")
                return "w";

            // === qoppa ===
            // ϙ→ḳ
            if (greek == "ϙ")
                return "ḳ";

            return null;    // 没有找到
        }

#region 字母集合

#region 字母 Alpha

        // 带有气号的希腊文 Alpha 字母
        // ①希腊字母集合1：Ἁ Ἃ Ἅ Ἇ ᾉ ᾋ ᾍ ᾏ（8个）→HA
        // 此集合中字符罗马化为拉丁字母HA。
        static string[] upper_alpha_with_dasia = new string[] {
            "\u1f09",   // (Ἁ)
            "\u1f0b",   // (Ἃ)
            "\u1f0d",   // (Ἅ)
            "\u1f0f",   // (Ἇ)
            "\u1f89",   // (ᾉ)
            "\u1f8b",   // (ᾋ)
            "\u1f8d",   // (ᾍ)
            "\u1f8f",   // (ᾏ)
        };

        // ②希腊字母集合2：Α Ά Ά Ὰ Ᾰ Ᾱ Ἀ Ἄ Ἂ ᾎ ᾼ Ἆ ᾈ ᾌ ᾊ（15个）→A
        // 此集合中字符罗马化为拉丁字母A。
        static string[] upper_alpha_without_dasia = new string[] {
            "\u0391",   // (Α)  // ?
            "\u0386",   // (Ά)
            "\u1fbb",   // (Ά)
            "\u1fba",   // (Ὰ)
            "\u1fb8",   // (Ᾰ)
            "\u1fb9",   // (Ᾱ)
            "\u1f08",   // (Ἀ)
            "\u1f0c",   // (Ἄ)
            "\u1f0a",   // (Ἂ)
            "\u1f8e",   // (ᾎ)
            "\u1fbc",   // (ᾼ)
            "\u1f0e",   // (Ἆ)
            "\u1f88",   // (ᾈ)
            "\u1f8c",   // (ᾌ)
            "\u1f8a",   // (ᾊ)
        };

        // ③希腊字母集合3：ἁ ἃ ἅ ἇ ᾁ ᾃ ᾅ ᾇ（8个）→ha
        // 集合中字符罗马化为拉丁字母ha。
        static string[] lower_alpha_with_dasia = new string[] {
            "\u1f01",   // (ἁ)
            "\u1f03",   // (ἃ)
            "\u1f05",   // (ἅ)
            "\u1f07",   // (ἇ)
            "\u1f81",   // (ᾁ)
            "\u1f83",   // (ᾃ)
            "\u1f85",   // (ᾅ)
            "\u1f87",   // (ᾇ)
        };

        // ④希腊字母集合4（lower_α_without dasia）：α ά ά ὰ ᾰ ᾱ ᾷ ἀ ἄ ἂ ᾆ ᾶ ᾳ ᾴ ᾲ ἆ ᾀ ᾄ ᾂ（19个）→a
        // 集合中字符罗马化为拉丁字母a。
        static string[] lower_alpha_without_dasia = new string[] {
            "\u03b1",   // (α)
            "\u03ac",   // (ά)
            "\u1f71",   // (ά)
            "\u1f70",   // (ὰ)
            "\u1fb0",   // (ᾰ)
            "\u1fb1",   // (ᾱ)
            "\u1fb7",   // (ᾷ)
            "\u1f00",   // (ἀ)
            "\u1f04",   // (ἄ)
            "\u1f02",   // (ἂ)
            "\u1f86",   // (ᾆ)
            "\u1fb6",   // (ᾶ)
            "\u1fb3",   // (ᾳ)
            "\u1fb4",   // (ᾴ)
            "\u1fb2",   // (ᾲ)
            "\u1f06",   // (ἆ)
            "\u1f80",   // (ᾀ)
            "\u1f84",   // (ᾄ)
            "\u1f82",   // (ᾂ)
        };

        static string[] upper_alpha_chars = new string[]
        {
            "\u0386",   //  (Ά)
            "\u0391",   //  (Α)
            "\u1f08",   //  (Ἀ)
            "\u1f09",   //  (Ἁ)
            "\u1f0a",   //  (Ἂ)
            "\u1f0b",   //  (Ἃ)
            "\u1f0c",   //  (Ἄ)
            "\u1f0d",   //  (Ἅ)
            "\u1f0e",   //  (Ἆ)
            "\u1f0f",   //  (Ἇ)
            "\u1f88",   //  (ᾈ)
            "\u1f89",   //  (ᾉ)
            "\u1f8a",   //  (ᾊ)
            "\u1f8b",   //  (ᾋ)
            "\u1f8c",   //  (ᾌ)
            "\u1f8d",   //  (ᾍ)
            "\u1f8e",   //  (ᾎ)
            "\u1f8f",   //  (ᾏ)
            "\u1fb8",   //  (Ᾰ)
            "\u1fb9",   //  (Ᾱ)
            "\u1fba",   //  (Ὰ)
            "\u1fbb",   //  (Ά)
            "\u1fbc",   //  (ᾼ)
        };

        static string[] lower_alpha_chars = new string[]
        {
            "\u03b1", // α 不带任何气号
            "\u03ac",   //   (ά)新增
            "\u1f00",   //   (ἀ)
            "\u1f02",   //   (ἂ)
            "\u1f04",   //   (ἄ)
            "\u1f06",   //   (ἆ)
            "\u1f70",   //   (ὰ)
            "\u1f71",   //   (ά)
            "\u1f80",   //   (ᾀ)
            "\u1f82",   //   (ᾂ)
            "\u1f84",   //   (ᾄ)
            "\u1f86",   //   (ᾆ)
            "\u1fb0",   //   (ᾰ)
            "\u1fb1",   //   (ᾱ)
            "\u1fb2",   //   (ᾲ)
            "\u1fb3",   //   (ᾳ)
            "\u1fb4",   //   (ᾴ)
            "\u1fb6",   //   (ᾶ)
            "\u1fb7",   //   (ᾷ)
        };

        static string[] lower_alpha_diphthong_ais = new string[] {
            "αἱ",   // 03b1 1f31
            "αἵ",   // 03b1 1f35
            "αἳ",   // 03b1 1f33
            "αἷ",   // 03b1 1f37
        };

        static string[] lower_alpha_diphthong_aus = new string[] {
            "αὑ",   // 03b1 1f51
            "αὕ",   // 03b1 1f55
            "αὓ",   // 03b1 1f53
            "αὗ",   // 03b1 1f57
        };

        static string[] upper_alpha_diphthong_ais = new string[] {
            "ΑἹ", // 0391 1f39
            "ΑἽ", // 0391 1f3d
            "ΑἻ", // 0391 1f3b
            "ΑἿ", // 0391 1f3f
        };

        static string[] upper_alpha_diphthong_aus = new string[] {
            "ΑὙ", // 0391 1f59
            "ΑὝ", // 0391 1f5d
            "ΑὛ", // 0391 1f5b
            "ΑὟ", // 0391 1f5f
        };

        static bool IsUpperAlpha(string char_string)
        {
            return upper_alpha_chars.Where(x => char_string == x).Any();
        }

        // 检测是不是带有气号的希腊文(大写) Alpha 字母
        static bool IsUpperAlphaDasia(string char_string)
        {
            return upper_alpha_with_dasia.Where(x => char_string == x).Any();
        }

        static bool IsLowerAlpha(string char_string)
        {
            return lower_alpha_chars.Where(x => char_string == x).Any();
        }

        // 检测是不是带有气号的希腊文(小写) Alpha 字母
        static bool IsLowerAlphaDasia(string char_string)
        {
            return lower_alpha_with_dasia.Where(x => char_string == x).Any();
        }

#endregion

#region 字母 Omicron

        // ①希腊字母集合（upper_omicron_with_dasia）：Ὁ Ὅ Ὃ（3个）→HO
        // 集合中字符转换为拉丁字母HO。
        static string[] upper_omicron_with_dasia = new string[] {
            "\u1f49",   // (Ὁ)
            "\u1f4d",   // (Ὅ)
            "\u1f4b",   // (Ὃ)
            // 共 3 个        
        };

        // ②希腊字母集合（upper_omicron_without_dasia）：Ο Ό Ό Ὸ Ὀ Ὄ Ὂ（7个）→O
        // 集合中字符罗马化为拉丁字母O。
        static string[] upper_omicron_without_dasia = new string[] {
            "\u039f",   // (Ο)
            "\u038c",   // (Ό)
            "\u1ff9",   // (Ό)
            "\u1ff8",   // (Ὸ)
            "\u1f48",   // (Ὀ)
            "\u1f4c",   // (Ὄ)
            "\u1f4a",   // (Ὂ)
            // 共 7 个        
        };

        // ③有气号（Dasia）以外其它变音符号（Ό Ό Ὸ Ὀ Ὄ Ὂ）不转换；
        static string[] upper_omicron_diacritics_exclude_dasia = new string[] {
            "\u038c",   // (Ό)
            "\u1ff9",   // (Ό)
            "\u1ff8",   // (Ὸ)
            "\u1f48",   // (Ὀ)
            "\u1f4c",   // (Ὄ)
            "\u1f4a",   // (Ὂ)
        };

        // ③希腊字母集合（lower_omicron_with_dasia）：ὁ ὅ ὃ（3个）→ho
        // 集合中个字符罗马化为拉丁字母ho。
        static string[] lower_omicron_with_dasia = new string[] {
            "\u1f41",   // (ὁ)
            "\u1f45",   // (ὅ)
            "\u1f43",   // (ὃ)
            // 共 3 个        
        };

        // ④希腊字母集合（lower_omicron_without_dasia）：ο ό ό ὸ ὀ ὄ ὂ（7个）→o
        // 集合中字符罗马化为拉丁字母ο。
        static string[] lower_omicron_without_dasia = new string[] {
            "\u03bf",   // (ο)
            "\u03cc",   // (ό)
            "\u1f79",   // (ό)
            "\u1f78",   // (ὸ)
            "\u1f40",   // (ὀ)
            "\u1f44",   // (ὄ)
            "\u1f42",   // (ὂ)
            // 共 7 个        
        };

        // ③有气号（Dasia）以外其它变音符号（ό ό ὸ ὀ ὄ ὂ）不转换；
        static string[] lower_omicron_diacritics_exclude_dasia = new string[] {
            "\u03cc",   // (ό)
            "\u1f79",   // (ό)
            "\u1f78",   // (ὸ)
            "\u1f40",   // (ὀ)
            "\u1f44",   // (ὄ)
            "\u1f42",   // (ὂ)
        };

#endregion

#region 字母 Omega

        // ①希腊字母集合（upper_omega_with_dasia）：Ὡ Ὥ Ὣ ᾯ Ὧ ᾩ ᾭ ᾫ（8个）→HŌ
        // 集合中字符罗马化为拉丁字母HŌ。
        static string[] upper_omega_with_dasia = new string[] {
            "\u1f69",   // (Ὡ)
            "\u1f6d",   // (Ὥ)
            "\u1f6b",   // (Ὣ)
            "\u1faf",   // (ᾯ)
            "\u1f6f",   // (Ὧ)
            "\u1fa9",   // (ᾩ)
            "\u1fad",   // (ᾭ)
            "\u1fab",   // (ᾫ)
            // 共 8 个
        };

        // 带变音符号的大写 Omega
        static bool IsUpperOmegaWithDasia(string char_string)
        {
            return upper_omega_with_dasia.Where(x => char_string == x).Any();
        }

        // ②希腊字母集合（upper_omega_without_dasia）：Ω Ώ Ώ Ὼ Ὠ Ὤ Ὢ ᾮ Ὦ ᾨ ᾬ ᾪ ῼ（13个）→Ō
        // 集合中字符罗马化为拉丁字母Ō。
        static string[] upper_omega_without_dasia = new string[] {
            "\u03a9",   // (Ω)
            "\u038f",   // (Ώ)
            "\u1ffb",   // (Ώ)
            "\u1ffa",   // (Ὼ)
            "\u1f68",   // (Ὠ)
            "\u1f6c",   // (Ὤ)
            "\u1f6a",   // (Ὢ)
            "\u1fae",   // (ᾮ)
            "\u1f6e",   // (Ὦ)
            "\u1fa8",   // (ᾨ)
            "\u1fac",   // (ᾬ)
            "\u1faa",   // (ᾪ)
            "\u1ffc",   // (ῼ)
            // 共 13 个        
        };

        /*
        // ③有气号（Dasia）以外其它变音符号（Ώ Ώ Ὼ Ὠ Ὤ Ὢ ᾮ Ὦ ᾨ ᾬ ᾪ ῼ）不转换。
        static string[] upper_omega_diacritics_exclude_dasia = new string[] {
            "\u038f",   // (Ώ)
            "\u1ffb",   // (Ώ)
            "\u1ffa",   // (Ὼ)
            "\u1f68",   // (Ὠ)
            "\u1f6c",   // (Ὤ)
            "\u1f6a",   // (Ὢ)
            "\u1fae",   // (ᾮ)
            "\u1f6e",   // (Ὦ)
            "\u1fa8",   // (ᾨ)
            "\u1fac",   // (ᾬ)
            "\u1faa",   // (ᾪ)
            "\u1ffc",   // (ῼ)
        };
        */

        // ③希腊字母集合（lower_omega_with_dasia）：ὡ ὥ ὣ ᾧ ὧ ᾡ ᾥ ᾣ（8个）→hō
        // 集合中字符转换为拉丁字母hō。
        // ②ὡ→hō、ὥ→hō、ὣ→hō、ᾧ→hō、ὧ→hō、ᾡ→hō、ᾥ→hō、ᾣ→hō；
        static string[] lower_omega_with_dasia = new string[] {
            "\u1f61",   // (ὡ)
            "\u1f65",   // (ὥ)
            "\u1f63",   // (ὣ)
            "\u1fa7",   // (ᾧ)
            "\u1f67",   // (ὧ)
            "\u1fa1",   // (ᾡ)
            "\u1fa5",   // (ᾥ)
            "\u1fa3",   // (ᾣ)
            // 共 8 个
        };

        // 带变音符号的小写 Omega
        static bool IsLowerOmegaWithDasia(string char_string)
        {
            return lower_omega_with_dasia.Where(x => char_string == x).Any();
        }

        // ④希腊字母集合（lower_omega_without_dasia）：ω ώ ώ ὼ ῷ ὠ ὤ ὢ ᾦ ῶ ῳ ῴ ῲ ὦ ᾠ ᾤ ᾢ（17个）→ō
        // 集合中字符转换为拉丁字母ō。
        static string[] lower_omega_without_dasia = new string[] {
            "\u03c9",   // (ω)  // issue 中缺这个
            "\u03ce",   // (ώ)
            "\u1f7d",   // (ώ)
            "\u1f7c",   // (ὼ)
            "\u1ff7",   // (ῷ)
            "\u1f60",   // (ὠ)
            "\u1f64",   // (ὤ)
            "\u1f62",   // (ὢ)
            "\u1fa6",   // (ᾦ)
            "\u1ff6",   // (ῶ)
            "\u1ff3",   // (ῳ)
            "\u1ff4",   // (ῴ)
            "\u1ff2",   // (ῲ)
            "\u1f66",   // (ὦ)
            "\u1fa0",   // (ᾠ)
            "\u1fa4",   // (ᾤ)
            "\u1fa2",   // (ᾢ)
            // 共 16 个        
        };

        /*
        // ③有气号（Dasia）以外其它变音符号（ώ ώ ὼ ῷ ὠ ὤ ὢ ᾦ ῶ ῳ ῴ ῲ ὦ ᾠ ᾤ ᾢ）不转换。
        static string[] lower_omega_diacritics_exclude_dasia = new string[] {
            "\u03ce",   // (ώ)
            "\u1f7d",   // (ώ)
            "\u1f7c",   // (ὼ)
            "\u1ff7",   // (ῷ)
            "\u1f60",   // (ὠ)
            "\u1f64",   // (ὤ)
            "\u1f62",   // (ὢ)
            "\u1fa6",   // (ᾦ)
            "\u1ff6",   // (ῶ)
            "\u1ff3",   // (ῳ)
            "\u1ff4",   // (ῴ)
            "\u1ff2",   // (ῲ)
            "\u1f66",   // (ὦ)
            "\u1fa0",   // (ᾠ)
            "\u1fa4",   // (ᾤ)
            "\u1fa2",   // (ᾢ)
        };
        */
#endregion

#region 字母 Eta

        // ①希腊字母集合（upper_eta_with_dasia）：Ἣ Ἡ Ἥ ᾟ Ἧ ᾙ ᾝ ᾛ（8个）→HĒ
        // 集合中字符转换为拉丁字母HĒ。
        static string[] upper_eta_with_dasia = new string[] {
            "\u1f2b",   // (Ἣ)
            "\u1f29",   // (Ἡ)
            "\u1f2d",   // (Ἥ)
            "\u1f9f",   // (ᾟ)
            "\u1f2f",   // (Ἧ)
            "\u1f99",   // (ᾙ)
            "\u1f9d",   // (ᾝ)
            "\u1f9b",   // (ᾛ)
            // 共 8 个
        };

        // ②希腊字母集合（upper_eta_without_dasia）：Η Ή Ή Ὴ Ἠ Ἤ Ἢ ᾞ ῌ Ἦ ᾘ ᾜ ᾚ（13个）→Ē
        // 集合中字符转换为拉丁字母Ē。
        static string[] upper_eta_without_dasia = new string[] {
            "\u0397",   // (Η)
            "\u0389",   // (Ή)
            "\u1fcb",   // (Ή)
            "\u1fca",   // (Ὴ)
            "\u1f28",   // (Ἠ)
            "\u1f2c",   // (Ἤ)
            "\u1f2a",   // (Ἢ)
            "\u1f9e",   // (ᾞ)
            "\u1fcc",   // (ῌ)
            "\u1f2e",   // (Ἦ)
            "\u1f98",   // (ᾘ)
            "\u1f9c",   // (ᾜ)
            "\u1f9a",   // (ᾚ)
            // 共 13 个        
        };

        // 带变音符号的大写 Eta
        static bool IsUpperEtaWithDasia(string char_string)
        {
            return upper_eta_with_dasia.Where(x => char_string == x).Any();
        }

        // ③有气号（Dasia）以外其它变音符号（Ή Ή Ὴ Ἠ Ἤ Ἢ ᾞ ῌ Ἦ ᾘ ᾜ ᾚ）不转换
        static string[] upper_eta_diacritics_exclude_dasia = new string[] {
            "\u0389",   // (Ή)
            "\u1fcb",   // (Ή)
            "\u1fca",   // (Ὴ)
            "\u1f28",   // (Ἠ)
            "\u1f2c",   // (Ἤ)
            "\u1f2a",   // (Ἢ)
            "\u1f9e",   // (ᾞ)
            "\u1fcc",   // (ῌ)
            "\u1f2e",   // (Ἦ)
            "\u1f98",   // (ᾘ)
            "\u1f9c",   // (ᾜ)
            "\u1f9a",   // (ᾚ)
        };

        // ③希腊字母集合（lower_eta_with_dasia）：ἡ ἥ ἣ ᾗ ἧ ᾑ ᾕ ᾓ（8个）→hē
        // 集合中字符转换为拉丁字母hē。
        static string[] lower_eta_with_dasia = new string[] {
            "\u1f21",   // (ἡ)
            "\u1f25",   // (ἥ)
            "\u1f23",   // (ἣ)
            "\u1f97",   // (ᾗ)
            "\u1f27",   // (ἧ)
            "\u1f91",   // (ᾑ)
            "\u1f95",   // (ᾕ)
            "\u1f93",   // (ᾓ)
            // 共 8 个
        };

        // ④希腊字母集合（lower_eta_without_dasia）：η ή ή ὴ ῇ ἠ ἤ ἢ ᾖ ῆ ῃ ῄ ῂ ἦ ᾐ ᾔ ᾒ（17个）→ē
        // 集合中任意1个字符转换为拉丁字母ē。
        static string[] lower_eta_without_dasia = new string[] {
            "\u03b7",   // (η)
            "\u03ae",   // (ή)
            "\u1f75",   // (ή)
            "\u1f74",   // (ὴ)
            "\u1fc7",   // (ῇ)
            "\u1f20",   // (ἠ)
            "\u1f24",   // (ἤ)
            "\u1f22",   // (ἢ)
            "\u1f96",   // (ᾖ)
            "\u1fc6",   // (ῆ)
            "\u1fc3",   // (ῃ)
            "\u1fc4",   // (ῄ)
            "\u1fc2",   // (ῂ)
            "\u1f26",   // (ἦ)
            "\u1f90",   // (ᾐ)
            "\u1f94",   // (ᾔ)
            "\u1f92",   // (ᾒ)
            // 共 17 个        
        };

        // 带变音符号的小写 Eta
        static bool IsLowerEtaWithDasia(string char_string)
        {
            return lower_eta_with_dasia.Where(x => char_string == x).Any();
        }

        // ③有气号（Dasia）以外其它变音符号（ή ή ὴ ῇ ἠ ἤ ἢ ᾖ ῆ ῃ ῄ ῂ ἦ ᾐ ᾔ ᾒ）不转换。
        static string[] lower_eta_diacritics_exclude_dasia = new string[] {
            "\u03ae",   // (ή)
            "\u1f75",   // (ή)
            "\u1f74",   // (ὴ)
            "\u1fc7",   // (ῇ)
            "\u1f20",   // (ἠ)
            "\u1f24",   // (ἤ)
            "\u1f22",   // (ἢ)
            "\u1f96",   // (ᾖ)
            "\u1fc6",   // (ῆ)
            "\u1fc3",   // (ῃ)
            "\u1fc4",   // (ῄ)
            "\u1fc2",   // (ῂ)
            "\u1f26",   // (ἦ)
            "\u1f90",   // (ᾐ)
            "\u1f94",   // (ᾔ)
            "\u1f92",   // (ᾒ)
        };

#endregion

#region 字母 Epsilon

        // ①希腊字母集合（upper_epsilon_with_dasia）：Ἑ Ἕ Ἓ（3个）→HE
        // 本集合中字符罗马化为拉丁字母HE。
        static string[] upper_epsilon_with_dasia = new string[] {
            "\u1f19",   // (Ἑ)
            "\u1f1d",   // (Ἕ)
            "\u1f1b",   // (Ἓ)
            // 共 3 个
        };

        // ②希腊字母集合2（upper_epsilon_without_dasia）：Ε Έ Έ Ὲ Ἐ Ἔ Ἒ（7个）→E
        // 本集合中字符罗马化为拉丁字母E。
        static string[] upper_epsilon_without_dasia = new string[] {
            "\u0395",   // (Ε)
            "\u0388",   // (Έ)
            "\u1fc9",   // (Έ)
            "\u1fc8",   // (Ὲ)
            "\u1f18",   // (Ἐ)
            "\u1f1c",   // (Ἔ)
            "\u1f1a",   // (Ἒ)
            // 共 7 个        
        };

        // ③希腊字母集合（lower_epsilon_with_dasia）：ἑ ἕ ἓ（3个）→he
        // 本集合中字符罗马化为拉丁字母he。
        static string[] lower_epsilon_with_dasia = new string[] {
            "\u1f11",   // (ἑ)
            "\u1f15",   // (ἕ)
            "\u1f13",   // (ἓ)
            // 共 3 个        
        };

        // ④希腊字母集合（lower_epsilon_without_dasia）：έ έ ὲ ἐ ἔ ἒ（6个）→e
        // 本集合中字符罗马化为拉丁字母e。
        static string[] lower_epsilon_without_dasia = new string[] {
            "\u03b5",   // (ε)  // ? 2022/10/20 添加
            "\u03ad",   // (έ)
            "\u1f73",   // (έ)
            "\u1f72",   // (ὲ)
            "\u1f10",   // (ἐ)
            "\u1f14",   // (ἔ)
            "\u1f12",   // (ἒ)
            // 共 6(7?) 个        
        };

        // ③有气号（Dasia）以外其它变音符号（Έ Έ Ὲ Ἐ Ἔ Ἒ）不转换；
        static string[] upper_epsilon_diacritics_exclude_dasia = new string[] {
            "\u0388",   // (Έ)
            "\u1fc9",   // (Έ)
            "\u1fc8",   // (Ὲ)
            "\u1f18",   // (Ἐ)
            "\u1f1c",   // (Ἔ)
            "\u1f1a",   // (Ἒ)
        };

        // ③有气号（Dasia）以外其它变音符号（έ έ ὲ ἐ ἔ ἒ）不转换；
        static string[] lower_epsilon_diacritics_exclude_dasia = new string[] {
            "\u03ad",   // (έ)
            "\u1f73",   // (έ)
            "\u1f72",   // (ὲ)
            "\u1f10",   // (ἐ)
            "\u1f14",   // (ἔ)
            "\u1f12",   // (ἒ)
        };

#endregion

#region 字母 Iota

        // ⑤希腊字母集合5（upper_iota_with dasia）：Ἱ Ἵ Ἳ Ἷ（4个）
        static string[] upper_iota_with_dasia = new string[] {
            "\u1f39",   // (Ἱ)
            "\u1f3d",   // (Ἵ)
            "\u1f3b",   // (Ἳ)
            "\u1f3f",   // (Ἷ)
            // 共 4 个
        };

        // ⑥希腊字母集合6（upper_Ι_without dasia）：Ι Ί Ί Ὶ Ϊ Ῐ Ῑ Ἰ Ἲ Ἴ Ἶ（11个）
        static string[] upper_iota_without_dasia = new string[] {
            "\u0399",   // (Ι)
            "\u038a",   // (Ί)
            "\u1fdb",   // (Ί)
            "\u1fda",   // (Ὶ)
            "\u03aa",   // (Ϊ)
            "\u1fd8",   // (Ῐ)
            "\u1fd9",   // (Ῑ)
            "\u1f38",   // (Ἰ)
            "\u1f3a",   // (Ἲ)
            "\u1f3c",   // (Ἴ)
            "\u1f3e",   // (Ἶ)
            // 共 11 个
        };

        // ③有气号以外的其它变音符号（Ί Ί Ὶ Ϊ Ῐ Ῑ Ἰ Ἲ Ἴ）不转换；
        static string[] upper_iota_diacritics_exclude_dasia = new string[] {
            "\u038a",   // (Ί)
            "\u1fdb",   // (Ί)
            "\u1fda",   // (Ὶ)
            "\u03aa",   // (Ϊ)
            "\u1fd8",   // (Ῐ)
            "\u1fd9",   // (Ῑ)
            "\u1f38",   // (Ἰ)
            "\u1f3a",   // (Ἲ)
            "\u1f3c",   // (Ἴ)
        };

        // ⑦希腊字母集合7：ἱ ἵ ἳ ἷ（4个）
        static string[] lower_iota_with_dasia = new string[] {
            "\u1f31",   // (ἱ)
            "\u1f35",   // (ἵ)
            "\u1f33",   // (ἳ)
            "\u1f37",   // (ἷ)
        };

        // ⑧希腊字母集合8：ι ί ί ὶ ϊ ῐ ῑ ΐ ΐ ῒ ἰ ἴ ἲ ῖ ῗ ἶ（16个）
        static string[] lower_iota_without_dasia = new string[] {
            "\u03b9",   // (ι)
            "\u03af",   // (ί)
            "\u1f77",   // (ί)
            "\u1f76",   // (ὶ)
            "\u03ca",   // (ϊ)
            "\u1fd0",   // (ῐ)
            "\u1fd1",   // (ῑ)
            "\u0390",   // (ΐ)
            "\u1fd3",   // (ΐ)
            "\u1fd2",   // (ῒ)
            "\u1f30",   // (ἰ)
            "\u1f34",   // (ἴ)
            "\u1f32",   // (ἲ)
            "\u1fd6",   // (ῖ)
            "\u1fd7",   // (ῗ)
            "\u1f36",   // (ἶ)
            // 共 16 个
        };

        // ③有气号以外的其它变音符号（ί ί ὶ ϊ ῐ ῑ ΐ ΐ ῒ ἰ ἴ ἲ ῖ ῗ ἶ）不转换；
        static string[] lower_iota_diacritics_exclude_dasia = new string[] {
            "\u03af",   // (ί)
            "\u1f77",   // (ί)
            "\u1f76",   // (ὶ)
            "\u03ca",   // (ϊ)
            "\u1fd0",   // (ῐ)
            "\u1fd1",   // (ῑ)
            "\u0390",   // (ΐ)
            "\u1fd3",   // (ΐ)
            "\u1fd2",   // (ῒ)
            "\u1f30",   // (ἰ)
            "\u1f34",   // (ἴ)
            "\u1f32",   // (ἲ)
            "\u1fd6",   // (ῖ)
            "\u1fd7",   // (ῗ)
            "\u1f36",   // (ἶ)
        };

#endregion

#region 字母 Upsilon

        // ⑨希腊字母集合（upper_upsilon_with_dasia）：Ὑ Ὕ Ὓ Ὗ（4个）
        static string[] upper_upsilon_with_dasia = new string[] {
            "\u1f59",   // (Ὑ)
            "\u1f5d",   // (Ὕ)
            "\u1f5b",   // (Ὓ)
            "\u1f5f",   // (Ὗ)
            // 共 4 个        
        };

        // ⑩希腊字母集合（upper_upsilon_without_dasia）：Υ Ύ Ύ Ὺ Ϋ Ῠ Ῡ ϒ ϓ ϔ（10个）
        static string[] upper_upsilon_without_dasia = new string[] {
            "\u03a5",   // (Υ)
            "\u038e",   // (Ύ)
            "\u1feb",   // (Ύ)
            "\u1fea",   // (Ὺ)
            "\u03ab",   // (Ϋ)
            "\u1fe8",   // (Ῠ)
            "\u1fe9",   // (Ῡ)
            "\u03d2",   // (ϒ)
            "\u03d3",   // (ϓ)
            "\u03d4",   // (ϔ)
            // 共 10 个        
        };

        // ③有气号（Dasia）以外其它变音符号（Ύ Ύ Ὺ Ϋ Ῠ Ῡ ϒ ϓ ϔ）不转换；
        static string[] upper_upsilon_diacritics_exclude_dasia = new string[] {
            "\u038e",   // (Ύ)
            "\u1feb",   // (Ύ)
            "\u1fea",   // (Ὺ)
            "\u03ab",   // (Ϋ)
            "\u1fe8",   // (Ῠ)
            "\u1fe9",   // (Ῡ)
            "\u03d2",   // (ϒ)
            "\u03d3",   // (ϓ)
            "\u03d4",   // (ϔ)
        };

        // ⑪希腊字母集合（lower_upsilon_with_dasia）：ὑ ὕ ὓ ὗ（4个）
        static string[] lower_upsilon_with_dasia = new string[] {
            "\u1f51",   // (ὑ)
            "\u1f55",   // (ὕ)
            "\u1f53",   // (ὓ)
            "\u1f57",   // (ὗ)
            // 共 4 个        
        };

        // ⑫希腊字母集合（lower_upsilon_without_dasia）：υ ύ ύ ὺ ϋ ῠ ῡ ΰ ΰ ῢ ὐ ὔ ὒ ῦ ῧ ὖ（16个）
        static string[] lower_upsilon_without_dasia = new string[] {
            "\u03c5",   // (υ)
            "\u03cd",   // (ύ)
            "\u1f7b",   // (ύ)
            "\u1f7a",   // (ὺ)
            "\u03cb",   // (ϋ)
            "\u1fe0",   // (ῠ)
            "\u1fe1",   // (ῡ)
            "\u03b0",   // (ΰ)
            "\u1fe3",   // (ΰ)
            "\u1fe2",   // (ῢ)
            "\u1f50",   // (ὐ)
            "\u1f54",   // (ὔ)
            "\u1f52",   // (ὒ)
            "\u1fe6",   // (ῦ)
            "\u1fe7",   // (ῧ)
            "\u1f56",   // (ὖ)
            // 共 16 个        
        };


        // ③有气号（Dasia）以外其它变音符号（ύ ύ ὺ ϋ ῠ ῡ ΰ ΰ ῢ ὐ ὔ ὒ ῦ ῧ ὖ）不转换；
        static string[] lower_upsilon_diacritics_exclude_dasia = new string[] {
            "\u03cd",   // (ύ)
            "\u1f7b",   // (ύ)
            "\u1f7a",   // (ὺ)
            "\u03cb",   // (ϋ)
            "\u1fe0",   // (ῠ)
            "\u1fe1",   // (ῡ)
            "\u03b0",   // (ΰ)
            "\u1fe3",   // (ΰ)
            "\u1fe2",   // (ῢ)
            "\u1f50",   // (ὐ)
            "\u1f54",   // (ὔ)
            "\u1f52",   // (ὒ)
            "\u1fe6",   // (ῦ)
            "\u1fe7",   // (ῧ)
            "\u1f56",   // (ὖ)
        };

#endregion

#endregion  // of 字母集合

#region utility functions

        // 判断第二个字符是否为大写
        static bool IsSecondCharUpperCase(string text, bool default_value)
        {
            if (string.IsNullOrEmpty(text))
                return default_value;
            if (text.Length < 2)
                return default_value;
            return char.IsUpper(text[1]);

        }

        // 调整第二个字符的大小写
        static string AdjustUpperLower(string text, bool upper_case)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            if (text.Length != 2)
            {
                throw new ArgumentException($"{nameof(text)} 参数值 '{text}' 字符数不正确，应该为 2 字符");
            }

            if (char.IsUpper(text[1]) == upper_case)
                return text;
            if (upper_case)
                return text.Substring(0, 1) + char.ToUpper(text[1]);
            return text.Substring(0, 1) + char.ToLower(text[1]);
        }

        public static void ParseTwoPart(string strText,
string strSep,
out string strPart1,
out string strPart2)
        {
            strPart1 = "";
            strPart2 = "";

            if (string.IsNullOrEmpty(strText) == true)
                return;

            int nRet = strText.IndexOf(strSep);
            if (nRet == -1)
            {
                strPart1 = strText;
                return;
            }

            strPart1 = strText.Substring(0, nRet).Trim();
            strPart2 = strText.Substring(nRet + strSep.Length).Trim();
        }

#endregion
    }

    [Flags]
    public enum LookupStyle
    {
        none = 0,
        ancient = 0x1,  // 古代
        modern = 0x2,   // 现代
        head = 0x04,    // 词首
        middle = 0x08,  // 词中
        tail = 0x10,    // 词尾
    }

    public class TransliterException : Exception
    {
        public string? Word { get; set; }

        public TransliterException(string message) : base(message)
        {
        }

        public TransliterException(string message, string word) : base(message)
        {
            Word = word;
        }
    }
}
