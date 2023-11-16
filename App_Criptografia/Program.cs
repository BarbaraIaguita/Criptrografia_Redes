using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string chave = "MinhaChaveSecreta";
        string id = "IDSecreto";

        Console.WriteLine("Digite a frase que deseja criptografar:");
        string input = Console.ReadLine();

        string fraseCriptografada = CriptografarAES(input, chave,id);
        Console.WriteLine($"Frase criptografada: {fraseCriptografada}");

        string fraseDescriptografada = DescriptografarAES(fraseCriptografada, chave, id);
        Console.WriteLine($"Frase descriptografada: {fraseDescriptografada}");
    }

    static string CriptografarAES(string texto, string chave, string id)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(chave);
            aesAlg.IV = Encoding.UTF8.GetBytes(id);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(texto);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }


    static string DescriptografarAES(string textoCriptografado, string chave, string id)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(chave);
            aesAlg.IV = Encoding.UTF8.GetBytes(id);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(textoCriptografado)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

     }

}

