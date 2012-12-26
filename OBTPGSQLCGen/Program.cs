/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Configuration;

using OBTUtils.Messaging;
using OBTUtils.Data.SQL;


namespace OBTPGSQLCGen
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Console SQL code generator for PostgreSQL Server");
			Console.WriteLine();
			
			if (args.Length < 2) {
				Console.WriteLine("Usage: {0} tableName [select|update|insert|delete|all]",
				                  Environment.CommandLine);
				Console.WriteLine();
				Console.WriteLine("You can setup database information in the config file");
				Environment.Exit(0);
			} else {
				try {
					string connectionString =
						ConfigurationManager.ConnectionStrings["PGSQLCS"].ConnectionString,
					tableName, sqlToGenerate, generatedCode;
					PGSQLGenerator pgsqlgen =
						new PGSQLGenerator(connectionString);
					
					tableName = args[0];
					sqlToGenerate = args[1];
					
					switch (sqlToGenerate.ToLower()) {
						case "select":
							generatedCode = pgsqlgen.generateSelect(tableName);
							break;
						case "update":
							generatedCode = pgsqlgen.generateUpdate(tableName);
							break;
						case "insert":
							generatedCode = pgsqlgen.generateInsert(tableName);
							break;
						case "delete":
							generatedCode = pgsqlgen.generateDelete(tableName);
							break;
						case "all":
							StringBuilder sbuilder = new StringBuilder();
							
							sbuilder.AppendLine(pgsqlgen.generateSelect(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(pgsqlgen.generateInsert(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(pgsqlgen.generateUpdate(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(pgsqlgen.generateDelete(tableName));

							generatedCode = sbuilder.ToString();
							break;
						default:
							generatedCode = String.Format("UNKNOWN SQL CODE " +
							                              "GENERATION PETITION: {0}",
							                              sqlToGenerate);
							break;
					}
					
					Console.WriteLine(generatedCode);
				} catch (Exception ex) {
					Console.WriteLine("Error while executing the requested operation: {0}{1}",
					                  Environment.NewLine, ex);
				}
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}