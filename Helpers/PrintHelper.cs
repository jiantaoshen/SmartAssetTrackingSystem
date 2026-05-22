using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace SmartAssetTrackingSystem.Helpers
{
    public static class PrintHelper
    {
        public static void PrintMenu()
        {
            Console.Clear();

            Console.WriteLine("1. Add Asset");
            Console.WriteLine("2. Show all Assets");
            Console.WriteLine("3. Update Asset");
            Console.WriteLine("4. Delete Asset");
            Console.WriteLine("5. Search Asset");
            Console.WriteLine("6. Get Office Report");
            Console.WriteLine("7. Get Company Report");
            Console.WriteLine("8. Exit");
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Header looks like this:     ================== title ==================
        public static void PrintHeader(string title, int totalWidth)
        {
            Console.Clear();

            int left = (totalWidth - title.Length) / 2;

            string titleline = new string('=', left) + 
                                " " + title + " " + 
                                new string('=', totalWidth - left - title.Length);

            Console.WriteLine(titleline + "\n");
        }

        /*Subheader looks like this:
         * Subtitle
         * ------------------------
        */

        public static void PrintSubHeader(string title, int totalWidth)
        {
            Console.WriteLine(title);
            Console.WriteLine(new string('-', totalWidth));
        }


        //Footer looks like this:     ====================================
        public static void PrintFooter(int totalWidth)
        {
            Console.WriteLine(new string('=', totalWidth));
        }
    }
}
