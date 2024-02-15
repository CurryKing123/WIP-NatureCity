const {
    getRace,
    getRaceByName,
    postRace,
    deleteRace,
    putRace,
  } = require("../api-service/mysql-db-processor");
  
  exports.getRaceFromDb = async (req, res) => {
    try {
      res.json(await getRace(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };

  exports.getRaceByNameFromDb = async (req, res) => {
    try {
      res.json(await getRaceByName(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.postRaceFromDb = async (req, res) => {
    try {
      res.json(await postRace(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.putRaceFromDb = async (req, res) => {
    try {
      res.json(await putRace(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  
  exports.deleteRaceFromDb = async (req, res) => {
    try {
      res.json(await deleteRace(req.query.page, req));
    } catch (err) {
      res.status(422).json({ message: err.message });
    }
  };
  