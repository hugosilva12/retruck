package com.example.a8180378.models;

import com.google.firebase.database.IgnoreExtraProperties;

/**
 * The ServiceTransport class stores information about an accepted transport service
 */
@IgnoreExtraProperties
public class Service {

    /**
     * contains the driver of the service
     */
    private String userNameDriver;

    /**
     * contains identifier of the service
     */
    private  String idService;

    /**
     * contains the origin of the service
     */
    private  String origin;

    /**
     * contains the destiny of the service
     */
    private String destiny;

    /**
     * contains the state of service
     */
    private ServicesStatus status;

    /**
     * contains the date of the service
     */
    private  String date;

    public Service() {

    }
    /**
     * Getters e Setters
     */
    public String getUserNameDriver() {
        return userNameDriver;
    }

    public void setUserNameDriver(String userNameDriver) {
        this.userNameDriver = userNameDriver;
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

    public String getIdService() {
        return idService;
    }

    public void setIdService(String idService) {
        this.idService = idService;
    }

    public ServicesStatus getStatus() {
        return status;
    }

    public void setStatus(ServicesStatus status) {
        this.status = status;
    }

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }
}
