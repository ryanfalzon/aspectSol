const parser = require('@solidity-parser/parser');

module.exports = (callback, input) => {
    callback(null, parser.parse(input));
};