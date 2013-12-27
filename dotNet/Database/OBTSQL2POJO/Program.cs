/***************************************************************************
 *   Copyright (C) 2014 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;

using OBTUtils.Data.Java;


namespace OBTSQL2POJO
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length < 3) {
				Console.WriteLine("Usage: {0} ProviderInvariantName ConnectionString " +
				                  "TableName [none|ormlite]",
				                  Environment.CommandLine);
				Console.WriteLine();
				
				Environment.Exit(0);
			} else {
				Console.WriteLine("Console SQL to Java POJOs code generator for multiple providers");
				Console.WriteLine();
				
				try {
					string providerinvariantname, connectionString,
					tableName, entitiesframework, generatedCode;
					POJOGenerator pojogen;
					JavaDBEntitiesManagers entitiesmanager;
					
					providerinvariantname = args[0];
					connectionString = args[1];
					tableName = args[2];
					
					if (args.Length >= 4)
						entitiesframework = args[3];
					else
						entitiesframework = "none";
					
					if (!JavaDBEntitiesManagers.TryParse(entitiesframework, out entitiesmanager)) {
						Console.WriteLine("Manejador de entidades no reconocido: {0}", entitiesframework);
						Environment.Exit(1);
					}
					
					pojogen = new POJOGenerator(
						providerinvariantname, connectionString, entitiesmanager
					);
					
					generatedCode = pojogen.generatePOJO(tableName);
					
//					switch (entitiesframework.ToLower()) {
//						case "none":
//							generatedCode = pojogen.generateSelect(tableName);
//							break;
//						case "ormlite":
//							generatedCode = pojogen.generateUpdate(tableName);
//							break;
//						case "all":
//							StringBuilder sbuilder = new StringBuilder();
//
//							sbuilder.AppendLine(pojogen.generateSelect(tableName));
//							sbuilder.AppendLine();
//							sbuilder.AppendLine(pojogen.generateInsert(tableName));
//							sbuilder.AppendLine();
//							sbuilder.AppendLine(pojogen.generateUpdate(tableName));
//							sbuilder.AppendLine();
//							sbuilder.AppendLine(pojogen.generateDelete(tableName));
//
//							generatedCode = sbuilder.ToString();
//							break;
//						default:
//							generatedCode = String.Format("UNKNOWN POJO CODE " +
//							                              "GENERATION PETITION: {0}",
//							                              entitiesframework);
//							break;
//					}
					
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