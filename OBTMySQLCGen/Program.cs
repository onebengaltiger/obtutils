/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;
using System.Configuration;

using OBTUtils.Messaging;
using OBTUtils.Data.SQL;



namespace OBTMySQLCGen
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Console SQL code generator for MySQL Server");
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
						ConfigurationManager.ConnectionStrings["MySQLCS"].ConnectionString,
					tableName, sqlToGenerate, generatedCode;
					MySQLSQLGenerator mysqlgen =
						new MySQLSQLGenerator(connectionString);
					
					tableName = args[0];
					sqlToGenerate = args[1];
					
					switch (sqlToGenerate.ToLower()) {
						case "select":
							generatedCode = mysqlgen.generateSelect(tableName);
							break;
						case "update":
							generatedCode = mysqlgen.generateUpdate(tableName);
							break;
						case "insert":
							generatedCode = mysqlgen.generateInsert(tableName);
							break;
						case "delete":
							generatedCode = mysqlgen.generateDelete(tableName);
							break;
						case "all":
							StringBuilder sbuilder = new StringBuilder();
							
							sbuilder.AppendLine(mysqlgen.generateSelect(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(mysqlgen.generateInsert(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(mysqlgen.generateUpdate(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(mysqlgen.generateDelete(tableName));

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
			
			Console.Write("Press any key to finish execution . . . ");
			Console.ReadKey(true);
		}
	}
}