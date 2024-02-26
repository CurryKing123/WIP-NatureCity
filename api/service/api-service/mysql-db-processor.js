const db = require("../mysql-handler/mysql-db");
const helper = require("../mysql-handler/helper");
const { mysql_config } = require("../config");
let fs = require("fs");
let query = "";

async function getMap(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/map/get-map.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function postMap(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.map_description);
  prepareStatement.push(req.body.map_effect);
  prepareStatement.push(req.body.map_productivity);
  query = fs.readFileSync("./service/api-service/sql/map/post-map.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putMap(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.map_description);
  prepareStatement.push(req.body.map_effect);
  prepareStatement.push(req.body.map_productivity);
  prepareStatement.push(req.query.map_id);
  query = fs.readFileSync("./service/api-service/sql/map/put-map.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteMap(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/map/delete-map.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.map_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Character Info



async function getChar(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/char/get-char.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}
async function getCharById(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/char/get-char-by-id.sql");
  let prepareStatement = [];  
  prepareStatement.push(req.query.user_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postChar(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.user_id);
  prepareStatement.push(req.body.user_name);
  prepareStatement.push(req.body.character_race);// id from race table
  prepareStatement.push(req.body.equip_item_1);//  id from item table
  prepareStatement.push(req.body.equip_item_2);//  id from item table
  prepareStatement.push(req.body.equip_item_3);//  id from item table
  prepareStatement.push(req.body.equip_item_4);//  id from item table
  prepareStatement.push(req.body.equip_item_5);//  id from item table
  prepareStatement.push(req.body.player_hp);// int value
  prepareStatement.push(req.body.player_stamina);//int value
  prepareStatement.push(req.body.player_status);// id from status table
  query = fs.readFileSync("./service/api-service/sql/char/post-char.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putChar(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.user_id);
  prepareStatement.push(req.body.user_name);
  prepareStatement.push(req.body.character_race);// id from race table
  prepareStatement.push(req.body.equip_item_1);//  id from item table
  prepareStatement.push(req.body.equip_item_2);//  id from item table
  prepareStatement.push(req.body.equip_item_3);//  id from item table
  prepareStatement.push(req.body.equip_item_4);//  id from item table
  prepareStatement.push(req.body.equip_item_5);//  id from item table
  prepareStatement.push(req.body.player_hp);// int value
  prepareStatement.push(req.body.player_stamina);//int value
  prepareStatement.push(req.body.player_status);// id from status table
  query = fs.readFileSync("./service/api-service/sql/char/put-char.sql");
  prepareStatement.push(req.query.char_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteChar(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/char/delete-char.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.char_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Character Stats



async function getCharStat(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/charstat/get-charstat.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function postCharStat(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.status_description);
  query = fs.readFileSync("./service/api-service/sql/charstat/post-charstat.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putCharStat(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.status_description);
  prepareStatement.push(req.query.status_type_id);
  query = fs.readFileSync("./service/api-service/sql/charstat/put-charstat.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteCharStat(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource_node/delete-charstat.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.status_type_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Resource



async function getRes(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource/get-resource.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getResById(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource/get-resource-by-id.sql");
  let prepareStatement = [];  
  prepareStatement.push(req.query.resource_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function getResByType(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource/get-resource-by-type.sql");
  let prepareStatement = [];  
  prepareStatement.push(req.query.resource_type);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postRes(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.resource_name);
  prepareStatement.push(req.body.light_cost);
  prepareStatement.push(req.body.resource_type);
  query = fs.readFileSync("./service/api-service/sql/resource/post-resource.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putRes(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.resource_name);
  prepareStatement.push(req.body.light_cost);
  prepareStatement.push(req.body.resource_type);
  prepareStatement.push(req.query.resource_id);
  query = fs.readFileSync("./service/api-service/sql/resource/put-resource.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteRes(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource/delete-resource.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.resource_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Resource Nodes



async function getResNode(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource_node/get-resource_node.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getResNodeById(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource_node/get-resource_node-by-id.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.resource_node_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postResNode(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.resource_node_name);
  prepareStatement.push(req.body.resource_amount);
  prepareStatement.push(req.body.gathering_time);
  prepareStatement.push(req.body.resource_id);
  query = fs.readFileSync("./service/api-service/sql/resource_node/post-resource_node.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putResNode(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.resource_node_name);
  prepareStatement.push(req.body.resource_amount);
  prepareStatement.push(req.body.gathering_time);
  prepareStatement.push(req.body.resource_id);
  prepareStatement.push(req.query.resource_node_id);
  query = fs.readFileSync("./service/api-service/sql/resource_node/put-resource_node.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteResNode(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/resource_node/delete-resource_node.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.resource_node_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

///account
async function getAccount(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/account/get-account.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getAccountLogin(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/account/get-account-login.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.username);
  prepareStatement.push(req.query.password);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postAccount(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.username);
  prepareStatement.push(req.body.password);
  query = fs.readFileSync("./service/api-service/sql/account/post-account.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putAccount(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.username);
  prepareStatement.push(req.body.password);
  prepareStatement.push(req.query.user_id);
  query = fs.readFileSync("./service/api-service/sql/account/put-account.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteAccount(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/account/delete-account.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.user_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



///position



async function getPos(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/position/get-pos.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function postPos(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.user_id);
  prepareStatement.push(req.body.map_id);
  prepareStatement.push(req.body.position_x);
  prepareStatement.push(req.body.position_y);
  prepareStatement.push(req.body.position_z);
  query = fs.readFileSync("./service/api-service/sql/position/post-pos.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putPos(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.user_id);
  prepareStatement.push(req.body.map_id);
  prepareStatement.push(req.body.position_x);
  prepareStatement.push(req.body.position_y);
  prepareStatement.push(req.body.position_z);
  prepareStatement.push(req.query.position_id);
  query = fs.readFileSync("./service/api-service/sql/position/put-pos.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deletePos(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/position/delete-pos.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.resource_node_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Item



async function getItem(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/item/get-item.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getItemByType(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/item/get-item-by-type.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.item_type);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postItem(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.item_type);
  prepareStatement.push(req.body.item_description);
  prepareStatement.push(req.body.item_name);
  query = fs.readFileSync("./service/api-service/sql/item/post-item.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putItem(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.item_type);
  prepareStatement.push(req.body.item_description);
  prepareStatement.push(req.body.item_name);
  prepareStatement.push(req.query.item_id);
  query = fs.readFileSync("./service/api-service/sql/item/put-item.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteItem(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/item/delete-item.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.item_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Bag



async function getBag(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/bag/get-bag.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function postBag(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.bag_name);
  prepareStatement.push(req.body.item_bag_capacity);
  query = fs.readFileSync("./service/api-service/sql/bag/post-bag.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putBag(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.bag_name);
  prepareStatement.push(req.body.item_bag_capacity);
  prepareStatement.push(req.query.bag_id);
  query = fs.readFileSync("./service/api-service/sql/bag/put-bag.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteBag(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/bag/delete-bag.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.bag_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Race



async function getRace(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/race/get-race.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getRaceByName(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/race/get-race-by-name.sql");
  let prepareStatement = [];  
  prepareStatement.push(req.query.race_name);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postRace(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.race_name);
  prepareStatement.push(req.body.move_speed);
  prepareStatement.push(req.body.resist);
  prepareStatement.push(req.body.carry_amount);
  query = fs.readFileSync("./service/api-service/sql/race/post-race.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putRace(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.race_name);
  prepareStatement.push(req.body.move_speed);
  prepareStatement.push(req.body.resist);
  prepareStatement.push(req.body.carry_amount);
  prepareStatement.push(req.query.race_id);
  query = fs.readFileSync("./service/api-service/sql/race/put-race.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteRace(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/race/del-race.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.race_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}



//Inventory



async function getInv(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/inventory/get-inv.sql");
  req.prepareStatement = [];
  return dbProcessor(page, query, req);
}

async function getInvByCount(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/inventory/get-inv-by-count.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.item_amount);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function getInvById(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/inventory/get-inv-by-id.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.char_id);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function postInv(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.char_id);
  prepareStatement.push(req.body.item_id);
  prepareStatement.push(req.body.item_amount);
  query = fs.readFileSync("./service/api-service/sql/inventory/post-inv.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function putInv(page = 1, req) {
  let prepareStatement = [];
  prepareStatement.push(req.body.item_id);
  prepareStatement.push(req.body.item_amount);
  prepareStatement.push(req.query.char_id);
  query = fs.readFileSync("./service/api-service/sql/inventory/put-inv.sql");
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}

async function deleteInv(page = 1, req) {
  query = fs.readFileSync("./service/api-service/sql/inventory/del-inv.sql");
  let prepareStatement = [];
  prepareStatement.push(req.query.item_count);
  req.prepareStatement = prepareStatement;
  return dbProcessor(page, query, req);
}





async function dbProcessor(page = 1, query, req) {
  let config = mysql_config;
  let rows = [];
  if (req.route.methods.get) {
    rows = await db.query(`${query}`, req.prepareStatement, config);
  } else {
    rows = await db.query(`${query}`, req.prepareStatement, config);
  }

  const data = helper.emptyOrRows(rows);
  const meta = { page };

  return {
    data,
    meta,
  };
}

module.exports = {
  getMap,
  postMap,
  putMap,
  deleteMap,

  getChar,
  getCharById,
  postChar,
  putChar,
  deleteChar,

  getResNode,
  getResNodeById,
  postResNode,
  putResNode,
  deleteResNode,

  getRes,
  getResById,
  getResByType,
  postRes,
  putRes,
  deleteRes,

  getCharStat,
  postCharStat,
  putCharStat,
  deleteCharStat,

  getAccount,
  getAccountLogin,
  postAccount,
  putAccount,
  deleteAccount,

  getPos,
  postPos,
  deletePos,
  putPos,

  getItem,
  getItemByType,
  postItem,
  deleteItem,
  putItem,

  getBag,
  postBag,
  deleteBag,
  putBag,

  getRace,
  getRaceByName,
  postRace,
  deleteRace,
  putRace,

  getInv,
  getInvById,
  getInvByCount,
  postInv,
  deleteInv,
  putInv
};
