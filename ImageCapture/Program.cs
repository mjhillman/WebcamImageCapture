namespace WebcamImageCapture
{
    internal static class Program
    {

        public static string CAPTURE_DIRECTORY { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CAPTURE_DIRECTORY = "D:\\Captures";
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}