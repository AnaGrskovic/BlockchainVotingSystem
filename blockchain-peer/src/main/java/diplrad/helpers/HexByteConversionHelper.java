package diplrad.helpers;

import java.util.Locale;
import java.util.Map;

public class HexByteConversionHelper {

    public static Map<String, Integer> hexLetters = Map.of("a", 10, "b", 11, "c", 12, "d", 13, "e", 14, "f", 15);
    public static Map<Integer, String> hexNumbers = Map.of(10, "a", 11, "b", 12, "c", 13, "d", 14, "e", 15, "f");

    public static byte[] hexToByte(String keyText) {
        int length = keyText.length();
        if (length % 2 == 1) throw new IllegalArgumentException();
        byte[] keyTextArray = new byte[length / 2];
        for (int i = 0; i < length; i += 2) {
            char firstChar = Character.toLowerCase(keyText.charAt(i));
            char secondChar = Character.toLowerCase(keyText.charAt(i + 1));
            int firstNum;
            int secondNum;
            if (hexLetters.containsKey(String.valueOf(firstChar))) {
                firstNum = hexLetters.get(String.valueOf(firstChar));
            } else if (Character.isDigit(firstChar)) {
                firstNum = Character.getNumericValue(firstChar);
            } else throw new IllegalArgumentException();
            if (hexLetters.containsKey(String.valueOf(secondChar))) {
                secondNum = hexLetters.get(String.valueOf(secondChar));
            } else if (Character.isDigit(secondChar)) {
                secondNum = Character.getNumericValue(secondChar);
            } else throw new IllegalArgumentException();
            int result = 16 * firstNum + secondNum;
            keyTextArray[i / 2] = (byte) result;
        }
        return keyTextArray;
    }

    public static String byteToHex(byte[] bytearray) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bytearray.length; i++) {
            int num = bytearray[i];
            if (num < 0) num += 256;   // turning negative number to positive
            int first = num / 16;
            int second = num % 16;
            if (hexNumbers.containsKey(first)) sb.append(hexNumbers.get(num / 16));
            else sb.append(first);
            if (hexNumbers.containsKey(second)) sb.append(hexNumbers.get(num % 16));
            else sb.append(second);
        }
        return sb.toString().toLowerCase(Locale.ROOT);
    }

}
