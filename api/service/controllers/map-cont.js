const {
  getMap,
  postMap,
  deleteMap,
  putMap,
} = require("../api-service/mysql-db-processor");

exports.getMapFromDb = async (req, res) => {
  try {
    res.json(await getMap(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.postMapFromDb = async (req, res) => {
  try {
    res.json(await postMap(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.putMapFromDb = async (req, res) => {
  try {
    res.json(await putMap(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};

exports.deleteMapFromDb = async (req, res) => {
  try {
    res.json(await deleteMap(req.query.page, req));
  } catch (err) {
    res.status(422).json({ message: err.message });
  }
};
