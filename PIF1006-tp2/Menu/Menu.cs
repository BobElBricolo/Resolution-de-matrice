using System;
using System.Collections.Generic;

namespace PIF1006_tp2;

public class Menu
{
    private List<Option> _listeOptions;
    private string _titreMenu;
    private int _indexSelection;

    private delegate void ExitDel();

    private static readonly ExitDel Exit = () => { };

    public Menu(List<Option> listeOptions, string titreMenu)
    {
        _listeOptions = listeOptions;
        _titreMenu = titreMenu;
        _indexSelection = 0;
    }

    public void Affichage()
    {
        Console.Clear();
        Console.WriteLine(_titreMenu);
        for (int i = 0; i < _listeOptions.Count; i++)
        {
            Console.ForegroundColor = i == _indexSelection? ConsoleColor.Green : ConsoleColor.Gray;
            
            Console.WriteLine($"{i + 1}. " + _listeOptions[i].TitreOption); // get. existe pas, sélectionner l'instance puis appliquer ce qu'on veut
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Choisissez votre option avec les flèches et appuyez sur Entrée.");
    }

    public void Demarrer()
    {
        AjoutOption("Quitter", Exit);
        
        while (true)
        {
            Delegate del = Selection();
            Console.Clear();
            
            // If the option is to exit ...
            if (del.GetType() == typeof(ExitDel)) break;
            
            del.DynamicInvoke();
            WaitForUser();
        }
    }

    public Delegate Selection()
    {
        bool fin = false;
        do
        {
            Affichage();
            var useKey = Console.ReadKey();
            
            switch(useKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if (_indexSelection > 0) _indexSelection--;
                    break;
                case ConsoleKey.DownArrow:
                    if (_indexSelection < _listeOptions.Count-1) _indexSelection++;
                    break;
                case ConsoleKey.Enter:
                    fin = true;
                    break;
            }
        } while (fin == false);

        return _listeOptions[_indexSelection].CodeOption;
    }

    public void AjoutOption(string titre, Delegate func)
    {
        _listeOptions.Add(new Option(titre, func));
    }

    private static void WaitForUser()
    {
        Console.WriteLine("Appuyer sur une touche pour continuer...");
        Console.ReadKey();
    }
}