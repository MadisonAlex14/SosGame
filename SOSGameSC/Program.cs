using System;
using System.Windows.Forms;

namespace SOSGameApp
{
    static class Program
    {
        // Main entry point
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the main form
            Application.Run(new Form1());
        }
    }
}
