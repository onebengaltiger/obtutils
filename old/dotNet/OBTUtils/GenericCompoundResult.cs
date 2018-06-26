/***************************************************************************
 *   Copyright (C) 2011-2015 by Rodolfo Conde Martinez                     *
 *   rcm@gmx.co.uk                                                         *
 ***************************************************************************/


using System;

namespace OBTUtils
{
	/// <summary>
	/// Generic class to be used as the return value
	/// of methods that execute complex operations and the
	/// programmer wants to give to the function caller as much
	///  information as possible about what happen with the
	///  invoked operation. The class can be extended to include
	///  extra functionality
	/// </summary>
	/// 
	/// <remarks>\author Rodolfo Conde</remarks>
	public class GenericCompoundResult
	{
		/// <summary>
		/// Indicates wether the executed operation
		/// was successful
		/// </summary>
		private bool __wasSuccessful;
		
		/// <summary>
		/// This can contain some bit of information about
		/// the executed operation
		/// </summary>
		private string __additionalDetails;
		
		/// <summary>
		/// Indicates exactly what kind of error happened
		/// while the operation was being performed
		/// </summary>
		private Exception __errorInformation;
		
		/// <summary>
		/// Indicates an error code, dependent on the application
		/// using this class
		/// </summary>
		private int __errno;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult(bool wassucceful, string details, Exception error,
		                             int errorcode)
		{
			__wasSuccessful = wassucceful;
			__additionalDetails = details;
			__errorInformation = error;
			__errno = errorcode;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult(bool wassucceful, string details, Exception error)
			: this(wassucceful, details, error, 0)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult(bool wassucceful, string details) :
			this(wassucceful, details, null)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult(bool wassucceful, Exception error) :
			this(wassucceful, String.Empty, error)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult(bool wassucceful) :
			this(wassucceful, String.Empty, null)
		{ }
		
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericCompoundResult() :
			this(false, String.Empty, null)
		{ }
		
		
		/// <summary>
		/// Destructor
		/// </summary>
		~GenericCompoundResult()
		{
			__additionalDetails = null;
			__errorInformation = null;
		}
		
		
		/// <summary>
		/// Gets a string representation of this object
		/// </summary>
		/// <returns>A representing the instance</returns>
		public override string ToString()
		{
			return string.Format("[Operation succeded={0}, AdditionalDetails={1}, " +
			                     "Error code: {3}, " +
			                     "ErrorInformation={2}]", __wasSuccessful,
			                     __additionalDetails,
			                     (__errorInformation == null ? "None" :
			                      __errorInformation.ToString()),
			                     __errno);
		}

		
		/// <summary>
		/// Gets or sets a value that indicates
		/// wether the executed operation
		/// was successful
		/// </summary>
		public bool WasSucceful {
			get {
				return __wasSuccessful;
			}
			
			set {
				__wasSuccessful = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a value which can contain
		/// some bit of information about
		/// the executed operation
		/// </summary>
		public string AdditionalDetails {
			get {
				return __additionalDetails;
			}
			
			set {
				__additionalDetails = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a value that indicates exactly
		/// what kind of error happened
		/// while the operation was being performed
		/// </summary>
		public Exception ErrorInformation {
			get {
				return __errorInformation;
			}
			
			set {
				__errorInformation = value;
			}
		}
		
		/// <summary>
		/// Gets or sets an error code, dependent on the application
		/// using this class
		/// </summary>
		public int Errno {
			get {
				return __errno;
			}
			
			set {
				__errno = value;
			}
		}
	}
}
