/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.ComponentModel;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// A base class for classes that must inherit from <c>Component</c> and
	/// desire to have messaging capabilities given by MessengersBoss
	/// and IMessenger.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	/// <seealso cref="IMessenger" />
	/// <seealso cref="MessengersBoss" />
	public class ComponentMessengers : Component
	{
		private MessengerClass realMessenger;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">A container to hold a reference to this
		/// component</param>
		/// <param name="messengers">optional array with all the
		/// messengers this class will have</param>
		public ComponentMessengers(IContainer container,
		                           params IMessenger []messengers) {
			if (container != null)
				container.Add(this);
			
			realMessenger = new MessengerClass(messengers);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="messengers">optional array with all the
		/// messengers this class will have</param>
		public ComponentMessengers(params IMessenger []messengers)
			: this(null, messengers) { }
		

		/// <summary>
		/// Free resources used by the current instance
		/// </summary>
		/// <param name="disposing"><c>true</c> to free all resources, <c>false</c>
		/// to free only managed resources</param>
		protected override void Dispose(bool disposing)
		{
			realMessenger = null;
			
			base.Dispose(disposing);
		}
		
		
		/// <summary>
		/// Gets the messengers manager
		/// </summary>
		public MessengersBoss Boss {
			get {
				return realMessenger.Boss;
			}
		}
	}
}
