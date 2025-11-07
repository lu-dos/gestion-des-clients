using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

bool quitter = false;

while (!quitter)
{
    Console.Clear();
    Console.WriteLine("Menu :");
    Console.WriteLine("1. Saisir un nouveau client");
    Console.WriteLine("2. Afficher un client");
    Console.WriteLine("3. Afficher tous les clients");
    Console.WriteLine("4. Afficher le nombre de client");
    Console.WriteLine("5. Modifier un client");
    Console.WriteLine("6. Supprimer une fiche");
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
                AfficheAllClients();
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
         
        Console.Write("Entrez le Numéro de fiche du client : ");
        string fiche = Console.ReadLine();
        sw.Write(fiche);

        Console.Write("Entrez le nom du client : ");
        string nom = Console.ReadLine();
        sw.Write(Majuscule(nom));

        Console.Write("Entrez le prénom du client : ");
        string prenom = Console.ReadLine();
        sw.Write(FirstMajuscule(prenom));

        Console.Write("Entrez le numéro du client : ");
        string num = Console.ReadLine();

        if (num.Length > 10)
        {
            num = num.Substring(0, 10);
            Console.WriteLine("Le numéro a été tronqué à 10 caractères maximum.");
        }
        else
        {
            Console.WriteLine("Client ajouté avec succès !");
        }
        sw.Write(num);

        Console.WriteLine($"Appuyez sur une touche pour continuer ...");
        Console.ReadLine();

        string nouvelleLigne = $"{fiche}, {nom},{prenom},{num}";

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
    string nomrecherche = Console.ReadLine() ?? "";
    string nomRechercheNormalise = nomrecherche.ToUpperInvariant();

    var matches = new List<(string fiche, string nom, string prenom, string numero)>();

    using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
    using (var sr = new BinaryReader(fs))
    {
        while (fs.Position < fs.Length)
        {
            string ficheStr = sr.ReadString();
            string nomFichier = sr.ReadString();
            string prenomFichier = sr.ReadString();
            string numFichier = sr.ReadString();

            if (string.Equals(nomFichier, nomRechercheNormalise, StringComparison.OrdinalIgnoreCase))
            {
                matches.Add((ficheStr, nomFichier, prenomFichier, numFichier));
            }
        }
    }

    if (matches.Count > 0)
    {
        Console.WriteLine($"Clients trouvés pour le nom \"{nomrecherche}\" :");
        foreach (var m in matches)
            Console.WriteLine($"Fiche #{m.fiche} -> Nom: {m.nom}, Prénom: {m.prenom}, Numéro: {m.numero}");
    }
    else
    {
        Console.WriteLine($"Aucun client trouvé pour le nom : {nomrecherche}");
    }

    Console.WriteLine("Appuyez sur une touche pour continuer ...");
    Console.ReadLine();
}


static void AfficheAllClients()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");


    Console.WriteLine("Liste de tous les clients :");

    using (FileStream fs = new FileStream(cheminfichier, FileMode.Append, FileAccess.Write))
    using (BinaryWriter sw = new BinaryWriter(fs))
    {


    }
    Console.ReadLine();

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
