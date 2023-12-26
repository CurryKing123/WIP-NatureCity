const express = require("express")
const router = express.Router()

const {
    getCharStatFromDb,
    postCharStatFromDb,
    deleteCharStatFromDb,
    putCharStatFromDb
} = require("../controllers/charstat-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-charstat").get(authValidator, getCharStatFromDb)
router.route("/post-charstat").post(authValidator, postCharStatFromDb)
router.route("/delete-charstat").delete(authValidator, deleteCharStatFromDb)
router.route("/put-charstat").put(authValidator,putCharStatFromDb)
module.exports = router;