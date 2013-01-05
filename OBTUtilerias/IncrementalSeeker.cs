/***************************************************************************
 *   Copyright (C) 2011-2013 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



namespace OBTUtils
{
	/// <summary>
	/// Delegate type to execute the search
	/// </summary>
	public delegate ICollection<T> realSeeker<T>(string criteriobusqueda);
	
	
	/// <summary>
	/// Implementa un buscador tipo incremental. La idea básica es que
	///  cuando se le da un criterio de busqueda a un objeto de esta clase
	/// por primera vez, el objeto guarda el criterio de busqueda y regresa
	/// ciertos resultados. En busquedas subsecuentes, el buscador verifica
	/// si el criterio de busqueda nuevo contiene el criterio de busqueda anterior,
	/// y si es asi, regresa un resultado que indica que no se realizo busqueda
	/// nueva, porque la busqueda anterior ya debe contener la busqueda nueva.
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class IncrementalSeeker<T>
	{
		/// <summary>
		/// Criterio anterior de busqueda
		/// </summary>
		private string lastcriterion;
		
		/// <summary>
		/// Delegado que implementa el método real
		/// de busqueda
		/// </summary>
		private realSeeker<T> theseeker;
		
		/// <summary>
		/// Resultado anterior de busqueda
		/// </summary>
		private ICollection<T> lastresult;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oneseeker">Delegado que implementa el método real
		/// de busqueda</param>
		/// <param name="initialcriterion">Criterio inicial de busqueda</param>
		public IncrementalSeeker(realSeeker<T> oneseeker, string initialcriterion)
		{
			theseeker = oneseeker;
			lastcriterion = initialcriterion;
			lastresult = emptyCollection();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="oneseeker">Delegado que implementa el método real
		/// de busqueda</param>
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
		/// Obtiene o asigna el criterio para realizar
		/// la busqueda
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
		/// Obtiene o asigna al delegado que implementa el método real
		/// de busqueda
		/// </summary>
		public realSeeker<T> Theseeker {
			get {
				return theseeker;
			}
		}
		
		
		/// <summary>
		/// Realiza una busqueda usando al delegado de busqueda con
		/// el criterio dado
		/// </summary>
		/// <param name="criterion">El criterio de la busqueda</param>
		/// <param name="dolocalsearch">indica si se hace una busqueda "loca",
		/// es decir, si se utilizan los resultados previamente obtenidos de una
		/// busqueda anterior</param>
		/// <returns>Una colección de valores, que son el resultado
		/// de la busqueda hecha</returns>
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
		/// Regresa la colección vacía
		/// </summary>
		/// <returns>La colección vacía</returns>
		private ICollection<T> emptyCollection() {
			return new List<T>(0);
		}
		
		/// <summary>
		/// Indica si la cadena dada como parámetro es un criterio
		/// de busqueda válido
		/// </summary>
		/// <param name="criterio">Cadena que será provada</param>
		/// <returns>true si la cadena dada como parámetro es un criterio
		/// de busqueda válido; false en otro caso</returns>
		private bool isValidCriterion(string criterio) {
			return !String.IsNullOrEmpty(criterio)
				&& !String.IsNullOrWhiteSpace(criterio);
		}
		
		/// <summary>
		/// Realiza la busqueda usando el delegado buscador y el criterio
		/// dado por el parámetro
		/// </summary>
		/// <param name="criterion">Criterio de busqueda</param>
		/// <returns>Una colección de tipo T que representa el resultado
		/// de la busqueda</returns>
		private ICollection<T> useSeeker(string criterion) {
			ICollection<T> retVal;
			
			retVal = theseeker(criterion);
			
			lastcriterion = criterion;
			lastresult = retVal;
			
			return retVal;
		}
		
		
		/// <summary>
		/// Delegado interno para realizar una prueba para
		/// verificar si item1 contiene a item2, en tal caso, el delegado
		/// debe dar como resultado true y false en caso contrario
		/// </summary>
		private delegate bool containsItemTest(T item1, string item2);
		
		
		/// <summary>
		/// Realiza una busqueda local en la colección del resultado de busqueda
		/// anterior, usando el criterio dado por el parámetro criterio
		/// </summary>
		/// <param name="criterion">Criterio de busqueda</param>
		/// <param name="testmethod">Delegado para verificar si un elemento de la lista
		/// del resultado anterior de busqueda contiene al criterio de busqueda</param>
		/// <returns>El resultado de la busqueda</returns>
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
