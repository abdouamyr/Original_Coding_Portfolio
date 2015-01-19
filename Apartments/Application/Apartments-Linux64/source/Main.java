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

public class Main extends PApplet {

//runs at function start
public void setup () {
  //sets the draw settings
  rectMode(CENTER);
  size(screenWidth, screenHeight);
  translate(0, apartmentHeight);
  fill(0);
  stroke(255);

  //enables key presses 
  keyPressCounter = keyPressDelay;

  //creates the apartments and elevator
  for (int y = 0; y < numApartments; y ++) {
    apartments[y] = new Floor(y);
  }
  elevator1 = new Elevator (screenWidth/2 - apartmentScale, apartments[49]);

  //sets the camera parameters
  setMaxHeight();

  //loads the button images
  loadImages();
}
public void draw () {
  //updates camera parameters 
  setCameraBounds();

  //draws the white background
  background(255);

  //performs the translations
  pushMatrix();
  translate(cameraX, cameraY); 

  //updates the scaling variables
  findOnScreenApartment();
  apartmentHeight = apartmentScale * numApartments;

  //draws the apartments and elevator
  for (int y = 0; y < numApartments; y++) {
    apartments[y].drawFloor();
  }
  elevator1.moveElevator();
  elevator1.drawElevator();

  //ends the translations 
  popMatrix();

  //draws the GUI
  drawGUI();
}

Floor middleAppt;
Float oldYPos;
public void setMaxHeight () {
  if (apartmentScale == 100) {
    maxCameraHeight = 100;
  } else if (apartmentScale == 200) {
    maxCameraHeight = 5400;
  } else if (apartmentScale == 400) {
    maxCameraHeight = 15200;
  }
  //  } else if (apartmentScale == 50) {
  //    maxCameraHeight = -2400;
  //  } else if (apartmentScale == 25) {
  //    maxCameraHeight = -3700;
  //  }
  //  if (cameraY < -1 *(apartments[49].refY * apartmentScale)) {
  //     cameraY = -1 * (apartments[49].refY * apartmentScale);
  //   }
}
public void setCameraBounds () {
  if (cameraY > maxCameraHeight) {
    cameraY = maxCameraHeight;
  }
}

public void findOnScreenApartment () {
  middleAppt = apartments[0];
  float middleFloor = abs(apartments[0].posY + cameraY - screenHeight/2);
  for (int i = 0; i < apartments.length; i++) {
    apartments[i].onScreen = false;
    if (middleFloor > abs(apartments[i].posY + cameraY - screenHeight/2)) {
      middleFloor =  abs(apartments[i].posY + cameraY - screenHeight/2);
      middleAppt = apartments[i];
    }
  }
  oldYPos = (float) middleAppt.posY;
  middleAppt.onScreen = true;
}

public void scaleForZoom () {
  cameraY += oldYPos - (middleAppt.refY * apartmentScale);  
  //  if (apartmentScale > 50) {
  //    cameraY += oldYPos - (middleAppt.refY * apartmentScale);  
  //  } else if (apartmentScale <= 50) {
  //    cameraY -= (middleAppt.refY * 100) - oldYPos;  
  //  }
}

public void processInput() {
  //handles key input
  resetButtons();
  if (keyPressed) {
    boolean inputEnabled = false;
    if (keyPressCounter == keyPressDelay) {
      keyPressCounter = 0; 
      inputEnabled = true;
    } else {
      keyPressCounter++;
    }
    if (inputEnabled) {
      elevator1.setFloor();
    }
    if (key == 'w' || keyCode == UP) {
      upNum = 1;
      if (cameraY < maxCameraHeight) {
        cameraY+= scrollSpeed;
      }
    } else if (key == 's' || keyCode == DOWN) {
      downNum = 1;
      if ((apartmentHeight + cameraY - screenHeight) * 100/apartmentScale > 0) {
        cameraY-= scrollSpeed;
      }
    } else if (key == 'a' || keyCode == LEFT) {
      cameraX += scrollSpeed;
      leftNum = 1;
    } else if (key == 'd' || keyCode == RIGHT) {
      cameraX -= scrollSpeed;
      rightNum = 1;
    } else if (key == '-' && inputEnabled && apartmentScale > minApartmentScale) {
      apartmentScale/= zoomSpeed;
      scaleForZoom();
      setMaxHeight();
    } else if (key == '='&& inputEnabled && apartmentScale < maxApartmentScale) {
      apartmentScale*= zoomSpeed;
      scaleForZoom();
      setMaxHeight();
    }
  }
  if (mousePressed) {
    if (upButtonClicked()) {
      upNum = 1;
      if (cameraY < maxCameraHeight) {
        cameraY+= scrollSpeed;
      }
    }
    if (downButtonClicked()) {
      downNum = 1;
      if ((apartmentHeight + cameraY - screenHeight) * 100/apartmentScale > 0) {
        cameraY-= scrollSpeed;
      }
    }
    if (leftButtonClicked()) {
      leftNum = 1;
      cameraX += scrollSpeed;
    }
    if (rightButtonClicked()) {
      rightNum = 1;
      cameraX -= scrollSpeed;
    }
  }
}

class Elevator {
  int x, floorNumber;
  float y, startY, toMove;
  float percent, speed;
  boolean unloaded = false;
  boolean floorInputInProgress = false;
  Floor currentApartment, lastUnloadFloor;
  ArrayList<Person> riders = new ArrayList<Person>();
  Elevator (int xPos, Floor startFloor) {
    currentApartment = startFloor;
    lastUnloadFloor = currentApartment;
    x = xPos;
    y = currentApartment.posY;
    startY = y;
    toMove = y;
    percent = 0;
    speed = 0.005f;
    floorNumber = 2;
  }  
  public void moveElevator () { 
    if (lastUnloadFloor != currentApartment) {
      unloaded = false;
    }
    if (keyPressed && keyCode == ALT) {
      unload();
    }
    if (!floorInputInProgress) {
      if (currentApartment != apartments[50 - floorNumber]) {
        currentApartment.elevator = false;
        currentApartment = apartments[50 - floorNumber];
        startY = y;
        toMove = currentApartment.posY - startY;
        percent = 0;
      }
      if (percent <= 1.0f) {
        percent+= speed;
        y = startY + (toMove * percent);
      } else {
        y = currentApartment.posY;  
        currentApartment.elevator = true;
        //        unload();
      }
    }
    x = screenWidth/2 - apartmentScale;
  }

  public void drawElevator () {

    textSize(0.4f * apartmentScale);
    strokeWeight(0.05f * apartmentScale);
    stroke(0);
    line(x - apartmentScale/2, -apartmentScale/2, x + apartmentScale/2, -apartmentScale/2);
    line(x - apartmentScale/2, apartmentHeight - apartmentScale/2, x + apartmentScale/2, apartmentHeight - apartmentScale/2);
    line (screenWidth/2 - apartmentScale, -apartmentScale/2, screenWidth/2 - apartmentScale, apartmentHeight - apartmentScale/2);
    stroke(255);
    strokeWeight(1);
    fill(0);
    stroke(255);
    rect(x, y, apartmentScale, apartmentScale); 
    if (floorInputInProgress) {
      fill(255, 0, 0);
    } else {
      fill(0, 255, 0);
    }
    rect(x - apartmentScale/4, y - apartmentScale/4, apartmentScale/8, apartmentScale/8);
    fill(255);
    text(floorNumber, x - apartmentScale/8, y - apartmentScale/8);
    for (int i = 0; i < riders.size (); i++) {
      riders.get(i).drawPerson(x - apartmentScale/3 + (apartmentScale * i/8), (int) (y + apartmentScale/3));
    }
    //    text(10 * (50 - floorNumber), x - apartmentScale/8, y + apartmentScale * 3/8);
  }

  public void setFloor () {
    int prevNum = floorNumber;
    if (key == '0' || key == '1' || key == '2' || key == '3' || key == '4' || key == '5' || key == '6' || key == '7' || key == '8' || key == '9') {
      if (floorInputInProgress) {
        floorNumber = 10 * prevNum + ((int) key - 48);
        if (50 - floorNumber > 49 || 50 - floorNumber < 0) {
          floorNumber = prevNum;
          floorInputInProgress = false;
        }
      } else {
        floorNumber = (int) key - 48;
        floorInputInProgress = true;
      }
    }
    if (keyCode == SHIFT) {
      if (50 - floorNumber > 49 || 50 - floorNumber < 0) {
        floorNumber = prevNum;
      }
      floorInputInProgress = false;
    }
  }

  public void unload () {
    unloaded = true;
    lastUnloadFloor = currentApartment;
    for (int i = 0; i < riders.size (); i++) {
      riders.get(i).stop = false;
      riders.get(i).inElevator = false;
      riders.get(i).happiness += 50;
      currentApartment.tenants.add(riders.get(i));
      currentApartment.population++;
      riders.remove(i);
    }
  }
}

class Floor {
  int roomColor;
  int population, floorNumber, posY, refY;
  boolean lowestFloor, elevator, onScreen;
  ArrayList<Person> tenants = new ArrayList<Person>();
  Floor (int y) {
    posY = y*originalApartmentScale;
    refY = y;
    floorNumber = 50 - y;
    population = (int) random(1, maxFloorPopulation);
    roomColor = (0);
    lowestFloor = false;
    elevator = false;
    onScreen = false;
    for (int i = 0; i < population; i++) {
      tenants.add(new Person (screenWidth/2 + (i * apartmentScale/8) - apartmentScale/3, posY + (int) (apartmentScale/2.5f)));
    }
  }
  public void drawFloor() {
    //    (apartmentHeight + cameraY - screenHeight)
    posY = refY * apartmentScale;
    if (lowestFloor) {
      roomColor = 255;
    } else {
      roomColor = 0;
    }
    fill(roomColor, 0, 0);
    rect(screenWidth/2, posY, apartmentScale, apartmentScale);
    fill(255);
    textSize(apartmentScale/10);
    text("Floor# " + floorNumber, screenWidth/2 - apartmentScale/3, posY - apartmentScale/3);
    text("Population: " + population, screenWidth/2 - apartmentScale/3, posY);
    //    text("posY: " + (posY), screenWidth/2 - apartmentScale/3, posY + apartmentScale/3);
    fill(0);
    for (int i = 0; i < population; i++) {
      updateHappiness (tenants.get(i));
      for (int j = 0; j < population; j++) {
        if (tenants.get(j) != tenants.get(i) && inBounds(tenants.get(i), tenants.get(j)) && !tenants.get(j).inConversation && !tenants.get(i).inConversation) {
          tenants.get(i).stop = true;
          tenants.get(i).inConversation = true;
          tenants.get(j).inConversation = true;
          tenants.get(j).stop = true;
        }
      }
      tenants.get(i).drawPerson(screenWidth/2 + (i * apartmentScale/8) - apartmentScale/3, posY + (int) (apartmentScale/2.5f));
      if (tenants.get(i).happiness == 0 && elevator && elevator1.unloaded == false) {
        leaveApartment(tenants.get(i));
      }
    }
  }
  //    text("-- Height: " + (apartmentHeight - posY) * 100/apartmentScale, screenWidth/2 + apartmentScale/2, posY - apartmentScale/3);
  public void leaveApartment (Person person) {
    elevator1.riders.add(person);
    //    Person [] newTenants = new Person[population - 1];
    //    int j = 0;
    for (int i = 0; i < population; i++) {
      if (i < tenants.size() && tenants.get(i) == person) {
        tenants.get(i).inElevator = true;
        tenants.get(i).stop = true;
        tenants.remove(i);
      }
    }
    //    tenants = newTenants;
    population--;
  }
  public boolean inBounds(Person personA, Person personB) {
    if (apartmentScale/8 > abs(personA.x - personB.x) && apartmentScale/32 < abs(personA.x - personB.x)) {
      return true;
    } else {
      return false;
    }
  }

  public boolean leftOut (Person personA) {
    if (!personA.inConversation) {
      for (int j = 0; j < population; j++) {
        if (!tenants.get(j).inConversation && tenants.get(j) != personA) {
          return false;
        }
      }
      return true;
    } else {
      return false;
    }
  }

  public void updateHappiness (Person person) {
    if (((population == 1 || population > 5)&& person.happiness > 0 && !person.inConversation) || leftOut(person)) {
      person.happiness-= 0.25f;
    }
    if ((person.inConversation  || population%2 == 0)&& person.happiness < 100) {
      person.happiness+= 1.5f;
    }
  }
}

//draws GUI 
public void drawGUI () {
  processInput();
  //draws the GUI 
  fill(0);
  rect(0, 0, screenWidth/1.6f, screenHeight/5);
  fill(255);
  textSize(15);
  text("Camera Height: " + (apartmentHeight + cameraY - screenHeight) * 100/apartmentScale, screenWidth/32, screenHeight/32);
  //  text("Camera Y: " + cameraY, screenWidth/32, screenHeight/16);  
  text("Camera Zoom: " + apartmentScale + "%", screenWidth/32, screenHeight *2.5f/32);  
  //  text("Elevator Y: " + elevator1.y, screenWidth/32, screenHeight *4/32);
  fill(0);
  drawButtons();
  drawElevatorButtons();
}

//loads all the button images
public void loadImages() {
  upButton[0] = loadImage("up-button.png");
  upButton[1] = loadImage("up-button-pressed.png");
  downButton[0] = loadImage("down-button.png");
  downButton[1] = loadImage("down-button-pressed.png");
  leftButton[0] = loadImage("left-button.png");
  leftButton[1] = loadImage("left-button-pressed.png");
  rightButton[0] = loadImage("right-button.png");
  rightButton[1] = loadImage("right-button-pressed.png");
}

//resets the buttons to their unpressed state
public void resetButtons () {
  upNum = 0;
  downNum = 0;
  rightNum = 0;
  leftNum = 0;
}

//draws the buttons to the screen
public void drawButtons () {
  image(upButton[upNum], originalApartmentScale, screenHeight - originalApartmentScale *5/4, originalApartmentScale/2, originalApartmentScale/2);
  image(downButton[downNum], originalApartmentScale, screenHeight - originalApartmentScale/1.5f, originalApartmentScale/2, originalApartmentScale/2);
  image(leftButton[leftNum], originalApartmentScale/2 - originalApartmentScale/8, screenHeight - originalApartmentScale/1.5f, originalApartmentScale/2, originalApartmentScale/2);
  image(rightButton[rightNum], originalApartmentScale * 3/2 + originalApartmentScale/8, screenHeight - originalApartmentScale/1.5f, originalApartmentScale/2, originalApartmentScale/2);
}

public void drawElevatorButtons () {
  textAlign(CENTER);
  int color1 = 0;
  int color2 = 255;
  if (keyPressed && keyCode == SHIFT) {
    color1 = 255;
    color2 = 0;
  }
  textSize(12.5f);
  fill(color1);
  rect(originalApartmentScale * 1.15f, originalApartmentScale * 1.05f, originalApartmentScale, originalApartmentScale/2.5f);
  noFill();
  stroke(color2);
  rect(originalApartmentScale * 1.15f, originalApartmentScale * 1.05f, originalApartmentScale/1.1f, originalApartmentScale/3);
  fill(color2);
  text("SHIFT to Go", originalApartmentScale * 1.15f, originalApartmentScale * 1.1f);
  noStroke();
  int color3 = 0;
  int color4 = 255;
  if (keyPressed && keyCode == ALT) {
    color3 = 255;
    color4 = 0;
  }
  fill(color3);
  rect(originalApartmentScale * 1.15f, originalApartmentScale * 1.5f, originalApartmentScale, originalApartmentScale/2.5f);
  noFill();
  stroke(color4);
  rect(originalApartmentScale * 1.15f, originalApartmentScale * 1.5f, originalApartmentScale/1.1f, originalApartmentScale/3);
  fill(color4);
  text("ALT to Unload", originalApartmentScale * 1.15f, originalApartmentScale * 1.55f);
  noStroke();
  float y = screenHeight/2 - originalApartmentScale * 2.5f;
  for (int i = 0; i < numApartments; i++) {
    if (i%5==0) {
      y+= originalApartmentScale/2;
    }
    if (i+1 == elevator1.currentApartment.floorNumber) {
      fill(255, 0, 0);
    } else if (originalApartmentScale/7 > abs(mouseX - (i%5 * originalApartmentScale/3 + originalApartmentScale/2)) &&
      originalApartmentScale/7 > abs(mouseY - (y - originalApartmentScale * 1/16))) {
      fill(220);
    } else if (apartmentScale/2 > abs(elevator1.y - apartments[50 - i - 1].refY * apartmentScale)) {
      fill(100);
    } else {
      fill(0);
    }
    ellipse(i%5 * originalApartmentScale/3 + originalApartmentScale/2, y - originalApartmentScale * 1/16, originalApartmentScale/3.5f, originalApartmentScale/3.5f);
    if (originalApartmentScale/7 > abs(mouseX - (i%5 * originalApartmentScale/3 + originalApartmentScale * 9/16)) &&
      originalApartmentScale/7 > abs(mouseY - (y - originalApartmentScale * 1/16))) {
      fill(0);
      if (mousePressed) {
        elevator1.floorNumber = i + 1;
      }
    } else {
      fill(255);
    }
    text(i+1, i%5 * originalApartmentScale/3 + originalApartmentScale/2, y);
  } 
  textAlign(LEFT);
}


public boolean upButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 5/4 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale - mouseY)) {
    return true;
  } else {
    return false;
  }
}
//originalApartmentScale, screenHeight - originalApartmentScale/1.5
public boolean downButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 5/4 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

public boolean leftButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 3/4 - originalApartmentScale/8 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

public boolean rightButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 3/2 + originalApartmentScale * 3/8 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

class Person {
  //  color myColor;
  int x, y, happiness, hunger, health;
  float moveSpeed, originalMovementSpeed;
  boolean movingLeft, stop, inConversation, inElevator;
  Person (int xPos, int yPos) {
    //   myColor = bodyColor; 
    x = xPos;
    y = yPos;
    originalMovementSpeed = random(1, 3);
    moveSpeed = originalMovementSpeed;
    happiness = 100;
    if (0 > random(-1, 1)) {
      movingLeft = true;
    } 
    stop = false;
    inConversation = false;
    inElevator = false;
  }

  public void drawPerson (int xPos, int yPos) {
    //    x = xPos;
    if (stop) {
      x = xPos;
    }
    y = yPos;
    if (!inElevator) {
      movePerson();
    }
    int happyColor = (int) map (happiness, 0, 100, 0, 255);
    fill(255 - happyColor, happyColor, 0);
    stroke(255);
    rect(x, y, apartmentScale/16, apartmentScale/8);
    ellipse(x, y - apartmentScale/10, apartmentScale/16, apartmentScale/16);
    if (inConversation) {
      talk();
    }
  }

  public void movePerson () {
    moveSpeed = originalMovementSpeed * apartmentScale/100;
    if (moveSpeed < originalMovementSpeed) {
      moveSpeed = originalMovementSpeed;
    }
    if (happiness < 1) {
      moveSpeed = 1;
    }
    if (stop) {
      moveSpeed = 0;
    }
    if (movingLeft) {
      x-= moveSpeed;
    } else {
      x+= moveSpeed;
    }
    if (x < screenWidth/2 - apartmentScale/2) {
      movingLeft = false;
    } else if (x > screenWidth/2 + apartmentScale/2) {
      movingLeft = true;
    }
  }

  public void talk () {
    fill(255);
    noStroke();
    ellipse(x, y - apartmentScale/4, apartmentScale/8, apartmentScale/16);
    stroke(255);
    line(x, y - apartmentScale/4, x, y - apartmentScale/6);
  }
}

int screenWidth = 600;
int screenHeight = 800;
int originalApartmentScale = 100;
int minApartmentScale = 25;
int maxApartmentScale = 400;
int apartmentScale = originalApartmentScale;
int apartmentHeight = 5000;
int numApartments = apartmentHeight/originalApartmentScale;
int scrollSpeed = 20;
float zoomSpeed = 2;
int cameraY = screenHeight - apartmentHeight;
int cameraX = 100;
float cameraZoom = 0;
int keyPressDelay = 10;
int keyPressCounter;
int maxFloorPopulation = 5;
int maxCameraHeight = 0;
int minCameraHeight = 100000;
Floor [] apartments = new Floor[numApartments];
Elevator elevator1;
PImage[]upButton = new PImage[2];
int upNum = 0;
PImage[]downButton = new PImage[2];
int downNum = 0;
PImage[]leftButton = new PImage[2];
int leftNum = 0;
PImage[]rightButton = new PImage[2];
int rightNum = 0;
  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "--full-screen", "--bgcolor=#666666", "--hide-stop", "Main" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
