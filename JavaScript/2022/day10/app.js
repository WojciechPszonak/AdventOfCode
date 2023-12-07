const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(0, -1);

// Write code below this line
const getStrength = (cycles) => {
    let executed = 0;
    let x = 1;

    for (let line of lines) {
        const values = line.split(" ");
        let value = null;

        if (values.length === 2) {
            // addx
            value = +values[1];
            executed += 2;
        }
        else {
            // noop
            executed++;
        }

        if (executed >= cycles) {
            return cycles * x;
        }
        else if (value !== null) {
            x += value;
        }
    }
}

const result = getStrength(20)
            + getStrength(60)
            + getStrength(100)
            + getStrength(140)
            + getStrength(180)
            + getStrength(220);

console.group("Part One");
console.log(result);
console.groupEnd();

const getInstructions = () => {
    let runAfter = 0;
    let instructions = [];

    for (let line of lines) {
        const values = line.split(" ");

        if (values.length === 2) {
            // addx
            const value = +values[1];
            runAfter += 2;
            instructions.push([value, runAfter]);
        }
        else {
            runAfter++;
        }
    }

    return instructions;
}

const draw = (cycles) => {
    let x = 1;
    let output = "";

    const instructions = getInstructions();

    for (let i = 0; i < 6; i++) {
        for (let j = 0; j < 40; j++) {
            let cursor = i * 40 + j;

            if ([x - 1, x, x + 1].includes(j)) {
                output += "#";
            }
            else {
                output += ".";
            }

            const instruction = instructions.find(y => y[1] - 1 === cursor)
            if (instruction) {
                x += instruction[0];
            }
        }

        output += "\n";
    }

    console.log(output);
}

console.group("Part Two");
draw();
console.groupEnd();