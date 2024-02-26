const {
    getInv,
    getInvByCount,
    getInvById,
    getInvById2,
    postInv,
    deleteInv,
    putInv,
  } = require("../api-service/mysql-db-processor");
  
  exports.getInvFromDb = async (req, res) => {
    try {
      res.json(await getInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getInvByCountFromDb = async (req, res) => {
    try {
      res.json(await getInvByCount(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getInvByIdFromDb = async (req, res) => {
    try {
      res.json(await getInvById(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postInvFromDb = async (req, res) => {
    try {
      res.json(await postInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putInvFromDb = async (req, res) => {
    try {
      res.json(await putInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteInvFromDb = async (req, res) => {
    try {
      res.json(await deleteInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  