function deleteFromFilters(itself)
{
  
    var d = "ContentPlaceHolder1_GridView" + itself.parentNode.id;
    console.log(d);
    var table = document.getElementById(d);
    
    $.ajax({
        type: "POST",
        url: "StepOneFilter.aspx/DeleteSingleFilter",
        data: "{'id':'" + itself.id + "','parentid':'" + itself.parentNode.id + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var d = document.getElementById(itself.parentNode.id + "Span");
            var h =  parseInt(d.innerHTML);
            h = h - 1;
            d.innerHTML = h+"";
            
            //           console.log(itself.parents().eq(3));
             if (table != null)
                 for (var i = 1, row; row = table.rows[i]; i++) {

                     //iterate through rows
                     //rows would be accessed using the "row" variable assigned in the for loop
                     for (var j = 0, col; col = row.cells[j]; j++) {
                         if ((j+2) % 3 == 0) {
                             var td = row.cells[j].getElementsByTagName('input');
                             for (var y = 0; y < td.length; y++) {
                                 if (row.cells[j -1].innerHTML == itself.id) {
                                     td[y].checked = false;
                                     itself.parentNode.remove();
                                 }
                             }

                         }

                     }
                 }
         else
             itself.parentNode.remove();
        }
    });
  
    itself.parentNode.remove();

  


}