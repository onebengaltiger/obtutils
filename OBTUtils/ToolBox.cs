/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

using OBTUtils.Messaging;



namespace OBTUtils
{
	/// <summary>
	/// Various utilities to be used in programs
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public static class Toolbox
	{
		/// <summary>
		/// Check if the two SecureString's are equal
		/// </summary>
		/// <param name="asecurestring">A secure string (SecureString)</param>
		/// <param name="anothersecurestring">A secure string (SecureString)</param>
		/// <returns>true if the two strings are equal; false otherwise</returns>
		public static unsafe bool areSecureStringsEqual(SecureString asecurestring,
		                                                SecureString anothersecurestring)
		{
			if (asecurestring.Length != anothersecurestring.Length)
				return false;
			
			IntPtr ptrastring, ptranotherstring;
			char *castring;
			char *cotherstring;
			bool retVal = true;

			ptrastring = Marshal.SecureStringToBSTR(asecurestring);
			ptranotherstring = Marshal.SecureStringToBSTR(anothersecurestring);
			
			castring = (char *) ptrastring.ToPointer();
			cotherstring = (char *) ptranotherstring.ToPointer();
			
			for (int i = 0; i < asecurestring.Length; ++i) {
				if (castring[i] != cotherstring[i])
					retVal = false;
				
				castring[i] = cotherstring[i] = '\0';
			}
			
			castring = cotherstring = null;
			
			Marshal.ZeroFreeBSTR(ptrastring);
			Marshal.ZeroFreeBSTR(ptranotherstring);
			
			return retVal;
		}
		
		/// <summary>
		/// Builds a string representation (in hex format) of a portion of
		/// the array of bytes
		/// </summary>
		/// <param name="buffer">An array of bytes</param>
		/// <param name="initialposition">The initial position in the array from which
		/// the string will be build</param>
		/// <param name="length">This parameter says how many bytes to copy into the
		/// returned string</param>
		/// <returns>A string representation, in hexadecimal format, of a portion of
		/// the array of bytes</returns>
		public static string byteArray2String(byte []buffer,
		                                      int initialposition, int length) {
			return byteArray2StringBuilder(buffer, initialposition, length).ToString();
		}
		
		/// <summary>
		/// Builds a string representation (in hex format) of
		/// the array of bytes
		/// </summary>
		/// <param name="buffer">An array of bytes</param>
		/// <returns>A string representation, in hexadecimal format, of a portion of
		/// the array of bytes</returns>
		public static string byteArray2String(byte []buffer) {
			return byteArray2String(buffer, 0, buffer.Length);
		}
		
		/// <summary>
		/// Builds a char array representation (with characters in hex format)
		/// of a portion of the array of bytes
		/// </summary>
		/// <param name="buffer">An array of bytes</param>
		/// <param name="initialposition">The initial position in the array from which
		/// the string will be build</param>
		/// <param name="length">This parameter says how many bytes to copy into the
		/// returned string</param>
		/// <returns>A character arrya representation, with characters in hexadecimal
		/// format, of a portion of the array of bytes</returns>
		public static char[] byteArray2charArray(byte []buffer,
		                                         int initialposition, int length) {
			StringBuilder strbuilder =
				byteArray2StringBuilder(buffer, initialposition, length);
			char []retVal = new char[strbuilder.Length];
			
			strbuilder.CopyTo(0, retVal, 0, retVal.Length);
			
			return retVal;
		}
		
		/// <summary>
		/// Builds a char array representation (with characters in hex format)
		/// of a portion of the array of bytes
		/// </summary>
		/// <param name="buffer">An array of bytes</param>
		/// <returns>A character array representation, with characters in hexadecimal
		/// format, of a portion of the array of bytes</returns>
		public static char[] byteArray2charArray(byte []buffer) {
			return byteArray2charArray(buffer, 0, buffer.Length);
		}
		
		
		/// <summary>
		/// Builds a StringBuilder containing a buffer of chars representing
		/// (with characters in hex format) a portion of the array of bytes
		/// </summary>
		/// <param name="buffer">An array of bytes</param>
		/// <param name="initialposition">The initial position in the array from which
		/// the string will be build</param>
		/// <param name="length">This parameter says how many bytes to copy into the
		/// returned string</param>
		/// <returns>A StringBuilder object representation, with characters in hexadecimal
		/// format, of a portion of the array of bytes</returns>
		private static StringBuilder byteArray2StringBuilder(byte []arreglo,
		                                                     int principio, int cuantos) {
			if (principio < 0 || principio > arreglo.Length - 1)
				throw new ArgumentOutOfRangeException("El argumento principio es invalido !!");
			
			if (cuantos <= 0 || cuantos > arreglo.Length)
				throw new ArgumentOutOfRangeException("El argumento cuantos es invalido !!");
			
			StringBuilder sbuilder = new StringBuilder();
			
			for (int i = principio; i < cuantos; ++i)
				sbuilder.AppendFormat("{0:x2}", arreglo[i]);
			
			return sbuilder;
		}
		
		/// <summary>
		/// Gets a string representation of the given bytes quantity in
		/// the parameter <c>bytes</c>
		/// </summary>
		/// <param name="bytes">the number of bytes</param>
		/// <returns>A string representation of the given bytes quantity</returns>
		public static string formatBytes2Megabytes(double bytes) {
			return String.Format("{0:.##}", bytes / 1048576.0);
		}
		
		/// <summary>
		/// Gets a string representation of the given bytes quantity in
		/// the parameter <c>bytes</c>
		/// </summary>
		/// <param name="bytes">the number of bytes</param>
		/// <returns>A string representation of the given bytes quantity</returns>
		public static string formatBytes2Megabytes(float bytes) {
			return String.Format("{0:.##}", bytes / 1048576.0);
		}
		
		/// <summary>
		/// Gets a string representation of the given bytes quantity in
		/// the parameter <c>bytes</c>
		/// </summary>
		/// <param name="bytes">the number of bytes</param>
		/// <returns>A string representation of the given bytes quantity</returns>
		public static string formatBytes2Megabytes(decimal bytes) {
			return String.Format("{0:.##}", bytes / (decimal) 1048576.0);
		}

		/// <summary>
		/// Gets a string representation of the given bytes quantity in
		/// the parameter <c>bytes</c>
		/// </summary>
		/// <param name="bytes">the number of bytes</param>
		/// <returns>A string representation of the given bytes quantity</returns>
		public static string formatBytes2Megabytes(int bytes) {
			return String.Format("{0:.##}", bytes / 1048576.0);
		}
		
		/// <summary>
		/// Builds a string that contains the list of all the objects given
		/// in the collection theobjects
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <returns>A string that contains the list of all the objects given
		/// in the collection theobjects</returns>
		public static string dumpObjects(IEnumerator theobjects) {
			StringBuilder builder = new StringBuilder();
			
			dumpObjects(theobjects, builder);
			
			return builder.ToString();
		}
		
		/// <summary>
		/// Prints in the buffer of the StringBuilder strbuilder all the elements
		/// of the collection theobjects
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		public static void dumpObjects(IEnumerator theobjects,
		                               StringBuilder strbuilder) {
			object anobject;
			bool movenext;
			
			theobjects.Reset();
			movenext = theobjects.MoveNext();
			
			while (movenext) {
				strbuilder.AppendFormat("{0}{1}",
				                        theobjects.Current,
				                        ((movenext = theobjects.MoveNext()) == true
				                         ? ", " : String.Empty));
			}
		}
		
		/// <summary>
		/// Builds a string that contains the list of all the objects given
		/// in the collection theobjects
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <returns>A string that contains the list of all the objects given
		/// in the collection theobjects</returns>
		public static string dumpObjects(ICollection theobjects) {
			return dumpObjects(theobjects);
		}
		
		/// <summary>
		/// Prints in the buffer of the StringBuilder strbuilder all the elements
		/// of the collection theobjects
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		public static void dumpObjects(ICollection theobjects,
		                               StringBuilder strbuilder) {
			dumpObjects(theobjects, strbuilder);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using one of the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="title">A title for the message containing the objects of
		/// the collection theobjects</param>
		/// <param name="usethismessenger">An index for the messenger's boss theboss,
		/// which is used to choose which messenger to use</param>
		public static void dumpObjects(IEnumerator theobjects,
		                               MessengersBoss theboss,
		                               string title, int usethismessenger) {
			StringBuilder builder = new StringBuilder();
			object anobject;
			bool movenext;
			int previousmessenger;
			
			theobjects.Reset();
			movenext = theobjects.MoveNext();
			
			while (movenext) {
				builder.AppendFormat("{0}{1}",
				                     theobjects.Current,
				                     ((movenext = theobjects.MoveNext()) == true
				                      ? ", " : String.Empty));
			}
			
			previousmessenger = theboss.TheMainMessenger;
			theboss.TheMainMessenger = usethismessenger;
			
			theboss.sendMessage(title, builder.ToString());
			
			theboss.TheMainMessenger = previousmessenger;
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using the main messenger of the object theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="title">A title for the message containing the objects of
		/// the collection theobjects</param>
		public static void dumpObjects(IEnumerator theobjects,
		                               MessengersBoss theboss,
		                               string title) {
			dumpObjects(theobjects, theboss, title, theboss.TheMainMessenger);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using the main messenger of the object theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		public static void dumpObjects(IEnumerator theobjects,
		                               MessengersBoss theboss) {
			dumpObjects(theobjects, theboss, String.Empty);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using all the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="title">A title for the message containing the objects of
		/// the collection theobjects</param>
		public static void dumpObjectsBroadcast(IEnumerator theobjects,
		                                        MessengersBoss theboss,
		                                        string title) {
			StringBuilder builder = new StringBuilder();
			bool movenext;
			
			theobjects.Reset();
			movenext = theobjects.MoveNext();
			
			while (movenext) {
				builder.AppendFormat("{0}{1}",
				                     theobjects.Current,
				                     ((movenext = theobjects.MoveNext()) == true
				                      ? ", " : String.Empty));
			}
			
			theboss.broadcastMessage(title, builder.ToString());
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using one of the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="usethismessenger">An index for the messenger's boss theboss, which
		///  is used to choose which messenger to use</param>
		public static void dumpObjects(IEnumerator theobjects,
		                               MessengersBoss theboss,
		                               int usethismessenger) {
			dumpObjects(theobjects, theboss, String.Empty, usethismessenger);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using all the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		public static void dumpObjectsBroadcast(IEnumerator theobjects,
		                                        MessengersBoss theboss) {
			dumpObjectsBroadcast(theobjects, theboss, String.Empty);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="title">A title for the message containing the objects of
		/// the collection theobjects</param>
		/// <param name="usethismessenger">An index for the messenger's boss theboss, which
		///  is used to choose which messenger to use</param>
		public static void dumpObjects(ICollection theobjects,
		                               MessengersBoss theboss,
		                               string title, int usethismessenger) {
			dumpObjects(theobjects, theboss, title, usethismessenger);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using all the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="title">A title for the message containing the objects of
		/// the collection theobjects</param>
		public static void dumpObjectsBroadcast(ICollection theobjects,
		                                        MessengersBoss theboss,
		                                        string title) {
			dumpObjectsBroadcast(theobjects, theboss, title);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using one of the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		/// <param name="usethismessenger">An index for the messenger's boss theboss, which
		///  is used to choose which messenger to use</param>
		public static void dumpObjects(ICollection theobjects,
		                               MessengersBoss theboss,
		                               int usethismessenger) {
			dumpObjects(theobjects, theboss, String.Empty, usethismessenger);
		}
		
		/// <summary>
		/// Print all the elements of the collection theobjects,
		/// using all the messengers given in theboss
		/// </summary>
		/// <param name="theobjects">A collection of objects</param>
		/// <param name="theboss">The IMessenger's boss that contains the messengers
		/// to use</param>
		public static void dumpObjectsBroadcast(ICollection theobjects,
		                                        MessengersBoss theboss) {
			dumpObjectsBroadcast(theobjects, theboss, String.Empty);
		}
	}
}
