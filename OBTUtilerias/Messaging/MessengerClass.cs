/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;



namespace OBTUtils.Messaging
{
	/// <summary>
	/// A base class for classes which desire to use the messaging capabilities
	/// given by <see cref="IMessenger" /> and <see cref="MessengersBoss" />
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	/// <seealso cref="IMessenger" />
	/// <seealso cref="MessengersBoss" />
	public class MessengerClass
	{
		/// <summary>
		/// The messengers' manager
		/// </summary>
		private MessengersBoss boss;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="debuggingoutput">true to send debugging output
		/// using the messengers; false otherwise</param>
		/// <param name="messengers">optional array with all the
		/// messengers this class can use</param>
		public MessengerClass(bool debuggingoutput, params IMessenger []messengers)
		{
			if (messengers != null && messengers.Length > 0)
				boss = new MessengersBoss(debuggingoutput, messengers);
			else
				boss = new MessengersBoss();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="messengers">optional array with all the
		/// messengers this class can use</param>
		public MessengerClass(params IMessenger []messengers)
			: this(true, messengers)
		{ }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~MessengerClass() {
			boss = null;
		}
		
		
		/// <summary>
		/// Gets the messengers manager
		/// </summary>
		[Obsolete("This property is deprecated, use TheMessengersBoss instead")]
		public MessengersBoss Boss {
			get {
				return boss;
			}
		}
		
		/// <summary>
		/// Gets the messengers manager
		/// </summary>
		public MessengersBoss TheMessengersBoss {
			get {
				return boss;
			}
		}
	}
}
