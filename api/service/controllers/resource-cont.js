const {
    getRes,
    postRes,
    deleteRes,
    putRes,
  } = require("../api-service/mysql-db-processor");
  
  exports.getResFromDb = async (req, res) => {
    try {
      res.json(await getRes(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postResFromDb = async (req, res) => {
    try {
      res.json(await postRes(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putResFromDb = async (req, res) => {
    try {
      res.json(await putRes(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteResFromDb = async (req, res) => {
    try {
      res.json(await deleteRes(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  