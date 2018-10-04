/*
	This file is part of OBTUtils.

    OBTUtils is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    OBTUtils is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with OBTUtils.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// This is the class to be used inside programs for messenging purposes.
	/// The classes implementing IMessenger and this interface are not mean
	/// to be used directly, instead you should add all your messengers
	/// to an instance of this class, set your main messenger and use the methods
	/// sendXXXMessage and broadcastXXXMessage.
	/// </summary>
	public sealed class MessengersBoss
	{
		/// <summary>
		/// Array containing all the messengers
		/// </summary>
		private IMessenger []messengers;
		
		/// <summary>
		/// Indicates which is the main messenger. This is
		/// used by the method sendMessage
		/// </summary>
		/// <see cref="sendMessage" />
		private int theMainMessenger;
		
		/// <summary>
		/// Gets or sets the main messenger's index in the
		/// array of messengers
		/// </summary>
		public int TheMainMessenger {
			get {
				return theMainMessenger;
			}
			
			set {
				if (0 > value || messengers.Length <= value)
					throw new ArgumentOutOfRangeException("TheMainMessenger",
					                                      "Argument out of range !!");
				
				theMainMessenger = value;
			}
		}
		
		/// <summary>
		/// True to send debugging message to the output
		/// devices; false otherwise
		/// </summary>
		/// <see cref="MessengersBoss.sendDebugMessage" />
		/// <see cref="MessengersBoss.broadcastDebugMessage" />
		private bool sendDebugging;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mainmessenger">Indicates which is the main messenger.
		/// This is used by the method sendMessage</param>
		/// <param name="senddebugoutput">True to send debugging message to the output
		/// devices; false otherwise</param>
		/// <param name="somemessengers">Array containing all the messengers</param>
		public MessengersBoss(int mainmessenger, bool senddebugoutput,
		                      params IMessenger []somemessengers)
		{
			if (somemessengers == null)
				throw new ArgumentException("No messengers were given in " +
				                  		    "arguments of MessengersBoss");
			
			if (somemessengers.Length > 0
			    && (mainmessenger < 0 || mainmessenger >= somemessengers.Length))
				throw new ArgumentException(
					String.Format("Invalid argument for MessengersBoss: {0}. It " +
				    "must be between 0 and {1}",
				    mainmessenger, somemessengers.Length),
					"mainmessenger"
				);
			
			messengers = new IMessenger[somemessengers.Length];
			Array.Copy(somemessengers, messengers, somemessengers.Length);
			
			theMainMessenger = mainmessenger;
			sendDebugging = senddebugoutput;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="senddebugoutput">True to send debugging message to the output
		/// devices; false otherwise</param>
		/// <param name="someMessengers">Array containing all the debuggers</param>
		/// <remarks>This constructor sets the first debugger in the array as the main
		/// debugger</remarks>
		/// <see cref="sendMessage" />
		public MessengersBoss(bool senddebugoutput, params IMessenger []someMessengers) :
			this(0, senddebugoutput, someMessengers) { }
		
		/// <summary>
		/// Constructor. The instance returned by this method has an unique
		/// debugger (an instance of ConsoleMessenger) and this is the main debugger
		/// </summary>
		/// <param name="senddebugoutput">True to send debugging message to the output
		/// devices; false otherwise</param>
		/// <see cref="sendMessage" />
		/// <see cref="ConsoleMessenger" />
		public MessengersBoss(bool senddebugoutput)
			: this(0, senddebugoutput, new ConsoleMessenger())
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MessengersBoss()
			: this(true) { }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~MessengersBoss()
		{
			messengers = null;
		}
		
		
		/// <summary>
		/// Sends a message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string format, params object []args)
		{
			sendTitleMessage(String.Empty, format, args);
		}
		
		/// <summary>
		/// Sends a message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="OBTUtils.Messaging.IMessenger.sendMessage" />
		public void sendTitleMessage(string title, string format, params object []args)
		{
			if (messengers.Length > 0)
				messengers[theMainMessenger].sendMessage(title, format, args);
		}
		
		/// <summary>
		/// Sends an information message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void sendInformationMessage(string format,
		                                   params object []args)
		{
			sendTitleInformationMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends an information message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void sendTitleInformationMessage(string title, string format,
		                                        params object []args)
		{
			if (messengers.Length > 0)
				messengers[theMainMessenger].sendInformationMessage(
					title, format, args
				);
		}
		
		/// <summary>
		/// Sends a warning message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void sendWarningMessage(string format, params object []args)
		{
			sendTitleWarningMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends a warning message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void sendTitleWarningMessage(
			string title, string format, params object []args
		)
		{
			if (messengers.Length > 0)
				messengers[theMainMessenger].sendWarningMessage(title, format, args);
		}
		
		/// <summary>
		/// Sends an error message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void sendErrorMessage(string format, params object []args)
		{
			sendTitleErrorMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends an error message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void sendTitleErrorMessage(
			string title, string format, params object []args
		)
		{
			if (messengers.Length > 0)
				messengers[theMainMessenger].sendErrorMessage(title, format, args);
		}
		
		/// <summary>
		/// Broadcast a message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastMessage(string format, params object []args)
		{
			broadcastTitleMessage(String.Empty, format, args);
		}
		
		/// <summary>
		/// Broadcast a message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastTitleMessage(string title, string format,
		                                  params object []args)
		{
			foreach (IMessenger aMessenger in messengers)
				aMessenger.sendMessage(title, format, args);
		}
		
		/// <summary>
		/// Broadcast an information message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void broadcastInformationMessage(string format,
		                                        params object []args)
		{
			broadcastTitleInformationMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast an information message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void broadcastTitleInformationMessage(string title, string format,
		                                             params object []args)
		{
			foreach (IMessenger aMessenger in messengers)
				aMessenger.sendInformationMessage(title, format, args);
		}
		
		/// <summary>
		/// Broadcast a warning message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void broadcastWarningMessage(string format,
		                                    params object []args)
		{
			broadcastTitleWarningMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast a warning message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void broadcastTitleWarningMessage(string title, string format,
		                                         params object []args)
		{
			foreach (IMessenger aMessenger in messengers)
				aMessenger.sendWarningMessage(title, format, args);
		}
		
		/// <summary>
		/// Broadcast an error message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void broadcastErrorMessage(string format,
		                                  params object []args)
		{
			broadcastTitleErrorMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast an error message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void broadcastTitleErrorMessage(string title, string format,
		                                       params object []args)
		{
			foreach (IMessenger aMessenger in messengers)
				aMessenger.sendErrorMessage(title, format, args);
		}
		
		
//		/// <summary>
//		/// Gets the array of messengers
//		/// </summary>
//		public IMessenger[] Messengers {
//			get {
//				return messengers;
//			}
//		}
		
		/// <summary>
		/// Indicates whether debugging output is turn on
		/// </summary>
		public bool SendDebugging {
			get {
				return sendDebugging;
			}
			
			set {
				sendDebugging = value;
			}
		}
		
		
		/// <summary>
		/// Sends a debug message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendDebugMessage(string format, params object []args)
		{
			if (sendDebugging)
				sendMessage(format, args);
		}
		
		/// <summary>
		/// Sends a debug message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendTitleDebugMessage(
			string title, string format, params object []args
		)
		{
			if (sendDebugging)
				sendTitleMessage(title, format, args);
		}
		
		/// <summary>
		/// Broadcast a debug message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastDebugMessage(string format, params object []args)
		{
			if (sendDebugging)
				broadcastMessage(format, args);
		}
		
		/// <summary>
		/// Broadcast a debug message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastTitleDebugMessage(string title, string format,
		                                       params object []args)
		{
			if (sendDebugging)
				broadcastMessage(format, args);
		}
		
		/// <summary>
		/// Sends a debug message using the main messenger, and
		/// then throws the given exception anexception
		/// </summary>
		/// <param name="anexception">An exception to be thrown after
		/// the message is output</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendExceptionMessage(Exception anexception,
		                                 string format, params object []args)
		{
			sendDebugMessage(format, args);
			throw anexception;
		}
		
		/// <summary>
		/// Sends a debug message using the main messenger, and
		/// then throws the given exception anexception
		/// </summary>
		/// <param name="anexception">An exception to be thrown after
		/// the message is output</param>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendTitleExceptionMessage(
			Exception anexception,
			string title, string format, params object []args
		)
		{
			sendTitleDebugMessage(title, format, args);
			throw anexception;
		}
		
		/// <summary>
		/// Broadcast a debug message using all the messengers, and
		/// then throws the given exception anexception
		/// </summary>
		/// <param name="anexception">An exception to be thrown after
		/// the message is output</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastExceptionMessage(
			Exception anexception,
			string format, params object []args
		)
		{
			broadcastDebugMessage(format, args);
			throw anexception;
		}
		
		/// <summary>
		/// Broadcast a debug message using all the messengers, and
		/// then throws the given exception anexception
		/// </summary>
		/// <param name="anexception">An exception to be thrown after
		/// the message is output</param>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void broadcastTitleDebugMessage(
			Exception anexception,
			string title, string format,
			params object []args
		)
		{
			broadcastTitleDebugMessage(title, format, args);
			throw anexception;
		}
	}
}
