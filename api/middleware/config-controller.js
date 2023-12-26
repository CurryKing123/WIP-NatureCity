
exports.authValidator = async (req, res, next) =>{

    if(req.headers["key"] !== "1"){
        console.log(req.headers["key"])
        return res.status(403).send("the key is not valid")
    }

    return next();
}