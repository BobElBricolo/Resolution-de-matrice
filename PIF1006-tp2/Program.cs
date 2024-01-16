/*##############################################################################
 Nom du fichier: Program.cs
 Nom de l'équipe: Équipe Tertiaire
 Membre de l'équipe: Janik Lesage, Julien Boisvert, Alexis Young
###############################################################################*/

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PIF1006_tp2
{

    // - Répartition des points -:
    // Program.cs: 2 pts
    // Matrix.cs: 3 pts
    // System.cs: 3 pts
    // Rapport + guide: 2 pts)

    class Program
    {
        private static System system = LoadSystem();
        static void Main(string[] args)
        {
            
            List<Option> listeOptions = new List<Option>();

            Option option1 = new Option("Charger un fichier système", () => system = LoadSystem(AskSystem()));
            Option option2 = new Option("Afficher le système", () => PrintSystem(system));
            Option option3 = new Option("Résoudre avec Cramer", () => system.SolveUsingCramer());
            Option option4 = new Option("Résoudre par la matrice inverse", () => system.SolveUsingInverseMatrix());
            Option option5 = new Option("Résoudre avec Gauss", () => system.SolveUsingGauss());
            Option option6 = new Option("Résoudre avec Jacobi", () => system.SolveUsingJacobi(AskEpsilon()));
            Option option7 = new Option("Résoudre avec Gauss-Seidel", () => system.SolveUsingGaussSeidel(AskEpsilon()));
            
            listeOptions.Add(option1);
            listeOptions.Add(option2);
            listeOptions.Add(option3);
            listeOptions.Add(option4);
            listeOptions.Add(option5);
            listeOptions.Add(option6);
            listeOptions.Add(option7);

            Menu menu = new Menu(listeOptions, "Menu");
            menu.Demarrer();
            

            

        }


        /// <summary>
        /// Demande à l'utilisateur d'entrer le chemin du fichier système
        /// </summary>
        /// <returns> Le chemin du fichier système </returns>
        static string AskSystem()
        {
            Console.WriteLine("Entrez le chemin du fichier système");
            string filePath = Console.ReadLine();
            return filePath;
        }
        
        /// <summary>
        /// Demande à l'utilisateur d'entrer l'epsilon
        /// </summary>
        /// <returns> L'epsilon entré par l'utilisateur </returns>
        static double AskEpsilon()
        {
            Console.WriteLine("Entrez l'epsilon");
            while (true)
            {
                try
                {
                    double epsilon = double.Parse(Console.ReadLine());
                    return epsilon;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Veuillez entrer un nombre valide");
                }
            }
        }
        
        /// <summary>
        /// Charge un système à partir d'un fichier JSON
        /// </summary>
        /// <param name="filePath"> Chemin du fichier JSON </param>
        /// <returns> Le système chargé </returns>
        static System LoadSystem(string filePath = "Objet.json")
        {
            // Si le chemin n'Est pas valide, tenter de le trouver dans le dossier parent
            if (!Path.IsPathFullyQualified(filePath) &&
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filePath)))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../", filePath);
            
            // Si le chemin n'est pas valide, tenter de le trouver dans le dossier courant
            if (!Path.IsPathFullyQualified(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (!File.Exists(filePath)) // Si le fichier n'existe pas, retour avec le système de base
            {
                Console.WriteLine("Le fichier n'existe pas, retour avec le systeme de base");
                return LoadSystem();
            }

            string jsonString = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), filePath));
            
            // On essaie de désérialiser le fichier JSON
            System deserializeObject = JsonConvert.DeserializeObject<System>(jsonString);

            if (deserializeObject == null)
            {
                Console.WriteLine("Le fichier n'est pas valide, retour avec le systeme de base");
                return LoadSystem();
            }
            
            return deserializeObject;
            
        }

        /// <summary>
        /// Affiche le système ainsi que ses matrices
        /// </summary>
        /// <param name="systemAfficher"> Le système à afficher </param>
        static void PrintSystem(System systemAfficher)
        {
            Console.WriteLine("Système:");
            Console.WriteLine(systemAfficher);
            
            Console.WriteLine("Matrice A:");
            Console.WriteLine(systemAfficher.A);
            
            Console.WriteLine("Matrice B:");
            Console.WriteLine(systemAfficher.B);
        }

    }
}
