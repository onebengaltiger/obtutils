/***************************************************************************
 *   Copyright (C) 2011 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;

using OBTUtils.Messaging;



namespace OBTUtils.Data.EDM
{
	/// <summary>
	/// Each instance of this class constructs an object of type
	/// ObjectContext, which in turn has an associated EntityTransaction
	/// object. Every DB operation performed by the context object is linked
	/// to the EntityTransaction object. The class EntityTransactionManager
	/// provides all the necessary methods to commit or rollback the transaction
	/// in the DB
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class EntityTransactionManager<T> : ComponentMessengers
		where T : ObjectContext, new()
	{
		/// <summary>
		/// Connection object associated with the context object
		/// </summary>
		private EntityConnection theConnection;
		
		/// <summary>
		/// The transaction associated with the connection object
		/// </summary>
		private EntityTransaction theTransaction;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">A container in which we can store the
		/// current component</param>
		/// <param name="messengers">An array of messengers</param>
		public EntityTransactionManager(Container container,
		                                params IMessenger []messengers)
			: base(container, messengers)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="container">A container in which we can store the
		/// current component</param>
		public EntityTransactionManager(Container container)
			: base(container)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="messengers">An array of messengers</param>
		public EntityTransactionManager(params IMessenger []messengers)
			: base(messengers)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public EntityTransactionManager()
			: this((Container) null, null)
		{ }
		
		
		/// <summary>
		/// Libera recursos. Para una instancia de esta clase, si la transacción aún
		/// está presente, intenta hacer un rollback y despues desecha el objeto de
		/// tipo transacción. Si la conexión a la BD aun esta presente y se encuentra
		/// en estado de abierta, esta instancia intenta cerrar la conexión y despues
		/// la desecha.
		/// </summary>
		/// <param name="disposing"><c>true</c> si se estan liberando recursos del sistema
		/// no manejados, <c>falso</c> en otro caso</param>
		protected override void Dispose(bool disposing)
		{
			if (theTransaction != null) {
				Boss.broadcastDebugMessage("ROLLING BACK TRANSACTION IN Dispose !!!");
				
				theTransaction.Rollback();
				
				theTransaction.Dispose();
				theTransaction = null;
			}
			
			if (theConnection != null
			    && theConnection.State != ConnectionState.Closed) {
				theConnection.Close();
				theConnection = null;
			}
			
			base.Dispose(disposing);
		}
		
		
		/// <summary>
		///  Gets and opens a transaction (with isolation leven isolationlevel) for
		/// the given ObjectContext object context.
		/// If context is null, then this method creates a new object
		/// </summary>
		/// <param name="context">An entity ObjectContext</param>
		/// <param name="isolationlevel">Isolation level for the transaction</param>
		/// <returns>If the parameter context is not null, then this object is
		/// returned; otherwise returns the newly created context</returns>
		public T getContextWithTransaction(
			T context, IsolationLevel isolationlevel
		) {
			if (theConnection != null || theTransaction != null)
				throw new OBTDataException("Este manejador de transacciones ya tiene" +
				                           " una conexión y/o transacción asociado(s)");
			
			T thecontext;

			if (context == null)
				thecontext = new T();
			else
				thecontext = context;
			
			theConnection = (EntityConnection) thecontext.Connection;
			
			if (theConnection.State == ConnectionState.Closed) {
				theConnection.Open();
			}
			
			theTransaction = theConnection.BeginTransaction(isolationlevel);
			
			return thecontext;
		}
		
		/// <summary>
		///  Gets and opens a transaction for the given ObjectContext object context.
		/// If context is null, then this method creates a new object
		/// </summary>
		/// <param name="context">An entity ObjectContext</param>
		/// <returns>If the parameter context is not null, then this object is
		/// returned; otherwise returns the newly created context</returns>
		public T getContextWithTransaction(T context) {
			return getContextWithTransaction(context,
			                                 IsolationLevel.Serializable);
		}
		
		/// <summary>
		/// Constructs a new ObjectContext object and opens a transaction for this
		/// object. The transaction has isolation level isolationlevel
		/// </summary>
		/// <param name="isolationlevel">Isolation level for the transaction</param>
		/// <returns>The newly created context</returns>
		public T getContextWithTransaction(IsolationLevel isolationlevel) {
			return getContextWithTransaction(null, isolationlevel);
		}
		
		/// <summary>
		/// Constructs a new ObjectContext object and opens a transaction for this
		/// object
		/// </summary>
		/// <returns>The newly created context</returns>
		public T getContextWithTransaction() {
			return getContextWithTransaction(IsolationLevel.Serializable);
		}
		
		/// <summary>
		/// Completa la transacción que esta asociada a un objeto de contexto de BD
		///  de tipo <c>SECTBitBaseDatosContext</c> que fue construido con la función
		/// obtenContextoConTransaccion.
		/// </summary>
		/// <param name="cierraConexion"><c>true</c> si este método debe cerrar la
		/// conexión, <c>false</c> en caso contrario</param>
		private void commitTransaction(bool closeconnection) {
			if (theConnection == null || theTransaction == null)
				throw new OBTDataException("ManejadorTransacciones: " +
				                           "No se tiene una conexión " +
				                           " y/o una transacción asociada");
			
			theTransaction.Commit();
			theTransaction = null;
			
			if (closeconnection) {
				theConnection.Close();
				theConnection = null;
			}
		}
		
		/// <summary>
		/// Completa la transacción que esta asociada a un objeto de contexto de BD
		///  de tipo <c>SECTBitBaseDatosContext</c> que fue construido con la función
		/// obtenContextoConTransaccion. Este método cierra la conexión despues de
		/// completar la transacción
		/// </summary>
		public void commitTransaction() {
			commitTransaction(true);
		}
		
		/// <summary>
		/// Cancela todas las operaciones hechas bajo la transacción asociada a
		/// un contexto de BD de tipo <c>SECTBitBaseDatosContext</c> que fue
		/// construido con la función obtenContextoConTransaccion.
		/// Este método cierra la conexión despues de revertir la transacción
		/// </summary>
		public void rollbackTransaction() {
			if (theConnection == null || theTransaction == null)
				throw new OBTDataException("ManejadorTransacciones: " +
				                           "No se tiene una conexión " +
				                           " y/o una transacción asociada");
			
			theTransaction.Rollback();
			theTransaction = null;
			
			theConnection.Close();
			theConnection = null;
		}
	}
}
