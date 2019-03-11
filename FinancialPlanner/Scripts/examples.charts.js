
(function( $ ) {

	'use strict';


    $('.meter').liquidMeter({
        shape: 'circle',
        color: '#0088CC',
        background: '#F9F9F9',
        fontSize: '24px',
        fontWeight: '600',
        stroke: '#F2F2F2',
        textColor: '#333',
        liquidOpacity: 0.9,
        liquidPalette: ['#333'],
        speed: 3000,
        animate: !$.browser.mobile
    });


}).apply( this, [ jQuery ]);