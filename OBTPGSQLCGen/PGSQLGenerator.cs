/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Npgsql;


namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// SQL code generator for PostgreSQL Server
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class PGSQLGenerator : GenericSQLGenerator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public PGSQLGenerator(string connectionString) :
			base(NpgsqlFactory.Instance, connectionString) { }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PGSQLGenerator() : base(NpgsqlFactory.Instance) { }
		
		
		#region PostgreSQL code generators
		
		/// <summary>
		/// SELECT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A SELECT statement for the DataTable <c>table</c></returns>
		protected override string selectGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnSelect{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("OUT {0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
//				if (i < table.Columns.Count - 1)
				sbuilder.AppendLine(",");
			}
			
			for (int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// INSERT code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A INSERT statement for the DataTable <c>table</c></returns>
		protected override string insertGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnInsert{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// UPDATE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A UPDATE statement for the DataTable <c>table</c></returns>
		protected override string updateGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnUpdate{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// DELETE code generator
		/// </summary>
		/// <param name="table">A DataTable</param>
		/// <returns>A DELETE statement for the DataTable <c>table</c></returns>
		protected override string deleteGenerator(DataTable table) {
			StringBuilder sbuilder = new StringBuilder();
			
			sbuilder.AppendFormat("CREATE OR REPLACE FUNCTION fnDelete{0} (", table.TableName);
			sbuilder.AppendLine();
			
			for(int i = 0; i < table.Columns.Count; ++i) {
				DataColumn col = table.Columns[i];
				sbuilder.AppendFormat("P_{0} {1}", col.ColumnName,
				                      obtenTipoSQL(col));
				
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
		/// Get the (Postgre)SQL datatype associated with the table column
		///  (by examining the .Net datatype)
		/// </summary>
		/// <param name="columna">A DataColumn</param>
		/// <returns>The SQL datatype associated with the column. If the function
		/// cannot find any suitable SQL datatype, the type returned will contain the
		///  substring UNKNOWNTYPE togheter with the original .Net datatype
		/// of the column</returns>
		protected override string obtenTipoSQL(DataColumn columna) {
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
				dbgboss.sendDebugMessage("obtenTipoSQL: " +
				                    "Tipo de dato no manejado: {0}",
				                    tipoColumna);
				retVal = String.Format("UNKNOWNTYPE{0}", tipoColumna);
			}
			
			return retVal;
		}
		
		#endregion
	}
}
