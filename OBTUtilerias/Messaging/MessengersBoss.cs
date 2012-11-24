/***************************************************************************
 *   Copyright (C) 2010 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Diagnostics;



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
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mainDebugger">Indicates which is the main messenger.
		/// This is
		/// used by the method sendMessage</param>
		/// <param name="someDebuggers">Array containing all the messengers</param>
		/// <see cref="sendMessage" />
		public MessengersBoss(int mainmessenger, params IMessenger []somemessengers)
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
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="someDebuggers">Array containing all the debuggers</param>
		/// <remarks>This constructor sets the first debugger in the array as the main
		/// debugger</remarks>
		/// <see cref="sendMessage" />
		public MessengersBoss(params IMessenger []someMessengers) :
			this(0, someMessengers) { }
		
		/// <summary>
		/// Constructor. The instance returned by this method has an unique
		/// debugger (an instance of DBGConsoleMessenger) and this is the main debugger
		/// </summary>
		/// <see cref="sendMessage" />
		/// <see cref="DBGConsoleMessenger" />
		public MessengersBoss() : this(0, new ConsoleMessenger()) { }
		
		
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
			if (messengers.Length > 0)
				messengers[theMainMessenger].sendMessage(format, args);
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
			foreach (IMessenger aMessenger in messengers)
				aMessenger.sendMessage(format, args);
		}
		
		
		/// <summary>
		/// Gets the array of messengers
		/// </summary>
		public IMessenger[] Messengers {
			get { 
				return messengers; 
			}
		}
		
		/// <summary>
		/// Sends a debug message using the main messenger
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		[Conditional("DEBUG")]
		public void sendDebugMessage(string format, params object []args)
		{
			sendMessage(format, args);
		}
		
		/// <summary>
		/// Broadcast a debug message using all the messengers
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		[Conditional("DEBUG")]
		public void broadcastDebugMessage(string format, params object []args)
		{
			broadcastMessage(format, args);
		}
	}
}
