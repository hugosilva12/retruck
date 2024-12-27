package com.example.a8180378.models;

/**
 * This class store the data needed to login
 */
public class Login {
    /**
     * Contains the username of the User
     */
    private String username;

    /**
     * Contains the password of the User
     */
    private String password;

    /**
     * Contains the role of the User
     */
    private int role;

    /**
     * Getters e Setters
     */

    public String getId() {
        return username;
    }

    public void setId(String id) {
        this.username = id;
    }

    public String getPassword() {
        return password;
    }


    public int getRole() {
        return role;
    }

    public void setRole(int role) {
        this.role = role;
    }


    public Login(String id, String password, int role) {
        this.username = id;
        this.password = password;
        this.role = role;
    }

    @Override
    public String toString() {
        return "Login{" +
                "id='" + username + '\'' +
                ", password='" + password + '\'' +
                ", role=" + role +
                '}';
    }
}
