/***************************************************************************
 *   Copyright (C) 2011-2015 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;



namespace OBTUtils.Messaging
{
	/// <summary>
	/// A simple Windows forms MessageBox messenger
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class MessageBoxWFMessenger : IMessenger
	{
		/// <summary>
		/// The form to which the messagebox
		/// belongs
		/// </summary>
		private IWin32Window myOwner;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="owner">Container which owns the messagebox</param>
		public MessageBoxWFMessenger(IWin32Window owner)
		{
			myOwner = owner;
			
//			try {
//				ResourceManager resmanager;
//				Assembly me = GetType().Assembly;
//
//				resmanager = new ResourceManager("OBTUtils.Properties.OBTResources", me);
//
//				strTitle = resmanager.GetString("messageboxdbgtitle");
//			} catch (Exception anexception) {
//				Console.WriteLine("An error occured inside DBGMessageBoxMessenger's constructor" +
//				                  " related to resources management: {0}", anexception);
//
//				strTitle = String.Empty;
//			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MessageBoxWFMessenger() : this(null)
		{
		}
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~MessageBoxWFMessenger()
		{
			myOwner = null;
		}
		
		
		/// <summary>
		/// Send a debugging message inside a
		/// windows forms MessageBox
		/// </summary>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string format, params object[] args)
		{
			if (myOwner != null)
				MessageBox.Show(myOwner, String.Format(format, args), String.Empty,
				                MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				MessageBox.Show(String.Format(format, args), String.Empty,
				                MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		/// <summary>
		/// Send a standar message using a messagebox
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, MessageBoxIcon.Information, format, args);
		}
		
		/// <summary>
		/// Send a warning message using a messagebox
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendWarningMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, MessageBoxIcon.Warning, format, args);
		}
		
		/// <summary>
		/// Send an information message using a messagebox
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendInformationMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, MessageBoxIcon.Information, format, args);
		}
		
		/// <summary>
		/// Send an error message using a messagebox
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		public void sendErrorMessage(string title, string format, params object[] args)
		{
			mySendMessage(title, MessageBoxIcon.Error, format, args);
		}
		
		
		/// <summary>
		/// Generic method to send message using a Windows Forms messagebox
		/// </summary>
		/// <param name="title">Title for the message</param>
		/// <param name="icon">Image to be displayed next to the text of
		/// the message</param>
		/// <param name="format">Formatting string</param>
		/// <param name="args">arguments to be replaced inside
		/// the format string</param>
		private void mySendMessage(string title, MessageBoxIcon icon,
		                           string format, params object[] args) {
			if (myOwner != null)
				MessageBox.Show(myOwner, String.Format(format, args), title,
				                MessageBoxButtons.OK, icon);
			else
				MessageBox.Show(String.Format(format, args), title,
				                MessageBoxButtons.OK, icon);
		}
	}
}
