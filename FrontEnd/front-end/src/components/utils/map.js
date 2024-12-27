import React from 'react'
import { GoogleMap, useJsApiLoader, Polyline, MarkerClusterer, Marker } from '@react-google-maps/api'
import locationIcon from "./imgs_map/icon.png"
import truckIcon from "./imgs_map/truck.png"
import start from "./imgs_map/start.png"
import finish from "./imgs_map/finish.png"
import home from "./imgs_map/home.png"

const containerStyle = {
    width: '100%',
    height: '400px'
};

const center = {
    lat: 41.242305755615234,
    lng: -7.940729141235352
};


const onLoad2 = polyline => {
    console.log('polyline: ', polyline)
};

const options2 = {
    strokeColor: '#009CFF',
    strokeOpacity: 0.8,
    strokeWeight: 2,
    fillColor: '#009CFF',
    fillOpacity: 0.35,
    clickable: false,
    draggable: false,
    editable: false,
    visible: true,
    radius: 30000,
    zIndex: 1
};

const options = {
    url: "http://www.geocodezip.com/mapIcons/markerclusterer/heart50.png", // so you must have m1.png, m2.png, m3.png, m4.png, m5.png and m6.png in that folder
}
function createKey(location) {
    return location.lat + location.lng
}


function MapComponentService(props) {
    const { isLoaded } = useJsApiLoader({
        id: 'google-map-script',
        googleMapsApiKey: "AIzaSyBXd0yjOuUk90C9suz2wQB03g2c5f4qXO8"
    })

    const [map, setMap] = React.useState(null)

    const onLoad = React.useCallback(function callback(map) {
        const bounds = new window.google.maps.LatLngBounds(center);
        map.fitBounds(bounds);
        setMap(map)
    }, [])

    const onUnmount = React.useCallback(function callback(map) {
        setMap(null)
    }, [])

    return isLoaded ? (
        <>
            <GoogleMap
                mapContainerStyle={containerStyle}
                center={props.locations[0]}
                zoom={8}
                onLoad={onLoad}
                onUnmount={onUnmount}
            >
                {props.initServiceAddress != undefined &&

                    <MarkerClusterer
                    >
                        {(clusterer) =>
                            <Marker icon={start} key={createKey(props.initServiceAddress)} position={props.initServiceAddress} clusterer={clusterer} />
                        }
                    </MarkerClusterer>

                }
                {props.organizationLocation != undefined &&

                    <MarkerClusterer
                    >
                        {(clusterer) =>
                            <Marker icon={home} key={createKey(props.organizationLocation)} position={props.organizationLocation} clusterer={clusterer} />
                        }
                    </MarkerClusterer>

                }
                <MarkerClusterer
                >
                    {(clusterer) =>
                            <Marker icon={truckIcon} key={createKey(props.nowLocationTruck)} position={props.nowLocationTruck} clusterer={clusterer} />
                    }
                </MarkerClusterer>

                {props.finishService != undefined &&

                    <MarkerClusterer
                    >
                        {(clusterer) =>
                            <Marker icon={finish} key={createKey(props.finishService)} position={props.finishService} clusterer={clusterer} />
                        }
                    </MarkerClusterer>

                }

                <Polyline
                    onLoad={onLoad2}
                    path={props.locations}
                    options={options2}
                />
            </GoogleMap>
        </>
    ) : <></>
}

export default React.memo(MapComponentService)