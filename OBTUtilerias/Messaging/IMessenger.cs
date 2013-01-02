/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// A general interface for showing messages
	/// in a variety of ways
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public interface IMessenger
	{
		/// <summary>
		/// Send a basic message to some output device
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <remarks>This method is obsolete, please use the other methods
		/// in the interface</remarks>
		[Obsolete]
		void sendMessage(string format, params object []args);
		
		/// <summary>
		/// Send a standar message to some ouput device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		void sendMessage(string title, string format, params object []args);
		
		/// <summary>
		/// Send a warning message to some ouput device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		void sendWarningMessage(string title, string format, params object []args);
		
		/// <summary>
		/// Send an information message to some ouput device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		void sendInformationMessage(string title, string format, params object []args);
		
		/// <summary>
		/// Send an error message to some ouput device
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		void sendErrorMessage(string title, string format, params object []args);
	}
}
