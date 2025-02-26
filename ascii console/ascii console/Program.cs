using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ascii_console
{
    internal class Program
    {
        private const double WIDTH_OFFSET = 2.5;
        private const int MAX_WIDTH = 400;
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *JPEG"
            };
            //openFileDialog.ShowDialog();
            Console.WriteLine("Press enter to start...\n");

            while (true)
            {
                Console.ReadLine();
                if (openFileDialog.ShowDialog() != DialogResult.OK) 
                    continue;
                Console.Clear();

                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayscale();
                //char[] _asciiTable = { '.', ',', ':', '+', '*', '?', '%', 'S', '#', '@' };
                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();

                foreach (var row in rows)
                    Console.WriteLine(row);

                File.WriteAllLines("image.txt", rows.Select(r => new string(r)));
                Console.SetCursorPosition(0, 0);
            }

        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxWidth = 350;
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;
            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            return bitmap;
        }
    }
}
