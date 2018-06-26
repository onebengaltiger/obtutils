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

package org.onebengaltiger.obtutils.utils;

/**
 *
 * A generic exception class with some useful constructors
 * 
 * @author Rodolfo Conde
 * 
 */
public class OBTException extends Exception {

    /**
     * 
     * Creates a new instance of
     * <code>OBTException</code> without detail message.
     * 
     */
    public OBTException() {
    }

    /**
     * 
     * Constructs an instance of
     * <code>OBTException</code> with the specified detail message.
     *
     * @param msg The detail message.
     * 
     */
    public OBTException(String msg) {
        super(msg);
    }
    

    /**
     *
     * Constructs an instance of
     * <code>OBTException</code> with the specified detail message
     * and the given throwable object
     * 
     * @param msg The detail message.
     * @param athrowable An instance of type <code>Throwable</code>
     * 
     */
    public OBTException(String msg, Throwable athrowable) {
        super(msg, athrowable);
    }
    
    /**
     *
     * Constructs an instance of
     * <code>OBTException</code> with the specified throwable object
     * 
     * @param athrowable An instance of type <code>Throwable</code>
     * 
     */
    public OBTException(Throwable athrowable) {
        super(athrowable);
    }
    
    /**
     *
     * Constructs an instance of
     * <code>OBTException</code> with a message given 
     * by a format string and some arguments
     * 
     * @param format The format string of the message
     * @param arguments The arguments of the format string
     * 
     */
    public OBTException(String format, Object... arguments) {
        super(String.format(format, arguments));
    }
    
    /**
     *
     * Constructs an instance of
     * <code>OBTException</code> with the specified throwable object and a message given 
     * by a format string and some arguments
     * 
     * @param athrowable An instance of type <code>Throwable</code>
     * @param format The format string of the message
     * @param arguments The arguments of the format string
     * 
     */
    public OBTException(Throwable athrowable, String format, Object... arguments) {
        super(String.format(format, arguments), athrowable);
    }
}
