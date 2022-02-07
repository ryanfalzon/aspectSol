// SPDX-License-Identifier: GPL-3.0

pragma solidity >=0.7.0 <0.9.0;

contract StorageA is Space {

    uint256 numberA;

    function store(uint256 num) public {
        numberA = num;
    }

    function retrieve() public view returns (uint256){
        return numberA;
    }
}

contract StorageB {

    uint256 numberB;
    StorageA storageA;

    function storeAnother(uint256 num) public {
        numberB = num;
    }

    function retrieveAnother() public view returns (uint256){
        return numberB;
    }

    function add(uint256 number1, uint256 number2) private pure onlyOwner returns (uint256) {
        return number1 + number2;
    }

    function areEqual(uint256 number1, uint256 number2) private pure returns (bool) {
        return number1 == number2;
    }

    function storeInStorageA() private view returns (uint256) {
        storageA.store(numberB);
        return numberB;
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }
}

interface Space {
    function store(uint256 num) public;
    function retrieve() public view returns(uint256);
}