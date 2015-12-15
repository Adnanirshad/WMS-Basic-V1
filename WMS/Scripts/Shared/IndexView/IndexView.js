/// <reference path="../../yui3/build/yui/yui-min.js" />

YUI.add('index-view', function (Y)
{
    function IndexView(config)
    {
        IndexView.superclass.constructor.apply(this, arguments);
    }

    IndexView.NAME = 'indexview';

    IndexView.ATTRS =
    {
        modelName: { value: null },
        filterOptionsUrl: { value: null },
        refreshTableUrl: { value: null }
    };

    Y.extend(IndexView, Y.Widget,
        {
            initializer: function()
            {
                this._filterKeyValueMap = {};
                this._sortKey = null;
                this._sortDirection = null;
                this._searchQuery = null;
            },
            renderUI: function()
            {
                var widget = this,
                    filterOptionsUrl = this.get('filterOptionsUrl'),
                    modelName = this.get('modelName'),
                    columnTitleNodeMap = this._columnTitleNodeMap = [];

                Y.all('.pure-table th').each(function(node, index)
                {
                    var columnTitle = node.get('text');
                    node.set('text', null);

                    node.append(IndexView.template({ title: columnTitle }));

                    columnTitleNodeMap.push({ title: columnTitle, node: node.one('.pure-menu') });
                });

                Y.io(filterOptionsUrl,
                {
                    headers: { 'Content-Type': 'application/json' },
                    data: 'modelName=' + modelName,
                    on:
                    {
                        success: function(id, response, arguments)
                        {
                            var filterOptions = JSON.parse(response.responseText);

                            function findItems(labelName)
                            {
                                return Y.Array.find(filterOptions, function(item, index)
                                {
                                    return item.label === labelName;
                                });
                            }

                            Y.each(columnTitleNodeMap, function(item, index)
                            {
                                item.node.set('text', null);

                                var menu = new Y.Menu(
                                {
                                    container: item.node,
                                    orientation: 'horizontal',
                                    hideOnOutsideClick: false,
                                    hideOnClick: false,
                                    items: findItems(item.title)
                                });

                                menu.render();
                                menu.show();
                            });

                            Y.on('click', Y.bind(widget._onMenuItemClick, widget), '.pure-menu-item');
                        },
                        failure: function(id, response, arguments)
                        {
                            Y.Alert.Show(Y.Alert.Types.Error(),
                                'Error getting column filter options (' + response.status + ': ' + response.statusText + ')');
                        }
                    }
                });
            },
            bindUI: function ()
            {
                Y.on('click', Y.bind(this._onSorterClick, this), '.sorter');
                Y.on('click', Y.bind(this._onSearchClick, this), '#searchButton');

                this._bindUpdatable();
            },
            syncUI: function()
            {
            },
            _bindUpdatable: function()
            {
                Y.on('click', Y.bind(this._onRowClick, this), '.pure-table tbody tr');
                Y.on('click', Y.bind(this._onPagerClick, this), '.pure-paginator li button:not(.pure-button-disabled)');
            },
            _onMenuItemClick: function(e)
            {
                e.stopPropagation();

                var node = e.target,
                    isTitle = node.ancestor('.pure-menu', false, (function(node)
                    {
                        var counter = 0;

                        return function() {
                            return ++counter === 4;
                        };
                    })(node)) !== null,
                    styleName = 'background-color',
                    color = 'rgb(214, 215, 215)',
                    nodeColor = node.getStyle(styleName)
                    filterKeyName = node.ancestor('th').one('span ul li a').get('text'),
                    filterValue = node.get('text'),
                    refreshTableUrl = this.get('refreshTableUrl'),
                    widget = this;

                    if(isTitle)
                        return;

                    if(nodeColor === color)
                    {
                        node.setStyle(styleName, 'inherit');

                        var remainingValues = Y.Array.filter(this._filterKeyValueMap[filterKeyName], function(item)
                        {
                            return item !== filterValue;
                        });

                        if(remainingValues.length === 0)
                            delete this._filterKeyValueMap[filterKeyName];
                        else
                            this._filterKeyValueMap[filterKeyName] = remainingValues;
                    }
                    else
                    {
                        node.setStyle(styleName, color);

                        if(!this._filterKeyValueMap.hasOwnProperty(filterKeyName))
                            this._filterKeyValueMap[filterKeyName] = [];

                        this._filterKeyValueMap[filterKeyName].push(filterValue);
                    }

                Y.io(refreshTableUrl,
                {
                    headers: { 'Content-Type': 'application/json' },
                    data: Y.QueryString.stringify(widget._getData()),
                    on:
                    {
                        success: function(id, response, arguments)
                        {
                            var model = JSON.parse(response.responseText);
                            widget._replaceUpdatable(model);
                        },
                        failure: function(id, response, arguments)
                        {
                            Y.Alert.Show(Y.Alert.Types.Error(),
                                'Error getting column filter options (' + response.status + ': ' + response.statusText + ')');
                        }
                    }
                });
                
            },
            _onSorterClick: function(e)
            {
                var sortKey = this._sortKey = e.target.ancestor('th').one('span ul li a').get('text'),
                    sortDirection = this._sortDirection = 'asc',
                    data = this._getData(),
                    iconNode = e.target,
                    refreshTableUrl = this.get('refreshTableUrl'),
                    widget = this;
                
                if(iconNode.hasClass('fa-sort'))
                {
                    iconNode.removeClass('fa-sort');
                    iconNode.addClass('fa-sort-asc');
                    iconNode.setAttribute('title', 'Sort Descending');
                }
                else if(iconNode.hasClass('fa-sort-desc'))
                {
                    iconNode.removeClass('fa-sort-desc');
                    iconNode.addClass('fa-sort-asc');
                    iconNode.setAttribute('title', 'Sort Descending');
                }
                else if(iconNode.hasClass('fa-sort-asc'))
                {
                    iconNode.removeClass('fa-sort-asc');
                    iconNode.addClass('fa-sort-desc');
                    iconNode.setAttribute('title', 'Sort Ascending');
                    data.sortDirection = 'desc';
                }

                e.target.ancestor('tr').all('th a i').each(function(node, index)
                {
                    if(node.ancestor('a').getAttribute('id') !== e.target.ancestor('a').getAttribute('id'))
                    {
                        node.setAttribute('class', 'fa fa-sort');
                        node.setAttribute('title', 'Sort Ascending');
                    }
                });

                Y.io(refreshTableUrl,
                {
                    headers: { 'Content-Type': 'application/json' },
                    data: Y.QueryString.stringify(data),
                    on:
                    {
                        success: function(id, response, arguments)
                        {
                            var model = JSON.parse(response.responseText);
                            widget._replaceUpdatable(model);

                        },
                        failure: function(id, response, arguments)
                        {
                            Y.Alert.Show(Y.Alert.Types.Error(),
                                'Error sorting table (' + response.status + ': ' + response.statusText + ')');
                        }
                    }
                });
            },
            _onPagerClick: function(e)
            {
                var node = e.target,
                    widget = this;
                
                Y.io(node.getData('link'),
                {
                    data: { modelName: widget.get('modelName') },
                    headers: { 'Content-Type': 'application/json' },
                    on:
                    {
                        success: function(id, response, arguments)
                        {
                            var model = JSON.parse(response.responseText);
                            widget._replaceUpdatable(model);
                        },
                        failure: function(id, response, arguments)
                        {
                            Y.Alert.Show(Y.Alert.Types.Error(),
                                'Error paging (' + response.status + ': ' + response.statusText + ')');
                        }
                    }
                });
            },
            _onSearchClick: function(e)
            {
                var node = e.target,
                    widget = this,
                    data = this._getData();

                this._searchQuery = data.searchQuery = Y.one('input[name="searchQuery"]').get('value');

                Y.io(node.getData('link'),
                {
                    data: Y.QueryString.stringify(data),
                    headers: { 'Content-Type': 'application/json' },
                    on:
                    {
                        success: function(id, response, arguments)
                        {
                            var model = JSON.parse(response.responseText);
                            widget._replaceUpdatable(model);
                        },
                        failure: function(id, response, arguments)
                        {
                            Y.Alert.Show(Y.Alert.Types.Error(),
                                'Error searching (' + response.status + ': ' + response.statusText + ')');
                        }
                    }
                });
            },
            _getData: function()
            {
                var filterKeys = [],
                    filterValues = [],
                    map = this._filterKeyValueMap;

                for(var property in map)
                {
                    if(map.hasOwnProperty(property))
                    {
                        Y.Array.each(map[property], function(item)
                        {
                            filterKeys.push(property);
                            filterValues.push(item);
                        });
                    }
                }

                var data =
                {
                    sortKey: this._sortKey,
                    sortDirection: this._sortDirection,
                    filterKeys: filterKeys,
                    filterValues: filterValues,
                    modelName: this.get('modelName')
                };

                if(this._searchQuery)
                    data.searchQuery = this._searchQuery;

                return data;
            },
            _onRowClick: function(e)
            {
                var node = e.target.ancestor();

                window.open(node.getData('link'), '_self');
            },
            _replaceUpdatable: function(model)
            {
                Y.one('.pure-table tbody').replace(model.table);
                Y.one('.pure-paginator').replace(model.pager);
                this._bindUpdatable();
            }
        });

    Y.IndexView = Y.mix(IndexView, Y.IndexView);

}, '1.0.0', {
    requires: ['widget',
        'io',
        'index-view-templates',
        'model-list',
        'gallery-sm-menu',
        'alert',
        'querystring']
});