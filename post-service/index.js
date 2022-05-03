const express = require("express");
const postRouter = require("./api/posts");
const eurekaHelper = require("./eureka/eureka");

const PORT = 3000;

const app = express();
app.use(express.json());
app.use("/api/posts", postRouter);

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});

app.get("/", (req, res) => {
  res.send("Hello world");
});

eurekaHelper.registerWithEureka("user-service", PORT);

