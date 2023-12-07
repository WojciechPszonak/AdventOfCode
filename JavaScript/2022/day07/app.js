const fs = require("fs");
const path = require("path");

const data = fs.readFileSync(path.resolve(__dirname, "input.txt"), "utf8");
const lines = data.split("\n").slice(1, -1);

// Write code below this line
class TreeNode {
    constructor(name, parent) {
        this.name = name;
        this.parent = parent;
    }
}

class File extends TreeNode {
    constructor(name, size, parent) {
        super(name, parent);
        this.size = size;
    }
}

class Directory extends TreeNode {
    constructor(name, parent) {
        super(name, parent);
        this.children = [];
    }

    get size() {
        return this.children.reduce((agg, curr) => agg += curr.size, 0);
    }
}

const root = new Directory("/", null);
let treeItems = [root];
let currentNode = root;

for (let line of lines) {
    const values = line.split(" ");

    if (values[0] == "$") {// is command 
        if (values[1] == "ls") { // ls
            continue;
        }
        else { // cd
            if (values[2] == "..") {
                currentNode = currentNode.parent;
            }
            else {
                currentNode = currentNode.children.find(x => x.name == values[2]);
            }
        }
    }
    else { // is ls output
        if (!isNaN(values[0])) { // is file
            const file = new File(values[1], +values[0], currentNode);
            currentNode.children.push(file);
            treeItems.push(file);
        }
        else { // is directory
            const directory = new Directory(values[1], currentNode);
            currentNode.children.push(directory);
            treeItems.push(directory);
        }
    }
}

const result = treeItems.filter(x => x instanceof Directory && x.size <= 100000)
                        .reduce((agg, curr) => agg += curr.size, 0);

console.group("Part One");
console.log(result);
console.groupEnd();

const totalDiskSpace = 70000000;
const targetUnusedSpace = 30000000;

const currentUnusedSpace = totalDiskSpace - root.size;
const spaceToBeDeleted = targetUnusedSpace - currentUnusedSpace;

const result2 = treeItems.filter(x => x instanceof Directory)
                         .sort((a, b) => a.size - b.size)
                         .find(x => x.size >= spaceToBeDeleted)
                         .size;

console.group("Part Two");
console.log(result2);
console.groupEnd();