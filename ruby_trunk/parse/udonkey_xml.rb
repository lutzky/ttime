require 'logic/faculty'
require 'xml/libxml'

# FIXME this shouldn't come from here

module TTime
  module Parse
    class UDonkeyXML
      COURSE_FIELDS = {
        "name" => "name",
        "number" => "number",
        "academic_points" => "courseAcademicPoints",
        # FIXME There's also tutorialHours
        "hours" => "lectureHours",
        "lecturer_in_charge" => "lecturerInCharge",
        "first_test_date" => "moedADate",
        "second_test_date" => "moedBDate",
      }
      GROUP_TYPES = {
        "הרצאה" => :lecture,
        "תרגיל" => :tutorial,
        "מעבדה" => :lab,
      }
      HEBREW_DAYS = [ nil, "א", "ב", "ג", "ד", "ה", "ו", "ש" ]

      class << self
        def convert_udonkey_xml(xml_file)
          doc = XML::Document.file xml_file.to_s
          faculties = []
          doc.find("/CourseDB/Faculty").each do |xml_faculty|
            faculty = TTime::Logic::Faculty.new xml_faculty.property("name")
            xml_faculty.find("Course").each do |xml_course|
              faculty.courses << convert_xml_course(xml_course)
            end
            faculties << faculty
          end
          faculties
        end

        private
        def convert_xml_course xml_course
          course = TTime::Logic::Course.new
          COURSE_FIELDS.each do |key, value|
            course.send("#{key}=", xml_course.property(value))
          end
          course.groups = xml_course.find("CourseEvent").collect do |xml_course_event|
            convert_xml_course_event xml_course_event, course
          end
          return course
        end

        def convert_xml_course_event xml_course_event, course
          group = TTime::Logic::Group.new
          group.number = xml_course_event.property("regNumber").to_i
          group.type = GROUP_TYPES[xml_course_event.property("eventType")]
          if group.type.nil?
            # For sports courses, the eventType is actually a description
            group.type = :other
            group.description = xml_course_event.property("eventType")
          end
          group.lecturer = xml_course_event.property("teacher")
          group.course = course
          group.events = xml_course_event.find("PlaceTime").collect do |xml_placetime|
            convert_xml_placetime xml_placetime, group
          end.reject { |e| e.nil? }
          group.events.each do |e|
            if e.class != TTime::Logic::Event
              exit 1
            end
          end
          return group
        end

        def convert_xml_placetime xml_placetime, group
          event = TTime::Logic::Event.new group
          event.day = HEBREW_DAYS.index xml_placetime.property("EventDay")
          event.start = xml_placetime.property("EventTime").gsub(".","").to_i
          event.end = event.start + \
            100 * xml_placetime.property("EventDuration").to_i
          event.place = xml_placetime.property("EventLocation")

          # Some events are malformed - for example, have invalid days. We'll
          # return nil as them, and reject nil events outside.
          return nil if event.day.nil?
          return event
        end
      end
    end
  end
end
