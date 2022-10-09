package com.api.vreduhub.service;

import com.api.vreduhub.entity.User;
import com.api.vreduhub.mapper.UserMapper;
import org.springframework.stereotype.*;
import javax.annotation.Resource;

@Service
public class UserService {
    //将dao层属性注入service层
    @Resource
    private UserMapper userMapper;

    public User LoginIn(String username, String password) {
        return userMapper.getInfo(username,password);
    }

    public void Insert(String username,String password){
        userMapper.saveInfo(username, password);
    }

    public User retrieveUser(String username) { return userMapper.retrieveUser(username);}
}


