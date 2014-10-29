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
 * 
 * @author Rodolfo Conde
 * 
 */
public class ToolBox {
    private ToolBox() { }
    
    
    public static char []byteArray2charArray(byte []buffer, int start, int length) {
        return null;
    }
    
    ///// http://stackoverflow.com/questions/9655181/convert-from-byte-array-to-hex-string-in-java
    
    /* Fast byte-array to hex char-array implementation */
    
    final private static char[] hexArray = "0123456789ABCDEF".toCharArray();
    
    private static char []bytesToHex(byte[] bytes, int start, int length) {
        char[] hexChars = new char[length * 2];
        for ( int j = start; j < length; j++ ) {
            int v = bytes[j] & 0xFF;
            hexChars[j * 2] = hexArray[v >>> 4];
            hexChars[j * 2 + 1] = hexArray[v & 0x0F];
        }
        
        return hexChars;
    }   
    
    ///// http://stackoverflow.com/questions/9655181/convert-from-byte-array-to-hex-string-in-java    
    
    
    public static char []bytes2Hexchars(byte []thebytes, int start, int length) {
        return bytesToHex(thebytes, start, length);
    }
    
    public static char []bytes2Hexchars(byte []thebytes) {
        return bytesToHex(thebytes, 0, thebytes.length);
    }
    
    
    public static class StringTools {
        public static final String Empty = "";
        
        
        private StringTools() { }
        
        
        public static boolean isNullOrEmpty(String astring) {
            return astring == null || Empty.equalsIgnoreCase(astring);
        }
        
        public static boolean isNullOrWhitespace(String astring) {
            boolean valorregreso; 
            
            valorregreso = (astring == null || Empty.equalsIgnoreCase(astring));
                
            if (!valorregreso) {
                for (int i = 0; i < astring.length(); ++i) {
                    if (!Character.isWhitespace(astring.charAt(i))) {
                        valorregreso = false;
                        break;
                    }
                }   
            }
            
            return valorregreso;
        }
    }
}
