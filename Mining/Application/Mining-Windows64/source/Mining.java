import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class Mining extends PApplet {

//game controlling variables 
int screenWidth = 800;
int screenHeight = 600;
int rarity = 20;
int stoneSize = 50;
int gravity = 10;
int goldLeft;
PImage gold;
PImage barrel;
int gameoverCounter = 0;
boolean gameover = false;
boolean first = true;
Stone [][] stones = new Stone [screenWidth/stoneSize][screenHeight/stoneSize];
Digger player = new Digger();
//creates the stones
public void setup () {
  size(screenWidth, screenHeight);
  for (int x = 0; x < screenWidth; x+=stoneSize) {
    for (int y = stoneSize; y < screenHeight; y+=stoneSize) {
      stones[x/stoneSize][y/stoneSize - 1] = new Stone(x, y);
    }
  }
  gold = loadImage("gold-chunk.png");
  barrel = loadImage("barrel.png");
  frameRate(60);
}


public void draw () {
  if (player.fuel <= 0)
    gameover = true;
  if (gameover) {
    gameoverCounter++;
  }
  background(255);
  if (first) {
    instructionScreen();
  } else if (gameoverCounter < 15) {
    playScreen();
  } else {
    gameOverScreen();
  }
}

public void mousePressed () {
  if (gameover &&  overlap(mouseX, mouseY, (int) (screenWidth/2 - stoneSize * 2.25f), screenHeight/2, stoneSize * 2)) {
    newGame();
  } 
  if (first && overlap(mouseX, mouseY, (int) (screenWidth/2), screenHeight/2, stoneSize * 4)) {
    first = false;
    player.x = screenWidth/2 + stoneSize/2;
    player.y = stoneSize/2;
  }
}

public void newGame () {
  player.fuel = player.maxFuel;
  player.goldCollected = 0;
  goldLeft = 0;
  player.x = screenWidth/2 + stoneSize/2;
  player.y = stoneSize/2; 
  gameoverCounter = 0;
  gameover = false;
  for (int x = 0; x < screenWidth; x+=stoneSize) {
    for (int y = stoneSize; y < screenHeight; y+=stoneSize) {
      stones[x/stoneSize][y/stoneSize - 1] = new Stone(x, y);
    }
  }
}

public void instructionScreen() {
  textSize(75);
  fill (0);
  pushMatrix();
  translate(0, - stoneSize * 4);
  if (overlap(mouseX, mouseY, (int) (screenWidth/2), screenHeight/2, stoneSize * 4) && mouseY < screenHeight/2) {
    fill(150);
  }
  text("Gold Mining", screenWidth/2 - stoneSize * 5, screenHeight/2); 
  noStroke();
  rectMode(CORNER);
  rect(screenWidth/2 - stoneSize * 5, screenHeight/2, stoneSize * 11, stoneSize * 2);
  fill(255);
  textSize(50);
  text("Click to Begin", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize); 
  popMatrix();
  fill(0);
  textSize(25);
  pushMatrix();
  translate(-stoneSize * 2, 0);
  text("- w, a, s, d to steer", screenWidth/2 - stoneSize * 5, screenHeight/2); 
  player.x = screenWidth/2 + stoneSize;
  player.y = screenHeight/2 - stoneSize/3;
  player.drawDigger(false);
  text("- collect oil barrels to stay alive and use your propulsors", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize); 
  image(barrel, screenWidth/2 + stoneSize * 9, screenHeight/2 + stoneSize/2, stoneSize, stoneSize);
  text("- collect gold to improve your drilling speed", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize * 2); 
  image(gold, screenWidth/2 + stoneSize * 6, screenHeight/2 + stoneSize * 1.25f, stoneSize, stoneSize);
  text("- collect all the gold and win", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize * 3); 
  text("- run out of fuel and you explode", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize * 4); 
  popMatrix();
}

public void playScreen () {
  player.resetCollides();
  //draws the stones
  for (int x = 0; x < screenWidth; x+=stoneSize) {
    for (int y = stoneSize; y < screenHeight; y+=stoneSize) {
      stones[x/stoneSize][y/stoneSize - 1].drawStone();
    }
  }
  //draws the player
  player.driveDigger();
  player.drawDigger(true);
}
public void gameOverScreen () {
  rectMode(CORNER);
  String gameOverText = "";
  if (goldLeft == 0) {
    gameOverText = "You Win";
  } else {
    gameOverText = "Game Over";
  }
  textSize(100);
  fill (0);

  if (overlap(mouseX, mouseY, (int) (screenWidth/2 - stoneSize * 2.25f), screenHeight/2, stoneSize * 2)) {
    fill(150);
  }
  text("Score: " + player.goldCollected * 10, screenWidth/2 - stoneSize * 5, screenHeight/2 - stoneSize * 3); 
  text("Game Over", screenWidth/2 - stoneSize * 5, screenHeight/2); 
  noStroke();
  rect(screenWidth/2 - stoneSize * 5, screenHeight/2, stoneSize * 5.5f, stoneSize * 2);
  fill(255);
  textSize(50);
  text("Play Again?", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize);
}

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
  public void drawDigger (boolean GUI) {
    int size = (int) (stoneSize * 0.75f);
    if (fuel > 0) {
      drillSpeed = 1 + goldCollected;
      fill(0);
      rectMode(CENTER);
      //main body
      rect(x, y, size, size/1.25f);
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
      text("Fuel:", screenWidth - 5.4f * size, size * 1.25f);
      text("Gold Remaining: " + goldLeft, screenWidth - stoneSize * 4, stoneSize * 0.5f);
      rectMode(CORNER);
      fill(200, 5, 5);
      float fuelBar = map (fuel, 0, maxFuel, 0, 100);
      rect(screenWidth - 3 * size, size * 0.8f, fuelBar, size/2);
    }
    rectMode(CENTER);
  }
  public void driveDigger () {
    if (fuel > 0) {
      fuel-= 0.1f;
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
  public void resetCollides () {
    collideLeft = false;
    collideRight = false;
    collideAbove = false;
    collideBelow = false;
    onGround = false;
  }
  public void propulsors () {
    int numFires = (int) random(1, 20);
    for (int i = 0; i < numFires; i++) {
      //make a particle
      int matchFill = PApplet.parseInt(random(100, 255));
      fill(matchFill, PApplet.parseInt(random(0, 255)), 0);
      noStroke();
      rect(x + (int) random(stoneSize) - stoneSize/2, y + stoneSize/3 + (int) random(stoneSize), stoneSize/5, stoneSize/5);
    }
  }
}

public boolean overlap (int x1, int y1, int x2, int y2, int radius) {
  if ( abs(x1 - x2) < radius && abs(y1 - y2) < radius) {
    return true;
  } else {
    return false;
  }
}

public boolean collide (int x1, int y1, int x2, int y2, String check, int radius) {
  if (check == "x" &&
    radius/4 > abs(x1 - x2) && radius*1.5f > abs(y1- y2)) {
    return true;
  } else if (check == "y" &&
    radius/4 > abs(y1 - y2) && radius*1.5f > abs(x1- x2)) {
    return true;
  } else {
    return false;
  }
}
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
  public void drawStone () {
    int displayHealth = (int) map(health, 0, 100, 255, 0);
    rectMode(CORNER);
    stroke(255);
    if (hasGold && !collected) {
      image(gold, x + size/8, y + size/8, (int) size * 0.75f, (int) size * 0.75f);
    } else if (fuel && !collected) {
      image(barrel, x + size/8, y + size/8, (int) size * 0.75f, (int) size * 0.75f);
    }
    fill(displayHealth, 255 - displayHealth);
    if (hasGold && !solid && !collected && overlap(x + size/8, y+size/8, player.drillX, player.drillY, size)) {
      collected = true;
      player.goldCollected++;
      goldLeft--;
    } else if (fuel && !solid && !collected && overlap(x + size/8, y+size/8, player.drillX, player.drillY, size)) {
      collected = true;
      if (player.fuel <= player.maxFuel * 0.75f) {
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
  public void stoneChunks (int displayHealth) {
    int numChunks = (int) random(1, 7);
    for (int i = 0; i < numChunks; i++) {
      //make a particle
      fill(255 - displayHealth);
      noStroke();
      rect(x + (int) random(size), y + (int) random(size), size/5, size/5);
    }
  }
}



  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "--full-screen", "--bgcolor=#666666", "--hide-stop", "Mining" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
