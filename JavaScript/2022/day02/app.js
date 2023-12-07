const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
const shapePoints = new Map([
    ["X", 1],
    ["Y", 2],
    ["Z", 3]
]);

const getResult = (their, mine) => {
    let outcome;

    if ((their === "A" && mine === "X")
        || (their === "B" && mine === "Y")
        || (their === "C" && mine === "Z")) {
        outcome = 3;
    }
    else if ((their === "A" && mine === "Y")
        || (their === "B" && mine === "Z")
        || (their === "C" && mine === "X")) {
        outcome = 6;
    }
    else {
        outcome = 0;
    }

    return outcome + shapePoints.get(mine);
}

let score1 = 0;
for (let line of lines) {
    var values = line.split(" ");
    score1 += getResult(values[0], values[1]);
}

console.group("Part One");
console.log(score1);
console.groupEnd();

const getCorrectResult = (their, result) => {
    /*
        X - lose
        Y - draw
        Z - win
    */
    let mine;

    if (their === "A") {
        switch (result) {
            case "X":
                mine = "Z";
                break;
            case "Y":
                mine = "X";
                break;
            case "Z":
                mine = "Y";
                break;
        }
    }
    else if (their === "B") {
        mine = result;
    }
    else {
        switch (result) {
            case "X":
                mine = "Y";
                break;
            case "Y":
                mine = "Z";
                break;
            case "Z":
                mine = "X";
                break;
        }
    }

    return getResult(their, mine);
}

let score2 = 0;
for (let line of lines) {
    var values = line.split(" ");
    score2 += getCorrectResult(values[0], values[1]);
}

console.group("Part Two");
console.log(score2);
console.groupEnd();