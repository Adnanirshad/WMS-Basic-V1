$(document).ready(function () {
    $('#location').css('display', 'none');
    $('#CriteriaID').change(function () {
        console.log($("#CriteriaID option:selected").html());
        switch ($("#CriteriaID option:selected").html()) {
            case 'Location': $('#location').css('display', 'inline');
                             $('#category').css('display', 'none');
                             $('#company').css('display', 'none');
                             break;
            case 'Category': $('#location').css('display', 'none');
                $('#category').css('display', 'inline');
                             $('#company').css('display', 'none');
                             break;
            case 'Company': $('#location').css('display', 'none');
                $('#category').css('display', 'none');
                $('#company').css('display', 'inline');
                break;
        }
    });

    $('#ProcessCats').change(function () {
        switch ($("#ProcessCats option:selected").html()) {
            case 'Yes':
                $('#CategoryID').css('display', 'inline');
                break;
            case 'No': $('#CategoryID').css('display', 'none');
                break;
        }
    });

});