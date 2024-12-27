package com.example.a8180378.models;

/**
 * The Transport class stores information about a transport created by a customer
 */
public class Transport {
    /**
     * contains the user of customer who created the transport
     */
    private String userName;
    /**
     * contains the status of the transport (accepted, rejected, for analyzing)
     */
    private Status status;

    /**
     * contains the category of truck that is to be used for the service
     */
    private TruckCategory truckCategory;

    /**
     * contains the date of the transport
     */
    private String date;

    /**
     * contains the capacity of the transport
     */
    private Double capacity;

    /**
     * contains the user of customer who created the transport
     */
    private Double weight;

    /**
     * contains the liters of the transport (if it is a tanker truck)
     */
    private int liters;

    /**
     * contains the value offered by the transport
     */
    private Double value_offered;

    /**
     * contains the origin of the transport
     */
    private String origin;

    /**
     * contains the destiny of the transport
     */
    private String destiny;

    /**
     * Getters e Setters
     */
    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public Status getStatus() {
        return status;
    }

    public void setStatus(Status status) {
        this.status = status;
    }

    public TruckCategory getTruckCategory() {
        return truckCategory;
    }

    public void setTruckCategory(TruckCategory truckCategory) {
        this.truckCategory = truckCategory;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public Double getCapacity() {
        return capacity;
    }

    public void setCapacity(Double capacity) {
        this.capacity = capacity;
    }

    public Double getWeight() {
        return weight;
    }

    public void setWeight(Double weight) {
        this.weight = weight;
    }

    public int getLiters() {
        return liters;
    }

    public void setLiters(int liters) {
        this.liters = liters;
    }

    public Double getValue_offered() {
        return value_offered;
    }

    public void setValue_offered(Double value_offered) {
        this.value_offered = value_offered;
    }

    public String getOrigin() {
        return origin;
    }

    public void setOrigin(String origin) {
        this.origin = origin;
    }

    public String getDestiny() {
        return destiny;
    }

    public void setDestiny(String destiny) {
        this.destiny = destiny;
    }
}
