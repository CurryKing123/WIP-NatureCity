const mysql_config = {
    db: {
        connectionLimit: 1000,
        host: "localhost",
        user: "root",
        password: "mauFJcuf5dhRMQrjj",
        database: "NATURE",
        insecureAuth : true
    },
    listPerPage: 1000,
}

module.exports = {
    mysql_config
}

// check mysql node git repo for more detail set up.