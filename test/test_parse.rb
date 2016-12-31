# encoding: utf-8

require 'coveralls'
Coveralls.wear!

require 'ttime/parse/repy'
require 'test/unit'
require 'ttime/encoding'

TTimeDays = {
  :sunday => 1,
  :monday => 2,
  :tuesday => 3,
  :wednesday => 4,
  :thursday => 5,
  :friday => 6,
  :saturday => 7
}

# For use with Date#wday
WDays = {
  :sunday => 0,
  :monday => 1,
  :tuesday => 2,
  :wednesday => 3,
  :thursday => 4,
  :friday => 5,
  :saturday => 6
}

class TestParse < Test::Unit::TestCase
  def load_hash_from unicode_string
    encoded = unicode_string.encode(Encoding::IBM862)
    repy = TTime::Parse::Repy.new(encoded)
    repy.hash
  end

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
    hash = load_hash_from(@@simple_data)

    faculty = hash[0]

    assert_equal "הנדסה אזרחית וסביבתית", faculty.name

    course = faculty.courses[0]

    assert_equal "014003", course.number
    assert_equal "סטטיסטיקה", course.name
    assert_equal 3.0, course.academic_points
    assert_equal "31 ב.פישביין", course.lecturer_in_charge

    assert_equal Date.new(2014,2,3),  course.first_test_date
    assert_equal WDays[:monday],      course.first_test_date.wday
    assert_equal Date.new(2014,3,9),  course.second_test_date
    assert_equal WDays[:sunday],      course.second_test_date.wday

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

  # Side-note: Check out course 014841, group 91... isn't that strange? Doesn't
  # fit in with the rest of the course.
  @@data_with_labs = <<-eos
+==========================================+
|       תיתביבסו תיחרזא הסדנה - תועש תכרעמ |
|            ד"עשת ףרוח רטסמס              |
+==========================================+
+------------------------------------------+
|          1 הדידמהו יופימה תודוסי  014841 |
|3.5 :קנ      2-מ 2-ת 2-ה:עובשב הארוה תועש |
+------------------------------------------+
|             תוילד.ש    ר"ד : יארחא  הרומ |
|                              ----------- |
|             18/02/14 'ג  םוי: ןושאר דעומ |
|                              ----------- |
|             25/03/14 'ג  םוי:   ינש דעומ |
|                              ----------- |
|        לולסממ םיטנדוטסל דעוימ אל סרוקה.1 |
|           היצמרופניא-ואיגו יופימ תסדנה   |
|      7 המוק ,הזילע לצא ליגרתל םשרהל שי.2 |
|               ++++++                  .סמ|
|                                     םושיר|
|                14.30-16.30'ב :האצרה      |
|               תוילד.ש    ר"ד : הצרמ      |
|                               -----      |
|                                          |
|                16.30-18.30'ב :ליגרת  11  |
|                12.30-14.30'ג :הדבעמ      |
|                                          |
|                16.30-18.30'ב :ליגרת  12  |
|                12.30-14.30'ג :הדבעמ      |
|                                          |
|                16.30-18.30'ב :ליגרת  13  |
|                 9.30-11.30'א :הדבעמ      |
|                                          |
|                 8.30-10.30'ג :ליגרת  14  |
|                 9.30-11.30'א :הדבעמ      |
|                                          |
|                 8.30-10.30'ג :ליגרת  15  |
|                10.30-12.30'ב :הדבעמ      |
|                                          |
|                 8.30-10.30'ג :ליגרת  16  |
|                10.30-12.30'ב :הדבעמ      |
|                                          |
|                14.30-16.30'ה :ליגרת  17  |
|                12.30-14.30'ב :הדבעמ      |
|                                          |
|                14.30-16.30'ה :ליגרת  18  |
|                12.30-14.30'ב :הדבעמ      |
|                                          |
|                14.30-16.30'ה :ליגרת  19  |
|                10.30-12.30'ה :הדבעמ      |
|               ++++++                  .סמ|
|                                     םושיר|
|                     -        :האצרה      |
|                                          |
|                     -        :ליגרת  91  |
|                12.30-14.30'ג :הדבעמ      |
+------------------------------------------+
|                       הקינכמואיג  014409 |
|4.0 :קנ      1-מ 1-ת 3-ה:עובשב הארוה תועש |
+------------------------------------------+
|            קינסלט.מ ח/פורפ : יארחא  הרומ |
|                              ----------- |
|             02/02/14 'א  םוי: ןושאר דעומ |
|                              ----------- |
|             07/03/14 'ו  םוי:   ינש דעומ |
|                              ----------- |
|                                 :ליגרת.1 |
|                 וא_10:30-11:30 ,'ה םוי   |
|                     9:30-10:30 ,'א םוי   |
|               ++++++                  .סמ|
|                                     םושיר|
|                11.30-12.30'ד :האצרה      |
|                 8.30-10.30'ה             |
|              קינסלט.מ ח/פורפ : הצרמ      |
|                               -----      |
|                                          |
|                     -        :ליגרת  11  |
|                10.30-12.30'א :הדבעמ      |
|                                          |
|                     -        :ליגרת  12  |
|                10.30-12.30'א :הדבעמ      |
|                                          |
|                     -        :ליגרת  13  |
|                 8.30-10.30'ג :הדבעמ      |
|                                          |
|                     -        :ליגרת  14  |
|                 8.30-10.30'ג :הדבעמ      |
|                                          |
|                     -        :ליגרת  15  |
|                14.30-16.30'ב :הדבעמ      |
|                                          |
|                     -        :ליגרת  16  |
|                14.30-16.30'ב :הדבעמ      |
|                                          |
|                     -        :ליגרת  17  |
|                12.30-14.30'ה :הדבעמ      |
|                                          |
|                     -        :ליגרת  18  |
|                12.30-14.30'ה :הדבעמ      |
|                                          |
|                     -        :ליגרת  19  |
|                12.30-14.30'ג :הדבעמ      |
|               ++++++                  .סמ|
|                                     םושיר|
|                11.30-12.30'ד :האצרה      |
|                 8.30-10.30'ה             |
|                                          |
|                     -        :ליגרת  21  |
|                12.30-14.30'ג :הדבעמ      |
|                                          |
|                     -        :ליגרת  22  |
|                 8.30-10.30'ד :הדבעמ      |
|                                          |
|                     -        :ליגרת  23  |
|                 8.30-10.30'ד :הדבעמ      |
|                                          |
|                     -        :ליגרת  24  |
|                13.30-15.30'א :הדבעמ      |
+------------------------------------------+
  eos

  def test_with_labs_partial
    hash = load_hash_from @@data_with_labs

    faculty = hash[0]

    course = faculty.courses.find { |c| c.number == "014409" }

    assert_equal 11,                  course.groups[1].number
    assert_equal 1,                   course.groups[1].events.length
    assert_equal 1030,                course.groups[1].events[0].start
    assert_equal 1230,                course.groups[1].events[0].end
    assert_equal TTimeDays[:sunday],  course.groups[1].events[0].day

    assert_equal 13,                  course.groups[3].number
    assert_equal 1,                   course.groups[3].events.length
    assert_equal 830,                 course.groups[3].events[0].start
    assert_equal 1030,                course.groups[3].events[0].end
    assert_equal TTimeDays[:tuesday], course.groups[3].events[0].day
  end

  def test_with_labs_full
    hash = load_hash_from @@data_with_labs

    faculty = hash[0]

    course = faculty.courses.find { |c| c.number == "014841" }

    assert_equal "014841", course.number

    # This is actually problematic, as the group has both a tutorial and a lab.
    # Quite possibly a design bug... CS faculty didn't seem to have these, at
    # least when TTime was originally written.
    assert_equal :tutorial, course.groups[1].type

    assert_equal 11,                  course.groups[1].number
    assert_equal 1630,                course.groups[1].events[0].start
    assert_equal 1830,                course.groups[1].events[0].end
    assert_equal TTimeDays[:monday],  course.groups[1].events[0].day
    assert_equal 11,                  course.groups[1].number
    assert_equal 1230,                course.groups[1].events[1].start
    assert_equal 1430,                course.groups[1].events[1].end
    assert_equal TTimeDays[:tuesday], course.groups[1].events[1].day
    assert_equal 13,                  course.groups[3].number
    assert_equal 930,                 course.groups[3].events[1].start
    assert_equal 1130,                course.groups[3].events[1].end
    assert_equal TTimeDays[:sunday],  course.groups[3].events[1].day
  end
end
