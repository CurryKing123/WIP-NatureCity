const {
    getPos,
    postPos,
    deletePos,
    putPos,
  } = require("../api-service/mysql-db-processor");
  
  exports.getPosFromDb = async (req, res) => {
    try {
      res.json(await getPos(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postPosFromDb = async (req, res) => {
    try {
      res.json(await postPos(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putPosFromDb = async (req, res) => {
    try {
      res.json(await putPos(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deletePosFromDb = async (req, res) => {
    try {
      res.json(await deletePos(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  