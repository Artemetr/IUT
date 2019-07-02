$(function () {
   $('#loadIMG').click(function () {
       $('#picture_form').fadeIn(500, function () {
           console.log('aaaa');
           $('#picture').css('opacity', '1');
       })
   });
});