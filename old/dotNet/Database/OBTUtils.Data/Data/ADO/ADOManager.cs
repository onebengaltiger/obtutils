/***************************************************************************
 *   Copyright (C) 2013-2015 by Rodolfo Conde Martínez                     *
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
		protected DbConnection __theconnection;
		
//		/// <summary>
//		/// The maximum waiting time for the execution of a command
//		/// </summary>
//		protected int timeoutCommandExecution_ms = 10 * 1000;
		
		
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		/// <param name="afactory">The ADO.Net factory object</param>
//		/// <param name="server">Server address</param>
//		/// <param name="dbname">Database's name</param>
//		/// <param name="username">User name for the database</param>
//		/// <param name="passwd">Password of the user</param>
//		/// <param name="messengers">Optional messengers' array</param>
//		public ADOManager(DbProviderFactory afactory,
//		                  string server, string dbname,
//		                  string username, string passwd,
//		                  params IMessenger []messengers)
//			: this(afactory, messengers)
//		{
//			initConnection(server, dbname, username, passwd);
//		}
		
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
//			if (theconnection != null
//			    && theconnection.State == ConnectionState.Open) {
//				TheMessengersBoss.sendDebugMessage("Intentando cerrar conexión de BD");
//				theconnection.Close();
//				TheMessengersBoss.sendDebugMessage("Se cerro la conexión de BD");
//			}
			
			__theconnection = null;
			dbFactory = null;
//			TheMessengersBoss.sendDebugMessage("Variables asignadas a null");
		}
		
		
//		/// <summary>
//		/// Prepare the database connection using the given parameters
//		/// </summary>
//		/// <param name="server">Server's address</param>
//		/// <param name="database">Database's name</param>
//		/// <param name="username">The user name</param>
//		/// <param name="passwd">User's password</param>
//		public void initConnection(string server, string database,
//		                           string username, string passwd) {
//			DbConnectionStringBuilder csbuilder
//				= dbFactory.CreateConnectionStringBuilder();
//
//			csbuilder.Add("server", server);
//			csbuilder.Add("user id", username);
//			csbuilder.Add("password", passwd);
//			csbuilder.Add("Initial catalog", database);
//
//			theconnection = dbFactory.CreateConnection();
//			theconnection.ConnectionString = csbuilder.ConnectionString;
//		}
		
		/// <summary>
		/// Prepare the database connection using the given connection string
		/// </summary>
		/// <param name="connectionstring">Database driver's specific
		/// connection string</param>
		public void initConnection(string connectionstring) {
			__theconnection = dbFactory.CreateConnection();
			__theconnection.ConnectionString = connectionstring;
		}
		
		/// <summary>
		/// Builds a new command object using the connection managed by this object
		/// </summary>
		/// <param name="cmdtype">Command type</param>
		/// <param name="commandtext">Text associated to the command</param>
		/// <param name="cmdtimeout">Timeout for command execution</param>
		/// <param name="parameters">An array of object, of even length. Each pair of
		/// objects in the array is mean to be a (name, value) pair used to
		/// build a parameter of the command</param>
		/// <returns>A database command in the ADO.Net framework, accordingly
		/// to the given values</returns>
		public DbCommand getCommand(CommandType cmdtype, string commandtext,
		                            int cmdtimeout, params object []parameters) {
			if (__theconnection != null) {
				DbCommand acommand = __theconnection.CreateCommand();
				
				acommand.CommandType = cmdtype;
				acommand.CommandText = commandtext;
				acommand.CommandTimeout = cmdtimeout;
				
				if (parameters.Length % 2 != 0)
					throw new ArgumentException(
						String.Format("The number of arguments for the " +
						              "paramters of the command is invalid: {0}",
						              parameters.Length)
					);
				else {
					for (int i = 0; i < parameters.Length; i += 2) {
						DbParameter aparameter = acommand.CreateParameter();
						
						aparameter.ParameterName = parameters[i].ToString();
						aparameter.Value = parameters[i + 1];
						
						acommand.Parameters.Add(aparameter);
					}
				}
				
				return acommand;
			} else
				throw new OBTDataException("This manager does not have a connection. " +
				                           "Impossible to build a command object !!");
		}
		
		/// <summary>
		/// Builds a new command object using the connection managed by this object
		/// </summary>
		/// <param name="cmdtype">Command type</param>
		/// <param name="commandtext">Text associated to the command</param>
		/// <param name="parameters">An array of object, of even length. Each pair of
		/// objects in the array is mean to be a (name, value) pair used to
		/// build a parameter of the command</param>
		/// <returns>A database command in the ADO.Net framework, accordingly
		/// to the given values</returns>
		public DbCommand getCommand(CommandType cmdtype, string commandtext,
		                            params object []parameters) {
			return getCommand(cmdtype, commandtext, 0, parameters);
		}
		
		/// <summary>
		/// Builds a new DbDataAdapter object using the connection
		/// managed by this object
		/// </summary>
		/// <returns>A new DbDataAdapter object using the connection
		/// managed by this object</returns>
		public DbDataAdapter getAdapter() {
			if (__theconnection != null)
				return dbFactory.CreateDataAdapter();
			else
				throw new OBTDataException("This manager does not have a connection. " +
				                           "Impossible to build an adapter object !!");
		}
		
		
//		/// <summary>
//		/// Gets or sets the maximum waiting time for the execution of a command
//		/// </summary>
//		public int TimeoutCommandExecution {
//			get {
//				return timeoutCommandExecution_ms / 1000;
//			}
//
//			set {
//				if (value < 0) value = -value;
//
//				timeoutCommandExecution_ms = value * 1000;
//			}
//		}
		
		/// <summary>
		/// Gets the connection object managed by this object
		/// </summary>
		public DbConnection Theconnection {
			get {
				return __theconnection;
			}
		}
	}
}
