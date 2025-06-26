# ChatbotAI

## Backend

### Requirements

Visual Studio  
Microsoft SQL Server

### Installation

Before running the app, fill appSettings.Development.json:  
```js
{
  "ConnectionStrings": {
    "MSSQL": "your_connection_string"
  },
  //...rest of the file
}
```
with path to a database of your choice.

Then run 
```bash
Update-Database
```
in Package Manager Console

### Run

To start a local development server, just hit Debug button.

Once the server is running, your frontend app should be able to communicate with this API at `https://localhost:7218/`.

## Frontend

### Requirements

Node v22  
Angular CLI v19

### Installation

Navigate to ./frontend directory in your Terminal and run:
```bash
npm i
```

### Run

To start a local development server, stay in ./frontend directory and run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`.
