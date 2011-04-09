package com.ttime;

public class Utils {
        public static String formatTime(int secondsSinceMidnight) {
                int hours = secondsSinceMidnight / 3600;
                int minutes = (secondsSinceMidnight / 60) % 60;
                return String.format("%02d:%02d", hours, minutes);
        }
}
