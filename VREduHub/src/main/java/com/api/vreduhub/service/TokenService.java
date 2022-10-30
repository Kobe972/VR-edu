package com.api.vreduhub.service;

import com.api.vreduhub.entity.Token;
import com.api.vreduhub.entity.User;
import com.api.vreduhub.mapper.TokenMapper;
import com.api.vreduhub.mapper.UserMapper;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.sql.Date;

@Service
public class TokenService {
    @Resource
    private TokenMapper tokenMapper;

    public boolean verify(String token, Date logtime) {
        Token oldToken=tokenMapper.retrieveToken(token);
        if(oldToken==null) return false;
        if(logtime.getTime()-oldToken.getDate().getTime()>=1000*60*60*24)
        {
            return false;
        }
        tokenMapper.updateToken(token, logtime);
        return true;
    }

    public Token retrieveToken(String token) {
        return tokenMapper.retrieveToken(token);
    }

    public void Insert(String username,String token, Date logtime){
        tokenMapper.insertToken(username, token, logtime);
    }
}
