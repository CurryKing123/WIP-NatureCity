const {
    getResNode,
    getResNodeById,
    postResNode,
    deleteResNode,
    putResNode,
  } = require("../api-service/mysql-db-processor");
  
  exports.getResNodeFromDb = async (req, res) => {
    try {
      res.json(await getResNode(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getResNodeByIdFromDb = async (req, res) => {
    try {
      res.json(await getResNodeById(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postResNodeFromDb = async (req, res) => {
    try {
      res.json(await postResNode(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putResNodeFromDb = async (req, res) => {
    try {
      res.json(await putResNode(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteResNodeFromDb = async (req, res) => {
    try {
      res.json(await deleteResNode(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  