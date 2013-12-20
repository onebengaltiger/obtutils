/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;

namespace OBTUtils.Messaging
{
	/// <summary>
	/// An empty IMessenger, that is, a messenger
	/// that does not send any message at all
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class NoMessagesMessenger : IMessenger
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public NoMessagesMessenger()
		{
		}
		
		
		/// <summary>
		/// This method does nothing
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <remarks>This method is obsolete, please use the other methods
		/// in the interface</remarks>
		public void sendMessage(string format, params object[] args)
		{

		}
		
		/// <summary>
		/// This method does nothing
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string title, string format, params object[] args)
		{

		}
		
		/// <summary>
		/// This method does nothing
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendWarningMessage(string title, string format, params object[] args)
		{

		}
		
		/// <summary>
		/// This method does nothing
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendInformationMessage(string title, string format, params object[] args)
		{

		}
		
		/// <summary>
		/// This method does nothing
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendErrorMessage(string title, string format, params object[] args)
		{

		}
	}
}
