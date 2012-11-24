/***************************************************************************
 *   Copyright (C) 2010 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Sql;

using OBTUtils.Messaging;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// Basic class to implement SQL code generator for
	/// different DBMS.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class GenericSQLGenerator
	{
		/// <summary>
		/// ADO.Net Provider factory
		/// </summary>
		private DbProviderFactory factory;
		
		/// <summary>
		/// Connection to the database server
		/// </summary>
		private DbConnection connection;
		
		
		/// <summary>
		/// Represents a SQL code generator
		/// </summary>
		protected delegate string SQLCodeGenerator(DataTable theTable);
		
		/// <summary>
		/// For debugging purposes
		/// </summary>
		protected MessengersBoss dbgboss;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		protected GenericSQLGenerator(DbProviderFactory fact, 
		                               string BDconnectionString)
		{
			dbgboss = new MessengersBoss();
			
			factory = fact;
			
			connection =  factory.CreateConnection();
			connection.ConnectionString = BDconnectionString;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericSQLGenerator(DbProviderFactory fact) : 
			this(fact, String.Empty) { }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~GenericSQLGenerator()
		{
			factory = null;

			if (connection.State == ConnectionState.Open)
				connection.Close();
			
			connection = null;
			
			dbgboss = null;
		}
		
		
		/// <summary>
		/// Get the SQL datatype associated with the table column
		///  (by examining the .Net datatype)
		/// </summary>
		/// <param name="columna">A DataColumn</param>
		/// <returns>The SQL datatype associated with the column. If the function
		/// cannot find any suitable SQL datatype, the type returned will contain the
		///  substring UNKNOWNTYPE togheter with the original .Net datatype
		/// of the column</returns>
		protected virtual string obtenTipoSQL(DataColumn columna) {
			Type tipoColumna = columna.DataType;
			string retVal = "VARCHAR(255)";
			
			if (tipoColumna.Equals(typeof(byte)))
				retVal = "TINYINT";
			else if (tipoColumna.Equals(typeof(sbyte)))
				retVal = "SMALLINT";
			else if (tipoColumna.Equals(typeof(short)))
				retVal = "SMALLINT";
			else if (tipoColumna.Equals(typeof(Int32)))
				retVal = "INTEGER";
			else if (tipoColumna.Equals(typeof(Single))) // float
				retVal = "FLOAT(24)";
			else if (tipoColumna.Equals(typeof(Double)))
				retVal = "FLOAT(53)";
			else if (tipoColumna.Equals(typeof(Boolean)))
				retVal = "BIT";
			else if (tipoColumna.Equals(typeof(Decimal)))
				retVal = "DECIMAL";
			else if (tipoColumna.Equals(typeof(String)))
				retVal = "VARCHAR(255)";
			else if (tipoColumna.Equals(typeof(DateTime)))
				retVal = "DATETIME";
			else {
				dbgboss.sendDebugMessage("obtenTipoSQL: " +
				                    "Tipo de dato no manejado: {0}",
				                    tipoColumna);
				retVal = String.Format("UNKNOWNTYPE{0}", tipoColumna);
			}
			
			return retVal;
		}
		
		/// <summary>
		/// Obtains the basic scheme and metadata information of the
		///  table named <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A DataTable with the desired information</returns>
		protected DataTable getTableMetaDataFromBD(string tableName)
		{
//			dbgboss.sendDebugMessage("Connection string is: {0}",
//			                    connection.ConnectionString);
//			
//			dbgboss.sendDebugMessage("Connecting to DB...");
			connection.Open();
//			dbgboss.sendDebugMessage("Connected !!!");
			
			DbCommand selectCmd;
			DataSet ds = new DataSet();
			DbDataAdapter adapter = factory.CreateDataAdapter();
			
			selectCmd = connection.CreateCommand();
			selectCmd.CommandText = String.Format("SELECT * FROM {0} WHERE 1 <> 1", tableName);
			selectCmd.CommandType = CommandType.Text;
			
			adapter.SelectCommand = selectCmd;
			
			adapter.Fill(ds, 0, 1, tableName);
			
			DataTable table = ds.Tables[tableName];
			
//			if (table.Rows.Count <= 1)
//				dbgboss.sendDebugMessage("It worked !!!");
			
			connection.Close();
			
			return table;
		}
		
		/// <summary>
		/// Generates SQL code for the table <c>tableName</c> using the SQL code generator
		///  <c>sqlgenerator</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <param name="sqlgenerator">SQL code generator</param>
		/// <returns>A string that contains the generated SQL code for the table
		/// <c>tableName</c></returns>
		private string generateSQLCode(string tableName, SQLCodeGenerator sqlgenerator)
		{
			DataTable theTable;
			
			theTable = getTableMetaDataFromBD(tableName);
			
			return sqlgenerator(theTable);
		}
		
		
		#region Basic SQL code generating functions
		
		/// <summary>
		/// Generate a SELECT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A SELECT statement for the table <c>tableName</c></returns>
		public string generateSelect(string tableName)
		{
			return generateSQLCode(tableName, selectGenerator);
		}
		
		/// <summary>
		/// Generate a UPDATE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A UPDATE statement for the table <c>tableName</c></returns>
		public string generateUpdate(string tableName)
		{
			return generateSQLCode(tableName, updateGenerator);
		}
		
		/// <summary>
		/// Generate a INSERT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A INSERT statement for the table <c>tableName</c></returns>
		public string generateInsert(string tableName)
		{
			return generateSQLCode(tableName, insertGenerator);
		}
		
		/// <summary>
		/// Generate a DELETE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A DELETE statement for the table <c>tableName</c></returns>
		public string generateDelete(string tableName)
		{
			return generateSQLCode(tableName, deleteGenerator);
		}
		
		#endregion
		
		
		/// <summary>
		/// Gets or sets the DB connection string
		/// </summary>
		public string ConnectionString {
			get {
				return connection.ConnectionString;
			}
			
			set {
				connection.ConnectionString = value;
			}
		}
		
		
		#region Basic SQL Generators
		
		/// <summary>
		/// SELECT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A SELECT statement for the DataTable <c>table</c></returns>
		protected virtual string selectGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spSelect{0}", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("@{0} AS {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine();
			
			sbuilder.AppendLine("SELECT  -- *");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendFormat("FROM {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = @{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// INSERT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A INSERT statement for the DataTable <c>table</c></returns>
		protected virtual string insertGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spInsert{0}", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("@{0} AS {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("INSERT INTO {0} (", table.TableName);
			sbuilder.AppendLine();
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(") VALUES (");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("@{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// UPDATE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A UPDATE statement for the DataTable <c>table</c></returns>
		protected virtual string updateGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spUpdate{0}", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("@{0} AS {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("UPDATE {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("SET");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = @{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = @{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// DELETE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A DELETE statement for the DataTable <c>table</c></returns>
		protected virtual string deleteGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spDelete{0}", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("@{0} AS {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("DELETE FROM {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = @{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		#endregion
	}
}
