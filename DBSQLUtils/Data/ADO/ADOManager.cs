/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using OBTUtils.Messaging;


namespace OBTUtils.Data.ADO
{
	/// <summary>
	/// A simple ADO.Net connections and operations manager
	/// </summary>
	/// 
	/// \author \rodolfo
	public class ADOManager : ComponentMessengers
	{
		/// <summary>
		/// The ADO.Net Factory object used to build specific
		/// BD Driver objects (connections, commands, . . .)
		/// </summary>
		protected DbProviderFactory dbFactory;
		
		
		/// <summary>
		/// The connection object
		/// </summary>
		protected DbConnection theconnection;
		
		/// <summary>
		/// The maximum waiting time for the execution of a command
		/// </summary>
		protected int timeoutCommandExecution_ms = 10 * 1000;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="afactory">The ADO.Net factory object</param>
		/// <param name="server">Server address</param>
		/// <param name="dbname">Database's name</param>
		/// <param name="username">User name for the database</param>
		/// <param name="passwd">Password of the user</param>
		/// <param name="messengers">Optional messengers' array</param>
		public ADOManager(DbProviderFactory afactory,
		                  string server, string dbname,
		                  string username, string passwd,
		                  params IMessenger []messengers)
			: this(afactory, messengers)
		{
			initConnection(server, dbname, username, passwd);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="afactory">The ADO.Net factory object</param>
		/// <param name="messengers">Optional messengers' array</param>
		/// <param name="connectionstring">Full connection string for the database
		/// connection</param>
		public ADOManager(DbProviderFactory afactory, string connectionstring,
		                  params IMessenger []messengers)
			: this(afactory, messengers)
		{
			initConnection(connectionstring);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="afactory">The ADO.Net factory object</param>
		/// <param name="messengers">Optional messengers' array</param>
		/// <remarks>The user must call one of the initConnection methods
		/// to begin works in the database</remarks>
		public ADOManager(DbProviderFactory afactory, params IMessenger []messengers)
			: base(messengers)
		{
			dbFactory = afactory;
		}
		
		/// <summary>
		/// Destructor
		/// </summary>
		~ADOManager() {
			if (theconnection != null && theconnection.State == ConnectionState.Open) {
				theconnection.Close();
			}
			
			theconnection = null;
			dbFactory = null;
		}
		
		
		/// <summary>
		/// Prepare the database connection using the given parameters
		/// </summary>
		/// <param name="server">Server's address</param>
		/// <param name="database">Database's name</param>
		/// <param name="username">The user name</param>
		/// <param name="passwd">User's password</param>
		public void initConnection(string server, string database,
		                           string username, string passwd) {
			DbConnectionStringBuilder csbuilder
				= dbFactory.CreateConnectionStringBuilder();
			
			csbuilder.Add("server", server);
			csbuilder.Add("user id", username);
			csbuilder.Add("password", passwd);
			csbuilder.Add("Initial catalog", database);
			
			theconnection = dbFactory.CreateConnection();
			theconnection.ConnectionString = csbuilder.ConnectionString;
		}
		
		/// <summary>
		/// Prepare the database connection using the given connection string
		/// </summary>
		/// <param name="connectionstring">Database driver's specific
		/// connection string</param>
		public void initConnection(string connectionstring) {
			theconnection = dbFactory.CreateConnection();
			theconnection.ConnectionString = connectionstring;
		}
		
		
		/// <summary>
		/// Gets or sets the maximum waiting time for the execution of a command
		/// </summary>
		public int TimeoutCommandExecution {
			get {
				return timeoutCommandExecution_ms / 1000;
			}
			
			set {
				if (value < 0) value = -value;
				
				timeoutCommandExecution_ms = value * 1000;
			}
		}
	}
}
