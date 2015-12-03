$(document).ready(function () {

       $('#CompanyID').change(function () {
           $('#TypeID').empty();
           var convalue = $('#CatID').val();
          //var URL = '/WMS/Emp/EmpTypeList';
            var URL = '/Emp/EmpTypeList';
           $.getJSON(URL + '/' + convalue, function (data) {
               var items;
               $.each(data, function (i, state) {
                   items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                   // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
               });
               $('#TypeID').html(items);
               $('#EmpTypeDivID').show();
           });
    });

});