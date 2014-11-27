jQuery(document).ready(function() {
//Google Map					
	var latlng = new google.maps.LatLng(45.56430428,9.21770096);
	var settings = {
		zoom: 11,
		center: new google.maps.LatLng(45.56430428,9.21770096), 
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		mapTypeControl: false,
		scrollwheel: false,
		draggable: true,
		navigationControl: false
		};
		
	var map = new google.maps.Map(document.getElementById("google_map"), settings);
	
	google.maps.event.addDomListener(window, "resize", function() {
		var center = map.getCenter();
		google.maps.event.trigger(map, "resize");
		map.setCenter(center);
	});
	
	var contentString = '<div class="map-tooltip">'+
		'<h6>AGM SOLUTIONS</h6>'+
		'<p>Via Astesani, 54 - 20161 Milano(MI)</p>'+
		'</div>';
	var infowindow = new google.maps.InfoWindow({
		content: contentString
	});
	
	var companyImage = new google.maps.MarkerImage('images/map-pin.png',
		new google.maps.Size(40,70),<!-- Width and height of the marker -->
		new google.maps.Point(0,0),
		new google.maps.Point(20,55)<!-- Position of the marker -->
	);
	
	var companyPos = new google.maps.LatLng(45.520733,9.1692198);
	
	var companyMarker = new google.maps.Marker({
		position: companyPos,
		map: map,
		icon: companyImage, 
		zIndex: 3});	
	
	google.maps.event.addListener(companyMarker, 'click', function() {
		infowindow.open(map,companyMarker);
	});
	
	var contentString2 = '<div class="map-tooltip">'+
		'<h6>AGM SOLUTIONS</h6>'+
		'<p>La nostra nuova sede opertiva di Lissone <br/> Via Agostoni, 16 - 20851 Lissone(MB)</p>'+
		'</div>';
	var infowindow2 = new google.maps.InfoWindow({
		content: contentString2
	});
	
	var companyImage2 = new google.maps.MarkerImage('images/map-pin.png',
		new google.maps.Size(40,70),<!-- Width and height of the marker -->
		new google.maps.Point(0,0),
		new google.maps.Point(20,55)<!-- Position of the marker -->
	);
	
	var companyPos2 = new google.maps.LatLng(45.607934,9.234367);
	
	var companyMarker2 = new google.maps.Marker({
		position: companyPos2,
		map: map,
		icon: companyImage2, 
		zIndex: 3});	
	
	google.maps.event.addListener(companyMarker2, 'click', function() {
		infowindow2.open(map,companyMarker2);
	});
	
	
//Google Map click Show/Hide	
    $('.button-map').click(function() {
        $('#google_map').slideToggle(300, function(){
                google.maps.event.trigger(map, "resize"); // resize map
                map.setCenter(latlng); // set the center
            }); // slide it down
        $(this).toggleClass('close-map show-map');
    });

});		