/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Text;


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
		/// Send a message to the standar output device
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
		
		/// <summary>
		/// Send a standar message to the standar output device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, null, format, args);
		}
		
		/// <summary>
		/// Send a warning message to the standar output device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendWarningMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, "Warning", format, args);
		}
		
		/// <summary>
		/// Send an information message to the standar output device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendInformationMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, "Information", format, args);
		}
		
		/// <summary>
		/// Send an error message to the standar output device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendErrorMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, "ERROR", format, args);
		}
		
		/// <summary>
		/// Internal method to send message to the standar output device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="subtitle">The subtitle of the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		private void mySendMessage(string title, string subtitle,
		                           string format, params object []args) {
			StringBuilder conscadena = new StringBuilder();
			
			if (!String.IsNullOrWhiteSpace(title))
				conscadena.AppendFormat("{0}: ", title);
			
			if (!String.IsNullOrWhiteSpace(subtitle))
				conscadena.AppendFormat("{0}: ", subtitle);
			
			conscadena.AppendFormat(format, args);
			
			Console.WriteLine(conscadena.ToString());
		}
	}
}
