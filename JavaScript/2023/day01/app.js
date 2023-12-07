const read = (useTest) => {
    const fs = require("fs");
    const path = require("path");

    const fileName = useTest ? "input2.txt" : "input.txt";
    const data = fs.readFileSync(path.resolve(__dirname, fileName), "utf8");
    const lines = data.split("\n").slice(0, -1);

    return lines;
}

// Write code below this line
const parse1 = (input) => {
    return input
        .map(x => [...x])
        .map(x => (x.find(y => isNumber(y)) + x.findLast(y => isNumber(y))))
        .map(x => +x);
}

const part1 = (input) => {
    const data = parse1(input);
    return data.reduce((prev, curr) => prev += curr);
}

const parse2 = (input) => {
    return input
        .map(x => getCalibrationValue(x))
        .map(x => +x);
}

const getCalibrationValue = (text) => {
    const numbers = [
        { name: "one", value: "1" },
        { name: "two", value: "2" },
        { name: "three", value: "3" },
        { name: "four", value: "4" },
        { name: "five", value: "5" },
        { name: "six", value: "6" },
        { name: "seven", value: "7" },
        { name: "eight", value: "8" },
        { name: "nine", value: "9" }
    ];

    let minIndex;
    let maxIndex;
    let minValue;
    let maxValue;

    for (let number of numbers.flatMap(x => Object.values(x))) {
        const index = text.indexOf(number);
        const lastIndex = text.lastIndexOf(number);

        if (index === -1) {
            continue;
        }

        const value = numbers.find(x => x.name === number || x.value === number).value;

        if (minIndex === undefined || index < minIndex) {
            minIndex = index;
            minValue = value;
        }
        if (maxIndex === undefined || lastIndex > maxIndex) {
            maxIndex = lastIndex;
            maxValue = value;
        }
    }

    return minValue + maxValue;
}

const part2 = (input) => {
    const data = parse2(input);
    return data.reduce((prev, curr) => prev += curr);
}

const isNumber = (letter) => letter >= "0" && letter <= "9";

// Output:
const input = read();
console.group("Part One");
console.log(part1(input));
console.groupEnd();

console.group("Part Two");
console.log(part2(input));
console.groupEnd();