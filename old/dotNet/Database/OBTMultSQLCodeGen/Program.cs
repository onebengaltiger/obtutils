/***************************************************************************
 *   Copyright (C) 2013-2015 by Rodolfo Conde Martínez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Text;

using OBTUtils.Data.SQL;



namespace OBTMultSQLCodeGen
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Console SQL code generator for multiple providers");
			Console.WriteLine();
			
			if (args.Length < 4) {
				Console.WriteLine("Usage: {0} ProviderInvariantName ConnectionString " +
				                  "TableName [select|update|insert|delete|all]",
				                  Environment.CommandLine);
				Console.WriteLine();
//				Console.WriteLine("You can setup database information in the config file");
				Environment.Exit(0);
			} else {
				try {
					string providerinvariantname, connectionString,
					tableName, sqlToGenerate, generatedCode;
					MultipleSQLGenerator multsqlgen;
					
					providerinvariantname = args[0];
					connectionString = args[1];
					tableName = args[2];
					sqlToGenerate = args[3];
					
					multsqlgen = new MultipleSQLGenerator(
						providerinvariantname, connectionString
					);
					
					switch (sqlToGenerate.ToLower()) {
						case "select":
							generatedCode = multsqlgen.generateSelect(tableName);
							break;
						case "update":
							generatedCode = multsqlgen.generateUpdate(tableName);
							break;
						case "insert":
							generatedCode = multsqlgen.generateInsert(tableName);
							break;
						case "delete":
							generatedCode = multsqlgen.generateDelete(tableName);
							break;
						case "all":
							StringBuilder sbuilder = new StringBuilder();
							
							sbuilder.AppendLine(multsqlgen.generateSelect(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(multsqlgen.generateInsert(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(multsqlgen.generateUpdate(tableName));
							sbuilder.AppendLine();
							sbuilder.AppendLine(multsqlgen.generateDelete(tableName));

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
					Console.WriteLine("Error while executing the requested " +
					                  "operation: {0}{1}",
					                  Environment.NewLine, ex);
				}
			}

#if DEBUG			
			Console.Write("Press any key to finish . . . ");
			Console.ReadKey(true);
#endif
		}
	}
}