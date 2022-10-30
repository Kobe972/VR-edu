CREATE DATABASE demo;
USE demo;
CREATE TABLE user (
`username` VARCHAR(100) NOT NULL,
`password` VARCHAR(100) NOT NULL,
PRIMARY KEY(`username`));
CREATE TABLE token (
`username` VARCHAR(100) NOT NULL,
`token` VARCHAR(100) NOT NULL,
`logtime` DATE NOT NULL,
PRIMARY KEY(`username`));