const {
    getAccount,
    postAccount,
    deleteAccount,
    putAccount,
  } = require("../api-service/mysql-db-processor");
  
  exports.getAccountFromDb = async (req, res) => {
    try {
      res.json(await getAccount(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postAccountFromDb = async (req, res) => {
    try {
      res.json(await postAccount(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putAccountFromDb = async (req, res) => {
    try {
      res.json(await putAccount(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteAccountFromDb = async (req, res) => {
    try {
      res.json(await deleteAccount(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  