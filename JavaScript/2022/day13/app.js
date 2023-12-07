const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8").slice(0, -1);
const lines = data.split("\n").filter(x => x != "").map(x => JSON.parse(x));
const pairs = data.split("\n\n").map(x => x.split("\n"));

// Write code below this line
const compare = (left, right) => {
    if (left == undefined) {
        return -1;
    }
    else if (right == undefined) {
        return 1;
    }

    const leftIsArray = Array.isArray(left);
    const rightIsArray = Array.isArray(right);

    if (leftIsArray && rightIsArray) { // Two arrays
        let result = 0;
        let index = 0;

        while (result === 0 && (left[index] != undefined || right[index] != undefined)) {
            result = compare(left[index], right[index]);
            index++;
        }

        return result;
    }
    else if (!leftIsArray && !rightIsArray) { // Two integers
        return left < right ? -1 : (left > right ? 1 : 0);
    }
    else { // Mixed types
        if (!leftIsArray) {
            return compare([left], right);
        }
        else {
            return compare(left, [right]);
        }
    }
}

let sum = 0;

for (let i = 0; i < pairs.length; i++) {
    const pair = pairs[i];
    const left = JSON.parse(pair[0]);
    const right = JSON.parse(pair[1]);

    const result = compare(left, right);

    if (result === -1) {
        sum += i + 1;
    }
}

console.group("Part One");
console.log(sum);
console.groupEnd();

const dividerPacket1 = [[2]];
const dividerPacket2 = [[6]];

lines.push(dividerPacket1);
lines.push(dividerPacket2);
lines.sort((left, right) => compare(left, right));
const index1 = lines.indexOf(dividerPacket1) + 1;
const index2 = lines.indexOf(dividerPacket2) + 1;

console.group("Part Two");
console.log(`${index1} * ${index2} = ${index1 * index2}`);
console.groupEnd();