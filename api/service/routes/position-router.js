const express = require("express")
const router = express.Router()

const {
    getPosFromDb,
    postPosFromDb,
    deletePosFromDb,
    putPosFromDb
} = require("../controllers/position-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-pos").get(authValidator, getPosFromDb)
router.route("/post-pos").post(authValidator, postPosFromDb)
router.route("/delete-pos").delete(authValidator, deletePosFromDb)
router.route("/put-pos").put(authValidator,putPosFromDb)
module.exports = router;