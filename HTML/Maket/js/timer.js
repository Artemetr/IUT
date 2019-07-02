var sec = 0;
function timer() {
    sec++;
    var h = (sec - (sec%3600))/3600;
    if (h < 10) h = '0' + h.toString();
    var m = ((sec - h*3600) - (sec - h*3600)%60)/60;
    if (m < 10) m = '0' + m.toString();
    var s = (sec - h*3600 - m*60);
    if (s < 10) s = '0' + s.toString();
    document.getElementById('timer').innerHTML = h + ':' + m + ':' + s ;
    setTimeout('timer()', 1000);
}