package com.ttime.gui;

import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.util.Comparator;
import java.util.LinkedList;
import java.util.List;
import java.util.Set;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JTabbedPane;
import javax.swing.KeyStroke;

import com.ttime.constraints.ConstraintManager;
import com.ttime.logic.Faculty;
import com.ttime.logic.Schedule;
import com.ttime.logic.Scheduler;

@SuppressWarnings("serial")
public class MainWindow extends JFrame {
    SchedulePanel schedulePanel = new SchedulePanel();
    CourseListPanel courseListPanel = new CourseListPanel();

    JMenuBar createMenuBar() {
        JMenuBar menuBar = new JMenuBar();

        JMenu file;

        file = new JMenu("File");
        file.setMnemonic(KeyEvent.VK_F);

        JMenuItem quit = new JMenuItem("Quit");
        quit.setMnemonic(KeyEvent.VK_Q);
        quit.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_Q,
                KeyEvent.CTRL_MASK));

        quit.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent arg0) {
                System.exit(0);
            }
        });

        file.add(quit);

        menuBar.add(file);

        JMenu schedules = new JMenu("Schedules");
        schedules.setMnemonic(KeyEvent.VK_S);

        JMenuItem findSchedules = new JMenuItem("Find schedules");
        findSchedules.setMnemonic(KeyEvent.VK_S);

        findSchedules.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent arg0) {
                // TODO This needs to happen in a separate thread
                Scheduler scheduler = new Scheduler(courseListPanel.getSelectedCourses(),
                        // TODO pass appropriate comparators and constraints
                        ConstraintManager.getInstance().getConstraints(),
                        new LinkedList<Comparator<Schedule>>()
                        );
                List<Schedule> schedules = scheduler.findSchedules();
                schedulePanel.setSchedules(schedules);
            }
        });

        schedules.add(findSchedules);

        menuBar.add(schedules);

        return menuBar;
    }

    JTabbedPane createTabbedPane() {
        JTabbedPane tabbedPane = new JTabbedPane();

        tabbedPane.addTab("Course List", courseListPanel);

        tabbedPane.addTab("Schedule", schedulePanel);

        JButton constraintsTab = new JButton("Constraints");

        tabbedPane.addTab("Constraints", constraintsTab);

        JButton ratingsTab = new JButton("Schedule ratings");

        tabbedPane.addTab("Schedule ratings", ratingsTab);

        return tabbedPane;
    }

    public MainWindow() {
        super("TTime");
        setPreferredSize(new Dimension(800, 600));

        setDefaultCloseOperation(EXIT_ON_CLOSE);

        getContentPane().add(createTabbedPane());

        setJMenuBar(createMenuBar());

        pack();

        setVisible(true);
    }

    public void setFaculties(Set<Faculty> faculties) {
        courseListPanel.setFaculties(faculties);
    }
}
