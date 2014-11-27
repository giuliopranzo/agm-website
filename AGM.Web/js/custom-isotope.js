$(window).load(function() {	
	// Isotope
	var $container = $('.grid');
	
	colWidth = function () {
		var w = $container.width(), 
			columnNum = 1,
			columnWidth = 0;
		if (w > 1440) {
			columnNum  = 7;
		} else if (w > 1365) {
			columnNum  = 5;
		} else if (w > 1279) {
			columnNum  = 5;
		} else if (w > 1023) {
			columnNum  = 5;
		} else if (w > 767) {
			columnNum  = 3;
		} else if (w > 479) {
			columnNum  = 2;
		}
		
		columnWidth = Math.floor(w/columnNum);
		$container.find('.grid-item').each(function() {
			var $item = $(this);
			
			if ($item.hasClass('item-wide')) {
				if (w < 480) {
					$('.item-wide').css({
						'width'		 : ((columnWidth)-4) + 'px',
						'height' : Math.round(((columnWidth)-4) * 0.7777777) + 'px'
					});
					$('.item-wide img').css({
						'width'		 : ((columnWidth)-4) + 'px',
						'height' : 'auto'
					});	
				}
				else {
					$('.item-wide').css({
						'width'		 : ((columnWidth*2)-4) + 'px',
						'height' : Math.round(((columnWidth*2)-4) * 0.7777777) + 'px'
					});
					$('.item-wide img').css({
						'width'		 : ((columnWidth*2)-4) + 'px',
						'height' : 'auto'
					});				
				}
			}	
			
			if ($item.hasClass('item-small')) {
				$('.item-small').css({
					'width'		 : ((columnWidth)-4) + 'px',
					'height' : Math.round(((columnWidth)-4) * 0.7777777) + 'px'
				});
				$('.item-small img').css({
					'width'		 : ((columnWidth)-4) + 'px',
					'height' : 'auto'
				});						
			}
				
			if ($item.hasClass('item-long')) {
				if (w < 480) {
					$('.item-long').css({
						'width'		 : ((columnWidth)-4) + 'px',
						'height' : Math.round(((columnWidth)-4) * 0.7777777/2) + 'px'
					});
					$('.item-long img').css({
						'width'		 : ((columnWidth)-4) + 'px',
						'height' : 'auto'
					});		
				}
				else {
					$('.item-long').css({
						'width'		 : ((columnWidth*2)-4) + 'px',
						'height' : Math.round(((columnWidth)-4) * 0.7777777) + 'px'
					});
					$('.item-long img').css({
						'width'		 : ((columnWidth*2)-4) + 'px',
						'height' : 'auto'
					});					
				}
			}
			
			if ($item.hasClass('item-high')) {
				$('.item-high').css({
					'width'		 : ((columnWidth)-4) + 'px',
					'height' : Math.round(((columnWidth*2)-4) * 0.7777777) + 'px'
				});
				$('.item-high img').css({
					'width'		 : ((columnWidth)-4) + 'px',
					'height' : 'auto'
				});				
			}				

		});
		return columnWidth;
	}
	
	// Isotope Call
	gridIsotope = function () {
		$container.isotope({
			layoutMode : 'masonry',
			itemSelector: '.grid-item',
			animationEngine: 'jquery',	
			resizable: false,
			masonry: { columnWidth: colWidth(), gutterWidth: 0 }
		});
	};
	gridIsotope();
	$(window).smartresize(gridIsotope);	
	

	// Portfolio Filtering
	var $optionSets = $('#options .option-set'),
		$optionLinks = $optionSets.find('a');

	$optionLinks.click(function(){
		var $this = $(this);
		// don't proceed if already selected
		if ( $this.hasClass('selected') ) {
			return false;
		}
		var $optionSet = $this.parents('.option-set');
		$optionSet.find('.selected').removeClass('selected');
		$this.addClass('selected');
  
		// make option object dynamically, i.e. { filter: '.my-filter-class' }
		var options = {},
			key = $optionSet.attr('data-option-key'),
			value = $this.attr('data-option-value');
		// parse 'false' as false boolean
		value = value === 'false' ? false : value;
		options[ key ] = value;
		if ( key === 'layoutMode' && typeof changeLayoutMode === 'function' ) {
		  // changes in layout modes need extra logic
		  changeLayoutMode( $this, options )
		} else {
		  // otherwise, apply new options
		  $container.isotope( options );
		}
		
		return false;
	});				
	

	// modified Isotope methods for gutters in masonry
	$.Isotope.prototype._getMasonryGutterColumns = function() {
		var gutter = this.options.masonry && this.options.masonry.gutterWidth || 0;
			containerWidth = this.element.width();
  
		this.masonry.columnWidth = this.options.masonry && this.options.masonry.columnWidth ||
			// or use the size of the first item
			this.$filteredAtoms.outerWidth(true) ||
			// if there's no items, use size of container
			containerWidth;

		this.masonry.columnWidth += gutter;

		this.masonry.cols = Math.floor( ( containerWidth + gutter ) / this.masonry.columnWidth );
		this.masonry.cols = Math.max( this.masonry.cols, 1 );
	};

	$.Isotope.prototype._masonryReset = function() {
		// layout-specific props
		this.masonry = {};
		// FIXME shouldn't have to call this again
		this._getMasonryGutterColumns();
		var i = this.masonry.cols;
		this.masonry.colYs = [];
		while (i--) {
			this.masonry.colYs.push( 0 );
		}
	};	
  
	$.Isotope.prototype._masonryResizeChanged = function() {
		var prevSegments = this.masonry.cols;
		// update cols/rows
		this._getMasonryGutterColumns();
		// return if updated cols/rows is not equal to previous
		return ( this.masonry.cols !== prevSegments );
	};  

	
	blogisotope = function () {
		var gutterwidth,
			conwidth = $('.blog-masonry').width(),
			columnwidth = Math.floor(conwidth);
		
		if ($('.blog-masonry').hasClass('on-two-columns') === true) {
			
			columnwidth = Math.floor(conwidth*0.48);
			gutterwidth = Math.floor(conwidth*0.04);
			
			if ($(window).width() < 768) {
			columnwidth = Math.floor(conwidth*1);
			}
			else {
				columnwidth = Math.floor(conwidth*0.48);
			}		
			
		} else
		
		if ($('.blog-masonry').hasClass('on-three-columns') === true) {
			columnwidth = Math.floor(conwidth*0.3033);
			gutterwidth = Math.floor(conwidth*0.04);

					if ($(window).width() < 1023) {
						if ($(window).width() < 768) {
						columnwidth = Math.floor(conwidth*1);
						}
						else {
							columnwidth = Math.floor(conwidth*0.48);
						}
					}
					else {
						columnwidth = Math.floor(conwidth*0.3033);
					}
		}	
		
		$('.blog-masonry').find('.post-masonry').each(function() {
			$(this).css({'width' : columnwidth});
		});		

		gallery_slider();		
		
		// Calculate Audio bar width	
		var audiowidth = $('.audio-item').width();
		$('.jp-progress').css({'width': audiowidth-250});		
		
		return gutterwidth;
	}	
	
	var $blog_container = $('.blog-masonry');	
	
		// Blog Isotope Call
		bloggingisotope = function() {
			  $blog_container.isotope({
					itemSelector: '.post-masonry',
					animationEngine: 'jquery',
					masonry: {
						gutterWidth: blogisotope()
					}				
			  });
		};
		bloggingisotope();
		$(window).smartresize(bloggingisotope);	

});