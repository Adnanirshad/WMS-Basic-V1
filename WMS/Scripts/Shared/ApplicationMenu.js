/// <reference path="../yui3/build/yui/yui-min.js" />

YUI.add('application-menu', function (Y)
{
    Y.ApplicationMenu = (function()
    {
        var _globalMenu = new Y.Menu({
            container: '#nav-menu',
            sourceNode: '#nav-menu-root',
            orientation: 'horizontal',
            hideOnOutsideClick: false,
            hideOnClick: false
        }),
        _contextMenu = null;

        if(Y.one('#contextMenu') !== null)
        {
            _contextMenu = new Y.Menu({
                container: '#contextMenu',
                sourceNode: '#contextMenuData',
                orientation: 'horizontal',
                hideOnOutsideClick: false,
                hideOnClick: false
            });
        }

        return {
            _handleMenuLinkClick: function (e)
            {
                var nav = Y.one('#globalContext');

                if (nav.getStyle('display') === 'none')
                    nav.setStyle('display', 'block');
                else
                    nav.setStyle('display', 'none');
            },

            _getGlobalMenu: function ()
            {
                return _globalMenu;
            },

            _getContextMenu: function()
            {
                return _contextMenu;
            },

            Initialize: function ()
            {
                Y.one('#menuLink').on('click', function (e)
                {
                    Y.ApplicationMenu._handleMenuLinkClick(e);
                });

                _globalMenu.render().show();

                if(_contextMenu !== null)
                    _contextMenu.render().show();
            }
        };
    })();

}, '1.0', { requires: ['gallery-sm-menu', 'node'] });