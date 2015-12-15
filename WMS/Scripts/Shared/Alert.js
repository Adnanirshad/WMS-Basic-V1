/// <reference path="../yui3/build/yui/yui-min.js" />

YUI.add('alert', function (Y)
{
    var template = Y.Template.Micro.compile(
        '<div class="alert <%= this.alertClass %>" style="text-align:center;" role="alert"><%= this.message %></div>'
    );

    Y.Alert =
        {
            Types:
                {
                    Error: function() { return 'alert-danger'; },
                    Success: function() { return 'alert-success'; }
                },
            Show: function(alertType, message)
            {
                if(message === null)
                    throw 'Message must be provided';

                if(alertType === null)
                    alertType = Y.Alert.Types.Error();

                Y.one('#container').prepend(template({ alertClass: alertType, message: message }));
            }
        };

}, '1.0', { requires: ['node', 'template-micro'] });