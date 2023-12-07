const read = (useTest) => {
    const fs = require("fs");
    const path = require("path");

    const fileName = useTest ? "input2.txt" : "input.txt";
    const data = fs.readFileSync(path.resolve(__dirname, fileName), "utf8");
    const lines = data.split("\n").slice(0, -1);

    return lines;
}

// Write code below this line
const isSymbol = (array, row, column) => {
    const arrayRow = array[row];
    if (!arrayRow) {
        return false;
    }

    const item = arrayRow[column];

    return item && item !== "." && isNaN(+item);
}

const isPart = (array, row, startColumn, endColumn) => {
    for (let i = startColumn - 1; i <= endColumn + 1; i++) {
        for (let j = row - 1; j <= row + 1; j++) {
            if (isSymbol(array, j, i)) {
                return true;
            }
        }
    }

    return false;
}

const parse1 = (input) => {
    const array = input.map(x => [...x]);
    const result = [];

    for (let row = 0; row < input.length; row++) {
        const line = input[row];
        const matches = line.matchAll(/\d+/gd);

        for (let match of matches) {
            if (isPart(array, row, match.indices[0][0], match.indices[0][1] - 1)) {
                result.push(+match[0]);
            }
        }
    }

    return result;
}

const part1 = (input) => {
    const parts = parse1(input);

    return parts.reduce((prev, curr) => prev += curr);
}

const getGearRatio = (array, row, column) => {
    const parts = [];

    for (let j = row - 1; j <= row + 1; j++) {
        const arrayRow = array[j];

        if (!arrayRow) {
            continue;
        }

        const line = arrayRow.join("");
        const matches = line.matchAll(/\d+/gd);

        for (let match of matches) {
            const startIndex = match.indices[0][0];
            const endIndex = match.indices[0][1] - 1;

            if (column - 1 <= endIndex && column + 1 >= startIndex) {
                parts.push(+match[0]);
            }
        }
    }

    return parts.length == 2 ? parts[0] * parts[1] : null;
}

const parse2 = (input) => {
    const array = input.map(x => [...x]);
    const result = [];

    for (let row = 0; row < input.length; row++) {
        const line = input[row];
        const matches = line.matchAll(/\*/g);

        const ratios = [...matches]
            .map(match => getGearRatio(array, row, match.index))
            .filter(x => x !== null);

        result.push(...ratios);
    }

    return result;
}

const part2 = (input) => {
    const ratios = parse2(input);

    return ratios.reduce((prev, curr) => prev += curr);
}

// Output:
const input = read();

console.group("Part One");
console.log(part1(input));
console.groupEnd();

console.group("Part Two");
console.log(part2(input));
console.groupEnd();