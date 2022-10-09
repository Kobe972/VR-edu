package com.api.vreduhub.controller;

import com.api.vreduhub.entity.User;
import com.api.vreduhub.service.UserService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.configurationprocessor.json.JSONObject;
import org.springframework.stereotype.*;
import org.springframework.web.bind.annotation.*;

import javax.annotation.Resource;
import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.Random;

@Controller
@Slf4j
public class UserController {
    //将Service注入Web层
    @Resource
    UserService userService;

    //实现登录
    @GetMapping("/login")
    public String show(){
        return "login";
    }
    @RequestMapping(value = "/loginIn",method = RequestMethod.POST)
    @ResponseBody
    public Map login(String username,String password) throws Exception {
        User userBean = userService.LoginIn(username, password);
        log.info("name:{}",username);
        log.info("password:{}",password);
        Map result=new HashMap();
        Random r = new Random();
        String raw=username+'\n'+password+'\n'+r.nextInt();
        MessageDigest md = MessageDigest.getInstance("MD5");
        md.update(raw.getBytes());
        String token = new BigInteger(1, md.digest()).toString(16);
        result.put("domain","login");
        if(userBean!=null){
            result.put("result","success");
            result.put("username",username);
            result.put("token",token);
        }else {
            result.put("result","fail");
        }
        return result;
    }
    @RequestMapping("/")
    @ResponseBody
    public String mainPage(){
        return "Index page not developed";
    }
    @RequestMapping("/signup")
    public String disp(){
        return "signup";
    }

    //实现注册功能
    @RequestMapping(value = "/register",method = RequestMethod.POST)
    @ResponseBody
    public Map signUp(String username,String password) throws NoSuchAlgorithmException {
        User userBean = userService.retrieveUser(username);
        Map result=new HashMap();
        Random r = new Random();
        String raw=username+'\n'+password+'\n'+r.nextInt();
        MessageDigest md = MessageDigest.getInstance("MD5");
        md.update(raw.getBytes());
        String token = new BigInteger(1, md.digest()).toString(16);
        result.put("domain","register");
        if(userBean!=null){
            result.put("result","fail");
        }else {
            result.put("result","success");
            result.put("username",username);
            result.put("token",token);
            result.put("result","success");
            userService.Insert(username, password);
        }
        return result;
    }
}

