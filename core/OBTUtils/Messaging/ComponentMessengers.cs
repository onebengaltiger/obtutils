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
using System.ComponentModel;


namespace OBTUtils.Messaging
{
	/// <summary>
	/// A base class for classes that must inherit from <c>Component</c> and
	/// desire to have messaging capabilities given by MessengersBoss
	/// and IMessenger.
	/// </summary>
	/// 
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
		[Obsolete("This property is deprecated, use TheMessengersBoss instead")]
		public MessengersBoss Boss {
			get {
				return realMessenger.Boss;
			}
		}
		
		/// <summary>
		/// Gets the messengers manager
		/// </summary>
		public MessengersBoss TheMessengersBoss {
			get {
				return realMessenger.TheMessengersBoss;
			}
		}
	}
}
