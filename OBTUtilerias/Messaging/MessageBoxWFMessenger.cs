/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;



namespace OBTUtils.Messaging
{	
	/// <summary>
	/// A simple Windows forms MessageBox debugging messenger
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class MessageBoxWFMessenger : IMessenger
	{
		/// <summary>
		/// The form to which the debugging messagebox
		/// belongs
		/// </summary>
		private IWin32Window myOwner;
		
		/// <summary>
		/// The title for the messagebox's message
		/// </summary>
		private string strTitle;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="owner">Container which owns the messagebox</param>
		public MessageBoxWFMessenger(IWin32Window owner)
		{
			myOwner = owner;
			
			try {
				ResourceManager resmanager;
				Assembly me = GetType().Assembly;
				
				resmanager = new ResourceManager("OBTUtils.Properties.OBTResources", me);
				
				strTitle = resmanager.GetString("messageboxdbgtitle");
			} catch (Exception anexception) {
				Console.WriteLine("An error occured inside DBGMessageBoxMessenger's constructor" +
				                  " related to resources management: {0}", anexception);
				
				strTitle = String.Empty;
			}
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
		/// <see cref="DBGMessenger.sendMessage" />
		public void sendMessage(string format, params object[] args)
		{			
			if (myOwner != null)
				MessageBox.Show(myOwner, String.Format(format, args), strTitle,
				                MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				MessageBox.Show(String.Format(format, args), strTitle,
				                MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
