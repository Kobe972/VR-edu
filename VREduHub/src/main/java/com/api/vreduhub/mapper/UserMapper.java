package com.api.vreduhub.mapper;
import com.api.vreduhub.entity.User;
import org.apache.ibatis.annotations.*;

@Mapper
public interface UserMapper {
    @Select("SELECT * FROM user WHERE username = #{username} AND password = #{password}")
    User getInfo(@Param("username") String username, @Param("password") String password);

    @Insert("insert into user(username,password)values(#{username},#{password})")
    void saveInfo(@Param("username") String username, @Param("password") String password);

    @Select("SELECT * FROM user WHERE username = #{username} limit 1")
    User retrieveUser(@Param("username") String username);


}

