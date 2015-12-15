/// <reference path="../../yui3/build/yui/yui-min.js" />

YUI.add('index-view-templates', function (Y)
{
    Y.namespace('IndexView').template = Y.Template.Micro.compile(
        '<span class="pure-menu pure-menu-open pure-menu-horizontal" style="background-color:inherit;"><%= this.title %></span>'+
        '<a href="#" class="sorter"><i class="fa fa-sort" title="Sort Ascending"></i></a>'
    );

}, '1.0.0', { requires: ['template-micro'] });