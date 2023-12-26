const mysql  = require("mysql2")

async function query(sql, prepareStatement = [], config) {
    const connection = await mysql.createPool(config.db)
    return new Promise ((resolve, reject) =>{
        connection.query(sql, prepareStatement, (error, results) =>{
            if(error) reject(error);
            else {
                connection.end();
                return resolve(results)
            }
        })
    })
}

module.exports ={
    query
}

// topic to study sql injection --> this is your research task. gotta have some understanding about this
// using preparestatement one of defense mecahnisms toprotect your api service server from sql injection 