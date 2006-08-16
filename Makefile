CSC=mcs
DEBUGFLAGS= -debug -d:DEBUG
CSCFLAGS=-codepage:utf8 -pkg:glade-sharp-2.0 -pkg:gtk-sharp-2.0 -pkg:gtkhtml-sharp-2.0 $(DEBUGFLAGS)

UDONKEY_EXE=UDonkey.exe
UDONKEY_DLL=UDonkey-Logic.dll
UDONKEY_PDB=UDonkey.exe
UDONKEY_DLL_SRC=./UDonkeyEnums.cs \
	./UDonkeyInterfaces.cs \
	./DB/CourseDB.cs \
	./DB/DBSerialBuilder.cs \
	./IO/IOManager.cs \
	./Logic/Configuration.cs \
	./Logic/Constraints/AbstractConstraint.cs \
	./Logic/Constraints/FreeDaysConstraint.cs \
	./Logic/Constraints/MaximalStudyHoursConstraint.cs \
	./Logic/Constraints/MinimalStudyHoursConstraint.cs \
	./Logic/Constraints/ScheduleObjectConstraint.cs \
	./Logic/Constraints/UsersEventConstraint.cs \
	./Logic/CourseScheduler/AbstractCourseIterator.cs \
	./Logic/CourseScheduler/Course.cs \
	./Logic/CourseScheduler/CourseEvent.cs \
	./Logic/CourseScheduler/CourseEventOccurrence.cs \
	./Logic/CourseScheduler/CourseIteratorComparer.cs \
	./Logic/CourseScheduler/CoursesScheduler.cs \
	./Logic/CourseScheduler/MixedGroupIterator.cs \
	./Logic/CourseScheduler/RegistrationGroup.cs \
	./Logic/CourseScheduler/RegistrationGroupIterator.cs \
	./Logic/CourseScheduler/SchedulerState.cs \
	./Logic/CourseScheduler/SchedulerStateComparer.cs \
	./Logic/Lib/AbstractConstraintCollection.cs \
	./Logic/Lib/AbstractCourseIteratorsCollection.cs \
	./Logic/Lib/AbstractCourseIteratorsList.cs \
	./Logic/Lib/AbstractScheduleStatisticList.cs \
	./Logic/Lib/CourseEventCollection.cs \
	./Logic/Lib/CourseEventOccurrenceCollection.cs \
	./Logic/Lib/CourseEventsList.cs \
	./Logic/Lib/CourseIDCollection.cs \
	./Logic/Lib/CoursesCollection.cs \
	./Logic/Lib/CoursesList.cs \
	./Logic/Lib/RegistrationGroupsCollection.cs \
	./Logic/Lib/ScheduleDayDataCollection.cs \
	./Logic/Lib/ScheduleObjectsList.cs \
	./Logic/Lib/SchedulerStatesCollection.cs \
	./Logic/Lib/SpecializedArrayList.cs \
	./Logic/Lib/SpecializedSortedList.cs \
	./Logic/Schedule/AbstractScheduleObject.cs \
	./Logic/Schedule/AbstractScheduleStatistic.cs \
	./Logic/Schedule/CourseEventOccurrencesScheduleObject.cs \
	./Logic/Schedule/EndHourStatistic.cs \
	./Logic/Schedule/FreeDaysStatistic.cs \
	./Logic/Schedule/HolesStatistic.cs \
	./Logic/Schedule/HourColumnScheduleEntry.cs \
	./Logic/Schedule/ImportedScheduleObject.cs \
	./Logic/Schedule/Schedule.cs \
	./Logic/Schedule/ScheduleDayData.cs \
	./Logic/Schedule/ScheduleException.cs \
	./Logic/Schedule/ScheduleObjectsBucket.cs \
	./Logic/Schedule/StartHourStatistic.cs \
	./Logic/Schedule/UsersEventScheduleObject.cs \
	./Logic/LoadDBFormLogic.cs \
	./RepFile/DosHeb.cs \
	./RepFile/RepToXML.cs \
	./Logic/AssemblyInfo.cs

UDONKEY_GUI_SHARED=./GUI/CustomEvents.cs \
	./GUI/ConfigurationController.cs \
	./GUI/DBLogic.cs \
	./GUI/ScheduleGridLogic.cs \
	./GUI/UDonkeyClass.cs \
	./GUI/MainFormLogic.cs \
	./GUI/Winforms/Resources.cs \
	./GUI/TabPageContainer.cs \
	./AssemblyInfo.cs 
	

UDONKEY_WINFORMS=./GUI/Winforms/AboutForm.cs \
	./GUI/Winforms/ConfigControl.cs \
	./GUI/Winforms/DBbrowser.cs \
	./GUI/Winforms/LoadDBForm.cs \
	./GUI/Winforms/MainForm.cs \
	./GUI/Winforms/RepFileConvertForm.cs \
	./GUI/Winforms/ScheduleDataGrid.cs \
	./GUI/Winforms/ScheduleDataGridColumnStyle.cs \
	./GUI/Winforms/ScheduleMenuItem.cs \
	./GUI/Winforms/SchedulingProgressbar.cs \
	./GUI/Winforms/SearchControl.cs \
	./GUI/Winforms/UDonkeyForm.cs \
	./GUI/Winforms/UsersEventForm.cs 

UDONKEY_WINFORMS_RESX=./GUI/Winforms/AboutForm.resx \
	./GUI/Winforms/ScheduleDataGridColumnStyle.resx \
	./GUI/Winforms/ConfigControl.resx \
	./GUI/Winforms/ScheduleDataGrid.resx \
	./GUI/Winforms/ScheduleMenuItem.resx \
	./GUI/Winforms/SchedulingProgressbar.resx \
	./GUI/Winforms/LoadDBForm.resx \
	./GUI/Winforms/SearchControl.resx \
	./GUI/Winforms/MainForm.resx \
	./GUI/Winforms/RepFileConvertForm.resx \
	./GUI/Winforms/UsersEventForm.resx \
	./GUI/Winforms/Resources.resx  
#	./GUI/Winforms/UDonkeyForm.resx \
#	./GUI/Winforms/DBbrowser.resx 			# crashes on linux
#	./GUI/Winforms/DBEditor.resx			# doesn't seem to be used 

UDONKEY_WINFORMS_RESOURCES:=$(patsubst %.resx, %.resources, $(UDONKEY_WINFORMS_RESX))

UDONKEY_GTK = ./GUI/Gtk/AboutForm.cs \
	./GUI/Gtk/DBbrowser.cs \
	./GUI/Gtk/MainForm.cs \
	./GUI/Gtk/RepFileConvertForm.cs \
	./GUI/Gtk/ScheduleDataGrid.cs \
	./GUI/Winforms/ScheduleMenuItem.cs \
	./GUI/Winforms/ScheduleDataGridColumnStyle.cs \
	./GUI/Winforms/UsersEventForm.cs \
	./GUI/Gtk/SchedulingProgressbar.cs \
	./GUI/Gtk/SearchControl.cs \
	./GUI/Gtk/UDonkeyForm.cs \
	./GUI/Gtk/UsersEventForm.cs \
	./GUI/Gtk/LoadDBForm.cs \
	./GUI/Gtk/CommonDialogs.cs \
	./GUI/Gtk/ConfigControl.cs 

UDONKEY_GTK_RESOURCSE=./GUI/lecture.bmp \
	./GUI/lab.bmp	\
	./GUI/project.bmp 	\
	./GUI/tutorial.bmp	\
	./GUI/Gtk/udonkey.glade

# common targets
all: UDonkey UDonkey-win dll

%.resources: %.resx
	resgen /compile $<

win/$(UDONKEY_EXE): $(UDONKEY_GUI_SHARED) $(UDONKEY_WINFORMS) $(UDONKEY_DLL) $(UDONKEY_WINFORMS_RESOURCES) 
	echo Compiling Windows version
	echo *************************
#	resgen /compile $(UDONKEY_RES)
	$(CSC) $(CSCFLAGS) /r:System.dll /r:System.Windows.Forms.dll /r:System.Xml.dll /r:System.Drawing.dll /r:System.Data.dll /r:ICSharpCode.SharpZipLib.dll /r:System.Web.dll /target:winexe /out:win/$(UDONKEY_EXE) -r:$(UDONKEY_DLL) $(UDONKEY_GUI_SHARED) $(UDONKEY_WINFORMS) $(patsubst %, -resource:%, $(UDONKEY_WINFORMS_RESOURCES))

$(UDONKEY_EXE): $(UDONKEY_DLL) $(UDONKEY_GTK) $(UDONKEY_GUI_SHARED) $(UDONKEY_GTK_RESOURCSE) ./GUI/Winforms/Resources.resources
	echo Compiling GTK version
	echo *********************
#	resgen /compile $(UDONKEY_RES)
	$(CSC) $(CSCFLAGS) /r:System.dll /r:System.Windows.Forms.dll /r:System.Xml.dll /r:System.Drawing.dll /r:System.Data.dll /r:ICSharpCode.SharpZipLib.dll /r:System.Web.dll /target:winexe /out:$(UDONKEY_EXE) -lib:. -r:$(UDONKEY_DLL) $(UDONKEY_GUI_SHARED) $(UDONKEY_GTK) $(patsubst %, -resource:%, $(UDONKEY_GTK_RESOURCSE)) -resource:GUI/Winforms/Resources.resources,UDonkey.GUI.Resources.resources

$(UDONKEY_DLL): $(UDONKEY_DLL_SRC)
	echo Compiling DLL
	echo *************
	$(CSC) $(CSCFLAGS) -r:System.dll -r:System.Windows.Forms.dll -r:System.Xml.dll -r:System.Drawing.dll -r:System.Data.dll -r:ICSharpCode.SharpZipLib.dll -r:System.Web.dll /target:library /out:$@ $(UDONKEY_DLL_SRC)

clean:
	-rm -f "$(UDONKEY_EXE)" 2> /dev/null
	-rm -f "$(UDONKEY_PDB)" 2> /dev/null
	-rm -f "win/$(UDONKEY_EXE)" 2> /dev/null
	-rm -f "win/$(UDONKEY_PDB)" 2> /dev/null
	-rm -f "$(UDONKEY_DLL)" 2> /dev/null


# project names as targets

UDonkey gtk: $(UDONKEY_EXE)
UDonkey-win win: win/$(UDONKEY_EXE)
dll: $(UDONKEY_DLL)

tags: $(UDONKEY_DLL_SRC) $(UDONKEY_GTK) $(UDONKEY_WINFORMS) $(UDONKEY_GUI_SHARED)
	ctags $^
