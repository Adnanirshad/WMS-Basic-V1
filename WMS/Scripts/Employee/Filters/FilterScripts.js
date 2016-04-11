function SelectAllCheckboxes(spanChk, gridName) {

    // Added as ASPX uses SPAN for checkbox
    var oItem = spanChk.children;
    var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
    xState = theBox.checked;
    elm = theBox.form.elements;

    for (i = 0; i < elm.length; i++)
        if (elm[i].type == "checkbox" &&
                 elm[i].id != theBox.id) {
            //elm[i].click();
            if (elm[i].checked != xState)
                if (elm[i].id.indexOf(gridName) > -1)
                elm[i].click();
            //elm[i].checked=xState;
        }
}