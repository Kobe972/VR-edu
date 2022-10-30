package com.api.vreduhub.mapper;

import com.api.vreduhub.entity.Token;
import com.api.vreduhub.entity.User;
import org.apache.ibatis.annotations.*;

import java.sql.Date;

@Mapper
public interface TokenMapper {
    @Insert("REPLACE into token(username,token,logtime) values(#{username},#{token},#{logtime})")
    void insertToken(@Param("username") String username, @Param("token") String token, @Param("logtime") Date logtime);

    @Select("SELECT * FROM token WHERE token = #{token} limit 1")
    Token retrieveToken(@Param("token") String token);

    @Update("UPDATE token SET logtime = #{logtime} where token = #{token}")
    void updateToken(@Param("token") String token, @Param("logtime") Date logtime);
}
