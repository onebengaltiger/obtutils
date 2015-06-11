/***************************************************************************
 *   Copyright (C) 2013-2015 by Rodolfo Conde Martínez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Sql;

using OBTUtils.Messaging;
using OBTUtils.Data;
using OBTUtils.Data.ADO;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// Basic class to implement SQL code generator for
	/// different DBMS.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class GenericSQLCodeGenerator : GenericDB2CodeGenerator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fact">The ADO.Net factory object</param>
		/// <param name="BDconnectionString">Full connection string for the database
		/// connection</param>
		protected GenericSQLCodeGenerator(DbProviderFactory fact,
		                                  string BDconnectionString)
			: base(fact, BDconnectionString)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fact">The ADO.Net factory object</param>
		public GenericSQLCodeGenerator(DbProviderFactory fact)
			: base(fact)
		{ }
		
		
		/// <summary>
		/// Get the SQL datatype associated with the table column
		///  (by examining the .Net datatype)
		/// </summary>
		/// <param name="column">A DataColumn</param>
		/// <returns>The SQL datatype associated with the column. If the function
		/// cannot find any suitable SQL datatype, the type returned will contain the
		///  substring UNKNOWNTYPE togheter with the original .Net datatype
		/// of the column</returns>
		protected virtual string getSQLType(DataColumn column) {
			Type columntype = column.DataType;
			string retVal = "VARCHAR(255)";
			
			if (columntype.Equals(typeof(byte)))
				retVal = "TINYINT";
			else if (columntype.Equals(typeof(sbyte)))
				retVal = "SMALLINT";
			else if (columntype.Equals(typeof(short)))
				retVal = "SMALLINT";
			else if (columntype.Equals(typeof(Int32)))
				retVal = "INTEGER";
			else if (columntype.Equals(typeof(Single))) // float
				retVal = "FLOAT(24)";
			else if (columntype.Equals(typeof(Double)))
				retVal = "FLOAT(53)";
			else if (columntype.Equals(typeof(Boolean)))
				retVal = "BIT";
			else if (columntype.Equals(typeof(Decimal)))
				retVal = "DECIMAL";
			else if (columntype.Equals(typeof(String)))
				retVal = "VARCHAR(255)";
			else if (columntype.Equals(typeof(DateTime)))
				retVal = "DATETIME";
			else {
				TheMessengersBoss.sendDebugMessage("getSQLType: " +
				                                   "Unknown data type: {0}",
				                                   columntype);
				retVal = String.Format("UNKNOWNTYPE{0}", columntype);
			}
			
			return retVal;
		}
		
		#region Basic SQL code generating functions
		
		/// <summary>
		/// Generate a SELECT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A SELECT statement for the table <c>tableName</c></returns>
		public string generateSelect(string tableName)
		{
			return generateCode(tableName, selectGenerator);
		}
		
		/// <summary>
		/// Generate a UPDATE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A UPDATE statement for the table <c>tableName</c></returns>
		public string generateUpdate(string tableName)
		{
			return generateCode(tableName, updateGenerator);
		}
		
		/// <summary>
		/// Generate a INSERT for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A INSERT statement for the table <c>tableName</c></returns>
		public string generateInsert(string tableName)
		{
			return generateCode(tableName, insertGenerator);
		}
		
		/// <summary>
		/// Generate a DELETE for the table <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A DELETE statement for the table <c>tableName</c></returns>
		public string generateDelete(string tableName)
		{
			return generateCode(tableName, deleteGenerator);
		}
		
		#endregion
		
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
				                      getSQLType(col));
				
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
				                      getSQLType(col));
				
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
				                      getSQLType(col));
				
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
				                      getSQLType(col));
				
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
