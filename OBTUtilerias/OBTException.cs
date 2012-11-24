/***************************************************************************
 *   Copyright (C) 2011 by Rodolfo Conde Martinez                          *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;
using System.Runtime.Serialization;

namespace OBTUtils
{
	/// <summary>
	/// An exception class with printf-like constructor (in the .Net way).
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	[Serializable]
	public class OBTException : Exception, ISerializable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public OBTException()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message related to this exception</param>
		public OBTException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message related to this exception</param>
		/// <param name="innerexception">An exception related to this exception</param>
		public OBTException(string message, Exception innerexception) 
			: base(message, innerexception)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="args">arguments to be replaces inside the format
		/// string</param>
		public OBTException(string format, params object []args)
			: base(String.Format(format, args))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">Format string</param>
		/// <param name="innerException">Some exception that ocurred before
		/// this exception</param>
		/// <param name="args">arguments to be replaces inside the format
		/// string</param>
		public OBTException(string format, Exception innerexception,
		                    params object []args)
			: base(String.Format(format, args), innerexception)
		{
		}

		
		// This constructor is needed for serialization.
		/// <summary>
		/// Protected constructor
		/// </summary>
		/// <param name="info">Information for serialization</param>
		/// <param name="context">Streaming context</param>
		protected OBTException(SerializationInfo info,
		                       StreamingContext context)
			: base(info, context)
		{
		}
	}
}
