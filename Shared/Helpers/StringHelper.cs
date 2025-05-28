using System.IO.Compression;
using System.Text;

namespace Virtubank_Qesh_API.Commons.Shared.Helpers
{
    public class StringHelper
    {
        public static string RemoveSpecialCaracters(string str)
        {
            string[] withAccent = ["ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û"];
            string[] withoutAccent = ["c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U"];


            for (int i = 0; i < withAccent.Length; i++)
            {
                str = str.Replace(withAccent[i], withoutAccent[i]);
            }

            string[] specialCaracters = { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };

            for (int i = 0; i < specialCaracters.Length; i++)
            {
                str = str.Replace(specialCaracters[i], "");
            }

            str = str.Replace("^\\s+", "");
            str = str.Replace("\\s+$", "");
            str = str.Replace("\\s+", " ");
            return str;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {

            base64EncodedData = base64EncodedData.Replace(" ", "+");
            int mod4 = base64EncodedData.Length % 4;
            if (mod4 > 0)
            {
                base64EncodedData += new string('=', 4 - mod4);
            }

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string FormatPhone(string phone)
        {
            string number = new(phone.Where(char.IsDigit).ToArray());

            if (number.Length == 11)
            {
                return string.Format("({0}) {1}-{2}",
                    number[..2],
                    number.Substring(2, 5),
                    number[7..]);
            }
            else if (number.Length == 10)
            {
                return string.Format("({0}) {1}-{2}",
                    number[..2],
                    number.Substring(2, 4),
                    number[6..]);
            }
            else
            {
                // invalid number
                return phone;
            }
        }

        public static string PasswordGenerator()
        {
            string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz023456789!@$^_-=";
            string pass = "";
            Random random = new();
            for (int f = 0; f < 12; f++)
            {
                pass = string.Concat(pass, chars.AsSpan(random.Next(0, chars.Length - 1), 1));
            }
            return pass;
        }

        public static string Compacta(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new();
            using (GZipStream zip = new(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            using MemoryStream outStream = new();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Descompacta(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using MemoryStream ms = new();

            int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

            byte[] buffer = new byte[msgLength];

            ms.Position = 0;
            using (GZipStream zip = new(ms, CompressionMode.Decompress))
            {
                zip.Read(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

        public static string CapitalizeFullName(string fullName)
        {
            if (fullName == null)
                return null;

            if (fullName == string.Empty)
                return string.Empty;

            var parts = fullName.Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1).ToLower();
            }
            return string.Join(' ', parts);
        }
    }
}
