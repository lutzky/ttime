package com.ttime.gui;

import java.awt.BorderLayout;
import java.awt.ComponentOrientation;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.util.Collection;
import java.util.LinkedList;
import java.util.Set;
import java.util.TreeSet;

import javax.swing.BorderFactory;
import javax.swing.DefaultListModel;
import javax.swing.GroupLayout;
import javax.swing.JButton;
import javax.swing.JEditorPane;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JSplitPane;
import javax.swing.JTree;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;

import com.ttime.logic.Course;
import com.ttime.logic.Faculty;

@SuppressWarnings("serial")
public class CourseListPanel extends JSplitPane {
    JEditorPane courseInfo = new JEditorPane();

    DefaultMutableTreeNode availableCoursesRoot = new DefaultMutableTreeNode(
            "Available Courses");
    DefaultTreeModel availableCoursesModel = new DefaultTreeModel(
            availableCoursesRoot);
    JTree availableCoursesTree = new JTree(availableCoursesModel);

    DefaultListModel selectedCoursesModel = new DefaultListModel();
    JList selectedCoursesList = new JList(selectedCoursesModel);

    Set<Faculty> faculties;

    private final Collection<Course> selectedCourses = new LinkedList<Course>();

    CourseListPanel() {
        super(JSplitPane.VERTICAL_SPLIT);
        JPanel topHalf = new JPanel();
        GroupLayout layout = new GroupLayout(topHalf);
        topHalf.setLayout(layout);

        JButton btnAdd = new JButton("Add");
        btnAdd.setMnemonic(KeyEvent.VK_A);
        JButton btnRemove = new JButton("Remove");
        btnRemove.setMnemonic(KeyEvent.VK_R);

        btnAdd.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                addCurrentCourse();
            }
        });

        btnRemove.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                removeCurrentCourse();
            }
        });

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
        selectedCoursesPanel.add(new JScrollPane(selectedCoursesList),
                BorderLayout.CENTER);

        layout.setHorizontalGroup(layout.createSequentialGroup().addGroup(
                layout.createParallelGroup()
                        .addComponent(availableCoursesPanel).addComponent(
                                btnAdd, GroupLayout.DEFAULT_SIZE,
                                GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addGroup(
                        layout.createParallelGroup().addComponent(
                                selectedCoursesPanel).addComponent(btnRemove,
                                GroupLayout.DEFAULT_SIZE,
                                GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)));

        layout.setVerticalGroup(layout.createSequentialGroup().addGroup(
                layout.createParallelGroup()
                        .addComponent(availableCoursesPanel).addComponent(
                                selectedCoursesPanel)

        ).addGroup(
                layout.createParallelGroup().addComponent(btnAdd).addComponent(
                        btnRemove)));

        this.add(topHalf);
        this.add(new JScrollPane(courseInfo));

        this.setDividerLocation(400);

        availableCoursesTree
                .addTreeSelectionListener(new TreeSelectionListener() {
                    @Override
                    public void valueChanged(TreeSelectionEvent ev) {
                        onTreeSelect(ev);
                    }
                });
    }

    protected void removeCurrentCourse() {
        if (!selectedCoursesList.isSelectionEmpty()) {
            Course c = (Course) selectedCoursesList.getSelectedValue();
            selectedCoursesModel.remove(selectedCoursesList.getSelectedIndex());
            selectedCourses.remove(c);
        }
    }

    void addCurrentCourse() {
        if (getSelectedAddableCourse() != null) {
            selectedCoursesModel.addElement(getSelectedAddableCourse());
            selectedCourses.add(getSelectedAddableCourse());
        }
    }

    Collection<Course> getSelectedCourses() {
        return selectedCourses;
    }

    Course getSelectedAddableCourse() {
        try {
            DefaultMutableTreeNode node = (DefaultMutableTreeNode) availableCoursesTree
                    .getLastSelectedPathComponent();
            if (node != null) {
                return (Course) node.getUserObject();
            }
        } catch (ClassCastException e) {
        }
        return null;
    }

    void onTreeSelect(TreeSelectionEvent ev) {
        if (getSelectedAddableCourse() != null) {
            courseInfo
                    .setComponentOrientation(ComponentOrientation.RIGHT_TO_LEFT);
            courseInfo.setText(getSelectedAddableCourse().getHtmlInfo());
        }
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
