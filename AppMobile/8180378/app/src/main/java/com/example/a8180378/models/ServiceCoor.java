package com.example.a8180378.models;

/**
 * The ServiceCoord class stores information about the location of the truck in a service
 */
public class ServiceCoor {

    /**
     * contains the id of service corresponding to the location
     */
    private  String idService;

    /**
     * contains the service corresponding to the location
     */
    private Coor coord ;

    public String getIdService() {
        return idService;
    }

    public void setIdService(String idService) {
        this.idService = idService;
    }

    public Coor getCoord() {
        return coord;
    }

    public void setCoord(Coor coord) {
        this.coord = coord;
    }
}
