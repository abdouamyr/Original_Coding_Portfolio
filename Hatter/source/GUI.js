var guiY = 10; 
var enemiesAlive = false;
var instructionCounter = 0;
var guiAlive = true;
var instructions = [
"Your Mission, should you choose to accept it...",
"Eliminate all hostiles",
"Use the \u2190 and \u2192 keys to move, \u2191	key to jump",
"Use the 'F' key to fire, and the 'Q' Key to switch weapons",
"Pick up health and bullets by pressing 'D'",
"Press 'E' when you're ready to begin"
]
var instructionText = instructions[0];
var instructionItem;
var updateBullets = function () {
	if (ammoCount < 28 && ammoCount > -1) {
		playerAmmo.frame = ammoCount;
	}
};

var addWinText = function () {
	 scoreText = game.add.text(300, 300, 'Mission Accomplished!', { fontSize: '32px', fill: 'white', stroke: "black", strokeThickness: 5 });
};

var addDefeatText = function () {
	 scoreText = game.add.text(300, 300, 'Defeated! Press "G" to try again', { fontSize: '32px', fill: 'white', stroke: "black", strokeThickness: 5 });
	 respawnKeyActive = true;
};

var checkForWin = function () {
	enemiesAlive = false;
	for (var i = 0; i < enemies.length; i++){
		if (enemies[i].alive === true) {
			enemiesAlive = true;
		}
	}
	if (enemiesAlive === false) {
		addWinText();
	}
};

var addInstructionText = function () {
	if (instructionCounter > 0) {
		instructionItem.destroy();
	}
	instructionItem = game.add.text(20, 300, 'Welcome to Hatter', { fontSize: '10px', fill: 'white', stroke: "black", strokeThickness: 7});
	keyPressItem1 = game.add.text(15, 350, 'Press "m" for next instruction', { fontSize: '10px', fill: 'grey'});
	keyPressItem2 = game.add.text(15, 430, 'Press "e" to engage', { fontSize: '10px', fill: 'grey'});
	keyPressItem3 = game.add.text(15, 390, 'Press "n" for previous', { fontSize: '10px', fill: 'grey'});
};

var changeInstructionText = function () {
	if (instructionCounter < instructions.length) {
		instructionCounter++;
    	instructionItem.content = instructions[instructionCounter-1];
	} 
};

var changeInstructionTextBack = function () {
	if (instructionCounter < instructions.length) {
		instructionCounter--;
    	instructionItem.content = instructions[instructionCounter-1];
	} 
};

var destroyGUI = function () {
	if (guiAlive) {
		guiAlive = false;
		instructionItem.content = '';
		keyPressItem1.content = '';
		keyPressItem2.content = '';
		keyPressItem3.content = '';
		console.debug('Destroyed GUI');
	}
};