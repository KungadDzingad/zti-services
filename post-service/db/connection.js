const mysql = require("mysql2");

const connection = mysql.createConnection({
  host: "localhost",
  user: "root",
  // password: "password",
  port: 3306,
  database: "zti-wyklad",
});

module.exports = connection;
