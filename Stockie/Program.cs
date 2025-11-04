using System;
using System.IO;
using System.Windows.Forms;

namespace Stockie
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Configurar el DataDirectory para que apunte al directorio de la aplicación
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);

                // Asegurarse de que exista el directorio App_Data si no existe
                string appDataPath = Path.Combine(baseDirectory, "App_Data");
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error iniciando la aplicación: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
