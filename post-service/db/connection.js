const mysql = require("mysql2");

const connection = mysql.createConnection({
  host: "localhost",
  user: "root",
  // password: "password",
  port: 3306,
  database: "zti_wyklad",
});

module.exports = connection;
