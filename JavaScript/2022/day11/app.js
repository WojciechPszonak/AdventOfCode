const fs = require("fs");
const path = require("path");
const { create, all } = require('mathjs')

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8").slice(0, -1);

// Write code below this line
const config = {
    number: 'BigNumber',
    precision: 64,
    epsilon: 1e-60
};
const math = create(all, config);
const monkeysInput = data.split("\n\n");

class Monkey {
    inspections = 0;
    items = [];
    operation;
    divisibleBy = 1;
    ifTrue = 0;
    ifFalse = 0;
}

let monkeys = [];

// Initialize
for (let item of monkeysInput) {
    const lines = item.split("\n");

    const monkey = new Monkey();
    monkey.items = lines[1].match(/\d+/g, " ").map(x => math.bignumber(x));
    const operationExpr = lines[2].slice(lines[2].indexOf("=") + 2);

    const parser = math.parser();
    parser.evaluate(`f(old) = ${operationExpr}`);

    monkey.operation = parser.get("f");

    monkey.divisibleBy = +lines[3].match(/\d+/g)[0];
    monkey.ifTrue = +lines[4].match(/\d+/g)[0];
    monkey.ifFalse = +lines[5].match(/\d+/g)[0];

    monkeys.push(monkey);
}

const allDivisors = monkeys.map(x => x.divisibleBy).reduce((agg, curr) => agg * curr, 1);

// Play
// const playRound = () => {
//     for (let monkey of monkeys) {
//         for (let item of monkey.items) {
//             const afterInspection = monkey.operation(item);
//             const worryLevel = Math.floor(afterInspection / 3.0);

//             const newMonkey = worryLevel % monkey.divisibleBy ? monkey.ifFalse : monkey.ifTrue;
//             monkey.inspections++;

//             monkeys[newMonkey].items.push(worryLevel);
//         }

//         monkey.items = [];
//     }
// }

// for (let i = 0; i < 20; i++) {
//     playRound();
// }

// const sorted = [...monkeys].sort((a, b) => b.inspections - a.inspections);

// console.group("Part One");
// console.log(sorted[0].inspections * sorted[1].inspections);
// console.groupEnd();

const getMod = (a, n) => {
    const aString = a.toString();
    let result = math.bignumber(0);

    for (let digit of aString) {
        result = math.multiply(result, 10);
        result = math.add(result, +digit);

        result = result > 100000 ? getMod(result, n) : math.mod(result, n);
    }

    return result;
}

const playRound2 = () => {
    for (let monkey of monkeys) {
        for (let item of monkey.items) {
            const worryLevel = monkey.operation(item) % allDivisors;
            
            const mod = math.mod(worryLevel, monkey.divisibleBy);

            const newMonkey = mod == 0 ? monkey.ifTrue : monkey.ifFalse;
            monkey.inspections++;

            monkeys[newMonkey].items.push(worryLevel);
        }

        monkey.items = [];
    }
}

for (let i = 0; i < 10000; i++) {
    playRound2();
}

const sorted2 = [...monkeys].sort((a, b) => b.inspections - a.inspections);

console.group("Part Two");
console.log(sorted2[0].inspections);
console.log(sorted2[1].inspections);
console.log(sorted2[0].inspections * sorted2[1].inspections);
console.groupEnd();