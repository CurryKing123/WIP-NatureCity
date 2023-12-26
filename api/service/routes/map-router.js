const express = require("express")
const router = express.Router()

const {
    getMapFromDb,
    postMapFromDb,
    deleteMapFromDb,
    putMapFromDb
} = require("../controllers/map-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-map").get(authValidator, getMapFromDb)
router.route("/post-map").post(authValidator, postMapFromDb)
router.route("/delete-map").delete(authValidator, deleteMapFromDb)
router.route("/put-map").put(authValidator,putMapFromDb)
module.exports = router;