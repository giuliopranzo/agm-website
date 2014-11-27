/*
	Scripts for Patti HTML - V1.0
*/

$(window).load(function() {	

	// Portfolio Gallery Slider
	$(".portfolio-slider").owlCarousel({
		stopOnHover : true,
		navigation:true,
		navigationText: [
			  "<i class='fa fa-angle-left'></i>",
			  "<i class='fa fa-angle-right'></i>"
			  ],		
		paginationSpeed : 1000,
		goToFirstSpeed : 2000,
		autoPlay : 8000,		
		singleItem : true,
		transitionStyle:"fade",
		afterAction: afterAction
	});	

	function afterAction(){
		$('.slider-nav').text(""+(this.owl.currentItem+1)+"/" + this.owl.owlItems.length+"");
	}	
	
	
	// Testimonials Slider
	$("#owl-testimonials").owlCarousel({
		autoHeight : true,
		singleItem : true,
		navigation:true,
		slideSpeed : 1000
	});
	
	// Clients Slider
	$("#owl-clients").owlCarousel({
		items : 4,
		navigation:true,
		autoPlay: 5000,
		slideSpeed : 1000
	});
	
	// Twitter Slider
	$("#owl-twitter").owlCarousel({
		autoHeight : true,
		singleItem : true,
		navigation:true,
		slideSpeed : 1000
	});		
	
	
	// Calculate Audio bar width	
	var audiowidth = $('.audio-item').width();
	$('.jp-progress').css({'width': audiowidth-250});
	
	
	// Take care of intro loader
	$("#spinner").delay(400).fadeOut(); 
	$(".whitebg").delay(800).fadeOut("slow");	
	

	// Parallax Backgrounds
	if(!( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) )) {	
		$('#contact').parallax("50%", 0.4);
		$('#services').parallax("50%", 0.4);
		$('#twitter').parallax("50%", 0.4);	
		$('#projectbg').parallax("50%", 0.4);	
		$('#quote').parallax("50%", 0.4);
	}
	
	// Parallax Fix for Mobile Devices
	if( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ) {
		$('.parallax-section').css({'background-attachment': 'scroll'});
	}

	// Fixes some Waypoint issues
	$('body').waypoint(function() {
		console.log('ready to go');
	}, 
	{ 
		triggerOnce: true
	});
		
});


function pattinav() {
	
	// Menu Superfish Call //
	$('ul#mainnav').superfish({
		delay: 800,
		speed: 'normal', 
		autoArrows: false,
		animation: {opacity:'show'},   
		animationOut: {opacity:'hide'}
	});
	
	$("ul#mainnav li").css({ "overflow":"visible"});

	//Scroll Nav
	$('#mainnav').onePageNav({
		currentClass: 'current',
		filter: ':not(.external)'
	});
	
	// Responsive Navigation 

	var nava = $(".nav-btn"),
		navb = $("#navigation"),
		wind = $(window).width();	
			
	// Add classes		
    $(window).resize(function () {
		var nava = $(".nav-btn"),
			navb = $("#navigation"),
			wind = $(window).width();
		
		if (wind > 1023) {
			navb.addClass("desktop");
			navb.removeClass("mobile")
		}
		if (wind < 1023) {
			navb.addClass("mobile");
			navb.removeClass("desktop")
		}
    });
			
		if (wind > 1023) {
			navb.addClass("desktop");
			navb.removeClass("mobile")
		}
		if (wind < 1023) {
			navb.addClass("mobile");
			navb.removeClass("desktop")
		}			
		
	// Click Tweak	
	nava.click(function () {
		if (navb.is(":visible")) {
			navb.slideUp()
		} else {
			navb.slideDown()
		}
	});	
	
	$("#navigation a").click(function () {
		if (navb.is(":visible") && navb.hasClass("mobile")) {
			navb.slideUp();
		}
	});		
		
	
	// Fixed Element Height
	var headerheight = $('#header').outerHeight();
	$('.menu-fixer').css({'height': headerheight});
}


// Counting Numbers
function counts() {

	var counterWrapper = $('.counter-wrapper').width(),
		$counterItem = $('.counter-item'),
		counters = $counterItem.length,
		counterWidth;
		
	if (counterWrapper < 768) {
		counterWidth = counterWrapper/2;
		$counterItem.css({'border-left': '0'});
	}
	else if (counterWrapper < 480) {
		counterWidth = counterWrapper;
		$counterItem.css({'border-left': '0'});
	}
	else {
		counterWidth = counterWrapper/counters;
	}
		
	$counterItem.css({'width': counterWidth-1});
	$counterItem.eq(0).addClass('first-item');


	$('.counter-wrapper').waypoint(function() {
		$('.counter-number').countTo();	
	}, 
	{ 
		offset: '90%',
		triggerOnce: true
	});
}


//Effect for Scrolltop Button	
function totop() {		
	$('.totop').hover(function(){	
	$(this).animate({bottom:"-5px"},{queue:false,duration:60}); },
	function() {         
		$(this).animate({bottom:"-10px"},{queue:false,duration:60})
	});
	
	//Scroll to top
	$('.totop').click(function(){
        $("html, body").animate({ scrollTop: 0 }, 700);
        return false;
    });	
}

	
// Tabs
function pattitabs() {	
	$('.tabs-wrapper').each(function() {
		$(this).find(".tab-content").hide(); //Hide all content
		$(this).find("ul.tabs li:first").addClass("active").show(); //Activate first tab
		$(this).find(".tab-content:first").show(); //Show first tab content
	});
	$("ul.tabs li").click(function(e) {
		$(this).parents('.tabs-wrapper').find("ul.tabs li").removeClass("active"); //Remove any "active" class
		$(this).addClass("active"); //Add "active" class to selected tab
		$(this).parents('.tabs-wrapper').find(".tab-content").hide(); //Hide all tab content

		var activeTab = $(this).find("a").attr("href"); //Find the href attribute value to identify the active tab + content
		$("li.tab-item:first-child").css("background", "none" );
		$(this).parents('.tabs-wrapper').find(activeTab).fadeIn(); //Fade in the active ID content
		e.preventDefault();
	});
	$("ul.tabs li a").click(function(e) {
		e.preventDefault();
	})
	$("li.tab-item:last-child").addClass('last-item');
}


// Toggles
function toggles() {
	$('#toggle-view li').click(function () {
        var text = $(this).children('div.panel');
        if (text.is(':hidden')) {
            text.slideDown('200');
            $(this).children('span').addClass('toggle-minus');     
            $(this).addClass('activated');     
        } else {
            text.slideUp('200');
			$(this).children('span').removeClass('toggle-minus'); 
            $(this).children('span').addClass('toggle-plus'); 
			$(this).removeClass('activated'); 			
        }
         
    });
}


// Skills 
function pattiskills() {
	$('.skillbar').each(function(){
		var barwidth = $(this).attr('data-percent');


		$(this).waypoint(function() {
		
			$(this).find('.skillbar-bar').animate({
				width: barwidth
			},2000);
			$(this).find('.skill-bar-percent').animate({
				'left':barwidth,
				'margin-left': '-19px',
				'opacity': 1
			}, 2000);	
		}, 
		{ 
			offset: '90%',
			triggerOnce: true
		});		
	});	
}


// Services
function pattiservices() {
	$('.dt-service-item').click(function() {
			$(this).parent().children('.dt-service-hover').fadeIn();
			$(this).parent().siblings().children('.dt-service-hover').fadeOut();
			$('.dt-service-item').addClass('under-opacity');			
	});	
	
	$('.dt-service-hover').click(function() {
		$(this).fadeOut();
		$('.dt-service-item').removeClass('under-opacity');	
	});
}



// Blog Gallery Slider
function gallery_slider() {
	$(".gallery-slider").owlCarousel({
		stopOnHover : true,
		navigation:true,
		navigationText: [
			  "<i class='fa fa-angle-left'></i>",
			  "<i class='fa fa-angle-right'></i>"
			  ],		
		paginationSpeed : 1000,
		goToFirstSpeed : 2000,
		singleItem : true,
		transitionStyle:"fade"
	});			
}


	
$(document).ready(function() {

	//Run Functions
	pattinav();
	counts();
	totop();	
	pattiservices();
	pattitabs();
	toggles();
	pattiskills();
	
	
	
	// Header Effect on Scroll
	$(window).scroll( function() {
			var value = $(this).scrollTop();
			if ( value > 120 )	{
				$("#header").addClass("scrolled-header");
				$("#header").css({"padding-top": "15px", "padding-bottom": "15px"});
				$(".scrolled-header").css({"background":"rgba(255,255,255,0.95)", "box-shadow": "0px 0px 3px rgba(0, 0, 0, 0.3)"});
				$(".no-rgba .scrolled-header").css({"background": "url(images/no-rgba-white.png) repeat scroll 0 0"});
				$(".logo img").css({"height": "30px", "width": "auto"});
				$(".scrolled-header #mainnav").css({"padding-top": "2px"});
				$(".scrolled-header ul#mainnav li ul li a").css({'background': 'rgba(255,255,255,0.95)'});
				$(".no-rgba .scrolled-header ul#mainnav li ul li a").css({"background": "url(images/no-rgba-white.png) repeat scroll 0 0"});
				
				$("#header.no-header").addClass("show");
				
				$(".no-csstransforms .no-header").css({"display": "block"});
			
			}
			else {
				$("#header").removeClass("scrolled-header");
				$("#header.transparent-header, #header.solid-header").css({"padding-top": "55px", "padding-bottom": "25px"});
				$(".logo img").css({"height": "auto", "width": "auto"});
				$("#header #mainnav").css({"padding-top": "10px"});
				
				$(".transparent-header").css({"background":"rgba(255,255,255,0.75)", "box-shadow": "none"});
				$(".no-rgba .transparent-header").css({"background":"url(images/no-rgba-white.png) repeat scroll 0 0"});
				$(".solid-header").css({"background":"#fff", "box-shadow": "none"});
				
				$("#header.no-header .logo img").css({"height": "30px", "width": "auto"});
				$("#header.no-header").removeClass("show");
				$(".no-csstransforms .no-header").css({"display": "none"});				
				
			}
	});	
	
	// Animations on Scroll

	$('#wrapper').jackInTheBox({offset: 0});
	
	
	// In and Out Effect
	$('.item-on-hover').hover(function(){		 		 
		$(this).animate({ opacity: 1 }, 200);
		$(this).children('.hover-link, .hover-image, .hover-video').animate({ opacity: 1 }, 200);
	}, function(){
		$(this).animate({ opacity: 0 }, 200);
		$(this).children('.hover-link, .hover-image, .hover-video').animate({ opacity: 0 }, 200);
	});
	
	
	// Portfolio Grid In and Out Effect //
	$('.grid-item-on-hover').hover(function(){
		$(this).animate({ opacity: 0.9 }, 200);
	}, function(){
			$(this).animate({ opacity: 0 }, 200);
		});
		

	//Leaving Page Fade Out Effect
	$('a.external').click(function(e){
		var url = $(this).attr('href');		
		e.preventDefault();		
	  		$('.whitebg').fadeIn(400, function(){		 
				$("#spinner").fadeIn(400);				
			    document.location.href = url;
		  	});
	  return false;
	});	
	
	
	// Video in Posts		
	$(".post-video").fitVids();		
	
	
	// Twitter Feed
	$(".tweet").tweet({
		modpath: 'twitter/',
		join_text: "auto",
		username: "deliciousthemes",
		count: 3,
		template: "{time}{text}{reply_action}{retweet_action}{favorite_action}",
		auto_join_text_reply: null,
		auto_join_text_default: null,        // [string]   auto text for non verb: "i said" bullocks
		auto_join_text_ed: null,                   // [string]   auto text for past tense: "i" surfed
		auto_join_text_ing: null,               // [string]   auto tense for present tense: "i was" surfing
		auto_join_text_reply: null,     // [string]   auto tense for replies: "i replied to" @someone "with"
		auto_join_text_url: null, 
		loading_text: "loading tweets..."
	});	

	
	// Flickr Widget
	$('#flickr').jflickrfeed({
		limit: 10,
		qstrings: {
			id: '58842866@N08',
			tags: 'architecture'
		},
		itemTemplate: 
		'<li>' +
			'<a href="{{image_b}}"><img src="{{image_s}}" alt="{{title}}" /></a>' +
		'</li>' 
	});	

	
	// PrettyPhoto
	$("a[rel^='prettyPhoto']").prettyPhoto({animation_speed:'normal',theme:'light_square',slideshow:3000, autoplay_slideshow: false});
	

	// Appending HTML Content for Info Boxes FontAwesome
	$('.box-error').prepend('<i class="fa fa-minus-circle"></i>');
	$('.box-notice').prepend('<i class="fa fa-exclamation-circle"></i>');
	$('.box-success').prepend('<i class="fa fa-flag"></i>');
	$('.box-info').prepend('<i class="fa fa-info-circle"></i>');

	
	// Audio Player
	$("#audio_jplayer").jPlayer({
		ready: function (event) {
			$(this).jPlayer("setMedia", {
				mp3:"media/audio.mp3",
				ogg:"media/audio.ogg"
			});
		},
		swfPath: "/media",
		supplied: "mp3, ogg",
		wmode: "window"
	});

});


/*-----------------------------------------------------------------------------------*/
/*	Social Networks Block
/*-----------------------------------------------------------------------------------*/
	
	$('.share-options a').click(function(e) {
		e.preventDefault();
	});
	
	// Twitter
	function twitterSharer(){
		window.open( 'http://twitter.com/intent/tweet?text='+jQuery(".title-content h2").text() +' '+window.location, 
			"twitterWindow", 
			"width=650,height=350" );
		return false;
	}

	// Facebook

	function facebookSharer(){
		window.open( 'https://www.facebook.com/sharer/sharer.php?u='+encodeURIComponent(location.href), 
			'facebookWindow', 
			'width=650,height=350');
		return false;
	}		

	// Pinterest

	function pinterestSharer(){
		window.open( 'http://pinterest.com/pin/create/bookmarklet/?media='+ jQuery('.begin-content img').first().attr('src') + '&description='+jQuery('.title-content h2').text()+' '+encodeURIComponent(location.href), 
			'pinterestWindow', 
			'width=750,height=430, resizable=1');
		return false;
	}	


	// Google Plus

	function googleSharer(){
		window.open( 'https://plus.google.com/share?url='+encodeURIComponent(location.href), 
			'googleWindow', 
			'width=500,height=500');
		return false;
	}	


	// Delicious

	function deliciousSharer(){
		window.open( 'http://delicious.com/save?url='+encodeURIComponent(location.href)+'?title='+jQuery(".title-content h2").text(), 
			'deliciousWindow', 
			'width=550,height=550, resizable=1');
		return false;
	}

	// Linkedin

	function linkedinSharer(){
		window.open( 'http://www.linkedin.com/shareArticle?mini=true&url='+encodeURIComponent(location.href)+'$title='+jQuery(".title-content h2").text(), 
			'linkedinWindow', 
			'width=650,height=450, resizable=1');
		return false;
	}