var enemyReloadCounter = 0;
var enemies = [];
var Enemy = function(sprite, gun, healthSprite, health, ammo, goingRight, goingLeft, reloadCounter, alive, farLeft, farRight, firingRate) {
    this.sprite = sprite;
    this.gun = gun;
    this.healthSprite = healthSprite;
    this.health = health;
    this.ammo = ammo;
    this.goingRight = goingRight; 
    this.goingLeft = goingLeft; 
    this.reloadCounter = reloadCounter;
    this.alive = alive;
    this.farLeft = farLeft;
    this.farRight = farRight;
    this.firingRate = firingRate;
};

Enemy.prototype.enemyAmmoReplenish = function () {
	this.reloadCounter++; 
	if (this.reloadCounter === 300) {
		this.reloadCounter = 0;		
		this.ammo += 3;
	}
};

Enemy.prototype.walkRight = function() {
	this.sprite.body.velocity.x = 150;
	this.sprite.animations.play('goRight');
};

Enemy.prototype.walkLeft = function () {
	this.sprite.body.velocity.x = -150;
	this.sprite.animations.play('goLeft');
};

Enemy.prototype.damageEnemy = function (enemy, bullet) {
	this.health--;
	bullet.kill();
};

Enemy.prototype.enemyHealthMatch = function() {
	this.healthSprite.x = this.sprite.x + 5;
    this.healthSprite.y = this.sprite.y - 10;
    if (this.health > -1) {
    	this.healthSprite.frame = 7 - this.health;   	
	}
};

Enemy.prototype.attackPlayer = function () {
	this.sprite.body.velocity.x = 0;
	this.sprite.frame = 3;
};

Enemy.prototype.killEnemy = function () {
	this.sprite.kill();
    this.healthSprite.kill();
    this.gun.kill();   
    this.alive = false;
};

Enemy.prototype.facingPlayer = function(enemy) {
	if ((enemy.goingRight && player.x > enemy.sprite.x) || (enemy.goingLeft && player.x < enemy.sprite.x)) {
		return true;
	} else {
		return false;
	}
};


Enemy.prototype.checkEnemyDamage = function () {
	game.physics.overlap (this.sprite, bullets, this.damageEnemy, null, this);
};

var enemiesActionUpdate = function() {
	for (var i = 0; i < enemies.length; i++) {
        if (enemies[i].alive) {
            enemies[i].enemyHealthMatch();
            pistolMatch(enemies[i].sprite, enemies[i].gun, enemies[i].goingLeft, enemies[i].goingRight);
            if (enemies[i].goingRight === true) {
               enemies[i].walkRight();
            } 
            if (enemies[i].sprite.x >= enemies[i].farRight) {
                enemies[i].goingLeft = true;
                enemies[i].goingRight = false;
            }

            if(enemies[i].goingLeft === true) { 
                enemies[i].walkLeft();
            } 
            if (enemies[i].sprite.x <= enemies[i].farLeft) {
                enemies[i].goingRight = true;
                enemies[i].goingLeft = false;
            }

            enemies[i].enemyAmmoReplenish();
        
            if (enemies[i].health === 0) {
                 enemies[i].killEnemy();
            }
            if (enemies[i].facingPlayer(enemies[i])) {
                pistolFire(enemies[i].sprite, enemies[i].gun, enemies[i].goingLeft, enemies[i].goingRight, enemies[i].ammo, enemies[i].firingRate, 'bullet');
            }
        }
    }   
};
var enemiesPhysicsUpdate = function () {
    for (var i = 0; i < enemies.length; i++) {
        game.physics.collide(enemies[i].sprite, platforms);
    }
};

var pauseEnemies = function () {
    for (var i = 0; i < enemies.length; i++) {
        enemies[i].sprite.body.velocity.x = 0;
        enemies[i].sprite.frame = 3;
    } 
};