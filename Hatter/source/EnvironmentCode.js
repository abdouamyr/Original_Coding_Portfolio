var addEnvironmentSprites = function() {
	 //  A simple background for our game
    game.add.sprite(0, 0, 'sky');
 
    //  The platforms group contains the ground and the 2 ledges we can jump on
    platforms = game.add.group();
    bullets = game.add.group();
    bulletPickUps = game.add.group();
    healthPickUps = game.add.group();
    // Here we create the ground.
    var ground = platforms.create(0, game.world.height - 64, 'ground');
 
    //  Scale it to fit the width of the game (the original sprite is 400x32 in size)
    ground.scale.setTo(2, 2);
 
    //  This stops it from falling away when you jump on it
    ground.body.immovable = true;
 
    //  Now let's create two ledges
    var ledge = platforms.create(400, 400, 'ground');
    ledge.body.immovable = true;
 
    var ledge = platforms.create(-150, 250, 'ground');
    ledge.body.immovable = true;		
};