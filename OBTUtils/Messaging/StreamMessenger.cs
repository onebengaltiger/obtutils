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
	/// An implementation of the IMessenger interface to
	/// be used with Stream derived classes
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class StreamMessenger : IMessenger
	{
		/// <summary>
		/// The stream associated with this messenger
		/// </summary>
		private Stream mystream;
		
		/// <summary>
		/// A StreamWriter to write the messages to
		/// the stream
		/// </summary>
		private StreamWriter streamwriter;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="thestream">The stream associated with this messenger</param>
		/// <param name="theencoding">Encoding used to write the messages
		/// in the stream</param>
		public StreamMessenger(Stream thestream, Encoding theencoding)
		{
			mystream = thestream;
			streamwriter = new StreamWriter(mystream, theencoding);
			streamwriter.AutoFlush = true;
		}				
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="thestream">The stream associated with this messenger</param>
		/// <remarks>With this constructor, the system's default encoding is used to write
		///  the data and this is appended
		/// in the stream</remarks>
		public StreamMessenger(Stream thestream)
			: this(thestream, Encoding.Default)
		{ }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~StreamMessenger() {
			streamwriter.Dispose();
			
			streamwriter = null;
			mystream = null;
		}
		
		
		/// <summary>
		/// Send a message to the associated stream with the
		/// given parameters
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="subtitle">A subtitle for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		private void write(string title, string subtitle,
		                   string format, params object[] args)
		{
			StringBuilder strbuilder = new StringBuilder();
			
			if (!String.IsNullOrWhiteSpace(title))
				strbuilder.AppendFormat("{0}: ", title);
			
			if (!String.IsNullOrWhiteSpace(subtitle))
				strbuilder.AppendFormat("{0}: ", subtitle);
			
			strbuilder.AppendFormat(format, args);
			
			streamwriter.Write(strbuilder.ToString());
		}
		
		
		/// <summary>
		/// Send a basic message to the associated stream
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <remarks>This method is obsolete, please use the other methods
		/// in the interface</remarks>
		public void sendMessage(string format, params object[] args)
		{
			write(String.Empty, String.Empty, format, args);
		}
		
		/// <summary>
		/// Send a standar message to the associated stream
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string title, string format, params object[] args)
		{
			write(title, String.Empty, format, args);
		}
		
		/// <summary>
		/// Send a warning message to the associated stream
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendWarningMessage(string title, string format, params object[] args)
		{
			write(title, "Warning", format, args);
		}
		
		/// <summary>
		/// Send an information message to the associated stream
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendInformationMessage(string title, string format,
		                                   params object[] args)
		{
			write(title, "Information", format, args);
		}
		
		/// <summary>
		/// Send an error message to the associated stream
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendErrorMessage(string title, string format, params object[] args)
		{
			write(title, "Error", format, args);
		}
	}
}
