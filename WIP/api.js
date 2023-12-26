import express from "express"

const api = express()
const port = 3000

api.get("/", (req, res) => {
    res.send("Hello World!");
});



