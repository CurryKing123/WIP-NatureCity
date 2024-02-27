const express = require("express")
const router = express.Router()

const {
    getBagFromDb,
    getBagByNameFromDb,
    postBagFromDb,
    deleteBagFromDb,
    putBagFromDb
} = require("../controllers/bag-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-bag").get(authValidator, getBagFromDb)
router.route("/get-bag-by-name").get(authValidator, getBagByNameFromDb)
router.route("/post-bag").post(authValidator, postBagFromDb)
router.route("/delete-bag").delete(authValidator, deleteBagFromDb)
router.route("/put-bag").put(authValidator,putBagFromDb)
module.exports = router;