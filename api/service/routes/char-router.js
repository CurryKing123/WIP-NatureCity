const express = require("express");
const router = express.Router();

const {
  getCharFromDb,
  getCharByIdFromDb,
  postCharFromDb,
  deleteCharFromDb,
  putCharFromDb,
} = require("../controllers/char-cont");

const { authValidator } = require("../../middleware/config-controller");

router.route("/get-char").get(authValidator, getCharFromDb);
router.route("/get-char-by-id").get(authValidator, getCharByIdFromDb);
router.route("/post-char").post(authValidator, postCharFromDb);
router.route("/delete-char").delete(authValidator, deleteCharFromDb);
router.route("/put-char").put(authValidator, putCharFromDb);

module.exports = router;
