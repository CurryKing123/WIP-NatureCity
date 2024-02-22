const express = require("express")
const router = express.Router()

const {
    getInvFromDb,
    getInvByCountFromDb,
    getInvByIdFromDb,
    postInvFromDb,
    deleteInvFromDb,
    putInvFromDb
} = require("../controllers/inventory-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-inv").get(authValidator, getInvFromDb)
router.route("/get-inv-by-count").get(authValidator, getInvByCountFromDb)
router.route("/get-inv-by-id").get(authValidator, getInvByIdFromDb)
router.route("/post-inv").post(authValidator, postInvFromDb)
router.route("/delete-inv").delete(authValidator, deleteInvFromDb)
router.route("/put-inv").put(authValidator,putInvFromDb)
module.exports = router;