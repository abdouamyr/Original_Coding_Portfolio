//the class for a stone
class Stone {
  boolean solid, rich, collected, hasGold, fuel;
  int x, y, size, health;
  Stone (int xPos, int yPos) {
    x = xPos;
    y = yPos;
    size = stoneSize;
    solid = true;
    collected = false;
    //determines whether there is something to mine from the stone
    int isRich = (int) random(0, rarity);
    if (isRich == 1 || isRich == 3) {
      rich = true;
      hasGold = true;
      fuel = false;
      goldLeft++;
    } else if (isRich%2 == 0){
      rich = true;
      fuel = true;
      hasGold = false;
    } else {
      rich = false;
      hasGold = false;
      fuel = false;
    }
    health = 100;
  }
  //funciton to draw the stone
  void drawStone () {
    int displayHealth = (int) map(health, 0, 100, 255, 0);
    rectMode(CORNER);
    stroke(255);
    if (hasGold && !collected) {
      image(gold, x + size/8, y + size/8, (int) size * 0.75, (int) size * 0.75);
    } else if (fuel && !collected) {
      image(barrel, x + size/8, y + size/8, (int) size * 0.75, (int) size * 0.75);
    }
    fill(displayHealth, 255 - displayHealth);
    if (hasGold && !solid && !collected && overlap(x + size/8, y+size/8, player.drillX, player.drillY, size)) {
      collected = true;
      player.goldCollected++;
      goldLeft--;
    } else if (fuel && !solid && !collected && overlap(x + size/8, y+size/8, player.drillX, player.drillY, size)) {
      collected = true;
      if (player.fuel <= player.maxFuel * 0.75) {
        player.fuel+= player.maxFuel/4;
      } else {
       player.fuel = player.maxFuel; 
      }
    }
    //checks for whether the player is standing on it
    if (solid && collide(x + size/2, y, player.treadX, player.treadY+size/16, "y", size/2)) {
      //      fill(255, 0, 0);
      player.onGround = true;
      player.collideBelow = true;
    }
    //checks for whether the player is colliding
    //left
    if (solid && collide(x, y, player.x + size/2, player.y - size/2, "x", size/2)) {
      //      fill(0, 255, 0);
      player.collideRight = true;
    }
    //right
    if (solid && collide(x + size, y, player.x - size/2, player.y - size/2, "x", size/2)) {
      //      fill(0, 255, 0);
      player.collideLeft = true;
    }
    //bottom
    if (solid && collide(x + size/2, y+size, player.x, player.y - size/2, "y", size/2)) {
      //      fill(0, 0, 255);
      player.collideAbove = true;
    }
    rect(x, y, size, size);
    if (overlap(x + size/2, y + size/2, player.drillX, player.drillY, size/2) && health > 0) {
      if (health > player.drillSpeed) {
        health-= player.drillSpeed;
      } else {
       health = 0; 
      }
      stoneChunks(displayHealth);
    }
    if (health == 0) {
      solid = false;
    }
    if (solid) {
      fill(0, 0, 255);
//      rect(x, y, size/8, size/8);
    }
    if (hasGold) {
      fill(255, 0, 0);
//      rect(x, y, size/8, size/8);
    }
    
  }
  void stoneChunks (int displayHealth) {
    int numChunks = (int) random(1, 7);
    for (int i = 0; i < numChunks; i++) {
      //make a particle
      fill(255 - displayHealth);
      noStroke();
      rect(x + (int) random(size), y + (int) random(size), size/5, size/5);
    }
  }
}



