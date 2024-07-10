const express = require("express")
const router = express.Router()

const {
    getBlacksmithInvFromDb,
    getBlacksmithInvByNameFromDb,
    postBlacksmithInvFromDb,
    deleteBlacksmithInvFromDb,
    putBlacksmithInvFromDb
} = require("../controllers/blacksmith_inventory-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-blacksmith_inv").get(authValidator, getBlacksmithInvFromDb)
router.route("/get-blacksmith_inv-by-name").get(authValidator, getBlacksmithInvByNameFromDb)
router.route("/post-blacksmith_inv").post(authValidator, postBlacksmithInvFromDb)
router.route("/delete-blacksmith_inv").delete(authValidator, deleteBlacksmithInvFromDb)
router.route("/put-blacksmith_inv").put(authValidator,putBlacksmithInvFromDb)
module.exports = router;