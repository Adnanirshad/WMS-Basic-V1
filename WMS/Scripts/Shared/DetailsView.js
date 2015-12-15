/// <reference path="../../yui3/build/yui/yui-min.js" />

YUI.add('details-view', function (Y)
{
    function DetailsView(config)
    {
        DetailsView.superclass.constructor.apply(this, arguments);
    }

    DetailsView.NAME = 'DetailsView';

    Y.extend(DetailsView, Y.Widget,
        {
            bindUI: function ()
            {
                Y.on('click', Y.bind(this._onDeleteClick, this), '.delete');
            },
            _onDeleteClick: function(e)
            {
                e.preventDefault();

                if(confirm('Are you sure you want to delete this item?'))
                    e.target.ancestor('form').submit();
            }
        });

    Y.DetailsView = Y.mix(DetailsView, Y.DetailsView);

}, '1.0.0', {
    requires: ['node', 'widget']
});