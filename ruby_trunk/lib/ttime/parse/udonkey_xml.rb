require 'logic/faculty'
require 'xml/libxml'

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
        nil     => :other,
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

        TIME_FMT="%d/%m/%y" # FIXME Y2K much? But this is what UDonkey uses...

        def UDonkeyXML.output_xml faculties
          doc = XML::Document.new
          doc.encoding = 'UTF-8'
          doc.root = XML::Node.new('CourseDB')
          faculties.each do |faculty|
            faculty_node = XML::Node.new('Faculty')
            doc.root << faculty_node
            faculty_node['name'] = faculty.name
            # FIXME faculty_node['semester'] = ?

            faculty.courses.each do |course|
              course_node = XML::Node.new('Course')
              faculty_node << course_node

              moedADate = nil
              if course.first_test_date
                moedADate = course.first_test_date.strftime(TIME_FMT)
              end

              moedBDate = nil
              if course.second_test_date
                moedBDate = course.second_test_date.strftime(TIME_FMT)
              end

              course_hash = {
                'name' => course.name,
                'number' => course.number,
                'courseAcademicPoints' => course.academic_points.to_s,
                # FIXME lectureHours, tutorialHours
                'lecturerInCharge' => course.lecturer_in_charge,
                'moedADate' => moedADate,
                'moedBDate' => moedBDate,
              }

              course_hash.each do |key, val|
                course_node[key] = val.gsub(%["],%[']) if val
              end

              course.groups.each do |group|
                group_node = XML::Node.new('CourseEvent')
                course_node << group_node

                group_hash = {
                  'regNumber' => group.number.to_s,
                  'eventType' => GROUP_TYPES.invert[group.type] || \
                                   group.description,
                  'teacher' => group.lecturer
                }

                group_hash.each do |key, val|
                  group_node[key] = val.gsub(%["],%[']) if val
                end

                group.events.each do |event|
                  event_node = XML::Node.new('PlaceTime')
                  group_node << event_node

                  event_time = "#{event.start / 100}.#{event.start % 100}"
                  event_duration = (event.end - event.start) / 100

                  event_hash = {
                    'EventDay' => HEBREW_DAYS[event.day],
                    'EventTime' => event_time.to_s,
                    'EventDuration' => event_duration.to_s,
                    'EventLocation' => event.place
                  }

                  event_hash.each do |key, val|
                    event_node[key] = val.gsub(%["],%[']) if val
                  end
                end
              end
            end
          end
          print doc.to_s
        end
      end
    end
  end
end
