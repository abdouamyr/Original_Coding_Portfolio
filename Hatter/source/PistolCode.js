var left = false;
var right = false;
var firing = false;
var fireCounter = 0;
var enemyHealthNum = 0;
var bulletCounter = 0;
var buttonCounter = 0;
var buttonCounter2 = 0;
var buttonCounter3 = 0;
var buttonCounter4 = 0;
var ammoCount = 8; 
var playerGunX = 24;
var bulletItems = [];
var bulletXPos = [50, 400, 400, 600, 150, 600];
var bulletYPos = [200, 500, 350, 350, 200, 500];

var pistolFire = function (character, gun, left, right, ammo, firingRate, ammoType) {
   //bulletCounter === 1%%
   if (ammo > 0) {
        firing = true;
        if (right) {
          gun.frame = 1; 
          var bullet = bullets.create(gun.x + 80, gun.y, ammoType);
          ammo--;
          bullet.body.velocity.x = 300; 
          bullet.frame = 0;
       } else if (left) {
            gun.frame = 3;
            var bullet = bullets.create(gun.x - 10, gun.y, ammoType);
            ammo--;
            bullet.body.velocity.x = -300; 
            bullet.frame = 1;
       } else if (ammo > 1){
           gun.frame = 5;
           var bullet = bullets.create(gun.x - 8, gun.y + 5, ammoType);
           bullet.body.velocity.x = -300; 
           bullet.frame = 1;
           var bullet = bullets.create(gun.x + 78, gun.y + 5, ammoType);
           bullet.body.velocity.x = 300; 
           bullet.frame = 0;
           ammo-=2;
        } 
        if (character === player) {
          ammoCount = ammo;
        } else {
          for (var i = 0; i < enemies.length; i++){ 
            if (character = enemies[i]){
              enemies[i].ammo = ammo;
            }
          }
        }
    }
};

var pistolMatch = function (character, gun, left, right) {
    if (firing === false) {
        if (right) {
            gun.frame = 0;      
        } else if (left) {
            gun.frame = 2;
        } else {
          gun.frame = 4;
        }
    }
    if (character === player) {
      gun.x = character.x - currentWeapon.x;
    } else {
    gun.x = character.x - 24;
    }  
    gun.y = character.y + 22;  
};

var checkBulletPickUp = function () {
   game.physics.overlap (player, bulletPickUps, addBullets, null, this);
};

var addBullets = function (player, bulletItem) {
  bulletItem.kill();
  ammoCount = currentWeapon.clip;
};

var bounceBulletPickUps = function () {
  for (var i = 0; i < bulletItems.length; i++) {
      bulletItems[i].animations.play('bounce');
  }
};


var checkButton = function (button, buttonDelay){
  if (button.isDown) {
    buttonCounter++
      if (buttonCounter >= buttonDelay) {
        buttonCounter = 0;
        return true;
      } else {
        return false;
      }
  } else {
    buttonCounter = buttonDelay; 
    return true;
  }
};

var checkButton2 = function (button, buttonDelay){
  if (button.isDown) {
    button2Counter++
      if (button2Counter >= buttonDelay) {
        button2Counter = 0;
        return true;
      } else {
        return false;
      }
  } else {
    button2Counter = buttonDelay; 
    return true;
  }
};

var checkButton3 = function (button, buttonDelay){
  if (button.isDown) {
    button3Counter++
      if (button3Counter >= buttonDelay) {
        button3Counter = 0;
        return true;
      } else {
        return false;
      }
  } else {
    button3Counter = buttonDelay; 
    return true;
  }
};

var checkButton4 = function (button, buttonDelay){
  if (button.isDown) {
    button4Counter++
      if (button4Counter >= buttonDelay) {
        button4Counter = 0;
        return true;
      } else {
        return false;
      }
  } else {
    button4Counter = buttonDelay; 
    return true;
  }
};