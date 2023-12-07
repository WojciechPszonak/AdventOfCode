const read = (useTest) => {
    const fs = require("fs");
    const path = require("path");

    const fileName = useTest ? "input2.txt" : "input.txt";
    const data = fs.readFileSync(path.resolve(__dirname, fileName), "utf8");
    const lines = data.split("\n").slice(0, -1);

    return lines;
}

// Write code below this line
const parse = (input) => {
    // ...
}

const part1 = (data) => {
    // ...
}

const part2 = (data) => {
    // ...
}

// Output:
const input = read();
const data = parse(input);

console.group("Part One");
console.log(part1(data));
console.groupEnd();

console.group("Part Two");
console.log(part2(data));
console.groupEnd();