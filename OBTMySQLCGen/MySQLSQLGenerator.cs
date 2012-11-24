/***************************************************************************
 *   Copyright (C) 2010 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// SQL code generator for MS SQL Server
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class MySQLSQLGenerator : GenericSQLGenerator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MySQLSQLGenerator(string connectionString) :
			base(MySqlClientFactory.Instance, connectionString) { }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MySQLSQLGenerator() : base(MySqlClientFactory.Instance) { }
		
		
		#region MySQL code generators
		
		/// <summary>
		/// SELECT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A SELECT statement for the DataTable <c>table</c></returns>
		protected override string selectGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spSelect{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// INSERT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A INSERT statement for the DataTable <c>table</c></returns>
		protected override string insertGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spInsert{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// UPDATE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A UPDATE statement for the DataTable <c>table</c></returns>
		protected override string updateGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROC spUpdate{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// DELETE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A DELETE statement for the DataTable <c>table</c></returns>
		protected override string deleteGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE PROCEDURE spDelete{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("IN P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
	}
}
