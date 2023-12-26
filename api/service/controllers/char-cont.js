const {
  getChar,
  getCharById,
  postChar,
  deleteChar,
  putChar,
} = require("../api-service/mysql-db-processor");

exports.getCharFromDb = async (req, res) => {
  try {
    res.json(await getChar(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.getCharByIdFromDb = async (req, res) => {
  try {
    res.json(await getCharById(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.postCharFromDb = async (req, res) => {
  try {
    res.json(await postChar(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.putCharFromDb = async (req, res) => {
  try {
    res.json(await putChar(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.deleteCharFromDb = async (req, res) => {
  try {
    res.json(await deleteChar(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};
