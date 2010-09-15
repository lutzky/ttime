package com.ttime.gui;

import java.awt.BorderLayout;
import java.awt.ComponentOrientation;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.util.Set;
import java.util.TreeSet;

import javax.swing.BorderFactory;
import javax.swing.JEditorPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JSplitPane;
import javax.swing.JTextPane;
import javax.swing.JTree;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;

import com.ttime.logic.Course;
import com.ttime.logic.Faculty;

public class CourseListPanel extends JSplitPane {
    JEditorPane courseInfo = new JEditorPane();

    DefaultMutableTreeNode availableCoursesRoot = new DefaultMutableTreeNode(
            "Available Courses");
    DefaultTreeModel availableCoursesModel = new DefaultTreeModel(
            availableCoursesRoot);
    JTree availableCoursesTree = new JTree(availableCoursesModel);

    Set<Faculty> faculties;

    CourseListPanel() {
        super(JSplitPane.VERTICAL_SPLIT);
        JPanel topHalf = new JPanel();
        GridLayout layout = new GridLayout(1, 2);
        topHalf.setLayout(layout);

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

        this.add(topHalf);
        this.add(new JScrollPane(courseInfo));

        this.setDividerLocation(400);

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
    }

    public void setFaculties(Set<Faculty> faculties) {
        this.faculties = faculties;
        populateFaculties();
    }

    void populateFaculties() {
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
