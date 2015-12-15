/// <reference path="../yui3/build/yui/yui-min.js" />

var YUIConfigurator =
{
    Configure: function (yuiBuildPath,
    galleryBuildPath,
    useDebugVersion)
    {
        var configuration =
        {
            classNamePrefix: 'pure',
            //filter: 'raw', // can be 'DEBUG' (debug version) or 'RAW' (non-minified)
            base: yuiBuildPath,
            groups:
                {
                    gallery:
                        {
                            base: galleryBuildPath,
                            patterns:
                                {
                                    'gallery-': {},
                                    'gallerycss-': { type: 'css' }
                                }
                        },
                    gallerycss:
                        {
                            base: galleryBuildPath,
                            modules:
                                {
                                    'gallery-sm-menu-core-css':
                                        {
                                            path: 'gallery-sm-menu/assets/gallery-sm-menu-core.css',
                                            type: 'css'
                                        }
                                }
                        }
                }
        };

        if (useDebugVersion === true)
        {
            configuration.filter = 'DEBUG';
        }

        YUI.applyConfig(configuration);
    }
};