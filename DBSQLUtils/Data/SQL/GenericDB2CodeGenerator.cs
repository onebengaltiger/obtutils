/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Sql;

using OBTUtils.Messaging;
using OBTUtils.Data.ADO;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// Basic class to implement SQL code generator for
	/// different DBMS.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class GenericDB2CodeGenerator : MessengerClass
	{
		/// <summary>
		/// The ADO.Net Manager used by this object
		/// </summary>
		private ADOManager dbmanager;
		
		/// <summary>
		/// Represents a code generator
		/// </summary>
		protected delegate string CodeGenerator(DataTable theTable);
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fact">The ADO.Net factory object</param>
		/// <param name="BDconnectionString">Full connection string for the database
		/// connection</param>
		protected GenericDB2CodeGenerator(DbProviderFactory fact,
		                                  string BDconnectionString)
		{
			dbmanager = new ADOManager(fact, BDconnectionString);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fact">The ADO.Net factory object</param>
		public GenericDB2CodeGenerator(DbProviderFactory fact)
		{
			dbmanager = new ADOManager(fact);
		}
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~GenericDB2CodeGenerator()
		{
			dbmanager.Dispose();
			dbmanager = null;
		}		
		
		
		/// <summary>
		/// Obtains the basic scheme and metadata information of the
		///  table named <c>tableName</c>
		/// </summary>
		/// <param name="tableName">The name of the table</param>
		/// <returns>A DataTable with the desired information</returns>
		protected DataTable getTableMetaDataFromBD(string tableName)
		{
			DbCommand selectCmd;
			DataSet ds = new DataSet();
			DbDataAdapter adapter = dbmanager.getAdapter();
			
			selectCmd = dbmanager.getCommand(
				CommandType.Text,
				String.Format("SELECT * FROM {0} WHERE 1 <> 1",
				              tableName),
				0
			);
			
			adapter.SelectCommand = selectCmd;
			
			adapter.Fill(ds, 0, 1, tableName);
			
			DataTable table = ds.Tables[tableName];
			
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
		protected string generateCode(string tableName, CodeGenerator sqlgenerator)
		{
			DataTable theTable;
			
			theTable = getTableMetaDataFromBD(tableName);
			
			return sqlgenerator(theTable);
		}
		
				
		/// <summary>
		/// Gets the database connection string
		/// </summary>
		public string ConnectionString {
			get {
				return dbmanager.Theconnection.ConnectionString;
			}
		}
	}
}
