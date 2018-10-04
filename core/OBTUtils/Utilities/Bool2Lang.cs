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

namespace OBTUtils.Utilities
{
	/// <summary>
	/// A simple class to convert boolean values to
	/// strings in some language
	/// </summary>
	public class Bool2Lang
	{
		/// <summary>
		/// Converts from boolean values to english (True = yes and False = no)
		/// </summary>
		private static Bool2Lang bool2En = new Bool2Lang(new string [] { "No", "Yes" });
		
		/// <summary>
		/// Gets an object to convert from boolean
		/// values to english (True = yes and False = no)
		/// </summary>
		public static Bool2Lang Bool2En {
			get { return bool2En; }
		}
		
		/// <summary>
		/// Converts from boolean values to spanish (True = sí and False = no)
		/// </summary>
		private static Bool2Lang bool2Es = new Bool2Lang(new string [] { "No", "Sí" });
		
		/// <summary>
		/// Gets an object to convert from boolean
		/// values to english (True = sí and False = no)
		/// </summary>
		public static Bool2Lang Bool2Es {
			get { return bool2Es; }
		}
		
		
		/// <summary>
		/// The two strings to convert boolean values
		/// </summary>
		private string []options;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="languageoptions">An array of strings. At least two strings
		/// must be passed inside this array. True will be replaced by the string
		/// languageoptions[1]; False will be replaces by languageoptions[0]</param>
		public Bool2Lang(string []languageoptions)
		{
			if (languageoptions != null && languageoptions.Length >= 2) {
				options = new string[2];
				
				options[0] = languageoptions[0];
				options[1] = languageoptions[1];
			} else
				throw new ArgumentException("The argument cannot be null or " +
				                            "have less that two elements",
				                            "languageoptions");
		}
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~Bool2Lang() {
			options[0] = options[1] = null;
			options = null;
		}
		
		
		/// <summary>
		/// Converts the given boolean value to one
		/// of the string given in the array Options
		/// </summary>
		/// <param name="avalue">A boolean value to convert</param>
		/// <returns>A string that represents the value given
		/// in avalue</returns>
		public string convert(bool avalue) {
			return (avalue ? options[1] : options[0]);
		}
		
		/// <summary>
		/// Converts the given boolean value to an uppercase version of one 
		/// of the string given in the array Options
		/// </summary>
		/// <param name="avalue">A boolean value to convert</param>
		/// <returns>An uppercase string that represents the value given
		/// in avalue</returns>
		public string convertUppercase(bool avalue) {
			return convert(avalue).ToUpper();
		}
		
		/// <summary>
		/// Converts the given boolean value to an lowercase version of one 
		/// of the string given in the array Options
		/// </summary>
		/// <param name="avalue">A boolean value to convert</param>
		/// <returns>A lowercase string that represents the value given
		/// in avalue</returns>
		public string convertLowercase(bool avalue) {
			return convert(avalue).ToLower();
		}
		
		
		/// <summary>
		/// Gets two strings to convert boolean values
		/// </summary>
		public string[] Options {
			get { return options; }
		}
		
		/// <summary>
		/// Converts the given boolean value to one
		/// of the string given in the array Options
		/// </summary>
		public string this[bool avalue] {
			get {
				return convert(avalue);
			}
		}
	}
}
