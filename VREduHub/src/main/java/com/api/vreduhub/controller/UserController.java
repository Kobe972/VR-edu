package com.api.vreduhub.controller;

import com.api.vreduhub.entity.Token;
import com.api.vreduhub.entity.User;
import com.api.vreduhub.service.TokenService;
import com.api.vreduhub.service.UserService;
import com.api.vreduhub.utils.FolderUtils;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.configurationprocessor.json.JSONObject;
import org.springframework.stereotype.*;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.File;
import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Date;
import java.time.LocalDate;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Map;
import java.util.Random;

@Controller
@Slf4j
public class UserController {
    //将Service注入Web层
    @Resource
    UserService userService;

    @Resource
    TokenService tokenService;

    //实现登录
    @GetMapping("/login")
    public String show(){
        return "login";
    }
    @RequestMapping(value = "/loginIn",method = RequestMethod.POST)
    @ResponseBody
    public Map login(String username,String password, HttpServletResponse response) throws Exception {
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
            Calendar calendar= Calendar.getInstance();
            Date DateUtil = calendar.getTime();
            java.sql.Date logtime = new java.sql.Date(DateUtil.getTime());
            tokenService.Insert(username, token, logtime);
            result.put("result","success");
            result.put("username",username);
            result.put("token",token);
            Cookie renew = new Cookie("token", null);
            Cookie cookie = new Cookie("token", token);
            response.addCookie(renew);
            response.addCookie(cookie);
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
    public Map signUp(String username, String password, HttpServletResponse response) throws NoSuchAlgorithmException {
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
            Calendar calendar= Calendar.getInstance();
            Date DateUtil = calendar.getTime();
            java.sql.Date logtime = new java.sql.Date(DateUtil.getTime());
            tokenService.Insert(username, token, (java.sql.Date) logtime);
            result.put("result","success");
            result.put("username",username);
            result.put("token",token);
            userService.Insert(username, password);
            Cookie cookie = new Cookie("token", token);
            Cookie renew = new Cookie("token", null);
            response.addCookie(cookie);
            response.addCookie(renew);
        }
        return result;
    }
    //实现注册功能
    @RequestMapping(value = "/upload/model", method = RequestMethod.GET)
    public String uploadModelView(@CookieValue(value = "token",
            defaultValue = "none") String token) throws NoSuchAlgorithmException {
        Calendar calendar= Calendar.getInstance();
        Date DateUtil = calendar.getTime();
        java.sql.Date logtime = new java.sql.Date(DateUtil.getTime());
        if(tokenService.verify(token,logtime))
        {
            return "upload";
        }
        return "login";
    }
    @PostMapping("/upload/model")
    @ResponseBody
    public Map uploadFolder(@CookieValue(value = "token",
            defaultValue = "none") String token, MultipartFile[] model) {
        Calendar calendar= Calendar.getInstance();
        Date DateUtil = calendar.getTime();
        java.sql.Date logtime = new java.sql.Date(DateUtil.getTime());
        Map result=new HashMap();
        result.put("domain","uploadModel");
        if(!tokenService.verify(token,logtime))
        {
            result.put("result","unauthorized");
            return result;
        }
        Token _token=tokenService.retrieveToken(token);
        String basePath = "static" + File.separator + "models" + File.separator + _token.getName() + File.separator;
        try {
            boolean success = FolderUtils.saveMultiFile(basePath, model);
            if(success)
            {
                result.put("result","success");
            }
            else
            {
                result.put("result","fail: path illegal or already exists");
            }
        } catch (Exception e) {
            result.put("result",e.toString());
        }
        return result;
    }
}

