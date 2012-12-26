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
	public delegate ICollection<T> buscadorReal<T>(string criteriobusqueda);
	
	
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
	public class BuscadorTG<T>
	{
		/// <summary>
		/// Criterio anterior de busqueda
		/// </summary>
		private string criterioAnterior;
		
		/// <summary>
		/// Delegado que implementa el método real 
		/// de busqueda
		/// </summary>
		buscadorReal<T> elBuscador;
		
		/// <summary>
		/// Resultado anterior de busqueda
		/// </summary>
		ICollection<T> resultadoAnterior;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="buscador">Delegado que implementa el método real 
		/// de busqueda</param>
		/// <param name="criterioInicial">Criterio inicial de busqueda</param>
		public BuscadorTG(buscadorReal<T> buscador, string criterioInicial)
		{
			elBuscador = buscador;
			criterioAnterior = criterioInicial;
			resultadoAnterior = coleccionVacia();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="buscador">Delegado que implementa el método real 
		/// de busqueda</param>
		public BuscadorTG(buscadorReal<T> buscador)
			: this(buscador, String.Empty) { }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~BuscadorTG() {
			elBuscador = null;
			criterioAnterior = null;
			
			if (resultadoAnterior != null) {
				resultadoAnterior.Clear();
				resultadoAnterior = null;
			}			
		}
		
		
		/// <summary>
		/// Obtiene o asigna el criterio para realizar 
		/// la busqueda
		/// </summary>
		public string CriterioAnterior {
			get {
				return criterioAnterior;
			}
			
			set {
				criterioAnterior = value;
			}
		}
		
		/// <summary>
		/// Obtiene o asigna al delegado que implementa el método real 
		/// de busqueda
		/// </summary>
		public buscadorReal<T> ElBuscador {
			get {
				return elBuscador;
			}
		}
		

		/// <summary>
		/// Realiza una busqueda usando al delegado de busqueda con
		/// el criterio dado
		/// </summary>
		/// <param name="criterio">El criterio de la busqueda</param>
		/// <param name="realizaBusquedalocal">indica si se hace una busqueda "loca",
		/// es decir, si se utilizan los resultados previamente obtenidos de una
		/// busqueda anterior</param>
		/// <returns>Una colección de valores, que son el resultado 
		/// de la busqueda hecha</returns>
		public ICollection<T> busca(string criterio, bool realizaBusquedalocal) {
			ICollection<T> retVal = null;
			
			if (esCriterioValido(criterio)) {
				if (!esCriterioValido(criterioAnterior))
					retVal = usaBuscador(criterio);
				else {
					if (!criterio.Contains(criterioAnterior))
						retVal = usaBuscador(criterio);
					else if (realizaBusquedalocal)
						retVal = hazBusquedalocal(criterio, null);
				}
			}
			
			return retVal;
		}
		
		
		/// <summary>
		/// Regresa la colección vacía
		/// </summary>
		/// <returns>La colección vacía</returns>
		private ICollection<T> coleccionVacia() {
			return new List<T>(0);
		}
		
		/// <summary>
		/// Indica si la cadena dada como parámetro es un criterio
		/// de busqueda válido
		/// </summary>
		/// <param name="criterio">Cadena que será provada</param>
		/// <returns>true si la cadena dada como parámetro es un criterio
		/// de busqueda válido; false en otro caso</returns>
		private bool esCriterioValido(string criterio) {
			return !String.IsNullOrEmpty(criterio)
				&& !String.IsNullOrWhiteSpace(criterio);
		}
		
		/// <summary>
		/// Realiza la busqueda usando el delegado buscador y el criterio 
		/// dado por el parámetro
		/// </summary>
		/// <param name="criterio">Criterio de busqueda</param>
		/// <returns>Una colección de tipo T que representa el resultado 
		/// de la busqueda</returns>
		private ICollection<T> usaBuscador(string criterio) {
			ICollection<T> retVal;
			
			retVal = elBuscador(criterio);
			
			criterioAnterior = criterio;
			resultadoAnterior = retVal;
			
			return retVal;
		}
		
		
		/// <summary>
		/// Delegado interno para realizar una prueba para 
		/// verificar si item1 contiene a item2, en tal caso, el delegado
		/// debe dar como resultado true y false en caso contrario
		/// </summary>
		private delegate bool contiene(T item1, string item2);
		
		
		/// <summary>
		/// Realiza una busqueda local en la colección del resultado de busqueda
		/// anterior, usando el criterio dado por el parámetro criterio
		/// </summary>
		/// <param name="criterio">Criterio de busqueda</param>
		/// <param name="prueba">Delegado para verificar si un elemento de la lista 
		/// del resultado anterior de busqueda contiene al criterio de busqueda</param>
		/// <returns>El resultado de la busqueda</returns>
		private ICollection<T> hazBusquedalocal(string criterio, contiene prueba)
		{
			ICollection<T> retVal = null;
			
			if (resultadoAnterior != null) {
				var cmSelect = (from item in resultadoAnterior
				                where prueba(item, criterio)
				                select item).Distinct();
				
				retVal = new List<T>(cmSelect);
			}
			
			return retVal;
		}
	}
}
