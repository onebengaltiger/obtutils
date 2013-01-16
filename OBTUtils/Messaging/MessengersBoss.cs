/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// This is the class to be used inside programs for messenging purposes.
	/// The classes implementing IMessenger and this interface are not mean
	/// to be used directly, instead you should add all your messengers
	/// to an instance of this class, set your main messenger and use the methods
	/// sendMessage and broadcastMessage. This class supports the DEBUG macro, so that
	/// when this macro is not defined, no debugging output is generated.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
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
		/// <see cref="sendDebugMessage" />
		/// <see cref="broadcastDebugMessage" />
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
				throw new OBTException("No messengers were given in " +
				                       "arguments of MessengersBoss");
			
			if (somemessengers.Length > 0
			    && (mainmessenger < 0 || mainmessenger >= somemessengers.Length))
				throw new OBTException("Invalid argument for MessengersBoss: {0}. It " +
				                       "must be between 0 and {1}",
				                       mainmessenger, somemessengers.Length);
			
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
		/// <see cref="IMessenger.sendMessage" />
		public void sendMessage(string format, params object []args)
		{
			sendMessage(String.Empty, format, args);
		}
		
		/// <summary>
		/// Sends a message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendMessage" />
		public void sendMessage(string title, string format, params object []args)
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
			sendInformationMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends an information message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void sendInformationMessage(string title, string format,
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
			sendWarningMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends a warning message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void sendWarningMessage(string title, string format, params object []args)
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
			sendErrorMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Sends an error message using the main messenger
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void sendErrorMessage(string title, string format, params object []args)
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
		/// <see cref="IMessenger.sendMessage" />
		public void broadcastMessage(string format, params object []args)
		{
			broadcastMessage(String.Empty, format, args);
		}
		
		/// <summary>
		/// Broadcast a message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendMessage" />
		public void broadcastMessage(string title, string format,
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
			broadcastInformationMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast an information message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendInformationMessage" />
		public void broadcastInformationMessage(string title, string format,
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
			broadcastWarningMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast a warning message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendWarningMessage" />
		public void broadcastWarningMessage(string title, string format,
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
			broadcastErrorMessage(String.Empty, format, args);
		}

		/// <summary>
		/// Broadcast an error message using all the messengers
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <see cref="IMessenger.sendErrorMessage" />
		public void broadcastErrorMessage(string title, string format,
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
		public void sendDebugMessage(string title, string format, params object []args)
		{
			if (sendDebugging)
				sendMessage(title, format, args);
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
		public void broadcastDebugMessage(string title, string format,
		                                  params object []args)
		{
			if (sendDebugging)
				broadcastMessage(format, args);
		}
	}
}
