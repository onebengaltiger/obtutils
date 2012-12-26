/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// A general interface for showing debugging message
	/// in a variety of ways
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public interface IMessenger
	{
		/// <summary>
		/// Send a debugging message to some output device
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside 
		/// the format string</param>
		void sendMessage(string format, params object []args);
	}
}
