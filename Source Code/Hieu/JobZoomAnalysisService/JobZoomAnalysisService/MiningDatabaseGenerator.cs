using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Net;

namespace JobZoomAnalysisService
{
    class MiningDatabaseGenerator
    {
        static void Main(string[] args)
        {
            BuildMiningDatabase();
            Console.ReadLine();
        }

        #region String Encryption
        /// <summary>
        ///     String encode before mining (Characters: .,;'`:/\*|?"&%$!+=()[]{}<>)
        /// </summary>
        /// <param name="input">Input string to encode</param>
        /// <returns>Encoded string</returns>
        private static string StringEncode(string input)
        {
            string[] source = {".", ",", ";", "'", "`", ":", "/", @"\", "*", "|", "?", "\"", "&", "%", "$", "!", "+", "=", "(", ")", "[", "]", "{", "}", "<", ">" };
            string[] target = { "_x002E_", "_x002C_", "_x003B_", "_x0027_", "_x0060_", "_x003A_", "_x002F_", "_x005C_", "_x002A_", "_x007C_", "_x003F_", "_x0022_", "_x0026_", "_x0025_", "_x0024_", "_x0021_", "_x002B_", "_x003D_", "_x0028_", "_x0029_", "_x005B_", "_x005D_", "_x007B_", "_x007D_", "_x003C_", "_x003E_" };
            for (int i = 0; i < source.Length; i++)
            {
                input = input.Replace(source[i], target[i]);
            }
            return input;
        }

        /// <summary>
        ///     String decode after mining
        /// </summary>
        /// <param name="input">Input string to decode</param>
        /// <returns>Decoded string</returns>
        private static string StringDecode(string input)
        {
            string[] target = { ".", ",", ";", "'", "`", ":", "/", @"\", "*", "|", "?", "\"", "&", "%", "$", "!", "+", "=", "(", ")", "[", "]", "{", "}", "<", ">" };
            string[] source = { "_x002E_", "_x002C_", "_x003B_", "_x0027_", "_x0060_", "_x003A_", "_x002F_", "_x005C_", "_x002A_", "_x007C_", "_x003F_", "_x0022_", "_x0026_", "_x0025_", "_x0024_", "_x0021_", "_x002B_", "_x003D_", "_x0028_", "_x0029_", "_x005B_", "_x005D_", "_x007B_", "_x007D_", "_x003C_", "_x003E_" };
            for (int i = 0; i < source.Length; i++)
            {
                input = input.Replace(source[i], target[i]);
            }
            return input;
        }
        #endregion

        #region Mining Database Generation.

        private static void BuildMiningDatabase()
        {
            string strPrefix = "Pivot";
            string strDBServerName = "TRUNGHIEU-PC"; // Database Engine
            string strASServerName = "TRUNGHIEU-PC"; //Analysis service
            string strProviderName = "msolap"; //Microsoft OLE DB Provider for Analysis Services 10.0
            string strDBName = "JobZoom"; // Database (Database Engine)
            string strMiningDBName = "Job Zoom Mining"; //Mining database name (Analysis Service)
            string strMiningDataSourceName = "Data Source"; //Mining datasource name (Analysis Service)
            string strMiningDataSourceViewName = "Data Source View"; //Mining datasource view name (Analysis Service)

            string[] strFactTableNames = getAllMiningTableNames(strDBServerName, strDBName, strPrefix); //tables in datasource view to mining
            
            string[,] strTableNamesAndKeys = { { "PivotProfile", "ProfileBasicId", "PivotProfile", "ProfileBasicId" }, };

            int intDimensionTableCount = 0;
            Server objServer = new Server();
            Database objDatabase = new Database();
            RelationalDataSource objDataSource = new RelationalDataSource();
            DataSourceView objDataSourceView = new DataSourceView();
            DataSet objDataSet = new DataSet();
            Dimension[] objDimensions = new Dimension[intDimensionTableCount];
            MiningStructure[] objMiningStructures = new MiningStructure[strFactTableNames.Length];


            Console.WriteLine("Mining creation process started.");
            Console.WriteLine("");

            Console.WriteLine("Step 1. Connecting to the Analysis Services.");
            Console.WriteLine("Step 1. Started!");
            objServer = (Server) ConnectAnalysisServices(strASServerName, strProviderName);
            Console.WriteLine("Step 1. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 2. Creating a Database.");
            Console.WriteLine("Step 2. Started!");
            objDatabase = (Database)CreateDatabase(objServer, strMiningDBName);
            strMiningDBName = objDatabase.Name;
            Console.WriteLine("Step 2. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 3. Creating a DataSource.");
            Console.WriteLine("Step 3. Started!");
            objDataSource = (RelationalDataSource) CreateDataSource(objServer, objDatabase, strMiningDataSourceName, strDBServerName, strDBName);
            Console.WriteLine("Step 3. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 4. Creating a DataSourceView.");
            Console.WriteLine("Step 4. Started!");
            //objDataSet = (DataSet)GenerateDWSchema(strDBServerName, strDBName, strFactTableName, strTableNamesAndKeys, intDimensionTableCount);
            objDataSet = (DataSet)GenerateDWSchema(strDBServerName, strDBName, strPrefix); //Get all mining views
            objDataSourceView = (DataSourceView)CreateDataSourceView(objDatabase, objDataSource, objDataSet, strMiningDataSourceViewName);
            Console.WriteLine("Step 4. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 5. Createing Mining Structures [with Decision Tree Algorithms]");
            Console.WriteLine("Step 5. Started!");
            objMiningStructures = (MiningStructure[])CreateMiningStructures(objDatabase, objDataSourceView, strFactTableNames, new DecisionTreeAlgorithmParameters());
            Console.WriteLine("Step 5. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 6. Export mining data to JobZoom Database (Database Engine)");
            Console.WriteLine("Step 6. Started!");
            
            Console.WriteLine("Preparing... Put website to maintenance mode");
            //EXEC WEB SITE MAINTENANCE SERVICE METHOD

            Console.WriteLine("Preparing... Cleaning DecisionTreeNode and DecisionTreeNodeDistribution");
            Console.WriteLine("\nStep 6. Finished!");
            Console.WriteLine("");
            exportMiningDataToDB(strDBServerName, strDBName, strASServerName, strFactTableNames, strPrefix);
            Console.WriteLine("Export completed! Release website to continuing for using");
            //WEBSITE CAN CONTINUE FOR USING
            Console.WriteLine("Process Full...");
            //objDatabase.Process(ProcessType.ProcessFull);
            Console.WriteLine("Analysis Service Database created successfully.");

            //Console.WriteLine("Step 7. Removing Analysis Database");
            //Console.WriteLine("Step 7. Started!");
            //Console.WriteLine(deleteDatabase(objServer, objDatabase.Name));
            //Console.WriteLine("Removing Analysis Database completely ...");
            //Console.WriteLine("\nStep 7. Finished!");

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        #region Connecting to the Analysis Services.
        /// <summary>
        ///     Connecting to the Analysis Services.
        /// </summary>
        /// <param name="strASServerName">Analysis Service Server Name.</param>
        /// <param name="strProviderName">Provider Name.</param>
        /// <returns>Database Server instance.</returns>
        private static object ConnectAnalysisServices(string strASServerName, string strProviderName)
        {
            try
            {
                Console.WriteLine("Connecting to the Analysis Services ...");

                Server objServer = new Server();
                string strConnection = "Data Source=" + strASServerName + ";Provider=" + strProviderName + ";";
                //Disconnect from current connection if it's currently connected.
                if (objServer.Connected)
                    objServer.Disconnect();
                else
                    objServer.Connect(strConnection);

                return objServer;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Connecting to the Analysis Services. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion Connecting to the Analysis Services.

        #region Manage Databases in Analysis Service.
        /// <summary>
        /// Creating a Database in Analysis service
        /// </summary>
        /// <param name="objServer">Analysis Service Connection Instance</param>
        /// <param name="strASDBName">Database name in analysis service to create</param>
        /// <returns>Analysis Service Database instance.</returns>
        private static object CreateDatabase(Server objServer, string strASDBName)
        {
            try
            {
                Console.WriteLine("Creating a Analysis Database ...");

                Database objDatabase = new Database();
                //Add Database to the Analysis Services.
                objDatabase = objServer.Databases.Add(objServer.Databases.GetNewName(strASDBName));
                //Save Database to the Analysis Services.
                objDatabase.Update();                

                return objDatabase;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Database. Error Message -> " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Delete a database in Analysis service
        /// </summary>
        /// <param name="objServer">Analysis Service Connection Instance</param>
        /// <param name="strASDBName">Database name in analysis service to delete</param>
        /// <returns>Result</returns>
        private static bool deleteDatabase(Server objServer, string strASDBName)
        {
            try
            {
                objServer.Databases.GetByName(strASDBName).Drop();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Database. Error Message -> " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Get a database instance in analysis service 
        /// </summary>
        /// <param name="objServer">Analysis Service Connection Instance</param>
        /// <param name="strASDBName">Database name in analysis service to create</param>
        /// <returns>Database instance in analysis service</returns>
        private static object GetDatabase(Server objServer, string strASDBName)
        {
            try
            {
                Database objDatabase = new Database();
                //Add Database to the Analysis Services.
                objDatabase = objServer.Databases.GetByName(strASDBName);
                //Save Database to the Analysis Services.
                objDatabase.Update();

                return objDatabase;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Database. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion Manage Databases in Analysis Service.

        #region Manage DataSources in Analysis Service
        /// <summary>
        /// Creating a DataSource in Analysis Service
        /// </summary>
        /// <param name="objServer">Analysis Service Connection Instance</param>
        /// <param name="objDatabase">Database instance in Analysis Service</param>
        /// <param name="strMiningDataSourceName">Mining DataSource Name to create</param>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database's name in Database engine</param>
        /// <returns>Analysis Service DataSource instance.</returns>
        private static object CreateDataSource(Server objServer, Database objDatabase, string strMiningDataSourceName, string strDBServerName, string strDBName)
        {
            try
            {
                Console.WriteLine("Creating a DataSource ...");
                RelationalDataSource objDataSource = new RelationalDataSource();
                //Add Data Source to the Database.
                objDataSource = objDatabase.DataSources.Add(objServer.Databases.GetNewName(strMiningDataSourceName));                
                objDataSource.ConnectionString = "Provider=SQLNCLI11.1; Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=SSPI;";
                objDataSource.Update();

                return objDataSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSource. Error Message -> " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get a DataSource in Analysis Service
        /// </summary>
        /// <param name="objServer">Analysis Service Connection Instance</param>
        /// <param name="objDatabase">Database instance in Analysis Service</param>
        /// <param name="strMiningDataSourceName">Mining DataSource Name to get</param>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database's name in Database engine</param>
        /// <returns>Analysis Service Datasource instance</returns>
        private static object GetDataSource(Server objServer, Database objDatabase, string strMiningDataSourceName, string strDBServerName, string strDBName)
        {
            try
            {
                Console.WriteLine("Creating a DataSource ...");
                DataSource objDataSource;
                //Add Data Source to the Database.
                objDataSource = objDatabase.DataSources.GetByName(strMiningDataSourceName);
                objDataSource.ConnectionString = "Provider=SQLNCLI11.1; Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=SSPI;";
                objDataSource.Update();

                return objDataSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSource. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion Manage DataSources in Analysis Service

        #region Manage DataSourceViews in Analysis Service
        /// <summary>
        /// Creating a DataSourceView.
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strFactTableName">FactTable Name</param>
        /// <param name="strTableNamesAndKeys">Array of TableNames and Keys.</param>
        /// <param name="intDimensionTableCount">Dimension Table Count.</param>
        /// <returns>DataSet instance.</returns>
        private static object GenerateDWSchema(string strDBServerName, string strDBName, string strFactTableName, string[,] strTableNamesAndKeys, int intDimensionTableCount)
        {
            try
            {
                Console.WriteLine("Creating a DataSourceView ...");
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);
                objConnection.Open();
                DataSet objDataSet = new DataSet();
                //Add FactTable in DataSet.
                objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, strFactTableName);

                //Add table in DataSet and Relation between them.
                for (int i = 0; i < intDimensionTableCount; i++)
                {
                    //Retrieve table's schema and assign the table's schema to the DataSet.
                    //Add primary key to the schema according to the primary key in the tables.
                    objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, strTableNamesAndKeys[i, 0]);
                    //objDataSet = (DataSet)AddDataTableRelation(objDataSet, strTableNamesAndKeys[i, 0], strTableNamesAndKeys[i, 1], strTableNamesAndKeys[i, 2], strTableNamesAndKeys[i, 3]);
                }

                return objDataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSourceView - GenerateDWSchema. Error Message -> " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Creating a DataSourceView.
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strPrefix">Table with this prefix will be added to DataSource View</param>
        /// <returns>Dataset instance</returns>
        private static object GenerateDWSchema(string strDBServerName, string strDBName, string strPrefix = "Pivot")
        {
            try
            {
                Console.WriteLine("Creating a DataSourceView ...");
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);
                objConnection.Open();
                DataSet objDataSet = new DataSet();

                //Fill all tables begin with prefix to dataset
                string[] miningTables = getAllMiningTableNames(strDBServerName, strDBName, strPrefix);
                foreach (string miningTable in miningTables)
                {
                    objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, miningTable);
                }
                return objDataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSourceView - GenerateDWSchema. Error Message -> " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Fill the DataSet with DataTables.
        /// </summary>
        /// <param name="objConnection">Database Engine Connection instance.</param>
        /// <param name="objDataSet">DataSet instance.</param>
        /// <param name="strTableName">TableName.</param>
        /// <returns>DataSet instance.</returns>
        private static object FillDataSet(SqlConnection objConnection, DataSet objDataSet, string strTableName)
        {
            try
            {
                string strCommand = "Select * from " + strTableName;
                SqlDataAdapter objEmpData = new SqlDataAdapter(strCommand, objConnection);
                objEmpData.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                objEmpData.FillSchema(objDataSet, SchemaType.Source, strTableName);

                return objDataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSourceView - FillDataSet. Error Message -> " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Add relations between DataTables of DataSet.
        /// </summary>
        /// <param name="objDataSet">DataSet instance.</param>
        /// <param name="strParentTableName">Parent Table Name (Dimension Table).</param>
        /// <param name="strParentTableKey">Parent Table Key.</param>
        /// <param name="strChildTableName">Child Table Name (Fact Table).</param>
        /// <param name="strChildTableKey">Child Table Key.</param>
        /// <returns>DataSet instance.</returns>
        private static object AddDataTableRelation(DataSet objDataSet, string strParentTableName, string strParentTableKey, string strChildTableName, string strChildTableKey)
        {
            try
            {
                objDataSet.Relations.Add(strChildTableName + "_" + strParentTableName + "_FK", objDataSet.Tables[strParentTableName].Columns[strParentTableKey], objDataSet.Tables[strChildTableName].Columns[strChildTableKey]);

                return objDataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSourceView - AddDataTableRelation. Error Message -> " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Creating a DataSourceView.
        /// </summary>
        /// <param name="objDatabase">Analysis Service Database intance</param>
        /// <param name="objDataSource">Analysis Service DataSource instance</param>
        /// <param name="objDataSet">Dataset</param>
        /// <param name="strMiningDataSourceViewName">Mining DataSourceView Name.</param>
        /// <returns>DataSourceView instance.</returns>
        private static object CreateDataSourceView(Database objDatabase, RelationalDataSource objDataSource, DataSet objDataSet, string strMiningDataSourceViewName)
        {
            try
            {
                DataSourceView objDataSourceView = new DataSourceView();
                //Add Data Source View to the Database.
                objDataSourceView = objDatabase.DataSourceViews.Add(objDatabase.DataSourceViews.GetNewName(strMiningDataSourceViewName));
                objDataSourceView.DataSourceID = objDataSource.ID;
                objDataSourceView.Schema = objDataSet;
                objDataSourceView.Update();

                return objDataSourceView;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a DataSourceView - CreateDataSourceView. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion Manage DataSourceViews in Analysis Service

        #region Mining structure Generation.
        /// <summary>
        ///     Create Mining Structures in Analysis Service
        /// </summary>
        /// <param name="objDatabase">Analysis Service Database instance</param>
        /// <param name="objDataSourceView">Analysis Service DataSourceView instance</param>
        /// <param name="strCaseTableNames">Array of mining tables</param>
        /// <returns>Array of created Mining Structures</returns>
        private static object[] CreateMiningStructures(Database objDatabase, DataSourceView objDataSourceView, string[] strCaseTableNames, DecisionTreeAlgorithmParameters dtParams)
        {
            MiningStructure[] miningStructures = new MiningStructure[strCaseTableNames.Length];
            try
            {
                for (int i = 0; i < strCaseTableNames.Length; i++)
                {
                    miningStructures[i] = (MiningStructure)GenerateMiningStructure(objDatabase, objDataSourceView, strCaseTableNames[i], dtParams);
                }
                return miningStructures;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Mining structure - GenerateMiningStructure. Error Message -> " + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     Generate a mining structure and full process it
        /// </summary>
        /// <param name="objDatabase">Analysis Service Database instance</param>
        /// <param name="objDataSourceView">Analysis Service DataSourceView instance</param>
        /// <param name="strCaseTableName">Mining table name</param>
        /// <returns>Mining structure</returns>
        private static object GenerateMiningStructure(Database objDatabase, DataSourceView objDataSourceView, string strCaseTableName, DecisionTreeAlgorithmParameters dtParams)
        {
            try
            {
                
                MiningStructure objMiningStructure = new MiningStructure();
                objMiningStructure = objDatabase.MiningStructures.Add(objDatabase.MiningStructures.GetNewName(strCaseTableName));
                objMiningStructure.HoldoutMaxPercent = dtParams.HoldoutMaxPercent; // Percent for testing
                objMiningStructure.Source = new DataSourceViewBinding(objDataSourceView.ID);
                objMiningStructure.CaseTableName = strCaseTableName;

                foreach (string name in getAllColumnName(objDataSourceView, strCaseTableName))
                {
                    string colName = StringEncode(name);
                    ScalarMiningStructureColumn column = new ScalarMiningStructureColumn(colName, colName);
                    switch (colName)
                    {
                        case "ID":
                            // ProfileBasicId column
                            column.Type = MiningStructureColumnTypes.Long;
                            column.Content = MiningStructureColumnContents.Key;
                            column.IsKey = true;
                            // Add the column to the mining structure
                            break;
                        case "ProfileBasicId":                            
                        case "JobPostingId":
                        case "UserId":
                        case "JobTitle":
                        case "CompanyId":
                        case "CompanyName":
                            column.Type = MiningStructureColumnTypes.Text;
                            column.Content = MiningStructureColumnContents.Discrete;
                            break;
                        case "IsApproved":
                        default:
                            column.Type = MiningStructureColumnTypes.Boolean;
                            column.Content = MiningStructureColumnContents.Discrete;
                            break;
                    }
                    // Add data binding to the column
                    //ProfileBasicId.KeyColumns.Add(strCaseTableName, "ProfileBasicId", OleDbType.WChar);
                    column.KeyColumns.Add(strCaseTableName, name);
                    // Add the column to the mining structure
                    objMiningStructure.Columns.Add(column);
                }

                MiningModel objMiningModel = objMiningStructure.CreateMiningModel(true, strCaseTableName);
                //MiningModel objMiningModel = objMiningStructure.MiningModels.Add(objMiningStructure.MiningModels.GetNewName(strMiningStructureName));
                objMiningModel.Algorithm = MiningModelAlgorithms.MicrosoftDecisionTrees;
                objMiningModel.AllowDrillThrough = true;
                objMiningModel.AlgorithmParameters.Add("SCORE_METHOD", dtParams.SCORE_METHOD);
                objMiningModel.AlgorithmParameters.Add("COMPLEXITY_PENALTY", dtParams.COMPLEXITY_PENALTY);
                objMiningModel.AlgorithmParameters.Add("SPLIT_METHOD", dtParams.SPLIT_METHOD); 
                objMiningModel.AlgorithmParameters.Add("MAXIMUM_INPUT_ATTRIBUTES", dtParams.MAXIMUM_INPUT_ATTRIBUTES);
                objMiningModel.AlgorithmParameters.Add("MAXIMUM_OUTPUT_ATTRIBUTES", dtParams.MAXIMUM_OUTPUT_ATTRIBUTES);
                objMiningModel.AlgorithmParameters.Add("MINIMUM_SUPPORT", dtParams.MINIMUM_SUPPORT); 

                int i = 0;
                foreach(MiningModelColumn col in objMiningModel.Columns)
                {
                    switch (col.Name)
                    {
                        case "IsApproved":
                            objMiningModel.Columns[i].Usage = "Predict";
                            break;
                        case "ID":
                            objMiningModel.Columns[i].Usage = "Key";
                            break;
                        default:
                            objMiningModel.Columns[i].Usage = "Input";
                            break;
                    }
                    ++i;
                }
                //objMiningModel.Update(UpdateOptions.ExpandFull);
                objMiningStructure.Update(UpdateOptions.ExpandFull);
                Console.WriteLine("Processing mining model " + objMiningStructure.Name + "...");
                objMiningModel.Process(ProcessType.ProcessFull);
                Console.WriteLine("Process " + objMiningStructure.Name + " finished!");
                return objMiningStructure;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Mining structure - GenerateMiningStructure. Error Message -> " + ex.Message);
                return null;
            }
        
        }

        /// <summary>
        /// Get all table column names
        /// </summary>
        /// <param name="objDataSourceView">Analysis Serivce Datasource View</param>
        /// <param name="tableName">Table's name to get its column names</param>
        /// <returns>Array of column names</returns>
        private static string[] getAllColumnName(DataSourceView objDataSourceView, string tableName)
        {
            
            DataTable allCols = objDataSourceView.Schema.Tables[tableName];

            string[] columnNames = new string[allCols.Columns.Count];
            int index = 0;
            foreach (DataColumn col in allCols.Columns)
            {
                columnNames.SetValue(col.ColumnName, index++);
            }
            
            return columnNames;
        }

        /// <summary>
        /// Get all table columns name
        /// </summary>
        /// <param name="strDBServerName">Database Server (Database Engine)</param>
        /// <param name="strDBName">Database name (Database Engine)</param>
        /// <param name="tableName">Table's name to get its column names</param>
        /// <returns>Array of column names</returns>
        private static string[] getAllColumnName(string strDBServerName, string strDBName, string tableName)
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = '" + tableName + "' ";
                DataTable columns = new DataTable("Tables");
                columns.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
                if (columns != null)
                {
                    string[] columnNames = new string[columns.Rows.Count];
                    int index = 0;
                    foreach (DataRow row in columns.Rows)
                    {
                        columnNames.SetValue(row[0].ToString(), index++);
                    }
                    return columnNames;
                }
                else
                {
                    Console.WriteLine(tableName + "doesn't exist!");
                    return null;
                }         
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in getting all mining tables - getAllMiningTableName. Error Message -> " + ex.Message);
                return null;
            }            
        }

        /// <summary>
        /// Get all mining table names
        /// </summary>
        /// <param name="dsv">DataSource View (Analysis Service)</param>
        /// <param name="strPrefix">Prefix</param>
        /// <returns>Array of mining table names</returns>
        private static string[] getAllMiningTableNames(DataSourceView dsv, string strPrefix = "Pivot")
        {
            if (dsv.Schema.Tables.Count > 0)
            {
                string[] tableNames = new string[dsv.Schema.Tables.Count];
                int index = 0;
                foreach (DataTable table in dsv.Schema.Tables)
                {
                    tableNames.SetValue(table.TableName, index++);
                }
                return tableNames;
            }
            else
            {
                Console.WriteLine("Don't have any mining table with suffix " + strPrefix + " in database!");
                return null;
            }
        }

        /// <summary>
        /// Get all mining table names
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strPrefix">Prefix</param>
        /// <returns>Array of mining table names</returns>
        private static string[] getAllMiningTableNames(string strDBServerName, string strDBName, string strPrefix = "Pivot")
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "select TABLE_NAME as Name from INFORMATION_SCHEMA.Views where [TABLE_NAME] like '" + strPrefix + "%'";
                DataTable tables = new DataTable("Tables");
                tables.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
                //tables.Load(command.ExecuteReader());
                if (tables != null)
                {
                    string[] tableNames = new string[tables.Rows.Count];
                    int index = 0;
                    foreach (DataRow row in tables.Rows)
                    {
                        tableNames.SetValue(row[0].ToString(), index++);
                    }
                    return tableNames;
                }
                else
                {
                    Console.WriteLine("Don't have any mining table with suffix " + strPrefix + " in database!");
                    return null;
                }

            }catch(Exception ex)
            {
                Console.WriteLine("Error in getting all mining tables - getAllMiningTableName. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Export Mining Data To Database
        /// <summary>
        /// Export mining data to DecisionTreeNode and DecisionTreeNodeDistribution table
        /// </summary>
        /// <param name="strDBServerName">Target Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Target Database Name (Database Engine)</param>
        /// <param name="strMiningStructureNames">Mining Structure Names to export</param>
        /// <param name="strPrefix">Table Prefix</param>
        private static void exportMiningDataToDB(string strDBServerName, string strDBName, string strASServerName, string[] strMiningStructureNames, string strPrefix)
        {
            try
            {
                string strQuery;
                string strLinkedServerName = "JobZoomMiningLinkedServer";

                Console.WriteLine("Preparing... Exists the linked server (Analysis Server)!");
                if (existsLinkedServer(strDBServerName, strDBName, strLinkedServerName))
                {
                    Console.WriteLine("'Linked Server exists' is true!");
                    Console.WriteLine("Deleting ... Result is " + deleteLinkedServer(strDBServerName, strDBName, strLinkedServerName));
                }

                Console.WriteLine("\nCreating a linked server...");
                if (createLinkedServer(strDBServerName, strDBName, strASServerName, strLinkedServerName))
                    Console.WriteLine("Creating a linked server... Successfully!");
                else
                {
                    Console.WriteLine("Creating a linked server... UN-SUCCESSFULLY!");
                    Console.WriteLine("Failed to export mining data to database. Process is stopped!");
                }

                //drop table DecisionTreeNode and DecisionTreeNodeDistribution and create new
                if (existsDecisionTreeNodeDistributionTable(strDBServerName, strDBName))
                    deleteDecisionTreeNodeDistributionTable(strDBServerName, strDBName);


                if (existsDecisionTreeNodeTable(strDBServerName, strDBName))
                    deleteDecisionTreeNodeTable(strDBServerName, strDBName);

                Console.WriteLine("Create DecisionTreeNode table ...");
                createDecisionTreeNodeTable(strDBServerName, strDBName);

                Console.WriteLine("Create DecisionTreeNodeDistribution table ...");
                createDecisionTreeNodeDistributionTable(strDBServerName, strDBName);


                //Step 1. Create the root note for all jobs
                strQuery = "INSERT INTO DecisionTreeNode(NODE_ID, MODEL_NAME, NODE_TYPE, CHILDREN_CARDINALITY, NODE_SUPPORT, MSOLAP_NODE_SCORE, NODE_PROBABILITY) VALUES('0', '0', 1, " + strMiningStructureNames.Length + ", 0, 0, 0)";
                executeQuery(strDBServerName, strDBName, strQuery);

                strQuery = "INSERT INTO DecisionTreeNodeDistribution([NODE_ID],[ATTRIBUTE_NAME],[ATTRIBUTE_VALUE],[SUPPORT],[PROBABILITY],[VARIANCE],[VALUETYPE]) VALUES('0', '0','0', 0, 0, 0, 0)";
                executeQuery(strDBServerName, strDBName, strQuery);

                int count = 0;
                foreach (string strMiningStructureName in strMiningStructureNames)
                {
                    count++;
                    strQuery = "INSERT INTO DecisionTreeNode " +
                                    "SELECT * FROM " +
                                    "OPENQUERY(" + strLinkedServerName + ", " +
                                    "'SELECT FLATTENED " +
                                    "''" + strPrefix + count + "_'' + [NODE_UNIQUE_NAME] AS [NODE_ID], " +
                                    "[MODEL_NAME], " +
                                    "[NODE_TYPE], " +
                                    "[NODE_CAPTION], " +
                                    "[CHILDREN_CARDINALITY], " +
                                    "''" + strPrefix + count + "_'' + [PARENT_UNIQUE_NAME] AS [PARENT_ID], " +
                                    "[NODE_DESCRIPTION], " +
                                    "[NODE_RULE], " +
                                    "[MARGINAL_RULE], " +
                                    "[NODE_PROBABILITY], " +
                                    "[MARGINAL_PROBABILITY], " +
                                    "[NODE_SUPPORT], " +
                                    "[MSOLAP_MODEL_COLUMN], " +
                                    "[MSOLAP_NODE_SCORE], " +
                                    "[MSOLAP_NODE_SHORT_CAPTION], " +
                                    "[ATTRIBUTE_NAME] " +
                                    "FROM [" + strMiningStructureName + "].CONTENT " +
                                    "WHERE [NODE_UNIQUE_NAME] <> ''0'' ORDER BY [NODE_UNIQUE_NAME] ASC')";
                    executeQuery(strDBServerName, strDBName, strQuery);

                    //Step 2. Insert data (except root node). (Rename node named "All" to strMiningStructureName
                    strQuery = "UPDATE DecisionTreeNode SET [NODE_CAPTION] = '" + strMiningStructureName +
                                                        "', [NODE_DESCRIPTION] ='" + strMiningStructureName +
                                                        "', [MSOLAP_NODE_SHORT_CAPTION] ='" + strMiningStructureName +
                                                        "', [PARENT_ID] = '0'" +
                                                        " WHERE [NODE_CAPTION] = 'All' AND [NODE_TYPE] = 2;";
                    executeQuery(strDBServerName, strDBName, strQuery);

                    strQuery = "INSERT INTO DecisionTreeNodeDistribution " +
                                    "SELECT * FROM " +
                                    "OPENQUERY(" + strLinkedServerName + ", " +
                                    "'SELECT FLATTENED " +
                                    "''" + strPrefix + count + "_'' + [NODE_UNIQUE_NAME] AS [NODE_ID], " +
                                    "[NODE_DISTRIBUTION] " +
                                    "FROM [" + strMiningStructureName + "].CONTENT " +
                                    "WHERE [NODE_UNIQUE_NAME] <> ''0''')";
                    executeQuery(strDBServerName, strDBName, strQuery);

                    //String Decode
                    string[] source = { "_x002E_", "_x002C_", "_x003B_", "_x0027_", "_x0060_", "_x003A_", "_x002F_", "_x005C_", "_x002A_", "_x007C_", "_x003F_", "_x0022_", "_x0026_", "_x0025_", "_x0024_", "_x0021_", "_x002B_", "_x003D_", "_x0028_", "_x0029_", "_x005B_", "_x005D_", "_x007B_", "_x007D_", "_x003C_", "_x003E_" };
                    string[] target = { ".", ",", ";", "''", "`", ":", "/", @"\", "*", "|", "?", "\"", "&", "%", "$", "!", "+", "=", "(", ")", "[", "]", "{", "}", "<", ">" };
                    //chu y kiem tra ky tu '
                    for (int i = 0; i < source.Length; i++)
                    {
                        strQuery = "UPDATE DecisionTreeNode SET NODE_CAPTION = REPLACE(NODE_CAPTION, '" + source[i] + "', '" + target[i] + "'), " +
                                "NODE_DESCRIPTION = REPLACE(cast(NODE_DESCRIPTION as NVARCHAR(MAX)), '" + source[i] + "', '" + target[i] + "')," + 
                                "MSOLAP_NODE_SHORT_CAPTION = REPLACE(MSOLAP_NODE_SHORT_CAPTION, '" + source[i] + "', '" + target[i] + "');";
                        executeQuery(strDBServerName, strDBName, strQuery);
                    }
                }

                strQuery = "ALTER TABLE DecisionTreeNode ADD constraint [FK_ParentNode_Node] FOREIGN KEY (PARENT_ID) REFERENCES DecisionTreeNode(NODE_ID);";
                executeQuery(strDBServerName, strDBName, strQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - exportMiningDataToDB. Error Message -> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     Is DecisionTreeNode table exists?
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name</param>
        /// <returns>True if DecisionTreeNode table is exists and reverse</returns>
        private static bool existsDecisionTreeNodeTable(string strDBServerName, string strDBName)
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);

                objConnection.Open();
                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "select 1 from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' AND [TABLE_NAME] = 'DecisionTreeNode'";
                if (command.ExecuteReader(CommandBehavior.CloseConnection).Read())
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - existsDecisionTreeNodeTable. Error Message -> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     Is DecisionTreeNodeDistribution table exists?
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name</param>
        /// <returns>True if DecisionTreeNodeDistribution table is exists and reverse</returns>
        private static bool existsDecisionTreeNodeDistributionTable(string strDBServerName, string strDBName)
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);

                objConnection.Open();
                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "select 1 from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' AND [TABLE_NAME] = 'DecisionTreeNodeDistribution'";
                if (command.ExecuteReader(CommandBehavior.CloseConnection).Read())
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - existsDecisionTreeNodeTable. Error Message -> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Create DecisionTreeNode table
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <returns>Result</returns>
        private static bool createDecisionTreeNodeTable(string strDBServerName, string strDBName)
        {
            try
            {
                if (!existsDecisionTreeNodeTable(strDBServerName, strDBName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "CREATE TABLE [dbo].[DecisionTreeNode](" +
                        "[NODE_ID] [varchar](100) CONSTRAINT [PK_NODEID] PRIMARY KEY ([NODE_ID]), " +
                        "[MODEL_NAME] [varchar](100) NOT NULL, " +
                        "[NODE_TYPE] [int] NOT NULL, " +
                        "[NODE_CAPTION] [nvarchar](256) NULL, " +
                        "[CHILDREN_CARDINALITY] int NOT NULL, " +
                        "[PARENT_ID] [varchar](100) NULL, " +
                        "[NODE_DESCRIPTION] [ntext] NULL, " +
                        "[NODE_RULE] [ntext] NULL, " +
                        "[MARGINAL_RULE] [ntext] NULL, " +
                        "[NODE_PROBABILITY] [float] NOT NULL, " +
                        "[MARGINAL_PROBABILITY] [float] NULL, " +
                        "[NODE_SUPPORT] [float] NOT NULL, " +
                        "[MSOLAP_MODEL_COLUMN] [nvarchar](256) NULL, " +
                        "[MSOLAP_NODE_SCORE] [float] NULL, " +
                        "[MSOLAP_NODE_SHORT_CAPTION] [nvarchar](256) NULL, " +
                        "[ATTRIBUTE_NAME] [nvarchar](256) NULL " +
                        ")";
                    command.ExecuteNonQuery();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Table has already existed or error - createDecisionTreeNodeTable. Error Message -> " + ex.Message);
                //throw new Exception(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create DecisionTreeNodeDistribution table
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <returns>Result/returns>
        private static bool createDecisionTreeNodeDistributionTable(string strDBServerName, string strDBName)
        {
            try
            {
                if (!existsDecisionTreeNodeDistributionTable(strDBServerName, strDBName) && existsDecisionTreeNodeTable(strDBServerName, strDBName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "CREATE TABLE [dbo].[DecisionTreeNodeDistribution]( " +
                                "[NODE_ID] [varchar](100) NOT NULL " +
                                    "CONSTRAINT FK_Node_NodeDistribution FOREIGN KEY (NODE_ID) REFERENCES [dbo].[DecisionTreeNode](NODE_ID), " +
                                "[ATTRIBUTE_NAME] [nvarchar](256), " +
                                "[ATTRIBUTE_VALUE] [nvarchar](7) CONSTRAINT [PK_NODEID_ATTNAME] PRIMARY KEY([NODE_ID], [ATTRIBUTE_VALUE]), " +
                                "[SUPPORT] [float] NOT NULL, " +
                                "[PROBABILITY] [float] NOT NULL, " +
                                "[VARIANCE] [float] NOT NULL, " +
                                "[VALUETYPE] [int] NOT NULL" +
                                ")";
                    command.ExecuteNonQuery();
                    return true;
                }
                else return false;
            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Table has already existed or error - createDecisionTreeNodeDistributionTable. Error Message -> " + ex.Message);
                //throw new Exception(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete DecisionTreeNode Table
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <returns>True if DecisionTreeNode table deleted sucessfully and reverse</returns>
        private static bool deleteDecisionTreeNodeTable(string strDBServerName, string strDBName)
        {
            try
            {
                if (!existsDecisionTreeNodeDistributionTable(strDBServerName, strDBName) && existsDecisionTreeNodeTable(strDBServerName, strDBName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "DROP TABLE DecisionTreeNode;";
                    command.ExecuteNonQuery();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - deleteDecisionTreeNodeTable. Error Message -> " + ex.Message);
                //throw new Exception(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Delete DecisionTreeNodeDistribution Table
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <returns>True if DecisionTreeNodeDistribution table deleted sucessfully and reverse</returns>
        private static bool deleteDecisionTreeNodeDistributionTable(string strDBServerName, string strDBName)
        {
            try
            {
                if (existsDecisionTreeNodeDistributionTable(strDBServerName, strDBName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "DROP TABLE DecisionTreeNodeDistribution;";
                    command.ExecuteNonQuery();
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - deleteDecisionTreeNodeDistributionTable. Error Message -> " + ex.Message);
                //throw new Exception(ex.Message);
                return false;
            }
        }

        #endregion

        #region Linked Server
        /// <summary>
        /// Create linked server on Database Engine to get data from Analysis Service by MDX language
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strLinkedServerName">Linked Server Name to create</param>
        /// <param name="strAnalysisDBName">Analysis Service Database Name</param>
        /// <returns></returns>
        public static bool createLinkedServer(string strDBServerName, string strDBName, string strASServerName, string strLinkedServerName = "JobZoomMiningLinkedServer", string strAnalysisDBName = "Job Zoom Mining")
        {
            try
            {
                if (!existsLinkedServer(strDBServerName, strDBName, strLinkedServerName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "EXEC master.dbo.sp_addlinkedserver @server='" + strLinkedServerName + "', @srvproduct='', @provider='MSOLAP', @datasrc='" + strASServerName + "', @catalog='" + strAnalysisDBName + "';";
                    command.ExecuteNonQuery();
                    return existsLinkedServer(strDBServerName, strDBName, strLinkedServerName);
                }
                else
                {
                    Console.WriteLine("Linked server existed!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in creating linked server - createLinkedServer. Error Message -> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Is Linked Server exist?
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strLinkedServerName">Linked server name to check</param>
        /// <returns>Result</returns>
        public static bool existsLinkedServer(string strDBServerName, string strDBName, string strLinkedServerName = "JobZoomMiningLinkedServer")
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);

                objConnection.Open();
                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "Select 1 Where Exists (Select [SRVID] From master..sysservers Where [srvName]='" + strLinkedServerName + "');";
                if (command.ExecuteReader(CommandBehavior.CloseConnection).Read())
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in checking linked server - existsLinkedServer. Error Message -> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete linked server
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="strLinkedServerName">Linked server name to delete</param>
        /// <returns>Result</returns>
        public static bool deleteLinkedServer(string strDBServerName, string strDBName, string strLinkedServerName = "JobZoomMiningLinkedServer")
        {
            try
            {
                if (existsLinkedServer(strDBServerName, strDBName, strLinkedServerName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "sp_dropserver '" + strLinkedServerName + "', 'droplogins';";
                    command.ExecuteNonQuery();
                    return !existsLinkedServer(strDBServerName, strDBName, strLinkedServerName);
                }
                else
                {
                    Console.WriteLine("Linked server doesn't exist!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in deleting linked server - deleteLinkedServer. Error Message -> " + ex.Message);
                return false;
            }

        }
        #endregion

        #region Database Engine Query
        /// <summary>
        /// Execute query (Database Engine Query)
        /// </summary>
        /// <param name="strDBServerName">Database Server Name (Database Engine)</param>
        /// <param name="strDBName">Database Name</param>
        /// <param name="strQuery">Query to execute</param>
        /// <returns>True if exexute sucessfully and reverse</returns>
        private static bool executeQuery(string strDBServerName, string strDBName, string strQuery)
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);

                //Open Connection
                //if (objConnection.State == ConnectionState.Closed)
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - executeQuery " + strQuery + ". Error Message -> " + ex.Message);
                return false;
            }
        }
        #endregion

        #endregion Mining Database Generation.

        #region Algorithm Parameters

        public class DecisionTreeAlgorithmParameters
        {
            private int _HoldoutMaxPercent = 10; //Default .Net 30%
            private int _SCORE_METHOD = 4; //Entropy (1), Bayesian with K2 Prior (2), or Bayesian Dirichlet Equivalent (BDE) Prior (4 - .Net Default)
            private float _COMPLEXITY_PENALTY = 0.1f; //Default 0.5
            private int _SPLIT_METHOD = 3; //.Net Default 3
            private int _MAXIMUM_INPUT_ATTRIBUTES = 255; //.Net Default 255
            private int _MAXIMUM_OUTPUT_ATTRIBUTES = 255; //.NetDefault 255
            private float _MINIMUM_SUPPORT = 0.05f; //.NetDefault 10

            /// <summary>
            /// Holdout Max Percent - Default is 10 (10%)
            /// </summary>
            public int HoldoutMaxPercent { get { return _HoldoutMaxPercent; } }

            /// <summary>
            /// SCORE_METHOD value must be 1 / 2 / 4 (default)
            /// </summary>
            public int SCORE_METHOD { get { return _SCORE_METHOD; } }

            /// <summary>
            /// COMPLEXITY_PENALTY - default: 0.1
            /// </summary>
            public float COMPLEXITY_PENALTY { get { return _COMPLEXITY_PENALTY; } }

            /// <summary>
            /// SPLIT_METHOD - default 3
            /// </summary>
            public int SPLIT_METHOD { get { return _SPLIT_METHOD; } }

            /// <summary>
            /// MAXIMUM_INPUT_ATTRIBUTES - default 255
            /// </summary>
            public int MAXIMUM_INPUT_ATTRIBUTES { get { return _MAXIMUM_INPUT_ATTRIBUTES; } }

            /// <summary>
            /// MAXIMUM_OUTPUT_ATTRIBUTES - default 255
            /// </summary>
            public int MAXIMUM_OUTPUT_ATTRIBUTES { get { return _MAXIMUM_OUTPUT_ATTRIBUTES; } }

            /// <summary>
            /// MINIMUM_SUPPORT - default 0.05
            /// </summary>
            public float MINIMUM_SUPPORT { get { return _MINIMUM_SUPPORT; } }

            public DecisionTreeAlgorithmParameters(int HoldoutMaxPercent, int SCORE_METHOD, float COMPLEXITY_PENALTY, int SPLIT_METHOD, int MAXIMUM_INPUT_ATTRIBUTES, int MAXIMUM_OUTPUT_ATTRIBUTES, float MINIMUM_SUPPORT)
            {
                if (SCORE_METHOD != 1 || SCORE_METHOD != 2 || SCORE_METHOD != 4)
                {
                    SCORE_METHOD = 4;
                }
                _HoldoutMaxPercent = HoldoutMaxPercent;
                _SCORE_METHOD = SCORE_METHOD;
                _COMPLEXITY_PENALTY = COMPLEXITY_PENALTY;
                _SPLIT_METHOD = SPLIT_METHOD;
                _MAXIMUM_INPUT_ATTRIBUTES = MAXIMUM_INPUT_ATTRIBUTES;
                _MAXIMUM_OUTPUT_ATTRIBUTES = MAXIMUM_OUTPUT_ATTRIBUTES;
                _MINIMUM_SUPPORT = MINIMUM_SUPPORT;
            }

            /// <summary>
            /// DecisionTree Mining with default AlgorithmParameters
            /// </summary>
            public DecisionTreeAlgorithmParameters()
            {
                _HoldoutMaxPercent = 10;
                _SCORE_METHOD = 4;
                _COMPLEXITY_PENALTY = 0.1f;
                _SPLIT_METHOD = 3;
                _MAXIMUM_INPUT_ATTRIBUTES = 255;
                _MAXIMUM_OUTPUT_ATTRIBUTES = 255;
                _MINIMUM_SUPPORT = 0.05f;
            }
        }

        #endregion
    }
}
