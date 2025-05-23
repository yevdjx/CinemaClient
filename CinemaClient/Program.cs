namespace CinemaClient
{
    using CinemaClient.Forms;
    using CinemaClient.Services;
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var api = new ApiService("http://91-236-79-198.nip.io");
            Application.Run(new LoginForm(api));
        }
    }
}