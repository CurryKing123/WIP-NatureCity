const express = require("express");
const cors = require("cors");

const app = express();

const mapRouter = require("./service/routes/map-router");
const charRouter = require("./service/routes/char-router");
const resnodeRouter = require("./service/routes/resnode-router");
const resRouter = require("./service/routes/resource-router");
const charstatRouter = require("./service/routes/charstat-router");
const accRouter = require("./service/routes/account-router");
const posRouter = require("./service/routes/position-router");
const itemRouter = require("./service/routes/item-router");
const bagRouter = require("./service/routes/bag-router");
const raceRouter = require("./service/routes/race-router");
const invRouter = require("./service/routes/inventory-router");


app.use(express.json())
app.use(cors())


app.use("/map", mapRouter)
app.use("/char", charRouter)
app.use("/resource_node", resnodeRouter)
app.use("/resource", resRouter)
app.use("/charstat", charstatRouter)
app.use("/account", accRouter)
app.use("/position", posRouter)
app.use("/item", itemRouter)
app.use("/bag", bagRouter)
app.use("/race", raceRouter)
app.use("/inventory", invRouter)



app.listen(8002, () =>{
    console.log("Server Running...")
})

module.exports = app;

//postman?