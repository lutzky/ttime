package com.ttime;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.text.ParseException;
import java.util.logging.Level;
import java.util.logging.Logger;

public class TTime {

    /**
     * @param args
     */
    public static void main(String[] args) {
        Logger log = Logger.getLogger("global").getParent();
        log.getHandlers()[0].setLevel(Level.ALL);
        log.setLevel(Level.INFO);

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
