/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Text;
using System.Data;
using System.Data.Common;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// SQL code generator for multiple
	/// DBMS. Currently, we support the following servers:
	/// <list type="bullet">
	/// 	<item>
	/// 		<description>SQL Server</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>Postgre SQL Server</description>
	/// 	</item>
	/// 	<item>
	/// 		<description>MySQL Server</description>
	/// 	</item>
	/// </list>
	/// </summary>
	/// <remarks>\author \rodolfo</remarks>
	/// <remarks>A generic generator is provided that can be used to generate 
	/// code suitable for non-supported DBMS</remarks>
	public class MultipleSQLGenerator : GenericSQLCodeGenerator
	{
		/// <summary>
		/// Internal enumeration to represent all
		/// supported database providers
		/// </summary>
		private enum DBProviders {
			/// <summary>
			/// Generic provider
			/// </summary>
			DBPGeneric,
			
			/// <summary>
			/// SQL Server provider
			/// </summary>
			DBPSQLServer,
			
			/// <summary>
			/// MySQL provider
			/// </summary>
			DBPMySQL,
			
			/// <summary>
			/// PostgreSQL provider
			/// </summary>
			DBPPostgreSQL
		}
		
		
		/// <summary>
		/// Current database provider
		/// </summary>
		private DBProviders theprovider;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="providergenericname">ADO.Net database provider's
		/// invariant name</param>
		/// <param name="bdconnectionstring">The connection string to be used</param>
		public MultipleSQLGenerator(string providergenericname,
		                            string dbconnectionstring)
			: base(DbProviderFactories.GetFactory(providergenericname),
			       dbconnectionstring)
		{
			if (String.IsNullOrWhiteSpace(providergenericname))
				throw new ArgumentException("Parameter cannot be null or whitespace !!!",
				                            "providergenericname");
			
			string lowercaseprovidername = providergenericname.ToLower();
			
			if (lowercaseprovidername.Contains("npgsql"))
				theprovider = DBProviders.DBPPostgreSQL;
			else if (lowercaseprovidername.Contains("mysql"))
				theprovider = DBProviders.DBPMySQL;
			else if (lowercaseprovidername.Contains("sqlclient"))
				theprovider = DBProviders.DBPSQLServer;
			else
				theprovider = DBProviders.DBPGeneric;
		}
		
		
		/// <summary>
		/// Get the SQL datatype associated with the table column
		/// and specific to the database provider given when this object
		/// was constructed
		/// </summary>
		/// <param name="column">A DataColumn</param>
		/// <returns>The SQL datatype associated with the column. If the function
		/// cannot find any suitable SQL datatype, the type returned will contain the
		///  substring UNKNOWNTYPE togheter with the original .Net datatype
		/// of the column</returns>
		protected override string getSQLType(DataColumn column)
		{
			if (theprovider == DBProviders.DBPPostgreSQL)
				return getPGSQLType(column);
			
			return base.getSQLType(column);
		}
		
		/// <summary>
		/// Generate a provider specific SELECT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A SELECT statement for the table <c>tableName</c></returns>
		protected override string selectGenerator(DataTable table)
		{
			if (theprovider == DBProviders.DBPPostgreSQL)
				return pgsqlselectGenerator(table);
			
			if (theprovider == DBProviders.DBPMySQL)
				return mysqlselectGenerator(table);
			
			return base.selectGenerator(table);
		}
		
		/// <summary>
		/// Generate a provider specific INSERT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A INSERT statement for the table <c>tableName</c></returns>
		protected override string insertGenerator(DataTable table)
		{
			if (theprovider == DBProviders.DBPPostgreSQL)
				return pgsqlinsertGenerator(table);
			
			if (theprovider == DBProviders.DBPMySQL)
				return mysqlinsertGenerator(table);
			
			return base.insertGenerator(table);
		}
		
		/// <summary>
		/// Generate a provider specific UPDATE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A UPDATE statement for the table <c>tableName</c></returns>
		protected override string updateGenerator(DataTable table)
		{
			if (theprovider == DBProviders.DBPPostgreSQL)
				return pgsqlupdateGenerator(table);
			
			if (theprovider == DBProviders.DBPMySQL)
				return mysqlupdateGenerator(table);
			
			return base.updateGenerator(table);
		}
		
		/// <summary>
		/// Generate a provider specific DELETE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A DELETE statement for the table <c>tableName</c></returns>
		protected override string deleteGenerator(DataTable table)
		{
			if (theprovider == DBProviders.DBPPostgreSQL)
				return pgsqldeleteGenerator(table);
			
			if (theprovider == DBProviders.DBPMySQL)
				return mysqldeleteGenerator(table);
			
			return base.deleteGenerator(table);
		}
		
		
		#region Database provider's specific SQL type and code generators
		
		#region PostgreSQL code generators
		
		/// <summary>
		/// SELECT code generator PostgreSQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A SELECT statement for the DataTable <c>table</c></returns>
		private string pgsqlselectGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnSelect{0} (",
			                      table.TableName);
			sbuilder.AppendLine();
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("OUT {0} {1}", col.ColumnName,
				                      getSQLType(col));
				
//				if (i < table.Columns.Count - 1)
				sbuilder.AppendLine(",");
			}
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine("RETURNS SETOF RECORD");
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine("$$");
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
				sbuilder.AppendFormat("{0} = ${1}", col.ColumnName, i + 1);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine("LANGUAGE SQL;");
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// INSERT code generator PostgreSQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A INSERT statement for the DataTable <c>table</c></returns>
		private string pgsqlinsertGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnInsert{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine("RETURNS VOID");
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine("$$");
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
				//DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("${0}", i + 1);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine("LANGUAGE SQL;");
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// UPDATE code generator PostgreSQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A UPDATE statement for the DataTable <c>table</c></returns>
		private string pgsqlupdateGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnUpdate{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine("RETURNS VOID");
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("UPDATE {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("SET");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = ${1}", col.ColumnName, i + 1);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = ${1}", col.ColumnName, i + 1);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine("LANGUAGE SQL;");
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// DELETE code generator for PostgreSQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A DELETE statement for the DataTable <c>table</c></returns>
		private string pgsqldeleteGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnDelete{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine("RETURNS VOID");
			sbuilder.AppendLine("AS");
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("DELETE FROM {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = ${1}", col.ColumnName, i + 1);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			sbuilder.AppendLine("$$");
			sbuilder.AppendLine("LANGUAGE SQL;");
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// Get the PostgreSQL datatype associated with the table column
		///  (by examining the .Net datatype)
		/// </summary>
		/// <param name="columna">A DataColumn</param>
		/// <returns>The SQL datatype associated with the column. If the function
		/// cannot find any suitable SQL datatype, the type returned will contain the
		///  substring UNKNOWNTYPE togheter with the original .Net datatype
		/// of the column</returns>
		private string getPGSQLType(DataColumn column) {
			Type tipoColumna = column.DataType;
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
				retVal = "REAL";
			else if (tipoColumna.Equals(typeof(Double)))
				retVal = "FLOAT(53)";
			else if (tipoColumna.Equals(typeof(Boolean)))
				retVal = "BIT";
			else if (tipoColumna.Equals(typeof(Decimal)))
				retVal = "FLOAT(53)";
			else if (tipoColumna.Equals(typeof(String)))
				retVal = "VARCHAR(255)";
			else if (tipoColumna.Equals(typeof(DateTime)))
				retVal = "DATE";
			else {
				TheMessengersBoss.sendDebugMessage("getPGSQLType: " +
				                                   "Unknown datatype: {0}",
				                                   tipoColumna);
				retVal = String.Format("UNKNOWNTYPE{0}", tipoColumna);
			}
			
			return retVal;
		}
		
		#endregion
		
		#region MySQL code generators
		
		/// <summary>
		/// SELECT code generator for MySQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A SELECT statement for the DataTable <c>table</c></returns>
		private string mysqlselectGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spSelect{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
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
				sbuilder.AppendFormat("{0} = P_{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// INSERT code generator for MySQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A INSERT statement for the DataTable <c>table</c></returns>
		private string mysqlinsertGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spInsert{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
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
				sbuilder.AppendFormat("P_{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// UPDATE code generator for MySQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A UPDATE statement for the DataTable <c>table</c></returns>
		private string mysqlupdateGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spUpdate{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("UPDATE {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("SET");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = P_{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = P_{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		/// <summary>
		/// DELETE code generator for MySQL provider
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A DELETE statement for the DataTable <c>table</c></returns>
		private string mysqldeleteGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spDelete{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      getSQLType(col));
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(",");
			}
			
			sbuilder.AppendLine();
			sbuilder.AppendLine(")");
			sbuilder.AppendLine();
			
			sbuilder.AppendFormat("DELETE FROM {0}", table.TableName);
			sbuilder.AppendLine();
			sbuilder.AppendLine("WHERE");
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("{0} = P_{0}", col.ColumnName);
				
				if (i < table.Columns.Count - 1)
					sbuilder.AppendLine(" AND");
			}
			
			sbuilder.AppendLine();
			
			
			return sbuilder.ToString();
		}
		
		#endregion
		
		#endregion
	}
}
