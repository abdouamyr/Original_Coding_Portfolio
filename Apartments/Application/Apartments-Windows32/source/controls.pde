void processInput() {
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

