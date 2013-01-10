/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Text;
using System.Diagnostics;

using OBTUtils.Messaging;


namespace OBTUtils.Enterprise.Messaging
{
	/// <summary>
	/// An implementation of IMessenger to manage messages
	/// in the Windows Event log (for example, it can be
	/// used in Windows services)
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class EventLogMessenger : IMessenger
	{
		/// <summary>
		/// 
		/// </summary>
		private EventLog evenlog;
		
		/// <summary>
		/// An object to be used to lock the access to
		/// the EventLog object. This is needed if the
		/// log is to be accessed by multiple threads
		/// </summary>
		private object lock_obj = new object();
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="eventlog">The event log to be managed by this object</param>
		public EventLogMessenger(EventLog eventlog) {
			this.evenlog = eventlog;
		}
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~EventLogMessenger() {
			evenlog = null;
		}
		
		
		/// <summary>
		/// Send a basic message to the event log
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		/// <remarks>This method is obsolete, please use the other methods
		/// in the interface</remarks>
		public void sendMessage(string format, params object[] args)
		{
			write2log(EventLogEntryType.Information, String.Empty,
			          format, args);
		}
		
		
		/// <summary>
		/// Write a message to the event log
		/// </summary>
		/// <param name="messagetype">Message's type</param>
		/// <param name="title">Message's title</param>
		/// <param name="format">Message's format string</param>
		/// <param name="args">The arguments for the format string</param>
		private void write2log(EventLogEntryType messagetype,
		                       string title, string format, params object []args) {
			StringBuilder strbuilder = new StringBuilder();
			
			if (!String.IsNullOrWhiteSpace(title))
				strbuilder.AppendFormat("{0}: ", title);
			
			strbuilder.AppendFormat(format, args);
			
			lock (lock_obj) {
				evenlog.WriteEntry(strbuilder.ToString(), messagetype);
			}
		}
		
		
		/// <summary>
		/// Send a standar message to the event log
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string title, string format, params object[] args)
		{
			write2log(EventLogEntryType.Information, title, format, args);
		}
		
		/// <summary>
		/// Send a warning message to the event log
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendWarningMessage(string title, string format, params object[] args)
		{
			write2log(EventLogEntryType.Warning, title, format, args);
		}
		
		/// <summary>
		/// Send an information message to the event log
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendInformationMessage(string title, string format,
		                                   params object[] args)
		{
			write2log(EventLogEntryType.Information, title, format, args);
		}
		
		/// <summary>
		/// Send an error message to the event log
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendErrorMessage(string title, string format, params object[] args)
		{
			write2log(EventLogEntryType.Error, title, format, args);
		}
	}
}
