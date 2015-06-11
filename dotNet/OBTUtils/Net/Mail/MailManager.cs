/***************************************************************************
 *   Copyright (C) 2011-2015 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.IO;
using System.Security;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

using OBTUtils.Messaging;



namespace OBTUtils.Net.Mail
{
	/// <summary>
	/// A class to send email messages with attachments
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class MailManager : ComponentMessengers
	{
		/// <summary>
		/// Maximum waiting time to complete the operations by the smtp client
		/// </summary>
		private int timeoutSMTPClient = 180;  // 3 minutes
		
		/// <summary>
		/// Maximum numbers of times for trying to send mails in the method
		/// sendMail
		/// </summary>
		private byte ntryTosendMails = 1;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">The container in which we store this component</param>
		/// <param name="timeoutsmptclient">Maximum waiting time to complete the
		/// operations by the smtp client</param>
		/// <param name="ntrytosendemail">Maximum numbers of times
		/// to try to send mails in the method sendMail</param>
		/// <param name="messengers">All available messengers to be used
		/// in this component</param>
		public MailManager(Container container, int timeoutsmptclient,
		                   byte ntrytosendemail,
		                   params IMessenger []messengers)
			: this(container, timeoutsmptclient, messengers)
		{
			NtryTosendMails = ntrytosendemail;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">The container in which we store this component</param>
		/// <param name="ntrytosendemail">Maximum numbers of times
		/// to try to send mails in the method sendMail</param>
		/// <param name="messengers">All available messengers to be used in
		/// this component</param>
		public MailManager(Container container,
		                   byte ntrytosendemail,
		                   params IMessenger []messengers)
			: this(container, messengers)
		{
			NtryTosendMails = ntrytosendemail;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">The container in which we store this component</param>
		/// <param name="timeoutsmptclient">Maximum waiting time to complete the
		/// operations by the smtp client</param>
		/// <param name="messengers">All available messengers to be used in
		/// this component</param>
		public MailManager(Container container, int timeoutsmptclient,
		                   params IMessenger []messengers)
			: base(container, messengers)
		{
			TimeoutSMTPClient = timeoutsmptclient;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">The container in which we store this component</param>
		/// <param name="messengers">All available messengers to be used in
		/// this component</param>
		public MailManager(Container container,
		                   params IMessenger []messengers)
			: base(container, messengers)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MailManager()
			: this(null, null)
		{ }
		
		
		/// <summary>
		/// Send an email from the address <c>fromAddress</c> to the given adresses in
		/// <c>tosAddresses</c>
		/// </summary>
		/// <param name="username">Name of the <c>fromAddress</c>'s name</param>
		/// <param name="password">Password of <c>fromAddress</c>'s account</param>
		/// <param name="smtpserver">Domain name or IP address of the SMTP server</param>
		/// <param name="serverport">SMTP Server's port</param>
		/// <param name="withssl">Enable or diable SSL when sending the email</param>
		/// <param name="fromAddress">The address of the person who is sending the email</param>
		/// <param name="tosAddresses">All the addresses which will reveive the email</param>
		/// <param name="subject">Email's subject</param>
		/// <param name="body">Email's contents</param>
		/// <param name="attatchments">All the attachments of the email</param>
		public void sendMail(string username, SecureString password,
		                     string smtpserver,
		                     int serverport,
		                     bool withssl,
		                     MailAddress fromAddress, ICollection<MailAddress> tosAddresses,
		                     string subject, string body,
		                     IEnumerable<Attachment> attatchments) {
			MailMessage msg = new MailMessage();
			byte local_ntryToSendMails = ntryTosendMails;
			
			msg.From = fromAddress;
			if (tosAddresses != null && tosAddresses.Count != 0)
				foreach (MailAddress ma in tosAddresses) msg.To.Add(ma);
			else
				throw new OBTException("Impossible to send email without a recipient !!!");
			
			msg.Subject = subject;
			msg.Body = body;
			//msg.IsBodyHtml = false;
			
			if (attatchments != null)
				foreach (Attachment a in attatchments)
					msg.Attachments.Add(a);
			
			SmtpClient smtpc = new SmtpClient(smtpserver, serverport);
			
			smtpc.Credentials = new NetworkCredential(username, password);
			smtpc.Timeout = timeoutSMTPClient * 1000;
			smtpc.EnableSsl = withssl;
			
			while (local_ntryToSendMails-- > 0) {
				try {
					smtpc.Send(msg);
					break;
				} catch (Exception anerror) {
					TheMessengersBoss.broadcastTitleDebugMessage("Excepción al intentar mandar mensaje" +
					                                             " de correo:{0}{1}",
					                                             Environment.NewLine, anerror);
					
					if (local_ntryToSendMails <= 0)
						throw;
//					else if (local_ntryToSendMails == 1)
//						smtpc.Timeout *= 2;
				}
			}
		}
		
		/// <summary>
		/// Send an email from the address <c>fromAddress</c> to the given adresses in
		/// <c>tosAddresses</c>
		/// </summary>
		/// <param name="username">Name of the <c>fromAddress</c>'s name</param>
		/// <param name="password">Password of <c>fromAddress</c>'s account</param>
		/// <param name="smtpserver">Domain name or IP address of the SMTP server</param>
		/// <param name="serverport">SMTP Server's port</param>
		/// <param name="withssl">Enable or diable SSL when sending the email</param>
		/// <param name="fromAddress">The address of the person who is sending the email</param>
		/// <param name="toAddresses">The address which will reveive the email</param>
		/// <param name="subject">Email's subject</param>
		/// <param name="body">Email's contents</param>
		public void sendMail(string username, SecureString password,
		                     string smtpserver,
		                     int serverport,
		                     bool withssl,
		                     MailAddress fromAddress, MailAddress toAddresses,
		                     string subject, string body) {
			sendMail(username, password, smtpserver, serverport, withssl,
			         fromAddress, new MailAddress [] { toAddresses },
			         subject, body,  (Attachment []) null);
		}
		
		/// <summary>
		/// Builds an attachment (an instance of the .Net Attachment class)
		/// with the contents of the stream contents
		/// </summary>
		/// <param name="attname">The attachment's name</param>
		/// <param name="contents">The attachment's contents</param>
		/// <param name="contentstype">Attachment's MIME type
		/// (i.e. .pdf, .txt, .jpg, etc)</param>
		/// <returns>An object of type attachment with the information given
		/// in the arguments</returns>
		public static Attachment buildAttachment(string attname, Stream contents,
		                                         string contentstype)
		{
			Attachment att = new Attachment(contents, contentstype);
			att.Name = attname;
			
			return att;
		}
		
//		public string buildFileName(string archivoDeQueTrata, DateTime dt, string extension) {
//			char []invalidos = Path.GetInvalidFileNameChars();
//
//			if (extension == null || extension.IndexOfAny(invalidos) >= 0)
//				throw new OBTException("El argumento de extension es invalido !!",
//				                       "extension");
//
//			if (String.IsNullOrEmpty(archivoDeQueTrata) || archivoDeQueTrata.IndexOfAny(invalidos) >= 0)
//				return String.Format("{0} {1:dd}-{1:MM}-{1:yy}.{2}", "reporte", dt, extension);
//
//			return String.Format("{0} {1} {2:dd}-{2:MM}-{2:yy}.{3}",
//			                     "reporte", archivoDeQueTrata, dt, extension);
//		}
		
		/// <summary>
		/// Gets or sets the maximum waiting time (in seconds) to complete
		/// the operations by the smtp client
		/// </summary>
		public int TimeoutSMTPClient {
			get {
				return timeoutSMTPClient;
			}
			
			set {
				if (value < 0)
					throw new OBTException("MailManager: The timeout value {0} " +
					                       "is invalid !!!", value);
				
				timeoutSMTPClient = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the maximum numbers of times for trying
		/// to send mails in the method sendMail
		/// </summary>
		public byte NtryTosendMails {
			get {
				return ntryTosendMails;
			}
			
			set {
				if (value <= 0)
					throw new OBTException("MailManager: The value {0} " +
					                       "is invalid for NtryTosendMails property !!!",
					                       value);
				
				ntryTosendMails = value;
			}
		}
		
	}
}
