/***************************************************************************
 *   Copyright (C) 2013 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;

using OBTUtils.Messaging;


namespace OBTUtils.Enterprise.Messaging
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceEventLogMessenger : IMessenger
	{
		public ServiceEventLogMessenger()
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		private EventLog logeventos;
		
		/// <summary>
		/// Pequeño objeto para bloquear el acceso a el Log
		/// del servicio de Windows, es necesario cuando es
		/// accesado por multiples hilos
		/// </summary>
		private object lock_obj = new object();
		
		
		
		public MensajeroLogServicio(EventLog logeventos) {
			this.logeventos = logeventos;
		}
		
		
		~MensajeroLogServicio() {
			logeventos = null;
		}
		
		public void sendMessage(string format, params object[] args)
		{
			EventLogEntryType tipoentrada;
			object tipoobjeto;
			object []argumentosreales = null;
			
			if (args.Length > 0) {
				tipoobjeto = args[0];
				
				if (tipoobjeto is EventLogEntryType) {
					tipoentrada = (EventLogEntryType) tipoobjeto;
					
					if (args.Length > 1) {
						argumentosreales = new object[args.Length - 1];
						Array.Copy(args, 1, argumentosreales, 0, args.Length - 1);
					}
				} else
					tipoentrada = EventLogEntryType.Information;
			} else {
				argumentosreales = args;
				tipoentrada = EventLogEntryType.Information;
			}
			
			escribeLog(tipoentrada, format, argumentosreales);
		}
		
		
		/// <summary>
		/// Escribe un mensaje en el log de servicios del sistema
		/// </summary>
		/// <param name="tipoMensaje">Tipo de mensaje (Informativo, error, ...)</param>
		/// <param name="fmt">Cadena de formato para el mensaje</param>
		/// <param name="args">Argumentos de la cadena de formato</param>
		private void escribeLog(EventLogEntryType tipoMensaje, string fmt, params object []args) {
			lock (lock_obj) { logeventos.WriteEntry(String.Format(fmt, args), tipoMensaje); }
		}
		
		
		/// <summary>
		/// Escribe un mensaje informativo en el log de servicios del sistema
		/// </summary>
		/// <param name="fmt">Cadena de formato para el mensaje</param>
		/// <param name="args">Argumentos de la cadena de formato</param>
		public void informacion(string fmt, params object []args) {
			escribeLog(EventLogEntryType.Information, fmt, args);
		}
		
		/// <summary>
		/// Escribe un mensaje de advertencia en el log de servicios del sistema
		/// </summary>
		/// <param name="fmt">Cadena de formato para el mensaje</param>
		/// <param name="args">Argumentos de la cadena de formato</param>
		public void advertencia(string fmt, params object []args) {
			escribeLog(EventLogEntryType.Warning, fmt, args);
		}
		
		/// <summary>
		/// Escribe un mensaje de error en el log de servicios del sistema
		/// </summary>
		/// <param name="fmt">Cadena de formato para el mensaje</param>
		/// <param name="args">Argumentos de la cadena de formato</param>
		public void error(string fmt, params object []args) {
			escribeLog(EventLogEntryType.Error, fmt, args);
		}
	}
}
