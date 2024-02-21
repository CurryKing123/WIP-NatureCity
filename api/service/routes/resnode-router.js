const express = require("express")
const router = express.Router()

const {
    getResNodeFromDb,
    getResNodeByIdFromDb,
    postResNodeFromDb,
    deleteResNodeFromDb,
    putResNodeFromDb
} = require("../controllers/resource_node-cont")

const { authValidator } = require("../../middleware/config-controller")

router.route("/get-resource_node").get(authValidator, getResNodeFromDb)
router.route("/get-resource_node-by-id").get(authValidator, getResNodeByIdFromDb)
router.route("/post-resource_node").post(authValidator, postResNodeFromDb)
router.route("/delete-resource_node").delete(authValidator, deleteResNodeFromDb)
router.route("/put-resource_node").put(authValidator,putResNodeFromDb)
module.exports = router;