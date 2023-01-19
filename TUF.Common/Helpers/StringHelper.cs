using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Knus.Common.Helpers;

#nullable disable
#pragma warning disable CS8632, IDE0060
public static class StringHelper
{

    /// <summary>
    /// 정수변환
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt(this string value)
    {
        value = value.Replace(".00", "");
        if (string.IsNullOrEmpty(value)) { return 0; }
        int iout = 0;
        if (int.TryParse(value, out iout))
            return iout;
        return iout;
    }

    public static double ToDouble(this string value)
    {
        //value = value.Replace(".00", "");
        if (string.IsNullOrEmpty(value)) { return 0; }
        double iout = 0;
        if (double.TryParse(value, out iout))
            return iout;
        return iout;
    }


    public static string TryTrim(this string  value,bool nonspace)
    {
        if (value == null)
            return "";
        if (nonspace)
            return value.Replace(" ", "").Trim();
        else
            return value;
    }
    public static string TextOnly(this string  value)
    {
        if (value.IsNullOrEmpty())
            return "";
        var res= Regex.Replace(value, @"[^a-zA-Z0-9가-힣]", "");
        if (res.IsNullOrEmpty())
            res = value.Substring(0, 1);
        return res;
    }

    

    public static int ToIntComma(this string value)
    {
        var arv = value.Split('.');
        if (arv.Length > 1)
            value = arv[0];

        if (string.IsNullOrEmpty(value)) { return 0; }
        int iout = 0;
        if (int.TryParse(value, out iout))
            return iout;
        return iout;
    }


    public static string GetKoreaDate(this DateTime value)
    {
        CultureInfo cultures = CultureInfo.CreateSpecificCulture("ko-KR");

        return value.ToString(string.Format("yyyy년 MM월dd일 HH시mm분 ddd요일", cultures));

    }


    /// <summary>
    /// 콤마찍기
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToComma(this double value)
    {
        return String.Format("{0:##,##}", value);

    }

    public static string ToComma(this int value)
    {
        return String.Format("{0:##,##}", value);

    }



    /// <summary>
    /// 숫자만 뽑기
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToNumber(this string value)
    {
        if (string.IsNullOrEmpty(value)) { return 0; }
        var str = Regex.Replace(value, @"\D", "");
        int iout = 0;
        if (int.TryParse(str, out iout))
            return iout;
        return iout;
    }




    /// <summary>
    /// 정수인지 체크
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumber(this string value)
    {
        if (string.IsNullOrEmpty(value)) { return false; }
        var pattern = "^[0-9]*$";
        return new Regex(pattern).IsMatch(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] SplitStart(this string value, char separator)
    {
        string[] fields = new string[2];
        int fi = 0;
        foreach (char c in value)
        {
            if (c.Equals(separator) && fi <= 0) { fi++; continue; }
            fields[fi] += c;
        }
        return fields;
    }

    public static string TryTrim(this string value)
    {
        if (value == null)
            return "";
        else            
            return value.Trim();             
    }

    /// <summary>
    /// Null여부체크
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {

        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        var szTemp = value.Trim();
        if (string.IsNullOrEmpty(szTemp))
        {
            return true;
        }

        szTemp = value.Replace(" ", "");
        if (string.IsNullOrEmpty(szTemp))
        {
            return true;
        }

        szTemp = value.Replace("\n", "").Replace("\r", "");
        if (string.IsNullOrEmpty(szTemp))
        {
            return true;
        }

        return false;
    }


    public static int CalcByteCount(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        int byteCount = 0;
        foreach (char c in input.ToCharArray())
        {
            byteCount = CalcByteCount(c, byteCount);
        }
        return byteCount;
    }

    const int maxAnsiCode = 255;
    public static int CalcByteCount(this char c, int byteCount = 0)
    {
        if (c > maxAnsiCode)
            byteCount += 2;
        else byteCount += 1;
        return byteCount;
    }

    public static int CalcByteForCount(this string text, int max)
    {
        var count = 0;
        var nb = 0;
        var temp = 0;
        var cs = text.ToCharArray();
        for (int i = 0; i < cs.Length; i++)
        {
            temp = cs[i].CalcByteCount(nb);
            if (temp > max) { break; }

            nb = temp;
            count = i;
        }
        return count + 1;
    }

    public static string SubstringExtension(this string val, int subcnt)
    {
        string strrt = string.Empty;

        if (val.Length > subcnt)
            strrt = val.Substring(0, subcnt) + "..";
        else
            strrt = val;
        return strrt;
    }


    public static string Serialize<T>(this T value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        try
        {
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();

            XmlWriterSettings settings = new XmlWriterSettings();
            // settings.NewLineHandling = NewLineHandling.Entitize;

            settings.Encoding = Encoding.UTF8;
            settings.Encoding = Encoding.GetEncoding("utf-8");
            settings.Indent = true;

            using (var writer = XmlWriter.Create(stringWriter, settings))
            {
                xmlserializer.Serialize(writer, value);

                return stringWriter.ToString().Replace("utf-16", "utf-8").Replace("&lt;", "<").Replace("&gt;", ">");

            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred", ex);
        }
    }


    public static bool TryConvertDate(this string source, out DateTime dt)
    {
        if (DateTime.TryParse(source, out dt))
        {
            return true;
        }
        else
        {
            var dc = $"{source.Substring(0, 4)}-{source.Substring(4, 2)}-{source.Substring(6, 2)}";
            if (source.Length == 12)
                dc += $" {source.Substring(8, 2)}:{source.Substring(10, 2)}";
            if (DateTime.TryParse(dc, out dt))
                return true;
            else
                return false;
        }
    }

    public static string GenerateCdata(this string content)
    {
        string cdata = "<![CDATA[{MENT}]]>";
        return cdata.Replace("{MENT}", content);//.Replace("<","").Replace(">", ""));
    }

    public static string Base64Encode(this string data)
    {
        try
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception e)
        {
            throw new Exception("Error in Base64Encode: " + e.Message);
        }
    }

    public static string Base64Decode(this string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error in Base64Decode: " + e.Message);
        }
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));
            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();
                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);
                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException )
        {
            return false;
        }
        catch (ArgumentException )
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }


    public static string RemoveTag(this string input)
    {
        return Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
    }

    public static string GetBlind(this string s, int st, int ed)
    {
        char[] charArr = s.ToCharArray();
        string name = "";
        int i = 0;
        foreach (char c in charArr)
        {
            if (i >= st && i <= ed)
            {
                name += "*";
            }
            else { name += c; }
            i++;
        }
        return name;
    }
    /// <summary>
    /// 정수 여부 검사
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static bool IntValid(this string val)
    {
        int rt = 0;
        if (int.TryParse(val, out rt))
            return true;
        else
            return false;
    }

    public static bool Ipcheck(this string ip)
    {
        IPAddress sip;
        var ipaddress = ip;
        bool ValidateIP = IPAddress.TryParse(ipaddress, out sip);
        if (!ValidateIP)
            return false;
        else
            return true;
    }

    public static string IPGenerate(this string sourceip)
    {
        var arip = sourceip.Split('.');
        string ipaddr = "";
        for (int i = 0; i < arip.Length; i++)
        {
            var cp1 = 3 - arip[i].Length;
            for (int c = 0; c < cp1; c++)
            {
                ipaddr += "_";
            }
            ipaddr += arip[i] + ".";
        }
        ipaddr = ipaddr.TrimEnd('.');
        return ipaddr;
    }

    public static string ConvertDate(this string source,string format)
    {
        DateTime dt;
        if (!DateTime.TryParse(source, out dt))
        {
            return source;
        }
        else
        {
            return dt.ToString(format);
        }
    }

    public static string UrlEncode(this string input)
    {
        if (input == null)
        {
            return "";
        }
        return Uri.EscapeDataString(input);
    }

    public static string UrlDecode(this string input)
    {
        if (input == null)
        {
            return "";
        }
        input = input.Replace("+", " ");
        return Uri.UnescapeDataString(input);
    }

}
