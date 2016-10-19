using System;
using System.Threading;

namespace lifeGame
{
    class Program
    {
        // faisons le scan sur un tableau carré de vDim colonnes et vDim lignes
        static int vDim = 20;
        static bool[,] vCurrentCellTab = new bool[vDim, vDim];
        static bool[,] vNewCellTab = new bool[vDim, vDim];

        //copie le contenu de vNewCellTab dans vCurrentCellTab et reinitialise vNewCellTab.
        public static void setNewCellTabInCurrentCellTab()
        {
            for (int vi = 0; vi < vDim; vi++)
                for (int vj = 0; vj < vDim; vj++)
                {
                    vCurrentCellTab[vi, vj] = vNewCellTab[vi, vj];
                    vNewCellTab[vi, vj] = false;
                }
        }

        // en examinant la cellule courante , on vérifie si elle sera toujours vivante (true) ou pas (false)
        public static bool checkCell(int ax, int ay)
        {
            int vNbrAroundCellAlive = 0;
            bool vbCurrentCellAlive = vCurrentCellTab[ax, ay];
            // on vérifie les 8 (9 - la cellule courante au centre) cellules autour de la cellule courante
            for (int vi = -1; vi < 2; vi++) // on parcourt -1 , 0 , 1
            {
                for (int vj = -1; vj < 2; vj++) // on parcourt -1 , 0 , 1
                {
                    if ((vi != 0) || (vj != 0)) // on ne prend pas la cellule qu'on examine
                    {
                        if (vCurrentCellTab[ax + vi, ay + vj]) // la cellule autour est vivante
                            vNbrAroundCellAlive++;
                    }
                }
            }
            
            if ((vbCurrentCellAlive) && ((vNbrAroundCellAlive == 2) || (vNbrAroundCellAlive == 3)))
                return true; // la cellule reste en vie
            if ((!vbCurrentCellAlive) && (vNbrAroundCellAlive == 3))
                return true; // la cellule revit
            return false;
        }
        // utilisons une méthode aléatoire pour remplir le tableau de cellules vivantes ou non
        public static void fillRandomCellTab()
        {
            int vRandomValue;
            int vDecal = 1;
            Random vRandom = new Random();
            for (int vi = vDecal; vi < (vDim - vDecal); vi++)
            {
                for (int vj = vDecal; vj < (vDim - vDecal); vj++)
                {
                    vRandomValue = vRandom.Next(1, 5); // le random renvoie 1, 2, 3, 4 ou 5 -> 1 chance sur 5 d'avoir 1
                    if (vRandomValue == 1) // une chance sur 5
                        vCurrentCellTab[vi, vj] = true;
                }
            }
        }

        //affiche le contenu du tableau de cellules
        public static void showCellTab()
        {
            // écrivons une grande chaîne de caractères ; le passage à la ligne se fera par \n

            // écrivons une ligne de séparation
            string vLineToWrite = "";
            for (int vi = 0; vi < vDim; vi++)
                vLineToWrite += "-";
            vLineToWrite += "\n"; // retour chariot à la fin de la ligne de séparation

            // écrivons une ligne de cellules vivantes suivant leur position sur la ligne
            for (int vi = 1; vi < vDim; vi++)
            {
                for (int vj = 1; vj < vDim; vj++)
                {
                    if (vCurrentCellTab[vi, vj])
                        vLineToWrite += "*"; // cellule vivante
                    else
                        vLineToWrite += " "; // cellule morte
                }
                vLineToWrite += "\n"; // retour chariot à la fin de la première ligne de cellules vivantes ou mortes
            }
            Console.Write(vLineToWrite);

            // attendons un peu avant d'écrire la suite, sinon le traitement est trop rapide.
            Thread.Sleep(100);
        }

        static void Main(string[] args)
        {
            fillRandomCellTab(); // initialisation aléatoire des cellule vivantes et mortes

            int vi = 0;
            int vj = 0;
            int vNbrCellAlive = 0;
            bool vbCellAlive = false;

            int vNbrCellTabScan = 100; // nombre de fois qu'on va scanner le tableau de cellules
            int vDecal = 1;
            for (int vNbrScan = 0; vNbrScan < vNbrCellTabScan; vNbrScan++)
            {
                for (vi = vDecal; vi < (vDim - vDecal); vi++)
                    for (vj = vDecal; vj < (vDim - vDecal); vj++)
                    {
                        vbCellAlive = checkCell(vi, vj);

                        if (vbCellAlive)
                        {
                            vNewCellTab[vi, vj] = true;
                            vNbrCellAlive++;
                        }
                        else
                            vNewCellTab[vi, vj] = false;
                    }

                // On copie le futur tableau dans le courant (pour la boucle suivante)
                setNewCellTabInCurrentCellTab();
                //On affiche un tableau représentant les cellules
                showCellTab();
                // s'il n'y a plus de cellule vivante, on arrête de scanner
                if (vNbrCellAlive == 0)
                    vNbrScan = vNbrCellTabScan;
                vNbrCellAlive = 0;
            }
            Console.ReadLine(); // attends une frappe de l'utilisateur pour sortir
        }
    }
}
