/***************************************************************************
 *   Copyright (C) 2014 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Data;
using System.Data.Common;

using OBTUtils.Data.SQL;



namespace OBTUtils.Data.Java
{
	/// <summary>
	/// All supported Java DB entities managers
	/// </summary>
	public enum JavaDBEntitiesManagers {
		/// <summary>
		/// No entitites support
		/// </summary>
		none,
		
		/// <summary>
		/// ORM Lite support
		/// </summary>
		ormlite
	}
	
	
	/// <summary>
	/// 
	/// </summary>
	/// 
	/// <remarks>\author \rodolfo</remarks>
	public class POJOGenerator : GenericDB2CodeGenerator
	{
		/// <summary>
		/// Indicates how to generate additional code in the POJOs
		/// so that they are suitable for use with the specified
		/// Java entities manager
		/// </summary>
		private JavaDBEntitiesManagers entitysupport;
		
		/// <summary>
		/// Gets or sets a value that indicates how to generate additional code in the POJOs
		/// so that they are suitable for use with the specified
		/// Java entities manager
		/// </summary>
		public JavaDBEntitiesManagers Entitysupport {
			get { return entitysupport; }
			set { entitysupport = value; }
		}
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="providergenericname">ADO.Net database provider's
		/// invariant name</param>
		/// <param name="bdconnectionstring">The connection string to be used</param>
		public POJOGenerator(string providergenericname,
		                     string dbconnectionstring, JavaDBEntitiesManagers entitysupport)
			: base(DbProviderFactories.GetFactory(providergenericname),
			       dbconnectionstring)
		{
			this.entitysupport = entitysupport;
		}
		
		
		private string generateNewPOJO(DataTable thetable) {
			return "";
		}
		
		
		public string generatePOJO(string thetable) {
			return generateCode(thetable, generateNewPOJO);
		}
	}
}
