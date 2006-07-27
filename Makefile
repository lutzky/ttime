CSC=mcs
CSCFLAGS=-codepage:utf8

UDONKEY_EXE=$(TARGET)/UDonkey.exe
UDONKEY_PDB=$(TARGET)/UDonkey.exe
UDONKEY_SRC=./AssemblyInfo.cs \
	./UDonkeyEnums.cs \
	./UDonkeyInterfaces.cs \
	./DB/CourseDB.cs \
	./DB/DBSerialBuilder.cs \
	./GUI/AboutForm.cs \
	./GUI/ColorMixer.cs \
	./GUI/ConfigControl.cs \
	./GUI/DBbrowser.cs \
	./GUI/DBEditor.cs \
	./GUI/LoadDBForm.cs \
	./GUI/MainForm.cs \
	./GUI/RepFileConvertForm.cs \
	./GUI/Resources.cs \
	./GUI/ScheduleDataGrid.cs \
	./GUI/ScheduleDataGridColumnStyle.cs \
	./GUI/ScheduleMenuItem.cs \
	./GUI/SchedulingProgressbar.cs \
	./GUI/SearchControl.cs \
	./GUI/UDonkeyForm.cs \
	./GUI/UsersEventForm.cs \
	./IO/IOManager.cs \
	./Logic/Configuration.cs \
	./GUI/ConfigurationController.cs \
	./GUI/DBLogic.cs \
	./GUI/MainFormLogic.cs \
	./GUI/ScheduleGridLogic.cs \
	./GUI/UDonkeyClass.cs \
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
	./RepFile/DosHeb.cs \
	./RepFile/RepToXML.cs

UDONKEY_RES=./GUI/AboutForm.resx \
	./GUI/ScheduleDataGridColumnStyle.resx \
	./GUI/ConfigControl.resx \
	./GUI/ScheduleDataGrid.resx \
	./GUI/DBbrowser.resx \
	./GUI/ScheduleMenuItem.resx \
	./GUI/DBEditor.resx \
	./GUI/SchedulingProgressbar.resx \
	./GUI/LoadDBForm.resx \
	./GUI/SearchControl.resx \
	./GUI/MainForm.resx \
	./GUI/UDonkeyForm.resx \
	./GUI/RepFileConvertForm.resx \
	./GUI/UsersEventForm.resx \
	./GUI/Resources.resx

UDONKEY_RESOURCES=$(patsubst %.resx, %.resources, $UDONKEY_RES)

$(UDONKEY_EXE): $(UDONKEY_SRC) 
#	resgen /compile $(UDONKEY_RES)
	$(CSC) $(CSCFLAGS) /r:System.dll /r:System.Windows.Forms.dll /r:System.Xml.dll /r:System.Drawing.dll /r:System.Data.dll /r:ICSharpCode.SharpZipLib.dll /r:System.Web.dll /target:winexe /out:$(UDONKEY_EXE) $(UDONKEY_SRC) # $(UDONKEY_RESOURCES)


# common targets

all:	$(UDONKEY_EXE)

clean:
	-rm -f "$(UDONKEY_EXE)" 2> /dev/null
	-rm -f "$(UDONKEY_PDB)" 2> /dev/null


# project names as targets

UDonkey: $(UDONKEY_EXE)
