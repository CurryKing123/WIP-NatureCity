const express = require("express")
const router = express.Router()

const {
    getAccountFromDb,
    postAccountFromDb,
    deleteAccountFromDb,
    putAccountFromDb
} = require("../controllers/account-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-account").get(authValidator, getAccountFromDb)
router.route("/post-account").post(authValidator, postAccountFromDb)
router.route("/delete-account").delete(authValidator, deleteAccountFromDb)
router.route("/put-account").put(authValidator,putAccountFromDb)
module.exports = router;