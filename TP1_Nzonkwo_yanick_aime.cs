using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P24_TP1
{
    internal class Program
    {

        enum categorie
        {
            zero, un, deux, trois, troisplus,
            quatre, quatreplus, cinq, cinqplus, six
        }

        struct combinaison
        {
            public int[] table;

        }


        //************définition des variables publiques et des constantes**********
        const int taille = 6, min = 1, max = 49;
        static bool fin_partie;
        private static int nb_combinaisons = 200;
        static int complement = alea();               // initialisations
        static combinaison gagnante = NewCombi();

        static categorie[] famille = {
            categorie.zero, categorie.un, categorie.deux, categorie.trois, categorie.troisplus,
            categorie.quatre, categorie.quatreplus, categorie.cinq, categorie.cinqplus, categorie.six 
        };

        static string[] fam =
        { 
            "0/6", "1/6", "2/6", "3/6", "3/6 +", "4/6",
             "4/6 +", "5/6", "5/6 +", "6/6"
        };

        static float[] taux_type = new float[10];
        static categorie[] type_resultat = new categorie[nb_combinaisons];
        static combinaison[] tirage = new combinaison[nb_combinaisons];
        static int[] nb_apparitions = new int[taille];
        // un tirage est un tableau formé de combinaisons


        //****************Définition des méthodes******************



        // Méthode qui génère un nombre aleatoire compris entre min et max
        static int alea()
        {
            Random nouveau = new Random(new Random(DateTime.Now.Millisecond*1000).Next()); // le plus de hazard possible
            int res = nouveau.Next(min, max + 1);
            return res;
        }

      

        // Méthode qui teste la présence d'un nombre dans une combinaison
        static bool Present(combinaison comb, int nb)
        {
            bool res = false;
            for (int i = 0; i < taille; i++)
            {
                if (comb.table[i] == nb)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }


        // Méthode qui génère une combinaison
        static combinaison NewCombi()
        {
            combinaison res; int tampon = alea();
            res.table = new int[taille];

            res.table[0] = alea();
            for (int i = 1; i < taille; i++)
            {
                do
                {
                    tampon = alea();
                    
                } while (Present(res, tampon));

                res.table[i] = tampon;
            }

            return res;
        }




        // Méthode qui affiche sur une ligne la combinaison reçue en argument
        static void AfficherCombi(combinaison comb)
        {
            Console.Write(comb.table[0]);
            for (int j = 1; j < taille; j++)
            {
                Console.Write($" - {comb.table[j]}");
            }

            //Console.WriteLine();
        }





        // Méthode qui génère et affiche les combinaisons de l'utilisateur
        static combinaison[] Tirer()
        {
            combinaison[] res = new combinaison[nb_combinaisons];

            for (int i = 0; i < nb_combinaisons; i++)
            {
                res[i] = NewCombi();
                AfficherCombi(res[i]);
                Console.WriteLine();
            }

            return res;
        }



        // Méthode qui détermine la famille de résultat d'une combinaison donnée relativement à 
        // une combinaison gagnante et un numéro complémentaire donnés
        static categorie TrouveCat(combinaison gagn, int comple, combinaison comb)
        {
            int nb = 0;
            bool plus = Present(comb, comple);
            categorie res = categorie.zero;
            foreach (int val in comb.table)
            {
                if (Present(gagn, val))
                {
                    nb++;
                }
            }

            switch (nb)
            {
                case 0:
                    res = categorie.zero;
                    break;
                case 1:
                    res = categorie.un;
                    break;
                case 2:
                    res = categorie.deux;
                    break;
                case 3:
                    res = (plus) ? categorie.troisplus : categorie.trois;
                    break;
                case 4:
                    res = (plus) ? categorie.quatreplus : categorie.quatre;
                    break;
                case 5:
                    res = (plus) ? categorie.cinqplus : categorie.cinq;
                    break;
                case 6:
                    res = categorie.six;
                    break;
            }

            return res;
        }




        // Méthode qui détermine le pourcentage de chaque famille de résultats
        static float[] TauxFamille()
        {

            float[] res = new float[10];

            for (int j = 0; j < 4; j++)
            {
                type_resultat[j] = TrouveCat(gagnante, complement, tirage[j]);

            }

            for (int i = 0; i < 10; i++)
            {
                int nb = 0;
                for (int j = 0; j < 4; j++)
                {
                    //type_resultat[j] = TrouveCat(gagnante, complement, tirage[j]);
                    if (type_resultat[j] == famille[i])
                    {
                        nb++;
                    }
                }
                res[i] = (float)nb / nb_combinaisons;
            }

            return res;

        }




        // Méthode qui pour chaque nombre pris dans une combinaison donnée,
        // détermine son nombre d'apparition dans un tirage (suite de combinaisons) tir donnée
        static int[] Apparitions(combinaison[] tir, combinaison comb)
        {
            int[] res = new int[taille];
            for (int i = 0; i < nb_combinaisons; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (Present(tir[i], comb.table[j]))
                    {
                        res[j]++;
                    }
                }
            }

            return res;

        }





        // Méthode qui démarre une session de jeux
        static void NouvellePartie()
        {
            string rep;
            bool valide = true;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("NOUVELLE PARTIE DE 6/49");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            do
            {
                if (!valide)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Erreur! veuiller saisir un entier compris entre 10 et 200");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Beep();
                        Thread.Sleep(200);
                    }
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Combien de combinaison voulez-vous (10-200)? ");
                rep = Console.ReadLine();
                Console.WriteLine();
                valide = ((int.TryParse(rep, out nb_combinaisons) == true) && (nb_combinaisons >= 10 && nb_combinaisons < 200));


            } while (!valide);

            tirage = Tirer();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            gagnante = NewCombi();
            Console.WriteLine();
            Console.WriteLine("combinaison gnagnate : ");
            Console.WriteLine();
            AfficherCombi(gagnante);
            Console.WriteLine();
            Console.WriteLine();
            complement = alea();
            Console.WriteLine($"Numéro complémentaire : {complement}");
            Console.WriteLine();

        }





        // Méthode qui effectue tous les calculs de la session en cours
        static void Calculer()
        {
            taux_type = TauxFamille();
            nb_apparitions = Apparitions(tirage, gagnante);

        }



        // Méthode qui affiche les résultats de la session en cours
        static void Afficher()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("RÉSULTATS DE CE TIRAGE");
            Console.WriteLine("=======================");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("** Les combinaisons gagnantes de l'utilisateur :  ");
            Console.WriteLine("-----------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < nb_combinaisons; i++)
            {
                bool gagne = ((int)type_resultat[i] != 0) && ((int)type_resultat[i] != 1) && ((int)type_resultat[i] != 2);
                if (gagne)
                {
                    AfficherCombi(tirage[i]);
                    Console.Write("     ");
                    for (int j = 0; j < 10; j++)
                    {
                        if ((int) type_resultat[i] == j)
                        {
                            Console.WriteLine("Catégorie : " + fam[j]);
                        }
                    }

                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("** nombre d'apparition des numéros gagnants :  ");
            Console.WriteLine("---------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();

            for (int j = 0; j < taille; j++)
            {
                Console.WriteLine($"numero : {gagnante.table[j]}  apparaît  {nb_apparitions[j]} fois");
            }
            Console.ResetColor();
            Console.WriteLine();



            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("** Pourcentage des familles de résultat dans les combinanaisons du joueur  :  ");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();

            for (int j = 0; j < 10; j++)
            {
                Console.WriteLine($"Famille de résultat : {fam[j]}            Pourcentage : {taux_type[j]*100:C2} %");
            }

        }




        // Méthode qui termine une session de jeux
        static bool Terminer()
        {
            bool res = false, valide = true;
            string rep;

            do
            {


                if (!valide)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Entrée non valide! Veuillez répondre par oui (O) ou par non (N)");
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Beep();
                        Thread.Sleep(200);
                    }
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.Write("Voulez-vous effectuer une nouvelle partie (oui / non)? ");
                rep = Console.ReadLine().ToLower();
                Console.WriteLine();
                valide = (rep == "oui" || rep == "non" || rep == "o" || rep == "n");
            } while (!valide);

            res = (rep == "n");
            return res;
        }




        // ****************************************************************************
        // *********************  CORPS DU PROGRAMME  *********************************
        // ****************************************************************************

        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                NouvellePartie();
                Calculer();
                Afficher();
                fin_partie = Terminer();
            } while (!fin_partie);

            Console.WriteLine("Aurevoir et à bientôt ☻!! ");
            Console.WriteLine("Appuyer sur une touche pour quitter : ");
            Console.ReadKey();


        }
    }
}
