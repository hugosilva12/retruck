package com.example.a8180378.models;

/**
 * The Coord class stores the coordinates at a time of a truck
 */
public class Coor {

    public Coor() {

    }

    /**
     * contains the latitude of the location
     */
    private float latitude;

    /**
     * contains the longitude of the location
     */
    private float longitude;


    public float getLatitude() {
        return latitude;
    }

    public void setLatitude(float latitude) {
        this.latitude = latitude;
    }

    public float getLongitude() {
        return longitude;
    }

    public void setLongitude(float longitude) {
        this.longitude = longitude;
    }
}
