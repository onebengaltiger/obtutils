/***************************************************************************
 *   Copyright (C) 2012-2015 by Rodolfo Conde Martínez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/



using System;
using System.Runtime.Serialization;

namespace OBTUtils.Data
{
	/// <summary>
	/// Exception class for error within the database
	/// methods
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class OBTDataException : OBTException, ISerializable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public OBTDataException()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message related to this exception</param>
		public OBTDataException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message related to this exception</param>
		/// <param name="innerexception">An exception related to this exception</param>
		public OBTDataException(string message, Exception innerexception)
			: base(message, innerexception)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="args">arguments to be replaces inside the format
		/// string</param>
		public OBTDataException(string format, params object []args)
			: base(format, args)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="innerexception">Some exception that ocurred before
		/// this exception</param>
		/// <param name="args">arguments to be replaces inside the format
		/// string</param>
		public OBTDataException(string format, Exception innerexception,
		                        params object []args)
			: base(format, innerexception, args)
		{
		}

		
		// This constructor is needed for serialization.
		/// <summary>
		/// Protected constructor
		/// </summary>
		/// <param name="info">Information for serialization</param>
		/// <param name="context">Streaming context</param>
		protected OBTDataException(SerializationInfo info,
		                           StreamingContext context)
			: base(info, context)
		{
		}
	}
}
