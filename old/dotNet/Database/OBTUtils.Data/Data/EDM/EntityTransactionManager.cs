/***************************************************************************
 *   Copyright (C) 2011-2015 by Rodolfo Conde Martinez                     *
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
			: base(null, messengers)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public EntityTransactionManager()
			: this((Container) null)
		{ }
		
		
		/// <summary>
		/// Free resources immediately. For an instance of this class, if there is a
		/// transaction in progress, then this method tries to rollback the transaction
		/// and then the transaction object is freed. Also, if the connection object
		/// is in the Open state, then this method closes the conecction and finally it
		/// frees the connection object
		/// </summary>
		/// <param name="disposing"><c>true</c> if non-managed resources are to be freed;
		/// <c>false</c> otherwise</param>
		protected override void Dispose(bool disposing)
		{
			if (theTransaction != null) {
				TheMessengersBoss.broadcastDebugMessage("ROLLING BACK TRANSACTION IN Dispose !!!");
				
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
		/// Gets and opens a transaction (with isolation leven isolationlevel) for
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
				throw new OBTDataException("This transaction's manager already as an " +
				                           "associated connection and/or " +
				                           "transaction object(s)");
			
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
		/// Gets and opens a transaction for the given ObjectContext object context.
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
		/// Commits the transaction associated with the ObjectContext obtained
		/// with one of the getContextWithTransaction methods
		/// </summary>
		/// <param name="closeconnection"><c>true</c> if the connection must be closed
		/// <c>false</c> otherwise</param>
		private void commitTransaction(bool closeconnection) {
			if (theConnection == null || theTransaction == null)
				throw new OBTDataException("EntityTransactionManager: " +
				                           "There is no associated connection and/or " +
				                           "transaction");
			
			theTransaction.Commit();
			theTransaction = null;
			
			if (closeconnection) {
				theConnection.Close();
				theConnection = null;
			}
		}
		
		/// <summary>
		/// Commits the transaction associated with the ObjectContext obtained
		/// with one of the getContextWithTransaction methods. This function
		/// closes the connection after commiting the transaction
		/// </summary>
		public void commitTransaction() {
			commitTransaction(true);
		}
		
		/// <summary>
		/// Rollback the transaction associated with the ObjectContext obtained
		/// with one of the getContextWithTransaction methods. This function
		/// closes the connection after canceling the transaction
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
