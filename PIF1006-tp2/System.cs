/*##############################################################################
 Nom du fichier: System.cs
 Nom de l'équipe: Équipe Tertiaire
 Membre de l'équipe: Janik Lesage, Julien Boisvert, Alexis Young
###############################################################################*/


using System;


namespace PIF1006_tp2
{
    public class System
    {
        public Matrix2D A { get; private set; }
        public Matrix2D B { get; private set; }

        public System(Matrix2D a, Matrix2D b)
        {
            // Devrait rester tel quel

            A = a;
            B = b;
        }
        
        
        /// <summary>
        /// Vérifie si la matrix A est carrée et si B est une matrice avec le même nb de ligne que A et une seule colonne
        /// </summary>
        /// <returns>True si le système est valide, false sinon</returns>
        public bool IsValid()
        {
            return A.IsSquare() && A.Lines == B.Lines && B.Columns == 1;
        }

        
        /// <summary>
        /// Résout le système en utilisant la méthode de Cramer
        /// </summary>
        /// <returns>Une matrice X de même dimension que B avec les valeurs des inconnus</returns>
        public Matrix2D SolveUsingCramer()
        {
            Console.WriteLine("\nAvec Cramer");
            
            // Vérifie si le système est valide
            if (!IsValid())
            {
                Console.WriteLine("Le système n'est pas valide");
                return null;
            }

            // On vérifie si le déterminant est nul
            if(A.Determinant()==0)
            {
                Console.WriteLine("Impossible, le déterminant est nul");
                return null;
            }
            
            Matrix2D result = new Matrix2D("Result", B.Lines, B.Columns);
            
            // On calcule les inconnus avec les déterminants
            for (int i = 0; i < A.Lines; i++)
            {
                Matrix2D ai = ReplaceColumn(A, B, i);
                double detAi = ai.Determinant();
                
                result.Matrix[i, 0] = detAi / A.Determinant();
            }
            
            Console.WriteLine(result);
            
            return result;
        }
        
        
        /// <summary>
        /// Remplace une colonne d'une matrice par une autre matrice
        /// </summary>
        /// <param name="matrix"> Matrice à modifier </param>
        /// <param name="column"> Matrice à insérer </param>
        /// <param name="index"> Index de la colonne à remplacer </param>
        /// <returns> La matrice modifiée </returns>
        private Matrix2D ReplaceColumn(Matrix2D matrix, Matrix2D column, int index)
        {
            Matrix2D newMatrix = new Matrix2D("newMatrix", matrix.Lines, matrix.Columns);
            
            // Copie de la matrice
            for (int i = 0; i < matrix.Lines; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    newMatrix.Matrix[i, j] = matrix.Matrix[i, j];     
                }
            }
            
            // Remplacement de la colonne
            for (int i = 0; i < matrix.Lines; i++)
            {
                newMatrix.Matrix[i, index] = column.Matrix[i, 0];
            }

            return newMatrix;
        }
        

        /// <summary>
        ///  Résout le système en utilisant la méthode de la matrice inverse
        /// </summary>
        /// <returns> Une matrice X de même dimension que B avec les valeurs des inconnus </returns>
        public Matrix2D SolveUsingInverseMatrix()
        {
            Console.WriteLine("\nAvec Inverse");
            
            // On vérifie si le système est valide
            if (!IsValid())
            {
                Console.WriteLine("Le système n'est pas valide");
                return null;
            }
            if(A.Determinant()==0)
            {
                Console.WriteLine("Impossible, le déterminant est nul");
                return null;
            }
            
            Matrix2D inverse = A.Inverse();
            Matrix2D result = inverse * B;
            
            Console.WriteLine(result);
            
            return result;
        }

        
        /// <summary>
        /// Résout le système en utilisant la méthode de Gauss
        /// </summary>
        /// <returns> Une matrice X de même dimension que B avec les valeurs des inconnus </returns>
        public Matrix2D SolveUsingGauss()
        {
            Console.WriteLine("\nAvec Gauss");
            
            if (!IsValid())
            {
                Console.WriteLine("Le système n'est pas valide");
                return null;
            }
            
            // On copie les matrices A et B pour ne pas les modifier
            Matrix2D copieB = Copie(B);
            Matrix2D copieA = Copie(A);
            
            // On fait la méthode de Gauss
            for(int pivotLine = 0; pivotLine < A.Lines; pivotLine++)
            {
                // On trouve le pivot
                double pivot = copieA.Matrix[pivotLine, pivotLine];

                if (pivot == 0)
                {
                    // On cherche une ligne avec un pivot non nul
                    for (int line = pivotLine + 1; line < A.Lines; line++)
                    {
                        if (copieA.Matrix[line, pivotLine] != 0)
                        {
                            // On échange les lignes
                            for (int col = 0; col < A.Columns; col++)
                            {
                                (copieA.Matrix[pivotLine, col], copieA.Matrix[line, col]) = (copieA.Matrix[line, col], copieA.Matrix[pivotLine, col]);
                            }
                            
                            // On échange les lignes de B (pour avoir le même résultat)
                            (copieB.Matrix[pivotLine, 0], copieB.Matrix[line, 0]) = (copieB.Matrix[line, 0], copieB.Matrix[pivotLine, 0]);

                            pivot = copieA.Matrix[pivotLine, pivotLine];
                            break;
                        }
                    }
                }
                
                
                // On divise la ligne par le pivot pour avoir un 1
                for(int col = 0; col < A.Columns; col++)
                {
                    copieA.Matrix[pivotLine, col] /= pivot;
                }
                
                // On divise la ligne de B par le pivot pour avoir le bon résultat
                copieB.Matrix[pivotLine, 0] /= pivot;
                
                
                // On soustrait la ligne du pivot aux autres lignes pour avoir des 0
                for(int line = 0; line < A.Lines; line++)
                {
                    if (line != pivotLine)
                    {
                        double coef = copieA.Matrix[line, pivotLine];
                        
                        for(int col = 0; col < A.Columns; col++)
                        {
                            copieA.Matrix[line, col] -= coef * copieA.Matrix[pivotLine, col];
                        }
                        copieB.Matrix[line, 0] -= coef * copieB.Matrix[pivotLine, 0];
                    }
                }
            }
            
            Console.WriteLine(copieB);
            
            return copieB;
        }

        
        /// <summary>
        /// Copie une matrice
        /// </summary>
        /// <param name="matrix"> Matrice à copier </param>
        /// <returns> La matrice copiée </returns>
        private static Matrix2D Copie(Matrix2D matrix)
        {
            Matrix2D newMatrix = new Matrix2D("Results", matrix.Lines, matrix.Columns);
            
            // Copie de la matrice
            for (int i = 0; i < matrix.Lines; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    newMatrix.Matrix[i, j] = matrix.Matrix[i, j];     
                }
            }

            return newMatrix;
        }
        
        
        /// <summary>
        /// Retourne le système sous forme de string
        /// </summary>
        /// <returns> Le système sous forme de string </returns>
        public override string ToString()
        {
            string system = "";
            
            for (int i = 0; i < A.Lines; i++)
            {
                for (int j = 0; j < A.Columns; j++)
                {
                    system += A.Matrix[i, j] + "x" + (j + 1) + " ";
                }
                system += "= " + B.Matrix[i, 0] + "\n";
            }
            
            return system;
        }
        
        
        /// <summary>
        /// Résout le système en utilisant la méthode de Jacobi
        /// </summary>
        /// <param name="epsilon"> Précision </param>
        /// <returns> Une matrice X de même dimension que B avec les valeurs des inconnus </returns>
        public Matrix2D SolveUsingJacobi(double epsilon = 0.01)
        {
            // Vérification de la validité du système
            if (!IsValid() || !ValidationMethodeIterative(A))
            {
                Console.WriteLine("\nLe système n'est pas valide");
                return null;
            }
            
            // Récupération des matrices L et U
            double[,] lEtU = GetLnU(A);
            
            // On récupère la matrice D
            double[] diviseur = GetD(A);
            
            // Résultats
            double [] resultsArray = new double[A.Lines];
            double [] ancienResultsArray = new double[A.Lines];
            
            // Variables de boucle
            bool continuer = true;
            int count = 0;
            
            while (continuer)
            {
                count++; // Nombre d'itérations
                
                string affichage = "";
                
                for (int j = 0; j < A.Lines; j++)
                { 
                    resultsArray[j] = B.Matrix[j, 0]; // On ajoute le résultat de B
                    for(int i = 0; i < A.Columns; i++)
                    {
                        if(lEtU[j,i] != 0)
                        {
                            resultsArray[j] +=lEtU[j,i] * ancienResultsArray[i];
                        }
                        
                    }
                    resultsArray[j] /= diviseur[j];
                    
                    
                }
            
                affichage += "   Iteration " + count + " : \n";
                
                // On vérifie si on continue
                for (int i = 0; i < A.Lines; i++)
                {
                    continuer = false;
                    if (!ValidationEspilon(resultsArray[i], ancienResultsArray[i], epsilon))
                    {
                        continuer = true;
                    }
                    
                    // On met les résultats de l'itération précédente dans ancienResultsArray
                    ancienResultsArray[i] = resultsArray[i];
                    
                    affichage += "\tX" + i + " = " + resultsArray[i] + "\n";
                } 
                
                Console.WriteLine(affichage);
            }
            
            // On met les résultats dans une matrice
            Matrix2D result = new Matrix2D("Result", B.Lines, B.Columns);
            for (int i = 0; i < A.Lines; i++)
            {
                result.Matrix[i, 0] = resultsArray[i];
            }
            
            return result;
        }
    
        
        /// <summary>
        /// Résout le système en utilisant la méthode de Gauss-Seidel
        /// </summary>
        /// <param name="epsilon"> Précision </param>
        /// <returns> Une matrice X de même dimension que B avec les valeurs des inconnus </returns>
        public Matrix2D SolveUsingGaussSeidel(double epsilon = 0.01)
        {
            // Vérification de la validité du système
            if (!IsValid() || !ValidationMethodeIterative(A))
            {
                Console.WriteLine("\nLe système n'est pas valide");
                return null;
            }
            
            // Récupération des matrices L et U
            double[,] lEtU = GetLnU(A);
            
            // On récupère la matrice D
            double[] diviseur = GetD(A);
            
            // Résultats
            double [] resultsArray = new double[A.Lines];
            double [] ancienResultsArray = new double[A.Lines];
            
            // Variables de boucle
            bool continuer = true;
            int count = 0;
            
            while (continuer)
            {
                count++;
                
                string affichage = "";
                
                for (int j = 0; j < A.Lines; j++)
                { 
                    resultsArray[j] = B.Matrix[j, 0];
                    for(int i = 0; i < A.Columns; i++)
                    {
                        if(lEtU[j,i] != 0)
                        {
                            resultsArray[j] +=lEtU[j,i] * ancienResultsArray[i];
                        }
                        
                    }
                    resultsArray[j] /= diviseur[j];
                    
                    continuer = false;
                    if (!ValidationEspilon(resultsArray[j], ancienResultsArray[j], epsilon))
                    {
                        continuer = true;
                    }
                    ancienResultsArray[j] = resultsArray[j];
                }
            
                affichage += "   Iteration " + count + " : \n";
                
                for (int i = 0; i < A.Lines; i++)
                {
                    affichage += "\tX" + i + " = " + resultsArray[i] + "\n";
                } 
                
                Console.WriteLine(affichage);
            }
            
            // On met les résultats dans une matrice
            Matrix2D result = new Matrix2D("Result", B.Lines, B.Columns);
            for (int i = 0; i < B.Lines; i++)
            {
                result.Matrix[i, 0] = resultsArray[i];
            }
            
            return result;
        }


        /// <summary>
        /// Calcule la matrice D
        /// </summary>
        /// <param name="a"> Matrice A </param>
        /// <returns> La matrice D </returns>
        private static double[] GetD(Matrix2D a)
        {
            double[] d = new double[a.Lines];
            for (int i = 0; i < a.Lines; i++)
            {
                d[i] = a.Matrix[i, i];
            }
            
            return d;
        }

        
        /// <summary>
        /// Calcule la matrice L + U
        /// </summary>
        /// <param name="a"> Matrice A </param>
        /// <returns> La matrice L + U </returns>
        private static double[,] GetLnU(Matrix2D a)
        {
            double[,] lnu = new double[a.Lines, a.Columns];
            
            // On remplit les matrices D, L et U
            for (int i = 0; i < a.Lines; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i != j)
                    {
                        lnu[i, j] = -a.Matrix[i, j];
                    }
                }
            }
            
            return lnu;
        }
        
        /// <summary>
        /// Valide si la différence entre deux nombres est inférieure à l'epsilon
        /// </summary>
        /// <param name="nombre1"> Premier nombre </param>
        /// <param name="nombre2"> Deuxième nombre </param>
        /// <param name="epsilon"> Précision </param>
        /// <returns> True si la différence est inférieure à l'epsilon, false sinon </returns>
        private static bool ValidationEspilon(double nombre1, double nombre2, double epsilon)
        {
            return Math.Abs(nombre1 - nombre2) < epsilon;
        }

        
        /// <summary>
        /// Valide si la matrice A est strictement diagonalement dominante et irréductiblement diagonalement dominante
        /// </summary>
        /// <returns> True si la matrice est strictement diagonalement dominante et irréductiblement diagonalement dominante, false sinon </returns>
        private static bool ValidationMethodeIterative(Matrix2D a)
        {
            bool strictement = true;
            bool irred = true;
            
            // On vérifie si la matrice est strictement diagonalement dominante
            for (int i = 0; i < a.Lines; i++)
            {
                double somme = 0;
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i != j)
                    {
                        somme += Math.Abs(a.Matrix[i, j]);
                    }
                }

                if (Math.Abs(a.Matrix[i, i]) <= somme)
                {
                    strictement = false;
                }
            }
            
            // On vérifie si la matrice est irréductiblement diagonalement dominante
            for (int i = 0; i < a.Lines; i++)
            {
                double somme = 0;
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i != j)
                        somme += Math.Abs(a.Matrix[i, j]);
                    
                }

                if (Math.Abs(a.Matrix[i, i]) < somme)
                    irred = false;
                
            }
            
            if (!strictement)
                Console.WriteLine("La matrice n'est pas strictement diagonalement dominante");
            
            if (!irred)
                Console.WriteLine("La matrice n'est pas irréductiblement diagonalement dominante");
            
            
            // On retourne true si la matrice est strictement diagonalement dominante et irréductiblement diagonalement dominante
            return strictement && irred;
            
        }
        
    }
}
