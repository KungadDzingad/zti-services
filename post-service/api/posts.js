const express = require("express");

const router = express.Router();

const connection = require("../db/connection");

router.get("/", (req, res) => {
  connection.connect((err) => {
    if (err) throw err;
    const sql = `SELECT * FROM post`;
    connection.query(sql, (err, result) => {
      if (err) throw err;
      res.send(result);
    });
  });
});

router.post("/", (req, res) => {
  const userId = req.headers["user-id"];
  connection.connect((err) => {
    if (err) throw err;
    const isoDate = new Date().toISOString();
    const mySqlDate = new Date(isoDate).toJSON().slice(0, 19).replace("T", " ");
    const sql = `INSERT INTO post (user_id, title, content, date) \
    VALUES ( \
        '${userId}', \
        '${req.body.title}', \
        '${req.body.content}', \
        '${mySqlDate}' \
    \ )`;
    connection.query(sql, (err, result) => {
      if (err) throw err;
      res.send(result);
    });
  });
});

module.exports = router;
