# encoding: utf-8

require './lib/ttime/parse/repy'
require 'test/unit'

TTimeDays = {
  :sunday => 1,
  :monday => 2,
  :tuesday => 3,
  :wednesday => 4,
  :thursday => 5,
  :friday => 6,
  :saturday => 7
}

TTimeSunday = 1


class TestParse < Test::Unit::TestCase
  @@simple_data = <<-eos
+==========================================+
|       תיתביבסו תיחרזא הסדנה - תועש תכרעמ |
|            ד"עשת ףרוח רטסמס              |
+==========================================+
+------------------------------------------+
|                        הקיטסיטטס  014003 |
|3.0 :קנ          2-ת 2-ה:עובשב הארוה תועש |
+------------------------------------------+
|           ןייבשיפ.ב     13 : יארחא  הרומ |
|                              ----------- |
|             03/02/14 'ב  םוי: ןושאר דעומ |
|                              ----------- |
|             09/03/14 'א  םוי:   ינש דעומ |
|                              ----------- |
|                       םימ יבאשמל דעוימ.1 |
|               ++++++                  .סמ|
|                                     םושיר|
|                14.30-16.30'ג :האצרה      |
|             ןייבשיפ.ב מ/פורפ : הצרמ      |
|                               -----      |
|                                          |
|                10.30-12.30'ד :ליגרת  11  |
|                                          |
|                10.30-12.30'ב :ליגרת  12  |
|                                          |
|                14.30-16.30'א :ליגרת  13  |
|                                          |
|                10.30-12.30'ד :ליגרת  14  |
|                                          |
|                16.30-18.30'ב :ליגרת  15  |
+------------------------------------------+
  eos

  def test_simple
    repy = TTime::Parse::Repy.new(@@simple_data.encode(Encoding::IBM862))
    hash = repy.hash

    faculty = hash[0]

    assert_equal "הנדסה אזרחית וסביבתית", faculty.name

    course = faculty.courses[0]

    assert_equal "014003", course.number
    assert_equal "סטטיסטיקה", course.name
    assert_equal 3.0, course.academic_points
    assert_equal "31 ב.פישביין", course.lecturer_in_charge
    assert_equal Date.new(2014,2,3), course.first_test_date
    assert course.first_test_date.monday?
    assert_equal Date.new(2014,3,9), course.second_test_date
    assert course.second_test_date.sunday?

    assert_equal :lecture,            course.groups[0].type
    assert_equal "פרופ/מ ב.פישביין",  course.groups[0].lecturer
    assert_equal TTimeDays[:tuesday], course.groups[0].events[0].day
    assert_equal 1430,               course.groups[0].events[0].start
    assert_equal 1630,               course.groups[0].events[0].end

    assert_equal :tutorial,             course.groups[1].type
    assert_equal 11,                    course.groups[1].number
    assert_equal TTimeDays[:wednesday], course.groups[1].events[0].day
    assert_equal 1030,                  course.groups[1].events[0].start
    assert_equal 1230,                  course.groups[1].events[0].end

    assert_equal :tutorial,             course.groups[2].type
    assert_equal 12,                    course.groups[2].number
    assert_equal TTimeDays[:monday],    course.groups[2].events[0].day
    assert_equal 1030,                  course.groups[2].events[0].start
    assert_equal 1230,                  course.groups[2].events[0].end

    assert_equal :tutorial,             course.groups[3].type
    assert_equal 13,                    course.groups[3].number
    assert_equal TTimeDays[:sunday],    course.groups[3].events[0].day
    assert_equal 1430,                  course.groups[3].events[0].start
    assert_equal 1630,                  course.groups[3].events[0].end

    assert_equal :tutorial,             course.groups[4].type
    assert_equal 14,                    course.groups[4].number
    assert_equal TTimeDays[:wednesday], course.groups[4].events[0].day
    assert_equal 1030,                  course.groups[4].events[0].start
    assert_equal 1230,                  course.groups[4].events[0].end

    assert_equal :tutorial,             course.groups[5].type
    assert_equal 15,                    course.groups[5].number
    assert_equal TTimeDays[:monday],    course.groups[5].events[0].day
    assert_equal 1630,                  course.groups[5].events[0].start
    assert_equal 1830,                  course.groups[5].events[0].end
  end
end
