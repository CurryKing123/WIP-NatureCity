const {
    getGlobalInv,
    getGlobalInvById,
    postGlobalInv,
    deleteGlobalInv,
    putGlobalInv,
  } = require("../api-service/mysql-db-processor");
  
  exports.getGlobalInvFromDb = async (req, res) => {
    try {
      res.json(await getGlobalInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getGlobalInvByIdFromDb = async (req, res) => {
    try {
      res.json(await getGlobalInvById(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postGlobalInvFromDb = async (req, res) => {
    try {
      res.json(await postGlobalInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putGlobalInvFromDb = async (req, res) => {
    try {
      res.json(await putGlobalInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteGlobalInvFromDb = async (req, res) => {
    try {
      res.json(await deleteGlobalInv(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  