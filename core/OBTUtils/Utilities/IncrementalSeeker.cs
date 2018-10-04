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
using System.Linq;
using System.Collections;
using System.Collections.Generic;



namespace OBTUtils.Utilities
{
	/// <summary>
	/// Delegate type to execute the search
	/// </summary>
	public delegate ICollection<T> realSeeker<T>(string criteriobusqueda);
	
	
	/// <summary>
	/// A class used to implement an incremental-type data seeker. Given an 
	/// object of type IncrementalSeeker, the first time this object is given
	///  a search criterion, the object stores this criterion and executed the search
	/// to return some values. In subsequent searches, the seeker compares the old 
	/// search criterion with the new one; if the new criterion contains the old one,
	/// then the return value of the search indicates that no search at all was performed,
	/// because the results of the last search must contain the results of the new search.
	/// </summary>
	public class IncrementalSeeker<T>
	{
		/// <summary>
		/// Last search's criterion
		/// </summary>
		private string lastcriterion;
		
		/// <summary>
		/// The method that realizes the search
		/// </summary>
		private realSeeker<T> theseeker;
		
		/// <summary>
		/// Last search's result
		/// </summary>
		private ICollection<T> lastresult;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oneseeker">The method that realizes the search</param>
		/// <param name="initialcriterion">Initial criterion</param>
		public IncrementalSeeker(realSeeker<T> oneseeker, string initialcriterion)
		{
			theseeker = oneseeker;
			lastcriterion = initialcriterion;
			lastresult = emptyCollection();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oneseeker">The method that realizes the search</param>
		public IncrementalSeeker(realSeeker<T> oneseeker)
			: this(oneseeker, String.Empty) { }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~IncrementalSeeker() {
			theseeker = null;
			lastcriterion = null;
			
			if (lastresult != null) {
				lastresult.Clear();
				lastresult = null;
			}
		}
		
		
		/// <summary>
		/// Gets or sets a search criterion
		/// </summary>
		public string Lastcriterion {
			get {
				return lastcriterion;
			}
			
			set {
				lastcriterion = value;
			}
		}
		
		/// <summary>
		/// Gets the method that realizes the search
		/// </summary>
		public realSeeker<T> Theseeker {
			get {
				return theseeker;
			}
		}
		
		
		/// <summary>
		/// Executes a search using the delegate Theseeker with the given 
		/// search criterion
		/// </summary>
		/// <param name="criterion">Current search's criterion</param>
		/// <param name="dolocalsearch">true to execute a "local" search, that is, 
		/// the seeker uses the results of the previous search
		/// </param>
		/// <returns>A collection of values related to the given search criterion</returns>
		public ICollection<T> search(string criterion, bool dolocalsearch) {
			ICollection<T> retVal = null;
			
			if (isValidCriterion(criterion)) {
				if (!isValidCriterion(lastcriterion))
					retVal = useSeeker(criterion);
				else {
					if (!criterion.Contains(lastcriterion))
						retVal = useSeeker(criterion);
					else if (dolocalsearch)
						retVal = executeLocalSearch(criterion, null);
				}
			}
			
			return retVal;
		}
		
		
		/// <summary>
		/// Returns an empty collection
		/// </summary>
		/// <returns>An empty collection</returns>
		private ICollection<T> emptyCollection() {
			return new List<T>(0);
		}
		
		/// <summary>
		/// Computes a boolean value that indicates if the given string 
		/// is a valid search criterion
		/// </summary>
		/// <param name="criterio">A string to test</param>
		/// <returns>true if the given string is a valid search criterion; 
		/// false otherwise</returns>
		private bool isValidCriterion(string criterio) {
			return !String.IsNullOrEmpty(criterio)
				&& !String.IsNullOrWhiteSpace(criterio);
		}
		
		/// <summary>
		/// Executes a search in the data using the delegate Theseeker and the
		/// given search's criterion
		/// </summary>
		/// <param name="criterion">The search's criterion</param>
		/// <returns>A collection of objects of type T representing the result
		/// of the search</returns>
		private ICollection<T> useSeeker(string criterion) {
			ICollection<T> retVal;
			
			retVal = theseeker(criterion);
			
			lastcriterion = criterion;
			lastresult = retVal;
			
			return retVal;
		}
		
		
		/// <summary>
		/// Internal delegate to realize a test to determine if 
		/// item1 contains item2. If this is the case, the delegate must give true 
		/// as the return value and false otherwise
		/// </summary>
		private delegate bool containsItemTest(T item1, string item2);
		
		
		/// <summary>
		/// Searchs for items containing the string criterion in the results 
		/// of the last search
		/// </summary>
		/// <param name="criterion">The search's criterion</param>
		/// <param name="testmethod">A delegate of type containsItemTest to execute the 
		/// search</param>
		/// <returns>A collection of values of type T, representing the result of 
		/// the search</returns>
		private ICollection<T> executeLocalSearch(string criterion,
		                                          containsItemTest testmethod)
		{
			ICollection<T> retVal = null;
			
			if (lastresult != null) {
				var cmSelect = (from item in lastresult
				                where testmethod(item, criterion)
				                select item).Distinct();
				
				retVal = new List<T>(cmSelect);
			}
			
			return retVal;
		}
	}
}
