using FATWA_DOMAIN.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Xml.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using MsgReader.Outlook;

namespace FATWA_GENERAL.Helper
{
    //<History Author = 'Hassan Abbas' Date='2024-04-24' Version="1.0" Branch="master"> General Extension for Encrypting/Decryption Docuemnts in the project</History>
    public static class DocumentEncryptionService
    {
        //<History Author = 'Hassan Abbas' Date='2024-04-24' Version="1.0" Branch="master"> Return Byte Array Response</History>
        public static async Task<byte[]> GetDecryptedDocumentBytes(string storagePath, string docType, string encryptionKey, bool skipImageConversion = false)
        {
            byte[] FileData;
            if (!String.IsNullOrEmpty(storagePath))
            {
#if DEBUG
                FileStream fsCrypt;
                if (System.IO.File.Exists(storagePath))
                {
                    fsCrypt = new FileStream(storagePath, FileMode.Open);
                }
                else
                {
                    return new byte[0];
                }
#else
                Stream fsCrypt;
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(storagePath);
                if (response.IsSuccessStatusCode)
                {
                    fsCrypt = await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    return new byte[0];
                }
#endif
                //Encryption/Descyption Key
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encryptionKey);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                MemoryStream fsOut = new MemoryStream();
                int data;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (new List<string> { ".jpg", ".png", ".jpeg" }.Contains(docType?.ToLower()))
                {
                    if(skipImageConversion)
                    {
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        FileData = fsOut.ToArray();
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
                        return FileData;
                    }
                    else
                    {
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        MemoryStream stream = new MemoryStream();
                        using (var document = new PdfDocument())
                        {
                            PdfPage page = document.AddPage();
                            using (XImage img = XImage.FromStream(fsOut))
                            {
                                int width = 600;
                                // Calculate new height to keep image ratio
                                var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                                // Change PDF Page size to match image
                                page.Width = width;
                                page.Height = height;

                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                gfx.DrawImage(img, 0, 0, width, height);
                            }
                            document.Save(stream, false);
                            document.Close();
                            stream.Close();
                            fsOut.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
                        FileData = stream.ToArray();
                        string base64String = Convert.ToBase64String(FileData);
                        return FileData;
                    }
                }
                else if (docType?.ToLower() == ".pdf")
                {
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    FileData = fsOut.ToArray();

                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();
                    return FileData;
                }
                else if (docType?.ToLower() == ".msg")
                {
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    var message = new Storage.Message(fsOut);
                    // Create a new PDF document
                    MemoryStream stream = new MemoryStream();
                    PdfDocument document = new PdfDocument();

                    // Create a new page
                    PdfPage page = document.AddPage();

                    // Create a new XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Set the font and text position
                    XFont font = new XFont("Arial", 12);
                    XTextFormatter textFormatter = new XTextFormatter(gfx);

                    // Extract the content from the .msg file
                    string content = $"From: {message.Sender.DisplayName} <{message.Sender.Email}>";
                    content += "\nTo: ";
                    foreach (var to in message.Headers.To)
                    {
                        content += $"{to.DisplayName} <{to.MailAddress}> ";
                    }
                    content += "\nCC: ";
                    foreach (var cc in message.Headers.Cc)
                    {
                        content += $"{cc.DisplayName} <{cc.MailAddress}> ";
                    }
                    content += $"\nDate: {message.ReceivedOn}";
                    content += $"\nSubject: {message.Subject}\n\n";
                    content += message.BodyText;

                    // Draw the text on the page
                    textFormatter.DrawString(content, font, XBrushes.Black,
                        new XRect(10, 10, page.Width - 20, page.Height - 20));

                    // Save the PDF document
                    document.Save(stream, false);
                    document.Close();
                    stream.Close();
                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();

                    FileData = stream.ToArray();
                    return FileData;
                }
                else
                {
                    return new byte[0];
                }
            }
            else
            {
                return new byte[0];
            }
        }
        //<History Author = 'Hassan Abbas' Date='2024-04-24' Version="1.0" Branch="master"> Return Base64 Response</History>
        public static async Task<string> GetDecryptedDocumentBase64(string storagePath, string docType, string encryptionKey, bool skipImageConversion = false)
        {
            byte[] FileData;
            string base64String;
            if (!String.IsNullOrEmpty(storagePath))
            {
#if DEBUG
                FileStream fsCrypt;
                if (System.IO.File.Exists(storagePath))
                {
                    fsCrypt = new FileStream(storagePath, FileMode.Open);
                }
                else
                {
                    return string.Empty;
                }
#else
                Stream fsCrypt;
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(storagePath);
                if (response.IsSuccessStatusCode)
                {
                    fsCrypt = await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    return string.Empty;
                }
#endif
                //Encryption/Descyption Key
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encryptionKey);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                MemoryStream fsOut = new MemoryStream();
                int data;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (new List<string> { ".jpg", ".png", ".jpeg" }.Contains(docType?.ToLower()))
                {
                    if (skipImageConversion)
                    {
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        FileData = fsOut.ToArray();
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
                        return Convert.ToBase64String(FileData);
                    }
                    else
                    {
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        MemoryStream stream = new MemoryStream();
                        using (var document = new PdfDocument())
                        {
                            PdfPage page = document.AddPage();
                            using (XImage img = XImage.FromStream(fsOut))
                            {
                                int width = 600;
                                // Calculate new height to keep image ratio
                                var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                                // Change PDF Page size to match image
                                page.Width = width;
                                page.Height = height;

                                XGraphics gfx = XGraphics.FromPdfPage(page);
                                gfx.DrawImage(img, 0, 0, width, height);
                            }
                            document.Save(stream, false);
                            document.Close();
                            stream.Close();
                            fsOut.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
                        FileData = stream.ToArray();
                        return Convert.ToBase64String(FileData);
                    }
                }
                else if (docType?.ToLower() == ".pdf")
                {
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    FileData = fsOut.ToArray();

                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();
                    return Convert.ToBase64String(FileData);
                }
                else if (docType?.ToLower() == ".msg")
                {
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    var message = new Storage.Message(fsOut);
                    // Create a new PDF document
                    MemoryStream stream = new MemoryStream();
                    PdfDocument document = new PdfDocument();

                    // Create a new page
                    PdfPage page = document.AddPage();

                    // Create a new XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Set the font and text position
                    XFont font = new XFont("Arial", 12);
                    XTextFormatter textFormatter = new XTextFormatter(gfx);

                    // Extract the content from the .msg file
                    string content = $"From: {message.Sender.DisplayName} <{message.Sender.Email}>";
                    content += "\nTo: ";
                    foreach (var to in message.Headers.To)
                    {
                        content += $"{to.DisplayName} <{to.MailAddress}> ";
                    }
                    content += "\nCC: ";
                    foreach (var cc in message.Headers.Cc)
                    {
                        content += $"{cc.DisplayName} <{cc.MailAddress}> ";
                    }
                    content += $"\nDate: {message.ReceivedOn}";
                    content += $"\nSubject: {message.Subject}\n\n";
                    content += message.BodyText;

                    // Draw the text on the page
                    textFormatter.DrawString(content, font, XBrushes.Black,
                        new XRect(10, 10, page.Width - 20, page.Height - 20));

                    // Save the PDF document
                    document.Save(stream, false);
                    document.Close();
                    stream.Close();
                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();

                    FileData = stream.ToArray();
                    return Convert.ToBase64String(FileData);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
