/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;

using OBTUtils.Messaging.Debug;



namespace OBTUtilsALLTests
{
	class Program
	{
		public static void Main(string[] args)
		{
			MessengersBoss debugboss = new MessengersBoss(new MessageBoxMessenger());
			
			debugboss.sendDebugMessage("A debugging test");
			
			Console.WriteLine((string) null);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}