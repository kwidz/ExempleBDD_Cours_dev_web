using System;
using System.Data;
using System.Data.SqlClient;

namespace exemplesBaseDeDonnees
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=info-web02;Initial Catalog=CGI_PROF;User ID=geoffrey.glangine;Password=test";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            Console.WriteLine("Connection Open  !");
            SimpleRead(cnn);
       
            int numberOfRecords =GetNumberOfRecords(cnn);

            Console.WriteLine();
            Console.WriteLine("Number of Records: {0}", numberOfRecords);
            Console.WriteLine("requete forgée");
            forgedRequest(cnn);
            Console.WriteLine("requete préparée");
            preparedRequest(cnn);

            cnn.Close();


        }

        public static void SimpleRead(SqlConnection conn)
        {
            // déclaration d'un dataReader
            SqlDataReader rdr = null;
            // create a command object
            SqlCommand cmd = new SqlCommand(
                "select * from ville", conn);
            try
            {
                // execution de la requete
                rdr = cmd.ExecuteReader();
                // affichage des headers
                Console.WriteLine(
"Code postal             ville                id");
                Console.WriteLine(
"------------             ------------        ------------");
                while (rdr.Read())
                {
                    // récupération des résultats pour chaque colonne
                    string cp = (string)rdr["ville_code_postal"];
                    string nom = (string)rdr["ville_nom"];
                    string identifiant = rdr["ville_id"].ToString();

                    // affichage des résultats
                    Console.Write("{0,-25}", cp);
                    Console.Write("{0,-20}", nom);
                    Console.Write("{0,-25}", identifiant);
                    Console.WriteLine();
                }
            }
            finally
            {
                // 3. fermeture du reader
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        public static void Insertdata(SqlConnection conn)
        {
            try
            {
                string insertString = @"
                 insert into ville
                 (ville_nom, ville_code_postal)
                 values ('maiche', '25120')";

                SqlCommand cmd = new SqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
           
        }

        public static int GetNumberOfRecords(SqlConnection conn)
        {
            int count = -1;

            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) from ville", conn);
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return count;
        }

        public static void forgedRequest(SqlConnection conn)
        {
            SqlDataReader rdr = null;
            string inputCity = "SEMUY";

            SqlCommand cmd = new SqlCommand(
            "select * from ville where ville_nom = '" + inputCity + "'",conn);
            try
            {
                // execution de la requete
                rdr = cmd.ExecuteReader();
                // affichage des headers
                Console.WriteLine(
"Code postal             ville                id");
                Console.WriteLine(
"------------             ------------        ------------");
                while (rdr.Read())
                {
                    // récupération des caleurs pour les colonnes souhaitées
                    string cp = (string)rdr["ville_code_postal"];
                    string nom = (string)rdr["ville_nom"];
                    string identifiant = rdr["ville_id"].ToString();

                    // affichage des résultats
                    Console.Write("{0,-25}", cp);
                    Console.Write("{0,-20}", nom);
                    Console.Write("{0,-25}", identifiant);
                    Console.WriteLine();
                }
            }
            finally
            {
                // 3. fermeture du reader
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }

        public static void preparedRequest(SqlConnection conn)
        {
            SqlDataReader rdr = null;
            string inputCity = "SEMUY";
            try
            {

                SqlCommand cmd = new SqlCommand(
                    "select * from ville where ville_nom = @City", conn);

                // 2. définition des paramètres
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@City";
                param.Value = inputCity;

                // 3. ajout des nouveaux paramètres
                cmd.Parameters.Add(param);

                // get data stream
                rdr = cmd.ExecuteReader();

                // affichage de chaque enregistrement
                while (rdr.Read())
                {
                    Console.WriteLine("{0}, {1}",
                        rdr["ville_nom"],
                        rdr["ville_code_postal"]);
                }
            }
            finally
            {
                // fereture reader
                if (rdr != null)
                {
                    rdr.Close();
                }

            }
        

        }
        public static void callStoredProcedure(SqlConnection conn)
        {

            SqlDataReader rdr = null;

            string userId = "1234";

            Console.WriteLine("\nhistorique des commande de l'utilisateur\n");

            try
            {

                SqlCommand cmd = new SqlCommand(
                    "historiqueUtilisateur", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(
                    new SqlParameter("@userID", userId));

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(
                        "Product: {0,-35} Total: {1,2}",
                        rdr["ProductName"],
                        rdr["Total"]);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
        }


    }
}
