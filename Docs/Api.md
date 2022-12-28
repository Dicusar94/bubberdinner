# Bubber Dinner API

- [Bubber Dinner API](#bubber-dinner-api)
  - [Auth](#auth)
    - [Register](#register)
      - [Register Request](#register-request)
      - [Register Response](#register-response)
    - [Login](#login)
      - [Login Request](#login-request)
      - [Login Response](#login-response)

## Auth

### Register

```js
POST {{host}}/auth/register
```

#### Register Request

```json
{
  "firstName": "Dicusar",
  "lastName": "Denis",
  "email": "dicusar.dv@gmail.com",
  "password": "dicusar.dv"
}
```

#### Register Response

```js
200 OK
```

### Login

```js
POST {{host}}/auth/login
```

#### Login Request

```json
{
  "email": "dicusar.dv@gmail.com",
  "password": "dicusar.dv"
}
```

#### Login Response

```js
200 OK
```
