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
void setup () {
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


void draw () {
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

void mousePressed () {
  if (gameover &&  overlap(mouseX, mouseY, (int) (screenWidth/2 - stoneSize * 2.25), screenHeight/2, stoneSize * 2)) {
    newGame();
  } 
  if (first && overlap(mouseX, mouseY, (int) (screenWidth/2), screenHeight/2, stoneSize * 4)) {
    first = false;
    player.x = screenWidth/2 + stoneSize/2;
    player.y = stoneSize/2;
  }
}

void newGame () {
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

void instructionScreen() {
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
  image(gold, screenWidth/2 + stoneSize * 6, screenHeight/2 + stoneSize * 1.25, stoneSize, stoneSize);
  text("- collect all the gold and win", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize * 3); 
  text("- run out of fuel and you explode", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize * 4); 
  popMatrix();
}

void playScreen () {
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
void gameOverScreen () {
  rectMode(CORNER);
  String gameOverText = "";
  if (goldLeft == 0) {
    gameOverText = "You Win";
  } else {
    gameOverText = "Game Over";
  }
  textSize(100);
  fill (0);

  if (overlap(mouseX, mouseY, (int) (screenWidth/2 - stoneSize * 2.25), screenHeight/2, stoneSize * 2)) {
    fill(150);
  }
  text("Score: " + player.goldCollected * 10, screenWidth/2 - stoneSize * 5, screenHeight/2 - stoneSize * 3); 
  text("Game Over", screenWidth/2 - stoneSize * 5, screenHeight/2); 
  noStroke();
  rect(screenWidth/2 - stoneSize * 5, screenHeight/2, stoneSize * 5.5, stoneSize * 2);
  fill(255);
  textSize(50);
  text("Play Again?", screenWidth/2 - stoneSize * 5, screenHeight/2 + stoneSize);
}

