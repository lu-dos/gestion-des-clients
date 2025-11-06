using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

bool quitter = false;

while (!quitter)
{
    Console.Clear();
    Console.WriteLine("Menu :");
    Console.WriteLine("1. Saisir un nouveau client");
    Console.WriteLine("2. Afficher un client");
    Console.WriteLine("3. Afficher tous les clients");
    Console.WriteLine("4. Afficher le nombre de client");
    Console.WriteLine("5. Option 5");
    Console.WriteLine("6. Option 6");
    Console.WriteLine("10. Quitter");
    Console.Write("Votre choix : ");

    int choix = 0;

    if (int.TryParse(Console.ReadLine(), out choix))
    {
        Console.Clear();

        switch (choix)
        {
            case 1:
                Console.WriteLine("Saisir un nouveau client : ");
                AjoutClient();
                break;
            case 2:
                Console.WriteLine("Afficher un client : ");
                AfficheClient();
                break;
            case 3:
                Console.WriteLine("Afficher tous les clients : ");
                break;
            case 4:
                Console.WriteLine("Afficher le nombre de client : ");
                break;
            case 5:
                Console.WriteLine("Modifier un client : ");
                break;
            case 6:
                Console.WriteLine("Supprimer une fiche : ");
                break;
            case 10:
                quitter = true;
                Console.WriteLine("Fermeture du programme...");
                break;
            default:
                Console.WriteLine("Choix invalide, veuillez réessayer.");
                break;
        }
    }
}

static void AjoutClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");
    using (FileStream fs = new FileStream(cheminfichier, FileMode.Append, FileAccess.Write))
    using (BinaryWriter sw = new BinaryWriter(fs))
    {

        Console.Write("Entrez le nom du client : ");
        string nom = Console.ReadLine();
        sw.Write(Majuscule(nom));

        Console.Write("Entrez le prénom du client : ");
        string prenom = Console.ReadLine();
        sw.Write(FirstMajuscule(prenom));

        string nouvelleLigne = $"{nom},{prenom}";

    }

}

static string Majuscule(string nom)
{
    nom = nom.ToUpper();
    return nom;
}
static string FirstMajuscule(string prenom)
{
    prenom = prenom.ToLower();
    prenom = char.ToUpper(prenom[0]) + prenom.Substring(1);
    return prenom;
}

static void AfficheClient()
{

    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");

    Console.Write("Entrez le nom du client à afficher : ");
    string nomrecherche = Console.ReadLine();

    var matches = new List<(string nom, string prenom)>();


    using (FileStream fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read))
    using (BinaryReader sr = new BinaryReader(fs))
    {

        while (fs.Position < fs.Length)
        {
            string nomFichier = sr.ReadString();
            string prenomFichier = sr.ReadString();

            if (string.Equals(nomFichier, nomrecherche, StringComparison.OrdinalIgnoreCase))
            {
                matches.Add((nomFichier, prenomFichier));
            }
        }

    }




    static void AfficheAllClients()
    {
        string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");
        using (FileStream fs = new FileStream(cheminfichier, FileMode.Append, FileAccess.Write))
        using (BinaryWriter sw = new BinaryWriter(fs))
        {

            Console.WriteLine("Liste de tous les clients : ");
            Console.WriteLine(Majuscule("nom") + " " + FirstMajuscule("prenom"));

        }

    }

}

static void NombreClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");
    using (FileStream fs = new FileStream(cheminfichier, FileMode.Append, FileAccess.Write))
    using (BinaryWriter sw = new BinaryWriter(fs))
    using (BinaryReader sr = new BinaryReader(fs))
    {
        int count = 0;
        while (fs.Position < fs.Length)
        {
            string nomFichier = sr.ReadString();
            string prenomFichier = sr.ReadString();
            count++;
        }
        Console.WriteLine("Nombre total de clients : " + count);
    }
}