/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.IO;
using System.Text;



namespace OBTUtils.Messaging
{
	/// <summary>
	/// A simple implementation of the IMessenger interface to be used
	/// with files
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
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
		/// <param name="automaticnewline">Contains a value indicating if a new line
		/// should be written to the associated file each time
		/// a message is send</param>
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
