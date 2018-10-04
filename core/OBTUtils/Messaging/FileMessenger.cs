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
using System.IO;
using System.Text;



namespace OBTUtils.Messaging
{
	/// <summary>
	/// A simple implementation of the IMessenger interface to be used
	/// with files
	/// </summary>
	public class FileMessenger : StreamMessenger
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">File path</param>
		/// <param name="fmode">FileMode to open the file</param>
		/// <param name="theencoding">Encoding used to write the messages
		/// in the file</param>
		/// <param name="automaticnewline">Contains a value indicating if a new line
		/// should be written to the associated file each time
		/// a message is send</param>
		public FileMessenger(string path, FileMode fmode, Encoding theencoding,
		                     bool automaticnewline)
			: base(new FileStream(path, fmode), theencoding,
			       automaticnewline)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">File path</param>
		/// <param name="fmode">FileMode to open the file</param>
		/// <param name="automaticnewline">Contains a value indicating if a new line
		/// should be written to the associated file each time
		/// a message is send</param>
		/// <remarks>The encoding used is the default
		/// system's encoding</remarks>
		public FileMessenger(string path, FileMode fmode, bool automaticnewline)
			: base(new FileStream(path, fmode), Encoding.Default, automaticnewline)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">File path</param>
		/// <param name="automaticnewline">Contains a value indicating if a new line
		/// should be written to the associated file each time
		/// a message is send</param>
		/// <remarks>The FileMode to open the file with this constructor is
		/// FileMode.Append and the encoding used is the default
		/// system's encoding</remarks>
		public FileMessenger(string path, bool automaticnewline)
			: base(new FileStream(path, FileMode.Append), Encoding.Default,
			       automaticnewline)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">File path</param>
		/// <remarks>The FileMode to open the file with this constructor is
		/// FileMode.Append; the encoding used is the default
		/// system's encoding and each time a message is send using this messenger,
		/// a new line is written at the end of each message</remarks>
		public FileMessenger(string path)
			: base(new FileStream(path, FileMode.Append), Encoding.Default,
			       true)
		{ }
	}
}
