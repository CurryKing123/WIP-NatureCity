let fs = require("fs");

function getOffset(currentPage = 1, listPerPage) {
    return (currentPage - 1) * [listPerPage]
}

function emptyOrRows(rows) {
    if (!rows){
        return [];
    }
    return rows;
}

function filesInExcutableOrder(path){
    let files = [];
    const dir = fs.opendirSync(path);
    let dirent;
    while((dirrent = dir.readSync()) !== null){
        files.push(dirent.name);
    }
    files.sort();
    return files;
}

module.exports = {
    getOffset,
    emptyOrRows,
    filesInExcutableOrder
}