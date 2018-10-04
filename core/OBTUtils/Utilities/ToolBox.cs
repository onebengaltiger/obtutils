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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

using OBTUtils.Messaging;



namespace OBTUtils.Utilities
{
	/// <summary>
	/// Various utilities to be used in programs
	/// </summary>
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
		private static StringBuilder byteArray2StringBuilder(byte []buffer,
		                                                     int initialposition, int length) {
			if (initialposition < 0 || initialposition > buffer.Length - 1)
				throw new ArgumentOutOfRangeException("El argumento principio es invalido !!");
			
			if (length <= 0 || length > buffer.Length)
				throw new ArgumentOutOfRangeException("El argumento cuantos es invalido !!");
			
			StringBuilder sbuilder = new StringBuilder();
			
			for (int i = initialposition; i < length; ++i)
				sbuilder.AppendFormat("{0:x2}", buffer[i]);
			
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
	}
}
