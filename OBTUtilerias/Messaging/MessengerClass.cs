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
		/// The debuggers manager
		/// </summary>
		private MessengersBoss boss;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbgMessengers">optional array with all the 
		/// debug messengers this class will have</param>
		public MessengerClass(params IMessenger []messengers)
		{
			if (messengers != null && messengers.Length > 0)
				boss = new MessengersBoss(messengers);
			else
				boss = new MessengersBoss();
		}
		
		/// <summary>
		/// Destructor
		/// </summary>
		~MessengerClass() {
			boss = null;
		}
		
		
		/// <summary>
		/// Gets the messengers manager
		/// </summary>
		public MessengersBoss Boss {
			get {
				return boss;
			}
		}
	}
}
