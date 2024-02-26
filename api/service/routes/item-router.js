const express = require("express")
const router = express.Router()

const {
    getItemFromDb,
    getItemByTypeFromDb,
    postItemFromDb,
    deleteItemFromDb,
    putItemFromDb
} = require("../controllers/item-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-item").get(authValidator, getItemFromDb)
router.route("/get-item-by-type").get(authValidator, getItemByTypeFromDb)
router.route("/post-item").post(authValidator, postItemFromDb)
router.route("/delete-item").delete(authValidator, deleteItemFromDb)
router.route("/put-item").put(authValidator,putItemFromDb)
module.exports = router;