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
	/// A general interface for showing messages
	/// in a variety of ways
	/// </summary>
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
		[Obsolete("This method is deprecated, use sendMessage(title, format, args) instead")]
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
