package com.api.vreduhub.entity;

import java.sql.Date;

public class Token {
    private Integer id;
    private String username;
    private String token;
    private Date logtime;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getName() {
        return username;
    }

    public Date getDate() {
        return logtime;
    }

    public void setDate(Date logtime) {
        this.logtime = logtime;
    }

    public void setName(String username) {
        this.username = username;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }

}
