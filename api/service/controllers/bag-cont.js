const {
    getBag,
    getBagByName,
    postBag,
    deleteBag,
    putBag,
  } = require("../api-service/mysql-db-processor");
  
  exports.getBagFromDb = async (req, res) => {
    try {
      res.json(await getBag(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getBagByNameFromDb = async (req, res) => {
    try {
      res.json(await getBagByName(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postBagFromDb = async (req, res) => {
    try {
      res.json(await postBag(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putBagFromDb = async (req, res) => {
    try {
      res.json(await putBag(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteBagFromDb = async (req, res) => {
    try {
      res.json(await deleteBag(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  