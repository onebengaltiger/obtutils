/***************************************************************************
 *   Copyright (C) 2010 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;



namespace OBTUtils.Data.SQL
{
	/// <summary>
	/// SQL code generator for MS SQL Server
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class SQLServerSQLGenerator : GenericSQLGenerator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SQLServerSQLGenerator(string connectionString) :
			base(SqlClientFactory.Instance, connectionString) { }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public SQLServerSQLGenerator() : base(SqlClientFactory.Instance) { }
	}
}
