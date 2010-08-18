package com.ttime.gui;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JComponent;
import javax.swing.JEditorPane;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JSplitPane;
import javax.swing.JTabbedPane;
import javax.swing.JTextPane;
import javax.swing.KeyStroke;

public class MainWindow extends JFrame {
    private static final long serialVersionUID = 1L;

    JEditorPane courseInfo;

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

        return menuBar;
    }

    JComponent createCourseListTab() {
        JPanel topHalf = new JPanel();
        GridLayout layout = new GridLayout(1, 2);
        topHalf.setLayout(layout);

        courseInfo = new JEditorPane();
        courseInfo.setContentType("text/html");
        courseInfo.setText("<h1>Hello, world!</h1><p>How's about some courses?</p>");
        courseInfo.setEditable(false);

        courseInfo.setPreferredSize(new Dimension(50, 50));

        JPanel availableCoursesPanel = new JPanel(new BorderLayout());
        availableCoursesPanel.setBorder(BorderFactory
                .createTitledBorder("Available courses"));
        availableCoursesPanel.add(new JScrollPane(new JTextPane()),
                BorderLayout.CENTER);

        JPanel selectedCoursesPanel = new JPanel(new BorderLayout());
        selectedCoursesPanel.setBorder(BorderFactory
                .createTitledBorder("Selected courses"));
        selectedCoursesPanel.add(new JScrollPane(new JTextPane()),
                BorderLayout.CENTER);

        topHalf.add(availableCoursesPanel);
        topHalf.add(selectedCoursesPanel);

        JSplitPane splitPane = new JSplitPane(JSplitPane.VERTICAL_SPLIT);
        splitPane.add(topHalf);
        splitPane.add(new JScrollPane(courseInfo));

        splitPane.setDividerLocation(400);

        return splitPane;
    }

    JTabbedPane createTabbedPane() {
        JTabbedPane tabbedPane = new JTabbedPane();

        tabbedPane.addTab("Course List", createCourseListTab());

        JButton scheduleTab = new JButton("Schedule");

        tabbedPane.addTab("Schedule", scheduleTab);

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

}
