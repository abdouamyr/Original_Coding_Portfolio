var healthItems = [];
var healthItemXPos = [25, 650, 700];
var healthItemYPos = [200, 500, 350];

var checkHealthPickUp = function () {
   game.physics.overlap (player, healthPickUps, addHealth, null, this);
};

var addHealth = function (player, healthItem) {
  healthItem.kill();
  playerHealthNum = 9;
};

var bounceHealthPickUps = function () {
	for (var i = 0; i < healthItems.length; i++) {
		healthItems[i].animations.play('bounce');
	}
}