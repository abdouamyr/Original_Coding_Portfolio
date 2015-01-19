//runs at function start
void setup () {
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
void draw () {
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

