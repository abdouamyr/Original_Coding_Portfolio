var preloadSprites = function () {
	game.load.image('sky', 'Assets/sky.png');
    game.load.image('ground', 'Assets/platform.png');
    game.load.image('redCross', 'Assets/health.png');
    game.load.image('pistolIcon', 'Assets/pistolicon.png');
    game.load.image('smgIcon', 'Assets/smgicon.png');
    game.load.image('crossbowIcon', 'Assets/crossbowIcon.png');
    game.load.spritesheet('bullet', 'Assets/bullet.png', 10, 3);
    game.load.spritesheet('arrow', 'Assets/arrow.png', 19, 5);
    game.load.spritesheet('pistol', 'Assets/pistol.png', 80, 16)
    game.load.spritesheet('smg', 'Assets/smg.png', 100, 16);
    game.load.spritesheet('crossbow', 'Assets/crossbow.png', 100, 16);
    game.load.spritesheet('hatter', 'Assets/Hatter.png', 32, 58);
    game.load.spritesheet('grunt','Assets/grunt.png', 32, 58);
    game.load.spritesheet('health', 'Assets/healthbar.png', 21, 4);
    game.load.spritesheet('playerhealth', 'Assets/hbar.png', 96, 20);
    game.load.spritesheet('bulletcount', 'Assets/bulleticon.png', 200, 20);	
    game.load.spritesheet('collectbullet', 'Assets/bulletitem.png', 10, 30);
    game.load.spritesheet('healthpickup', 'Assets/healthpickup.png', 20, 30);
};