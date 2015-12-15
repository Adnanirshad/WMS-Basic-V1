/// <reference path="../../yui3/build/yui/yui-min.js" />

YUI.add('update-view', function (Y)
{
    function UpdateView(config)
    {
        UpdateView.superclass.constructor.apply(this, arguments);
    }

    UpdateView.NAME = 'updateview';

    Y.extend(UpdateView, Y.Widget,
        {
            bindUI: function ()
            {
                Y.on('click', Y.bind(this._onSaveClick, this), '#saveButton');
            },
            _onSaveClick: function(e)
            {
                Y.one('.pure-form').submit();
            }
        });

    Y.UpdateView = Y.mix(UpdateView, Y.UpdateView);

}, '1.0.0', {
    requires: ['node', 'widget']
});