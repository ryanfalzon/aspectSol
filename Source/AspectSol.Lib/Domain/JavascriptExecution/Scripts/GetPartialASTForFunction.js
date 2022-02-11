const parser = require("@solidity-parser/parser");

function injectStatementsIntoSmartContractTemplate(statements) {
    return '// SPDX-License-Identifier: GPL-3.0\n' +
    'pragma solidity >=0.7.0 <0.9.0;\n' +
    'contract TemplateContract {\n' +
    '    function functionPlaceholder() public {\n' +
    '        ${statements}\n' +
    '    }\n' +
    '}';
}

module.exports = (callback, input) => {
    let smartContractTemplate = injectStatementsIntoSmartContractTemplate(input);
    let ast = JSON.parse(parser.parse(smartContractTemplate).toString());
    
    // 1. We take the 2nd (Index 1) child of the children property as the first is the pargma directive
    // 2. We then take the 1st (Index 0) child of the subNodes proeprty, i.e. the functionPlaceholder function
    // 3. We then keep only the statements array found in the function body property
    let statementAst = ast[1].subNodes[0].body.statements;
    
    callback(null, statementAst);
};