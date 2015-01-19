//draws GUI 
void drawGUI () {
  processInput();
  //draws the GUI 
  fill(0);
  rect(0, 0, screenWidth/1.6, screenHeight/5);
  fill(255);
  textSize(15);
  text("Camera Height: " + (apartmentHeight + cameraY - screenHeight) * 100/apartmentScale, screenWidth/32, screenHeight/32);
  //  text("Camera Y: " + cameraY, screenWidth/32, screenHeight/16);  
  text("Camera Zoom: " + apartmentScale + "%", screenWidth/32, screenHeight *2.5/32);  
  //  text("Elevator Y: " + elevator1.y, screenWidth/32, screenHeight *4/32);
  fill(0);
  drawButtons();
  drawElevatorButtons();
}

//loads all the button images
void loadImages() {
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
void resetButtons () {
  upNum = 0;
  downNum = 0;
  rightNum = 0;
  leftNum = 0;
}

//draws the buttons to the screen
void drawButtons () {
  image(upButton[upNum], originalApartmentScale, screenHeight - originalApartmentScale *5/4, originalApartmentScale/2, originalApartmentScale/2);
  image(downButton[downNum], originalApartmentScale, screenHeight - originalApartmentScale/1.5, originalApartmentScale/2, originalApartmentScale/2);
  image(leftButton[leftNum], originalApartmentScale/2 - originalApartmentScale/8, screenHeight - originalApartmentScale/1.5, originalApartmentScale/2, originalApartmentScale/2);
  image(rightButton[rightNum], originalApartmentScale * 3/2 + originalApartmentScale/8, screenHeight - originalApartmentScale/1.5, originalApartmentScale/2, originalApartmentScale/2);
}

void drawElevatorButtons () {
  textAlign(CENTER);
  int color1 = 0;
  int color2 = 255;
  if (keyPressed && keyCode == SHIFT) {
    color1 = 255;
    color2 = 0;
  }
  textSize(12.5);
  fill(color1);
  rect(originalApartmentScale * 1.15, originalApartmentScale * 1.05, originalApartmentScale, originalApartmentScale/2.5);
  noFill();
  stroke(color2);
  rect(originalApartmentScale * 1.15, originalApartmentScale * 1.05, originalApartmentScale/1.1, originalApartmentScale/3);
  fill(color2);
  text("SHIFT to Go", originalApartmentScale * 1.15, originalApartmentScale * 1.1);
  noStroke();
  int color3 = 0;
  int color4 = 255;
  if (keyPressed && keyCode == ALT) {
    color3 = 255;
    color4 = 0;
  }
  fill(color3);
  rect(originalApartmentScale * 1.15, originalApartmentScale * 1.5, originalApartmentScale, originalApartmentScale/2.5);
  noFill();
  stroke(color4);
  rect(originalApartmentScale * 1.15, originalApartmentScale * 1.5, originalApartmentScale/1.1, originalApartmentScale/3);
  fill(color4);
  text("ALT to Unload", originalApartmentScale * 1.15, originalApartmentScale * 1.55);
  noStroke();
  float y = screenHeight/2 - originalApartmentScale * 2.5;
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
    ellipse(i%5 * originalApartmentScale/3 + originalApartmentScale/2, y - originalApartmentScale * 1/16, originalApartmentScale/3.5, originalApartmentScale/3.5);
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


boolean upButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 5/4 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale - mouseY)) {
    return true;
  } else {
    return false;
  }
}
//originalApartmentScale, screenHeight - originalApartmentScale/1.5
boolean downButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 5/4 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

boolean leftButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 3/4 - originalApartmentScale/8 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

boolean rightButtonClicked () {
  if (originalApartmentScale/4 > abs(originalApartmentScale * 3/2 + originalApartmentScale * 3/8 - mouseX) && 
    originalApartmentScale/4 > abs(screenHeight - originalApartmentScale * 2/4 - mouseY)) {
    return true;
  } else {
    return false;
  }
}

