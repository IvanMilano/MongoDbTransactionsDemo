# MongoDbTransactionsDemo
This is a dot net solution that demonstrates mongodb transactions with eventsourcing

## Setup

1. Install MongoDb locally
2. Run the following command to set up a replica set:
```
  mongod --replSet rs0 --dbpath c:\data\rs1 --port 37017
  mongod --replSet rs0 --dbpath c:\data\rs2 --port 37018 
  mongod --replSet rs0 --dbpath c:\data\rs3 --port 37019
```
3. Run the following command to configure the replica set:
```
  mongo --port 37017
  
  config = {  
    _id: "rs0",
    members: [
      {_id: 0, host: "localhost:37017"},
      {_id: 1, host: "localhost:37018"},
      {_id: 2, host: "localhost:37019"}
    ]
}
rs.initiate(config)
```
4. Initialize the needed db and collections:
```
use usersystem
db.createCollection("events")
db.createCollection("userreadmodel")
```
## Run the solution
1. From the following folder /src/MongoDbTransactionsDemo.Api build and run the solution:
```
dotnet build
dotnet run
```
