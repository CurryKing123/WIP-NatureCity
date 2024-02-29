const {
    getItem,
    getItemById,
    getItemByName,
    getItemByType,
    postItem,
    deleteItem,
    putItem,
  } = require("../api-service/mysql-db-processor");
  
  exports.getItemFromDb = async (req, res) => {
    try {
      res.json(await getItem(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getItemByIdFromDb = async (req, res) => {
    try {
      res.json(await getItemById(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getItemByNameFromDb = async (req, res) => {
    try {
      res.json(await getItemByName(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getItemByTypeFromDb = async (req, res) => {
    try {
      res.json(await getItemByType(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postItemFromDb = async (req, res) => {
    try {
      res.json(await postItem(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putItemFromDb = async (req, res) => {
    try {
      res.json(await putItem(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteItemFromDb = async (req, res) => {
    try {
      res.json(await deleteItem(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  