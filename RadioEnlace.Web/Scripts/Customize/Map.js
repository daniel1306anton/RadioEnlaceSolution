// *
// * Coordinates search
// * 2013 - en.marnoto.com
// *

// Required variables.
var map;
var marker;
var geocoder;
var mapOptions;

var mapEstablishment;
var markerEstablishment;
var geocoderEstablishment;
var mapOptionsEstablishment;

var markers = [];
var markerInit;
var markerEnd;

function initializeMap(latOpco, lonOpco) {
    mapOptions = {
        center: new google.maps.LatLng(latOpco, lonOpco),
        zoom: 17,
        mapTypeId: 'roadmap'
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    geocoder = new google.maps.Geocoder();

    // This event detects a click on the map.
    google.maps.event.addListener(map, "click", function (event) {

        // Get lat lng coordinates.
        // This method returns the position of the click on the map.
        var lat = event.latLng.lat().toFixed(6);
        var lng = event.latLng.lng().toFixed(6);

        // Call createMarker() function to create a marker on the map.
        createMarker(lat, lng, marker, map);

        // getCoords() function inserts lat and lng values into text boxes.
        getCoords(lat, lng);

    });


}


// Function that creates the marker.
function createMarker(lat, lng, markerObject, mapObject) {

    // The purpose is to create a single marker, so
    // check if there is already a marker on the map.
    // With a new click on the map the previous
    // marker is removed and a new one is created.

    // If the marker variable contains a value
    var markerId = "markerInit";
    if (markers && markers.length >=1) {
        if (markers.length === 2) {
            alert('Solo se permiten dos marcas.');
            return;
        }        
        for (var i = 0; i < markers.length; i++) {
            //markers[i].setMap(null);
            
        }
        //markers = [];
        markerId = "markerEnd";
    }

    

    // Set marker variable with new location
    markerObject = new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        draggable: true, // Set draggable option as true
        map: mapObject,
        id: markerId
    });

    if (markerId === "markerInit") {
        markerInit = markerObject;
    }
    if (markerId === "markerEnd") {
        markerEnd = markerObject;
    }

    // This event detects the drag movement of the marker.
    // The event is fired when left button is released.
    google.maps.event.addListener(markerObject, 'dragend', function () {

        // Updates lat and lng position of the marker.
        markerObject.position = markerObject.getPosition();

        // Get lat and lng coordinates.
        var lat = markerObject.position.lat().toFixed(6);
        var lng = markerObject.position.lng().toFixed(6);

        var markId = markerObject.id;
        // Update lat and lng values into text boxes.
        getCoords(lat, lng, markId);

    });

    markers.push(markerObject);
}

function SetMarkerDivision() {

    var heading = $('#ai_headingflow').val();
    var partions = Number($('#ai_partitionflow').val());
    var initLat = Number($('#ai_init_latitude').val());
    var initLong = Number($('#ai_init_longitude').val());
    var distance = Number($('#ai_distanceflow').val());

    var sep = distance / partions;

    var latlonginit = new google.maps.LatLng(initLat, initLong);    
    const youNameIt = 'http://maps.google.com/mapfiles/ms/icons/blue-pushpin.png';
    for (var i = 1; i < partions; i++) {
        latlonginit = google.maps.geometry.spherical.computeOffset(latlonginit, sep, heading);

        markerObject = new google.maps.Marker({
            position: latlonginit,
            draggable: true, // Set draggable option as true
            map: map,
            id: "Division" + i,
            icon: youNameIt
        });

    }
    

}

// This function updates text boxes values.
function getCoords(lat, lng, markId = "") {

    // Update latitude text box.
    $('.latitud').val(lat);

    // Update longitude text box.
    $('.longitude').val(lng);

    if (markers) {
        if (markers.length <= 1 || markId === "markerInit") {
            $('#ai_init_latitude').val(lat);

            // Update longitude text box.
            $('#ai_init_longitude').val(lng);
        } else {
            $('#ai_end_latitude').val(lat);

            // Update longitude text box.
            $('#ai_end_longitude').val(lng);
        }
    }  
    setDistance();    
}

var line = [];
function drawLine(initLat,initLong,endLat,endLong) {
    var flightPlanCoordinates = [
        { lat: initLat, lng: initLong },
        { lat: endLat, lng: endLong }
    ];   
    for (i = 0; i < line.length; i++) {
        line[i].setMap(null); //or line[i].setVisible(false);
    }
    var flightPath = new google.maps.Polyline({
        path: flightPlanCoordinates,
        geodesic: true,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });
    line.push(flightPath);
    flightPath.setMap(map);
}
function setDistance() {
    var initLat = $('#ai_init_latitude').val();
    var initLong = $('#ai_init_longitude').val();
    var endLat = $('#ai_end_latitude').val();
    var endLong = $('#ai_end_longitude').val();

    if (initLat !== "" && initLong !== ""
        && endLat !== "" && endLong !== "") {

        var initLatNum = Number(initLat);
        var initLongNum = Number(initLong);
        var endLatNum = Number(endLat);
        var endLongNum = Number(endLong);
        var a = new google.maps.LatLng(initLatNum, initLongNum);
        var b = new google.maps.LatLng(endLatNum, endLongNum);
        var distance = google.maps.geometry.spherical.computeDistanceBetween(a, b);
        $('#ai_distanceflow').val(distance);

        
        drawLine(initLatNum, initLongNum, endLatNum, endLongNum);
        serHeading();

    }
}

function serHeading() {
    
    var path = [markerInit.getPosition(), markerEnd.getPosition()];
    
    var heading = google.maps.geometry.spherical.computeHeading(
        path[0],
        path[1]
    );
    $('#ai_headingflow').val(heading);
}
function pointAddress(lat, lng) {
    var myLatlng = new google.maps.LatLng(lat, lng);
    mapOptions = {
        center: myLatlng,
        zoom: 17,
        mapTypeId: 'roadmap'
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    map.setCenter(myLatlng);
    // Place a draggable marker on the map
    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        draggable: true
    });

    google.maps.event.addListener(map, "click", function (event) {

        // Get lat lng coordinates.
        // This method returns the position of the click on the map.
        var lat = event.latLng.lat().toFixed(6);
        var lng = event.latLng.lng().toFixed(6);

        // Call createMarker() function to create a marker on the map.
        createMarker(lat, lng, marker, map);

        // getCoords() function inserts lat and lng values into text boxes.
        getCoords(latOpco, lonOpco);

    });
}

function setListener() {
    // This event detects the drag movement of the marker.
    // The event is fired when left button is released.
    google.maps.event.addListener(marker, 'dragend', function () {

        // Updates lat and lng position of the marker.
        marker.position = marker.getPosition();

        // Get lat and lng coordinates.
        var lat = marker.position.lat();
        var lng = marker.position.lng();

        // Update lat and lng values into text boxes.
        getCoords(lat, lng);

    });
}

function internetConnectionValidation() {
    var online = navigator.onLine;
    if (online) {
        fetch('https://www.google.com/')
            .then(function (response) {

                if (!response.ok) {

                    //throw Error(response.statusText);
                    return false;
                }
                else {
                    return true;
                }

                //return response;

            }).then(function (response) {
                // response.ok fue true
                console.log('ok');
                resolve(response);
            }).catch(function (error) {
                console.log('Problema al realizar la solicitud: ' + error.message);
                reject(error);
            });
    }
    return false;
}