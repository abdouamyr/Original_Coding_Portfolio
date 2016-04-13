var createGameItems = function () {
	var enemyXPos = [200, 400, 100];
    var enemyYPos = [game.world.height-120, game.world.height-260, game.world.height-410];
    var enemyFarLeft = [100, 500, 25];
    var enemyFarRight = [700, 700, 200];
    var enemyGun = [];
    var grunt = [];
    var enemyHealth = [];
    
    addEnvironmentSprites();
    
    for (var i = 0; i < enemyYPos.length; i++) {
        grunt[i] = game.add.sprite(enemyXPos[i], enemyYPos[i], 'grunt');
        enemyHealth[i] = game.add.sprite(enemyXPos[i], enemyYPos[i]+20, 'health');
        enemyGun[i] = game.add.sprite(enemyXPos[i]-13, enemyYPos[i]+20, 'pistol');
        enemies[i] = new Enemy (grunt[i], enemyGun[i], enemyHealth[i], 7, 3, false, true, 0, true, enemyFarLeft[i], enemyFarRight[i], 15);
    }

    for (var i = 0; i < bulletXPos.length; i++) {
    bulletItems[i] = bulletPickUps.create(bulletXPos[i], bulletYPos[i], 'collectbullet');
        bulletItems[i].animations.add('bounce', [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20], 30, true);
    }

    for (var i = 0; i < healthItemXPos.length; i++) {
        healthItems[i] = healthPickUps.create(healthItemXPos[i], healthItemYPos[i], 'healthpickup'); 
        healthItems[i].animations.add('bounce', [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24], 30, true);
    }
   
    for (var i = 0; i < enemies.length; i++) {
        enemies[i].sprite.body.bounce.y = 0;
        enemies[i].sprite.body.gravity.y = 10;
        enemies[i].sprite.body.collideWorldBounds = true;
        enemies[i].enemyHealthMatch();
     
        //  Our two animations, walking left and right.
        enemies[i].sprite.animations.add('goLeft', [4, 5, 6], 6, true);
        enemies[i].sprite.animations.add('goRight', [0, 1, 2], 6, true);
    }

    createPlayer();

    fireKey = game.input.keyboard.addKey(Phaser.Keyboard.F);
    weaponSwitchKey = game.input.keyboard.addKey(Phaser.Keyboard.Q);
    testKey = game.input.keyboard.addKey(Phaser.Keyboard.W);
    pickUpKey = game.input.keyboard.addKey(Phaser.Keyboard.D);
    pauseKey = game.input.keyboard.addKey(Phaser.Keyboard.P);
    resumeKey = game.input.keyboard.addKey(Phaser.Keyboard.E);
    menuKey = game.input.keyboard.addKey(Phaser.Keyboard.M);
    menuBackKey = game.input.keyboard.addKey(Phaser.Keyboard.N);
    respawnKey = game.input.keyboard.addKey(Phaser.Keyboard.G);
    cursors = game.input.keyboard.createCursorKeys();
    pistolMatch(player, playerGun, playerLeft, playerRight);
    addInstructionText();
};