#!/usr/bin/perl

print header();

print "<div align=center class='menu'>no menu yet...</div>\n";

print "<table border=1 rules=all>\n";
print " <tr>\n";
foreach $i (("חמישי","רביעי","שלישי","שני","ראשון","")){
  print "   <td class=day>".rtl($i)."</td>\n";
}
print " </tr>\n";


for($i=8;$i<=20;$i++){
  print " <tr>\n";
  for($j=0;$j<5;$j++){
    print "   <td><div id='".(5-$j)."-$i:30' >\n";
    print "   </div></td>";
  }
  print "<td class=hour>$i:30</td>";
  print "</tr>";
}

print "</table>";


print footer();


###############################################################################
###############################################################################
###############################################################################
###############################################################################
###############################################################################
##########                                                        #############
##########           end of printing code. begin subs             #############
##########                                                        #############
###############################################################################
###############################################################################
###############################################################################
###############################################################################
###############################################################################

sub header {
 
"<html>
  <head>
  <meta http-equiv='Content-Type' content='text/html;charset=utf-8' >
" . css() ."
" . js()  ."
  </head>
  <body onLoad='initEvent()'>
  <div align=center>
  "
}


sub css {
"   <style type='text/css'>
td {width: 16%; height: 7% }
div.menu {width: 98%; height:8%; border: #000000 1px solid; color: #eeeeff; margin: 5px}
table {width: 98%; height: 90%}
td.day {background: #cccccc}
td.hour {background: #eeeeee}
div.event {width: 99%; border:#000000 1px solid; margin: 1px}
    </style>"

}

sub footer {
"
</div>
</body>
</html>"
}

sub js {
  <<EOF
  <script language=javascript>

  var colorIndex=new Array('#EADEFF','#E0FFDE','#FAFFDE','#FEFFDE','#FFF9DE','#FFE3DE','#FFDEED','#F9DEFF','#EEE5D5','#E2EED5','#EED7D5');



  // time for a javascript refresher :)
  function addEvent(course_index,day,start,end,desc)
  {
    var i;
    for(i=start;i<=end;i++){
      var eventStr = '';
      eventStr = '<div class="event" style="background-color:'+colorIndex[course_index]+'"> ' + 
      desc + '</div>';
      target = document.getElementById("1-8:30");
      target.innerHTML = eventStr;
//    target.innerHTML = "test";
 

    }
  }
function initEvent(){
//TEMPLATE//
}

  </script>
EOF

}

sub rtl {
  "<div align=center>".pop()."</div>"
}
