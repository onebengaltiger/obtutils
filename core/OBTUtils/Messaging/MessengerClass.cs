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
	/// A base class for classes which desire to use the messaging capabilities
	/// given by <see cref="IMessenger" /> and <see cref="MessengersBoss" />
	/// </summary>
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
