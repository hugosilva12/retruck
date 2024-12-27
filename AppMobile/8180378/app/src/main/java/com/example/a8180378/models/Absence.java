package com.example.a8180378.models;

/**
 * The Absence class stores information about the Absence
 */
public class Absence {

    /**
     * contains the date of the absence
     */
    private String date;

    /**
     * contains the description of the absence
     */
    private String description;

    /**
     * contains the description of the absence
     */
    private AbsenceType absenceType;

    /**
     * contains the driver of the absence
     */
    private String id;

    /**
     * Getters e Setters
     */

    public String getDate() {
        return date;
    }

    public void setDate(String date) {
        this.date = date;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public AbsenceType getAbsenceType() {
        return absenceType;
    }

    public void setAbsenceType(AbsenceType absenceType) {
        this.absenceType = absenceType;
    }
}
