/*##############################################################################
 Nom du fichier: Matrix.cs
 Nom de l'équipe: Équipe Tertiaire
 Membre de l'équipe: Janik Lesage, Julien Boisvert, Alexis Young
###############################################################################*/

using System;

using Newtonsoft.Json;

namespace PIF1006_tp2
{
    public class Matrix2D
    {
        public string Name { get; private set; }
        
        [JsonProperty]// Permet la désérialisation même si le set est privé
        public double[,] Matrix { get; private set; }
        
        public int Lines => Matrix.GetLength(0);
        public int Columns => Matrix.GetLength(1);

        /// <summary>
        ///  Constructeur de matrice
        /// </summary>
        /// <param name="name">Nom de la matrice</param>
        /// <param name="lines">Nombre de lignes</param>
        /// <param name="columns">Nombre de colonnes</param>
        public Matrix2D(string name, int lines, int columns)
        {
            // Devrait rester tel quel

            Matrix = new double[lines, columns];
            Name = name;
            
        }

        /// <summary>
        /// Transpose la matrice
        /// </summary>
        /// <returns>Matrice transposée</returns>
        public Matrix2D Transpose()
        {
            // On crée une nouvelle matrice avec les lignes et colonnes inversées
            Matrix2D transpose = new Matrix2D("Transpose", Columns, Lines);
            transpose.Matrix = new double[Columns, Lines];
            
            // On transpose la matrice
            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    transpose.Matrix[j, i] = Matrix[i, j];
                }
            }
            
            return transpose;
        }

        
        /// <summary>
        /// Vérifie si la matrice est carrée
        /// </summary>
        /// <returns>True si la matrice est carrée, false sinon</returns>
        public bool IsSquare()
        {
            if(Columns != Lines)
                Console.WriteLine("La matrice n'est pas carrée");
            
            return Lines == Columns;
        }

        
        /// <summary>
        /// Calcule le déterminant de la matrice
        /// </summary>
        /// <returns> Double représentant le déterminant</returns>
        public double Determinant()
        {
            
            if (IsSquare())
            {
                return Det(Matrix);
            }
            
            Console.WriteLine("La matrice n'est pas carrée"); 
            return 0;
            
        }

        
        /// <summary>
        /// Calcule le déterminant d'une matrice carrée de façon récursive
        /// </summary>
        /// <param name="matrix">Matrice à calculer</param>
        /// <returns>Double représentant le déterminant</returns>
        private double Det(double[,] matrix)
        {
            double det = 0;
            int ligne = matrix.GetLength(0);
            int colonne = matrix.GetLength(1);
            
            
            if (colonne == 2)
            {
                det = matrix[0,0] * matrix[1,1] - matrix[0,1] * matrix[1,0];
            }

            // Si la matrice à plus de 2 colonnes
            else
            {
                for (int i = 0; i < ligne; i++)
                {
                    if (i % 2 == 0)
                    {
                        det+=matrix[0,i] * Det(GetReduce(ligne, 0, i, matrix));    
                    }
                    else
                    {
                        det+=matrix[0,i] * -(Det(GetReduce(ligne, 0, i, matrix)));   
                    }
                    
                }
            }
            
            return det;
            
        }
        
        
        /// <summary>
        /// Retourne une matrice réduite avec la ligne et la colonne en moins
        /// </summary>
        /// <param name="line"> Nombre de lignes</param>
        /// <param name="lineChange">Ligne à changer</param>
        /// <param name="columnChange"> Colonne à changer</param>
        /// <param name="matrice"> Matrice à réduire</param>
        /// <returns>Matrice réduite</returns>
        private double[,] GetReduce(int line, int lineChange , int columnChange, double[,] matrice)
        {
            
            int minorLines = line - 1;
            
            double[,] minor = new double[minorLines, minorLines];

            for (int i = 0; i < minorLines; i++)
            {
                for (int j = 0; j < minorLines; j++)
                {
                    // Si on est avant la ligne à changer
                    if (i < lineChange)
                    {
                        // Si on est avant la colonne à changer
                        if (j < columnChange)
                        {
                            minor[i, j] = matrice[i, j];
                        }
                        else
                        {
                            minor[i, j] = matrice[i, j + 1];
                        }   
                    }
                    else
                    {
                        // Si on est avant la colonne à changer
                        if (j < columnChange)
                        {
                            minor[i, j] = matrice[i + 1, j];
                        }
                        else
                        {
                            minor[i, j] = matrice[i + 1, j + 1];
                        }   
                    }
                }
            }

            return minor;
        }
        

        /// <summary>
        /// Calcule la comatrice de la matrice
        /// </summary>
        /// <returns> Matrice comatrice</returns>
        public Matrix2D Comatrix()
        {
            
            if (!IsSquare())
            {
                Console.WriteLine("La matrice n'est pas carrée");
                return null;
            }

            Matrix2D comatrix = new Matrix2D("Comatrix", Lines, Columns);
            
            comatrix.Matrix = new double[Lines, Columns];
            int pair = 0;
            
            // Si la matrice est de 2x2
            if(Lines == 2)
            {
                comatrix.Matrix[0, 0] = Matrix[1, 1];
                comatrix.Matrix[0, 1] = -Matrix[1, 0];
                comatrix.Matrix[1, 0] = -Matrix[0, 1];
                comatrix.Matrix[1, 1] = Matrix[0, 0];
                
                return comatrix;
            }
            
            // Sinon on calcule la comatrice
            for(int i = 0; i < Lines; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    if (pair++ % 2 == 0)
                    {
                        comatrix.Matrix[i, j] = Det(GetReduce(Lines, i, j, Matrix));
                    }
                    else
                    {
                        comatrix.Matrix[i, j] = -Det(GetReduce(Lines, i, j, Matrix));
                    }
                }
            }
            
            return comatrix;
        }

        
        /// <summary>
        ///  Calcule l'inverse de la matrice
        /// </summary>
        /// <returns>Matrice inverse</returns>
        public Matrix2D Inverse()
        {
            // Si la matrice n'est pas carrée
            if (!IsSquare())
            {
                Console.WriteLine("La matrice n'est pas carrée");
                return null;
            }
            
            // Si le déterminant est nul
            if (Determinant() ==0)
            {
                Console.WriteLine("Le déterminant est nul");
                return null;
            }
            
            Matrix2D inverse = new Matrix2D("Inverse", Lines, Columns);
            inverse.Matrix = new double[Lines, Columns];
            
            // On récupère le déterminant
            double det = Determinant();
            
            // On récupère la comatrice et on la transpose
            Matrix2D comatrix = Comatrix();
            comatrix = comatrix.Transpose();
            
            
            for(int i = 0; i < Lines; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    inverse.Matrix[i, j] = comatrix.Matrix[i, j] / det;
                }
            }
            
            return inverse;
        }

        
        /// <summary>
        /// Affiche la matrice
        /// </summary>
        /// <returns>String représentant la matrice</returns>
        public override string ToString()
        {
            string matrix = Name + ":\n";
            for (int i = 0; i < Lines; i++)
            {
                matrix += "| ";
                for (int j = 0; j < Columns; j++)
                {
                    matrix += Matrix[i, j] + " ";
                }
                matrix += "|\n";
            }

            return matrix;
        }
        
        
        /// <summary>
        /// Opérateur de multiplication de matrice
        /// </summary>
        /// <param name="a">Matrice a multiplier</param>
        /// <param name="b">Matrice a multiplier</param>
        /// <returns>Matrice résultante</returns>
        /// <exception cref="Exception">Si les matrices ne sont pas de la bonne taille</exception>
        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        { 
            // Vérification de la taille des matrices
            if (a.Columns != b.Lines)
            {
                throw new Exception("Missmatching size");
            }
            Matrix2D results = new Matrix2D("Results",a.Lines, b.Columns);
            
            // Multiplication des matrices
            for (int i = 0; i < a.Lines; i++)
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    results.Matrix[i, j] = 0;
                    for (int k = 0; k < a.Lines; k++)
                        results.Matrix[i, j] += a.Matrix[i, k] * b.Matrix[k, j];
                }
            }
            return results;
        }

    }
}
