const {
    getBlacksmithInv,
    getBlacksmithInvByName,
    postBlacksmithInv,
    deleteBlacksmithInv,
    putBlacksmithInv,
  } = require("../api-service/mysql-db-processor");
  
  exports.getBlacksmithInvFromDb = async (req, res) => {
    try {
      res.json(await getBlacksmithInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getBlacksmithInvByNameFromDb = async (req, res) => {
    try {
      res.json(await getBlacksmithInvByName(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postBlacksmithInvFromDb = async (req, res) => {
    try {
      res.json(await postBlacksmithInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putBlacksmithInvFromDb = async (req, res) => {
    try {
      res.json(await putBlacksmithInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteBlacksmithInvFromDb = async (req, res) => {
    try {
      res.json(await deleteBlacksmithInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  