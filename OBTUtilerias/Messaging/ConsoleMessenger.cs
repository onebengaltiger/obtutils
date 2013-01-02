/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;



namespace OBTUtils.Messaging
{
	/// <summary>
	/// A simple console messenger
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class ConsoleMessenger : IMessenger
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConsoleMessenger()
		{
		}
		
		/// <summary>
		/// Send a message to the
		/// console standard output
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
	}
}
