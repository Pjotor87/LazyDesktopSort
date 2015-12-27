using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LazyDesktopSort
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && 
                args.Length >= 4 &&
                args.Length <= 7 &&
                !string.IsNullOrEmpty(args[0]) &&
                !string.IsNullOrEmpty(args[1]) &&
                !string.IsNullOrEmpty(args[2]) &&
                !string.IsNullOrEmpty(args[3]))
            {
                LazyDesktopSort form = new LazyDesktopSort();
                if (args.Length == 4)
                    form.SortDesktop(args[0], args[1], args[2], args[3], string.Empty, string.Empty, string.Empty);
                else if (args.Length == 5)
                    form.SortDesktop(args[0], args[1], args[2], args[3], args[4], string.Empty, string.Empty);
                else if (args.Length == 6)
                    form.SortDesktop(args[0], args[1], args[2], args[3], args[4], args[5], string.Empty);
                else if (args.Length == 7)
                    form.SortDesktop(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LazyDesktopSort());
            }
        }
    }
}