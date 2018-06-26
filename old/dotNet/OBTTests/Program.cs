/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Net.Mail;
using System.Security;
using OBTUtils;
using OBTUtils.Messaging;
using OBTUtils.Net.Mail;
using OBTUtils.Utilities;

namespace OBTUtils.Tests
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("OBT SDK Tests !!!");
			
			Console.WriteLine();
			
			Console.WriteLine("Testing the MailManager. . .");
			
			MailManager mmanager = new MailManager(null, 60, 1, new ConsoleMessenger());
			SecureString secure = new SecureString();			
			
			
			secure.MakeReadOnly();
			
//			mmanager.sendMail("gps@gasmetropolitano.com.mx", secure, "192.168.15.3", 25, true,
//			                  new MailAddress("gps@gasmetropolitano.com.mx", "GPS"),
//			                  new MailAddress [] { new MailAddress("rcm@gmx.co.uk", "RCM") },
//			                  "Prueba de envio de correo", "Prueba básica", null
//			                 );
			
			// Secure method uses STARTTLS on port 587
			mmanager.sendMail("onebengaltiger@gmail.com", secure, "smtp.googlemail.com", 587, true,
			                  new MailAddress("onebengaltiger@gmail.com", "OBT"),
			                  new MailAddress [] { new MailAddress("rcm@gmx.co.uk", "RCM") },
			                  "Prueba de envio de correo", "Prueba básica", null
			                 );
			
			Console.Write("Email send. Press any key to finish . . . ");
			Console.ReadKey(true);
		}
	}
}