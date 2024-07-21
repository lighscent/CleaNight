using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

internal class CleaNight
{
    internal static async Task Main(string[] _)
    {
        string repo = "Light2k4/CleaNight";
        var latestReleaseInfo = await GetLatestReleaseInfo(repo);
        string latestVersion = latestReleaseInfo.Item1;
        string versionType = latestReleaseInfo.Item2;

        string ascii = @"

                         ██████╗██╗     ███████╗ █████╗ ███╗   ██╗██╗ ██████╗ ██╗  ██╗████████╗
                        ██╔════╝██║     ██╔════╝██╔══██╗████╗  ██║██║██╔════╝ ██║  ██║╚══██╔══╝
                        ██║     ██║     █████╗  ███████║██╔██╗ ██║██║██║  ███╗███████║   ██║   
                        ██║     ██║     ██╔══╝  ██╔══██║██║╚██╗██║██║██║   ██║██╔══██║   ██║   
                        ╚██████╗███████╗███████╗██║  ██║██║ ╚████║██║╚██████╔╝██║  ██║   ██║   
                         ╚═════╝╚══════╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   
                                                                       
";

        string getVersionLocal = "1.0.2";

        string header = ascii + "\n" + "                            Current Version: " + getVersionLocal + "      Last version: " + latestVersion + " (" + versionType + ")" + "\n\n";

        CheckAdminRights(header);

        Console.Title = "CleaNight - github.com/Light2k4/CleaNight";
        Console.ForegroundColor = ConsoleColor.Green;

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(header);
        Console.ForegroundColor = ConsoleColor.Green;

        bool continuer = true;
        bool afficherMenu = true;
        while (continuer)
        {
            if (afficherMenu)
            {
                Console.WriteLine("1. Run CleaNight");
                Console.WriteLine("2. Discord");
                Console.WriteLine("3. Github");
                Console.WriteLine("4. Quit");
                Console.Write("\nSelect an option [1-4]:");
            }

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    SupprimerDossiers(header);
                    afficherMenu = false;
                    break;
                case "2":
                    System.Diagnostics.Process.Start("https://discord.gg/Acdp4B6KUv");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(header);
                    Console.ForegroundColor = ConsoleColor.Green;
                    afficherMenu = false;
                    break;
                case "3":
                    System.Diagnostics.Process.Start("https://github.com/light2k4/CleaNight");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(header);
                    Console.ForegroundColor = ConsoleColor.Green;
                    afficherMenu = false;
                    break;
                case "4":
                    continuer = false;
                    Console.WriteLine("Bye...");
                    await Task.Delay(1000);
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    await Task.Delay(1000);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(header);
                    Console.ForegroundColor = ConsoleColor.Green;
                    afficherMenu = true;
                    break;
            }
        }
    }

    private static void CheckAdminRights(string header)
    {
        if (!new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(header);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please run this program as an administrator.");
            Environment.Exit(0);
        }
    }



    private static async Task<Tuple<string, string>> GetLatestReleaseInfo(string repo)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = $"https://api.github.com/repos/{repo}/releases/latest";
                client.DefaultRequestHeaders.UserAgent.ParseAdd("CleaNightApp/1.0 (HttpClient .NET)");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var root = Newtonsoft.Json.Linq.JObject.Parse(responseBody);
                string tagName = (string)root["tag_name"];
                bool preRelease = (bool)root["prerelease"];

                string versionType = preRelease ? "Dev build" : "Stable build";
                return new Tuple<string, string>(tagName, versionType);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return new Tuple<string, string>("Could not retrieve version", "Unknown");
            }
        }
    }

    private static async void SupprimerDossiers(string header)
    {
        bool confirmation = DemanderConfirmation("Are you sure you want to delete all temp folders?");
        if (!confirmation)
        {
            Console.WriteLine("Operation cancelled.");
            return;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(header);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Deleting temp folders...\n");

        // Définition des répertoires à nettoyer
        string[] cleanupDirs = new string[]
        {
        Environment.GetEnvironmentVariable("WinDir") + @"\Temp",
        Environment.GetEnvironmentVariable("WinDir") + @"\Prefetch",
        Environment.GetEnvironmentVariable("Temp"),
        Environment.GetEnvironmentVariable("AppData") + @"\Local\Temp",
        Environment.GetEnvironmentVariable("LocalAppData") + @"\Temp",
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Microsoft\Windows\INetCache",
        Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\AMD",
        Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\NVIDIA",
        Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\INTEL"
        };

        foreach (string dossier in cleanupDirs)
        {
            try
            {
                if (Directory.Exists(dossier))
                {
                    Directory.Delete(dossier, true);
                    Console.WriteLine($"Deleted: {dossier}");
                }
                else
                {
                    Console.WriteLine($"Not found: {dossier}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting folder {dossier}: {e.Message}");
            }
        }

        Console.WriteLine("\n\nAll temp folders have been deleted.\n\n");
        Console.WriteLine("1. Leave a star on Github");
        Console.WriteLine("2. Quit");
        Console.Write("\nSelect an option [1-2] : ");
        string choixEnd = Console.ReadLine();

        switch (choixEnd)
        {
            case "1":
                System.Diagnostics.Process.Start("https://github.com/light2k4/CleaNight/");
                await Task.Delay(1000);
                Environment.Exit(0);
                break;
            case "2":
                Console.WriteLine("Bye...");
                await Task.Delay(1000);
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. \nBye...");
                await Task.Delay(1000);
                Environment.Exit(0);
                break;
        }
    }


    private static bool DemanderConfirmation(string message)
    {
        Console.Write($"{message} (Y/N): ");
        string reponse = Console.ReadLine();
        return reponse.Equals("Y", StringComparison.OrdinalIgnoreCase);
    }
}
