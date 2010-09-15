package com.ttime.gui;

import java.awt.BorderLayout;
import java.awt.ComponentOrientation;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.util.Set;
import java.util.TreeSet;

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
import javax.swing.JTree;
import javax.swing.KeyStroke;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;

import com.ttime.logic.Course;
import com.ttime.logic.Faculty;

public class MainWindow extends JFrame {
    private static final long serialVersionUID = 1L;

    JEditorPane courseInfo;

    SchedulePanel schedulePanel = new SchedulePanel();

    DefaultMutableTreeNode availableCoursesRoot = new DefaultMutableTreeNode(
            "Available Courses");
    DefaultTreeModel availableCoursesModel = new DefaultTreeModel(
            availableCoursesRoot);
    JTree availableCoursesTree = new JTree(availableCoursesModel);

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
        courseInfo
                .setText("<h1>Hello, world!</h1><p>How's about some courses?</p>");
        courseInfo.setEditable(false);

        courseInfo.setPreferredSize(new Dimension(50, 50));

        JPanel availableCoursesPanel = new JPanel(new BorderLayout());
        availableCoursesPanel.setBorder(BorderFactory
                .createTitledBorder("Available courses"));
        availableCoursesPanel.add(new JScrollPane(availableCoursesTree),
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

        availableCoursesTree
                .addTreeSelectionListener(new TreeSelectionListener() {

                    @Override
                    public void valueChanged(TreeSelectionEvent ev) {
                        try {
                            DefaultMutableTreeNode node = (DefaultMutableTreeNode) availableCoursesTree
                                    .getLastSelectedPathComponent();
                            if (node != null) {
                                Course course = (Course) node.getUserObject();
                                courseInfo
                                        .setComponentOrientation(ComponentOrientation.RIGHT_TO_LEFT);
                                courseInfo.setText(course.getHtmlInfo());
                            }
                        } catch (ClassCastException e) {
                            // Do nothing
                        }
                    }
                });

        return splitPane;
    }

    JTabbedPane createTabbedPane() {
        JTabbedPane tabbedPane = new JTabbedPane();

        tabbedPane.addTab("Course List", createCourseListTab());

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

    public void populateFaculties(Set<Faculty> faculties) {
        for (Faculty faculty : new TreeSet<Faculty>(faculties)) {
            DefaultMutableTreeNode facultyNode = new DefaultMutableTreeNode(
                    faculty);

            availableCoursesModel.insertNodeInto(facultyNode,
                    availableCoursesRoot, availableCoursesRoot.getChildCount());

            for (Course course : new TreeSet<Course>(faculty.getCourses())) {
                DefaultMutableTreeNode courseNode = new DefaultMutableTreeNode(
                        course);
                availableCoursesModel.insertNodeInto(courseNode, facultyNode,
                        facultyNode.getChildCount());
            }
        }

        availableCoursesTree.expandPath(new TreePath(availableCoursesRoot
                .getPath()));
    }
}
