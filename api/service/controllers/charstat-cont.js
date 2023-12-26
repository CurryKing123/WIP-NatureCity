const {
    getCharStat,
    postCharStat,
    deleteCharStat,
    putCharStat,
  } = require("../api-service/mysql-db-processor");
  
  exports.getCharStatFromDb = async (req, res) => {
    try {
      res.json(await getCharStat(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postCharStatFromDb = async (req, res) => {
    try {
      res.json(await postCharStat(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putCharStatFromDb = async (req, res) => {
    try {
      res.json(await putCharStat(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteCharStatFromDb = async (req, res) => {
    try {
      res.json(await deleteCharStat(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  