# AssignmentP0

#### I have written four endpoints for Calendly API below.

## 1. /availability/{userID}
#### This is a get method to fetch a user's availabilities with a given "userID".

## 2. /availability
#### This is a post method to add new availability for a user and is protected by a password. The request payload is as follows -
```
{
  "password": "string",
  "userID": "string",
  "date": "string",
  "startTime": "string",
  "endTime": "string"
}
```
## 3. /createUser
#### This is a post method to add a new user. It takes payload as follows - 
```
{
  "userID": "string",
  "name": "string",
  "password": "string"
}
```
## 4. /overlap
#### This is a get method to find overlapping availability of two users on a given date. The date should be in dd/mm/yyyy format. It takes parameter as follows - 
```
{
  "UserID1": "string",
  "UserID2": "string",
  "Date": "string"
}
```

## Running & Testing the solution

#### Running the soluton - Clone the repository, Build the solution and Run without Debugging. Swagger UI will get open, where you can test the endpoints.
#### Testing - I have added dummy data for two users. I have used in-memory Entity Framework, so everytime you close a session the data added during the session will be lost.
