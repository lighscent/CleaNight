using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

internal class CleaNight
{
    internal static async Task Main(string[] _)
    {
        string repo = "Light2k4/CleaNight";
        string latestReleaseInfo = await GetLatestReleaseInfo(repo);
        Console.WriteLine(latestReleaseInfo);

        string ascii = @"

             ██████╗██╗     ███████╗ █████╗ ███╗   ██╗██╗ ██████╗ ██╗  ██╗████████╗
            ██╔════╝██║     ██╔════╝██╔══██╗████╗  ██║██║██╔════╝ ██║  ██║╚══██╔══╝
            ██║     ██║     █████╗  ███████║██╔██╗ ██║██║██║  ███╗███████║   ██║   
            ██║     ██║     ██╔══╝  ██╔══██║██║╚██╗██║██║██║   ██║██╔══██║   ██║   
            ╚██████╗███████╗███████╗██║  ██║██║ ╚████║██║╚██████╔╝██║  ██║   ██║   
             ╚═════╝╚══════╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   
                                                                       
";

        string getVersionLocal = "1.0.0";
        string getVersionGithub = latestReleaseInfo.Split(' ')[4];

        string header = ascii + "\n" + "Version: " + getVersionLocal + "      Last version: " + getVersionGithub + "\n\n";

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
                Console.WriteLine("1. Supprimer les dossiers temporaires");
                Console.WriteLine("2. Rejoindre le serveur Discord");
                Console.WriteLine("3. Quitter");
                Console.Write("\nVotre choix : ");
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
                    continuer = false;
                    Console.WriteLine("Bye...");
                    await Task.Delay(1000);
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
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

    private static async Task<string> GetLatestReleaseInfo(string repo)
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

                string versionType = preRelease ? "version Dev (pre release)" : "version Stable (release)";
                return $"La dernière version est {tagName} et c'est une {versionType}.";
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return "Impossible de récupérer les informations de la dernière version.";
            }
        }
    }

    private static async void SupprimerDossiers(string header)
    {
        bool confirmation = DemanderConfirmation("Êtes-vous sûr de vouloir supprimer les dossiers temporaires ?");
        if (!confirmation)
        {
            Console.WriteLine("Suppression annulée.");
            return;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(header);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Suppression des dossiers en cours...");

        string[] dossiers = Directory.GetDirectories(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Temp");

        foreach (string dossier in dossiers)
        {
            try
            {
                Directory.Delete(dossier, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la suppression du dossier " + dossier + ": " + e.Message);
            }
        }

        Console.WriteLine("\n\nSuppression terminée.");
        Console.WriteLine("1. Laissez un avis");
        Console.WriteLine("2. Quitter");
        Console.Write("\nVotre choix : ");
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
                Console.WriteLine("Choix invalide. \nBye...");
                await Task.Delay(1000);
                Environment.Exit(0);
                break;
        }
    }

    private static bool DemanderConfirmation(string message)
    {
        Console.Write($"{message} (O/N): ");
        string reponse = Console.ReadLine();
        return reponse.Equals("O", StringComparison.OrdinalIgnoreCase);
    }
}
