using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace JobZoom.Core.Framework.DataMining
{
    public class ExportViewToMining
    {

        //static void Main(string[] args)
        //{
        //    string MainServerConnectionString = "Data Source=TRUNGHIEU-PC; Initial Catalog=JobZoom; Integrated Security=SSPI;";
        //    string TargetServerConnectionString = "Data Source=TRUNGHIEU-PC; Initial Catalog=JobZoom; Integrated Security=SSPI;";

        //    ExportJobs(MainServerConnectionString, TargetServerConnectionString);
        //}
        /// <summary>
        /// Export all profiles to view (use Pivot Transformation)
        /// </summary>
        /// <param name="MainServerConnectionString">Source Database Server</param>
        /// <param name="TargetServerConnectionString">Target Database Server</param>
        /// <param name="strJobTitles">Job Titles Array</param>
        /// <param name="strSPName">Store Procedure name</param>
        /// <param name="strPrefix">Prefix</param>
        public static void Export(string MainServerConnectionString, string TargetServerConnectionString, string[] strJobTitles = null, string strSPName = "GetPivotJob", string strPrefix = "JB")
        {
            try
            {
                if (!existsLinkedServer(TargetServerConnectionString, "JobZoomLinkedServer"))
                {
                    createLinkedServer(TargetServerConnectionString, MainServerConnectionString, "JobZoomLinkedServer");
                }
                else
                {
                    deleteLinkedServer(TargetServerConnectionString, "JobZoomLinkedServer");
                    createLinkedServer(TargetServerConnectionString, MainServerConnectionString, "JobZoomLinkedServer");
                }
                if (strJobTitles == null)
                {
                    strJobTitles = getAllJobTitles(MainServerConnectionString);
                }
                foreach (string tilte in strJobTitles)
                {
                    executeStoreProcedure(MainServerConnectionString, strSPName, tilte, strPrefix);
                    Console.WriteLine(tilte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Export all profiles to view (use Pivot Transformation)
        /// </summary>
        /// <param name="DatabaseConnectionString">Source Database Server</param>
        /// <param name="strJobTitles">Job Titles Array</param>
        /// <param name="strSPName">Store Procedure name</param>
        /// <param name="strPrefix">Prefix</param>
        public static void Export(string DatabaseConnectionString, string[] strJobTitles, string strSPName = "GetPivotJob", string strPrefix = "JB")
        {
            try
            {
                if (strJobTitles == null)
                {
                    strJobTitles = getAllJobTitles(DatabaseConnectionString);
                }
                foreach (string tilte in strJobTitles)
                {
                    executeStoreProcedure(DatabaseConnectionString, strSPName, tilte, strPrefix);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    

        #region Database Engine Query
        /// <summary>
        /// Execute procedure (Database Engine Query)
        /// </summary>
        /// <param name="DatabaseConnectionString">Database Server Connection String (Database Engine)</param>
        /// <param name="strSPName">Procedure name</param>
        /// <returns>True if exexute sucessfully and reverse</returns>
        private static bool executeStoreProcedure(string DatabaseConnectionString, string strSPName, string strJobTitle, string strPrefix)
        {
            try
            {
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(DatabaseConnectionString);

                //Open Connection
                //if (objConnection.State == ConnectionState.Closed)
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = strSPName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@JobTitle", strJobTitle);
                command.Parameters.AddWithValue("@Prefix", strPrefix);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error - executeQuery. Error Message -> " + ex.Message);
            }
        }

        /// <summary>
        /// Convert a job title name to model name
        /// </summary>
        /// <param name="JobTitle">The job title name you want to convert</param>
        /// <param name="prefix">Prefix</param>
        /// <returns>The model name</returns>
        public static string convertJobTitleNameToModelName(string JobTitle, string prefix = "PF")
        {
            return prefix + JobTitle.Replace(" ", "");
        }

        /// <summary>
        /// Get all job titles on databases
        /// </summary>
        /// <param name="DatabaseConnectionString">Database Server Connection String</param>
        /// <returns></returns>
        private static string[] getAllJobTitles(string DatabaseConnectionString)
        {
            try
            {
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(DatabaseConnectionString);

                //Open Connection
                //if (objConnection.State == ConnectionState.Closed)
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "SELECT DISTINCT JobTitle FROM [Job.Posting];";
                SqlDataReader dr = command.ExecuteReader();

                List<string> results = new List<string>();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        results.Add(dr[0].ToString());
                    }
                }
                return results.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Error - executeQuery. Error Message -> " + ex.Message);
            }
        }
        #endregion

        #region Linked Server
        /// <summary>
        /// Create linked server on Database Engine to get data from Analysis Service by MDX language
        /// </summary>
        /// <param name="TargetServerConnectionString">Database server connetion string to create linked server</param>
        /// <param name="MainServerConnectionString">Source database server connection string to get data from</param>
        /// <param name="strLinkedServerName">Linked server name</param>
        /// <returns></returns>
        public static bool createLinkedServer(string TargetServerConnectionString, string MainServerConnectionString, string strLinkedServerName = "JobZoomLinkedServer")
        {
            try
            {
                if (!existsLinkedServer(TargetServerConnectionString, strLinkedServerName))
                {
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(TargetServerConnectionString);

                    SqlConnectionStringBuilder sourceDBConnectionString = new SqlConnectionStringBuilder(MainServerConnectionString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "EXEC master.dbo.sp_addlinkedserver @server = N'" + strLinkedServerName + "', @srvproduct=N'', @provider=N'SQLNCLI', @datasrc=N'" + sourceDBConnectionString.DataSource +"', @catalog=N'" + sourceDBConnectionString.InitialCatalog + "'; " +
                                        "EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'" + strLinkedServerName + "',@useself=N'True',@locallogin=NULL,@rmtuser=NULL,@rmtpassword=NULL";
                    command.ExecuteNonQuery();
                    return existsLinkedServer(TargetServerConnectionString, strLinkedServerName);
                }
                else
                {
                    throw new Exception("Linked server existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in creating linked server - createLinkedServer. Error Message -> " + ex.Message);
            }
        }

        /// <summary>
        /// Is Linked Server exist?
        /// </summary>
        /// <param name="DatabaseConnectionString">Database Server Name (Database Engine)</param>
        /// <param name="strLinkedServerName">Linked server name to check</param>
        /// <returns>Result</returns>
        public static bool existsLinkedServer(string DatabaseConnectionString, string strLinkedServerName = "JobZoomLinkedServer")
        {
            try
            {
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(DatabaseConnectionString);

                objConnection.Open();
                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "Select 1 Where Exists (Select [SRVID] From master..sysservers Where [srvName]='" + strLinkedServerName + "');";
                if (command.ExecuteReader().Read())
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in checking linked server - existsLinkedServer. Error Message -> " + ex.Message);
            }
        }

        /// <summary>
        /// Delete linked server
        /// </summary>
        /// <param name="DatabaseConnectionString">Database Server Connection String (Database Engine)</param>
        /// <param name="strLinkedServerName">Linked server name to delete</param>
        /// <returns>Result</returns>
        public static bool deleteLinkedServer(string DatabaseConnectionString, string strLinkedServerName = "JobZoomLinkedServer")
        {
            try
            {
                if (existsLinkedServer(DatabaseConnectionString, strLinkedServerName))
                {
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(DatabaseConnectionString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "sp_dropserver '" + strLinkedServerName + "', 'droplogins';";
                    command.ExecuteNonQuery();
                    return !existsLinkedServer(DatabaseConnectionString, strLinkedServerName);
                }
                else
                {
                    throw new Exception("Linked server doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in deleting linked server - deleteLinkedServer. Error Message -> " + ex.Message);
            }

        }
        #endregion

        #region Database Engine Query
        /// <summary>
        /// Execute query (Database Engine Query)
        /// </summary>
        /// <param name="DatabaseConnectionString">Database Server Connection String (Database Engine)</param>
        /// <param name="strQuery">Query to execute</param>
        /// <returns>True if exexute sucessfully and reverse</returns>
        private static bool executeQuery(string DatabaseConnectionString, string strQuery)
        {
            try
            {
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(DatabaseConnectionString);

                //Open Connection
                //if (objConnection.State == ConnectionState.Closed)
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                objConnection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error - executeQuery " + strQuery + ". Error Message -> " + ex.Message);
            }
        }
        #endregion

    }
}
