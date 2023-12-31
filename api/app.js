const express = require("express");
const cors = require("cors");

const app = express();

const mapRouter = require("./service/routes/map-router")
const charRouter = require("./service/routes/char-router")
const resnodeRouter = require("./service/routes/resnode-router")
const resRouter = require("./service/routes/resource-router")
const charstatRouter = require("./service/routes/charstat-router")
const accRouter = require("./service/routes/account-router")
const posRouter = require("./service/routes/position-router")
const itemRouter = require("./service/routes/item-router")
const bagRouter = require("./service/routes/bag-router");
const { mysql_config } = require("./service/config");
const { getAccountFromDb } = require("./service/controllers/account-cont");
const { postAccount, getAccount } = require("./service/api-service/mysql-db-processor");

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

///Getting Account Info
app.get("/account", async (req,res) => {
    const {rUsername, rPassword} = req.query;
    if(rUsername == null || rPassword == null)
    {
        res.sendStatus("Invalid credentials");
        return;
    }

    var userAccount = .findOne(x => x.username == username);
    if(userAccount == null)
    {
        console.log("Create new account")
    }
    else
    {
        if(uPassword == userAccount.password)
        {
            userAccount.save()
            res.send(userAccount)
            console.log("Login successful")
            return
        }
    }


})

app.listen(8002, () =>{
    console.log("Server Running...")
})

module.exports = app;

//postman?