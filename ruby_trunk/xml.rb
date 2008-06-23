require 'rexml/document'
require 'logic/faculty'

# FIXME this shouldn't come from here
XML_FILENAME = 'data/MainDB.xml'

module TTime
  class XMLData
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
      "תרגיל" => :tutorial
    }
    HEBREW_DAYS = [ nil, "א", "ב", "ג", "ד", "ה", "ו", "ש" ]

    class << self
      def convert_udonkey_xml(xml_file = open(XML_FILENAME))
        doc = REXML::Document.new xml_file
        faculties = doc.root.elements.collect do |xml_faculty|
          faculty = TTime::Logic::Faculty.new xml_faculty.attributes["name"]
          faculty.courses += xml_faculty.elements.collect do |xml_course|
            convert_xml_course xml_course
          end
          faculty
        end
      end

      private
      def convert_xml_course xml_course
        course = TTime::Logic::Course.new
        COURSE_FIELDS.each do |key, value|
          course.send("#{key}=", xml_course.attributes[value])
        end
        course.groups = xml_course.elements.collect do |xml_course_event|
          convert_xml_course_event xml_course_event, course
        end
        return course
      end

      def convert_xml_course_event xml_course_event, course
        group = TTime::Logic::Group.new
        group.number = xml_course_event.attributes["regNumber"].to_i
        group.type = GROUP_TYPES[xml_course_event.attributes["eventType"]]
        group.lecturer = xml_course_event.attributes["teacher"]
        group.course = course
        group.events = xml_course_event.elements.collect do |xml_placetime|
          convert_xml_placetime xml_placetime, group
        end.reject { |e| e.nil? }
        return group
      end

      def convert_xml_placetime xml_placetime, group
        event = TTime::Logic::Event.new nil, group
        event.day = HEBREW_DAYS.index xml_placetime.attributes["EventDay"]
        event.start = xml_placetime.attributes["EventTime"].gsub(".","").to_i
        event.end = event.start + \
          100 * xml_placetime.attributes["EventDuration"].to_i
        event.place = xml_placetime.attributes["EventLocation"]

        # Some events are malformed - for example, have invalid days. We'll
        # return nil as them, and reject nil events outside.
        return nil if event.day.nil?
        return event
      end
    end
  end
end
