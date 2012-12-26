/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Sql;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using Npgsql;
using System.Configuration;

using OBTUtils.Messaging.Debug;



namespace DBSQLUtilsConsoleTests
{
	class Program
	{
		private static MessengersBoss dbgboss = new MessengersBoss();
		
		
		public static void Main(string[] args)
		{
			try {
				connectSQLServerExp();
				
				Program progobj = new Program();
				
				DbProviderFactory fact = NpgsqlFactory.Instance;
				
				DbConnection conn = fact.CreateConnection();
				DbConnectionStringBuilder connstrbuild = fact.CreateConnectionStringBuilder();
				
				
				connstrbuild.Add("Server", "127.0.0.1");
				connstrbuild.Add("Port", "5432");
				connstrbuild.Add("User Id", "rodolfo");
				connstrbuild.Add("Password", "lOCMFopn");
				connstrbuild.Add("Database", "CinturonApretado");
				
				//conn.ConnectionString = connstrbuild.ConnectionString;
				
//				conn.ConnectionString =
//					ConfigurationManager.ConnectionStrings["RawPostgreSQL"].ConnectionString;
//				DBSQLUtilsConsoleTestsContext obj;
//				dbgboss.sendMessage("Connection string is: {0}", connstrbuild.ConnectionString);
//				
//				Console.WriteLine("Connecting to PGSQL...");
//				conn.Open();
//				Console.WriteLine("Connected !!!");
//				
//				DbCommand selectCmd, insertCmd;
//				DataSet ds = new DataSet();
//				DbDataAdapter adapter = fact.CreateDataAdapter();
//				
//				selectCmd = conn.CreateCommand();
//				insertCmd = conn.CreateCommand();
//				selectCmd.CommandText = "SELECT * FROM RepartoPresupuestoDelMes";
//				selectCmd.CommandType = CommandType.Text;
//				
//				adapter.SelectCommand = selectCmd;
//				
//				adapter.Fill(ds, 0, 1, "RepartoPresupuestoDelMes");
//				
//				DataTable table = ds.Tables["RepartoPresupuestoDelMes"];
//				
//				if (table.Rows.Count <= 1)
//					Console.WriteLine("It worked !!!");
//				
//				Console.WriteLine("Opened table RepartoPresupuestoDelMes");
//				
//				if (table.ChildRelations.Count > 0) {
//					Console.WriteLine("The table has child relations !!!");
//				}
//				
//				if (table.Constraints.Count > 0) {
//					Console.WriteLine("The table has constrains defined !!!");
//				}
//				
//				if (table.ParentRelations.Count > 0) {
//					Console.WriteLine("The table has parent relations !!!");
//				}
//				
//				if (table.PrimaryKey.Length > 0) {
//					Console.WriteLine("The table has a primary key !!!");
//				}
//				
//				foreach (DataColumn col in table.Columns) {
//					Console.WriteLine("Columna {0} tiene el tipo de dato {1}",
//					                  col.ColumnName,
//					                  progobj.obtenTipoSQL(col));
//				}
//				
//				Console.WriteLine();
//				Console.WriteLine("Procedimiento Select para tabla {0} es",
//				                  table.TableName);
//				Console.WriteLine();
//				
//				Console.WriteLine(progobj.generaSelectSP(table));
//				System.IO.File.AppendAllText("select.sql", progobj.generaSelectSP(table));
//				
//				Console.WriteLine();
//				Console.WriteLine("Procedimiento insert para tabla {0} es",
//				                  table.TableName);
//				Console.WriteLine();
//				
//				Console.WriteLine(progobj.generaInsertSP(table));
//				System.IO.File.AppendAllText("insert.sql", progobj.generaInsertSP(table));
//				
//				
//				Console.WriteLine();
//				Console.WriteLine("Procedimiento update para tabla {0} es",
//				                  table.TableName);
//				Console.WriteLine();
//				
//				Console.WriteLine(progobj.generaUpdateSP(table));
//				System.IO.File.AppendAllText("update.sql", progobj.generaUpdateSP(table));
//				
//				
//				Console.WriteLine();
//				Console.WriteLine("Procedimiento delete para tabla {0} es",
//				                  table.TableName);
//				Console.WriteLine();
//				
//				Console.WriteLine(progobj.generaDeleteSP(table));
//				System.IO.File.AppendAllText("delete.sql", progobj.generaDeleteSP(table));
				
//				selectCmd.CommandText = "fnselectgastosdelmes";
//				selectCmd.CommandType = CommandType.StoredProcedure;
//				
//				DbDataReader reader = selectCmd.ExecuteReader();
				
//				if (reader.GetSchemaTable().Columns.Count > 1) {
//					Console.WriteLine("It works !!!");
//					Console.WriteLine(progobj.generaDeleteSP(reader.GetSchemaTable()));
//				}
				
//				if (reader.HasRows)
//					Console.WriteLine("Reader has rows");
//				else
//					Console.WriteLine("Reader has no rows");
//				
//				reader.Close();
//				
//				insertCmd.CommandType = CommandType.StoredProcedure;
//				insertCmd.CommandText = "spinsertpresupuestomes";
//				
//				DbParameter param;
//				
//				param = insertCmd.CreateParameter();
//				param.ParameterName = "fecha";
//				param.Value = DateTime.Now.Date;
//				insertCmd.Parameters.Add(param);
//				
//				param = insertCmd.CreateParameter();
//				param.ParameterName = "cantidadpresupuesto";
//				param.Value = 15000;
//				insertCmd.Parameters.Add(param);
//				
//				int nrows = insertCmd.ExecuteNonQuery();
//				
//				Console.WriteLine("Result of insert sp is: {0}", nrows);
//				
//				Console.WriteLine("Closing connection...");
//				conn.Close();
			} catch (Exception ex) {
				Console.WriteLine("There was an error during the execution of this program !!!");
				dbgboss.sendDebugMessage("Error detail is: {0}", ex);
			} finally {
				Console.Write("Press any key to exit the program . . . ");
				Console.ReadKey(true);
			}
		}
		
		static void connectSQLServerExp()
		{
			DbProviderFactory fact = System.Data.SqlClient.SqlClientFactory.Instance;
			DbConnection conn = fact.CreateConnection();
			
			conn.ConnectionString = "Data Source=127.0.0.1;Initial Catalog=model;" +
				"Integrated Security=SSPI;User ID=.\rodolfo;Password=lOCMFopn;";
			
			conn.Open();
			
			Console.WriteLine("SQL Server connected !!!");
			
			DbCommand cmd = conn.CreateCommand();
			
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = String.Format("SELECT * FROM PresupuestoMes",
			                               Environment.NewLine);
			cmd.ExecuteReader();
			
			Console.WriteLine("Success making database ??");
			
			Console.ReadKey(true);
			
			conn.Close();
		}
		
		
		private string obtenTipoSQL(DataColumn columna) {
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
				retVal = "FLOAT";
			else if (tipoColumna.Equals(typeof(Double)))
				retVal = "DOUBLE";
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
				retVal = String.Format("TIPODESCONOCIDO{0}", tipoColumna);
			}
			
			return retVal;
		}
		
		// papito linto no sirve esto no me lo gusta programar
		// mmm 8in 0 99 hi jajaj no entiendo nada
		
		private string generaSelectSP(DataTable table) {
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
		
		private string generaInsertSP(DataTable table) {
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
		
		private string generaUpdateSP(DataTable table) {
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
		
		private string generaDeleteSP(DataTable table) {
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
	}
}
