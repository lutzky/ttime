package com.ttime;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.text.ParseException;
import java.util.Set;
import java.util.logging.Level;
import java.util.logging.Logger;

import com.ttime.gui.MainWindow;
import com.ttime.logic.Faculty;
import com.ttime.parse.Repy;

public class TTime {

    private static Set<Faculty> faculties;

    public static int seconds(String time) {
        int result = 0;
        String[] split = time.split(":");
        assert (split.length == 3 || split.length == 2);
        result += new Integer(split[0]) * 3600;
        result += new Integer(split[1]) * 60;
        if (split.length == 3) {
            result += new Integer(split[2]);
        }

        return result;
    }

    /**
     * @param args
     */
    public static void main(String[] args) {
        Logger log = Logger.getLogger("global").getParent();
        log.getHandlers()[0].setLevel(Level.ALL);
        log.setLevel(Level.INFO);

        try {
            Repy.DEFAULT_PATH.getParentFile().mkdirs();
            Update.downloadRepy();
            Repy r = new Repy(Repy.DEFAULT_PATH);
            faculties = r.getFaculties();

            try {
                // UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }

            javax.swing.SwingUtilities.invokeLater(new Runnable() {
                @Override
                public void run() {
                    MainWindow mw = new MainWindow();
                    mw.setFaculties(faculties);
                }
            });
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
