/***************************************************************************
 *   Copyright (C) 2012 by Rodolfo Conde Martínez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;


namespace OBTUtils.Data.ADO
{
	/// <summary>
	/// Administrador de las conexiones y operaciones en la BD
	/// </summary>
	/// 
	/// \author \rodolfo
	public class ADODBAdmin
	{
		/// <summary>
		/// Fabrica de métodos y objetos especificos de
		/// un manejador de BD (SQL Server, PostgreSQL, MySQL, ...)
		/// </summary>
		protected readonly DbProviderFactory fabricaBD
			= System.Data.SqlClient.SqlClientFactory.Instance;
		
		
		/// <summary>
		/// Conexión a la BD
		/// </summary>
		protected DbConnection conexion;
		
		/// <summary>
		/// Tiempo máximo de espera para ejecutar un comando en
		/// el manejador de BD
		/// </summary>
		protected int tiempoEsperaEjecucionComandos_ms = 10 * 1000;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public ADODBAdmin()
		{
		}
		
		/// <summary>
		/// Destructor
		/// </summary>
		~ADODBAdmin() {
			if (conexion != null && conexion.State == ConnectionState.Open) {
				conexion.Close();
			}
			
			conexion = null;
		}
		
		
		/// <summary>
		/// Prepara la conexión a la base de datos usando los parametros
		/// indicados
		/// </summary>
		/// <param name="servidor">Dirección del servidor de BD</param>
		/// <param name="basedatos">Nombre de la base de datos</param>
		/// <param name="usuario">Nombre de inicio de sesión en el servidor de BD</param>
		/// <param name="clave">Clave de ingreso a la BD</param>
		public void preparaConexion(string servidor, string basedatos,
		                            string usuario, string clave) {
			DbConnectionStringBuilder constructorCadenas
				= fabricaBD.CreateConnectionStringBuilder();
			
			constructorCadenas.Add("server", servidor);
			constructorCadenas.Add("user id", usuario);
			constructorCadenas.Add("password", clave);
			constructorCadenas.Add("Initial catalog", basedatos);
			
			conexion = fabricaBD.CreateConnection();
			conexion.ConnectionString = constructorCadenas.ConnectionString;
		}
		
		/// <summary>
		/// Prepara la conexión a la base de datos usando la cadena
		/// de conexión dada
		/// </summary>
		/// <param name="cadenaConeccion">Cadena de conexión a la base de datos
		/// (especifica del manejador que se utiliza)</param>
		public void preparaConexion(string cadenaConeccion) {
			conexion = fabricaBD.CreateConnection();
			conexion.ConnectionString = cadenaConeccion;
		}
		
		
		/// <summary>
		/// Obtiene o asigna el tiempo máximo de espera para
		/// ejecutar un comando en el manejador de BD
		/// </summary>
		public int TiempoEsperaEjecucionComandos {
			get {
				return tiempoEsperaEjecucionComandos_ms / 1000;
			}
			
			set {
				if (value < 0) value = -value;
				
				tiempoEsperaEjecucionComandos_ms = value * 1000;
			}
		}
		
		
//		/// <summary>
//		/// Obten la información establecida en la BD sobre el monitoreo de vehículos
//		/// correspondiente al periodo de tiempo indicado por los parámetros
//		/// </summary>
//		/// <param name="fechainicio">Fecha de inicio del periodo solicitado</param>
//		/// <param name="fechafin">Fecha final del periodo solicitado</param>
//		/// <returns>La información de los vehiculos monitoreados que esta guardada
//		/// en la BD</returns>
//		internal DataTable ObtenInformacionVehiculosMonitoreados(
//			DateTime fechainicio, DateTime fechafin
//		)
//		{
//			DataTable valorregreso = new DataTable();
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataAdapter adaptador = fabricaBD.CreateDataAdapter();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoDatosVehiculos
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT
//						T.Grupo,
//						T.Ruta,
//						T.Vehiculo,
//						NumeroEconomico,
//						Fecha,
//						HoraSalidaPlanta,
//						HoraIngresoRuta,
//						HoraInicial AS HoraPrimerSuministro,
//						HoraUltimoSuministro,
//						HoraFinRuta,
//						HoraSalidaRuta,
//						HoraIngresoPlanta,
//						HoraUltimoPanico,
//						EstadoUbicacion,
//						U.FGps AS HoraUltimaPosicion
//						FROM TableroMonitoreo AS T
//						INNER JOIN Vehiculo AS V
//						ON T.Vehiculo = V.Vehiculo
//						LEFT JOIN
//						(SELECT DISTINCT
//							Vehiculo,
//							HoraInicial
//						FROM Surtido
//						WHERE
//						NumeroSurtido = 1 AND
//						FSurtido BETWEEN @FechaInicio AND @FechaFin) AS S
//						ON T.Vehiculo = S.Vehiculo
//						LEFT JOIN
//						(SELECT
//							Vehiculo,
//							FGps
//						FROM UltimaPosicion
//						WHERE FGps BETWEEN @FechaInicio AND @FechaFin) AS U
//						ON T.Vehiculo = U.Vehiculo
//						WHERE
//						Fecha BETWEEN @FechaInicio AND @FechaFin";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaInicio";
//			parametro.Value = fechainicio;
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaFin";
//			parametro.Value = fechafin;
//			comandosql.Parameters.Add(parametro);
//			
//			adaptador.SelectCommand = comandosql;
//			
//			adaptador.Fill(valorregreso);
//			
//			return valorregreso;
//		}
//		
//		/// <summary>
//		/// Obten las geocercas de las rutas de los vehículos
//		/// </summary>
//		/// <returns>La información de los vehiculos monitoreados que esta guardada
//		/// en la BD</returns>
//		internal DataTable ObtenGeocercas()
//		{
//			DataTable valorregreso = new DataTable();
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataAdapter adaptador = fabricaBD.CreateDataAdapter();
//			
//			// spMSTableroMonitoreoTodasGeocercas
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT
//					Grupo,
//					Ruta,
//					Fecha,
//					Poligono,
//					Vertice,
//					ABS(Longitud) AS Longitud,
//					Latitud
//					FROM Geocerca
//					ORDER BY Grupo, Ruta, Fecha, Vertice";
//			
//			adaptador.SelectCommand = comandosql;
//			
//			adaptador.Fill(valorregreso);
//			
//			return valorregreso;
//		}
//		
//		/// <summary>
//		/// Obten todos los puntos correspondientes a la geocerca especificada por
//		/// los parámetros
//		/// </summary>
//		/// <param name="grupo">El grupo que esta delimitado por esta geocerca</param>
//		/// <param name="ruta">La ruta que esta delimitada por la geocerca</param>
//		/// <param name="fecha">La fecha máximo de los puntos de la geocerca que deseamos
//		///  incluir</param>
//		/// <returns>La información de la geocerca que esta guardada
//		/// en la BD</returns>
//		internal Punto []ObtenGeocerca(
//			int grupo, int ruta, DateTime fecha
//		)
//		{
//			Queue<Punto> poligono;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataReader lectorgeocerca;
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaGeocerca
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT
//						Grupo,
//						Ruta,
//						Fecha,
//						Poligono,
//						Vertice,
//						ABS(Longitud) AS Longitud,
//						Latitud
//						FROM Geocerca
//						WHERE
//						Grupo = @Grupo AND
//						Ruta = @Ruta AND
//						Fecha <= @Fecha
//						ORDER BY Fecha DESC, Vertice
//						";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Grupo";
//			parametro.Value = grupo;
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Ruta";
//			parametro.Value = ruta;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Fecha";
//			parametro.Value = fecha;
//			comandosql.Parameters.Add(parametro);
//			
//			conexion.Open();
//			
//			lectorgeocerca = comandosql.ExecuteReader(CommandBehavior.CloseConnection);
//			
//			poligono = new Queue<Punto>();
//			
//			while (lectorgeocerca.Read()) {
//				decimal x, y;
//				
//				x = lectorgeocerca.GetDecimal(5);
//				y = lectorgeocerca.GetDecimal(6);
//				
//				poligono.Enqueue(new Punto(x, y));
//			}
//			
//			lectorgeocerca.Close();
//			
//			return poligono.ToArray();
//		}
//		
//		/// <summary>
//		/// Obten la última posicion del vehículo, en el rango de fechas
//		/// indicados por los parámetros dados
//		/// </summary>
//		/// <param name="fechainicio">Fecha de inicio del periodo solicitado</param>
//		/// <param name="fechafin">Fecha final del periodo solicitado</param>
//		/// <returns>La última posición del vehículo
//		/// en la BD</returns>
//		internal Punto ObtenUltimaPosicion(
//			DateTime fechainicio, DateTime fechafin, int vehiculo
//		)
//		{
//			Punto valorregreso = new Punto(0, 0);
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataReader lectorposiciones;
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaUltimaPosicion
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT TOP 1
//						FGps,
//						ABS(Longitud) AS Longitud,
//						Latitud,
//						Vehiculo
//						FROM UltimaPosicion
//						WHERE
//						FGps BETWEEN @FechaInicio AND @FechaFin
//						AND Vehiculo = @Vehiculo
//						--ORDER BY FGps DESC";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaInicio";
//			parametro.Value = fechainicio;
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaFin";
//			parametro.Value = fechafin;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//			conexion.Open();
//			
//			lectorposiciones = comandosql.ExecuteReader(CommandBehavior.CloseConnection);
//			
//			if (lectorposiciones.HasRows) {
//				lectorposiciones.Read();
//				
//				valorregreso.x = lectorposiciones.GetDecimal(1);
//				valorregreso.y = lectorposiciones.GetDecimal(2);
//			}
//			
//			if (!lectorposiciones.IsClosed)
//				lectorposiciones.Close();
//			
//			lectorposiciones = null;
//			
//			return valorregreso;
//		}
//		
//		/// <summary>
//		/// Obten la información establecida en la BD de todas las posiciones
//		/// del vehículo, en el rango de fechas indicados por los parámetros dados
//		/// </summary>
//		/// <param name="fechainicio">Fecha de inicio del periodo solicitado</param>
//		/// <param name="fechafin">Fecha final del periodo solicitado</param>
//		/// <returns>Las posiciones del vehículo
//		/// en la BD</returns>
//		internal DbDataReader ObtenRangoPosicionesVehiculo(
//			DateTime fechainicio, DateTime fechafin, int vehiculo
//		)
//		{
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataReader lectorposiciones;
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaRangoPosiciones
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT
//						FGps,
//						--NumeroSurtido,
//						ABS(Longitud) AS Longitud,
//						Latitud,
//						--Velocidad,
//						--Orientacion,
//						Vehiculo --,
//						--TipoPosicion,
//						--FCorreo,
//						--FRecepcion,
//						--Alarmas,
//						--NumeroEvento
//						FROM Posicion
//						WHERE
//						FGps BETWEEN @FechaInicio AND @FechaFin
//						AND Vehiculo = @Vehiculo
//						ORDER BY FGps";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaInicio";
//			parametro.Value = fechainicio;
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaFin";
//			parametro.Value = fechafin;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//			conexion.Open();
//			
//			lectorposiciones = comandosql.ExecuteReader(CommandBehavior.CloseConnection);
//			
//			
//			return lectorposiciones;
//		}
//		
//		/// <summary>
//		/// Busca en la BD el primer surtido del vehículo en la fecha
//		/// indicados en los parámetros
//		/// </summary>
//		/// <param name="vehiculo">El ID de BD del vehículo</param>
//		/// <param name="fecha">La fecha en la cual buscar el surtido</param>
//		/// <returns>El primero surtido hecho por el vehículo</returns>
//		public object CalculaHoraPrimerSuministro(int vehiculo, DateTime fecha)
//		{
//			object valorregreso = null;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaPrimerSurtido
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT TOP 1
//					HoraInicial
//					FROM surtido
//					WHERE
//					NumeroSurtido = 1 AND
//					FSurtido = @FSurtido AND
//					Vehiculo = @Vehiculo";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FSurtido";
//			parametro.Value = fecha.Date;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//			conexion.Open();
//			valorregreso = comandosql.ExecuteScalar();
//			conexion.Close();
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Busca en la BD el último surtido del vehículo en la fecha
//		/// indicados en los parámetros
//		/// </summary>
//		/// <param name="vehiculo">El ID de BD del vehículo</param>
//		/// <param name="fecha">La fecha en la cual buscar el surtido</param>
//		/// <returns>El último surtido hecho por el vehículo</returns>
//		public object CalculaHoraUltimoSuministro(int vehiculo, DateTime fecha)
//		{
//			object valorregreso = null;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaUltimoSurtido
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT TOP 1
//					HoraInicial
//					FROM surtido
//					WHERE
//					FSurtido = @FSurtido AND
//					Vehiculo = @Vehiculo
//					ORDER BY NumeroSurtido DESC";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FSurtido";
//			parametro.Value = fecha.Date;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//			
//			conexion.Open();
//			valorregreso = comandosql.ExecuteScalar();
//			conexion.Close();
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Obten la hora del fin de ruta indicada en la tabla Llamada
//		/// del Sigamet
//		/// </summary>
//		/// <param name="numeroeconomico">Económico del vehículo</param>
//		/// <param name="fecha">Fecha en la cual buscar la hora del fin
//		/// de ruta</param>
//		/// <returns>La hora del fin de ruta del vehículo</returns>
//		public object CalculaHoraFinRutaSIGAMET(int numeroeconomico,
//		                                        DateTime fecha)
//		{
//			object valorregreso = null;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoBuscaFinRuta
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT --Autotanque,
//					MIN(FLlamada) AS FLlamada
//					FROM Llamada WITH (NOLOCK)
//					WHERE Autotanque = @NumeroEconomico
//					AND Autotanque > 0
//					AND MotivoLlamada = 40
//					AND DAY(FLlamada) = DAY(@Fecha)
//					AND MONTH(FLlamada) = MONTH(@Fecha)
//					AND YEAR(FLlamada) = YEAR(@Fecha)
//					GROUP BY Autotanque";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Fecha";
//			parametro.Value = fecha.Date;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@NumeroEconomico";
//			parametro.Value = numeroeconomico;
//			comandosql.Parameters.Add(parametro);
//			
//			
//			conexion.Open();
//			valorregreso = comandosql.ExecuteScalar();
//			conexion.Close();
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Obten la hora del fin de ruta del vehículo indicado
//		/// por numeroeconomico, en la tabla tablafinesruta
//		/// </summary>
//		/// <param name="tablafinesruta">Tabla local con todos los fines de ruta
//		/// de los vehículos</param>
//		/// <param name="numeroeconomico">Económico del vehículo</param>
//		/// <returns>La hora del fin de ruta del vehículo</returns>
//		public object CalculaHoraFinRutaSIGAMET(DataTable tablafinesruta,
//		                                        int numeroeconomico)
//		{
//			object valorregreso = null;
//			
//			if (tablafinesruta != null) {
//				DataRow []renglones = tablafinesruta.Select(
//					String.Format("Autotanque = {0}", numeroeconomico)
//				);
//				
//				if (renglones.Length != 0)
//					valorregreso = renglones[0]["FLlamada"];
//			}
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Obten la información de todos los fines de ruta de vehículos
//		/// en el sigamet
//		/// </summary>
//		/// <param name="fecha">Fecha de inicio del periodo solicitado</param>
//		/// <returns>La información de fin de ruta de todos
//		/// los vehiculos en el SIGAMET</returns>
//		internal DataTable ObtenTodosFinesRutaSIGAMET(
//			DateTime fecha
//		)
//		{
//			DataTable valorregreso = new DataTable();
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataAdapter adaptador = fabricaBD.CreateDataAdapter();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoBuscaTodosFinesRuta
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT Autotanque,
//					MIN(FLlamada) AS FLlamada
//					FROM Llamada WITH (NOLOCK)
//					WHERE Autotanque > 0
//					AND MotivoLlamada = 40
//					AND DAY(FLlamada) = DAY(@Fecha)
//					AND MONTH(FLlamada) = MONTH(@Fecha)
//					AND YEAR(FLlamada) = YEAR(@Fecha)
//					GROUP BY Autotanque";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Fecha";
//			parametro.Value = fecha;
//			comandosql.Parameters.Add(parametro);
//
//			
//			adaptador.SelectCommand = comandosql;
//			
//			adaptador.Fill(valorregreso);
//			
//			return valorregreso;
//		}
//		
//		/// <summary>
//		/// Obten la hora de la última posición de pánico
//		/// del vehículo
//		/// </summary>
//		/// <param name="vehiculo">El ID de BD del vehículo</param>
//		/// <param name="fechainicio">Fecha inicial del rango de busqueda</param>
//		/// <param name="fechafin">Fecha final del rango de busqueda</param>
//		/// <returns>La hora de la última posición
//		/// de pánico del vehículo</returns>
//		internal object ObtenHoraUltimoPanico(int vehiculo,
//		                                      DateTime fechainicio,
//		                                      DateTime fechafin) {
//			object valorregreso = null;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaUltimoPanico
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT TOP 1
//						FGps
//						FROM Posicion
//						WHERE
//						FGps BETWEEN @FechaInicio AND @FechaFin
//						AND Vehiculo = @Vehiculo
//						AND TipoPosicion = 2
//						ORDER BY FGps DESC";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaInicio";
//			parametro.Value = fechainicio;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaFin";
//			parametro.Value = fechafin;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//
//			conexion.Open();
//			valorregreso = comandosql.ExecuteScalar();
//			conexion.Close();
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Obten la hora de la última posición
//		/// del vehículo
//		/// </summary>
//		/// <param name="vehiculo">El ID de BD del vehículo</param>
//		/// <param name="fechainicio">Fecha inicial del rango de busqueda</param>
//		/// <param name="fechafin">Fecha final del rango de busqueda</param>
//		/// <returns>La hora de la última posición del vehículo</returns>
//		internal object ObtenHoraUltimaPosicion(int vehiculo,
//		                                        DateTime fechainicio,
//		                                        DateTime fechafin) {
//			object valorregreso = null;
//			DbCommand comandosql = conexion.CreateCommand();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoSeleccionaFechaUltimaPosicion
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"SELECT TOP 1
//						FGps
//						FROM UltimaPosicion
//						WHERE
//						FGps BETWEEN @FechaInicio AND @FechaFin
//						AND Vehiculo = @Vehiculo
//						ORDER BY FGps DESC";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaInicio";
//			parametro.Value = fechainicio;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@FechaFin";
//			parametro.Value = fechafin;
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.Value = vehiculo;
//			comandosql.Parameters.Add(parametro);
//			
//
//			conexion.Open();
//			valorregreso = comandosql.ExecuteScalar();
//			conexion.Close();
//			
//			return valorregreso != null ? valorregreso : DBNull.Value;
//		}
//		
//		/// <summary>
//		/// Actualiza los datos de los vehiculos indicados en la tabla
//		/// dtvehiculos
//		/// </summary>
//		/// <param name="dtvehiculos">Tabla con la información de los vehículos</param>
//		/// <returns>El número de vehículos actualizados</returns>
//		public int ActualizaVehiculos(DataTable dtvehiculos)
//		{
//			DbCommand comandosql = conexion.CreateCommand();
//			DbDataAdapter adaptador = fabricaBD.CreateDataAdapter();
//			DbParameter parametro;
//			
//			// spMSTableroMonitoreoActualizaVehiculo
//			comandosql.CommandTimeout = tiempoEsperaEjecucionComandos_ms;
//			comandosql.CommandType = CommandType.Text;
//			comandosql.CommandText =
//				@"UPDATE TableroMonitoreo
//					SET
//					--Grupo = @Grupo,
//					--Ruta = @Ruta,
//					--Vehiculo = @Vehiculo,
//					--Fecha = @Fecha,
//					HoraSalidaPlanta = @HoraSalidaPlanta,
//					HoraIngresoRuta = @HoraIngresoRuta,
//					HoraPrimerSuministro = @HoraPrimerSuministro,
//					HoraUltimoSuministro = @HoraUltimoSuministro,
//					HoraFinRuta = @HoraFinRuta,
//					HoraSalidaRuta = @HoraSalidaRuta,
//					HoraIngresoPlanta = @HoraIngresoPlanta,
//					HoraUltimoPanico = @HoraUltimoPanico,
//					EstadoUbicacion = @EstadoUbicacion,
//					HoraUltimaPosicion = @HoraUltimaPosicion
//					WHERE
//					Vehiculo = @Vehiculo AND
//					Fecha = @Fecha";
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Grupo";
//			parametro.SourceColumn = "Grupo";
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Ruta";
//			parametro.SourceColumn = "Ruta";
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Vehiculo";
//			parametro.SourceColumn = "Vehiculo";
//			comandosql.Parameters.Add(parametro);
//
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@Fecha";
//			parametro.SourceColumn = "Fecha";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraSalidaPlanta";
//			parametro.SourceColumn = "HoraSalidaPlanta";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraIngresoRuta";
//			parametro.SourceColumn = "HoraIngresoRuta";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraPrimerSuministro";
//			parametro.SourceColumn = "HoraPrimerSuministro";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraUltimoSuministro";
//			parametro.SourceColumn = "HoraUltimoSuministro";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraFinRuta";
//			parametro.SourceColumn = "HoraFinRuta";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraSalidaRuta";
//			parametro.SourceColumn = "HoraSalidaRuta";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraIngresoPlanta";
//			parametro.SourceColumn = "HoraIngresoPlanta";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraUltimoPanico";
//			parametro.SourceColumn = "HoraUltimoPanico";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@EstadoUbicacion";
//			parametro.SourceColumn = "EstadoUbicacion";
//			comandosql.Parameters.Add(parametro);
//			
//			parametro = comandosql.CreateParameter();
//			parametro.ParameterName = "@HoraUltimaPosicion";
//			parametro.SourceColumn = "HoraUltimaPosicion";
//			comandosql.Parameters.Add(parametro);
//
//			
//			adaptador.UpdateCommand = comandosql;
//			
//			
//			return adaptador.Update(dtvehiculos);
//		}
	}
}
