package com.ttime.gui;

import java.awt.BorderLayout;
import java.awt.ComponentOrientation;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
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

		availableCoursesTree.addMouseListener(new MouseListener() {

			@Override
			public void mouseReleased(MouseEvent e) {
				// TODO Auto-generated method stub
			}

			@Override
			public void mousePressed(MouseEvent e) {
				// TODO Auto-generated method stub
			}

			@Override
			public void mouseExited(MouseEvent e) {
				// TODO Auto-generated method stub
			}

			@Override
			public void mouseEntered(MouseEvent e) {
				// TODO Auto-generated method stub
			}

			@Override
			public void mouseClicked(MouseEvent e) {
				if (e.getClickCount() == 2) {
					addCurrentCourse();
				}
			}
		});

		availableCoursesTree.addKeyListener(new KeyListener() {

			@Override
			public void keyPressed(KeyEvent arg0) {
				if (arg0.getKeyCode() == KeyEvent.VK_ENTER) {
					addCurrentCourse();
				}
			}

			@Override
			public void keyReleased(KeyEvent arg0) {
			}

			@Override
			public void keyTyped(KeyEvent arg0) {
			}
		});
	}

	protected void removeCurrentCourse() {
		if (!selectedCoursesList.isSelectionEmpty()) {
			Object[] selectedRemovalCourses = selectedCoursesList
					.getSelectedValues();
			int[] selectedRemovalIndices = selectedCoursesList.getSelectedIndices();

			for (Object o : selectedRemovalCourses) {
				selectedCourses.remove(o);
			}

			for (int i = selectedRemovalIndices.length - 1; i >= 0; i--) {
				/*
				 * Remove from list in reverse order, to avoid removing the
				 * wrong element due to renumbering on delete.
				 */
				selectedCoursesModel.remove(selectedRemovalIndices[i]);
			}
		}
	}

	void addCurrentCourse() {
		for (Course c : getSelectedAddableCourses()) {
			if (c != null) {
				selectedCoursesModel.addElement(c);
				selectedCourses.add(c);
			}
		}
	}

	Collection<Course> getSelectedCourses() {
		return selectedCourses;
	}

	Course[] getSelectedAddableCourses() {
		TreePath[] selectedPaths = availableCoursesTree.getSelectionPaths();
		Course[] result = new Course[selectedPaths.length];
		for (int i = 0; i < selectedPaths.length; i++) {
			DefaultMutableTreeNode node = (DefaultMutableTreeNode) selectedPaths[i]
					.getLastPathComponent();

			try {
				result[i] = (node == null) ? null : (Course) node
						.getUserObject();
			} catch (ClassCastException e) {
				result[i] = null;
			}
		}

		return result;
	}

	void onTreeSelect(TreeSelectionEvent ev) {
		if (getSelectedAddableCourses()[0] != null) {
			courseInfo
					.setComponentOrientation(ComponentOrientation.RIGHT_TO_LEFT);
			courseInfo.setText(getSelectedAddableCourses()[0].getHtmlInfo());
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
