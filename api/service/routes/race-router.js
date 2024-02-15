const express = require("express")
const router = express.Router()

const {
    getRaceFromDb,
    getRaceByNameFromDb,
    postRaceFromDb,
    deleteRaceFromDb,
    putRaceFromDb
} = require("../controllers/race-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-race").get(authValidator, getRaceFromDb)
router.route("/get-race-by-name").get(authValidator, getRaceByNameFromDb)
router.route("/post-race").post(authValidator, postRaceFromDb)
router.route("/delete-race").delete(authValidator, deleteRaceFromDb)
router.route("/put-race").put(authValidator,putRaceFromDb)
module.exports = router;