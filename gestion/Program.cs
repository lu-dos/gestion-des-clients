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
                NombreClient();
                break;
            case 5:
                Console.WriteLine("Modifier un client : ");
                ModifClient();
                break;
            case 6:
                Console.WriteLine("Supprimer une fiche : ");
                SuppClient();
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

static string GetNextFiche(string cheminfichier)
{
    int maxId = 0;
    if (!File.Exists(cheminfichier))
        return "1";

    try
    {
        using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var br = new BinaryReader(fs))
        {
            while (fs.Position < fs.Length)
            {
                string fiche = br.ReadString();
                // lire les autres champs pour avancer la position
                br.ReadString();
                br.ReadString();
                br.ReadString();

                if (int.TryParse(fiche, out int id))
                {
                    if (id > maxId) maxId = id;
                }
            }
        }
    }
    catch
    {
        // en cas d'erreur de lecture, retourner 1 comme valeur sûre
        return "1";
    }

    return (maxId + 1).ToString();
}

static void AjoutClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");

    // calculer la fiche auto-incrémentée
    string fiche = GetNextFiche(cheminfichier);
    Console.WriteLine($"Fiche assignée automatiquement : {fiche}");

    Console.Write("Entrez le nom du client : ");
    string nomInput = Console.ReadLine() ?? "";
    string nom = Majuscule(nomInput.Trim());

    Console.Write("Entrez le prénom du client : ");
    string prenomInput = Console.ReadLine() ?? "";
    string prenom = string.IsNullOrWhiteSpace(prenomInput) ? "" : FirstMajuscule(prenomInput.Trim());

    Console.Write("Entrez le numéro du client : ");
    string num = Console.ReadLine() ?? "";

    if (num.Length > 10)
    {
        num = num.Substring(0, 10);
        Console.WriteLine("Le numéro a été tronqué à 10 caractères maximum.");
    }

    try
    {
        using (FileStream fs = new FileStream(cheminfichier, FileMode.Append, FileAccess.Write))
        using (BinaryWriter sw = new BinaryWriter(fs))
        {
            sw.Write(fiche);
            sw.Write(nom);
            sw.Write(prenom);
            sw.Write(num);
        }

        Console.WriteLine("Client ajouté avec succès !");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur lors de l'écriture du fichier : " + ex.Message);
    }

    Console.WriteLine("Appuyez sur une touche pour continuer ...");
    Console.ReadLine();
}

static string Majuscule(string nom)
{
    if (nom == null) return "";
    return nom.ToUpperInvariant();
}
static string FirstMajuscule(string prenom)
{
    if (string.IsNullOrWhiteSpace(prenom)) return "";
    prenom = prenom.ToLowerInvariant();
    return char.ToUpper(prenom[0]) + prenom.Substring(1);
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

    using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
    using (var br = new BinaryReader(fs))
    {
        long ficheIndex = 0;
        while (fs.Position < fs.Length)
        {
            string fiche = br.ReadString();
            string nom = br.ReadString();
            string prenom = br.ReadString();
            string numero = br.ReadString();

            Console.WriteLine($"Fiche #{ficheIndex} -> N°Fiche: {fiche} | Nom: {nom} | Prénom: {prenom} | Numéro: {numero}");
            ficheIndex++;
        }
    }
    Console.WriteLine("Appuyez sur Entrée pour continuer ...");
    Console.ReadLine();
}



static void NombreClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");
    using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
    using (var br = new BinaryReader(fs))
    {
        int count = 0;
        while (fs.Position < fs.Length)
        {
            br.ReadString();
            br.ReadString();
            br.ReadString();
            br.ReadString();
            count++;
        }
        Console.WriteLine("Nombre total de clients : " + count);
    }

    Console.WriteLine("Voulez-vous afficher la liste complète des clients ? (1=Oui, 2=Non) : ");

    Console.WriteLine("1. Oui");
    Console.WriteLine("2. Non");

    int choix = 0;
    if (int.TryParse(Console.ReadLine(), out choix))
    {
        switch (choix)
        {
            case 1:
                Console.WriteLine("Oui");
                AfficheAllClients();
                break;
            case 2:
                Console.WriteLine("Non");
                Console.WriteLine("Appuyez sur Entrée pour continuer ...");
                Console.ReadLine();
                break;
        }
    }
}

static void ModifClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");


    var clients = new List<(string fiche, string nom, string prenom, string numero)>();

    using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
    using (var br = new BinaryReader(fs))
    {
        while (fs.Position < fs.Length)
        {
            string fiche = br.ReadString();
            string nom = br.ReadString();
            string prenom = br.ReadString();
            string numero = br.ReadString();
            clients.Add((fiche, nom, prenom, numero));
        }
    }

    Console.Write("Entrez la fiche du client à modifier : ");
    string ficheRecherche = Console.ReadLine();

    var correspondances = new List<int>();
    for (int i = 0; i < clients.Count; i++)
    {
        if (string.Equals(clients[i].fiche, ficheRecherche, StringComparison.OrdinalIgnoreCase))
            correspondances.Add(i);
    }

    if (correspondances.Count == 0)
    {
        Console.WriteLine($"Aucun client trouvé pour la fiche : {ficheRecherche}");
        Console.WriteLine("Appuyez sur Entrée pour continuer ...");
        Console.ReadLine();
        return;
    }

    int indexChoisi = correspondances[0];
    if (correspondances.Count > 1)
    {
        Console.WriteLine("Plusieurs fiches correspondent. Choisissez l'index à modifier :");
        for (int k = 0; k < correspondances.Count; k++)
        {
            int idx = correspondances[k];
            var c = clients[idx];
            Console.WriteLine($"{k + 1}. Index global {idx} -> Fiche: {c.fiche} | Nom: {c.nom} | Prénom: {c.prenom} | Numéro: {c.numero}");
        }
        Console.Write("Votre choix (numéro) : ");
        if (int.TryParse(Console.ReadLine(), out int choix) && choix >= 1 && choix <= correspondances.Count)
        {
            indexChoisi = correspondances[choix - 1];
        }
        else
        {
            Console.WriteLine("Choix invalide. Annulation de la modification.");
            Console.WriteLine("Appuyez sur Entrée pour continuer ...");
            Console.ReadLine();
            return;
        }
    }

    var client = clients[indexChoisi];
    Console.WriteLine("Valeurs actuelles :");
    Console.WriteLine($"Fiche : {client.fiche}");
    Console.WriteLine($"Nom   : {client.nom}");
    Console.WriteLine($"Prénom: {client.prenom}");
    Console.WriteLine($"Numéro: {client.numero}");
    Console.WriteLine();
    Console.WriteLine("Entrez les nouvelles valeurs (laissez vide pour conserver la valeur actuelle) :");

    // La fiche n'est pas modifiable : on la conserve telle quelle
    string nouvelleFiche = client.fiche;

    Console.Write("Nouveau nom : ");
    string nouveauNom = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(nouveauNom))
        nouveauNom = client.nom;
    else
        nouveauNom = Majuscule(nouveauNom);

    Console.Write("Nouveau prénom : ");
    string nouveauPrenom = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(nouveauPrenom))
        nouveauPrenom = client.prenom;
    else
    {
        nouveauPrenom = nouveauPrenom.Trim();
        if (nouveauPrenom.Length > 0)
            nouveauPrenom = FirstMajuscule(nouveauPrenom);
    }

    Console.Write("Nouveau numéro : ");
    string nouveauNumero = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(nouveauNumero))
        nouveauNumero = client.numero;
    else
    {
        if (nouveauNumero.Length > 10)
        {
            nouveauNumero = nouveauNumero.Substring(0, 10);
            Console.WriteLine("Le numéro a été tronqué à 10 caractères maximum.");
        }
    }

    // Mettre à jour la liste en conservant la fiche existante
    clients[indexChoisi] = (nouvelleFiche, nouveauNom, nouveauPrenom, nouveauNumero);

    // Réécrire tout le fichier
    using (var fs = new FileStream(cheminfichier, FileMode.Create, FileAccess.Write, FileShare.None))
    using (var bw = new BinaryWriter(fs))
        foreach (var c in clients)
        {
            bw.Write(c.fiche);
            bw.Write(c.nom);
            bw.Write(c.prenom);
            bw.Write(c.numero);
        }

    Console.WriteLine("Modification enregistrée avec succès !");


    Console.WriteLine("Appuyez sur Entrée pour continuer ...");
    Console.ReadLine();
}

static void SuppClient()
{
    string repertoryprojet = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    string cheminfichier = Path.Combine(repertoryprojet, "clients.dat");
    var clients = new List<(string fiche, string nom, string prenom, string numero)>();
    using (var fs = new FileStream(cheminfichier, FileMode.Open, FileAccess.Read, FileShare.Read))
    using (var br = new BinaryReader(fs))
    {
        while (fs.Position < fs.Length)
        {
            string fiche = br.ReadString();
            string nom = br.ReadString();
            string prenom = br.ReadString();
            string numero = br.ReadString();
            clients.Add((fiche, nom, prenom, numero));
        }
    }
    Console.Write("Entrez la fiche du client à supprimer : ");
    string ficheRecherche = Console.ReadLine();
    int indexASupprimer = clients.FindIndex(c => string.Equals(c.fiche, ficheRecherche, StringComparison.OrdinalIgnoreCase));
    if (indexASupprimer == -1)
    {
        Console.WriteLine($"Aucun client trouvé pour la fiche : {ficheRecherche}");
        Console.WriteLine("Appuyez sur Entrée pour continuer ...");
        Console.ReadLine();
        return;
    }

    clients.RemoveAt(indexASupprimer);
    using (var fs = new FileStream(cheminfichier, FileMode.Create, FileAccess.Write, FileShare.None))
    using (var bw = new BinaryWriter(fs))
        foreach (var c in clients)
        {
            bw.Write(c.fiche);
            bw.Write(c.nom);
            bw.Write(c.prenom);
            bw.Write(c.numero);
        }
    Console.WriteLine("Client supprimé avec succès !");
    Console.WriteLine("Appuyez sur Entrée pour continuer ...");
    Console.ReadLine();
}