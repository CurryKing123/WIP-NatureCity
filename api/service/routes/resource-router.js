const express = require("express")
const router = express.Router()

const {
    getResFromDb,
    postResFromDb,
    deleteResFromDb,
    putResFromDb
} = require("../controllers/resource-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-resource").get(authValidator, getResFromDb)
router.route("/post-resource").post(authValidator, postResFromDb)
router.route("/delete-resource").delete(authValidator, deleteResFromDb)
router.route("/put-resource").put(authValidator,putResFromDb)
module.exports = router;