/*
 * TEMAT PROJEKTU: STEGANOGRAFIA
 * OPIS: PROGRAM MA ZA ZADANIE ZAKODOWANIE W BITACH WGRYWANEGO OBRAZU WIADOMOŚCI
 * TEKSTOWEJ. JEST TO MOŻLIWE POPRZEZ ZMIANĘ NAJMNIEJ ZNACZĄCEGO BITU.
 * KODOWANIE ODBYWA SIĘ ZA POMOCĄ FUNCJI NAPISANEJ W JĘZYKU WYSOKOPOZIOMOWYM
 * C#, BĄDZ PROCEDURY ASEMBLEROWEJ. DODATKOWO PROGRAM DEKODUJE ZASZYFROWANE WIADOMOŚCI
 * W ANALOGICZNY SPOSÓB DO WYBRANEGO ZAKODOWANIA.
 * DATA: 06/02/23
 * AUTOR: JOLANTA LACHMAN
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace SteganographyProject
{
    public partial class Form1 : Form
    {
        //WGRYWANY OBRAZ
        private Image img = null;
        //BITMAPA PRZECHOWYJĄCA WGRYWANY OBRAZ
        private Bitmap bmp = null;
        //TABLICA BAJTÓW PRZECHOWYJĄCA BAJTY WGRYWANEGO OBRAZU
        private byte[] rgbValues = null;
        //INDEKS ZAPISANEGO PLIKU
        private int fileIndex = 1;
      
        

        //KODOWANIE ZA POMOCĄ ASM
        [DllImport("SteganographyAsm.dll")]
        unsafe static extern void EncryptAsm(byte* bmpKey, byte* symbol);

        //DEKODOWANIE ZA POMOCĄ ASM
        [DllImport("SteganographyAsm.dll")]
        unsafe static extern char DecryptAsm(byte* bmpKey);

        //SPRAWDZENIE, CZY PROCESOR WSPIERA MMX
        [DllImport("SteganographyAsm.dll")]
        static extern bool checkMMXCapability();

        public Form1()
        {
            InitializeComponent();
        }
        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };


        //WGRANIE OBRAZU DO PROGRAMU I ZAPISANIE GO W ZMIENNEJ img
        private void btnClick_Click(object sender, EventArgs e)
        {
            if (File.Exists(sourcePicture.Text))
            {
                pictureBox1.Image = Image.FromFile(sourcePicture.Text);
                img = Image.FromFile(sourcePicture.Text);

                lblClick.Text = "The picture is now entered!";
            }
            else
            {
                lblClick.Text = "The file does not exist!";
            }
        }

        //OBSŁUGA PRZYCISKU KODOWANIA ZA POMOCĄ C#
        private void button1_Click(object sender, EventArgs e)
        {
            if(img!=null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img = encryptFunc((Bitmap)img, sourceMessage.Text);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = ts.TotalMilliseconds.ToString();
                label1.Text = "Encrypted in " + elapsedTime + " ms";                
                pictureBox1.Image = img;
                lblClick.Text = "Message successfully encrypted";
            }
            else
            {
                lblClick.Text = "The picture was not entered!";
            }

        }

        //OBSŁUGA PRZYCISKU DEKODOWANIA ZA POMOCĄ C#
        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                lblClick.Text = decryptFunc((Bitmap)img);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = ts.TotalMilliseconds.ToString();
                label1.Text = "Decrypted in " + elapsedTime + " ms";
            }
            else
            {
                lblClick.Text = "The picture was not entered!";
            }

        }

        //OBSŁUGA PRZYCISKU KODOWANIA ZA POMOCĄ ASM ORAZ ZCZYTANIE DANYCH Z img do bmp i rgbValues
        unsafe private void asmEncryptButton_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                if (checkMMXCapability())
                {
                    
                    bmp = (Bitmap)img;
                    Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    IntPtr ptr = bmpData.Scan0;
                    int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                    rgbValues = new byte[bytes];
                    Marshal.Copy(ptr, rgbValues, 0, bytes);
                    byte[] messageByte = Encoding.ASCII.GetBytes(sourceMessage.Text);
                    byte[] temp = new byte[messageByte.Length+1];
                    temp[0] = (byte)messageByte.Length;
                    messageByte.CopyTo(temp, 1);
                    messageByte = temp;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    fixed (byte* ptrImage = rgbValues)
                    fixed (byte* ptrMessage = messageByte)
                    {
                        for(int i=0;i<messageByte.Length;i++)
                        {
                            EncryptAsm(ptrImage+i*8, ptrMessage+i);
                        }                        
                    }
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    string elapsedTime = ts.TotalMilliseconds.ToString();
                    label1.Text = "Encrypted in " + elapsedTime + " ms";
                    Marshal.Copy(rgbValues, 0, ptr, bytes);                    
                    bmp.UnlockBits(bmpData);
                    pictureBox1.Image = bmp;
                    lblClick.Text = "Message successfully encrypted";

                }
                else
                {
                    lblClick.Text = "Sorry, the ASM encoding cannot be used on this processor";
                }
            }
            else
            {
                lblClick.Text = "The picture was not entered!";
            }          
            
        }

        //OBSŁUGA PRZYCISKU DEKODOWANIA ZA POMOCĄ ASM
        unsafe private void asmDecryptButton_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                if (checkMMXCapability())
                {
                    string output = "";
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    fixed (byte* ptrImage = rgbValues)
                    {
                        int size = DecryptAsm(ptrImage);
                        for (int i = 1; i <= size; i++)
                        {
                            output += DecryptAsm(ptrImage + i * 8);
                        }
                    }
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    string elapsedTime = ts.TotalMilliseconds.ToString();
                    label1.Text = "Decrypted in " + elapsedTime + " ms";
                    lblClick.Text = output;
                }
                else
                {
                    lblClick.Text = "Sorry, the ASM encoding cannot be used on this processor";
                }
            }
            else
            {
                lblClick.Text = "The picture was not entered!";
            }
        }

        
        //ZAPISANIE UZYSKANEGO OBRAZU W PLIKU
        private void button1_Click_1(object sender, EventArgs e)
        {

            img.Save("output"+fileIndex+".png");
            lblClick.Text = "The picture was saved";
            fileIndex++;
        }

        //FUNCKJA KODOWANIA ZA POMOCĄ C#
        private Image encryptFunc(Bitmap bmp, string text)
        {

            // USTAWIENIE STANU NA UKRYWANIE WIADOMOŚCI
            State state = State.Hiding;

            // INDEKS AKTUALNIE UKRYWANEJ ZMIENNEJ ZNAKOWEJ
            int charIndex = 0;

            // WARTOŚĆ ZMIENNEJ ZNAKOWEJ PRZEKONWERTOWANEJ NA INT
            int charValue = 0;

            // INDEKS AKTUALNEGO KOLORU SPOŚRÓD RGB
            long pixelElementIndex = 0;

            // LICZBA ZER DODANA PO ZAKODOWANIU WIADOMOŚCI
            int zeros = 0;

            // WARTOŚCI PIKSELA
            int R = 0, G = 0, B = 0;

            //PRZEJŚCIE PRZEZ KAŻDĄ KOLUMNĘ I RZĄD OBRAZU
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    // PIKSEL, KTÓRY ULEGA ZMIANIE
                    Color pixel = bmp.GetPixel(j, i);

                    // USUNIĘCIE NAJMNIEJ ZNACZĄCEGO BITU Z WARTOŚCI PIKSELA
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // PRZEJŚCIE PRZEZ KAŻDE Z RGB PIKSELA
                    for (int n = 0; n < 3; n++)
                    {
                        
                        if (pixelElementIndex % 8 == 0)
                        {
                            // SPRAWDZENIE, CZY 8 ZER ZOSTAŁO DODANE -> KODOWANIE SIĘ ZAKOŃCZYŁO
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return bmp;
                            }

                            // SPRAWDZENIE, CZY KAŻDA ZMIENNA ZNAKOWA WIADOMOŚCI ZOSTAŁA UKRYTA
                            if (charIndex >= text.Length)
                            {
                                //STAN ZMIENIONY NA DODAWANIE 8 ZER
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                //PRZEJŚCIE DO NASTĘPNEJ ZMIENNEJ
                                charValue = text[charIndex++];
                            }
                        }
                        //UKRYCIE ZMIENNEJ W ELEMENTACH PIKSELA
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        R += charValue % 2;
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }

                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.Filling_With_Zeros)
                        {
                            //INKREMENTACJA LICZBY UKRYTYCH ZER
                            zeros++;
                        }
                    }
                }
            }

            return (Image)bmp;
        }

        //FUNCKJA DEKODOWANIA ZA POMOCĄ C#
        private string decryptFunc(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            // ZMIENNA, KTÓRA BĘDZIE PRZECHOWYWAĆ ODCZYTANĄ WIADOMOŚĆ
            string extractedText = String.Empty;

            //PRZEJŚCIE PRZEZ KAŻDĄ KOLUMNĘ I RZĄD OBRAZU
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    // PRZEJŚCIE PRZEZ KAŻDE Z RGB PIKSELA
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            //WYDOBYCIE ZMIENNEJ W ELEMENTACH PIKSELA
                            case 0:
                                {
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                       //DODANIE ZMIENNEJ ZNAKOWEJ DO STRINGA
                        if (colorUnitIndex % 8 == 0)
                        {
                            charValue = reverseBits(charValue);
                            if (charValue == 0)
                            {
                                return extractedText;
                            }
                            char c = (char)charValue;
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        //FUNCJA POMOCNICZA DO DEKODOWANIA W C#, ODWRACA BITY
        private int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }        
    }
}
