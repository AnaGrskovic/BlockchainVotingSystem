package diplrad.helpers;

public class WaitHelper {

    public static void doUselessWork(long numberOfMinutes) {
        long endTime = System.currentTimeMillis() + numberOfMinutes * 60 * 1000;
        while (System.currentTimeMillis() < endTime) {
            int result = 0;
            for (int i = 0; i < 100000; i++) {
                result += i;
            }
        }
    }

}
