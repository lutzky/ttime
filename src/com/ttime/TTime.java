package com.ttime;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.text.ParseException;

public class TTime {

    /**
     * @param args
     */
    public static void main(String[] args) {
        try {
            com.ttime.parse.Repy r = new com.ttime.parse.Repy(new File(
                    "/home/ohad/.ttime/data/REPY"));
            System.out.println(r);
        } catch (FileNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (ParseException e) {
            // TODO Auto-generated catch block
            System.err.printf("Parse error in REPY line %d: %s\n", e
                    .getErrorOffset(), e.getMessage());
            e.printStackTrace();
        }
    }
}
