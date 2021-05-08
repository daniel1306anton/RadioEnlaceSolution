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
var partionFlows = [];
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
    partionFlows = [];
    var sep = distance / partions;
    var counter = 0;
    var latlonginit = new google.maps.LatLng(initLat, initLong);    
    const youNameIt = 'http://maps.google.com/mapfiles/ms/icons/blue-pushpin.png';
    for (var i = 1; i < partions; i++) {
        counter = counter + sep;


        latlonginit = google.maps.geometry.spherical.computeOffset(latlonginit, sep, heading);

        markerObject = new google.maps.Marker({
            position: latlonginit,
            draggable: true, // Set draggable option as true
            map: map,
            id: "Division" + i,
            icon: youNameIt
        });

        
        var flow = {
            Index: i,
            Latitude: latlonginit.lat(),
            longitude: latlonginit.lng(),
            Distance: counter
        };

        partionFlows.push(flow);

    }
    ExecuteEarthProfile(sep);
}

function ExecuteEarthProfile(sepa) {

    
    $('#buttonExecute').prop('disabled', true);
    var modelJs = {
        DistanceFlow: $('#ai_distanceflow').val(),
        HeadingFlow: $('#ai_headingflow').val(),
        InitLatitude: $('#ai_init_latitude').val(),
        InitLongitude: $('#ai_init_longitude').val(),
        EndLatitude: $('#ai_end_latitude').val(),
        EndtLongitude: $('#ai_end_longitude').val(),
        PartitionFlow: $('#ai_partitionflow').val(),
        H1: $('#ai_h1').val(),
        H2: $('#ai_h2').val(),
        Frequency: $('#ai_Frequency').val(),
        At: $('#ai_at').val(),
        Bt: $('#ai_bt').val(),
        SeparateDistance: sepa,
        PartitionList: partionFlows

    };

    var jsonString = JSON.stringify({ model: modelJs });
    var jsonReq = JSON.parse(jsonString);

    $.ajax({
        url: "/ProfileEarth/Execute/",
        data: jsonString,
        type: "POST",
        dataType: "json",
        contentType: "application/json",        
        success: function (result) {
            if (result.Success === false) {

                alert(result.ErrorMessage);
            } else {
                SetGraphs(result);
                $('#chartTableDataModal').on('shown.bs.modal', function () {
                    
                    LoadPartial(result.TableData, "#result-table-data-earth");
                });
                LoadPartial(result.LinkRadio, "#linkEnlace");
                LoadPartial(result.LinkRadio, "#linkEnlace2");
                $("#dataEarth").css("display", "block");
            }
            
        },
        error: function(xhr, ajaxOptions, thrownError) {
            alert('Error saving corporate : \n' + xhr.responseText);
        }
    });
}

function LoadPartial(content, selector) {
    if (typeof content !== "string") {
        content = content.responseText;
    }
    return $(selector).html(content);
}



function SetGraphs(data) {


    var chartComplete = new CanvasJS.Chart("chartCompleteContainer", {
        animationEnabled: true,        
        title: {
            text: "Gráfico conjugado de elevación terrestre."
        },
        axisX: {
            title: "Distancia Antena 1 - Antena 2.",
            suffix: "m"
        },
        axisY: {
            title: "Elevación del terreno.",            
            suffix: "m"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            horizontalAlign: "center",
            dockInsidePlotArea: true
        },
        data: [
            {                
                type: "line",
                name: "Lm",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.LmList.Content)
            },
            {
                type: "line",
                name: "La",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.LaList.Content)
            },
            {
                type: "line",
                name: "Ht",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.HtList.Content)
            },
            {
                type: "line",
                name: "Zf",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.ZfList.Content)
            }

        ]
    });

    $('#chartCompleteContainerModal').on('shown.bs.modal', function () {
        chartComplete.render();
    });


    

    var chart = new CanvasJS.Chart("chartLmContainer", {
        animationEnabled: true,        
        theme: "light1",//light1        
        title: {
            text: "Gráfico LM."
        },
        axisX: {
            title: "Distancia Antena 1 - Antena 2.",
            suffix: "m"
        },
        axisY: {
            title: "Elevación del terreno.",
            suffix: "m"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            horizontalAlign: "center",
            dockInsidePlotArea: true
        },
        data: [
            {
                // Change type to "bar", "splineArea", "area", "spline", "pie",etc.
                type: "line",
                name: "Lm",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.LmList.Content)
            }
        ]
    });

    

    $('#chartLmContainerModal').on('shown.bs.modal', function () {
        chart.render();
    });

    var chartLa = new CanvasJS.Chart("chartLaContainer", {
        animationEnabled: true,
        //color: "#E83A15",
        theme: "light2",//light1
        title: {
            text: "Gráfico LA."
        },
        axisX: {
            title: "Distancia Antena 1 - Antena 2.",
            suffix: "m"
        },
        axisY: {
            title: "Elevación del terreno.",
            suffix: "m"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            horizontalAlign: "center",
            dockInsidePlotArea: true
        },
        data: [
            {
                // Change type to "bar", "splineArea", "area", "spline", "pie",etc.
                type: "line",
                name: "La",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.LaList.Content)
            }
        ]
    });

    
    $('#chartLaContainerModal').on('shown.bs.modal', function () {
        chartLa.render();
    });

    var chartHt = new CanvasJS.Chart("chartHtContainer", {
        animationEnabled: true,        
        theme: "light2",//light1
        title: {
            text: "Gráfico Ht."
        },
        axisX: {
            title: "Distancia Antena 1 - Antena 2.",
            suffix: "m"
        },
        axisY: {
            title: "Elevación del terreno.",
            suffix: "m"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            horizontalAlign: "center",
            dockInsidePlotArea: true
        },
        data: [
            {
                // Change type to "bar", "splineArea", "area", "spline", "pie",etc.
                type: "line",
                name: "Ht",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.HtList.Content)
            }
        ]
    });

    
    $('#chartHtContainerModal').on('shown.bs.modal', function () {
        chartHt.render();
    });
    

    var chartZf = new CanvasJS.Chart("chartZfContainer", {
        animationEnabled: true,        
        theme: "light2",//light1
        title: {
            text: "Gráfico Zf."
        },
        axisX: {
            title: "Distancia Antena 1 - Antena 2.",
            suffix: "m"
        },
        axisY: {
            title: "Elevación del terreno.",
            suffix: "m"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            verticalAlign: "top",
            horizontalAlign: "center",
            dockInsidePlotArea: true
        },
        data: [
            {
                // Change type to "bar", "splineArea", "area", "spline", "pie",etc.
                type: "line",
                name: "Zf",
                showInLegend: true,
                markerSize: 0,
                dataPoints: JSON.parse(data.ZfList.Content)
            }
        ]
    });

    $('#chartZfContainerModal').on('shown.bs.modal', function () {
        chartZf.render();
    });   

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