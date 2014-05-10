/*
 *  This file is part of OBTUtils SDK for Java.
 *
 *  OBTUtils SDK for Java is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  OBTUtils SDK for Java is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with OBTUtils SDK for Java.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

package org.onebengaltiger.obtutils.generic;

import java.io.Serializable;

/**
 *
 * @author Rodolfo Conde
 * 
 */
public class GenericCompoundResult<TStatus, TInfo> implements Serializable {
    /// <summary>
    /// Indicates wether the executed operation
    /// was successful
    /// </summary>
    private boolean wasSuccessful;
		
    /// <summary>
    /// This can contain some bit of information about
    /// the executed operation
    /// </summary>
    private Object additionalDetails;
		
    /// <summary>
    /// This field can contain some extra result
    /// of the executed operation
    /// </summary>
    private TInfo moreInformation;
		
    /// <summary>
    /// Indicates exactly what kind of error happened
    /// while the operation was being performed
    /// </summary>
    private Exception errorInformation;
		
    /// <summary>
    /// Indicates an error code, dependent on the application
    /// using this class
    /// </summary>
    private TStatus errno;

    
    public GenericCompoundResult(boolean wasSuccessful, Object additionalDetails, 
            TInfo moreInformation, Exception errorInformation, TStatus errno) {
        this.wasSuccessful = wasSuccessful;
        this.additionalDetails = additionalDetails;
        this.moreInformation = moreInformation;
        this.errorInformation = errorInformation;
        this.errno = errno;
    }

    public GenericCompoundResult() {
        this(false, null, null, null, null);        
    }
    
    
    @Override
    protected void finalize() throws Throwable {
        additionalDetails = null;
        moreInformation = null;
        errorInformation = null;
        errno = null;
        
        super.finalize();
    }

    
    public boolean isWasSuccessful() {
        return wasSuccessful;
    }

    public void setWasSuccessful(boolean wasSuccessful) {
        this.wasSuccessful = wasSuccessful;
    }

    public Object getAdditionalDetails() {
        return additionalDetails;
    }

    public void setAdditionalDetails(Object additionalDetails) {
        this.additionalDetails = additionalDetails;
    }

    public TInfo getMoreInformation() {
        return moreInformation;
    }

    public void setMoreInformation(TInfo moreInformation) {
        this.moreInformation = moreInformation;
    }

    public Exception getErrorInformation() {
        return errorInformation;
    }

    public void setErrorInformation(Exception errorInformation) {
        this.errorInformation = errorInformation;
    }

    public TStatus getErrno() {
        return errno;
    }

    public void setErrno(TStatus errno) {
        this.errno = errno;
    }

    
    @Override
    public String toString() {
        return String.format("[Operation succeded: %b, Additional details: %s, " +
                             "Result: %s, Error code: %s, " +
	                     "Error information: %s]",
	                     wasSuccessful,
	                     additionalDetails,
	                     moreInformation,
	                     errno,
	                     (errorInformation == null ? "None" :
	                      errorInformation.toString())
	                    );
    }
}
