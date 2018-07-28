using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HOH_DEMO
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //// adicionar argv para obter o ficheiro a abrir

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Mainform());
        }
    }
}
