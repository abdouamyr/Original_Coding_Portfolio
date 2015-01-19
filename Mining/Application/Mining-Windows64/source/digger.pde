class Digger {
  boolean digging, onGround, collideLeft, collideRight, collideAbove, collideBelow;
  String facing; 
  int x, y, drillX, drillY, treadX, treadY, speed, drillSpeed, goldCollected;
  float fuel, maxFuel;
  Digger () {
    digging = false;
    facing = "RIGHT";
    x = screenWidth/2 + stoneSize/2;
    y = stoneSize/2;
    speed = 3;
    goldCollected = 0;
    maxFuel = 100;
    fuel = maxFuel;
  }
  void drawDigger (boolean GUI) {
    int size = (int) (stoneSize * 0.75);
    if (fuel > 0) {
      drillSpeed = 1 + goldCollected;
      fill(0);
      rectMode(CENTER);
      //main body
      rect(x, y, size, size/1.25);
      ellipse(x, y + size/2, size, size/3);
      treadX = x;
      treadY = y + size/2;
      //details 
      fill(255);
      ellipse(x, y + size/2, size/5, size/5);
      ellipse(x + size/3, y + size/2, size/6, size/6);
      ellipse(x - size/3, y + size/2, size/6, size/6);
      //drill
      float drillColor = map (goldCollected, 0, goldCollected + goldLeft, 0, 255);
      if (facing == "RIGHT") {
        rect (x + size/4, y - size/8, size/4, size/4);
        drillX = x + size;
        drillY = y;
        fill(drillColor, drillColor, 0);
        triangle(x+ size, y, x + size/2, y + size/3, x + size/2, y - size/3);
      } else if (facing == "LEFT") {
        rect (x - size/4, y - size/8, size/4, size/4);
        drillX = x - size;
        drillY = y;
        fill(drillColor, drillColor, 0);
        triangle(x- size, y, x - size/2, y + size/3, x-size/2, y - size/3);
      } else if (facing == "DOWN") {
        rect (x, y - size/8, size/4, size/4);
        drillX = x;
        drillY = y + stoneSize;
        fill(drillColor, drillColor, 0);
        triangle(x, y + size, x - size/2, y + size/3, x + size/2, y + size/3);
      } else if (facing == "UP") {
        rect (x, y - size/8, size/4, size/4);
        drillX = x;
        drillY = y - size;
        fill(drillColor, drillColor, 0);
        triangle(x, y - size, x - size/2, y - size/2, x + size/2, y - size/2);
      }
    } else {
      pushMatrix();
      translate(0, -stoneSize);
      propulsors(); 
      popMatrix();
    }
    if (GUI) {
      fill(0);
      textSize(20);
      text("Fuel:", screenWidth - 5.4 * size, size * 1.25);
      text("Gold Remaining: " + goldLeft, screenWidth - stoneSize * 4, stoneSize * 0.5);
      rectMode(CORNER);
      fill(200, 5, 5);
      float fuelBar = map (fuel, 0, maxFuel, 0, 100);
      rect(screenWidth - 3 * size, size * 0.8, fuelBar, size/2);
    }
    rectMode(CENTER);
  }
  void driveDigger () {
    if (fuel > 0) {
      fuel-= 0.1;
      if (!onGround && y < screenHeight - stoneSize/2) {
        y+= 3;
      }
      if (keyPressed) {
        if (key == 'd' && x < screenWidth) {
          if (!collideRight) {
            x+=speed;
          }
          facing = "RIGHT";
        } else if (key == 'a' && x > 0) {
          if (!collideLeft) {
            x-=speed;
          }
          facing = "LEFT";
        } else if (key == 's' && y < screenHeight) {
          if (!collideBelow) {
            y++;
          }
          facing = "DOWN";
        } else if (key == 'w' && y > 0) {
          if (!collideAbove && fuel > 0) {
            y-= 10;
            fuel--;
            propulsors();
          }
          facing = "UP";
        }
      }
    }
  }
  void resetCollides () {
    collideLeft = false;
    collideRight = false;
    collideAbove = false;
    collideBelow = false;
    onGround = false;
  }
  void propulsors () {
    int numFires = (int) random(1, 20);
    for (int i = 0; i < numFires; i++) {
      //make a particle
      int matchFill = int(random(100, 255));
      fill(matchFill, int(random(0, 255)), 0);
      noStroke();
      rect(x + (int) random(stoneSize) - stoneSize/2, y + stoneSize/3 + (int) random(stoneSize), stoneSize/5, stoneSize/5);
    }
  }
}

