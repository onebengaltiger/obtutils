/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Diagnostics;

using OBTUtils.Messaging;


namespace OBTUtils.Enterprise.Messaging
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceEventLogMessenger : IMessenger
	{
		/// <summary>
		/// 
		/// </summary>
		private EventLog evenlog;
		
		/// <summary>
		/// Pequeño objeto para bloquear el acceso a el Log
		/// del servicio de Windows, es necesario cuando es
		/// accesado por multiples hilos
		/// </summary>
		private object lock_obj = new object();
		
		
		
		public ServiceEventLogMessenger(EventLog eventlog) {
			this.evenlog = eventlog;
		}
		
		
		~ServiceEventLogMessenger() {
			evenlog = null;
		}
		
		
		public void sendMessage(string format, params object[] args)
		{
			escribeLog(EventLogEntryType.Information, format, args);
		}
		
		
		/// <summary>
		/// Escribe un mensaje en el log de servicios del sistema
		/// </summary>
		/// <param name="messagetype">Tipo de mensaje (Informativo, error, ...)</param>
		/// <param name="format">Cadena de formato para el mensaje</param>
		/// <param name="args">Argumentos de la cadena de formato</param>
		private void escribeLog(EventLogEntryType messagetype,
		                        string format, params object []args) {
			lock (lock_obj) {
				evenlog.WriteEntry(String.Format(format, args), messagetype);
			}
		}
		
		public void sendMessage(string title, string format, params object[] args)
		{
			throw new NotImplementedException();
		}
		
		public void sendWarningMessage(string title, string format, params object[] args)
		{
			throw new NotImplementedException();
		}
		
		public void sendInformationMessage(string title, string format,
		                                   params object[] args)
		{
			throw new NotImplementedException();
		}
		
		public void sendErrorMessage(string title, string format, params object[] args)
		{
			throw new NotImplementedException();
		}
	}
}
