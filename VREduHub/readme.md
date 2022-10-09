# Prepare
This is the backend api. Before using it, you should set the mysql database as follows, or you may need to modify the code:
* username=root, password=xuyichang2003, port=localhost:3306.
* initialize the database according to database_init.sql.

# Java Requirement
You may need Java SE 17 in order to run the project.

# Run
After configuring your database, you can run VREduHub.jar in target folder.

# API Documentation
## GET Request
* /login: get the login page
* /signup: get the signup page
## POST Request
### /signup
* username: username to register
* password: corresponding password
### /login
* username: username to register
* password: corresponding password
## POST Response
The backend returns in json format.
* result: success or fail
* domain: action the user attempts to perform, i.e. login or register
* token: token generated for this session
For example, a successful login action returns:
```{"result":"success","domain":"login","username":"lhm","token":"559919ee65544e5072f64706932c80e5"}```