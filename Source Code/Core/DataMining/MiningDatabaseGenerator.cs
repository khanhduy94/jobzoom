using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AnalysisServices;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Net;

namespace JobZoom.Core
{
    class MiningDatabaseGenerator
    {
        static void Main(string[] args)
        {
            //test();
            //Build();
            BuildMiningDatabase();
            Console.ReadLine();
        }

        private static void test()
        {
            //string s = StringEncode(".,;'`:/\\*|?\"&%$!+=()[]{}<>");
            string s = StringEncode("C++");
            Console.WriteLine(s);
            s = StringDecode(s);
            Console.WriteLine(s);
        }

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
        #region Cube Generation.

        private static void BuildMiningDatabase()
        {
            string strPrefix = "Pivot%";
            string strDBServerName = "TRUNGHIEU-PC"; //Analysis service
            string strProviderName = "msolap"; //Microsoft OLE DB Provider for Analysis Services 10.0
            string strDBName = "JobZoom"; // Database (Database Engine)
            string strMiningDBName = "Job Zoom Mining"; //Mining database name
            string strMiningDataSourceName = "Data Source"; //Mining datasource name
            string strMiningDataSourceViewName = "Data Source View"; //Mining datasource view name
            string[] strFactTableName = getAllMiningTableName(strDBServerName, strDBName, strPrefix); //tables in datasource view to mining
            
            string[,] strTableNamesAndKeys = { { "PivotProfile", "ProfileBasicId", "PivotProfile", "ProfileBasicId" }, };

            int intDimensionTableCount = 0;
            Server objServer = new Server();
            Database objDatabase = new Database();
            RelationalDataSource objDataSource = new RelationalDataSource();
            DataSourceView objDataSourceView = new DataSourceView();
            DataSet objDataSet = new DataSet();
            Dimension[] objDimensions = new Dimension[intDimensionTableCount];
            MiningStructure[] objMiningStructures = new MiningStructure[strFactTableName.Length];


            Console.WriteLine("Mining creation process started.");
            Console.WriteLine("");

            Console.WriteLine("Step 1. Connecting to the Analysis Services.");
            Console.WriteLine("Step 1. Started!");
            objServer = (Server) ConnectAnalysisServices(strDBServerName, strProviderName);
            Console.WriteLine("Step 1. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 2. Creating a Database.");
            Console.WriteLine("Step 2. Started!");
            objDatabase = (Database)CreateDatabase(objServer, strMiningDBName);
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

            Console.WriteLine("Step 5. Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects.");
            Console.WriteLine("Step 5. Started!");
            //objDimensions = (Dimension[])CreateDimension(objDatabase, objDataSourceView, strTableNamesAndKeys, intDimensionTableCount);
            Console.WriteLine("Step 5. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 6. Creating the Cube, MeasureGroup, Measure, and Partition Objects.");
            Console.WriteLine("Step 6. Started!");
            //CreateCube(objDatabase, objDataSourceView, objDataSource, objDimensions, strFactTableName, strTableNamesAndKeys, intDimensionTableCount);
            Console.WriteLine("Step 6. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Step 7. Createing Mining Structures [with Decision Tree Algorithms]");
            Console.WriteLine("Step 7. Started!");
            
            objMiningStructures = (MiningStructure[]) CreateMiningStructures(objDatabase, objDataSourceView, strFactTableName);
            Console.WriteLine("Step 7. Finished!");
            Console.WriteLine("");


            //Console.WriteLine("Listing all mining tables and its columns");
            //Console.WriteLine("Started!");
            //string[] tableNames = getAllMiningTableName(strDBServerName, strDBName);
            //foreach (string tableName in tableNames)
            //{
            //    Console.WriteLine(tableName + " is listing...");
            //    string[] colNames = getAllColumnName(objDataSourceView, tableName);
            //    foreach (string colName in colNames)
            //    {
            //        Console.WriteLine(colName);
            //    }
            //}            
            //Console.WriteLine("Finished!");
            //Console.WriteLine("");

            Console.WriteLine("Step 8. Export mining data to JobZoom Database (Database Engine)");
            Console.WriteLine("Step 8. Started!");
            Console.WriteLine("Preparing... Exists the linked server (Analysis Server)!");
            if (existsLinkedServer(strDBServerName, strDBName))
                Console.WriteLine("'Linked Server exists' is true!");
            else
            {
                Console.WriteLine("'Linked Server exists' is false!");
                Console.WriteLine("\nCreating a linked server...");
                if (createLinkedServer(strDBServerName, strDBName))
                    Console.WriteLine("Creating a linked server... Successfully!");
                else
                {
                    Console.WriteLine("Creating a linked server... UN-SUCCESSFULLY!");
                    Console.WriteLine("Failed to export mining data to database. Process is stopped!");
                    return;
                }
            }
            Console.WriteLine("Preparing... Put website to maintenance mode");
            //EXEC WEB SITE MAINTENANCE SERVICE METHOD

            Console.WriteLine("Preparing... Cleaning DecisionTreeNode and DecisionTreeNodeDistribution");
            Console.WriteLine("\nStep 8. Finished!");
            Console.WriteLine("");

            Console.WriteLine("Export completed! Release website to continuing for using");
            //WEBSITE CAN CONTINUE FOR USING
            Console.WriteLine("Saving...");
            objDatabase.Process(ProcessType.ProcessFull);
            Console.WriteLine("Analysis Service Database created successfully.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        #region Connecting to the Analysis Services.
        /// <summary>
        /// Connecting to the Analysis Services.
        /// </summary>
        /// <param name="strDBServerName">Database Server Name.</param>
        /// <param name="strProviderName">Provider Name.</param>
        /// <returns>Database Server instance.</returns>
        private static object ConnectAnalysisServices(string strDBServerName, string strProviderName)
        {
            try
            {
                Console.WriteLine("Connecting to the Analysis Services ...");

                Server objServer = new Server();
                string strConnection = "Data Source=" + strDBServerName + ";Provider=" + strProviderName + ";";
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

        #region Creating a Database.
        /// <summary>
        /// Creating a Database.
        /// </summary>
        /// <param name="objServer">Database Server Name.</param>
        /// <param name="strCubeDBName">Cube DB Name.</param>
        /// <returns>DB instance.</returns>
        private static object CreateDatabase(Server objServer, string strCubeDBName)
        {
            try
            {
                Console.WriteLine("Creating a Database ...");

                Database objDatabase = new Database();
                //Add Database to the Analysis Services.
                objDatabase = objServer.Databases.Add(objServer.Databases.GetNewName(strCubeDBName));
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
        #endregion Creating a Database.

        private static object GetDatabase(Server objServer, string strCubeDBName)
        {
            try
            {
                Console.WriteLine("Creating a Database ...");

                Database objDatabase = new Database();
                //Add Database to the Analysis Services.
                objDatabase = objServer.Databases.GetByName(strCubeDBName);
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

        #region Creating a DataSource.
        /// <summary>
        /// Creating a DataSource.
        /// </summary>
        /// <param name="objServer">Database Server Name.</param>
        /// <param name="objDatabase">Database Name.</param>
        /// <param name="strCubeDataSourceName">Cube DataSource Name.</param>
        /// <param name="strDBServerName">DB Server Name.</param>
        /// <param name="strDBName">DB Name.</param>
        /// <returns>DataSource instance.</returns>
        private static object CreateDataSource(Server objServer, Database objDatabase, string strCubeDataSourceName, string strDBServerName, string strDBName)
        {
            try
            {
                Console.WriteLine("Creating a DataSource ...");
                RelationalDataSource objDataSource = new RelationalDataSource();
                //Add Data Source to the Database.
                objDataSource = objDatabase.DataSources.Add(objServer.Databases.GetNewName(strCubeDataSourceName));                
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
        #endregion Creating a DataSource.

        private static object GetDataSource(Server objServer, Database objDatabase, string strCubeDataSourceName, string strDBServerName, string strDBName)
        {
            try
            {
                Console.WriteLine("Creating a DataSource ...");
                DataSource objDataSource;
                //Add Data Source to the Database.
                objDataSource = objDatabase.DataSources.GetByName(strCubeDataSourceName);
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

        #region Creating a DataSourceView.
        /// <summary>
        /// Creating a DataSourceView.
        /// </summary>
        /// <param name="strDBServerName">DB Server Name.</param>
        /// <param name="strDBName">DB Name.</param>
        /// <param name="strFactTableName">FactTable Name.</param>
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

        private static object GenerateDWSchema(string strDBServerName, string strDBName, string strPrefix = "Pivot%")
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

                //Do tat ca cac bang vao dataset
                string[] miningTables = getAllMiningTableName(strDBServerName, strDBName, strPrefix);
                foreach (string miningTable in miningTables)
                {
                    objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, miningTable);
                }

                //Add FactTable in DataSet.
                

                //objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, strFactTableName2);

                ////Add table in DataSet and Relation between them.
                //for (int i = 0; i < intDimensionTableCount; i++)
                //{
                //    //Retrieve table's schema and assign the table's schema to the DataSet.
                //    //Add primary key to the schema according to the primary key in the tables.
                //    objDataSet = (DataSet)FillDataSet(objConnection, objDataSet, strTableNamesAndKeys[i, 0]);
                //    //objDataSet = (DataSet)AddDataTableRelation(objDataSet, strTableNamesAndKeys[i, 0], strTableNamesAndKeys[i, 1], strTableNamesAndKeys[i, 2], strTableNamesAndKeys[i, 3]);
                //}

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
        /// <param name="objConnection">Connection instance.</param>
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
        /// <param name="objDatabase">DB instance.</param>
        /// <param name="objDataSource">DataSource instance.</param>
        /// <param name="objDataSet">DataSet instance.</param>
        /// <param name="strCubeDataSourceViewName">Cube DataSourceView Name.</param>
        /// <returns>DataSourceView instance.</returns>
        private static object CreateDataSourceView(Database objDatabase, RelationalDataSource objDataSource, DataSet objDataSet, string strCubeDataSourceViewName)
        {
            try
            {
                DataSourceView objDataSourceView = new DataSourceView();
                //Add Data Source View to the Database.
                objDataSourceView = objDatabase.DataSourceViews.Add(objDatabase.DataSourceViews.GetNewName(strCubeDataSourceViewName));
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
        #endregion Creating a DataSourceView.

        #region Creating a Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects.
        /// <summary>
        /// Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects.
        /// </summary>
        /// <param name="objDatabase">DB instance.</param>
        /// <param name="objDataSourceView">DataSource instance.</param>
        /// <param name="strTableNamesAndKeys">Array of Table names and keys.</param>
        /// <param name="intDimensionTableCount">Dimension table count.</param>
        /// <returns>Dimension Array.</returns>
        private static object[] CreateDimension(Database objDatabase, DataSourceView objDataSourceView, string[,] strTableNamesAndKeys, int intDimensionTableCount)
        {
            try
            {
                Console.WriteLine("Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects ...");

                Dimension[] objDimensions = new Dimension[intDimensionTableCount];
                for (int i = 0; i < intDimensionTableCount; i++)
                {
                    objDimensions[i] = (Dimension)GenerateDimension(objDatabase, objDataSourceView, strTableNamesAndKeys[i, 0], strTableNamesAndKeys[i, 1]);
                }

                ////Add Hierarchy and Level
                //Hierarchy objHierarchy = objDimension.Hierarchies.Add("ProductByCategory");
                //objHierarchy.Levels.Add("Category").SourceAttributeID = objCatKeyAttribute.ID;
                //objHierarchy.Levels.Add("Product").SourceAttributeID = objProdKeyAttribute.ID;
                ////Add Member Property
                ////objProdKeyAttribute.AttributeRelationships.Add(objProdDescAttribute.ID);
                //objDimension.Update();

                return objDimensions;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects. Error Message -> " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Generate single dimension.
        /// </summary>
        /// <param name="objDatabase">DB instance.</param>
        /// <param name="objDataSourceView">DataSourceView instance.</param>
        /// <param name="strTableName">Table name.</param>
        /// <param name="strTableKeyName">Table key.</param>
        /// <returns>Dimension instance.</returns>
        private static object GenerateDimension(Database objDatabase, DataSourceView objDataSourceView, string strTableName, string strTableKeyName)
        {
            try
            {
                Dimension objDimension = new Dimension();

                //Add Dimension to the Database
                objDimension = objDatabase.Dimensions.Add(strTableName);
                objDimension.Source = new DataSourceViewBinding(objDataSourceView.ID);
                DimensionAttributeCollection objDimensionAttributesColl = objDimension.Attributes;
                //Add Dimension Attributes
                DimensionAttribute objAttribute = objDimensionAttributesColl.Add(strTableKeyName);
                //Set Attribute usage and source
                objAttribute.Usage = AttributeUsage.Key;
                objAttribute.KeyColumns.Add(strTableName, strTableKeyName, OleDbType.Integer);

                objDimension.Update();

                return objDimension;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects - GenerateDimension. Error Message -> " + ex.Message);
                return null;
            }
        }
        #endregion Creating a Creating the Dimension, Attribute, Hierarchy, and MemberProperty Objects.

        #region Creating the Cube, MeasureGroup, Measure, and Partition Objects.
        /// <summary>
        /// Creating the Cube, MeasureGroup, Measure, and Partition Objects.
        /// </summary>
        /// <param name="objDatabase">DB instance.</param>
        /// <param name="objDataSourceView">DataSourceView instance.</param>
        /// <param name="objDataSource">DataSource instance.</param>
        /// <param name="objDimensions">Dimensions array instance.</param>
        /// <param name="strFactTableName">FactTable Name.</param>
        /// <param name="strTableNamesAndKeys">Array of Table Names and Keys.</param>
        /// <param name="intDimensionTableCount">DimensionTable Count.</param>
        private static void CreateCube(Database objDatabase, DataSourceView objDataSourceView, RelationalDataSource objDataSource, Dimension[] objDimensions, string strFactTableName, string[,] strTableNamesAndKeys, int intDimensionTableCount)
        {
            try
            {
                Console.WriteLine("Creating the Cube, MeasureGroup, Measure, and Partition Objects ...");
                Cube objCube = new Cube();
                Measure objSales = new Measure();
                Measure objQuantity = new Measure();
                MdxScript objTotal = new MdxScript();
                String strScript;

                Partition objPartition = new Partition();
                Command objCommand = new Command();
                //Add Cube to the Database and set Cube source to the Data Source View
                objCube = objDatabase.Cubes.Add("SampleCube");
                objCube.Source = new DataSourceViewBinding(objDataSourceView.ID);
                //Add Measure Group to the Cube
                //MeasureGroup objMeasureGroup = objCube.MeasureGroups.Add("FactSales");
                MeasureGroup objMeasureGroup = objCube.MeasureGroups.Add(strFactTableName);

                //Add Measure to the Measure Group and set Measure source
                objSales = objMeasureGroup.Measures.Add("Amount");
                objSales.Source = new DataItem(strFactTableName, "SalesAmount", OleDbType.Currency);

                objQuantity = objMeasureGroup.Measures.Add("Quantity");
                objQuantity.Source = new DataItem(strFactTableName, "OrderQuantity", OleDbType.Integer);

                ////Calculated Member Definition
                //strScript = "Calculated; Create Member CurrentCube.[Measures].[Total] As [Measures].[Quantity] * [Measures].[Amount]";
                ////Add Calculated Member
                //objTotal.Name = "Total Sales";
                //objCommand.Text = strScript;
                //objTotal.Commands.Add(objCommand);
                //objCube.MdxScripts.Add(objTotal);

                for (int i = 0; i < intDimensionTableCount; i++)
                {
                    GenerateCube(objCube, objDimensions[i], objMeasureGroup, strFactTableName, strTableNamesAndKeys[i, 3]);
                }

                objPartition = objMeasureGroup.Partitions.Add(strFactTableName);
                objPartition.Source = new TableBinding(objDataSource.ID, "dbo", strFactTableName);

                objPartition.ProcessingMode = ProcessingMode.Regular;
                objPartition.StorageMode = StorageMode.Molap;
                //Save Cube and all major objects to the Analysis Services
                objCube.Update(UpdateOptions.ExpandFull);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating the Cube, MeasureGroup, Measure, and Partition Objects. Error Message -> " + ex.Message);
            }
        }
        /// <summary>
        /// Generate cube.
        /// </summary>
        /// <param name="objCube">Cube instance.</param>
        /// <param name="objDimension">Dimension instance.</param>
        /// <param name="objMeasureGroup">MeasureGroup instance.</param>
        /// <param name="strFactTableName">FactTable Name.</param>
        /// <param name="strTableKey">Table Key.</param>
        private static void GenerateCube(Cube objCube, Dimension objDimension, MeasureGroup objMeasureGroup, string strFactTableName, string strTableKey)
        {
            try
            {                
                CubeDimension objCubeDim = new CubeDimension();
                RegularMeasureGroupDimension objRegMGDim = new RegularMeasureGroupDimension();
                MeasureGroupAttribute objMGA = new MeasureGroupAttribute();
                //Add Dimension to the Cube
                objCubeDim = objCube.Dimensions.Add(objDimension.ID);
                //Use Regular Relationship Between Dimension and FactTable Measure Group
                objRegMGDim = objMeasureGroup.Dimensions.Add(objCubeDim.ID);
                //Link TableKey in DimensionTable with TableKey in FactTable Measure Group
                objMGA = objRegMGDim.Attributes.Add(objDimension.KeyAttribute.ID);

                objMGA.Type = MeasureGroupAttributeType.Granularity;
                objMGA.KeyColumns.Add(strFactTableName, strTableKey, OleDbType.Integer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating the Cube, MeasureGroup, Measure, and Partition Objects - GenerateCube. Error Message -> " + ex.Message);
            }
        }
        #endregion Creating the Cube, MeasureGroup, Measure, and Partition Objects.

        #endregion Cube Generation.

        #region Mining structure Generation.

        private static object[] CreateMiningStructures(Database objDatabase, DataSourceView objDataSourceView, string[] strCaseTableNames)
        {
            MiningStructure[] miningStructures = new MiningStructure[strCaseTableNames.Length];
            try
            {
                for (int i = 0; i < strCaseTableNames.Length; i++)
                {
                    miningStructures[i] = (MiningStructure) GenerateMiningStructure(objDatabase, objDataSourceView, strCaseTableNames[i]);
                }
                return miningStructures;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Mining structure - GenerateMiningStructure. Error Message -> " + ex.Message);
                return null;
            }
        }
        private static object GenerateMiningStructure(Database objDatabase, DataSourceView objDataSourceView, string strCaseTableName)
        {
            try
            {
                
                MiningStructure objMiningStructure = new MiningStructure();
                objMiningStructure = objDatabase.MiningStructures.Add(objDatabase.MiningStructures.GetNewName(strCaseTableName));
                objMiningStructure.HoldoutMaxPercent = 30; //30% of testing
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
                return objMiningStructure;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Creating a Mining structure - GenerateMiningStructure. Error Message -> " + ex.Message);
                return null;
            }
        
        }

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

        private static string[] getAllMiningTableName(DataSourceView dsv, string strPrefix = "Pivot%")
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

        private static string[] getAllMiningTableName(string strDBServerName, string strDBName, string strPrefix = "Pivot%")
        {
            try
            {
                //Create the connection string.
                string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                //Create the SqlConnection.
                SqlConnection objConnection = new SqlConnection(conxString);
                objConnection.Open();

                SqlCommand command = objConnection.CreateCommand();
                command.CommandText = "select TABLE_NAME as Name from INFORMATION_SCHEMA.Views where [TABLE_NAME] like '" + strPrefix + "'";
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
        /// 
        /// </summary>
        /// <param name="strDBServerName">Server Name (Analysis Service)</param>
        /// <param name="strDBName">Database Name (Database Engine)</param>
        /// <param name="objDatabase">Database (Analysis Service)</param>
        /// <param name="strMiningStructureName">Mining structure name to export</param>
        /// <param name="strIndustry">The root industry in DecisionTreeNode table</param>
        private static void exportMiningDataToDB(string strDBServerName, string strDBName, Database objDatabase, string[] strMiningStructureName)
        {
            Console.WriteLine("Open connection to database engine ...");
            //Create the connection string.
            string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
            //Create the SqlConnection.
            SqlConnection objConnection = new SqlConnection(conxString);

            //Open Connection
            //if (objConnection.State == ConnectionState.Closed)
            objConnection.Open();
            
            SqlCommand command = objConnection.CreateCommand();
            command.CommandText = "";


        }

        private static bool preparingDecisionTreeTables(string strDBServerName, string strDBName)
        {
            return false;
        }

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
                        "[NODEID] [varchar](100) PRIMARY KEY, " +
                        "[NODE_TYPE] [int] NULL, " +
                        "[NODE_CAPTION] [nvarchar](256) NULL, " +
                        "[CHILDREN_CARDINALITY] int NULL, " +
                        "[PARENTID] [varchar](100) NULL, " +
                        "[NODE_DESCRIPTION] [ntext] NULL, " +
                        "[NODE_RULE] [ntext] NULL, " +
                        "[MARGINAL_RULE] [ntext] NULL, " +
                        "[NODE_PROBABILITY] [float] NULL, " +
                        "[MARGINAL_PROBABILITY] [float] NULL, " +
                        "[NODE_SUPPORT] [float] NULL, " +
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
                                "[NODEID] [varchar](100) NOT NULL " +
                                    "REFERENCES [dbo].[DecisionTreeNode](NODEID), " +
                                "[ATTRIBUTE_NAME] [nvarchar](256) NULL, " +
                                "[ATTRIBUTE_VALUE] [nvarchar](7) NULL, " +
                                "[SUPPORT] [float] NULL, " +
                                "[PROBABILITY] [float] NULL, " +
                                "[VARIANCE] [float] NULL, " +
                                "[VALUETYPE] [int] NULL " +
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
        #endregion

        #region Linked Server
        public static bool createLinkedServer(string strDBServerName, string strDBName, string strLinkedServerName = "JobZoomMiningLinkedServer", string strAnalysisDBName = "Job Zoom Mining")
        {
            try
            {
                if (!existsLinkedServer(strDBServerName, strDBName, strLinkedServerName, strAnalysisDBName))
                {
                    //Create the connection string.
                    string conxString = "Data Source=" + strDBServerName + "; Initial Catalog=" + strDBName + "; Integrated Security=True;";
                    //Create the SqlConnection.
                    SqlConnection objConnection = new SqlConnection(conxString);

                    objConnection.Open();
                    SqlCommand command = objConnection.CreateCommand();
                    command.CommandText = "EXEC master.dbo.sp_addlinkedserver @server='" + strLinkedServerName + "', @srvproduct='', @provider='MSOLAP', @datasrc='TRUNGHIEU-PC', @catalog='" + strAnalysisDBName + "';";
                    command.ExecuteNonQuery();
                    return existsLinkedServer(strDBServerName, strDBName, strLinkedServerName, strAnalysisDBName);
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

        public static bool existsLinkedServer(string strDBServerName, string strDBName, string strLinkedServerName = "JobZoomMiningLinkedServer", string strAnalysisDBName = "Job Zoom Mining")
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
        #endregion
    }
}
