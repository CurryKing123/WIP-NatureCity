const express = require("express")
const router = express.Router()

const {
    getGlobalInvFromDb,
    getGlobalInvByIdFromDb,
    postGlobalInvFromDb,
    deleteGlobalInvFromDb,
    putGlobalInvFromDb
} = require("../controllers/global_inventory-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-global_inv").get(authValidator, getGlobalInvFromDb)
router.route("/get-global_inv-by-id").get(authValidator, getGlobalInvByIdFromDb)
router.route("/post-global_inv").post(authValidator, postGlobalInvFromDb)
router.route("/delete-global_inv").delete(authValidator, deleteGlobalInvFromDb)
router.route("/put-global_inv").put(authValidator,putGlobalInvFromDb)
module.exports = router;